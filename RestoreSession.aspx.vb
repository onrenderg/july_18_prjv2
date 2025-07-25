Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Security.Cryptography
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Web.Services
Imports Newtonsoft.Json
Public Class RestoreSession
    Inherits System.Web.UI.Page
    Dim MyInterface As ErmsInterface.hbInterface
    Dim Implementer As New ErmsBLL
    Dim validation As New validation
    Dim mycommand As New SqlCommand
    Dim mytable, mytable13 As New DataTable
    Dim StrSql As String = ""
    Dim Result As New SqlClient.SqlParameter("@Result", SqlDbType.VarChar, 4000)
    Dim SqlErrMsg As New SqlClient.SqlParameter("@SqlErrMsg", SqlDbType.VarChar, 3072)
    Dim objCommonFunctions As New commonFunctions

    Protected Overrides Sub InitializeCulture()
        If Request.Form("ddllang") IsNot Nothing Then
            Dim selectedLanguage As String = Request.Form("ddllang")
            UICulture = Request.Form("ddllang")
            Culture = Request.Form("ddllang")
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(selectedLanguage)
            Thread.CurrentThread.CurrentUICulture = New CultureInfo(selectedLanguage)
            Session("MyLanguage") = selectedLanguage
            Application("CultureInfo") = selectedLanguage
        ElseIf Application("CultureInfo") IsNot Nothing And Application("CultureInfo") <> "" Then
            UICulture = Application("CultureInfo")
            Culture = Application("CultureInfo")
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Application("CultureInfo"))
            Thread.CurrentThread.CurrentUICulture = New CultureInfo(Application("CultureInfo").ToString())
            Session("MyLanguage") = Application("CultureInfo")
        End If
        'End If
        MyBase.InitializeCulture()
    End Sub
    Private Function decryptQueryString(ByVal strQueryString As String) As String



        Dim barr As Byte()

        If strQueryString Is Nothing Then
            barr = Convert.FromBase64String("")
        Else
            barr = Convert.FromBase64String(strQueryString)
        End If
        Dim encoding As New System.Text.ASCIIEncoding()
        Dim strdec As String = encoding.GetString(barr)

        Return (strdec)

    End Function

    Public Function finduserbytoken(token As String)
        Dim userid As String = ""
        Try
            MyInterface = Implementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@token", token)
            mytable13 = MyInterface.SELECTData(mycommand, "UpdatetokenDetails")
            If mytable13.Rows.Count > 0 Then
                If mytable13.Rows(0)("Msg").ToString() = "S" Then
                    userid = mytable13.Rows(0)("userid").ToString()
                    ' deleteuserid(token)
                End If
            End If


        Catch ex As Exception

        End Try
        Return userid
    End Function


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitializeCulture()
            Dim DBAccess = New commonFunctions()
            'Dim encryptedUserID As String = Request.QueryString("userID")
            Dim encryptedUserPass As String = Request.QueryString("userpass")
            Dim token As String = Request.QueryString("token")
            Dim userID As String = ""
            Dim userpass As String = ""
            If Not String.IsNullOrEmpty(encryptedUserPass) Then
                userpass = decryptQueryString(encryptedUserPass)
            End If
            If Not String.IsNullOrEmpty(token) Then
                userID = finduserbytoken(token)
                If Not String.IsNullOrEmpty(userID) Then
                    Dim result = DBAccess.UserLogin(userID, userpass, Session.SessionID)
                    If result.status_code = 200 Then
                        Dim jsonString As String = JsonConvert.SerializeObject(result.data)
                        Session.Add("user", jsonString)
                        Dim user As UserData = commonFunctions.GetUserDataFromSession()

                        'This Code is to be removed from this solution. use UserData Instead of too much session values. I have done in addBallotBox

                        Session("UserID") = user.Code
                        ' Session("UserDesg") = mytable.Rows(0)("DESG").ToString()
                        Session("UserDesg") = user.Description
                        Session("UserDesgLocal") = user.Description_Local
                        Session("UserLevel") = user.UserLevel
                        Session("gblStateCode") = user.StateCode
                        Session("gblDistrictCode") = user.DistrictCode
                        Session("gblDistrictName") = user.District_Name
                        Session("gblDistrictNameLocal") = user.District_Name_local
                        Session("gblBlockCode") = user.BlockCode
                        Session("gblBlockName") = user.Block_Name
                        Session("gblBlockNameLocal") = user.block_name_local
                        If Len(user.panchayat_name) > 0 Then
                            Session("gblPanchayatCode") = user.panchayat_code
                            Session("gblPanchayatName") = user.panchayat_name
                            Session("gblPanchayatNameLocal") = user.panchayat_name_local
                        End If
                        Session("gblWhAddress") = user.WareHouse_Address
                        Session("gblWhAddressLocal") = user.WareHouse_Address_Local
                        Session("EmailId") = user.EmailID
                        Session("MobileNo") = user.MobileNo
                        Session("officerName") = user.officerName
                        Session("landLine") = user.landLine
                        Session("NNN") = user.NNN_Type
                        Session("NNN_Code") = user.NNN_Code
                        Dim pass1 As String = ""
                        If Not IsDBNull(user.IsDefaultPassWord) And user.IsDefaultPassWord.ToUpper.Trim = "Y" Then
                            Session("DefultPass") = "yes"
                        Else
                            Session("DefultPass") = "N"
                        End If

                        CreateCookie()
                        Context.ApplicationInstance.CompleteRequest()
                        updateLogDetailsGO("I")


                        Try
                            If Session("DefultPass") = "yes" Then
                                Response.Redirect("~/changePassword.aspx", True)
                                Context.ApplicationInstance.CompleteRequest() ' This is still important
                                Exit Sub
                                Return
                            Else

                                Response.Redirect("~/index.aspx", True)
                                Context.ApplicationInstance.CompleteRequest() ' This is still important
                                Exit Sub
                                Return
                            End If
                        Catch ex As Exception
                            'lblMsg.Text = ex.Message
                        End Try



                    End If
                Else
                    UserMsgBox("Invalid or expired token.")
                    Response.Redirect("~/index.aspx", True)
                End If
            Else
                UserMsgBox("Invalid or expired token.")
                Response.Redirect("~/index.aspx", True)
            End If
        End If


    End Sub
    Private Sub UserMsgBox(ByVal msg As String)
        Try

            msg = msg.Replace("'", "\'")
            msg = msg.Replace(Chr(34), "\" & Chr(34))
            msg = msg.Replace(vbCrLf, "\n")
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "ShowStatus", "javascript:alert('" + msg + "');", True)
        Catch ex As Exception
            Dim msg1 As String = ex.Message
        End Try
    End Sub
    Private Sub CreateCookie()
        Try
            Dim tkt As FormsAuthenticationTicket
            Dim cookiestr As String
            Dim ck As HttpCookie

            tkt = New FormsAuthenticationTicket(1, Session("UserID"), DateTime.Now, DateTime.Now.AddMinutes(480), False, Session("UserID")) ', lstDistrict.SelectedValue)

            cookiestr = FormsAuthentication.Encrypt(tkt)
            ck = New HttpCookie(FormsAuthentication.FormsCookieName, cookiestr)
            ck.Path = FormsAuthentication.FormsCookiePath
            ck.HttpOnly = True
            ck.Secure = Request.IsSecureConnection
            'ck.Path = FormsAuthentication.FormsCookiePath
            Response.Cookies.Add(ck)
            'Create session variables
            Session.Timeout = 5

            Dim cookie As HttpCookie = Request.Cookies(".ASPXFORMDATA")
            Session("TicketID") = cookie.Value
            'Session hijecking Prevention
            Session("RndVar") = Guid.NewGuid().ToString()
            Dim st As String = (Request.Browser.Browser & Request.Browser.MajorVersion & Request.Browser.Platform & Request.Browser.MinorVersion & Session("RndVar"))
            Dim CNewRnd As HttpCookie = New HttpCookie("CRnd", st)

            CNewRnd.Path = FormsAuthentication.FormsCookiePath '"/temp"
            CNewRnd.Domain = "sechimachal"
            CNewRnd.HttpOnly = True
            CNewRnd.Secure = Request.IsSecureConnection
            Response.Cookies.Add(CNewRnd)
            Session("SRnd") = st

            Dim _browserInfo As String = Request.Browser.Browser + Request.Browser.Version + Request.UserAgent + "~" + Request.ServerVariables("REMOTE_ADDR")
            Dim _sessionValue As String = Session("UserID").ToString & "^" & System.DateTime.Now.Ticks.ToString & "^" & _browserInfo & "^" & Guid.NewGuid().ToString
            'Dim _sessionValue As String = (Convert.ToString(Convert.ToString(Session("UserId")) & "^" & DateTime.Now.Ticks & "^") & _browserInfo) + "^" & Guid.NewGuid()
            Dim _encodeAsBytes As Byte() = System.Text.ASCIIEncoding.ASCII.GetBytes(_sessionValue)
            Dim _encryptedString As String = System.Convert.ToBase64String(_encodeAsBytes)
            Session("encryptedSession") = _encryptedString

        Catch ex As Exception
            ' objActivityLog.insertErrorLog(txtUserID.Value, Request.ServerVariables("REMOTE_ADDR").ToString(), "Login", "CreateCookie", ex.Message)
        End Try
    End Sub
    Private Sub updateLogDetailsGO(ByRef Flag As String)
        Dim userid As String = ""
        Try
            MyInterface = Implementer
            mycommand.Parameters.Clear()

            If Session.SessionID Is Nothing Or Session.SessionID = "" Then
                mycommand.Parameters.Add("@SessionID", SqlDbType.VarChar, 100).Value = "No Session ID"
            Else
                mycommand.Parameters.Add("@SessionID", SqlDbType.VarChar, 100).Value = Session.SessionID
            End If

            If Session("UserID") Is Nothing Then
                userid = "No User Id"
            Else
                userid = Session("UserID").ToString
            End If
            mycommand.Parameters.Add("@UserID", SqlDbType.NVarChar, 50).Value = userid
            mycommand.Parameters.Add("@macIP", SqlDbType.VarChar, 20).Value = Request.UserHostAddress
            mycommand.Parameters.Add("@ExternalIP", SqlDbType.VarChar, 50).Value = Request.ServerVariables("REMOTE_ADDR").ToString()
            mycommand.Parameters.Add("@IPDetails", SqlDbType.NVarChar, 200).Value = Request.UserHostAddress 'getgeoloction()
            mycommand.Parameters.Add("@Flag", SqlDbType.Char, 1).Value = Flag

            mytable = MyInterface.UPDATEData(mycommand, "UpdateLogDetails")
            If mytable.Rows.Count > 0 Then
                If mytable.Rows(0).Item(0) <> "Saved Successfully" Then
                    Return
                End If
            End If
        Catch ex As Exception
            ' objActivityLog.insertErrorLog(userid, Request.ServerVariables("REMOTE_ADDR").ToString(), "Login", "updateLogDetailsGO", ex.Message)
        End Try
    End Sub

End Class
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Security.Cryptography
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Web.Services
Imports Newtonsoft.Json

Public Class login
    Inherits System.Web.UI.Page
    Dim MyInterface As ErmsInterface.hbInterface
    Dim Implementer As New ErmsBLL
    Dim validation As New validation
    Dim mycommand As New SqlCommand
    Dim mytable, mytable12 As New DataTable
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

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.AddHeader("Cache-control", "no-store, must-revalidate,private,no-cache")
        Response.AddHeader("PRAGMA", "NO-Cache")
        Response.Expires = -1
        Response.Expires = 0
        Response.Cache.SetNoStore()
        Response.Cache.SetExpires(DateTime.Now.AddDays(-1))

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session("alert_login") IsNot Nothing Then
                ClientScript.RegisterStartupScript(Me.GetType(), "Alert", $"alert('{Session("alert_login")}');", True)
            End If
            If Session("MyLanguage") IsNot Nothing Or Session("MyLanguage") <> "" Then
                Thread.CurrentThread.CurrentCulture = New CultureInfo(Trim(Session("MyLanguage")))
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(Trim(Session("MyLanguage")))
                Session("MyLanguage") = Trim(Session("MyLanguage"))
                ddllang.SelectedIndex = ddllang.Items.IndexOf(ddllang.Items.FindByValue(Trim(Session("MyLanguage"))))
                Application("CultureInfo") = Trim(Session("MyLanguage"))
            ElseIf Application("CultureInfo") IsNot Nothing And Application("CultureInfo") <> "" Then
                Thread.CurrentThread.CurrentUICulture = New CultureInfo(Application("CultureInfo").ToString())
                Thread.CurrentThread.CurrentCulture = New CultureInfo(Application("CultureInfo").ToString())
                ddllang.SelectedIndex = ddllang.Items.IndexOf(ddllang.Items.FindByValue(Application("CultureInfo").ToString()))
                InitializeCulture()
            Else
                Thread.CurrentThread.CurrentCulture = New CultureInfo("hi-IN")
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("hi-IN")
                Session("MyLanguage") = "hi-IN"
                Application("CultureInfo") = "hi-IN"
            End If
            Try
                Parallel.Invoke(Sub()
                                    Try
                                        If Not String.IsNullOrEmpty(Session("UserID")) Then
                                            updateLogDetailsGO("O")
                                            Session.RemoveAll()
                                            Session.Abandon()
                                        End If
                                    Catch ex As Exception
                                    End Try
                                End Sub,
                            Sub()
                                Response.Cache.SetCacheability(HttpCacheability.NoCache)
                                Response.Cache.SetNoStore()
                                Response.Cache.SetExpires(DateTime.Now - New TimeSpan(1, 0, 0))
                                Response.Cache.SetLastModified(DateTime.Now)
                                Response.Cache.SetAllowResponseInBrowserHistory(False)
                                If Not Page.IsPostBack Then
                                    Dim randomclass As New Random
                                    Session("Sseed") = randomclass.Next
                                    btnLogin.Attributes.Add("onclick", "javascript:hashPassword(" & Session("Sseed") & ");")
                                    Response.Cookies("AuthCookieNew").Expires = DateTime.Now.AddDays(-30)
                                End If

                            End Sub)
            Catch ex As Exception
            End Try
        Catch ex As Exception
        End Try

        If loginform.Visible Then
            SetFocus(txtUserID)
        Else
            SetFocus(txtID)
        End If
    End Sub
    Public Shared Function CreateHash(ByRef saltAndPassword As String) As String
        'Dim Algorithm As MD5 = MD5.Create()
        Dim Algorithm As SHA256 = SHA256.Create()
        Dim Data As Byte() = Algorithm.ComputeHash(Encoding.UTF8.GetBytes(saltAndPassword))
        Dim Hashed As String = ""

        For i As Integer = 0 To Data.Length - 1
            Hashed &= Data(i).ToString("x2").ToUpperInvariant()
        Next
        Return Hashed
    End Function

    Private Function encryptQueryString(ByVal strQueryString As String) As String
        Dim encoding As New System.Text.ASCIIEncoding()

        Dim barr As Byte()

        If strQueryString Is Nothing Then
            barr = encoding.GetBytes("")
        Else
            barr = encoding.GetBytes(strQueryString)
        End If

        Dim str64 As String = Convert.ToBase64String(barr, 0, barr.Length, Base64FormattingOptions.None)
        Return str64

    End Function

    Public Function ComputeSHA512Hash(input As String) As String
        ' Convert the input string to a byte array
        Dim bytes As Byte() = Encoding.UTF8.GetBytes(input)

        ' Use SHA512Managed class (fully managed implementation)
        Using sha512 As New SHA512Managed()
            ' Compute the hash
            Dim hashBytes As Byte() = sha512.ComputeHash(bytes)

            ' Convert hash bytes to a lowercase hexadecimal string
            Dim builder As New StringBuilder()
            For Each b As Byte In hashBytes
                builder.Append(b.ToString("x2"))
            Next

            Return builder.ToString()
        End Using
    End Function

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


    Public Function InsertIntoLoginToken(userid As String, token As String)
        Try
            MyInterface = Implementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@UserID", userid)
            mycommand.Parameters.AddWithValue("@token", token)
            mytable = MyInterface.SELECTData(mycommand, "InsertLoginToken")

            If mytable12.Rows.Count > 0 Then

            End If
        Catch ex As Exception
            lblMsg.Text = GetLocalResourceObject("errMsg")

        End Try

    End Function
    Protected Sub btnLogIn_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim DBAccess = New commonFunctions()
        Try
            If SearchCaptcha IsNot Nothing Then
                SearchCaptcha.ValidateCaptcha(txtCaptcha.Text)
            Else
                lblMsg.Text = GetLocalResourceObject("errCaptcha")
                Exit Sub
                Return
            End If

            If SearchCaptcha.UserValidated Then
                Dim result = DBAccess.UserLogin(txtUserID.Value, txtUserPass.Value, Session.SessionID)
                lblMsg.Text = result.status_message
                If result.status_code = 200 Then

                    Session.Clear()
                    Session.Abandon()
                    Dim sessionCookie As New HttpCookie("ASP.NET_SessionId", "")
                    sessionCookie.Expires = DateTime.Now.AddYears(-1)
                    Response.Cookies.Add(sessionCookie)
                    'Response.Redirect("RestoreSession.aspx?userID=" & objCommonFunctions.EncryptAES(txtUserID.Value.Trim), True)
                    ' Response.Redirect("RestoreSession.aspx?userID=" & encryptQueryString(txtUserID.Value.Trim) & "&userpass=" & encryptQueryString(txtUserPass.Value.Trim), True)

                    Dim newGuid As Guid = Guid.NewGuid()
                    Dim userID As String = encryptQueryString(txtUserID.Value.Trim)
                    Dim token1 As String = encryptQueryString(newGuid.ToString())
                    Dim generationTime As DateTime = DateTime.Now
                    Dim combinedString As String = token1 & userID & generationTime
                    Dim token As String = ComputeSHA512Hash(combinedString)
                    Dim url As String = "RestoreSession.aspx?token=" & token & "&userpass=" & encryptQueryString(txtUserPass.Value.Trim)
                    '& "&token=" & token
                    InsertIntoLoginToken(decryptQueryString(userID), token)
                    Response.Redirect(url, True)
                Else
                    lblMsg.Text = result.status_message
                End If
            Else
                lblMsg.Text = GetLocalResourceObject("errCaptcha")
            End If
        Catch ex As Exception
            lblMsg.Text = ex.Message
            Exit Sub
            Return
        End Try

        Return 'Stop Old Code


        'lblMsg.Text = ""
        'Dim blLoginSucc As Boolean = False
        'Try
        '    If Len(txtUserID.Value.Trim) > 30 Then
        '        lblMsg.Text = GetLocalResourceObject("errUserId")
        '        Exit Sub
        '        Return

        '    End If
        '    Session("UserID") = txtUserID.Value.Trim
        '    Session("DefultPass") = Nothing
        '    Try

        '    Catch ex As Exception
        '        lblMsg.Text = GetLocalResourceObject("errCaptcha")
        '        Exit Sub
        '        Return
        '    End Try
        '    If SearchCaptcha.UserValidated Then
        '        Dim passwordDatabase As String = ""
        '        Dim passwordHash As String = ""
        '        Dim md5passFirstLogin As String = ""
        '        passwordHash = txtUserPass.Value ' hidHash.Value
        '        txtCaptcha.Text = ""
        '        txtUserPass.Value = ""
        '        Try
        '            MyInterface = Implementer
        '            mycommand.Parameters.Clear()
        '            mycommand.Parameters.AddWithValue("@UserID", txtUserID.Value.Trim)
        '            mycommand.Parameters.AddWithValue("@ip", Request.UserHostAddress)
        '            If Session.SessionID IsNot Nothing Or Session.SessionID <> "" Then
        '                mycommand.Parameters.Add("@SessionID", SqlDbType.VarChar, 100).Value = Session.SessionID
        '            End If
        '            mytable = MyInterface.SELECTData(mycommand, "checkUserLogin")
        '            If mytable.Rows.Count > 0 Then
        '                If mytable.Rows(0).Item(0).ToString.ToLower() = "offline" Then
        '                    lblMsg.Text = mytable.Rows(0).Item("message").ToString()
        '                    Exit Sub
        '                ElseIf mytable.Rows(0).Item(0).ToString.ToLower() = "locked" Then
        '                    lblMsg.Text = mytable.Rows(0).Item("message").ToString()
        '                    Exit Sub
        '                ElseIf mytable.Rows(0).Item(0).ToString.ToLower() = "loggedin" Then
        '                    lblMsg.Text = mytable.Rows(0).Item("message").ToString()
        '                    Exit Sub
        '                End If
        '                If Not mytable.Rows(0).Item(0).ToString.ToUpper() = "UNF" Then
        '                    passwordDatabase = System.Convert.ToString(mytable.Rows(0).Item("UserPassword"))
        '                    md5passFirstLogin = passwordDatabase
        '                    Try
        '                        If Session("Sseed") Is Nothing Then
        '                            Session("Sseed") = hidHash.Value
        '                        Else
        '                            Dim seed As String = Session("Sseed").ToString
        '                        End If
        '                    Catch ex As Exception
        '                        Session("Sseed") = hidHash.Value
        '                        'objActivityLog.insertErrorLog(txtUserID.Value, Request.ServerVariables("REMOTE_ADDR").ToString(), "Login", "btnLogin_Click", ex.Message)
        '                    End Try
        '                    passwordDatabase = passwordDatabase.ToUpper + Session("Sseed").ToString
        '                    ' to be commented on hosting

        '                    'If (0 = 0) Then ' --uncomment this line to allow login for all users without authentication
        '                    If (0 = String.Compare(passwordDatabase.ToUpper, passwordHash.ToUpper, False)) Then
        '                        ''''Check if user account status locked then update status
        '                        If mytable.Rows(0).Item("Locked").ToString().ToUpper() = "Y" Or Convert.ToInt32(mytable.Rows(0).Item("FailedAttempt")) > 0 Then
        '                            Try
        '                                Dim MyInterface As ErmsInterface.hbInterface
        '                                Dim MyImplementer As New ErmsBLL
        '                                Dim validation As New validation
        '                                Dim mycommand As New SqlCommand
        '                                Dim mytable As New DataTable

        '                                MyInterface = MyImplementer
        '                                mycommand.Parameters.Clear()
        '                                mycommand.Parameters.AddWithValue("@UserID", txtUserID.Value.Trim)
        '                                mycommand.Parameters.AddWithValue("@Func", "UnlockAccount")
        '                                Result.Direction = Data.ParameterDirection.Output
        '                                mycommand.Parameters.Add(Result)
        '                                SqlErrMsg.Direction = Data.ParameterDirection.Output
        '                                mycommand.Parameters.Add(SqlErrMsg)
        '                                MyInterface.UPDATEData(mycommand, "UpdateLoginLocked")
        '                            Catch ex As Exception
        '                                ' objActivityLog.insertErrorLog(Session("UserID"), Request.ServerVariables("REMOTE_ADDR").ToString(), "Login", "btnLogIn_Click_UpdateLoginLocked", ex.Message)
        '                            End Try
        '                        End If
        '                        Session("UserID") = mytable.Rows(0).Item("Code")
        '                        ' Session("UserDesg") = mytable.Rows(0)("DESG").ToString()
        '                        Session("UserDesg") = mytable.Rows(0)("Description").ToString()
        '                        Session("UserDesgLocal") = mytable.Rows(0)("Description_Local").ToString()
        '                        Session("UserLevel") = mytable.Rows(0)("UserLevel").ToString()
        '                        Session("gblStateCode") = mytable.Rows(0)("StateCode").ToString()
        '                        Session("gblDistrictCode") = mytable.Rows(0)("DistrictCode").ToString()
        '                        Session("gblDistrictName") = mytable.Rows(0)("District_Name").ToString()
        '                        Session("gblDistrictNameLocal") = mytable.Rows(0)("District_Name_Local").ToString()
        '                        Session("gblBlockCode") = mytable.Rows(0)("BlockCode").ToString()
        '                        Session("gblBlockName") = mytable.Rows(0)("Block_Name").ToString()
        '                        Session("gblBlockNameLocal") = mytable.Rows(0)("Block_Name_Local").ToString()
        '                        If Len(mytable.Rows(0)("Panchayat_Name").ToString()) > 0 Then
        '                            Session("gblPanchayatCode") = mytable.Rows(0)("Panchayat_Code").ToString()
        '                            Session("gblPanchayatName") = mytable.Rows(0)("Panchayat_Name").ToString()
        '                            Session("gblPanchayatNameLocal") = mytable.Rows(0)("Panchayat_Name_Local").ToString()
        '                        End If
        '                        Session("gblWhAddress") = mytable.Rows(0)("WareHouse_Address").ToString()
        '                        Session("gblWhAddressLocal") = mytable.Rows(0)("WareHouse_Address_Local").ToString()
        '                        Session("EmailId") = mytable.Rows(0)("EmailID").ToString()
        '                        Session("MobileNo") = mytable.Rows(0)("MobileNo").ToString()
        '                        Session("officerName") = mytable.Rows(0)("officerName").ToString()
        '                        Session("landLine") = mytable.Rows(0)("landLine").ToString()
        '                        Session("NNN") = mytable.Rows(0)("NNN_type").ToString
        '                        Dim pass1 As String = ""
        '                        If Not IsDBNull(mytable.Rows(0).Item("IsDefaultPassWord")) And mytable.Rows(0).Item("IsDefaultPassWord").ToString.ToUpper.Trim = "Y" Then
        '                            Session("DefultPass") = "yes"
        '                        Else
        '                            Session("DefultPass") = "N"
        '                        End If
        '                        'Session("DefultPass") = "N" ' to be commented on hosting

        '                        'If Session("UserLevel") = "001" Or Session("UserLevel") = "006" Then 'district user
        '                        '    Session("gblDistrictCode") = mytable.Rows(0)("DistrictCode").ToString().Trim()
        '                        '    Session("gblDistrictName") = mytable.Rows(0)("District_Name").ToString().Trim()
        '                        '    Session("gblDistrictNameLocal") = mytable.Rows(0)("District_Name_Local").ToString().Trim()
        '                        'ElseIf Session("UserLevel") = "002" Or Session("UserLevel") = "003" Or Session("UserLevel") = "007" Then 'block user
        '                        '    Session("gblDistrictCode") = mytable.Rows(0)("DistrictCode").ToString().Trim()
        '                        '    Session("gblDistrictName") = mytable.Rows(0)("District_Name").ToString().Trim()
        '                        '    Session("gblDistrictNameLocal") = mytable.Rows(0)("District_Name_Local").ToString().Trim()
        '                        '    Session("gblBlockCode") = mytable.Rows(0)("BlockCode").ToString().Trim()
        '                        '    Session("gblBlockName") = mytable.Rows(0)("Block_Name").ToString().Trim()
        '                        '    Session("gblBlockNameLocal") = mytable.Rows(0)("Block_Name_Local").ToString().Trim()
        '                        '    If Len(mytable.Rows(0)("Panchayat_Name").ToString().Trim()) > 0 Then
        '                        '        Session("gblPanchayatCode") = mytable.Rows(0)("Panchayat_Code").ToString().Trim()
        '                        '        Session("gblPanchayatName") = mytable.Rows(0)("Panchayat_Name").ToString().Trim()
        '                        '        Session("gblPanchayatNameLocal") = mytable.Rows(0)("Panchayat_Name_Local").ToString().Trim()
        '                        '    End If
        '                        'ElseIf Session("UserLevel") = "004" Or Session("UserLevel") = "005" Then 'panchayat user
        '                        '    Session("gblDistrictCode") = mytable.Rows(0)("DistrictCode").ToString().Trim()
        '                        '    Session("gblDistrictName") = mytable.Rows(0)("District_Name").ToString().Trim()
        '                        '    Session("gblDistrictNameLocal") = mytable.Rows(0)("District_Name_Local").ToString().Trim()
        '                        '    Session("gblBlockCode") = mytable.Rows(0)("BlockCode").ToString().Trim()
        '                        '    Session("gblBlockName") = mytable.Rows(0)("Block_Name").ToString().Trim()
        '                        '    Session("gblBlockNameLocal") = mytable.Rows(0)("Block_Name_Local").ToString().Trim()
        '                        '    Session("gblPanchayatCode") = mytable.Rows(0)("Panchayat_Code").ToString().Trim()
        '                        '    Session("gblPanchayatName") = mytable.Rows(0)("Panchayat_Name").ToString().Trim()
        '                        '    Session("gblPanchayatNameLocal") = mytable.Rows(0)("Panchayat_Name_Local").ToString().Trim()
        '                        'ElseIf Session("UserLevel") = "012" Or Session("UserLevel") = "013" Then 'WareHouse user
        '                        '    Session("gblDistrictCode") = mytable.Rows(0)("DistrictCode").ToString().Trim()
        '                        '    Session("gblDistrictName") = mytable.Rows(0)("District_Name").ToString().Trim()
        '                        '    Session("gblDistrictNameLocal") = mytable.Rows(0)("District_Name_Local").ToString().Trim()
        '                        '    Session("gblBlockCode") = mytable.Rows(0)("BlockCode").ToString().Trim()
        '                        '    Session("gblBlockName") = mytable.Rows(0)("Block_Name").ToString().Trim()
        '                        '    Session("gblBlockNameLocal") = mytable.Rows(0)("Block_Name_Local").ToString().Trim()
        '                        '    If Len(mytable.Rows(0)("Panchayat_Name").ToString().Trim()) > 0 Then
        '                        '        Session("gblPanchayatCode") = mytable.Rows(0)("Panchayat_Code").ToString().Trim()
        '                        '        Session("gblPanchayatName") = mytable.Rows(0)("Panchayat_Name").ToString().Trim()
        '                        '        Session("gblPanchayatNameLocal") = mytable.Rows(0)("Panchayat_Name_Local").ToString().Trim()
        '                        '    End If
        '                        '    Session("gblWhAddress") = mytable.Rows(0)("WareHouse_Address").ToString().Trim()
        '                        '    Session("gblWhAddressLocal") = mytable.Rows(0)("WareHouse_Address_Local").ToString().Trim()
        '                        'End If
        '                        CreateCookie()
        '                        Context.ApplicationInstance.CompleteRequest()
        '                        updateLogDetailsGO("I")
        '                        blLoginSucc = True

        '                        Try
        '                            If Session("DefultPass") = "yes" Then
        '                                Response.Redirect("changePassword.aspx", True)

        '                            Else
        '                                Response.Redirect("index.aspx", True)
        '                            End If

        '                        Catch ex1 As ThreadAbortException
        '                        Catch ex As Exception
        '                            Throw New Exception(ex.Message)
        '                        End Try

        '                        Exit Sub
        '                    Else
        '                        lblMsg.Text = GetLocalResourceObject("InvalidUser")
        '                        Dim strReturn As String = UpdateFailedAttempts()
        '                        If strReturn = "AccountLocked" Then
        '                            lblMsg.Text = GetLocalResourceObject("AccountLocked")
        '                            Exit Sub
        '                        ElseIf strReturn = "AttemptUpdated" Then
        '                            lblMsg.Text = GetLocalResourceObject("InvalidUser")
        '                        End If
        '                        Dim randomclass As New Random
        '                        Session("Sseed") = randomclass.Next
        '                        btnLogin.Attributes.Add("onclick", "javascript:hashPassword(" & Session("Sseed") & ");")
        '                        ' objActivityLog.insertErrorLog(Session("UserID"), Request.ServerVariables("REMOTE_ADDR").ToString(), "Login", "btnLogIn_Click", "Login Failed Password not match")
        '                        Exit Sub
        '                    End If
        '                Else
        '                    Dim strReturn As String = UpdateFailedAttempts()
        '                    If strReturn = "AccountLocked" Then
        '                        lblMsg.Text = GetLocalResourceObject("AccountLocked")
        '                        Exit Sub
        '                    ElseIf strReturn = "AttemptUpdated" Then
        '                        lblMsg.Text = GetLocalResourceObject("InvalidUser")
        '                    Else
        '                        lblMsg.Text = GetLocalResourceObject("NoUser")
        '                    End If
        '                    lblMsg.Text = GetLocalResourceObject("InvalidUser")
        '                    lblMsg.Visible = True
        '                    Dim randomclass As New Random
        '                    Session("Sseed") = randomclass.Next
        '                    btnLogin.Attributes.Add("onclick", "javascript:hashPassword(" & Session("Sseed") & ");")
        '                    'objActivityLog.insertErrorLog(Session("UserID"), Request.ServerVariables("REMOTE_ADDR").ToString(), "Login", "btnLogIn_Click", "Invalid User User Not Found")
        '                    txtCaptcha.Text = ""
        '                    txtUserPass.Value = ""
        '                    txtUserID.Value = ""
        '                    SetFocus(txtUserID)
        '                    Exit Sub
        '                End If
        '            Else
        '                txtCaptcha.Text = ""
        '                txtUserPass.Value = ""
        '                txtUserID.Value = ""
        '                SetFocus(txtUserID)
        '                lblMsg.Text = GetLocalResourceObject("InvalidUser")
        '                Try
        '                    If mytable.ToString.Contains("Could not open a connection") Then
        '                        lblMsg.Text = GetLocalResourceObject("errDbConn") '"Could not open a connection to database server"
        '                        Exit Sub
        '                    End If
        '                Catch ex As Exception
        '                    Throw New Exception(ex.Message)
        '                End Try

        '                Dim strReturn As String = UpdateFailedAttempts()
        '                If strReturn = "AccountLocked" Then
        '                    lblMsg.Text = GetLocalResourceObject("AccountLocked")
        '                    Exit Sub
        '                ElseIf strReturn = "AttemptUpdated" Then
        '                    lblMsg.Text = GetLocalResourceObject("InvalidUser")
        '                End If
        '                'objActivityLog.insertErrorLog(Session("UserID"), Request.ServerVariables("REMOTE_ADDR").ToString(), "Login", "btnLogIn_Click", "Invalid Login No row return from mytable ")
        '                Exit Sub
        '            End If
        '        Catch ex1 As ThreadAbortException
        '        Catch ex As Exception
        '            If ex.Message.Contains("A network-related or instance-specific error occurred while ") Then
        '                lblMsg.Text = GetLocalResourceObject("errDbConn")
        '            Else
        '                lblMsg.Text = GetLocalResourceObject("errMsg") 'ex.Message.ToString
        '            End If
        '            '  objActivityLog.insertErrorLog(Session("UserID"), Request.ServerVariables("REMOTE_ADDR").ToString(), "Login", "btnLogIn_Click", ex.Message)
        '            Throw New Exception(lblMsg.Text)
        '        End Try
        '    Else
        '        Dim randomclass1 As New Random
        '        Session("Sseed") = randomclass1.Next
        '        btnLogin.Attributes.Add("onclick", "javascript:hashPassword(" & Session("Sseed") & ");")
        '        lblMsg.Text = GetLocalResourceObject("errCaptcha") '"You have entered an invalid value for the captcha"
        '        Session("UserID") = "0"
        '        ' objActivityLog.insertErrorLog(Session("UserID"), Request.ServerVariables("REMOTE_ADDR").ToString(), "Login", "btnLogIn_Click", "Login Failed")
        '        Exit Sub
        '    End If
        'Catch ex1 As ThreadAbortException
        'Catch ex As Exception
        '    lblMsg.Text = GetLocalResourceObject("errMsg")
        '    'objActivityLog.insertErrorLog(txtUserID.Value, Request.ServerVariables("REMOTE_ADDR").ToString(), "Login", "btnLogIn_Click", ex.Message)
        '    'Finally
        '    'If blLoginSucc Then
        '    '    objActivityLog.InsertUserActivityLog(txtUserID.Value, Request.ServerVariables("REMOTE_ADDR").ToString(), "Login", "Login" & "-SL", "User Login Successfully")
        '    'Else
        '    '    objActivityLog.InsertUserActivityLog(txtUserID.Value, Request.ServerVariables("REMOTE_ADDR").ToString(), "Login", "Login" & "-UL", "User Login Failed")
        '    'End If
        'End Try
    End Sub
    Private Sub CreateCookie()
        Try
            Dim tkt As FormsAuthenticationTicket
            Dim cookiestr As String
            Dim ck As HttpCookie

            tkt = New FormsAuthenticationTicket(1, txtUserID.Value, DateTime.Now, DateTime.Now.AddMinutes(480), False, txtUserID.Value) ', lstDistrict.SelectedValue)

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
    Private Function UpdateFailedAttempts() As String
        Dim strReturn As String = ""
        Try
            MyInterface = Implementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@UserID", txtUserID.Value.Trim)
            mycommand.Parameters.AddWithValue("@Func", "UpdateAttempt")
            Result.Direction = Data.ParameterDirection.Output
            mycommand.Parameters.Add(Result)
            SqlErrMsg.Direction = Data.ParameterDirection.Output
            mycommand.Parameters.Add(SqlErrMsg)
            mytable = MyInterface.UPDATEData(mycommand, "UpdateLoginLocked")
            If mytable.TableName = "tblUPDATEDataDefault" And Len(Trim(Result.Value)) > 0 Then
                strReturn = Trim(Result.Value)
            End If
        Catch ex As Exception
            '  objActivityLog.insertErrorLog(txtUserID.Value, Request.ServerVariables("REMOTE_ADDR").ToString(), "Login", "btnLogIn_Click", ex.Message)
        End Try
        Return strReturn
    End Function
    Protected Sub ImageButton1_Click(sender As Object, e As ImageClickEventArgs)
        upCaptcha.Update()
        txtCaptcha.Text = ""
        SetFocus(txtCaptcha)
    End Sub
    Private Sub btnSendOTP_Click(sender As Object, e As EventArgs) Handles btnSendOTP.Click, btnResendOTP.Click
        Dim strUserID As String = txtID.Value
        Dim Phone As String = ""
        Dim MailID As String = ""
        If Not String.IsNullOrEmpty(strUserID) Then
            lbl_msg.Visible = True
            getEmailMobile(strUserID, Phone, MailID)
        Else
            lbl_msg.Visible = False
        End If

        If Regex.IsMatch(Phone.Trim, "\d{10}") Or Not String.IsNullOrEmpty(MailID) Then
            'Send OTP to reset user password
            Dim randomclass As New Random
            ' HttpContext.Current.Session("otp" & Phone) = randomclass.Next(100000, 999999)
            Try
                Dim msg As String = objCommonFunctions.sendOTP(strUserID, Request.ServerVariables("REMOTE_ADDR").ToString(), Phone, MailID)
                lbl_msg.Text = IIf(ddllang.SelectedValue = "hi-IN", msg.Split("|")(1), msg.Split("|")(0))
                lbl_msg.Visible = True
                If msg.Contains("Could not sent OTP") Then
                    dvOTP.Visible = False
                    dvOtpBtns.Visible = True
                Else
                    dvOTP.Visible = True
                    dvOtpBtns.Visible = False
                End If
                txtUserID = txtID
            Catch ex As Exception
                dvOTP.Visible = False
                lbl_msg.Text = ex.Message.ToString
                Dim exMsg As String = ex.Message
                'Dim objActivityLog As New ActivityLog
                'objActivityLog.insertErrorLog(txtUserID.Value, Request.ServerVariables("REMOTE_ADDR").ToString(), "login", "btnSendOTP_Click", ex.Message)
            End Try
        Else
            lbl_msg.Visible = True
            lbl_msg.Text = IIf(ddllang.SelectedValue = "hi-IN", "प्रशासक की सहायता से अपना पासवर्ड रिसेट करवाएं", "You can reset your password with the help of your respective administrator")
        End If
        Dim Str As String = "$('#loginform').slideUp(); $('#recoverform').fadeIn();"
        ScriptManager.RegisterStartupScript(up1, up1.GetType, "google", Str, True)
        ScriptManager.RegisterStartupScript(upRecoverForm, upRecoverForm.GetType, "google", Str, True)
    End Sub
    Private Sub getEmailMobile(ByRef userId As String, ByRef Phone As String, ByRef emailID As String, Optional ByRef userlevel As String = "")
        Try
            MyInterface = Implementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@userId", userId)
            mytable = MyInterface.SELECTData(mycommand, "getEmailMobile")
            If mytable.Rows.Count > 0 Then
                emailID = mytable.Rows(0)("emailID").ToString
                Phone = mytable.Rows(0)("mobileNo").ToString
                userlevel = mytable.Rows(0)("userlevel").ToString
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub btnConfirmOTP_Click(sender As Object, e As EventArgs) Handles btnConfirmOTP.Click
        Try
            Dim strUserID As String = txtID.Value
            Dim Phone As String = ""
            Dim MailID As String = ""
            If txtOTP.Value.Trim = "" Then
                lbl_otp.Text = IIf(ddllang.SelectedValue = "hi-In", "अपने ईमेल और/या मोबाइल नंबर पर प्राप्त ओटीपी दर्ज करें", "Enter OTP Received On Your Email And/Or Mobile Number")
                lbl_otp.Visible = True
            ElseIf txtOTP.Value.Length = 6 Then
                Dim userlevel As String = ""
                getEmailMobile(strUserID, Phone, MailID, userlevel)
                If Session("otp" & Phone) = txtOTP.Value Then
                    Session.Remove("otp" & Phone)
                    Session("UserID") = strUserID
                    Session("EmailId") = MailID
                    Session("MobileNo") = Phone
                    Session("UserLevel") = userlevel
                    Session("DefultPass") = "yes"
                    CreateCookie()
                    Context.ApplicationInstance.CompleteRequest()
                    Response.Redirect("ChangePassword.aspx", True)
                Else
                    lbl_otp.Text = IIf(ddllang.SelectedValue = "hi-In", "ओ टी पी गलत है", "Wrong OTP")
                    lbl_otp.Visible = True
                End If
            Else
                lbl_otp.Text = IIf(ddllang.SelectedValue = "hi-In", "ओ टी पी गलत है", "Wrong OTP")
                lbl_otp.Visible = True
            End If
        Catch ex1 As ThreadAbortException
        Catch ex As Exception
            lbl_otp.Text = IIf(ddllang.SelectedValue = "hi-In", "ओ टी पी गलत है", "Wrong OTP")
            lbl_otp.Visible = True
            Dim exMsg As String = ex.Message
            'Dim objActivityLog As New ActivityLog
            'objActivityLog.insertErrorLog("", Request.ServerVariables("REMOTE_ADDR").ToString(), "login", "btnConfirmOTP_Click", ex.Message)
        End Try
        Dim Str As String = "$('#loginform').slideUp(); $('#recoverform').fadeIn();"
        ScriptManager.RegisterStartupScript(up1, up1.GetType, "google", Str, True)
        ScriptManager.RegisterStartupScript(upRecoverForm, upRecoverForm.GetType, "google", Str, True)
    End Sub

End Class
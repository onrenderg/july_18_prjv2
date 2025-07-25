Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.Helpers
Imports System.Web.Script.Serialization
Imports System.Web.Services

Public Class CommonHandler
    Implements System.Web.IHttpHandler, IRequiresSessionState 'System.Web.IHttpHandler
    Dim MyInterface As ErmsInterface.hbInterface
    Dim ErmsImplementer As New ErmsBLL

    Dim validation As New validation
    Dim mycommand As New SqlCommand
    Dim mytable As New DataTable
    ' Dim objActivityLog As New ActivityLog

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        'context.Response.ContentType = "text/plain"
        'context.Response.Write("Hello World!")
        Dim strResponse As String = String.Empty
        If Not String.IsNullOrEmpty(context.Request.Params("RequestType")) Then
            If Convert.ToString(context.Request.Params("RequestType")).ToLower() = "changepasswordprocess" Then
                Dim strValidateMsg As String = "" ' ValidateRequestHeader()
                If strValidateMsg = "" Then
                    strResponse = ChangePasswordProcess(context)
                Else
                    strResponse = "Invalid Request" ''"<Response><Status>" & "false" & "</Status><Message>Invalid Request.</Message></Response>"
                End If
                ''context.Response.ContentType = "text/xml"
                context.Response.ContentType = "text/plain"
                ' objActivityLog.InsertUserActivityLog(HttpContext.Current.Session("UserID"), HttpContext.Current.Request.ServerVariables("REMOTE_ADDR").ToString(), "CommonHandler", "Update", "Change Password|upd_Password")
            ElseIf Convert.ToString(context.Request.Params("RequestType")).ToLower() = "tokenvalue" Then
                Try
                    strResponse = GetToken()
                Catch ex As Exception
                    strResponse = GetToken()
                End Try
                context.Response.ContentType = "text/xml"
            ElseIf Convert.ToString(context.Request.Params("RequestType")).ToLower() = "updateprofileprocess" Then
                Dim strValidateMsg As String = ValidateRequestHeader()
                If strValidateMsg = "" Then
                    strResponse = UpdateProfileProcess(context)
                Else
                    strResponse = "Invalid Request" ''"<Response><Status>" & "false" & "</Status><Message>Invalid Request.</Message></Response>"
                End If
                context.Response.ContentType = "text/plain"
                '     objActivityLog.InsertUserActivityLog(HttpContext.Current.Session("UserID"), HttpContext.Current.Request.ServerVariables("REMOTE_ADDR").ToString(), "CommonHandler", "Update", "Update Profile|upd_Profile")
            End If

        Else
            strResponse = "Invalid Request"
        End If
        context.Response.Write(strResponse)
    End Sub
    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Public Function GetToken() As String
        Dim cookieToken As String, formToken As String
        Try

            AntiForgery.GetTokens(Nothing, cookieToken, formToken)
            Return "<Response><t>" & Convert.ToString(cookieToken & Convert.ToString(":")) & formToken & "</t></Response>"
        Catch ex As Exception
            'objActivityLog.insertErrorLog(HttpContext.Current.Session("UserID"), HttpContext.Current.Request.ServerVariables("REMOTE_ADDR").ToString(), "CommonHanlder", "GetToken", ex.Message)
            Return "<Response><t></t></Response>"
        End Try
    End Function

    Private Function ValidateRequestHeader() As String
        Dim strRes As String = ""
        Try
            Dim tokenHeaders As String
            Dim cookieToken As String = ""
            Dim formToken As String = ""
            tokenHeaders = HttpContext.Current.Request.Headers.[Get]("RequestVerificationToken")
            If tokenHeaders <> "" Then
                Dim tokens As String() = tokenHeaders.ToString.Split(":") 'tokenHeaders.First().Split(":"c)
                If tokens.Length = 2 Then
                    cookieToken = tokens(0).Trim()
                    formToken = tokens(1).Trim()
                End If
            End If
            AntiForgery.Validate(cookieToken, formToken)
        Catch ex As Exception
            strRes = ex.Message.ToString()
            ' objActivityLog.insertErrorLog(HttpContext.Current.Session("UserID"), HttpContext.Current.Request.ServerVariables("REMOTE_ADDR").ToString(), "CommonHanlder", "ValidateRequestHeader", ex.Message)
        End Try
        Return strRes
    End Function

    Private Function ChangePasswordProcess(context As HttpContext) As String
        Dim strUserCode As String = String.Empty, strOldPassword As String = String.Empty, strNewPassword As String = String.Empty, strFunc As String = String.Empty
        'Dim strtatus As String = "false"
        'Dim strMessage As String = String.Empty
        Dim strRet As String = "-99"
        'Dim strErrorXmlRes As String = "<Response><Status>false</Status><Message>Invalid Parameter</Message></Response>"
        Try
            If Not String.IsNullOrEmpty(context.Request.Params("CP_UserId")) Then
                strUserCode = Convert.ToString(context.Request.Params("CP_UserId"))
            Else
                Return strRet
                Exit Function
            End If
            If Not String.IsNullOrEmpty(context.Request.Params("CP_OldPass")) Then
                strOldPassword = Convert.ToString(context.Request.Params("CP_OldPass"))
            Else
                Return strRet
                Exit Function
            End If
            If Not String.IsNullOrEmpty(context.Request.Params("CP_NewPass")) Then
                strNewPassword = Convert.ToString(context.Request.Params("CP_NewPass"))
            Else
                Return strRet
                Exit Function
            End If
            If Not String.IsNullOrEmpty(context.Request.Params("CP_Func")) Then
                strFunc = Convert.ToString(context.Request.Params("CP_Func"))
            Else
                strFunc = "changepass"
            End If
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@UserName", strUserCode)
            mycommand.Parameters.AddWithValue("@NewPassword", strNewPassword)
            mycommand.Parameters.AddWithValue("@OldPassword", strOldPassword)
            mycommand.Parameters.AddWithValue("@MobileOTP", "")
            mycommand.Parameters.AddWithValue("@RefNo", "")
            mycommand.Parameters.AddWithValue("@fun", strFunc)
            mytable = MyInterface.UPDATEData(mycommand, "updateUserPassword")
            If mytable.Rows.Count > 0 Then
                strRet = mytable.Rows(0)("status")
            End If
            ''strMessage = Convert.ToString(mytable.Rows(0)("result"))
            ''Return (Convert.ToString((Convert.ToString("<Response><Status>") & strtatus) & "</Status><Message>") & strMessage) & "</Message></Response>"
        Catch ex As Exception
            'objActivityLog.insertErrorLog(HttpContext.Current.Session("UserID"), HttpContext.Current.Request.ServerVariables("REMOTE_ADDR").ToString(), "CommonHanlder", "ChangePasswordProcess", ex.Message)
            ''Return "<Response><Status>false</Status><Message>" & ex.Message.ToString & "</Message></Response>"
        End Try
        Return strRet
    End Function


    Private Function UpdateProfileProcess(context As HttpContext) As String
        Dim strUserCode As String = String.Empty, strMobile As String = String.Empty, strEmailID As String = String.Empty
        'Dim strtatus As String = "false"
        'Dim strMessage As String = String.Empty
        Dim strRet As String = "failed"
        'Dim strErrorXmlRes As String = "<Response><Status>false</Status><Message>Invalid Parameter</Message></Response>"
        Try
            If Not String.IsNullOrEmpty(context.Request.Params("UP_UserId")) Then
                strUserCode = Convert.ToString(context.Request.Params("UP_UserId"))
            Else
                strRet = "Invalid Request"
                Return strRet
                Exit Function
            End If
            If Not String.IsNullOrEmpty(context.Request.Params("UP_Mobile")) Then
                strMobile = Convert.ToString(context.Request.Params("UP_Mobile"))
            Else
                strRet = "Invalid Request"
                Return strRet
                Exit Function
            End If
            If Not String.IsNullOrEmpty(context.Request.Params("UP_Email")) Then
                strEmailID = Convert.ToString(context.Request.Params("UP_Email"))
            Else
                strRet = "Invalid Request"
                Return strRet
                Exit Function
            End If

            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@UserID", strUserCode)
            mycommand.Parameters.AddWithValue("@Mobile", strMobile)
            mycommand.Parameters.AddWithValue("@EmailId", strEmailID)
            If HttpContext.Current.Session("userlevelCode") = 9 Or HttpContext.Current.Session("userlevelCode") = 10 Then
                mycommand.Parameters.AddWithValue("@UserType", "bnk")
            Else
                mycommand.Parameters.AddWithValue("@UserType", "mut")
            End If

            mytable = MyInterface.UPDATEData(mycommand, "upd_Profile")
            If mytable.Rows.Count > 0 Then
                If Convert.ToString(mytable.Rows(0)("status")) = 1 Then
                    ''strtatus = "true"
                    strRet = "success"
                End If
            End If
            ''If strtatus = "true" Then
            ''    strMessage = Convert.ToString(mytable.Rows(0)("result"))
            ''    'Return (Convert.ToString((Convert.ToString("<Response><Status>") & strtatus) & "</Status><Message>") & strMessage) & "</Message></Response>"
            ''    Return "1"
            ''Else
            ''    strMessage = "Invalid "
            ''    Return (Convert.ToString((Convert.ToString("<Response><Status>") & strtatus) & "</Status><Message>") & strMessage) & "</Message></Response>"
            ''End If
        Catch ex As Exception
            ''"<Response><Status>false</Status><Message>" & ex.Message.ToString & "</Message></Response>"
            ' objActivityLog.insertErrorLog(HttpContext.Current.Session("UserID"), HttpContext.Current.Request.ServerVariables("REMOTE_ADDR").ToString(), "CommonHanlder", "UpdateProfileProcess", ex.Message)
        End Try
        Return strRet
    End Function
End Class
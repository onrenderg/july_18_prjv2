Imports System.Web.SessionState

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
        Session.Timeout = 15
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Capture the error details
        Dim ex As Exception = Server.GetLastError()
        Dim erroCode As String = "0"  ' Initialize with default value
        Dim errMsg As String = ""
        Dim errinnerMsg As String = ""
        Dim userCode As String = ""
        Dim erSource As String = ""

        If Not ex Is Nothing Then
            ' Extract error information
            erroCode = ex.HResult.ToString()  ' Get HRESULT code (if available)
#If DEBUG Then
            errMsg = Left(ex.Message.ToString, 200)  ' Truncate message for security

            errinnerMsg = GetExceptionStacktrace(ex) ' Include stack trace
            ' Extract user identity (if authenticated)
            If HttpContext.Current.User.Identity.IsAuthenticated Then
                userCode = HttpContext.Current.User.Identity.Name.ToString()
            End If
        Else
            errMsg = "Something went wrong. Please re-login"
            errinnerMsg = ""
            userCode = ""
#End If


            ' Log the error details (optional)
            ' You can implement error logging using a database, file system, or a third-party service
            Server.ClearError()

            ' Redirect to error page with comprehensive information
            Response.Redirect("~/frmError.aspx?errorCode=" & erroCode & "&errMsg=" & Server.UrlEncode(errMsg) & "&errinnerMsg=" & Server.UrlEncode(Left(errinnerMsg, 300)) & "&userCode=" & Server.UrlEncode(userCode), False)  ' Use False for POST request (optional)
            Response.Clear()
        End If
    End Sub

    Function GetExceptionStacktrace(ByVal ex As Exception) As String
        ' Recursively build the stack trace string
        Dim stackTrace As String = ex.StackTrace
        If Not ex.InnerException Is Nothing Then
            stackTrace = ex.InnerException.ToString
        End If
        Return stackTrace
    End Function

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
        Try
            Response.Redirect("logout.aspx", True)
            'HttpContext.Current.Server.Execute("logout.aspx", True)
        Catch ex1 As Threading.ThreadAbortException
        Catch ex As Exception
            'Response.Redirect("Login.aspx", True)
            'HttpContext.Current.Server.Execute("Login.aspx", True)
        End Try
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub
    Sub Application_PreSendRequestHeaders()
        If (Not (HttpContext.Current) Is Nothing) Then
            HttpContext.Current.Response.Headers.Remove("Server")
        End If
    End Sub
End Class
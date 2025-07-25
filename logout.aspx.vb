Imports System.Data
Imports System.Data.SqlClient
Public Class logout1
    Inherits System.Web.UI.Page
    Private Sub logout_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim userid As String = ""
        If Session("UserID") Is Nothing Then
            userid = "No User Id"
        Else
            userid = Session("UserID").ToString
            updateLogDetailsGO(userid)
            Response.Cache.SetLastModified(DateTime.Now)
            Response.Cache.SetAllowResponseInBrowserHistory(False)
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Response.Cache.SetNoStore()
            Session.Clear()
            Session.Abandon()
        End If

        Response.Cookies("ASP.NET_SessionId").Expires = DateTime.Now.AddDays(-30)
        Response.Cookies("AuthCookieNew").Expires = DateTime.Now.AddDays(-30)
        ''If Not Response.Cookies("AuthCookieGlb") Is Nothing Then
        ''    Response.Cookies("AuthCookieGlb").Expires = DateTime.Now.AddDays(-30)
        ''End If
        Session.RemoveAll()
        FormsAuthentication.SignOut()
        FormsAuthentication.RedirectToLoginPage()
        'Dim objActivityLog As New ActivityLog()
        'objActivityLog.InsertUserActivityLog(userid, Request.ServerVariables("REMOTE_ADDR").ToString(), "LogOut", "LogOut", "Logout|")
        Response.Redirect("Login.aspx")
    End Sub
    Private Sub updateLogDetailsGO(userid As String)
        Try
            Dim MyInterface As ErmsInterface.hbInterface
            Dim MyImplementer As New ErmsBLL
            Dim mycommand As New SqlCommand
            MyInterface = MyImplementer
            mycommand.Parameters.Clear()
            If Session.SessionID Is Nothing Or Session.SessionID = "" Then
                mycommand.Parameters.Add("@SessionID", SqlDbType.VarChar, 100).Value = "No Session ID"
            Else
                mycommand.Parameters.Add("@SessionID", SqlDbType.VarChar, 100).Value = Session.SessionID
            End If
            mycommand.Parameters.Add("@UserID", SqlDbType.NVarChar, 50).Value = userid
            mycommand.Parameters.Add("@macIP", SqlDbType.VarChar, 20).Value = Request.UserHostAddress
            mycommand.Parameters.Add("@ExternalIP", SqlDbType.VarChar, 50).Value = Request.ServerVariables("REMOTE_ADDR").ToString()
            mycommand.Parameters.Add("@IPDetails", SqlDbType.VarChar).Value = Request.UserHostAddress 'getgeoloction()
            mycommand.Parameters.Add("@Flag", SqlDbType.Char).Value = "O"
            MyInterface.UPDATEData(mycommand, "UpdateLogDetails")
        Catch ex As Exception
            '  Dim objActivityLog As New ActivityLog
            '  objActivityLog.insertErrorLog(userid, Request.ServerVariables("REMOTE_ADDR").ToString(), "Login", "updateLogDetailsGO", ex.Message)
        End Try
    End Sub

End Class
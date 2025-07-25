Public Class frmError
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Retrieve error information from query string
            Dim errorCode As String = Request.QueryString("errorCode")
            Dim errMsg As String = Server.UrlDecode(Request.QueryString("errMsg"))
            Dim errinnerMsg As String = Server.UrlDecode(Request.QueryString("errinnerMsg"))
            Dim userCode As String = Server.UrlDecode(Request.QueryString("userCode"))

            ' Display error information on the page
            lblErrorCode.Text = errorCode
            lblErrorMessage.Text = errMsg
            litStackTrace.Text = errinnerMsg
            lblUserCode.Text = userCode
        End If
    End Sub

End Class
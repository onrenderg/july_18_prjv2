Public Class viewOnMap
    Inherits System.Web.UI.Page

    Dim objCommonFunctions As New commonFunctions()
    Dim MyTable As New DataTable
    Dim userName, userLevel, dis_code, blk_code, myLang As String



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetLastModified(DateTime.Now)
        Response.Cache.SetAllowResponseInBrowserHistory(False)
        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        Response.Cache.SetNoStore()
        myLang = Session("MyLanguage").ToString

        fillMapData()
    End Sub

    Private Sub fillMapData()

        MyTable = Session("mapData")
        If MyTable.Rows.Count > 0 Then
            rptMarkers.DataSource = MyTable
            rptMarkers.DataBind()
        Else
            ScriptManager.RegisterStartupScript(Me, Page.GetType(), "alert", "alert('No Data Found !');", True)
        End If
    End Sub
    'Protected Sub btnGetData_Click(sender As Object, e As EventArgs)
    '    fillMapData()
    'End Sub
End Class
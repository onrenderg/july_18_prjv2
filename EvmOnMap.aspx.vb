Public Class EvmOnMap
    Inherits System.Web.UI.Page

    Dim objCommonFunctions As New commonFunctions()
    Dim MyTable As New DataTable
    Dim userName, userLevel, dis_code, blk_code, myLang As String



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        userName = Session("UserID")
        dis_code = Session("gblDistrictCode")
        userLevel = Session("UserLevel")

        Response.Cache.SetLastModified(DateTime.Now)
        Response.Cache.SetAllowResponseInBrowserHistory(False)
        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        Response.Cache.SetNoStore()
        myLang = Session("MyLanguage").ToString

        If Not IsPostBack Then
            objCommonFunctions.PopulateUnitTypes(rbUnitType)
        End If
    End Sub

    Private Sub fillMapData()

        MyTable = objCommonFunctions.fillMapData(dis_code, rbUnitType.SelectedValue)
        If MyTable.Rows.Count > 0 Then
            rptMarkers.DataSource = MyTable
            rptMarkers.DataBind()
        Else
            ScriptManager.RegisterStartupScript(Me, Page.GetType(), "alert", "alert('No EVM Data Found !');", True)
        End If
    End Sub

    Protected Sub rbUnitType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbUnitType.SelectedIndexChanged
        rptMarkers.DataSource = New DataTable
        rptMarkers.DataBind()

    End Sub
    Protected Sub btnGetData_Click(sender As Object, e As EventArgs)
        fillMapData()
    End Sub
End Class
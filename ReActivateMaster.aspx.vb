Public Class ReActivateMaster
    Inherits SetCulture
    Dim objCommonFunctions As New commonFunctions()
    Dim MyTable As New DataTable
    Dim userName, userLevel, dis_code, myLang As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetLastModified(DateTime.Now)
        Response.Cache.SetAllowResponseInBrowserHistory(False)
        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetNoStore()

        userName = Session("UserID")
        dis_code = Session("gblDistrictCode")
        userLevel = Session("UserLevel")
        myLang = Session("MyLanguage").ToString

        If Not IsPostBack Then
            alert_div.Visible = False
            Dim db_result = objCommonFunctions.ShowAvailableStockById(Request.QueryString("id"))
            If IsNothing(db_result) Then
                ' Show alert using ClientScript 
                ClientScript.RegisterStartupScript(Me.GetType(), "alert", "<script>alert('Stock Item not found.');</script>", True)
                Response.Redirect($"updateStationeryMasters.aspx")
            Else
                lblElectionFor.Text = IIf(myLang = "hi-IN", db_result.ItemFor_Desc_Loc, db_result.ItemFor_Desc)
                lblrbUnitType.Text = IIf(myLang = "hi-IN", db_result.ItemType_Desc_Loc, db_result.ItemType_Desc)

                hidden_id.Value = Request.QueryString("id")
                hidden_itemType.Value = db_result.ItemType
                hidden_itemFor.Value = db_result.ItemFor
                hidden_itemSpecification.Value = db_result.ItemSpecification
                ItemName.Text = db_result.ItemName
                ItemNameLocal.Text = db_result.ItemNameLocal

                ItemDescription.Text = db_result.ItemDescription
                ItemDescriptionLocal.Text = db_result.ItemDescriptionLocal

                de_activation_remakrs.Text = db_result.de_activation_remakrs
            End If
        Else
            Dim db_result = objCommonFunctions.stationeryMaster_DeActivate(hidden_id.Value,
                                                                     de_activation_remakrs.Text,
                                                                     userName)
            If db_result.status_code = 200 Or db_result.status_code = 201 Then
                Response.Redirect($"updateStationeryMasters.aspx")
            Else
                alert_div.Visible = True
                lbl_alert.Text = IIf(myLang = "hi-IN", db_result.status_message_local, db_result.status_message)
            End If
        End If
    End Sub

End Class
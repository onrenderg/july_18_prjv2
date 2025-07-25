Public Class AddNewMaster
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
            'rbPriPost,rbUnitType
            Dim rbPriPost = Request.QueryString("rbPriPost")
            Dim rbUnitType = Request.QueryString("rbUnitType")
            Dim list_electionfor As New DropDownList
            Dim list_UnitType As New DropDownList
            objCommonFunctions.PopulateSECCommonMasterValues(list_electionfor, "ELECTIONFOR", "000")
            objCommonFunctions.PopulateCommonMasterDropdown(list_UnitType, "STATIONERY", rbPriPost)

            lblElectionFor.Text = list_electionfor.Items.FindByValue(rbPriPost).Text
            lblrbUnitType.Text = list_UnitType.Items.FindByValue(rbUnitType).Text
            hidden_itemSpecification.Value = Request.QueryString("rbBallotBoxStationery")
            hidden_id.Value = 0
            hidden_itemType.Value = list_UnitType.Items.FindByValue(rbUnitType).Value
            Dim itemFor_ = IIf(list_electionfor.Items.FindByValue(rbPriPost).Value = "001", "GPYT", IIf(list_electionfor.Items.FindByValue(rbPriPost).Value = "003", "MC", "NPLK"))
            hidden_itemFor.Value = itemFor_

        Else
            Dim db_result = objCommonFunctions.stationeryMaster_Post(hidden_id.Value,
                                                                     hidden_itemType.Value,
                                                                     hidden_itemFor.Value,
                                                                     ItemName.Text,
                                                                     ItemNameLocal.Text,
                                                                     ItemDescription.Text,
                                                                     ItemDescriptionLocal.Text,
                                                                     hidden_itemSpecification.Value,
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
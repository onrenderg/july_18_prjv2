Imports System.Data.SqlClient
Imports System.Drawing

Public Class updateStationeryMasters
    Inherits SetCulture
    Dim objCommonFunctions As New commonFunctions()
    Dim MyTable As New DataTable
    Dim myLang As String
    Dim LoginData As UserData
    Dim unitCounts As Int64
    Dim itemid As Label
    Dim count As TextBox
    Dim fromSrNo As TextBox
    Dim toSrNo As TextBox
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetLastModified(DateTime.Now)
        Response.Cache.SetAllowResponseInBrowserHistory(False)
        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetNoStore()

        'userName = Session("UserID")
        'dis_code = Session("gblDistrictCode")
        'userLevel = Session("UserLevel")
        LoginData = commonFunctions.GetUserDataFromSession()

        myLang = Session("MyLanguage").ToString
        lblTitle.Text = Resources.Resource.menuUpdateStock
        If Not IsPostBack Then
            Dim ddl As New DropDownList
            objCommonFunctions.PopulateSECCommonMasterValues(rbPriPost, "ELECTIONFOR", "000")
            rbPriPost_SelectedIndexChanged(rbPriPost, EventArgs.Empty)
        End If
    End Sub
    Private Sub fillgrid()
        If LoginData.UserLevel = "000" Then
            DV.Visible = False
            MyTable = objCommonFunctions.PopulateStationeryAvailableDataTable(LoginData.Code, rbUnitType.SelectedValue, rbPriPost.SelectedValue, LoginData.UserLevel, True)
            If MyTable.Rows.Count > 0 Then
                If rbUnitType.SelectedValue = "BALLOT" Then
                    Dim m = MyTable.AsEnumerable() _
                .GroupBy(Function(row) row.Field(Of String)("ItemSpecification")) _
                .Select(Function(g) New With {
                    .ItemName = g.First().Field(Of String)("ItemName"),
                    .ID = g.First().Field(Of String)("ItemSpecification")
                }) _
                .ToList()

                    rbBallotBoxStationery.DataSource = m
                    rbBallotBoxStationery.DataTextField = "ItemName"
                    rbBallotBoxStationery.DataValueField = "ID"
                    rbBallotBoxStationery.DataBind()

                    rbBallotBoxStationery.SelectedIndex = 0
                    rbBallotBoxStationery_SelectedIndexChanged(rbBallotBoxStationery, EventArgs.Empty)
                Else
                    existing_grid.DataSource = MyTable
                    existing_grid.DataBind()
                End If
            Else
                existing_grid.DataSource = Nothing
                existing_grid.DataBind()
            End If
        Else
            existing_grid.DataSource = Nothing
            existing_grid.DataBind()
        End If
    End Sub

    Protected Sub rbPriPost_SelectedIndexChanged(sender As Object, e As EventArgs)
        If (rbPriPost.SelectedIndex <> -1) Then
            Dim ddl As New DropDownList
            objCommonFunctions.PopulateCommonMasterDropdown(ddl, "STATIONERY", rbPriPost.SelectedValue)
            rbUnitType.DataSource = ddl.DataSource
            rbUnitType.DataTextField = ddl.DataTextField
            rbUnitType.DataValueField = ddl.DataValueField
            rbUnitType.DataBind()
            rbUnitType.SelectedIndex = 0
            rbUnitType_SelectedIndexChanged(rbUnitType, EventArgs.Empty)
            'existing_grid.DataSource = New DataTable
        End If

    End Sub

    Protected Sub rbUnitType_SelectedIndexChanged(sender As Object, e As EventArgs)
        If rbUnitType.SelectedIndex <> -1 Then
            fillgrid()
            'MyTable = objCommonFunctions.PopulateStationeryAvailableDataTable(LoginData.Code, rbUnitType.SelectedValue, rbPriPost.SelectedValue, LoginData.DistrictCode, True)
            'If MyTable.Rows.Count > 0 Then
            '    existing_grid.DataSource = MyTable
            '    existing_grid.DataBind()
            'End If
        End If
        'existing_grid.DataSource = New DataTable
        'existing_grid.DataBind()
        'objCommonFunctions.PopulateStationeryAvailableDataTable(userName, rbUnitType.SelectedValue, rbPriPost.SelectedValue, dis_code, True)

        ''DV.Visible = False
        'If MyTable.Rows.Count > 0 Then
        '    existing_grid.DataSource = MyTable

        '    'existing_grid.DataBind()
        'Else

        'End If

    End Sub

    Private Sub MessageBox(sender As Object, ByVal sMsg As String)
        Dim sb As New StringBuilder()
        Dim oFormObject As System.Web.UI.Control

        sMsg = sMsg.Replace("'", "\'")
        sMsg = sMsg.Replace(Chr(34), "\" & Chr(34))
        sMsg = sMsg.Replace(vbCrLf, "\n")
        sMsg = "alert('" & sMsg & "');"

        sb = New StringBuilder()
        sb.Append(sMsg)

        For Each oFormObject In Me.Controls
            If TypeOf oFormObject Is HtmlForm Then
                Exit For
            End If
        Next
        ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.[GetType](), "", sMsg, True)
    End Sub

    Protected Sub rbBallotBoxStationery_SelectedIndexChanged(sender As Object, e As EventArgs)
        fillgrid_ballot()
    End Sub

    Protected Sub fillgrid_ballot()
        If rbBallotBoxStationery.SelectedIndex <> -1 Then
            DV.Visible = True
            Dim MyTable = objCommonFunctions.PopulateStationeryAvailableDataTable(LoginData.Code, rbUnitType.SelectedValue, rbPriPost.SelectedValue, LoginData.DistrictCode, True)
            Dim filteredTable = MyTable.AsEnumerable() _
                                        .Where(Function(row) row.Field(Of String)("ItemSpecification") = rbBallotBoxStationery.SelectedValue) _
                                        .CopyToDataTable()
            existing_grid.DataSource = filteredTable
            existing_grid.DataBind()
            'DV.Visible = True
        End If
    End Sub


    Protected Sub existing_grid_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)

    End Sub

    Private Sub existing_grid_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles existing_grid.RowDataBound

    End Sub
    Protected Sub btn_add_new_Click(sender As Object, e As EventArgs)
        Response.Redirect($"AddNewMaster.aspx?rbPriPost={rbPriPost.SelectedValue}&rbUnitType={rbUnitType.SelectedValue}&rbBallotBoxStationery={rbBallotBoxStationery.SelectedValue}")
    End Sub
    Protected Sub existing_grid_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName = "Edit" Then
            Response.Redirect($"EditMaster.aspx?id={e.CommandArgument}&rbBallotBoxStationery={rbBallotBoxStationery.SelectedValue}")
        ElseIf e.CommandName = "DeActivate" Then
            Response.Redirect($"DeactivateMaster.aspx?id={e.CommandArgument}")
        ElseIf e.CommandName = "ReActivate" Then
            Response.Redirect($"ReactivateMaster.aspx?id={e.CommandArgument}")
        End If
    End Sub
End Class
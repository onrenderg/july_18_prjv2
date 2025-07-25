Imports System.Data.SqlClient
Imports System.Drawing
Public Class StationeryStockConsume
    Inherits SetCulture
    Dim objCommonFunctions As New commonFunctions()
    Dim MyTable As New DataTable
    Dim myLang As String
    Dim unitCounts As Int64
    Dim LoginUser As UserData
    Dim total As Int64

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetLastModified(DateTime.Now)
        Response.Cache.SetAllowResponseInBrowserHistory(False)
        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetNoStore()

        LoginUser = commonFunctions.GetUserDataFromSession()
        myLang = Session("MyLanguage").ToString
        lblTitle.Text = Resources.Resource.menuConsumedStationery

        If Not IsPostBack Then
            Dim ddl As New DropDownList

            objCommonFunctions.PopulateSECCommonMasterValues(rbPriPost, "ELECTIONFOR", Session("NNN_Code"))
            rbPriPost.SelectedIndex = 0
            objCommonFunctions.PopulateCommonMasterDropdown(ddl, "STATIONERY", rbPriPost.SelectedValue)
            rbUnitType.DataSource = ddl.DataSource
            rbUnitType.DataTextField = ddl.DataTextField
            rbUnitType.DataValueField = ddl.DataValueField
            rbUnitType.DataBind()
            rbUnitType.SelectedIndex = 0

            lbl_error.Visible = False
            lblStatus.Visible = False
            MyTable = objCommonFunctions.PopulateStationeryAvailableDataTable(LoginUser.Code, rbUnitType.SelectedValue, rbPriPost.SelectedValue, LoginUser.Code, True)

            If LoginUser.UserLevel = "001" Or LoginUser.UserLevel = "006" Or LoginUser.UserLevel = "007" Then
                ddlDis.Items.Clear()
                ddlDis.Items.Insert(0, New ListItem(IIf(myLang = "hi-IN", Session("gblDistrictNameLocal"), Session("gblDistrictName")), LoginUser.DistrictCode))
                ddlDis.SelectedIndex = 0
            End If
            If MyTable.Rows.Count > 0 Then
                existing_grid.DataSource = MyTable
                existing_grid.DataBind()
                lbl_error.Visible = False
            Else
                lbl_error.Visible = True
                lbl_error.Text = GetLocalResourceObject("errNoRecord")
            End If
        End If
    End Sub

    Protected Sub save_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save_button.Click

        Dim tbl_StationeryMapping As New DataTable
        tbl_StationeryMapping.Columns.Add(New DataColumn("UnitID", GetType(Integer)))
        tbl_StationeryMapping.Columns.Add(New DataColumn("AllotedQuantity", GetType(Int64)))
        tbl_StationeryMapping.Columns.Add(New DataColumn("FromSrNo", GetType(Integer)))
        tbl_StationeryMapping.Columns.Add(New DataColumn("ToSrNo", GetType(Integer)))
        tbl_StationeryMapping.Columns.Add(New DataColumn("remarks", GetType(String)))
        tbl_StationeryMapping.Columns.Add(New DataColumn("Show_ToSrNo", GetType(Integer)))

        Dim drStationeryMapping As DataRow = Nothing
        For Each dr As GridViewRow In existing_grid.Rows
            Dim id As Label = DirectCast(dr.FindControl("id"), Label)
            Dim qty As TextBox = DirectCast(dr.FindControl("lblItemCounts"), TextBox)
            Dim lblFromSrNo As TextBox = DirectCast(dr.FindControl("lblFromSrNo"), TextBox)
            Dim lblToSrNo As TextBox = DirectCast(dr.FindControl("lblToSrNo"), TextBox)
            Dim ItemCounts As Label = DirectCast(dr.FindControl("ItemCounts"), Label)

            If Val(qty.Text.ToString) > 0 Then
                drStationeryMapping = tbl_StationeryMapping.NewRow()
                drStationeryMapping("UnitID") = id.Text
                'drStationeryMapping("AllotedQuantity") = Val(ItemCounts.Text) + Val(qty.Text)
                drStationeryMapping("AllotedQuantity") = Val(qty.Text)
                drStationeryMapping("FromSrNo") = Val(lblFromSrNo.Text)
                drStationeryMapping("ToSrNo") = Val(lblToSrNo.Text)
                'If Val(lblFromSrNo.Text) > 0 Then
                '    If Val(lblToSrNo.Text) - Val(lblFromSrNo.Text) + 1 <> Val(qty.Text) Then
                '        lbl_error.Visible = True
                '        lbl_error.Text = "Invalid Count of Items! Count of Items (From Sr.No. to To Sr.No.) must be equal to Total Available Stock. " 'GetLocalResourceObject("errInvalidCount")
                '        qty.BorderColor = Color.Red
                '        lblFromSrNo.BorderColor = Color.Red
                '        lblToSrNo.BorderColor = Color.Red
                '        qty.Focus()
                '        Exit Sub
                '    Else
                '        qty.BorderColor = Color.Empty
                '        lblFromSrNo.BorderColor = Color.Empty
                '        lblToSrNo.BorderColor = Color.Empty
                '    End If
                'End If
                tbl_StationeryMapping.Rows.Add(drStationeryMapping)
            End If
        Next
        Dim mytable As New DataTable
        Dim DisCode As String
        If LoginUser.UserLevel = "000" Then
            DisCode = "13"
        Else
            DisCode = LoginUser.DistrictCode
        End If
        '"BALLOT","001"'
        Try
            Dim dbconnect = New commonFunctions()

            mytable = dbconnect.UpdateStationeryStock(tbl_StationeryMapping, LoginUser.DistrictCode, rbUnitType.SelectedValue, rbPriPost.SelectedValue, LoginUser.UserLevel, LoginUser.Code, LoginUser.Code, False, True)
            If mytable.Rows.Count > 0 Then
                If mytable.Rows(0)(0).ToString = "updated" Then
                    lbl_error.Visible = False
                    lblStatus.Text = GetLocalResourceObject("msgSave").ToString
                    lblStatus.Visible = True

                    mytable = objCommonFunctions.PopulateStationeryAvailableDataTable(LoginUser.Code, rbUnitType.SelectedValue, rbPriPost.SelectedValue, LoginUser.DistrictCode, True)


                    If mytable.Rows.Count > 0 Then
                        If rbUnitType.SelectedValue = "BALLOT" Then
                            fillgrid_ballot()
                        Else
                            existing_grid.DataSource = mytable
                            existing_grid.DataBind()
                            lbl_error.Visible = False
                        End If
                    Else
                        lbl_error.Visible = True
                        lbl_error.Text = GetLocalResourceObject("errNoRecord")
                    End If
                Else
                    lbl_error.Text = mytable.Rows(0)(0).ToString
                    lbl_error.Visible = True
                    lblStatus.Visible = False
                End If
            End If
        Catch ex As Exception
            lbl_error.Text = ex.Message
            lbl_error.Visible = True
            lblStatus.Visible = False
        End Try
    End Sub
    Protected Sub cancel_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_button.Click
        Response.Redirect("StationeryStockConsume.aspx")
    End Sub

    Protected Sub rbPriPost_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As New DropDownList
        objCommonFunctions.PopulateCommonMasterDropdown(ddl, "STATIONERY", rbPriPost.SelectedValue)
        rbUnitType.DataSource = ddl.DataSource
        rbUnitType.DataTextField = ddl.DataTextField
        rbUnitType.DataValueField = ddl.DataValueField
        rbUnitType.DataBind()
        rbUnitType.SelectedIndex = 0

        existing_grid.DataSource = New DataTable
        existing_grid.DataBind()

        ' ddlBlk_SelectedIndexChanged(ddlBlk, Nothing)
        lbl_error.Visible = False
        lblStatus.Visible = False
        DV.Visible = False
        MyTable = objCommonFunctions.PopulateStationeryAvailableDataTable(LoginUser.Code, rbUnitType.SelectedValue, rbPriPost.SelectedValue, LoginUser.DistrictCode, True)

        If MyTable.Rows.Count > 0 Then
            existing_grid.DataSource = MyTable
            existing_grid.DataBind()
            lbl_error.Visible = False
        Else
            lbl_error.Visible = True
            lbl_error.Text = GetLocalResourceObject("errNoRecord")
        End If
    End Sub

    Protected Sub rbUnitType_SelectedIndexChanged(sender As Object, e As EventArgs)

        existing_grid.DataSource = New DataTable
        existing_grid.DataBind()
        MyTable = objCommonFunctions.PopulateStationeryAvailableDataTable(LoginUser.Code, rbUnitType.SelectedValue, rbPriPost.SelectedValue, LoginUser.DistrictCode, True)

        DV.Visible = False
        If MyTable.Rows.Count > 0 Then
            existing_grid.DataSource = MyTable
            If rbUnitType.SelectedValue = "BALLOT" Then
                Dim m = MyTable.AsEnumerable() _
                .GroupBy(Function(row) row.Field(Of String)("ItemName")) _
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

                'Filling selected index 0 values to grid
                fillgrid_ballot()

                DV.Visible = True
            Else
                DV.Visible = False
            End If
            existing_grid.DataBind()
            lbl_error.Visible = False
        Else
            lbl_error.Visible = True
            lbl_error.Text = GetLocalResourceObject("errNoRecord")
        End If

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
    Protected Sub existing_grid_RowCommand(sender As Object, e As GridViewCommandEventArgs)

    End Sub
    Protected Sub rbBallotBoxStationery_SelectedIndexChanged(sender As Object, e As EventArgs)
        fillgrid_ballot()
    End Sub

    Protected Sub fillgrid_ballot()
        If rbBallotBoxStationery.SelectedIndex <> -1 Then
            Dim MyTable = objCommonFunctions.PopulateStationeryAvailableDataTable(LoginUser.Code, rbUnitType.SelectedValue, rbPriPost.SelectedValue, LoginUser.DistrictCode, True)

            Dim filteredTable = MyTable.AsEnumerable() _
                                        .Where(Function(row) row.Field(Of String)("ItemSpecification") = rbBallotBoxStationery.SelectedValue) _
                                        .CopyToDataTable()
            existing_grid.DataSource = filteredTable
            existing_grid.DataBind()
            lbl_error.Visible = False
            lblStatus.Visible = False
        End If
    End Sub

    Protected Sub existing_grid_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)

    End Sub

    Private Sub existing_grid_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles existing_grid.RowDataBound
        If rbUnitType.SelectedValue = "PNKSEAL" Or rbUnitType.SelectedValue = "RCBOOK" Or rbUnitType.SelectedValue = "BALLOT" Then
            e.Row.Cells(0).Visible = False
            e.Row.Cells(4).Visible = True
            e.Row.Cells(5).Visible = True
            'e.Row.Cells(6).Visible = False
        Else
            e.Row.Cells(4).Visible = False
            e.Row.Cells(5).Visible = False
            'e.Row.Cells(6).Visible = False
        End If

        If e.Row.RowType = DataControlRowType.Header Then
            unitCounts = 0
        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblFromSrNo As TextBox = DirectCast(e.Row.FindControl("lblFromSrNo"), TextBox)
            Dim lblItemCounts As TextBox = DirectCast(e.Row.FindControl("lblItemCounts"), TextBox)
            Dim lblCnt As Label = DirectCast(e.Row.FindControl("lblCnt"), Label)
            Dim FromSrNo As Label = DirectCast(e.Row.FindControl("FromSrNo"), Label)
            Dim ToSrNo As Label = DirectCast(e.Row.FindControl("ToSrNo"), Label)

            Try

                If lblFromSrNo IsNot Nothing Then
                    Dim trigger1 As New AsyncPostBackTrigger()
                    trigger1.ControlID = lblFromSrNo.UniqueID ' Use UniqueID for GridView controls
                    trigger1.EventName = "TextChanged"
                    panel1.Triggers.Add(trigger1)
                End If

                If lblItemCounts IsNot Nothing Then
                    Dim trigger2 As New AsyncPostBackTrigger()
                    trigger2.ControlID = lblItemCounts.UniqueID ' Use UniqueID for GridView controls
                    trigger2.EventName = "TextChanged"
                    panel1.Triggers.Add(trigger2)
                End If

            Catch ex As Exception

            End Try
            unitCounts = unitCounts + IIf(lblItemCounts.Text = "" Or lblItemCounts.Text = "0", 0, lblItemCounts.Text)

            lblCnt.Text = ""
        End If
    End Sub

    Protected Sub lblFromSrNo_TextChanged(sender As Object, e As EventArgs)
        UpdateToSrNo(sender)
    End Sub

    Protected Sub lblItemCounts_TextChanged(sender As Object, e As EventArgs)
        ' Find the parent row of the TextBox that triggered the event
        Dim txtBox As TextBox = DirectCast(sender, TextBox)
        Dim row As GridViewRow = DirectCast(txtBox.NamingContainer, GridViewRow)

        ' Check if column 4 is visible
        If row.Cells(4).Visible Then
            UpdateToSrNo(row)
        End If
    End Sub

    Private Sub UpdateToSrNo(sender As Object)
        Try
            ' Find the parent row of the TextBox that triggered the event
            Dim txtBox As TextBox = DirectCast(sender, TextBox)
            Dim row As GridViewRow = DirectCast(txtBox.NamingContainer, GridViewRow)

            ' Get From Sr. No. and New Quantity textboxes
            Dim fromSrNoTextBox As TextBox = DirectCast(row.FindControl("lblFromSrNo"), TextBox)
            Dim itemCountTextBox As TextBox = DirectCast(row.FindControl("lblItemCounts"), TextBox)
            Dim toSrNoTextBox As TextBox = DirectCast(row.FindControl("lblToSrNo"), TextBox)

            ' Convert values to integers, handling empty inputs
            Dim fromSrNo As Integer = If(String.IsNullOrEmpty(fromSrNoTextBox.Text), 0, Convert.ToInt32(fromSrNoTextBox.Text))
            Dim newQuantity As Integer = If(String.IsNullOrEmpty(itemCountTextBox.Text), 0, Convert.ToInt32(itemCountTextBox.Text))

            ' Calculate To Sr. No.
            If fromSrNo > 0 AndAlso newQuantity > 0 Then
                toSrNoTextBox.Text = (fromSrNo + newQuantity - 1).ToString()
                'Else
                '    toSrNoTextBox.Text = ""
            End If

        Catch ex As Exception
            ' Handle any errors
            ClientScript.RegisterStartupScript(Me.GetType(), "error", $"alert('Error: {ex.Message}');", True)
        End Try
    End Sub

End Class
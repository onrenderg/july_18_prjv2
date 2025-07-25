Imports System.Data.SqlClient
Imports System.Drawing
Public Class StationeryStockAllocation
    Inherits SetCulture
    Dim objCommonFunctions As New commonFunctions()
    Dim MyTable As New DataTable
    Dim myLang As String
    Dim unitCounts As Int64
    Dim LoginUser As UserData
    Dim itemid As Label
    Dim count As TextBox
    Dim fromSrNo As TextBox
    Dim toSrNo As TextBox
    Dim itemFor As String = ""
    Dim total As Int64


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetLastModified(DateTime.Now)
        Response.Cache.SetAllowResponseInBrowserHistory(False)
        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetNoStore()
        LoginUser = commonFunctions.GetUserDataFromSession()
        'LoginUser.Code = Session("UserID")
        'dis_code = Session("gblDistrictCode")
        'LoginUser.userLevel = Session("LoginUser.userLevel")
        myLang = Session("MyLanguage").ToString

        If Not IsPostBack Then
            lblTitle.Text = Resources.Resource.menuAllocateStationery
            'objCommonFunctions.PopulateSECCommonMasterValues(rbPriPost, "ELECTIONFOR", "000")
            objCommonFunctions.PopulateSECCommonMasterValues(rbPriPost, "ELECTIONFOR", Session("NNN_Code"))

            If (LoginUser.UserLevel = "002" Or LoginUser.UserLevel = "003") Then
                Dim foundItem As ListItem = rbPriPost.Items.Cast(Of ListItem)().FirstOrDefault(Function(item) item.Value = LoginUser.NNN_Code)

                If foundItem IsNot Nothing Then
                    rbPriPost.SelectedIndex = rbPriPost.Items.IndexOf(foundItem)
                    rbPriPost.Enabled = False
                End If
            End If
            rbPriPost_SelectedIndexChanged(rbPriPost, EventArgs.Empty)
        End If
        'Why to use

    End Sub

    Protected Sub save_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save_button.Click

        Dim tbl_StationeryMapping As New DataTable
        tbl_StationeryMapping.Columns.Add(New DataColumn("UnitID", GetType(Integer)))
        tbl_StationeryMapping.Columns.Add(New DataColumn("AllotedQuantity", GetType(Int64)))
        tbl_StationeryMapping.Columns.Add(New DataColumn("FromSrNo", GetType(Integer)))
        tbl_StationeryMapping.Columns.Add(New DataColumn("ToSrNo", GetType(Integer)))
        tbl_StationeryMapping.Columns.Add(New DataColumn("Show_ToSrNo", GetType(Integer)))

        tbl_StationeryMapping.Columns.Add(New DataColumn("remarks", GetType(String)))

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
                If rbUnitType.SelectedValue = "PNKSEAL" Or rbUnitType.SelectedValue = "RCBOOK" Or rbUnitType.SelectedValue = "BALLOT" Then
                    If (Val(lblFromSrNo.Text) < 1 Or Val(lblToSrNo.Text) < 1) Then
                        lbl_error.Visible = True
                        lbl_error.Text = "Invalid Count of Items! " 'GetLocalResourceObject("errInvalidCount")
                        qty.BorderColor = Color.Red
                        lblFromSrNo.BorderColor = Color.Red
                        lblToSrNo.BorderColor = Color.Red
                        qty.Focus()
                        Exit Sub
                    Else
                        drStationeryMapping("FromSrNo") = Val(lblFromSrNo.Text)
                        drStationeryMapping("ToSrNo") = (Val(lblFromSrNo.Text) + Val(qty.Text)) - 1
                        drStationeryMapping("Show_ToSrNo") = Val(lblToSrNo.Text)
                        qty.BorderColor = Color.Empty
                        lblFromSrNo.BorderColor = Color.Empty
                        lblToSrNo.BorderColor = Color.Empty
                    End If
                Else
                    drStationeryMapping("FromSrNo") = 0
                    drStationeryMapping("ToSrNo") = 0
                    drStationeryMapping("Show_ToSrNo") = 0
                End If


                If (Int64.Parse(ItemCounts.Text) < Int64.Parse(qty.Text)) Then
                    lbl_error.Visible = True
                    lbl_error.Text = "Invalid Count of Items!" 'GetLocalResourceObject("errInvalidCount")
                    qty.BorderColor = Color.Red
                    lblFromSrNo.BorderColor = Color.Red
                    lblToSrNo.BorderColor = Color.Red
                    qty.Focus()
                    Exit Sub
                    Return
                End If

                tbl_StationeryMapping.Rows.Add(drStationeryMapping)
            End If
        Next
        'Dim mytable As New DataTable
        'Dim DisCode As String
        'If LoginUser.LoginUser.userLevel = "000" Then
        '    DisCode = "13"
        'Else
        '    DisCode = dis_code
        'End If
        '"BALLOT","001"'
        Dim to_user_code As String = ""
        If (ddlBlk.SelectedIndex <> -1) Then
            to_user_code = ddlBlk.SelectedValue
        ElseIf (ddlDis.SelectedIndex <> -1) Then
            to_user_code = ddlDis.SelectedValue
        Else
            to_user_code = LoginUser.StateCode
        End If

        Try
            Dim dbconnect = New commonFunctions()

            MyTable = dbconnect.UpdateStationeryStock(tbl_StationeryMapping,
                                                      LoginUser.DistrictCode,
                                                      rbUnitType.SelectedValue,
                                                      rbPriPost.SelectedValue,
                                                      LoginUser.UserLevel,
                                                      LoginUser.Code,
                                                      to_user_code,
                                                      False,
                                                    False)
            If MyTable.Rows.Count > 0 Then
                If MyTable.Rows(0)(0).ToString = "updated" Then
                    lbl_error.Visible = False
                    lblStatus.Text = GetLocalResourceObject("msgSave").ToString
                    lblStatus.Visible = True

                    fillgrid()
                Else
                    lbl_error.Text = MyTable.Rows(0)(0).ToString
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
        Response.Redirect("StationeryStockAllocation.aspx")
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
        End If
    End Sub

    Protected Sub rbUnitType_SelectedIndexChanged(sender As Object, e As EventArgs)

        If rbUnitType.SelectedIndex <> -1 Then
            objCommonFunctions.PopulatePriDistricts(ddlDis, rbPriPost.SelectedValue)
            Dim foundItem As ListItem = ddlDis.Items.Cast(Of ListItem)().FirstOrDefault(Function(item) item.Value = LoginUser.DistrictCode)

            If foundItem IsNot Nothing Then
                ddlDis.SelectedIndex = ddlDis.Items.IndexOf(foundItem)

            End If
            ddlDis_SelectedIndexChanged(ddlDis, EventArgs.Empty)
        End If

    End Sub

    Protected Sub ddlDis_SelectedIndexChanged(sender As Object, e As EventArgs)
        If ddlDis.SelectedIndex <> -1 Then
            If LoginUser.UserLevel = "000" Then 'Ask state user to select district , populate district for other users
                'ddlDis.Enabled = True
                ddlDis.AutoPostBack = True
                dvBlock.Visible = False
                lbl_error.Visible = False
                lblStatus.Visible = False
                If ddlDis.SelectedValue < 1 Then
                    dvList.Visible = False
                Else
                    dvList.Visible = True
                End If

                fillgrid()
            ElseIf (LoginUser.UserLevel = "001" Or LoginUser.UserLevel = "006") Then 'DISTRICT USER
                lbl_block_hint.Visible = False
                If (ddlDis.SelectedValue = LoginUser.DistrictCode) Then
                    If LoginUser.UserLevel = "001" Or LoginUser.UserLevel = "002" Or LoginUser.UserLevel = "003" Or LoginUser.UserLevel = "006" Or LoginUser.UserLevel = "007" Then
                        Select Case rbPriPost.SelectedValue
                            Case "001"
                                lblBlk.Text = Resources.Resource.Blk.ToString
                            Case "002"
                                lblBlk.Text = Resources.Resource.Ulb.ToString
                            Case "003"
                                lblBlk.Text = Resources.Resource.Mc.ToString
                        End Select
                        objCommonFunctions.PopulatePriBlocks(ddlBlk, LoginUser.DistrictCode, rbPriPost.SelectedValue)
                        dvBlock.Visible = True
                        If (LoginUser.UserLevel = "002" Or LoginUser.UserLevel = "003") Then
                            ddlDis.Enabled = False
                        End If
                        If ddlDis.SelectedValue < 1 Then
                            dvList.Visible = False
                        Else
                            dvList.Visible = True
                        End If
                        ddlBlk_SelectedIndexChanged(ddlBlk, EventArgs.Empty)
                    Else
                        lbl_error.Visible = True
                        lbl_error.Text = GetLocalResourceObject("errNotAllowed")
                    End If
                Else
                    If ddlDis.SelectedValue < 1 Then
                        dvList.Visible = False
                    Else
                        dvList.Visible = True
                    End If
                    ddlDis.AutoPostBack = True
                    dvBlock.Visible = False
                    lbl_error.Visible = False
                    lblStatus.Visible = False
                    fillgrid()
                End If
            ElseIf LoginUser.UserLevel = "001" Or LoginUser.UserLevel = "002" Or LoginUser.UserLevel = "003" Or LoginUser.UserLevel = "006" Or LoginUser.UserLevel = "007" Then
                lbl_block_hint.Visible = True
                Select Case rbPriPost.SelectedValue
                    Case "001"
                        lblBlk.Text = Resources.Resource.Blk.ToString
                    Case "002"
                        lblBlk.Text = Resources.Resource.Ulb.ToString
                    Case "003"
                        lblBlk.Text = Resources.Resource.Mc.ToString
                End Select
                objCommonFunctions.PopulatePriBlocks(ddlBlk, LoginUser.DistrictCode, rbPriPost.SelectedValue)
                dvBlock.Visible = True
                If (LoginUser.UserLevel = "002" Or LoginUser.UserLevel = "003") Then
                    ddlDis.Enabled = False
                End If
                ddlBlk_SelectedIndexChanged(ddlBlk, EventArgs.Empty)
            Else
                lbl_error.Visible = True
                lbl_error.Text = GetLocalResourceObject("errNotAllowed")
            End If
        Else
        End If
    End Sub
    Protected Sub ddlBlk_SelectedIndexChanged(sender As Object, e As EventArgs)
        If ddlBlk.SelectedIndex <> -1 Then
            If ddlBlk.SelectedValue < 1 Then

                dvList.Visible = False
            Else
                dvList.Visible = True
            End If
            fillgrid()
        End If
    End Sub
    Protected Sub existing_grid_RowCommand(sender As Object, e As GridViewCommandEventArgs)

    End Sub
    Protected Sub rbBallotBoxStationery_SelectedIndexChanged(sender As Object, e As EventArgs)
        If (rbBallotBoxStationery.SelectedIndex <> -1) Then
            fillgrid()
        End If

    End Sub
    Protected Sub fillgrid()
        Dim MyTable As DataTable = New DataTable()

        If (ddlBlk.SelectedIndex <> -1) Then
            MyTable = objCommonFunctions.PopulateStationeryAvailableDataTable(LoginUser.Code, rbUnitType.SelectedValue, rbPriPost.SelectedValue, ddlBlk.SelectedValue, True)
        ElseIf ddlDis.SelectedIndex <> -1 Then
            MyTable = objCommonFunctions.PopulateStationeryAvailableDataTable(LoginUser.Code, rbUnitType.SelectedValue, rbPriPost.SelectedValue, ddlDis.SelectedValue, True)
        Else
            MyTable = objCommonFunctions.PopulateStationeryAvailableDataTable(LoginUser.Code, rbUnitType.SelectedValue, rbPriPost.SelectedValue, LoginUser.DistrictCode, True)
        End If

        If MyTable.Rows.Count > 0 Then
            If rbUnitType.SelectedValue = "BALLOT" Then
                DV.Visible = True
                If rbBallotBoxStationery.SelectedIndex = -1 Then
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
                    rbBallotBoxStationery_SelectedIndexChanged(rbBallotBoxStationery, EventArgs.Empty)
                Else
                    Dim filteredTable = MyTable.AsEnumerable() _
                                            .Where(Function(row) row.Field(Of String)("ItemSpecification") = rbBallotBoxStationery.SelectedValue) _
                                            .CopyToDataTable()
                    existing_grid.DataSource = filteredTable
                    existing_grid.DataBind()
                End If
            Else
                existing_grid.DataSource = MyTable
                existing_grid.DataBind()
                DV.Visible = False
            End If
            Dim sum As Object = MyTable.Compute("SUM(items_allocated_to_selected_user)", "")

            'Handle potential nulls (DBNull)
            If sum Is DBNull.Value Then
                sum = 0 ' Or any other default value

            End If
            total = CDbl(sum) ' Convert to the appropriate data type (Double, Integer, Decimal, etc.)
            Try
                If ddlBlk.SelectedIndex = 0 Then
                    lblMappingCount.Text = ""
                ElseIf ddlDis.SelectedIndex = 0 Then
                    lblMappingCount.Text = ""
                Else
                    If LoginUser.UserLevel = "000" Then
                        lblMappingCount.Text = IIf(myLang = "hi-IN", ddlDis.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + total.ToString, rbUnitType.SelectedItem.Text + " Allocated to " + ddlDis.SelectedItem.Text + ": " + total.ToString)
                    ElseIf LoginUser.UserLevel = "001" Or LoginUser.UserLevel = "006" Then
                        If (ddlBlk.Visible = False) Then
                            lblMappingCount.Text = IIf(myLang = "hi-IN", ddlDis.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + total.ToString, rbUnitType.SelectedItem.Text + " Allocated to " + ddlDis.SelectedItem.Text + ": " + total.ToString)

                        Else
                            lblMappingCount.Text = IIf(myLang = "hi-IN", ddlBlk.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + total.ToString, rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + total.ToString)
                        End If
                    Else
                        If rbPriPost.SelectedValue = "001" Then
                            lblMappingCount.Text = IIf(myLang = "hi-IN", ddlBlk.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + total.ToString, rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + total.ToString)
                        End If
                    End If
                End If

            Catch ex As Exception

            End Try

            lbl_error.Visible = False
        Else
            lbl_error.Visible = True
            lbl_error.Text = GetLocalResourceObject("errNoRecord")
        End If
    End Sub
    Protected Sub existing_grid_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)

    End Sub

    Private Sub existing_grid_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles existing_grid.RowDataBound
        If rbUnitType.SelectedValue = "PNKSEAL" Or rbUnitType.SelectedValue = "RCBOOK" Or rbUnitType.SelectedValue = "BALLOT" Then
            e.Row.Cells(0).Visible = False
            e.Row.Cells(5).Visible = True
            e.Row.Cells(6).Visible = True
            'e.Row.Cells(6).Visible = False
        Else
            e.Row.Cells(5).Visible = False
            e.Row.Cells(6).Visible = False
            'e.Row.Cells(6).Visible = False
        End If
        Dim templateField As TemplateField = DirectCast(existing_grid.Columns(2), TemplateField)

        If e.Row.RowType = DataControlRowType.Header Then
            unitCounts = 0
            If ddlBlk.SelectedIndex <> -1 Then
                If ddlBlk.SelectedIndex = 0 Then
                    If (ddlBlk.Visible = False) Then
                        e.Row.Cells(2).Text = templateField.HeaderText + " (" + ddlDis.SelectedItem.Text + ")"
                    Else
                        e.Row.Cells(2).Text = templateField.HeaderText + " (" + ddlBlk.SelectedItem.Text + ")"
                    End If
                    'e.Row.Cells(2).Text = templateField.HeaderText
                Else
                    If (ddlBlk.Visible = False) Then
                        e.Row.Cells(2).Text = templateField.HeaderText + " (" + ddlDis.SelectedItem.Text + ")"
                    Else
                        e.Row.Cells(2).Text = templateField.HeaderText + " (" + ddlBlk.SelectedItem.Text + ")"

                    End If
                End If
            ElseIf ddlDis.SelectedIndex <> -1 Then
                If ddlDis.SelectedIndex = 0 Then
                    e.Row.Cells(2).Text = templateField.HeaderText
                Else
                    e.Row.Cells(2).Text = templateField.HeaderText + " (" + ddlDis.SelectedItem.Text + ")"
                End If
            Else
                e.Row.Cells(2).Text = templateField.HeaderText
            End If
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
            If rbUnitType.SelectedValue = "PNKSEAL" Or rbUnitType.SelectedValue = "RCBOOK" Or rbUnitType.SelectedValue = "BALLOT" Then
                'If Val(FromSrNo.Text) > 0 Then
                '    lblCnt.Text = "; क्र.सं.: " + FromSrNo.Text + " - " + ToSrNo.Text
                '    lblCnt.Visible = True
                'Else

                'End If
                lblCnt.Visible = False
            End If
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
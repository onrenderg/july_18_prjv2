Imports System.Data.SqlClient
Imports System.Drawing
Public Class BallotBoxAllocation
    Inherits SetCulture
    Dim objCommonFunctions As New commonFunctions()
    Dim MyTable As New DataTable
    Dim user As UserData
    Dim myLang As String


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetLastModified(DateTime.Now)
        Response.Cache.SetAllowResponseInBrowserHistory(False)
        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetNoStore()

        user = commonFunctions.GetUserDataFromSession()
        myLang = HttpContext.Current.Session("MyLanguage").ToString()
        If Not IsPostBack Then
            lblTitle.Text = Resources.Resource.menuBallotBoxAllocation
            'lblBlk.Text = Resources.Resource.Mc.ToString
            'lblDis.Text = Resources.Resource.Dis.ToString
            lbl_error.Visible = False
            lblStatus.Visible = False

            Dim state_ = objCommonFunctions.State_Master_Get()
            If state_.status_code = 200 Then
                Dim list_ As List(Of State_Master)
                ' Usign Direct Cast as I have received a list from Proc
                list_ = DirectCast(state_.data, List(Of State_Master))
                ddlState.DataSource = list_
                ddlState.DataTextField = "State_Name" ' The property to display
                ddlState.DataValueField = "State_Code"          ' The property to use as the value
                ddlState.DataBind()
                ddlState_SelectedIndexChanged(ddlState, EventArgs.Empty)
            End If
        End If
    End Sub

    Protected Sub save_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save_button.Click

        Dim tbl_StationeryMapping As New DataTable
        tbl_StationeryMapping.Columns.Add(New DataColumn("UnitID", GetType(Integer)))
        tbl_StationeryMapping.Columns.Add(New DataColumn("AllotedQuantity", GetType(Int64)))
        tbl_StationeryMapping.Columns.Add(New DataColumn("FromSrNo", GetType(Integer)))
        tbl_StationeryMapping.Columns.Add(New DataColumn("ToSrNo", GetType(Integer)))

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
                If Val(lblFromSrNo.Text) > 0 Then
                    If Val(lblToSrNo.Text) - Val(lblFromSrNo.Text) + 1 <> Val(qty.Text) Then
                        lbl_error.Visible = True
                        lbl_error.Text = "Invalid Count of Items! Count of Items (From Sr.No. to To Sr.No.) must be equal to Total Available Stock. " 'GetLocalResourceObject("errInvalidCount")
                        qty.BorderColor = Color.Red
                        lblFromSrNo.BorderColor = Color.Red
                        lblToSrNo.BorderColor = Color.Red
                        qty.Focus()
                        Exit Sub
                    Else
                        qty.BorderColor = Color.Empty
                        lblFromSrNo.BorderColor = Color.Empty
                        lblToSrNo.BorderColor = Color.Empty
                    End If
                End If
                tbl_StationeryMapping.Rows.Add(drStationeryMapping)
            End If
        Next

        Dim to_user_code As String = ""
        If (ddlBlk.SelectedIndex >= 0) Then
            to_user_code = ddlBlk.SelectedValue
        ElseIf (ddlDis.SelectedIndex >= 0) Then
            to_user_code = ddlDis.SelectedValue
        Else
            to_user_code = "13"
        End If

        Try
            Dim dbconnect = New commonFunctions()

            'MyTable = dbconnect.UpdateStationeryStock(tbl_StationeryMapping, dis_code, "", "", userLevel, userName, to_user_code)
            'If MyTable.Rows.Count > 0 Then
            '    If MyTable.Rows(0)(0).ToString = "updated" Then
            '        lbl_error.Visible = False
            '        lblStatus.Text = GetLocalResourceObject("msgSave").ToString
            '        lblStatus.Visible = True

            '        MyTable = objCommonFunctions.PopulateStationeryAvailableDataTable(userName, "", "", dis_code, True)
            '        If MyTable.Rows.Count > 0 Then
            '            existing_grid.DataSource = MyTable
            '            existing_grid.DataBind()
            '            lbl_error.Visible = False
            '        Else
            '            lbl_error.Visible = True
            '            lbl_error.Text = GetLocalResourceObject("errNoRecord")
            '        End If
            '    Else
            '        lbl_error.Text = MyTable.Rows(0)(0).ToString
            '        lbl_error.Visible = True
            '        lblStatus.Visible = False
            '    End If
            'End If
        Catch ex As Exception
            lbl_error.Text = ex.Message
            lbl_error.Visible = True
            lblStatus.Visible = False
        End Try

    End Sub
    Protected Sub cancel_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_button.Click
        Response.Redirect("BallotBoxAllocation.aspx")
    End Sub
    Protected Sub ddlState_SelectedIndexChanged(sender As Object, e As EventArgs)
        If (ddlState.SelectedIndex <> -1) Then
            Dim district_ = objCommonFunctions.District_Master_Get(ddlState.SelectedValue)
            If district_.status_code = 200 Then
                div_district.Visible = True
                Dim list_ As List(Of District_Master)
                ' Usign Direct Cast as I have received a list from Proc
                list_ = DirectCast(district_.data, List(Of District_Master))
                ddlDis.DataSource = list_
                ddlDis.DataTextField = "District_Name"  ' The property to display
                ddlDis.DataValueField = "District_Code" ' The property to use as the value
                ddlDis.DataBind()
                If user.UserLevel <> "000" Then
                    ddlState.Enabled = False
                Else
                    ddlState.Enabled = True
                End If
                Dim foundSize As ListItem = ddlDis.Items.FindByValue(user.DistrictCode)
                If foundSize IsNot Nothing Then
                    ddlDis.SelectedIndex = ddlDis.Items.IndexOf(foundSize)
                Else
                    ' Handle the case where the value was not found
                    ddlDis.SelectedIndex = 0 'sets the index to the first item.
                End If
                ddlDis_SelectedIndexChanged(ddlDis, EventArgs.Empty)
            Else
                FillGrid(sender)
                div_district.Visible = False
                'div_warehouse.Visible = False
                div_block.Visible = False
                div_Polparty.Visible = False
            End If
        End If
    End Sub
    Protected Sub ddlDis_SelectedIndexChanged(sender As Object, e As EventArgs)
        If (ddlDis.SelectedIndex <> -1) Then
            If ddlDis.SelectedValue = user.DistrictCode Then
                Dim block_ = objCommonFunctions.Block_Master_Get(ddlState.SelectedValue, ddlDis.SelectedValue)
                If block_.status_code = 200 Then
                    div_block.Visible = True
                    Dim list_ As List(Of Block_Master)
                    ' Usign Direct Cast as I have received a list from Proc
                    list_ = DirectCast(block_.data, List(Of Block_Master))
                    ddlBlk.DataSource = list_
                    ddlBlk.DataTextField = "Block_Name"     ' The property to display
                    ddlBlk.DataValueField = "Block_Code"    ' The property to use as the value
                    ddlBlk.DataBind()
                    Dim foundSize As ListItem = ddlBlk.Items.FindByValue(user.BlockCode)
                    If foundSize IsNot Nothing Then
                        ddlBlk.SelectedIndex = ddlBlk.Items.IndexOf(foundSize)
                    Else
                        ' Handle the case where the value was not found
                        ddlBlk.SelectedIndex = 0 'sets the index to the first item.
                    End If
                    'ddlBlk.SelectedIndex = 0
                    ddlBlk_SelectedIndexChanged(ddlBlk, EventArgs.Empty)
                Else
                    FillGrid(sender)
                    div_block.Visible = False
                    div_Polparty.Visible = False
                End If
            Else
                FillGrid(ddlDis)
                div_block.Visible = False
                div_Polparty.Visible = False
            End If
        End If
    End Sub
    'Protected Sub ddlWarehouse_SelectedIndexChanged(sender As Object, e As EventArgs)
    '    If (ddlDis.SelectedIndex <> -1) Then
    '        Dim block_ = objCommonFunctions.Block_Master_Get(ddlState.SelectedValue, ddlDis.SelectedValue)
    '        If block_.status_code = 200 Then
    '            div_block.Visible = True
    '            Dim list_ As List(Of Block_Master)
    '            ' Usign Direct Cast as I have received a list from Proc
    '            list_ = DirectCast(block_.data, List(Of Block_Master))
    '            ddlBlk.DataSource = list_
    '            ddlBlk.DataTextField = "Block_Name"     ' The property to display
    '            ddlBlk.DataValueField = "Block_Code"    ' The property to use as the value
    '            ddlBlk.DataBind()
    '            ddlBlk.SelectedIndex = 0
    '            ddlBlk_SelectedIndexChanged(ddlBlk, EventArgs.Empty)
    '        Else
    '            FillGrid(sender)
    '            div_block.Visible = False
    '            div_Polparty.Visible = False
    '        End If
    '    End If
    'End Sub
    Protected Sub ddlBlk_SelectedIndexChanged(sender As Object, e As EventArgs)
        If (user.UserLevel = "000") Then 'State user
            FillGrid(ddlDis)
            div_block.Visible = False
            Return
        ElseIf (user.UserLevel = "001" Or user.UserLevel = "006") Then 'DISTRICT USER
            If (ddlBlk.SelectedIndex <> -1) Then

                FillGrid(ddlBlk)
                div_block.Visible = True
                Return
            End If
        ElseIf (user.UserLevel = "002" Or user.UserLevel = "003") Then 'BLOCK USER
            If (ddlBlk.SelectedIndex <> -1) Then
                ddlDis.Enabled = False
                FillGrid(ddlBlk)
                div_block.Visible = True
                'Dim foundSize As ListItem = ddlBlk.Items.FindByValue(user.BlockCode)
                If (ddlBlk.SelectedValue = user.BlockCode) Then
                    lbl_error.Visible = False
                    Dim Polparty_ = objCommonFunctions.Polparty_Master_Get(ddlBlk.SelectedValue)
                    If Polparty_.status_code = 200 Then
                        div_Polparty.Visible = True
                        Dim list_ As List(Of Polparty_Master)
                        ' Usign Direct Cast as I have received a list from Proc
                        list_ = DirectCast(Polparty_.data, List(Of Polparty_Master))
                        ddlPolparty.DataSource = list_
                        ddlPolparty.DataTextField = "Polparty_Name"     ' The property to display
                        ddlPolparty.DataValueField = "Polparty_Code"    ' The property to use as the value
                        ddlPolparty.DataBind()
                        ddlPolparty.SelectedIndex = 0
                        ddlPolparty_SelectedIndexChanged(ddlPolparty, EventArgs.Empty)
                    Else
                        Dim list_ As List(Of Polparty_Master)
                        ddlPolparty.DataSource = list_
                        ddlPolparty.Visible = False
                        lbl_error.Text = Polparty_.status_message
                        lbl_error.Visible = True
                        FillGrid(sender)
                        div_Polparty.Visible = False
                    End If
                Else
                    Dim list_ As List(Of Polparty_Master)
                    ddlPolparty.DataSource = list_

                    div_Polparty.Visible = False
                    Return
                End If
            End If
        Else

                Dim Polparty_ = objCommonFunctions.Polparty_Master_Get(ddlBlk.SelectedValue)
            If Polparty_.status_code = 200 Then
                div_Polparty.Visible = True
                Dim list_ As List(Of Polparty_Master)
                ' Usign Direct Cast as I have received a list from Proc
                list_ = DirectCast(Polparty_.data, List(Of Polparty_Master))
                ddlPolparty.DataSource = list_
                ddlPolparty.DataTextField = "Polparty_Name"     ' The property to display
                ddlPolparty.DataValueField = "Polparty_Code"    ' The property to use as the value
                ddlPolparty.DataBind()
                ddlPolparty.SelectedIndex = 0
                ddlPolparty_SelectedIndexChanged(ddlPolparty, EventArgs.Empty)
            Else
                FillGrid(sender)
                div_Polparty.Visible = False
            End If
        End If

    End Sub

    Protected Sub ddlPolparty_SelectedIndexChanged(sender As Object, e As EventArgs)
        If (ddlPolparty.SelectedIndex <> -1) Then
            FillGrid(sender)
        End If
    End Sub
    Protected Sub existing_grid_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName = "AllocateQR" Then
            Dim values As String() = e.CommandArgument.ToString().Split(",")
            Dim qrTextDescription As String = values(0)
            Dim qr_to_user As String = values(1)

            ' Perform your update logic here using qrTextDescription
            ' Example: Update a database record based on qrTextDescription
            ' System.Diagnostics.Debug.WriteLine($"Update row triggered for qr_text_description: {qrTextDescription}")

            ' Example: Update the data source and rebind the GridView
            UpdateData(qrTextDescription, qr_to_user, 3)

        End If
    End Sub

    Protected Sub existing_grid1_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName = "DeAllocateQR" Then
            Dim values As String() = e.CommandArgument.ToString().Split(",")
            Dim qrTextDescription As String = values(0)
            Dim qr_to_user As String = values(1)

            ' Perform your update logic here using qrTextDescription
            ' Example: Update a database record based on qrTextDescription
            ' System.Diagnostics.Debug.WriteLine($"Update row triggered for qr_text_description: {qrTextDescription}")

            ' Example: Update the data source and rebind the GridView
            UpdateData(qrTextDescription, qr_to_user, 4)

        End If
    End Sub

    Protected Sub existing_grid_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)

    End Sub

    Private Sub existing_grid_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles existing_grid.RowDataBound

    End Sub

    Public Sub FillGrid(_picker As DropDownList, Optional _value As String = "")
        Dim available_stock_list_ As List(Of Ballot.Allocate_QR)
        available_stock_list_ = New List(Of Ballot.Allocate_QR)
        Dim ballot_stock_at_picker_list_ As List(Of Ballot.Allocate_QR)
        ballot_stock_at_picker_list_ = New List(Of Ballot.Allocate_QR)


        If _picker.SelectedValue <> "" Then
            _value = _picker.SelectedValue
        End If
        Dim ballot_stock_ = objCommonFunctions.Show_Ballot_User_Id(user.Code, _value, "from")
        If (ballot_stock_.status_code = 200) Then
            available_stock_list_ = DirectCast(ballot_stock_.data, List(Of Ballot.Allocate_QR))
        End If

        Dim ballot_stock_at_picker_ = objCommonFunctions.Show_Ballot_User_Id(user.Code, _value, "to")
        If (ballot_stock_at_picker_.status_code = 200) Then
            ballot_stock_at_picker_list_ = DirectCast(ballot_stock_at_picker_.data, List(Of Ballot.Allocate_QR))

        End If

        existing_grid.DataSource = available_stock_list_
        existing_grid.DataBind()
        'If available_stock_list_.Any Then
        '    existing_grid.DataSource = available_stock_list_
        'Else
        '    existing_grid.DataSource = Nothing

        'End If
        'existing_grid.DataBind()

        existing_grid1.DataSource = ballot_stock_at_picker_list_
        existing_grid1.DataBind()

        'existing_grid1.Controls.Clear()
        'If ballot_stock_at_picker_list_.Count >= 0 Then
        '    existing_grid1.DataSource = ballot_stock_at_picker_list_
        '    existing_grid1.DataBind()
        'Else
        '    existing_grid1.DataSource = Nothing
        '    existing_grid1.DataBind()
        'End If



    End Sub

    Private Sub UpdateData(qrTextDescription As String, qr_to_user As String, action As Int16)
        Dim DBAccess = New commonFunctions()
        Dim Result As Generic_Response = DBAccess.UpdateBallotBoxStock(qrTextDescription, "", 1, 0, 0, 0, 0, user.Code, qr_to_user, action)
        If (Result.status_code = 200 Or Result.status_code = 201) Then
            Dim mypicker As DropDownList = New DropDownList
            FillGrid(mypicker, qr_to_user)
        End If
    End Sub

End Class
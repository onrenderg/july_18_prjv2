Imports System.Drawing

Public Class allocateStationery
    Inherits SetCulture
    Dim objCommonFunctions As New commonFunctions()
    Dim MyTable As New DataTable
    Dim userName, userLevel, dis_code, myLang As String
    Dim unitCounts As Int64
    Dim itemFor As String = ""
    Dim total As Int64
    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    '    Response.Cache.SetLastModified(DateTime.Now)
    '    Response.Cache.SetAllowResponseInBrowserHistory(False)
    '    Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
    '    Response.Cache.SetCacheability(HttpCacheability.NoCache)

    '    Response.Cache.SetNoStore()
    '    userName = Session("UserID")
    '    dis_code = Session("gblDistrictCode")
    '    userLevel = Session("UserLevel")
    '    myLang = Session("MyLanguage").ToString
    '    If Not IsPostBack Then
    '        Dim ddl As New DropDownList
    '        ' check for MC,ULB, PRI below 
    '        objCommonFunctions.PopulateSECCommonMasterValues(rbPriPost, "ELECTIONFOR")
    '        objCommonFunctions.PopulateCommonMasterDropdown(ddl, "STATIONERY", rbPriPost.SelectedValue)
    '        rbUnitType.DataSource = ddl.DataSource
    '        rbUnitType.DataTextField = ddl.DataTextField
    '        rbUnitType.DataValueField = ddl.DataValueField
    '        rbUnitType.DataBind()
    '        rbUnitType.SelectedIndex = 0
    '        lblBlk.Text = Resources.Resource.Mc.ToString

    '        If userLevel = "000" Then 'Ask state user to select district , populate district for other users
    '            objCommonFunctions.PopulatePriDistricts(ddlDis, "003")
    '            ddlDis.Enabled = True
    '            ddlDis.AutoPostBack = True
    '            dvBlock.Visible = False
    '            lbl_error.Visible = False
    '            lblStatus.Visible = False
    '            MyTable = objCommonFunctions.PopulateStationeryAllocationDataTable(userName, rbUnitType.SelectedValue, "MC", "0")
    '        ElseIf userLevel = "001" Or userLevel = "006" Or userLevel = "007" Then
    '            ddlDis.Items.Clear()
    '            ddlDis.Items.Insert(0, New ListItem(IIf(myLang = "hi-IN", Session("gblDistrictNameLocal"), Session("gblDistrictName")), dis_code))
    '            ddlDis.SelectedIndex = 0
    '            ddlDis.AutoPostBack = False
    '            objCommonFunctions.PopulatePriBlocks(ddlBlk, dis_code, rbPriPost.SelectedValue, "All")
    '            dvBlock.Visible = True
    '            MyTable = objCommonFunctions.PopulateStationeryAllocationDataTable(userName, rbUnitType.SelectedValue, "MC", "0")
    '        Else
    '            lbl_error.Visible = True
    '            lbl_error.Text = GetLocalResourceObject("errNotAllowed")
    '        End If
    '        If MyTable.Rows.Count > 0 Then
    '            existing_grid.DataSource = MyTable
    '            existing_grid.DataBind()
    '            lbl_error.Visible = False
    '        Else
    '            lbl_error.Visible = True
    '            lbl_error.Text = GetLocalResourceObject("errNoRecord")
    '        End If
    '    End If

    '    Select Case rbPriPost.SelectedValue
    '        Case "001"
    '            itemFor = "GPYT"
    '        Case "002"
    '            itemFor = "NPLK"
    '        Case "003"
    '            itemFor = "MC"
    '        Case Else
    '            itemFor = rbPriPost.SelectedValue
    '    End Select
    'End Sub

    'Protected Sub save_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save_button.Click
    '    Dim flag As Boolean = True
    '    Dim errorStr As String = ""
    '    Dim Item As New ListItem

    '    Dim id As Label
    '    Dim AvailQty As Label
    '    Dim qty As TextBox
    '    Dim AvailFromSrNo As Label
    '    Dim AvailToSrNo As Label
    '    Dim FromSrNo As TextBox
    '    Dim ToSrNo As TextBox

    '    If userLevel = "000" Then
    '        If Val(ddlDis.SelectedValue) = 0 Then
    '            flag = False
    '            errorStr = "<li>" + GetLocalResourceObject("errNoDis") + "</li>"
    '        End If
    '    ElseIf Val(ddlBlk.SelectedValue) = 0 Then
    '        flag = False
    '        errorStr = "<li>" + GetLocalResourceObject("errNoBlock") + "</li>"
    '    End If
    '    If flag = False Then
    '        lbl_error.Text = errorStr
    '        lbl_error.Visible = True
    '    Else
    '        'saveStationeryMapping
    '        Dim tbl_StationeryMapping As New DataTable
    '        tbl_StationeryMapping.Columns.Add(New DataColumn("UnitID", GetType(Integer)))
    '        tbl_StationeryMapping.Columns.Add(New DataColumn("AllotedQuantity", GetType(String)))
    '        tbl_StationeryMapping.Columns.Add(New DataColumn("FromSrNo", GetType(Integer)))
    '        tbl_StationeryMapping.Columns.Add(New DataColumn("ToSrNo", GetType(Integer)))
    '        Dim drStationeryMapping As DataRow = Nothing

    '        For Each dr As GridViewRow In existing_grid.Rows
    '            id = DirectCast(dr.FindControl("id"), Label)
    '            AvailQty = DirectCast(dr.FindControl("lblStockCnt"), Label)
    '            qty = DirectCast(dr.FindControl("lblItemCounts"), TextBox)
    '            AvailFromSrNo = DirectCast(dr.FindControl("lblFrmSrNo"), Label)
    '            AvailToSrNo = DirectCast(dr.FindControl("lblToSr"), Label)
    '            FromSrNo = DirectCast(dr.FindControl("lblFromSrNo"), TextBox)
    '            ToSrNo = DirectCast(dr.FindControl("lblToSrNo"), TextBox)
    '            If Val(qty.Text.ToString) > 0 Then
    '                drStationeryMapping = tbl_StationeryMapping.NewRow()
    '                drStationeryMapping("UnitID") = id.Text
    '                drStationeryMapping("AllotedQuantity") = qty.Text
    '                drStationeryMapping("FromSrNo") = Val(FromSrNo.Text)
    '                drStationeryMapping("ToSrNo") = Val(ToSrNo.Text)

    '                'Sr No Logic is to be Fixed
    '                Try

    '                    If rbUnitType.SelectedValue = "PNKSEAL" Or rbUnitType.SelectedValue = "RCBOOK" Or rbUnitType.SelectedValue = "BALLOT" Then
    '                        AvailFromSrNo.Text = AvailFromSrNo.Text.Substring(1)
    '                        AvailToSrNo.Text = AvailToSrNo.Text.Substring(3)
    '                        AvailToSrNo.Text = AvailToSrNo.Text.Substring(0, Len(AvailToSrNo.Text) - 1)

    '                        If Not (Val(FromSrNo.Text) >= Val(AvailFromSrNo.Text) And Val(FromSrNo.Text) <= Val(AvailToSrNo.Text)) Then
    '                            lbl_error.Visible = True
    '                            lbl_error.Text = "Invalid From Sr.No. Please check SrNo of the Available Stock. " 'GetLocalResourceObject("errInvalidCount")
    '                            errorStr = errorStr + "<li>" + lbl_error.Text + "</li>"
    '                            dr.BorderColor = Color.Red
    '                            FromSrNo.Focus()
    '                            flag = False
    '                        Else
    '                            dr.BorderColor = Color.Empty
    '                        End If

    '                        If Not (Val(ToSrNo.Text) >= Val(AvailFromSrNo.Text) And Val(ToSrNo.Text) <= Val(AvailToSrNo.Text)) Then
    '                            lbl_error.Visible = True
    '                            lbl_error.Text = "Invalid To Sr.No. Please check SrNo of the Available Stock. " 'GetLocalResourceObject("errInvalidCount")
    '                            errorStr = errorStr + "<li>" + lbl_error.Text + "</li>"
    '                            dr.BorderColor = Color.Red
    '                            ToSrNo.Focus()
    '                            flag = False
    '                        Else
    '                            dr.BorderColor = Color.Empty
    '                        End If
    '                        If Val(ToSrNo.Text) - Val(FromSrNo.Text) + 1 <> Val(qty.Text) Then
    '                            lbl_error.Visible = True
    '                            lbl_error.Text = "Invalid Count of Items! Count of Items (From Sr.No. to To Sr.No.) must be equal to Total Available Stock. " 'GetLocalResourceObject("errInvalidCount")
    '                            errorStr = errorStr + "<li>" + lbl_error.Text + "</li>"
    '                            dr.BorderColor = Color.Red
    '                            qty.Focus()
    '                            flag = False
    '                        Else
    '                            dr.BorderColor = Color.Empty
    '                        End If
    '                    End If
    '                Catch ex As Exception

    '                End Try

    '                If Val(AvailQty.Text) < Val(qty.Text) Then
    '                    lbl_error.Visible = True
    '                    lbl_error.Text = "Invalid Count of Items! Items allotted cannot exceed Available Stock Count. " 'GetLocalResourceObject("errInvalidCount")
    '                    errorStr = errorStr + "<li>" + lbl_error.Text + "</li>"
    '                    dr.BorderColor = Color.Red
    '                    qty.Focus()
    '                    flag = False
    '                Else
    '                    dr.BorderColor = Color.Empty
    '                End If
    '                tbl_StationeryMapping.Rows.Add(drStationeryMapping)
    '            End If
    '        Next

    '        If flag Then
    '            Dim mytable As New DataTable
    '            Dim mycommand As New SqlClient.SqlCommand
    '            Dim MyInterface As ErmsInterface.hbInterface
    '            Dim Implementer As New ErmsBLL
    '            '    'This is to save the stationery allocation
    '            Try
    '                MyInterface = Implementer
    '                mycommand.Parameters.Clear()
    '                ' @UnitID,@UnitType,@BlkCode,@AllottedBy
    '                mycommand.Parameters.AddWithValue("@tbl_StaioneryMapping", tbl_StationeryMapping)
    '                mycommand.Parameters.AddWithValue("@user_id", Session("UserID"))

    '                Dim AllotedTo As String = ""
    '                Select Case userLevel
    '                    Case "000"
    '                        AllotedTo = ddlDis.SelectedValue '& "000000000"
    '                    Case "001", "006"
    '                        AllotedTo = ddlBlk.SelectedValue '& "000000"

    '                        'Case "002", "003"
    '                        '    AllotedTo = ddlPanch.SelectedValue & "000"
    '                End Select
    '                mycommand.Parameters.AddWithValue("@AllotedTo", AllotedTo)
    '                mycommand.Parameters.AddWithValue("@role", userLevel)

    '                mytable = MyInterface.INSERTData(mycommand, "saveStationeryMapping")

    '                If mytable.Rows.Count > 0 Then
    '                    If Not String.IsNullOrEmpty(mytable.Rows(0)(0).ToString) Then
    '                        lbl_error.Text = mytable.Rows(0)(0).ToString
    '                        lbl_error.Visible = True
    '                        lblStatus.Visible = False
    '                        Exit Sub
    '                    End If
    '                End If
    '            Catch ex As Exception
    '                lbl_error.Text = ex.Message
    '                lbl_error.Visible = True
    '                lblStatus.Visible = False
    '                Exit Sub
    '            End Try

    '            lbl_error.Visible = False

    '            lblStatus.Text = GetLocalResourceObject("msgSave").ToString
    '            lblStatus.Visible = True
    '            mytable = objCommonFunctions.PopulateStationeryAllocationDataTable(userName, rbUnitType.SelectedValue, itemFor, "0")
    '            existing_grid.DataSource = mytable
    '            existing_grid.DataBind()
    '        Else
    '            lbl_error.Text = errorStr
    '            lbl_error.Visible = True
    '            lblStatus.Visible = False
    '        End If
    '    End If
    'End Sub
    'Protected Sub cancel_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_button.Click
    '    Response.Redirect("allocateStationery.aspx")
    'End Sub

    'Protected Sub rbPriPost_SelectedIndexChanged(sender As Object, e As EventArgs)
    '    Dim ddl As New DropDownList
    '    If userLevel = "000" Then
    '        objCommonFunctions.PopulatePriDistricts(ddlDis, rbPriPost.SelectedValue)
    '    End If
    '    objCommonFunctions.PopulateCommonMasterDropdown(ddl, "STATIONERY", rbPriPost.SelectedValue)
    '    rbUnitType.DataSource = ddl.DataSource
    '    rbUnitType.DataTextField = ddl.DataTextField
    '    rbUnitType.DataValueField = ddl.DataValueField
    '    rbUnitType.DataBind()
    '    rbUnitType.SelectedIndex = 0

    '    lblMappingCount.Text = ""
    '    existing_grid.DataSource = New DataTable
    '    existing_grid.DataBind()

    '    '' ddlBlk_SelectedIndexChanged(ddlBlk, Nothing)
    '    lbl_error.Visible = False
    '    lblStatus.Visible = False

    '    If dvBlock.Visible Then
    '        Select Case rbPriPost.SelectedValue
    '            Case "001"
    '                lblBlk.Text = Resources.Resource.Blk.ToString
    '            Case "002"
    '                lblBlk.Text = Resources.Resource.Ulb.ToString
    '            Case "003"
    '                lblBlk.Text = Resources.Resource.Mc.ToString
    '        End Select
    '        objCommonFunctions.PopulatePriBlocks(ddlBlk, ddlDis.SelectedValue, rbPriPost.SelectedValue, "All")
    '    End If


    '    MyTable = objCommonFunctions.PopulateStationeryAllocationDataTable(userName, rbUnitType.SelectedValue, itemFor, "0")
    '    If MyTable.Rows.Count > 0 Then
    '        existing_grid.DataSource = MyTable
    '        existing_grid.DataBind()
    '        lbl_error.Visible = False
    '    Else
    '        lbl_error.Visible = True
    '        lbl_error.Text = GetLocalResourceObject("errNoRecord")
    '    End If
    '    Exit Sub
    'End Sub

    'Protected Sub rbUnitType_SelectedIndexChanged(sender As Object, e As EventArgs)
    '    existing_grid.DataSource = New DataTable
    '    existing_grid.DataBind()

    '    lbl_error.Visible = False
    '    lblStatus.Visible = False
    '    MyTable = objCommonFunctions.PopulateStationeryAllocationDataTable(userName, rbUnitType.SelectedValue, itemFor, "")
    '    If MyTable.Rows.Count > 0 Then
    '        existing_grid.DataSource = MyTable
    '        existing_grid.DataBind()
    '        lbl_error.Visible = False
    '    Else
    '        lbl_error.Visible = True
    '        lbl_error.Text = GetLocalResourceObject("errNoRecord")
    '    End If
    'End Sub

    'Private Sub MessageBox(sender As Object, ByVal sMsg As String)
    '    Dim sb As New StringBuilder()
    '    Dim oFormObject As System.Web.UI.Control

    '    sMsg = sMsg.Replace("'", "\'")
    '    sMsg = sMsg.Replace(Chr(34), "\" & Chr(34))
    '    sMsg = sMsg.Replace(vbCrLf, "\n")
    '    sMsg = "alert('" & sMsg & "');"

    '    sb = New StringBuilder()
    '    sb.Append(sMsg)

    '    For Each oFormObject In Me.Controls
    '        If TypeOf oFormObject Is HtmlForm Then
    '            Exit For
    '        End If
    '    Next
    '    ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.[GetType](), "", sMsg, True)
    'End Sub

    'Protected Sub ddlDis_SelectedIndexChanged(sender As Object, e As EventArgs)
    '    If Val(ddlDis.SelectedValue) > 0 Then
    '        existing_grid.DataSource = Nothing
    '        existing_grid.DataBind()
    '        MyTable = objCommonFunctions.PopulateStationeryAllocationDataTable(userName, rbUnitType.SelectedValue, itemFor, ddlDis.SelectedValue)


    '        If MyTable.Rows.Count > 0 Then
    '            dvList.Visible = True
    '            existing_grid.DataSource = MyTable
    '            existing_grid.DataBind()

    '            Dim sum As Object = MyTable.Compute("SUM(items_allocated_To_selected_user)", "")

    '            'Handle potential nulls (DBNull)
    '            If sum Is DBNull.Value Then
    '                sum = 0 ' Or any other default value
    '            End If

    '            total = CDbl(sum) ' Convert to the appropriate data type (Double, Integer, Decimal, etc.)

    '            If userLevel = "000" Then
    '                lblMappingCount.Text = IIf(myLang = "hi-IN", ddlDis.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + total.ToString, rbUnitType.SelectedItem.Text + " Allocated to " + ddlDis.SelectedItem.Text + ": " + total.ToString)
    '            ElseIf userLevel = "001" Or userLevel = "006" Then
    '                lblMappingCount.Text = IIf(myLang = "hi-IN", ddlBlk.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + total.ToString, rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + total.ToString)
    '            Else
    '                If rbPriPost.SelectedValue = "001" Then
    '                    lblMappingCount.Text = IIf(myLang = "hi-IN", ddlBlk.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + total.ToString, rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + total.ToString)
    '                    'Else
    '                    '    lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + unitCounts.ToString, rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + unitCounts.ToString)
    '                End If
    '            End If
    '            lbl_error.Visible = False
    '            If dvBlock.Visible Then
    '                objCommonFunctions.PopulatePriBlocks(ddlBlk, ddlDis.SelectedValue, rbPriPost.SelectedValue, "All")
    '            End If
    '        Else
    '            dvList.Visible = False

    '            lblMappingCount.Text = ""
    '            lbl_error.Visible = True
    '            lbl_error.Text = GetLocalResourceObject("errNoRecord")
    '        End If
    '    Else
    '        dvList.Visible = False
    '        lblMappingCount.Text = ""
    '    End If
    'End Sub
    'Protected Sub ddlBlk_SelectedIndexChanged(sender As Object, e As EventArgs)
    '    If Val(ddlBlk.SelectedValue) > 0 Then
    '        MyTable = objCommonFunctions.PopulateStationeryAllocationDataTable(ddlDis.SelectedValue, rbUnitType.SelectedValue, itemFor, ddlBlk.SelectedValue)

    '        If MyTable.Rows.Count > 0 Then
    '            existing_grid.DataSource = MyTable
    '            existing_grid.DataBind()
    '            lbl_error.Visible = False
    '            dvList.Visible = True
    '        Else
    '            lbl_error.Visible = True
    '            lbl_error.Text = GetLocalResourceObject("errNoRecord")
    '            dvList.Visible = False
    '        End If
    '    Else
    '        dvList.Visible = False

    '    End If
    'End Sub

    'Protected Sub existing_grid_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)

    'End Sub

    'Private Sub existing_grid_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles existing_grid.RowDataBound

    '    If e.Row.RowType = DataControlRowType.Header Then
    '        unitCounts = 0
    '        If rbUnitType.SelectedValue = "PNKSEAL" Or rbUnitType.SelectedValue = "RCBOOK" Or rbUnitType.SelectedValue = "BALLOT" Then
    '            'e.Row.Cells(2).Text = "Stock Available<br/>[From SrNo - To SrNo]"
    '            e.Row.Cells(5).Visible = True
    '            e.Row.Cells(4).Visible = True
    '        Else

    '            Dim templateField As TemplateField = DirectCast(existing_grid.Columns(2), TemplateField)
    '            If ddlDis.SelectedValue IsNot Nothing Then
    '                If ddlDis.SelectedIndex <> 0 Then
    '                    e.Row.Cells(2).Text = templateField.HeaderText + ": " + ddlDis.SelectedItem.Text
    '                End If
    '            Else
    '                e.Row.Cells(2).Text = templateField.HeaderText
    '            End If
    '            e.Row.Cells(4).Visible = False
    '            e.Row.Cells(5).Visible = False
    '        End If
    '    ElseIf e.Row.RowType = DataControlRowType.DataRow Then
    '        Dim lblItemCounts As TextBox = DirectCast(e.Row.FindControl("lblItemCounts"), TextBox)
    '        Dim lblCnt As Label = DirectCast(e.Row.FindControl("lblCnt"), Label)
    '        'Dim RangeValidator1 As RangeValidator = DirectCast(e.Row.FindControl("RangeValidator1"), RangeValidator)
    '        'Dim RangeValidator2 As RangeValidator = DirectCast(e.Row.FindControl("RangeValidator2"), RangeValidator)
    '        Dim lblFrmSrNo As Label = DirectCast(e.Row.FindControl("lblFrmSrNo"), Label)
    '        Dim lblToSr As Label = DirectCast(e.Row.FindControl("lblToSr"), Label)
    '        Dim lblFromSrNo As TextBox = DirectCast(e.Row.FindControl("lblFromSrNo"), TextBox)
    '        Dim lblToSrNo As TextBox = DirectCast(e.Row.FindControl("lblToSrNo"), TextBox)


    '        If rbUnitType.SelectedValue = "PNKSEAL" Or rbUnitType.SelectedValue = "RCBOOK" Or rbUnitType.SelectedValue = "BALLOT" Then
    '            e.Row.Cells(5).Visible = True
    '            e.Row.Cells(4).Visible = True
    '            lblFrmSrNo.Visible = True
    '            lblToSr.Visible = True
    '            'RangeValidator1.Enabled = True
    '            'RangeValidator2.Enabled = True
    '            'RangeValidator1.MinimumValue = Val(lblFrmSrNo.Text)
    '            'RangeValidator1.MaximumValue = Val(lblToSr.Text)
    '            'RangeValidator2.MinimumValue = Val(lblFrmSrNo.Text)
    '            'RangeValidator2.MaximumValue = Val(lblToSr.Text)
    '            lblFromSrNo.Text = lblFrmSrNo.Text
    '            lblFrmSrNo.Text = " [" + lblFrmSrNo.Text
    '            lblToSr.Text = " - " + lblToSr.Text + "]"
    '        Else
    '            e.Row.Cells(4).Visible = False
    '            e.Row.Cells(5).Visible = False
    '            lblFrmSrNo.Visible = False
    '            lblToSr.Visible = False
    '            Try
    '                'RangeValidator1.Enabled = False
    '                'RangeValidator2.Enabled = False
    '            Catch ex As Exception

    '            End Try

    '        End If

    '        unitCounts = unitCounts + IIf(lblItemCounts.Text = "" Or lblItemCounts.Text = "0", 0, lblItemCounts.Text)
    '        lblCnt.Text = unitCounts.ToString
    '        If Val(lblItemCounts.Text) = 0 Then
    '            lblItemCounts.Text = ""
    '        End If

    '        If Val(lblCnt.Text) = 0 Then
    '            lblCnt.Text = ""
    '        End If
    '    ElseIf e.Row.RowType = DataControlRowType.Footer Then

    '    End If
    'End Sub

    'Protected Sub existing_grid_RowCommand(sender As Object, e As GridViewCommandEventArgs)

    'End Sub


End Class
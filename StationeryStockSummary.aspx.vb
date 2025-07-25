Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO
Imports ClosedXML.Excel
Imports SEC_InventoryMgmt.Ballot

Public Class StationeryStockSummary
    Inherits SetCulture
    Dim objCommonFunctions As New commonFunctions()
    Dim MyTable As New DataTable
    Dim userName, userLevel, dis_code, myLang As String
    Dim unitCounts As Int64
    Dim LoginUser As UserData

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
        LoginUser = commonFunctions.GetUserDataFromSession()
        userName = Session("UserID")
        dis_code = Session("gblDistrictCode")
        userLevel = Session("UserLevel")
        myLang = Session("MyLanguage").ToString

        If Not IsPostBack Then
            Dim ddl As New DropDownList
            lblTitle.Text = Resources.Resource.menuStationaryStockSummary
            objCommonFunctions.PopulateSECCommonMasterValues(rbPriPost, "ELECTIONFOR", Session("NNN_Code"))
            rbPriPost.SelectedIndex = 0
            rbPriPost_SelectedIndexChanged(rbPriPost, EventArgs.Empty)

            lbl_error.Visible = False
            lblStatus.Visible = False
            'objCommonFunctions.PopulateCommonMasterDropdown(ddl, "STATIONERY", rbPriPost.SelectedValue)
            'rbUnitType.DataSource = ddl.DataSource
            'rbUnitType.DataTextField = ddl.DataTextField
            'rbUnitType.DataValueField = ddl.DataValueField
            'rbUnitType.DataBind()
            'rbUnitType.SelectedIndex = 0

            'If userLevel = "001" Or userLevel = "006" Or userLevel = "007" Then
            '    ddlDis.Items.Clear()
            '    ddlDis.Items.Insert(0, New ListItem(IIf(myLang = "hi-IN", Session("gblDistrictNameLocal"), Session("gblDistrictName")), dis_code))
            '    ddlDis.SelectedIndex = 0
            'End If
            'fill_grid()
        End If
    End Sub
    Private Sub fill_grid()
        Dim user = LoginUser.Code
        Dim is_self As Boolean = False
        If (ddlDis.SelectedIndex > 1 And ddlBlk.SelectedIndex >= 0) Then
            If ddlBlk.SelectedValue = "99999" Then
                is_self = True
                user = ddlDis.SelectedValue
            ElseIf ddlBlk.SelectedValue < 1300 Then
                is_self = False
                user = ddlDis.SelectedValue
            Else
                is_self = False
                user = ddlBlk.SelectedValue
            End If
        ElseIf ddlDis.SelectedValue = "99999" Then
            is_self = True
        ElseIf ddlDis.SelectedValue > 13 Then
            is_self = False
            user = ddlDis.SelectedValue
        Else
            user = user
        End If

        Dim stock_owner_ = objCommonFunctions.GetStationeryStockSummary(user, rbPriPost.SelectedValue, rbUnitType.SelectedValue, is_self)
        If (stock_owner_.status_code = 200) Then
            Dim available_stock_list_ As List(Of StockItem.StockSummary)
            available_stock_list_ = DirectCast(stock_owner_.data, List(Of StockItem.StockSummary))
            existing_grid.DataSource = available_stock_list_
            existing_grid.DataBind()
            If available_stock_list_.Count > 0 Then
                lbl_error.Visible = False
            Else
                lbl_error.Visible = True
                lbl_error.Text = GetLocalResourceObject("errNoRecord")
            End If
        End If
    End Sub

    Protected Sub cancel_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_button.Click
        Response.Redirect("StationeryStockSummary.aspx")
    End Sub

    Protected Sub rbPriPost_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As New DropDownList
        objCommonFunctions.PopulateCommonMasterDropdown(ddl, "STATIONERY", rbPriPost.SelectedValue)
        rbUnitType.DataSource = ddl.DataSource
        rbUnitType.DataTextField = ddl.DataTextField
        rbUnitType.DataValueField = ddl.DataValueField
        rbUnitType.DataBind()
        rbUnitType.SelectedIndex = 0
        rbUnitType_SelectedIndexChanged(rbUnitType, EventArgs.Empty)
        'existing_grid.DataSource = New DataTable
        'existing_grid.DataBind()

        '' ddlBlk_SelectedIndexChanged(ddlBlk, Nothing)
        'lbl_error.Visible = False
        'lblStatus.Visible = False
        'DV.Visible = False
        'fill_grid()
    End Sub

    Protected Sub rbUnitType_SelectedIndexChanged(sender As Object, e As EventArgs)

        'fill_grid()
        If rbUnitType.SelectedIndex <> -1 Then
            objCommonFunctions.PopulateDistricts(ddlDis, "Self")
            Dim foundItem As ListItem = ddlDis.Items.Cast(Of ListItem)().FirstOrDefault(Function(item) item.Value = LoginUser.DistrictCode)

            If LoginUser.UserLevel <> "000" Then
                If foundItem IsNot Nothing Then
                    ddlDis.SelectedIndex = ddlDis.Items.IndexOf(foundItem)
                End If
                ddlDis.Enabled = False
            Else
                ddlDis.SelectedIndex = 0
            End If
            ddlDis_SelectedIndexChanged(ddlDis, EventArgs.Empty)
        End If
    End Sub

    Protected Sub ddlDis_SelectedIndexChanged(sender As Object, e As EventArgs)
        If ddlDis.SelectedIndex <> -1 Then
            If LoginUser.UserLevel = "000" Or LoginUser.UserLevel = "001" Or LoginUser.UserLevel = "002" Or LoginUser.UserLevel = "003" Or LoginUser.UserLevel = "006" Or LoginUser.UserLevel = "007" Then
                Select Case rbPriPost.SelectedValue
                    Case "001"
                        lblBlk.Text = Resources.Resource.Blk.ToString
                    Case "002"
                        lblBlk.Text = Resources.Resource.Ulb.ToString
                    Case "003"
                        lblBlk.Text = Resources.Resource.Mc.ToString
                End Select
                objCommonFunctions.PopulatePriBlocks(ddlBlk, ddlDis.SelectedValue, rbPriPost.SelectedValue, "Self")
                dvBlock.Visible = True
                If (LoginUser.UserLevel = "002" Or LoginUser.UserLevel = "003") Then
                    ddlDis.Enabled = False
                End If
                If ddlDis.Enabled = False Then
                    Dim foundItem As ListItem = ddlBlk.Items.Cast(Of ListItem)().FirstOrDefault(Function(item) item.Value = LoginUser.BlockCode)

                    If foundItem IsNot Nothing Then
                        ddlBlk.SelectedIndex = ddlBlk.Items.IndexOf(foundItem)
                        ddlBlk.Enabled = False
                    End If

                End If
                ddlBlk_SelectedIndexChanged(ddlBlk, EventArgs.Empty)
                fill_grid()
            Else
                lbl_error.Visible = True
                lbl_error.Text = GetLocalResourceObject("errNotAllowed")
            End If
        Else

        End If
    End Sub
    Protected Sub ddlBlk_SelectedIndexChanged(sender As Object, e As EventArgs)
        If ddlBlk.SelectedIndex <> -1 Then
            fill_grid()
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
            Dim stock_owner_ = objCommonFunctions.GetStationeryStockSummary(userName, rbUnitType.SelectedValue, rbPriPost.SelectedValue, False)
            If (stock_owner_.status_code = 200) Then
                Dim available_stock_list_ As List(Of StockItem.StockSummary)
                available_stock_list_ = DirectCast(stock_owner_.data, List(Of StockItem.StockSummary))
                existing_grid.DataSource = available_stock_list_
                existing_grid.DataBind()
            End If
            Dim MyTable = existing_grid.DataSource
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
        'If rbUnitType.SelectedValue = "PNKSEAL" Or rbUnitType.SelectedValue = "RCBOOK" Or rbUnitType.SelectedValue = "BALLOT" Then
        '    e.Row.Cells(0).Visible = False
        '    e.Row.Cells(4).Visible = True
        '    e.Row.Cells(5).Visible = True
        '    'e.Row.Cells(6).Visible = False
        'Else
        '    e.Row.Cells(4).Visible = False
        '    e.Row.Cells(5).Visible = False
        '    'e.Row.Cells(6).Visible = False
        'End If

        'If e.Row.RowType = DataControlRowType.Header Then
        '    unitCounts = 0
        'ElseIf e.Row.RowType = DataControlRowType.DataRow Then
        '    Dim lblFromSrNo As TextBox = DirectCast(e.Row.FindControl("lblFromSrNo"), TextBox)
        '    Dim lblItemCounts As TextBox = DirectCast(e.Row.FindControl("lblItemCounts"), TextBox)
        '    Dim lblCnt As Label = DirectCast(e.Row.FindControl("lblCnt"), Label)
        '    Dim FromSrNo As Label = DirectCast(e.Row.FindControl("FromSrNo"), Label)
        '    Dim ToSrNo As Label = DirectCast(e.Row.FindControl("ToSrNo"), Label)

        '    Try

        '        If lblFromSrNo IsNot Nothing Then
        '            Dim trigger1 As New AsyncPostBackTrigger()
        '            trigger1.ControlID = lblFromSrNo.UniqueID ' Use UniqueID for GridView controls
        '            trigger1.EventName = "TextChanged"
        '            panel1.Triggers.Add(trigger1)
        '        End If

        '        If lblItemCounts IsNot Nothing Then
        '            Dim trigger2 As New AsyncPostBackTrigger()
        '            trigger2.ControlID = lblItemCounts.UniqueID ' Use UniqueID for GridView controls
        '            trigger2.EventName = "TextChanged"
        '            panel1.Triggers.Add(trigger2)
        '        End If

        '    Catch ex As Exception

        '    End Try
        '    'unitCounts = unitCounts + IIf(lblItemCounts.Text = "" Or lblItemCounts.Text = "0", 0, lblItemCounts.Text)

        '    lblCnt.Text = ""
        'End If
    End Sub

    Protected Sub lblFromSrNo_TextChanged(sender As Object, e As EventArgs)
        UpdateToSrNo(sender)
    End Sub

    'Protected Sub lblItemCounts_TextChanged(sender As Object, e As EventArgs)
    '    ' Find the parent row of the TextBox that triggered the event
    '    Dim txtBox As TextBox = DirectCast(sender, TextBox)
    '    Dim row As GridViewRow = DirectCast(txtBox.NamingContainer, GridViewRow)

    '    ' Check if column 4 is visible
    '    If row.Cells(4).Visible Then
    '        UpdateToSrNo(row)
    '    End If
    'End Sub

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
    Protected Sub ExportExcel(sender As Object, e As EventArgs)
        Dim dt As DataTable = New DataTable("existing_grid")
        For Each cell As TableCell In existing_grid.HeaderRow.Cells
            dt.Columns.Add(cell.Text)
        Next

        For Each row As GridViewRow In existing_grid.Rows
            dt.Rows.Add()
            For i As Integer = 0 To row.Cells.Count - 1
                If row.Cells(i).Controls.Count > 0 Then
                    dt.Rows(dt.Rows.Count - 1)(i) = (TryCast(row.Cells(i).Controls(1), Label)).Text
                Else
                    dt.Rows(dt.Rows.Count - 1)(i) = row.Cells(i).Text
                End If
            Next
        Next

        Using wb As XLWorkbook = New XLWorkbook()
            wb.Worksheets.Add(dt)
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=SummaryReport.xlsx")
            Using MyMemoryStream As MemoryStream = New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub
End Class
Imports System.IO
Imports ClosedXML.Excel

Public Class printQR
    Inherits SetCulture
    Dim objCommonFunctions As New commonFunctions()
    Dim MyTable As New DataTable
    Dim userName, userLevel, dis_code, blk_code, myLang As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        userName = Session("UserID")
        dis_code = Session("gblDistrictCode")
        blk_code = Session("gblBlockCode")
        userLevel = Session("UserLevel")
        myLang = Session("MyLanguage").ToString
        'Session.Remove("mapData")
        If Not IsPostBack Then
            Session.Remove("mapData")
            Dim UnitType As String = "AL"
            If userLevel = "012" Or userLevel = "013" Then
                If blk_code.StartsWith("1") Then
                    UnitType = "BB"
                ElseIf blk_code.StartsWith("0") Then
                    UnitType = "BC"
                End If
            End If
            objCommonFunctions.PopulateUnitTypes(rbUnitType, UnitType)

            If userLevel <> "000" Then 'Ask state user to select district , populate district for other users
                Dim ddl As New DropDownList
                objCommonFunctions.PopulateDistricts(ddl)
                For Each li As ListItem In ddl.Items
                    If li.Value = dis_code Then
                        ddlDistName.Items.Insert(0, li)
                        ddlDistName.SelectedIndex = 0
                        ddlDistName.DataBind()
                        Exit For
                    End If
                Next
            Else
                objCommonFunctions.PopulateDistricts(ddlDistName)
            End If

            dis_code = ddlDistName.SelectedValue
            If userLevel = "012" Or userLevel = "013" Then
                ddlWareHouse.Items.Insert(0, New ListItem(IIf(myLang = "hi-IN", Session("gblBlockNameLocal"), Session("gblBlockName")), Session("gblBlockCode")))
                ddlWareHouse.Visible = True
                lblWareHouse.Visible = True
                lblDistName.Visible = True
                ddlDistName.Visible = True
                btnMap.Visible = True
                ddlWareHouse.SelectedIndex = 0
                MyTable = objCommonFunctions.GetEVMDataTable(dis_code, rbUnitType.SelectedValue, "XX", ddlWareHouse.SelectedValue)
            ElseIf userLevel = "000" Or userLevel = "001" Or userLevel = "006" Then
                ddlWareHouse.Visible = True
                lblWareHouse.Visible = True
                lblDistName.Visible = True
                ddlDistName.Visible = True
                btnMap.Visible = True
                If ddlDistName.Items.Count > 1 Then
                    existing_grid.DataSource = New DataTable
                    existing_grid.DataBind()
                    existing_grid.Visible = False
                    lbl_error.Visible = False
                    btnExport.Visible = False
                    btnSrId.Visible = False
                    view_button.Visible = False
                    view_on_map.Visible = False
                    btnMap.Visible = False
                    Session.Remove("mapData1")
                    Exit Sub
                End If
                objCommonFunctions.PopulateWareHouses(ddlWareHouse, dis_code, rbUnitType.SelectedValue)
                ddlWareHouse.SelectedIndex = 0
                If ddlWareHouse.Items.Count > 1 Then
                    existing_grid.DataSource = New DataTable
                    existing_grid.DataBind()
                    existing_grid.Visible = False
                    lbl_error.Visible = False
                    btnExport.Visible = False
                    btnSrId.Visible = False
                    view_button.Visible = False
                    view_on_map.Visible = False
                    btnMap.Visible = False
                    Session.Remove("mapData1")
                    Exit Sub
                Else
                    MyTable = objCommonFunctions.GetEVMDataTable(dis_code, rbUnitType.SelectedValue, "XX", ddlWareHouse.SelectedValue)
                End If
            Else
                ddlWareHouse.Visible = False
                lblWareHouse.Visible = False
                lblDistName.Visible = False
                ddlDistName.Visible = False
                btnMap.Visible = False
                ddlWareHouse.Items.Insert(0, New ListItem("All", "0000"))
                ddlWareHouse.SelectedIndex = 0
                MyTable = objCommonFunctions.GetEVMBlockDataTable(blk_code, rbUnitType.SelectedValue, "XX")
            End If

            If MyTable.Rows.Count > 0 Then
                existing_grid.DataSource = MyTable
                existing_grid.DataBind()
                existing_grid.Visible = True
                lbl_error.Visible = False
                btnExport.Visible = True
                btnSrId.Visible = True
                view_button.Visible = True
                view_on_map.Visible = True
                Session("mapData1") = MyTable
            Else
                existing_grid.DataSource = New DataTable
                existing_grid.DataBind()
                existing_grid.Visible = False
                lbl_error.Text = GetLocalResourceObject("errNoRecord")
                lbl_error.Visible = True
                btnExport.Visible = False
                btnSrId.Visible = False
                view_button.Visible = False
                view_on_map.Visible = False
                Session.Remove("mapData1")
            End If
        End If
    End Sub
    'Protected Sub existing_grid_RowCommand(sender As Object, e As GridViewCommandEventArgs)
    '    Dim rowIndex As Int16 = CInt(e.CommandArgument.ToString)
    '    Dim mytable As New DataTable
    '    Dim str As String = ""

    '    Dim ev As String = e.CommandName

    '    Dim lblId As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblID"), Label)
    '    Dim lblut As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblut"), Label)
    '    Dim lblSrs As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblSrs"), Label)
    '    Dim lblsn As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblsn"), Label)
    '    Dim lbllt As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lbllt"), Label)
    '    Dim lblln As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblln"), Label)
    '    Dim make As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("make"), Label)
    '    Dim model As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("model"), Label)
    '    Dim mon As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("mon"), Label)
    '    Dim yr As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("yr"), Label)
    '    Dim dis As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("dis"), Label)
    '    Dim wstatus As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("wstatus"), Label)
    '    Dim QRText As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("QRText"), Label)
    '    If e.CommandName = "viewQR" Then
    '        'load QR from file system
    '        Dim type As String = ".png"
    '        Dim name As String = lblsn.Text.ToString
    '        Dim path As String = Server.MapPath("/appImages/" + (name + type))

    '        Try
    '            Dim fs As FileStream = New FileStream(Replace(path, "/", "\"), FileMode.Open, FileAccess.Read)
    '            Dim br As BinaryReader = New BinaryReader(fs)
    '            Dim fileBytes As Byte() = br.ReadBytes(Convert.ToInt32(fs.Length))
    '            With HttpContext.Current.Response
    '                .ClearContent()
    '                .ClearHeaders()
    '                .ContentType = "application/png"
    '                .AddHeader("Content-Disposition", "inline; filename=" + (name + type).ToString)
    '                .BinaryWrite(fileBytes)
    '                .Flush()
    '                .SuppressContent = True
    '                HttpContext.Current.ApplicationInstance.CompleteRequest()
    '            End With
    '            br.Close()
    '            fs.Dispose()
    '        Catch ex As Exception
    '            Dim exMsg As String = ex.Message
    '        End Try
    '    End If
    'End Sub

    Protected Sub cancel_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_button.Click
        Response.Redirect("printQR.aspx")
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

    Protected Sub rbUnitType_SelectedIndexChanged(sender As Object, e As EventArgs)
        lbl_error.Visible = False
        dis_code = ddlDistName.SelectedValue
        If dis_code = "0000" Then
            Return
        End If
        If userLevel = "012" Or userLevel = "013" Then
            ddlWareHouse.Visible = True
            lblWareHouse.Visible = True
            lblDistName.Visible = True
            ddlDistName.Visible = True
            btnMap.Visible = True
            MyTable = objCommonFunctions.GetEVMDataTable(dis_code, rbUnitType.SelectedValue, "XX", ddlWareHouse.SelectedValue)
        ElseIf userLevel = "000" Or userLevel = "001" Or userLevel = "006" Then
            ddlWareHouse.Visible = True
            lblWareHouse.Visible = True
            lblDistName.Visible = True
            ddlDistName.Visible = True
            btnMap.Visible = True
            MyTable = objCommonFunctions.GetEVMDataTable(dis_code, rbUnitType.SelectedValue, "XX", ddlWareHouse.SelectedValue)
            'ElseIf userLevel = "000" Then
            '    ddlWareHouse.Visible = True
            '    lblWareHouse.Visible = True
            '    lblDistName.Visible = True
            '    ddlDistName.Visible = True
            '    btnMap.Visible = True
            '    'objCommonFunctions.PopulateWareHouses(ddlWareHouse, dis_code)
            '    'ddlWareHouse.SelectedIndex = 0
            '    MyTable = objCommonFunctions.GetEVMDataTable(dis_code, rbUnitType.SelectedValue, "XX", ddlWareHouse.SelectedValue)
        Else
            ddlWareHouse.Visible = False
            lblWareHouse.Visible = False
            lblDistName.Visible = False
            ddlDistName.Visible = False
            btnMap.Visible = False
            MyTable = objCommonFunctions.GetEVMBlockDataTable(blk_code, rbUnitType.SelectedValue, "XX")
        End If
        If MyTable.Rows.Count > 0 Then
            existing_grid.DataSource = MyTable
            existing_grid.DataBind()
            existing_grid.Visible = True
            lbl_error.Visible = False

            btnExport.Visible = True
            btnSrId.Visible = True
            view_button.Visible = True
            view_on_map.Visible = True
            Session("mapData1") = MyTable
        Else
            existing_grid.DataSource = New DataTable
            existing_grid.DataBind()
            existing_grid.Visible = False
            lbl_error.Text = GetLocalResourceObject("errNoRecord")
            lbl_error.Visible = True

            btnExport.Visible = False
            btnSrId.Visible = False
            view_button.Visible = False
            view_on_map.Visible = False
            Session.Remove("mapData1")
        End If
    End Sub

    Protected Sub btnSrId_Click(sender As Object, e As EventArgs)
        Dim strText As New StringBuilder("<table style='font-family:Verdana;font-size:9pt;' border='1'><tr><th>EVM Unit Series-SrNo</th><th>EVM Unit QR ID</th></tr>")
        Dim col As Int16 = 0
        Dim cnt As Int16 = 0

        For Each row As GridViewRow In existing_grid.Rows
            Dim grdCheckBox As CheckBox = DirectCast(row.FindControl("grdCheckBox"), CheckBox)
            Dim lblID As Label = DirectCast(row.FindControl("lblID"), Label)
            Dim lblSrs As Label = DirectCast(row.FindControl("lblSrs"), Label)
            Dim lblsn As Label = DirectCast(row.FindControl("lblsn"), Label)
            Dim lblQR As Label = DirectCast(row.FindControl("QRText"), Label)

            If grdCheckBox.Checked Then
                cnt += 1
                strText.Append("<tr>")
                strText.Append("<td>" + lblSrs.Text.Trim + lblsn.Text.Trim + "</td>")
                strText.Append("<td>" + lblID.Text.Trim + "</td>")
                strText.Append("</tr>")
            End If
        Next
        If cnt > 0 Then
            strText.Append("</table>")
            Session("strText") = strText
            Session("unitType") = rbUnitType.SelectedItem.Text
            Response.Write("<script language=JavaScript>window.open('printForm.aspx' , '_blank' ,'toolbar=no,menubar=no,status=no,location=no,scrollbars=no,resizable=1,fullscreen=yes');</script>")
        Else
            MessageBox(view_button, GetLocalResourceObject("NothingToPrint"))
        End If
    End Sub

    Protected Sub view_on_map_Click(sender As Object, e As EventArgs)
        If existing_grid.Rows.Count > 0 Then
            Session("mapData") = Session("mapData1")

            ScriptManager.RegisterStartupScript(Me, Page.GetType(), "New Window", "window.open('viewOnMap.aspx');", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Page.GetType(), "alert", "alert('" + Resources.Resource.WarehouseLocationNotFound + "');", True)
        End If
    End Sub

    Protected Sub ddlDistName_SelectedIndexChanged(sender As Object, e As EventArgs)
        If ddlDistName.SelectedValue = "00" Or ddlDistName.SelectedValue = "" Then
            ddlWareHouse.DataSource = New DataTable
            ddlWareHouse.DataBind()
            Return
        End If
        objCommonFunctions.PopulateWareHouses(ddlWareHouse, ddlDistName.SelectedValue, rbUnitType.SelectedValue)
        If ddlWareHouse.Items.Count = 0 Then
            'When its 0 then what to do
        ElseIf ddlWareHouse.Items.Count = 1 Then
            ddlWareHouse.SelectedIndex = 0
            MyTable = objCommonFunctions.GetEVMDataTable(ddlDistName.SelectedValue, rbUnitType.SelectedValue, "XX", ddlWareHouse.SelectedValue)
        Else
            ddlWareHouse.SelectedIndex = 0
            existing_grid.DataSource = New DataTable
            existing_grid.DataBind()
            existing_grid.Visible = False

            btnExport.Visible = False
            btnSrId.Visible = False
            view_button.Visible = False
            view_on_map.Visible = False
            Session.Remove("mapData1")
        End If
        If MyTable.Rows.Count > 0 Then
            existing_grid.DataSource = MyTable
            existing_grid.DataBind()
            existing_grid.Visible = True
            lbl_error.Visible = False

            btnExport.Visible = True
            btnSrId.Visible = True
            view_button.Visible = True
            view_on_map.Visible = True
            Session("mapData1") = MyTable
        Else
            existing_grid.DataSource = New DataTable
            existing_grid.DataBind()
            existing_grid.Visible = False
            lbl_error.Text = GetLocalResourceObject("errNoRecord")
            lbl_error.Visible = True

            btnExport.Visible = False
            btnSrId.Visible = False
            view_button.Visible = False
            view_on_map.Visible = False
            Session.Remove("mapData1")
        End If
    End Sub

    'Protected Sub mainCheckbox_CheckedChanged(sender As Object, e As EventArgs)
    '    Dim chk As CheckBox = DirectCast(existing_grid.HeaderRow.FindControl("mainCheckbox"), CheckBox)
    '    For Each gr As GridViewRow In existing_grid.Rows
    '        Dim grdCheckBox As CheckBox = DirectCast(existing_grid.HeaderRow.FindControl("grdCheckBox"), CheckBox)
    '        grdCheckBox.Checked = chk.Checked
    '    Next
    'End Sub

    Private Sub existing_grid_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles existing_grid.RowDataBound
        ''header select all function
        If e.Row.RowType = DataControlRowType.Header Then
            If rbUnitType.SelectedValue = "BB" Then
                e.Row.Cells(3).Visible = False
                e.Row.Cells(5).Visible = False
                e.Row.Cells(4).Text = GetLocalResourceObject("Size")
            Else
                'e.Row.Cells(1).Visible = True
                'e.Row.Cells(3).Visible = True
                'e.Row.Cells(5).Visible = True
                e.Row.Cells(3).Visible = True
                e.Row.Cells(5).Visible = True
                e.Row.Cells(4).Text = GetLocalResourceObject("TemplateFieldResource4.HeaderText")
            End If
        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            If rbUnitType.SelectedValue = "BB" Then
                e.Row.Cells(3).Visible = False
                e.Row.Cells(5).Visible = False
            Else
                e.Row.Cells(3).Visible = True
                e.Row.Cells(5).Visible = True
            End If


            Dim lblwsL As Label = DirectCast(e.Row.FindControl("lblwsL"), Label)
            Dim lblws As Label = DirectCast(e.Row.FindControl("lblws"), Label)

            If myLang = "hi-IN" Then
                lblwsL.Visible = True
                lblws.Visible = False
            Else
                lblwsL.Visible = False
                lblws.Visible = True
            End If
        End If
    End Sub

    Private Sub view_button_Click(sender As Object, e As EventArgs) Handles view_button.Click

        Dim strText As New StringBuilder("<table style='font-family:Verdana;font-size:9pt;'>")
        Dim col As Int16 = 1
        Dim cnt As Int16 = 1

        For Each row As GridViewRow In existing_grid.Rows
            Dim grdCheckBox As CheckBox = DirectCast(row.FindControl("grdCheckBox"), CheckBox)
            Dim lblID As Label = DirectCast(row.FindControl("lblID"), Label)
            Dim lblSrs As Label = DirectCast(row.FindControl("lblSrs"), Label)
            Dim lblsn As Label = DirectCast(row.FindControl("lblsn"), Label)
            Dim lblQR As Label = DirectCast(row.FindControl("QRText"), Label)
            Dim QRTextCode As Byte() = objCommonFunctions.getQRCODE(lblQR.Text)

            If grdCheckBox.Checked Then
                cnt += 1
                If col = 1 Or col + 1 Mod 4 = 0 Then
                    strText.Append("<tr>")
                End If
                strText.Append("<td style='text-align:center;'><img src ='data:image/png;base64," + Convert.ToBase64String(QRTextCode) + "' alt='" + lblQR.Text + "' height='200px' width='200px' />")
                'strText.Append("<td style='text-align:center;'><img src ='data:image/jpeg;base64," + Convert.ToBase64String(QRTextCode) + "' alt='" + lblQR.Text + "' height='200px' width='200px' />")
                strText.Append("<br/>" + lblID.Text.Trim + "</td>")

                If (col >= 4 And col Mod 4 = 0) Then
                    strText.Append("</tr>")
                    col = 1
                Else
                    col += 1
                End If
            End If
        Next

        If cnt > 0 Then
            strText.Append("</table>")
            ' Response.Write(strText.ToString)
            Session("strText") = strText
            Session("unitType") = rbUnitType.SelectedItem.Text
            Response.Write("<script language=JavaScript>window.open('printForm.aspx' , '_blank' ,'toolbar=no,menubar=no,status=no,location=no,scrollbars=no,resizable=1,fullscreen=yes');</script>")
        Else
            MessageBox(view_button, GetLocalResourceObject("NothingToPrint"))
        End If
    End Sub

    Protected Sub ExportExcel(sender As Object, e As EventArgs)
        Dim dt As DataTable = New DataTable("EVM_" & rbUnitType.SelectedValue & "_List")
        For Each cell As TableCell In existing_grid.HeaderRow.Cells
            If cell.Text = "CodeVals" Then
                Exit For
            ElseIf cell.Text = "Column1" Then
                dt.Columns.Add("")
            End If
            dt.Columns.Add(cell.Text)
        Next

        For Each row As GridViewRow In existing_grid.Rows
            dt.Rows.Add()
            For i As Integer = 0 To row.Cells.Count - 2
                If row.Cells(i).Controls.Count > 0 And i > 0 Then
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
            Response.AddHeader("content-disposition", "attachment;filename=" & rbUnitType.SelectedValue & "_Report.xlsx")
            Using MyMemoryStream As MemoryStream = New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub
    Protected Sub btnMap_Click(sender As Object, e As EventArgs)
        If ddlWareHouse.SelectedValue = "00" Or ddlWareHouse.SelectedValue = "" Then
            lbl_error.Visible = True
            lbl_error.Text = GetLocalResourceObject("nowarehouse")
            Exit Sub
        End If
        lbl_error.Visible = False

        MyTable = objCommonFunctions.fillWarehouseMapData(ddlWareHouse.SelectedValue)
        If MyTable.Rows.Count > 0 Then
            Session("mapData") = MyTable
            ScriptManager.RegisterStartupScript(Me, Page.GetType(), "New Window", "window.open('viewOnMap.aspx');", True)

            'rptMarkers.DataSource = MyTable
            'rptMarkers.DataBind()
        Else
            ScriptManager.RegisterStartupScript(Me, Page.GetType(), "alert", "alert('" + Resources.Resource.WarehouseLocationNotFound + "');", True)
        End If
    End Sub
End Class
Imports System.IO
Imports System.Threading.Tasks

Public Class addEVM
    Inherits SetCulture
    Dim objCommonFunctions As New commonFunctions()
    Dim MyTable As New DataTable
    Dim userName, userLevel, dis_code As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        userName = Session("UserID")
        dis_code = Session("gblDistrictCode")
        userLevel = Session("UserLevel")
        If Not IsPostBack Then
            objCommonFunctions.PopulateUnitTypes(rbUnitType, "BC")
            objCommonFunctions.PopulateCommonMasterDropdown(ddlMake, "MAKE")
            objCommonFunctions.PopulateCommonMasterDropdown(ddlModel, "MODEL")
            objCommonFunctions.PopulateCommonMasterDropdown(ddlOperationStatus, "WORKINGSTATUS")
            objCommonFunctions.BindMonthDropdown(ddlMon)
            objCommonFunctions.BindYearDropdown(ddlYr)
            txtUnitID.Text = ""
            txtUnitSeries.Focus()

            If userLevel <> "000" Then 'Ask state user to select district , populate district for other users
                Dim ddl As New DropDownList
                objCommonFunctions.PopulateDistricts(ddl)

                For Each li As ListItem In ddl.Items
                    If li.Value = dis_code Then
                        ddlAvailableAt.Items.Insert(0, li)
                        ddlAvailableAt.SelectedIndex = 0
                        ddlAvailableAt.DataBind()
                        Exit For
                    End If
                Next
            Else
                objCommonFunctions.PopulateDistricts(ddlAvailableAt)
            End If

            MyTable = objCommonFunctions.GetEVMDataTable(dis_code, rbUnitType.SelectedValue)
            If MyTable.Rows.Count > 0 Then
                existing_grid.DataSource = MyTable
                existing_grid.DataBind()
                dvList.Visible = True
            Else
                existing_grid.DataSource = New DataTable
                existing_grid.DataBind()
                dvList.Visible = False
            End If
        End If
    End Sub
    Protected Sub existing_grid_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim rowIndex As Int16 = CInt(e.CommandArgument.ToString)
        Dim mytable As New DataTable
        Dim str As String = ""

        Dim lblId As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblID"), Label)
        Dim lblut As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblut"), Label)
        Dim lblSrs As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblSrs"), Label)
        Dim lblsn As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblsn"), Label)
        Dim lbllt As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lbllt"), Label)
        Dim lblln As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblln"), Label)
        Dim make As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("make"), Label)
        Dim model As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("model"), Label)
        Dim mon As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("mon"), Label)
        Dim yr As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("yr"), Label)
        Dim dis As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("dis"), Label)
        Dim wstatus As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("wstatus"), Label)
        Dim QRText As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("QRText"), Label)
        If e.CommandName = "editWard" Then
            txtUnitID.Text = lblId.Text
            txtUnitSeries.Text = lblSrs.Text
            txtUnitSrNo.Text = lblsn.Text
            rbUnitType.SelectedIndex = rbUnitType.Items.IndexOf(rbUnitType.Items.FindByText(lblut.Text))
            ddlMake.SelectedIndex = ddlMake.Items.IndexOf(ddlMake.Items.FindByValue(make.Text))
            ddlModel.SelectedIndex = ddlModel.Items.IndexOf(ddlModel.Items.FindByValue(model.Text))
            ddlMon.SelectedIndex = ddlMon.Items.IndexOf(ddlMon.Items.FindByText(mon.Text))
            ddlYr.SelectedIndex = ddlYr.Items.IndexOf(ddlYr.Items.FindByValue(yr.Text))
            ddlAvailableAt.SelectedIndex = ddlAvailableAt.Items.IndexOf(ddlAvailableAt.Items.FindByValue(dis.Text))
            ddlOperationStatus.SelectedIndex = ddlOperationStatus.Items.IndexOf(ddlOperationStatus.Items.FindByValue(wstatus.Text))
            txtLat.Text = lbllt.Text
            txtLong.Text = lblln.Text

            str = "$('#" + txtUnitID.ClientID + "').val('" + txtUnitID.Text + "');"
            str = str + "$('#" + txtUnitSeries.ClientID + "').val('" + txtUnitSeries.Text.Trim + "');"
            str = str + "$('#" + txtUnitSrNo.ClientID + "').val('" + txtUnitSrNo.Text.Trim + "');"
            str = str + "$('#" + ddlMake.ClientID + "').val('" + ddlMake.SelectedValue + "');"
            str = str + "$('#" + ddlModel.ClientID + "').val('" + ddlModel.SelectedValue + "');"
            str = str + "$('#" + ddlMon.ClientID + "').val('" + ddlMon.SelectedValue + "');"
            str = str + "$('#" + ddlYr.ClientID + "').val('" + ddlYr.SelectedValue + "');"
            str = str + "$('#" + ddlAvailableAt.ClientID + "').val('" + ddlAvailableAt.SelectedValue + "');"
            str = str + "$('#" + ddlOperationStatus.ClientID + "').val('" + ddlOperationStatus.SelectedValue + "');"
            str = str + "$('#" + txtLat.ClientID + "').val('" + txtLat.Text + "');"
            str = str + "$('#" + txtLong.ClientID + "').val('" + txtLong.Text + "');"

            ScriptManager.RegisterStartupScript(panel1, panel1.GetType, "google", str, True)
        ElseIf e.CommandName = "viewQR" Then
            'load QR from file system
            Dim type As String = ".png"
            Dim name As String = lblsn.Text.ToString
            Dim path As String = Server.MapPath("/appImages/" + (name + type))

            ' generate QR
            generateQRCode(lblsn.Text, QRText.Text, Server.MapPath("/appImages/"))

            Try
                Dim fs As FileStream = New FileStream(Replace(path, "/", "\"), FileMode.Open, FileAccess.Read)
                Dim br As BinaryReader = New BinaryReader(fs)
                Dim fileBytes As Byte() = br.ReadBytes(Convert.ToInt32(fs.Length))
                With HttpContext.Current.Response
                    .ClearContent()
                    .ClearHeaders()
                    .ContentType = "application/png"
                    .AddHeader("Content-Disposition", "inline; filename=" + (name + type).ToString)
                    .BinaryWrite(fileBytes)
                    .Flush()
                    .SuppressContent = True
                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                End With
                br.Close()
                fs.Dispose()
            Catch ex As Exception
                Dim exMsg As String = ex.Message
            End Try
        End If
    End Sub
    Public Sub generateQRCode(ByRef UnitSrNo As String, ByRef QR_Text As String, ByRef path As String)

        'Dim name As String = UnitSrNo
        'Dim type As String = ".png"
        'path = path + (name + type)


        'Dim qrCode As New QRCodeEncoder
        'qrCode.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE
        'qrCode.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L

        'Dim imgBarCode As New System.Web.UI.WebControls.Image()
        'imgBarCode.Height = 120
        'imgBarCode.Width = 120

        'Using bitMap As Bitmap = qrCode.Encode(QR_Text, System.Text.Encoding.UTF8)
        '    bitMap.Save(path)
        '    'Using ms As New MemoryStream()
        '    '    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
        '    '    Dim byteImage As Byte() = ms.ToArray()
        '    '    imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage)
        '    'End Using
        '    'plh_bar_code.Controls.Add(imgBarCode)
        'End Using
    End Sub

    Protected Sub existing_grid_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)

        Dim lblId As Label = DirectCast(existing_grid.Rows(e.RowIndex).FindControl("lblID"), Label)
        Dim lblsn As Label = DirectCast(existing_grid.Rows(e.RowIndex).FindControl("lblsn"), Label)

        Dim MyInterface As ErmsInterface.hbInterface
        Dim MyCommand As New SqlClient.SqlCommand
        Dim implementer As New ErmsBLL
        Try
            MyInterface = implementer
            MyCommand.Parameters.Clear()
            MyCommand.Parameters.AddWithValue("@ID", lblId.Text)
            MyTable = MyInterface.DELETEData(MyCommand, "DeleteEVMMaster")
            If MyTable.TableName = "Success" Then
                If MyTable.Rows.Count > 0 Then
                    MessageBox(existing_grid, GetLocalResourceObject("errNotDeleted"))
                    lbl_error.Text = MyTable.Rows(0)(0).ToString
                    lbl_error.Visible = True
                    lblStatus.Visible = False
                    Exit Sub
                End If
                Dim evmTable As DataTable = objCommonFunctions.GetEVMDataTable(dis_code)
                existing_grid.DataSource = evmTable
                existing_grid.DataBind()

                lblId.Text = ""
                txtUnitSrNo.Text = ""
                ddlMake.SelectedIndex = 0
                ddlModel.SelectedIndex = 0
                ddlMon.SelectedIndex = 0
                ddlYr.SelectedIndex = 0
                ddlAvailableAt.SelectedIndex = 0
                ddlOperationStatus.SelectedIndex = 0
                txtLat.Text = ""
                txtLong.Text = ""

                lblStatus.Visible = True
                lbl_error.Visible = False
                lblStatus.Text = GetLocalResourceObject("msgDeleted")
            Else
                MessageBox(existing_grid, GetLocalResourceObject("errNotDeleted"))
                lbl_error.Text = GetLocalResourceObject("errNotDeleted")
                lbl_error.Visible = True
                lblStatus.Visible = False
            End If
        Catch ex As Exception
            MessageBox(existing_grid, GetLocalResourceObject("errNotDeleted"))
            lbl_error.Text = GetLocalResourceObject("errNotDeleted")
            lbl_error.Visible = True
            lblStatus.Visible = False
        End Try
    End Sub
    Protected Sub save_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save_button.Click
        Dim flag As Boolean = True
        Dim errorStr As String = ""
        Dim Item As New ListItem
        Dim i As Integer = 0

        If String.IsNullOrEmpty(txtUnitID.Text.Trim) Then
            For Each row As GridViewRow In existing_grid.Rows
                If DirectCast(row.FindControl("lblsn"), Label).Text.Trim = txtUnitSrNo.Text.Trim Then
                    errorStr = errorStr + "<li>" + GetLocalResourceObject("errEVMExist") + "</li>"
                    flag = False
                    Exit For
                End If
            Next
        End If

        If txtUnitSrNo.Text.Trim = "" Then
            flag = False
            errorStr = errorStr + "<li>" + GetLocalResourceObject("errSrNoRequired") + "</li>"
        End If

        If ddlMake.SelectedValue = "0" Then
            flag = False
            errorStr = errorStr + "<li>" + GetLocalResourceObject("errNoMake") + "</li>"
        End If
        If ddlModel.SelectedValue = "0" Then
            flag = False
            errorStr = errorStr + "<li>" + GetLocalResourceObject("errNoModel") + "</li>"
        End If
        If ddlMon.SelectedValue = "0" Then
            flag = False
            errorStr = errorStr + "<li>" + GetLocalResourceObject("errNoMonth") + "</li>"
        End If
        If ddlYr.SelectedValue = "0" Then
            flag = False
            errorStr = errorStr + "<li>" + GetLocalResourceObject("errNoYear") + "</li>"
        End If
        If ddlAvailableAt.SelectedValue = "0" Then
            flag = False
            errorStr = errorStr + "<li>" + GetLocalResourceObject("errNoPlace") + "</li>"
        End If
        If flag Then
            Dim mytable As New DataTable
            Dim mycommand As New SqlClient.SqlCommand
            Dim MyInterface As ErmsInterface.hbInterface
            Dim Implementer As New ErmsBLL
            '    'This is to save the new EVM Unit
            MyInterface = Implementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@Series", txtUnitSeries.Text.Trim)
            mycommand.Parameters.AddWithValue("@SrNo", txtUnitSrNo.Text.Trim)
            mycommand.Parameters.AddWithValue("@UnitType", rbUnitType.SelectedValue)
            mycommand.Parameters.AddWithValue("@Make", ddlMake.SelectedValue)
            mycommand.Parameters.AddWithValue("@Model", ddlModel.SelectedValue)
            mycommand.Parameters.AddWithValue("@MonYrManufacturing", Right("00" + ddlMon.SelectedValue, 2) & "-" & ddlYr.SelectedValue)
            mycommand.Parameters.AddWithValue("@WorkingStatus", ddlOperationStatus.SelectedValue)
            mycommand.Parameters.AddWithValue("@LocatedAt", ddlAvailableAt.SelectedValue)
            mycommand.Parameters.AddWithValue("@Latitude", txtLat.Text.Trim)
            mycommand.Parameters.AddWithValue("@Longitude", txtLong.Text.Trim)
            mycommand.Parameters.AddWithValue("@EntryBy", userName)
            If txtUnitID.Text.Trim <> "" Then
                mycommand.Parameters.AddWithValue("@ID", txtUnitID.Text.Trim)
            End If
            Try
                mytable = MyInterface.INSERTData(mycommand, "saveEVMMaster")
                If mytable.Rows.Count > 0 Then
                    lbl_error.Text = mytable.Rows(0)(0).ToString
                    lbl_error.Visible = True
                    lblStatus.Visible = False
                    Exit Sub
                End If
            Catch ex As Exception
                lbl_error.Text = ex.Message
                lbl_error.Visible = True
                lblStatus.Visible = False
                Exit Sub
            End Try

            Dim evmTable As DataTable = objCommonFunctions.GetEVMDataTable(dis_code)
            If evmTable.Rows.Count > 0 Then
                existing_grid.DataSource = evmTable
                existing_grid.DataBind()
                dvList.Visible = True
            Else
                existing_grid.DataSource = New DataTable
                existing_grid.DataBind()
                dvList.Visible = False
            End If

            Parallel.Invoke(Sub()
                                lblStatus.Visible = True
                                lbl_error.Visible = False
                                If txtUnitID.Text.Trim <> "" Then
                                    lblStatus.Text = GetLocalResourceObject("msgUpdate").ToString
                                Else
                                    lblStatus.Text = GetLocalResourceObject("msgSave").ToString
                                End If
                            End Sub,
                            Sub()
                                Dim QR_Text As String = ddlAvailableAt.SelectedValue & "|" & txtUnitSrNo.Text.Trim & "|" & ddlMake.SelectedValue & "|" & ddlModel.SelectedValue & "|" & ddlMon.SelectedValue & "|" & ddlYr.SelectedValue
                                generateQRCode(txtUnitSrNo.Text.Trim, QR_Text, Server.MapPath("/appImages/"))
                                ' generateQRCode(txtUnitSrNo.Text, ddlAvailableAt.SelectedValue, ddlMake.SelectedValue, ddlModel.SelectedValue, ddlMon.SelectedValue, ddlYr.SelectedValue)
                            End Sub
                )

            txtUnitSeries.Text = ""
            txtUnitID.Text = ""
            txtUnitSrNo.Text = ""
            ddlMake.SelectedIndex = 0
            ddlModel.SelectedIndex = 0
            ddlMon.SelectedIndex = 0
            ddlYr.SelectedIndex = 0
            ddlAvailableAt.SelectedIndex = 0
            ddlOperationStatus.SelectedIndex = 0
            txtLat.Text = ""
            txtLong.Text = ""

        Else
            lbl_error.Text = errorStr
            lbl_error.Visible = True
            lblStatus.Visible = False
        End If
    End Sub
    Protected Sub cancel_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_button.Click
        Response.Redirect("addEVM.aspx")
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
        MyTable = objCommonFunctions.GetEVMDataTable(dis_code, rbUnitType.SelectedValue)
        If MyTable.Rows.Count > 0 Then
            existing_grid.DataSource = MyTable
            existing_grid.DataBind()
            dvList.Visible = True
        Else
            existing_grid.DataSource = New DataTable
            existing_grid.DataBind()
            dvList.Visible = False
        End If
    End Sub

    Private Sub existing_grid_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles existing_grid.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblut As Label = DirectCast(e.Row.FindControl("lblut"), Label)
            Dim lblmk As Label = DirectCast(e.Row.FindControl("lblmk"), Label)
            Dim lblmd As Label = DirectCast(e.Row.FindControl("lblmd"), Label)
            Dim lblmy As Label = DirectCast(e.Row.FindControl("lblmy"), Label)
            Dim lbllc As Label = DirectCast(e.Row.FindControl("lbllc"), Label)
            Dim lblws As Label = DirectCast(e.Row.FindControl("lblws"), Label)
            Dim mon As Label = DirectCast(e.Row.FindControl("mon"), Label)
            Dim yr As Label = DirectCast(e.Row.FindControl("yr"), Label)

            lblut.Text = rbUnitType.Items.FindByValue(lblut.Text).Text
            lblmk.Text = ddlMake.Items.FindByValue(lblmk.Text).Text
            lblmd.Text = ddlModel.Items.FindByValue(lblmd.Text).Text
            lblmy.Text = ddlMon.Items.FindByValue(CInt(lblmy.Text.Substring(0, 2))).Text & lblmy.Text.Substring(2)
            lbllc.Text = ddlAvailableAt.Items.FindByValue(lbllc.Text).Text
            lblws.Text = ddlOperationStatus.Items.FindByValue(lblws.Text).Text
            mon.Text = ddlMon.Items.FindByValue(mon.Text).Text
            yr.Text = ddlYr.Items.FindByValue(yr.Text).Text
        End If
    End Sub

    Private Sub view_button_Click(sender As Object, e As EventArgs) Handles view_button.Click
        If existing_grid.Rows.Count > 0 Then
            Dim strText As New StringBuilder("<table cellspacing='10'>")
            Dim col As Int16 = 0
            For Each row As GridViewRow In existing_grid.Rows
                Dim lblsn As Label = DirectCast(row.FindControl("lblsn"), Label)

                If col Mod 5 = 0 Then
                    strText.Append("<tr>")
                End If
                strText.Append("<td style='text-align:center;'><img src='" + "appImages/" + lblsn.Text + ".png" + "' height='85%' width='85%' alt='QR Image' /><br/>")
                strText.Append(lblsn.Text + "</td>")

                If row.RowIndex = existing_grid.Rows.Count - 1 Or (col > 0 And col Mod 5 = 0) Then
                    strText.Append("</tr>")
                    col = 0
                Else
                    col += 1
                End If
            Next
            strText.Append("</table>")
            ' Response.Write(strText.ToString)
            Session("strText") = strText
            Session("unitType") = rbUnitType.SelectedItem.Text
            Response.Write("<script language=JavaScript>window.open('printForm.aspx' , '_blank' ,'toolbar=no,menubar=no,status=no,location=no,scrollbars=no,resizable=1,fullscreen=yes');</script>")
        Else
            MessageBox(view_button, GetLocalResourceObject("NothingToPrint"))
        End If
    End Sub

End Class
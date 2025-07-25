Public Class addFLC
    Inherits SetCulture

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' RangeValidatorDt1.MinimumValue = Convert.ToDateTime(Date.Today.AddDays(-14), System.Globalization.CultureInfo.GetCultureInfo("en-US").DateTimeFormat)
        RangeValidatorDt1.MaximumValue = Convert.ToDateTime(Date.Today, System.Globalization.CultureInfo.GetCultureInfo("en-US").DateTimeFormat)

    End Sub
    Protected Sub existing_grid_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim rowIndex As Int16 = CInt(e.CommandArgument.ToString)
        'Dim mytable As New DataTable
        'Dim str As String = ""

        'Dim lblId As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblID"), Label)
        'Dim lblut As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblut"), Label)
        'Dim lblSrs As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblSrs"), Label)
        'Dim lblsn As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblsn"), Label)
        'Dim lbllt As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lbllt"), Label)
        'Dim lblln As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblln"), Label)
        'Dim make As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("make"), Label)
        'Dim model As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("model"), Label)
        'Dim mon As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("mon"), Label)
        'Dim yr As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("yr"), Label)
        'Dim dis As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("dis"), Label)
        'Dim wstatus As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("wstatus"), Label)
        'Dim QRText As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("QRText"), Label)
        'If e.CommandName = "editWard" Then
        '    txtUnitID.Text = lblId.Text
        '    txtUnitSeries.Text = lblSrs.Text
        '    txtUnitSrNo.Text = lblsn.Text
        '    rbUnitType.SelectedIndex = rbUnitType.Items.IndexOf(rbUnitType.Items.FindByText(lblut.Text))
        '    ddlMake.SelectedIndex = ddlMake.Items.IndexOf(ddlMake.Items.FindByValue(make.Text))
        '    ddlModel.SelectedIndex = ddlModel.Items.IndexOf(ddlModel.Items.FindByValue(model.Text))
        '    ddlMon.SelectedIndex = ddlMon.Items.IndexOf(ddlMon.Items.FindByText(mon.Text))
        '    ddlYr.SelectedIndex = ddlYr.Items.IndexOf(ddlYr.Items.FindByValue(yr.Text))
        '    ddlAvailableAt.SelectedIndex = ddlAvailableAt.Items.IndexOf(ddlAvailableAt.Items.FindByValue(dis.Text))
        '    ddlOperationStatus.SelectedIndex = ddlOperationStatus.Items.IndexOf(ddlOperationStatus.Items.FindByValue(wstatus.Text))
        '    txtLat.Text = lbllt.Text
        '    txtLong.Text = lblln.Text

        '    str = "$('#" + txtUnitID.ClientID + "').val('" + txtUnitID.Text + "');"
        '    str = str + "$('#" + txtUnitSeries.ClientID + "').val('" + txtUnitSeries.Text.Trim + "');"
        '    str = str + "$('#" + txtUnitSrNo.ClientID + "').val('" + txtUnitSrNo.Text.Trim + "');"
        '    str = str + "$('#" + ddlMake.ClientID + "').val('" + ddlMake.SelectedValue + "');"
        '    str = str + "$('#" + ddlModel.ClientID + "').val('" + ddlModel.SelectedValue + "');"
        '    str = str + "$('#" + ddlMon.ClientID + "').val('" + ddlMon.SelectedValue + "');"
        '    str = str + "$('#" + ddlYr.ClientID + "').val('" + ddlYr.SelectedValue + "');"
        '    str = str + "$('#" + ddlAvailableAt.ClientID + "').val('" + ddlAvailableAt.SelectedValue + "');"
        '    str = str + "$('#" + ddlOperationStatus.ClientID + "').val('" + ddlOperationStatus.SelectedValue + "');"
        '    str = str + "$('#" + txtLat.ClientID + "').val('" + txtLat.Text + "');"
        '    str = str + "$('#" + txtLong.ClientID + "').val('" + txtLong.Text + "');"

        '    ScriptManager.RegisterStartupScript(panel1, panel1.GetType, "google", str, True)

        'End If
    End Sub
    Protected Sub existing_grid_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)

        'Dim lblId As Label = DirectCast(existing_grid.Rows(e.RowIndex).FindControl("lblID"), Label)
        'Dim lblsn As Label = DirectCast(existing_grid.Rows(e.RowIndex).FindControl("lblsn"), Label)

        'Dim MyInterface As ErmsInterface.hbInterface
        'Dim MyCommand As New SqlClient.SqlCommand
        'Dim implementer As New ErmsBLL
        'Try
        '    MyInterface = implementer
        '    MyCommand.Parameters.Clear()
        '    MyCommand.Parameters.AddWithValue("@ID", lblId.Text)
        '    MyTable = MyInterface.DELETEData(MyCommand, "DeleteEVMMaster")
        '    If MyTable.TableName = "Success" Then
        '        If MyTable.Rows.Count > 0 Then
        '            MessageBox(existing_grid, GetLocalResourceObject("errNotDeleted"))
        '            lbl_error.Text = MyTable.Rows(0)(0).ToString
        '            lbl_error.Visible = True
        '            lblStatus.Visible = False
        '            Exit Sub
        '        End If
        '        Dim evmTable As DataTable = objCommonFunctions.GetEVMDataTable(dis_code)
        '        existing_grid.DataSource = evmTable
        '        existing_grid.DataBind()

        '        lblId.Text = ""
        '        txtUnitSrNo.Text = ""
        '        ddlMake.SelectedIndex = 0
        '        ddlModel.SelectedIndex = 0
        '        ddlMon.SelectedIndex = 0
        '        ddlYr.SelectedIndex = 0
        '        ddlAvailableAt.SelectedIndex = 0
        '        ddlOperationStatus.SelectedIndex = 0
        '        txtLat.Text = ""
        '        txtLong.Text = ""

        '        lblStatus.Visible = True
        '        lbl_error.Visible = False
        '        lblStatus.Text = GetLocalResourceObject("msgDeleted")
        '    Else
        '        MessageBox(existing_grid, GetLocalResourceObject("errNotDeleted"))
        '        lbl_error.Text = GetLocalResourceObject("errNotDeleted")
        '        lbl_error.Visible = True
        '        lblStatus.Visible = False
        '    End If
        'Catch ex As Exception
        '    MessageBox(existing_grid, GetLocalResourceObject("errNotDeleted"))
        '    lbl_error.Text = GetLocalResourceObject("errNotDeleted")
        '    lbl_error.Visible = True
        '    lblStatus.Visible = False
        'End Try
    End Sub

End Class
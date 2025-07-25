Public Class addBallotBox
    Inherits System.Web.UI.Page
    Dim objCommonFunctions As New commonFunctions()
    Dim MyTable As New Data.DataTable
    Dim userName, userLevel, dis_code As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        userName = Session("UserID")
        dis_code = Session("gblDistrictCode")
        userLevel = Session("UserLevel")

        If Not IsPostBack Then
            objCommonFunctions.PopulateCommonMasterDropdown(ddlModel, "BALLOTBOXSIZE")
            objCommonFunctions.BindMonthDropdown(ddlMon)
            objCommonFunctions.BindYearDropdown(ddlYr)
            If userLevel <> "000" Then 'Ask state user to select district , populate district for other users
                Dim ddl As New DropDownList
                objCommonFunctions.PopulateDistricts(ddl)

                For Each li As ListItem In ddl.Items
                    If li.Value = dis_code Then
                        ddlAvailableAt.Items.Insert(0, li)
                        ddlAvailableAt.SelectedIndex = 0
                        ddlAvailableAt.DataBind()
                        objCommonFunctions.PopulateWareHouses(ddlWarehouseAt, ddlAvailableAt.SelectedValue, "BB")
                        ddlWarehouseAt.SelectedIndex = 0
                        Exit For
                    End If
                Next
            Else
                objCommonFunctions.PopulateDistricts(ddlAvailableAt)
            End If

            txtUnitID.Text = ""
            txtUnitCounts.Focus()
            MyTable = objCommonFunctions.GetBallotBoxDataTable(dis_code, ddlWarehouseAt.SelectedValue)
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
        Dim mytable As New Data.DataTable
        Dim str As String = ""

        Dim BBSize As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("BBSize"), Label)
        Dim BBCount As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("BBCount"), Label)
        Dim mon As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("MonYrPurchase"), Label)
        Dim yr As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("MonYrPurchase"), Label)
        Dim dis As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lbllc"), Label)
        Dim wstatus As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("wstatus"), Label)
        ddlModel.SelectedValue = BBSize.Text
        txtUnitCounts.Text = BBCount.Text
        ddlAvailableAt.SelectedIndex = ddlAvailableAt.Items.IndexOf(ddlAvailableAt.Items.FindByValue(dis.Text))
        ddlMon.SelectedValue = CInt(mon.Text.Substring(0, 2))
        ddlYr.SelectedValue = CInt(yr.Text.Substring(3))

        str = "$('#" + txtUnitID.ClientID + "').val('" + txtUnitID.Text + "');"
        str = str + "$('#" + txtUnitCounts.ClientID + "').val('" + txtUnitCounts.Text.Trim + "');"
        str = str + "$('#" + ddlMon.ClientID + "').val('" + ddlMon.SelectedValue + "');"
        str = str + "$('#" + ddlYr.ClientID + "').val('" + ddlYr.SelectedValue + "');"
        str = str + "$('#" + ddlAvailableAt.ClientID + "').val('" + ddlAvailableAt.SelectedValue + "');"


        ScriptManager.RegisterStartupScript(panel1, panel1.GetType, "google", str, True)
    End Sub


    Protected Sub save_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save_button.Click
        Dim flag As Boolean = True
        Dim errorStr As String = ""
        Dim Item As New ListItem
        Dim i As Integer = 0

        If txtUnitCounts.Text.Trim = "" Then
            flag = False
            errorStr = errorStr + "<li>" + GetLocalResourceObject("errTotalRequired") + "</li>"
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
            Dim mytable As New Data.DataTable
            Dim mycommand As New Data.SqlClient.SqlCommand
            Dim MyInterface As ErmsInterface.hbInterface
            Dim Implementer As New ErmsBLL
            '    'This is to save the new EVM Unit
            MyInterface = Implementer
            mycommand.Parameters.Clear()
            '@BBCount int,@BBSize char,@MonYr char(7),@WorkingStatus varchar(10),@LocatedAt char(4),@wareHouseCode char(4),@Latitude numeric(12,9),
            '@Longitude numeric(12,9),@EntryBy varchar(30)

            mycommand.Parameters.AddWithValue("@BBCount", txtUnitCounts.Text.Trim)
            mycommand.Parameters.AddWithValue("@BBSize", ddlModel.SelectedValue)
            mycommand.Parameters.AddWithValue("@MonYr", Right("00" + ddlMon.SelectedValue, 2) & "-" & ddlYr.SelectedValue)
            mycommand.Parameters.AddWithValue("@WorkingStatus", "OK")
            mycommand.Parameters.AddWithValue("@LocatedAt", ddlAvailableAt.SelectedValue)
            mycommand.Parameters.AddWithValue("@wareHouseCode", ddlWarehouseAt.SelectedValue)
            mycommand.Parameters.AddWithValue("@EntryBy", userName)
            Try
                mytable = MyInterface.INSERTData(mycommand, "saveBallotMaster")
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

            Dim evmTable As DataTable = objCommonFunctions.GetBallotBoxDataTable(dis_code, ddlWarehouseAt.SelectedValue)
            If evmTable.Rows.Count > 0 Then
                existing_grid.DataSource = evmTable
                existing_grid.DataBind()
                dvList.Visible = True
            Else
                existing_grid.DataSource = New DataTable
                existing_grid.DataBind()
                dvList.Visible = False
            End If

            txtUnitID.Text = ""
            txtUnitCounts.Text = ""
            ddlModel.SelectedIndex = 0
            ddlMon.SelectedIndex = 0
            ddlYr.SelectedIndex = 0
            ddlAvailableAt.SelectedIndex = 0


            lblStatus.Visible = True
            lbl_error.Visible = False
            If txtUnitID.Text.Trim <> "" Then
                lblStatus.Text = GetLocalResourceObject("msgUpdate").ToString
            Else
                lblStatus.Text = GetLocalResourceObject("msgSave").ToString
            End If
            txtUnitID.Text = ""
        Else
            lbl_error.Text = errorStr
            lbl_error.Visible = True
            lblStatus.Visible = False
        End If
    End Sub
    Protected Sub cancel_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_button.Click
        Response.Redirect("addBallotBox.aspx")
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

    Private Sub existing_grid_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles existing_grid.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim BBSize As Label = DirectCast(e.Row.FindControl("BBSize"), Label)
            Dim BBCount As Label = DirectCast(e.Row.FindControl("BBCount"), Label)
            Dim MonYrPurchase As Label = DirectCast(e.Row.FindControl("MonYrPurchase"), Label)
            Dim lbllc As Label = DirectCast(e.Row.FindControl("lbllc"), Label)
            Dim lblwh As Label = DirectCast(e.Row.FindControl("lblwh"), Label)
            ddlWarehouseAt.SelectedValue = lblwh.Text
            BBSize.Text = ddlModel.Items.FindByValue(BBSize.Text).Text
            lbllc.Text = ddlAvailableAt.Items.FindByValue(lbllc.Text).Text
            lblwh.Text = "[" & ddlWarehouseAt.Items.FindByValue(lblwh.Text).Text & "]"
            ddlModel.SelectedIndex = 0

            txtUnitCounts.Text = ""

        End If
    End Sub

    Private Sub ddlAvailableAt_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAvailableAt.SelectedIndexChanged
        If ddlAvailableAt.SelectedValue = "00" Then
            ddlWarehouseAt.DataSource = New DataTable
            ddlWarehouseAt.DataBind()
        Else
            objCommonFunctions.PopulateWareHouses(ddlWarehouseAt, ddlAvailableAt.SelectedValue, "BB")
            ddlWarehouseAt.SelectedIndex = 0
        End If

    End Sub
End Class
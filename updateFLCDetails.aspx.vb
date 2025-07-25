Imports System.IO
Imports System.Web.Script.Serialization
Imports ClosedXML.Excel

Public Class updateFLCDetails
    Inherits SetCulture
    Dim objCommonFunctions As New commonFunctions()
    Dim MyTable As New DataTable
    Dim userName, userLevel, dis_code, dis_name, wh_code, myLang, unit_type As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        userName = Session("UserID")
        dis_code = Session("gblDistrictCode")
        dis_name = Session("gblDistrictName")
        userLevel = Session("UserLevel")
        unit_type = "AL"
        If userLevel = "012" Or userLevel = "013" Then
            wh_code = Session("gblBlockCode")
            If wh_code.StartsWith("1") Then
                unit_type = "BB"
            ElseIf wh_code.StartsWith("0") Then
                unit_type = "BC"
            End If
        Else
            wh_code = "0000"
        End If
        Response.Cache.SetLastModified(DateTime.Now)
        Response.Cache.SetAllowResponseInBrowserHistory(False)
        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        Response.Cache.SetNoStore()
        myLang = Session("MyLanguage").ToString

        If Not IsPostBack Then
            objCommonFunctions.populateFLCData(ddlFLC, dis_code)
            objCommonFunctions.PopulateUnitTypes(rbUnitType, unit_type)
        End If

    End Sub
    Protected Sub ddlFLC_SelectedIndexChanged(sender As Object, e As EventArgs)
        If ddlFLC.SelectedValue = "" Or ddlFLC.SelectedValue = "0" Then
            existing_grid.DataSource = New DataTable
            existing_grid.DataBind()
            btnExport.Visible = False
        Else
            MyTable = objCommonFunctions.GetEVM_FLCDataTable(dis_code, rbUnitType.SelectedValue, ddlFLC.SelectedValue)
            existing_grid.DataSource = MyTable
            existing_grid.DataBind()
            btnExport.Visible = True
        End If
    End Sub

    Protected Sub rbUnitType_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddlFLC_SelectedIndexChanged(sender, e)
    End Sub

    Protected Sub existing_grid_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim rowIndex As Int16 = CInt(e.CommandArgument.ToString)
        Dim mytable As New DataTable
        Dim str As String = ""

        Dim lblId As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblID"), Label)
        Dim lblut As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblut"), Label)
        Dim lblSrs As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblSrs"), Label)
        Dim lblsn As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblsn"), Label)
        Dim lblmk As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblmk"), Label)
        Dim lblmd As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblmd"), Label)
        Dim lblwsL As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblwsL"), Label)
        Dim lblws As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblws"), Label)
        Dim lblRemark As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblRemark"), Label)
        Dim lblPinkSeal As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblPinkSeal"), Label)
        Dim UsedForTraining As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("UsedForTraining"), Label)
        Dim lblWsCode As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblwsCode"), Label)
        If e.CommandName = "edit" Then
            '  rbUnitType.SelectedIndex = rbUnitType.Items.IndexOf(rbUnitType.Items.FindByText(lblut.Text))

            txtUnitID.Text = lblId.Text
            txtUnitSeries.Text = lblSrs.Text
            txtUnitSrNo.Text = lblsn.Text
            objCommonFunctions.PopulateCommonMasterDropdown(ddlOperationStatus, "WORKINGSTATUS")
            If String.IsNullOrEmpty(lblWsCode.Text) Then
                ddlOperationStatus.SelectedIndex = 0
            Else
                ddlOperationStatus.SelectedValue = lblWsCode.Text
            End If

            ' txtRemark.Text = lblRemark.Text
            If String.IsNullOrEmpty(lblPinkSeal.Text) Then
                txtRemark.Text = lblRemark.Text
            Else
                txtRemark.Text = lblPinkSeal.Text
            End If

            If ddlForTraining.Items.Count <= 0 Then
                objCommonFunctions.BindYesNoRadioButton(ddlForTraining)
            End If
            ddlForTraining.SelectedIndex = ddlForTraining.Items.IndexOf(ddlForTraining.Items.FindByValue(IIf(UsedForTraining.Text = "", "N", UsedForTraining.Text)))
            dvEVM.Visible = True
            str = "$('#" + txtUnitID.ClientID + "').val('" + txtUnitID.Text + "');"
            str = str + "$('#" + txtUnitSeries.ClientID + "').val('" + txtUnitSeries.Text.Trim + "');"
            str = str + "$('#" + txtUnitSrNo.ClientID + "').val('" + txtUnitSrNo.Text.Trim + "');"
            str = str + "$('#" + txtRemark.ClientID + "').val('" + txtRemark.Text + "');"
            str = str + "$('#" + ddlOperationStatus.ClientID + "').val('" + ddlOperationStatus.Text + "');"
            str = str + "$('#" + ddlForTraining.ClientID + "').val('" + ddlForTraining.Text + "');"

            ScriptManager.RegisterStartupScript(panel1, panel1.GetType, "google", str, True)
        End If
    End Sub

    Protected Sub existing_grid_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)

        Dim lblId As Label = DirectCast(existing_grid.Rows(e.RowIndex).FindControl("lblID"), Label)

        Dim MyInterface As ErmsInterface.hbInterface
        Dim MyCommand As New SqlClient.SqlCommand
        Dim implementer As New ErmsBLL
        Try
            MyInterface = implementer
            MyCommand.Parameters.Clear()
            MyCommand.Parameters.AddWithValue("@unitType", rbUnitType.SelectedValue)
            MyCommand.Parameters.AddWithValue("@unitId", lblId.Text)
            MyCommand.Parameters.AddWithValue("@flcID", ddlFLC.SelectedValue)
            MyTable = MyInterface.DELETEData(MyCommand, "DeleteFLCDetails")
            If MyTable.TableName = "Success" Then
                If MyTable.Rows.Count > 0 Then
                    MessageBox(existing_grid, GetLocalResourceObject("errNotDeleted"))
                    lbl_error.Text = MyTable.Rows(0)(0).ToString
                    lbl_error.Visible = True
                    lblStatus.Visible = False
                    Exit Sub
                End If
                Dim dtTable As DataTable = objCommonFunctions.GetEVM_FLCDataTable(dis_code, rbUnitType.SelectedValue, ddlFLC.SelectedValue)
                existing_grid.DataSource = dtTable
                existing_grid.DataBind()
                If dtTable.Rows.Count > 0 Then
                    btnExport.Visible = True
                Else
                    btnExport.Visible = False
                End If

                txtUnitID.Text = ""
                txtUnitSrNo.Text = ""
                If ddlOperationStatus.Items.Count > 0 Then
                    ddlOperationStatus.SelectedIndex = 0
                End If
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


        If ddlOperationStatus.SelectedValue = "OK" And ddlForTraining.SelectedValue = "N" And String.IsNullOrEmpty(txtRemark.Text) Then
            flag = False
            errorStr = errorStr + "<li>" + GetLocalResourceObject("errNoPinkSeal") + "</li>"
        ElseIf ddlOperationStatus.SelectedValue = "FM" And String.IsNullOrEmpty(txtRemark.Text) Then
            flag = False
            errorStr = errorStr + "<li>" + GetLocalResourceObject("errNoRemark") + "</li>"
        End If
        If flag Then
            Dim mytable As New DataTable
            Dim mycommand As New SqlClient.SqlCommand
            Dim MyInterface As ErmsInterface.hbInterface
            Dim Implementer As New ErmsBLL
            '@unitType varchar(5),	@unitId varchar(20),	@ws varchar(5),	@remark nvarchar(100),
            '@usedTraining char,	@flcID int,	@user varchar(30)
            MyInterface = Implementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@unitId", txtUnitID.Text.Trim)
            mycommand.Parameters.AddWithValue("@UnitType", rbUnitType.SelectedValue)
            mycommand.Parameters.AddWithValue("@ws", ddlOperationStatus.SelectedValue)
            mycommand.Parameters.AddWithValue("@remark", txtRemark.Text.Trim)
            mycommand.Parameters.AddWithValue("@usedTraining", ddlForTraining.SelectedValue)
            mycommand.Parameters.AddWithValue("@flcID", ddlFLC.SelectedValue)
            mycommand.Parameters.AddWithValue("@user", userName)
            Try
                mytable = MyInterface.UPDATEData(mycommand, "updateFLCDetails")
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

            Dim evmTable As DataTable = objCommonFunctions.GetEVM_FLCDataTable(dis_code, rbUnitType.SelectedValue, ddlFLC.SelectedValue)

            existing_grid.DataSource = evmTable
            existing_grid.DataBind()
            If evmTable.Rows.Count > 0 Then
                btnExport.Visible = True
            Else
                btnExport.Visible = False
            End If
            dvEVM.Visible = False
            lblStatus.Visible = True
            lbl_error.Visible = False
            If txtUnitID.Text.Trim <> "" Then
                lblStatus.Text = GetLocalResourceObject("msgUpdate").ToString
            Else
                lblStatus.Text = GetLocalResourceObject("msgSave").ToString
            End If

            txtUnitSeries.Text = ""
            txtUnitID.Text = ""
            txtUnitSrNo.Text = ""
            txtRemark.Text = ""
            ddlOperationStatus.SelectedIndex = 0
            ddlForTraining.SelectedIndex = 0

        Else
            lbl_error.Text = errorStr
            lbl_error.Visible = True
            lblStatus.Visible = False
        End If
    End Sub
    Protected Sub cancel_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_button.Click
        Response.Redirect("updateFLCDetails.aspx")
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

    Private Sub existing_grid_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles existing_grid.RowEditing
        dvEVM.Visible = True
    End Sub

    Protected Sub ExportExcel(sender As Object, e As EventArgs)
        Dim dt As DataTable = New DataTable("FLC_" & dis_name & "_Report")
        Dim cntr As Int16 = 0
        For Each cell As TableCell In existing_grid.HeaderRow.Cells
            If cntr = 4 Then
                Exit For
            End If
            dt.Columns.Add(cell.Text)
        Next

        For Each row As GridViewRow In existing_grid.Rows
            dt.Rows.Add()
            For i As Integer = 0 To row.Cells.Count - 2
                dt.Rows(dt.Rows.Count - 1)(i) = (TryCast(row.Cells(i).Controls(1), Label)).Text
            Next
        Next

        Using wb As XLWorkbook = New XLWorkbook()
            wb.Worksheets.Add(dt)
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=FLC_" & dis_name & "_Report.xlsx")
            Using MyMemoryStream As MemoryStream = New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub

    Private Sub existing_grid_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles existing_grid.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim lblID As Label = DirectCast(e.Row.FindControl("lblID"), Label)
            Dim lblwsL As Label = DirectCast(e.Row.FindControl("lblwsL"), Label)
            Dim lblws As Label = DirectCast(e.Row.FindControl("lblws"), Label)
            Dim lblRemark As Label = DirectCast(e.Row.FindControl("lblRemark"), Label)
            Dim lblPinkSeal As Label = DirectCast(e.Row.FindControl("lblPinkSeal"), Label)
            If String.IsNullOrEmpty(lblPinkSeal.Text) Then
                lblPinkSeal.Visible = False
                lblRemark.Visible = True
            Else
                lblPinkSeal.Visible = True
                lblRemark.Visible = False
            End If
            If Session("MyLanguage").ToString() = "hi-IN" Then
                lblwsL.Visible = True
                lblws.Visible = False
            Else
                lblwsL.Visible = False
                lblws.Visible = True
            End If

        End If
    End Sub
End Class
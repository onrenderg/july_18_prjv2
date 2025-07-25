Imports System.IO
Imports ClosedXML.Excel
Imports SEC_InventoryMgmt.commonFunctions

Public Class listFLC
    Inherits SetCulture
    Dim objCommonFunctions As New commonFunctions()
    Dim MyTable As New DataTable
    Dim userName, userLevel, dis_code, dis_name, myLang As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        userName = Session("UserID")
        dis_code = Session("gblDistrictCode")
        dis_name = Session("gblDistrictName")
        userLevel = Session("UserLevel")
        myLang = Session("MyLanguage").ToString
        If Not IsPostBack Then

            MyTable = objCommonFunctions.getFLCDataTable(dis_code)
            If MyTable.Rows.Count > 0 Then
                existing_grid.DataSource = MyTable
                existing_grid.DataBind()
                dvList.Visible = True
                existing_grid.Visible = True
                lbl_error.Visible = False
                btnExport.Visible = True

            Else
                dvList.Visible = False
                existing_grid.DataSource = New DataTable
                existing_grid.DataBind()
                existing_grid.Visible = False
                lbl_error.Text = GetLocalResourceObject("errNoRecord")
                lbl_error.Visible = True
                btnExport.Visible = False

            End If
        End If

    End Sub
    Protected Sub existing_grid_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim rowIndex As Int16 = CInt(e.CommandArgument.ToString)
        Dim mytable As New DataTable
        Dim str As String = ""

        Dim lblId As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("lblID"), Label)
        Dim FLC_Date As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("FLC_Date"), Label)
        Dim Hall_Name As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("Hall_Name"), Label)
        Dim Hall_Name_Local As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("Hall_Name_Local"), Label)
        Dim Hall_Address As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("Hall_Address"), Label)
        Dim Hall_Address_Local As Label = DirectCast(existing_grid.Rows(rowIndex).FindControl("Hall_Address_Local"), Label)
        If e.CommandName = "edit" Then
            txtFlcID.Text = lblId.Text
            '            txtFlcdate.Text = FLC_Date.Text.Substring(0, 10)
            txtFlcdate.Text = Convert.ToDateTime(FLC_Date.Text.Substring(0, 10), System.Globalization.CultureInfo.GetCultureInfo("en-GB").DateTimeFormat)
            txtHallName.Text = Hall_Name.Text
            txtHallNameL.Text = Hall_Name_Local.Text
            txtHallAddress.Text = Hall_Address.Text
            txtHallAddressL.Text = Hall_Address_Local.Text
            form1.Visible = True
            str = "$('#" + txtFlcID.ClientID + "').val('" + txtFlcID.Text + "');"
            str = str + "$('#" + txtFlcdate.ClientID + "').val('" + txtFlcdate.Text.Trim + "');"
            str = str + "$('#" + txtHallName.ClientID + "').val('" + txtHallName.Text.Trim + "');"
            str = str + "$('#" + txtHallNameL.ClientID + "').val('" + txtHallNameL.Text.Trim + "');"
            str = str + "$('#" + txtHallAddress.ClientID + "').val('" + txtHallAddress.Text.Trim + "');"
            str = str + "$('#" + txtHallAddressL.ClientID + "').val('" + txtHallAddressL.Text + "');"

            ScriptManager.RegisterStartupScript(panel1, panel1.GetType, "google", str, True)
            txtFlcdate.Focus()
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
            MyCommand.Parameters.AddWithValue("@ID", lblId.Text)
            MyTable = MyInterface.DELETEData(MyCommand, "DeleteFLCMaster")
            If MyTable.TableName = "Success" Then
                If MyTable.Rows.Count > 0 Then
                    MessageBox(existing_grid, GetLocalResourceObject("errNotDeleted"))
                    lbl_error.Text = MyTable.Rows(0)(0).ToString
                    lbl_error.Visible = True
                    lblStatus.Visible = False
                    Exit Sub
                End If
                Dim FlcTable As DataTable = objCommonFunctions.getFLCDataTable(dis_code)
                existing_grid.DataSource = FlcTable
                existing_grid.DataBind()

                lblId.Text = ""
                txtFlcdate.Text = ""
                txtFlcID.Text = ""
                txtHallName.Text = ""
                txtHallNameL.Text = ""
                txtHallAddress.Text = ""
                txtHallAddressL.Text = ""

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
        Dim dt As DateTime
        If existsReservewords(txtFlcdate.Text.Trim) = True Then
            flag = False
            errorStr = errorStr + "<li>" + GetLocalResourceObject("errInvalidWordsDate") + "</li>"
        Else

            If txtFlcdate.Text.Trim = "" Then
                flag = False
                errorStr = errorStr + "<li>" + GetLocalResourceObject("errDateRequired") + "</li>"
            Else
                dt = Convert.ToDateTime(txtFlcdate.Text, System.Globalization.CultureInfo.GetCultureInfo("en-GB").DateTimeFormat)
                dt = Convert.ToDateTime(dt, System.Globalization.CultureInfo.GetCultureInfo("en-US").DateTimeFormat)
            End If
        End If


        If existsReservewords(txtHallName.Text.Trim) = True Then
            flag = False
            errorStr = errorStr + "<li>" + GetLocalResourceObject("errInvalidWordsName") + "</li>"
        End If
        If existsReservewords(txtHallNameL.Text.Trim) = True Then
            flag = False
            errorStr = errorStr + "<li>" + GetLocalResourceObject("errInvalidWordsNameL") + "</li>"
        Else
            If txtHallName.Text.Trim = "" Then
                flag = False
                errorStr = errorStr + "<li>" + GetLocalResourceObject("errNoHallName") + "</li>"
            ElseIf txtHallNameL.Text.Trim = "" Then
                txtHallNameL.Text = txtHallName.Text
            End If
        End If
        If existsReservewords(txtHallAddress.Text.Trim) = True Then
            flag = False
            errorStr = errorStr + "<li>" + GetLocalResourceObject("errInvalidWordsAddress") + "</li>"
        Else
            If txtHallAddress.Text.Trim = "" Then
                flag = False
                errorStr = errorStr + "<li>" + GetLocalResourceObject("errNoHallAddress") + "</li>"
            ElseIf txtHallAddressL.Text.Trim = "" Then
                txtHallAddressL.Text = txtHallAddress.Text
            End If
        End If

        If existsReservewords(txtHallAddressL.Text.Trim) = True Then
            flag = False
            errorStr = errorStr + "<li>" + GetLocalResourceObject("errInvalidWordsAddressL") + "</li>"
        End If

        If flag Then
            Dim mytable As New DataTable
            Dim mycommand As New SqlClient.SqlCommand
            Dim MyInterface As ErmsInterface.hbInterface
            Dim Implementer As New ErmsBLL

            MyInterface = Implementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@flcdate", dt.ToString("yyyy-MM-dd")) 'txtFlcdate.Text.Trim)
            mycommand.Parameters.AddWithValue("@hallName", txtHallName.Text.Trim)
            mycommand.Parameters.AddWithValue("@hallNameL", txtHallNameL.Text.Trim)
            mycommand.Parameters.AddWithValue("@hallAddress", txtHallAddress.Text.Trim)
            mycommand.Parameters.AddWithValue("@hallAddressL", txtHallAddressL.Text.Trim)
            mycommand.Parameters.AddWithValue("@flcID", txtFlcID.Text.Trim)
            Try
                mytable = MyInterface.UPDATEData(mycommand, "updateFLCMaster")
                If mytable.Rows.Count > 0 Then
                    lbl_error.Text = mytable.Rows(0)(0).ToString
                    lbl_error.Visible = True
                    lblStatus.Visible = False
                    SetFocus(txtFlcID)
                    form1.Visible = True
                    save_button.Visible = True
                    Exit Sub
                Else
                    form1.Visible = False
                    save_button.Visible = False
                End If
            Catch ex As Exception
                lbl_error.Text = ex.Message
                lbl_error.Visible = True
                lblStatus.Visible = False
                Exit Sub
            End Try

            Dim evmTable As DataTable = objCommonFunctions.getFLCDataTable(dis_code)
            If evmTable.Rows.Count > 0 Then
                existing_grid.DataSource = evmTable
                existing_grid.DataBind()
                dvList.Visible = True
            Else
                existing_grid.DataSource = New DataTable
                existing_grid.DataBind()
                dvList.Visible = False
            End If

            lblStatus.Visible = True
            lbl_error.Visible = False
            lblStatus.Text = GetLocalResourceObject("msgUpdate").ToString

            txtFlcID.Text = ""
            txtFlcdate.Text = ""
            txtHallName.Text = ""
            txtHallNameL.Text = ""
            txtHallAddress.Text = ""
            txtHallAddressL.Text = ""

        Else
            lbl_error.Text = errorStr
            lbl_error.Visible = True
            lblStatus.Visible = False
        End If
    End Sub
    Protected Sub cancel_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_button.Click
        Response.Redirect("listFLC.aspx")
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
    Protected Sub ExportExcel(sender As Object, e As EventArgs)
        Dim dt As DataTable = New DataTable("FLC_" & dis_name & "_List")
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
                'If row.Cells(i).Controls.Count > 0 And i > 0 Then
                dt.Rows(dt.Rows.Count - 1)(i) = (TryCast(row.Cells(i).Controls(1), Label)).Text
                'Else
                '    dt.Rows(dt.Rows.Count - 1)(i) = row.Cells(i).Text
                'End If
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

    Private Sub existing_grid_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles existing_grid.RowEditing
        save_button.Visible = True
        SetFocus(txtFlcdate)
    End Sub

    Private Sub existing_grid_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles existing_grid.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim FLC_Date As Label = DirectCast(e.Row.FindControl("FLC_Date"), Label)
            Dim Hall_Name As Label = DirectCast(e.Row.FindControl("Hall_Name"), Label)
            Dim Hall_Name_Local As Label = DirectCast(e.Row.FindControl("Hall_Name_Local"), Label)
            Dim Hall_Address As Label = DirectCast(e.Row.FindControl("Hall_Address"), Label)
            Dim Hall_Address_Local As Label = DirectCast(e.Row.FindControl("Hall_Address_Local"), Label)
            Dim dt As DateTime = Convert.ToDateTime(FLC_Date.Text, System.Globalization.CultureInfo.GetCultureInfo("en-GB").DateTimeFormat)
            FLC_Date.Text = dt.ToString("dd-MM-yyyy")

            If existsReservewords(FLC_Date.Text) = True Then
                FLC_Date.Text = "Invalid Text Identified"
            End If
            If existsReservewords(Hall_Name.Text) = True Then
                Hall_Name.Text = "Invalid Text Identified"
            End If
            If existsReservewords(Hall_Name_Local.Text) = True Then
                Hall_Name_Local.Text = "Invalid Text Identified"
            End If
            If existsReservewords(Hall_Address.Text) = True Then
                Hall_Address.Text = "Invalid Text Identified"
            End If
            If existsReservewords(Hall_Address_Local.Text) = True Then
                Hall_Address_Local.Text = "Invalid Text Identified"
            End If
        End If
    End Sub
End Class
Imports System.Drawing
Imports System.IO
Imports System.Web.Services
Imports ClosedXML.Excel

Public Class viewEVM
    Inherits SetCulture

    Dim objCommonFunctions As New commonFunctions()
    Public MyTable, MyTableDis As New DataTable
    Dim userName, userLevel, dis_code As String
    Protected json_string As String = ""
    Dim MyInterface As ErmsInterface.hbInterface
    Dim Implementer As New ErmsBLL
    Dim MyCommand As New Data.SqlClient.SqlCommand

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        userName = Session("UserID")
        dis_code = Session("gblDistrictCode")
        userLevel = Session("UserLevel")
        hdnLangCode.Value = IIf(Session("MyLanguage") IsNot Nothing, Session("MyLanguage"), "hi-IN")
        If Not IsPostBack Then

            MyTableDis = objCommonFunctions.GetEVMSummaryDataTable()
            If MyTableDis.Rows.Count > 0 Then

                existing_grid.DataSource = MyTableDis
                existing_grid.DataBind()
                existing_grid.Visible = True
                lbl_error.Visible = False
                btnExport.Text = Resources.Resource.Download.ToString
                btnExport.Visible = True
            Else
                existing_grid.Visible = False
                lbl_error.Visible = True
                lbl_error.Text = GetLocalResourceObject("lblNoRecords")
                btnExport.Visible = False

            End If
        End If
    End Sub
    Protected Sub cancel_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_button.Click
        Response.Redirect("viewEVM.aspx")
    End Sub
    Private Sub existing_grid_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles existing_grid.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lbldiscode As Label = DirectCast(e.Row.FindControl("lbldiscode"), Label)
            Dim lblDisNameL As Label = DirectCast(e.Row.FindControl("lblDisNameL"), Label)
            Dim lblDisName As Label = DirectCast(e.Row.FindControl("lblDisName"), Label)
            Select Case lbldiscode.Text
                Case "9998"
                    e.Row.BackColor = Drawing.Color.LightPink
                Case "9999"
                    e.Row.Font.Bold = True
                    e.Row.BackColor = Drawing.Color.LightGray
            End Select

        End If
    End Sub

    'Private Sub view_button_Click(sender As Object, e As EventArgs) Handles view_button.Click
    '    If existing_grid.Rows.Count > 0 Then
    '        Dim strText As New StringBuilder("<table cellspacing='10'>")
    '        Dim col As Int16 = 0
    '        For Each row As GridViewRow In existing_grid.Rows
    '            Dim lblsn As Label = DirectCast(row.FindControl("lblsn"), Label)

    '            If col Mod 5 = 0 Then
    '                strText.Append("<tr>")
    '            End If
    '            strText.Append("<td style='text-align:center;'><img src='" + "appImages/" + lblsn.Text + ".png" + "' height='85%' width='85%' alt='QR Image' /><br/>")
    '            strText.Append(lblsn.Text + "</td>")

    '            If row.RowIndex = existing_grid.Rows.Count - 1 Or (col > 0 And col Mod 5 = 0) Then
    '                strText.Append("</tr>")
    '                col = 0
    '            Else
    '                col += 1
    '            End If
    '        Next
    '        strText.Append("</table>")

    '        Session("strText") = strText
    '        Session("unitType") = rbUnitType.SelectedItem.Text
    '        Response.Write("<script language=JavaScript>window.open('printForm.aspx' , '_blank' ,'toolbar=no,menubar=no,status=no,location=no,scrollbars=no,resizable=1,fullscreen=yes');</script>")
    '    Else
    '        MessageBox(view_button, GetLocalResourceObject("NothingToPrint"))
    '    End If
    'End Sub

    <WebMethod(EnableSession:=True)>
    Public Shared Function getChartData() As String
        Dim lang As String = "hi-IN"
        Dim dis As String = IIf(String.IsNullOrEmpty(HttpContext.Current.Session("gblDistrictCode")), "0000", HttpContext.Current.Session("gblDistrictCode"))
        Try
            lang = HttpContext.Current.Session("MyLanguage").ToString
            dis = HttpContext.Current.Session("gblDistrictCode").ToString
        Catch ex As Exception
            lang = "hi-IN"
            dis = "0000"
        End Try
        Dim json_string As String = ""
        Dim MyInterface As ErmsInterface.hbInterface
        Dim Implementer As New ErmsBLL
        Dim MyCommand As New SqlClient.SqlCommand
        MyInterface = Implementer
        MyCommand.Parameters.Clear()
        MyCommand.Parameters.Add("@lang", SqlDbType.VarChar, 10).Value = lang
        MyCommand.Parameters.Add("@dis", SqlDbType.VarChar, 4).Value = dis
        Dim mytable As DataTable = MyInterface.SELECTData(MyCommand, "GetChartData")
        Dim obj As commonFunctions = New commonFunctions
        json_string = obj.getJsonStringofTable(mytable)

        Return json_string
    End Function

    'Protected Sub ExportExcel(sender As Object, e As EventArgs)
    '    Dim dt As DataTable = Session("myTable")
    '    Using wb As New XLWorkbook()
    '        Dim dr0 As DataRow = dt.NewRow
    '        ' dr0(0) = lblModTitle.Text.Trim
    '        dt.Rows.Add(dr0)
    '        dt.AcceptChanges()
    '        wb.Worksheets.Add(dt, "Sheet1")

    '        Response.Clear()
    '        Response.Buffer = True
    '        Response.Charset = ""
    '        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
    '        Response.AddHeader("content-disposition", "attachment;filename=MyRepresentatives.xlsx")
    '        Using MyMemoryStream As New MemoryStream()
    '            wb.SaveAs(MyMemoryStream)
    '            MyMemoryStream.WriteTo(Response.OutputStream)
    '            Response.Flush()
    '            Response.End()
    '        End Using
    '    End Using
    '    Session.Remove("myTable")
    'End Sub

    Protected Sub ExportExcel(sender As Object, e As EventArgs)


        Dim dt As DataTable = New DataTable("EVM_Summary")
        Dim rowCount As Int16 = dt.Rows.Count
        If existing_grid.Rows.Count <= 0 Then
            lbl_error.Text = "No records to download"
            lbl_error.Visible = True
        Else
            lbl_error.Visible = False
            For Each cell1 As TableCell In existing_grid.HeaderRow.Cells
                dt.Columns.Add(cell1.Text)
            Next
            For Each row As GridViewRow In existing_grid.Rows
                dt.Rows.Add()
                dt.Rows(dt.Rows.Count - 1)(0) = DirectCast(row.FindControl("lblDisName"), Label).Text
                dt.Rows(dt.Rows.Count - 1)(1) = DirectCast(row.FindControl("lblCU"), Label).Text
                dt.Rows(dt.Rows.Count - 1)(2) = DirectCast(row.FindControl("lblBU"), Label).Text
                dt.Rows(dt.Rows.Count - 1)(3) = DirectCast(row.FindControl("lblTotalUnits"), Label).Text
                'For i As Integer = 0 To row.Cells.Count - 2
                '    dt.Rows(dt.Rows.Count - 1)(i) = row.Cells(i).Text

                'If i = 0 Then
                '    dt.Rows(dt.Rows.Count - 1)(i) = (cnts + 1).ToString
                'Else
                '    dt.Rows(dt.Rows.Count - 1)(i) = row.Cells(i).Text
                'End If
                'Next
                'cnts += 1
            Next
            Using wb As XLWorkbook = New XLWorkbook()
                wb.Worksheets.Add(dt)
                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename=EvmReport.xlsx")

                Using MyMemoryStream As MemoryStream = New MemoryStream()
                    wb.SaveAs(MyMemoryStream)
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.Flush()
                    Response.End()
                End Using
            End Using
        End If
    End Sub
    'Private Function UserHasPermissionToExport() As Boolean
    '    ' Example: Only users with "Admin" role can export data
    '    User.IsInRole("User")
    '    Return User.IsInRole("Admin")
    'End Function
    'Public Overrides Sub VerifyRenderingInServerForm(control As Control)
    '    ' Verifies that the control is rendered
    'End Sub
End Class
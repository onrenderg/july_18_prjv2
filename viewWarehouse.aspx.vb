Imports System.Drawing
Imports System.IO
Imports System.Web.Services
Imports ClosedXML.Excel

Public Class viewWarehouse
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
            MyTableDis = objCommonFunctions.getWareHousesDataTable("0000")
            If MyTableDis.Rows.Count > 0 Then
                Session("mapData") = MyTableDis
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
                Session.Remove("mapData")
            End If
        End If
    End Sub

    Protected Sub view_button_Click(sender As Object, e As EventArgs)
        MyTable = objCommonFunctions.fillWarehouseMapData("0000")
        If MyTable.Rows.Count > 0 Then
            Session("mapData") = MyTable
            ScriptManager.RegisterStartupScript(Me, Page.GetType(), "New Window", "window.open('viewOnMap.aspx');", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Page.GetType(), "alert", "alert('" + Resources.Resource.WarehouseLocationNotFound + "');", True)
        End If
    End Sub

    Protected Sub cancel_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_button.Click
        Response.Redirect("viewWarehouse.aspx")
    End Sub
    Private Sub existing_grid_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles existing_grid.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lbldiscode As Label = DirectCast(e.Row.FindControl("lbldiscode"), Label)
            Select Case lbldiscode.Text
                Case "9998"
                    e.Row.BackColor = Drawing.Color.LightPink
                Case "9999"
                    e.Row.Font.Bold = True
                    e.Row.BackColor = Drawing.Color.LightGray
            End Select
        End If
    End Sub

    Protected Sub ExportExcel(sender As Object, e As EventArgs)
        Dim dt As DataTable = New DataTable("Warehouse_Summary")
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
            Response.AddHeader("content-disposition", "attachment;filename=WarehouseReport.xlsx")
            Using MyMemoryStream As MemoryStream = New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub

End Class
Imports System.IO
Imports System.Web.Services
Imports ClosedXML.Excel

Public Class viewBallotBox
    Inherits SetCulture
    Dim user As UserData
    Dim objCommonFunctions As New commonFunctions()
    Public MyTable, MyTableDis As New DataTable
    Dim userName, userLevel, dis_code As String
    Protected json_string As String = ""
    Dim MyInterface As ErmsInterface.hbInterface
    Dim Implementer As New ErmsBLL
    Dim MyCommand As New Data.SqlClient.SqlCommand

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        user = commonFunctions.GetUserDataFromSession()
        userName = Session("UserID")
        dis_code = Session("gblDistrictCode")
        userLevel = Session("UserLevel")
        hdnLangCode.Value = IIf(Session("MyLanguage") IsNot Nothing, Session("MyLanguage"), "hi-IN")
        If Not IsPostBack Then
            lblTitle.Text = Resources.Resource.menuBBSummary
            Dim summary_ = objCommonFunctions.getBallotBoxListSummary_Category_Wise(user.Code)
            If summary_.status_code = 200 Then
                Dim list_ = DirectCast(summary_.data, List(Of Ballot.BallotSummary))
                existing_grid.DataSource = list_
                existing_grid.DataBind()
                existing_grid.Visible = True
                lbl_error.Visible = False
                btnExport.Text = Resources.Resource.Download.ToString
                btnExport.Visible = True
            Else
                existing_grid.Visible = False
                lbl_error.Visible = True
                lbl_error.Text = summary_.status_message
                btnExport.Visible = False
            End If
        End If
    End Sub
    Protected Sub cancel_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_button.Click
        Response.Redirect("viewBallotBox.aspx")
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
        MyCommand.Parameters.Add("@chartFor", SqlDbType.VarChar, 5).Value = "BB"
        Dim mytable As DataTable = MyInterface.SELECTData(MyCommand, "GetChartData")
        Dim obj As commonFunctions = New commonFunctions
        json_string = obj.getJsonStringofTable(mytable)

        Return json_string
    End Function

    Protected Sub ExportExcel(sender As Object, e As EventArgs)
        Dim dt As DataTable = New DataTable("BallotBox_Summary")
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
            Response.AddHeader("content-disposition", "attachment;filename=BallotBoxReport.xlsx")
            Using MyMemoryStream As MemoryStream = New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using

    End Sub
End Class
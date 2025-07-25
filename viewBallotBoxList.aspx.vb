Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO
Imports ClosedXML.Excel
Imports SEC_InventoryMgmt.Ballot

Public Class viewBallotBoxList
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
            lblTitle.Text = Resources.Resource.menuBBUnitList

            objCommonFunctions.PopulateDistricts(ddlDis, "All")
            Dim foundItem As ListItem = ddlDis.Items.Cast(Of ListItem)().FirstOrDefault(Function(item) item.Value = LoginUser.DistrictCode)

            If LoginUser.UserLevel <> "000" Then
                If foundItem IsNot Nothing Then
                    ddlDis.SelectedIndex = ddlDis.Items.IndexOf(foundItem)
                End If
                ddlDis.Enabled = False
            Else
                ddlDis.SelectedIndex = 0
            End If

            ' Restore selected index for district
            If Session("selectedDis") IsNot Nothing Then
                Dim selectedDisIndex As Integer
                If Integer.TryParse(Session("selectedDis").ToString(), selectedDisIndex) Then
                    If selectedDisIndex >= 0 AndAlso selectedDisIndex < ddlDis.Items.Count Then
                        ddlDis.SelectedIndex = selectedDisIndex
                    End If
                End If
            End If
            Session("selectedDis") = Nothing
            ddlDis_SelectedIndexChanged(ddlDis, EventArgs.Empty)
            fill_grid()
            lbl_error.Visible = False
            lblStatus.Visible = False
        End If
    End Sub
    Private Sub fill_grid()
        Dim user = LoginUser.Code
        Dim is_self As Boolean = False
        If (ddlBlk.SelectedIndex <> -1) Then
            If CInt(ddlBlk.SelectedValue) > 0 Then
                user = ddlBlk.SelectedValue
            Else
                user = ddlDis.SelectedValue
            End If
        Else
            user = ddlDis.SelectedValue
        End If


        Dim stock_owner_ = objCommonFunctions.getBallotBoxListSummary_Unit_Wise(user, "")
        If (stock_owner_.status_code = 200) Then
            Dim available_bb_list_ As List(Of BallotSummaryList)
            available_bb_list_ = DirectCast(stock_owner_.data, List(Of BallotSummaryList))
            existing_grid.DataSource = available_bb_list_
            existing_grid.DataBind()
            If available_bb_list_.Count > 0 Then
                dvList.Visible = True
                lbl_error.Visible = False
            Else
                dvList.Visible = False
                lbl_error.Visible = True
                lbl_error.Text = GetLocalResourceObject("errNoRecord")
            End If
        Else
            dvList.Visible = False

            lbl_error.Visible = True
            lbl_error.Text = stock_owner_.status_message
        End If
    End Sub

    Protected Sub cancel_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_button.Click
        Response.Redirect("viewBallotBoxList.aspx")
    End Sub


    Protected Sub ddlDis_SelectedIndexChanged(sender As Object, e As EventArgs)
        If ddlDis.SelectedIndex <> -1 Then
            lblBlk.Text = Resources.Resource.Blk.ToString

            objCommonFunctions.PopulatePriBlocks(ddlBlk, ddlDis.SelectedValue, "001", "All")
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
            ' Restore selected index for block
            If Session("selectedBlock") IsNot Nothing Then
                Dim selectedBlkIndex As Integer
                If Integer.TryParse(Session("selectedBlock").ToString(), selectedBlkIndex) Then
                    If selectedBlkIndex >= 0 AndAlso selectedBlkIndex < ddlBlk.Items.Count Then
                        ddlBlk.SelectedIndex = selectedBlkIndex
                    End If
                End If
            End If
            Session("selectedBlock") = Nothing
            ddlBlk_SelectedIndexChanged(ddlBlk, EventArgs.Empty)
            'fill_grid()
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
        If e.CommandName = "ShowQR" Then
            Dim qr_text_description As String = e.CommandArgument.ToString()
            Dim encQR = objCommonFunctions.getQRCODE(qr_text_description)
            Dim strText As New StringBuilder("<table style='font-family:Verdana;font-size:9pt;'>")
            strText.Append("<tr>")
            strText.Append("<td style='text-align:center;'><img src ='data:image/png;base64," + Convert.ToBase64String(encQR) + "' alt='" + qr_text_description + "' height='200px' width='200px' />")
            strText.Append("<br/>" + qr_text_description + "</td>")
            strText.Append("</tr>")
            strText.Append("</table>")
            Session("strText") = strText
            Session("unitType") = Resources.Resource.BallotBox

            ' Use ScriptManager to open printForm.aspx
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openPrintWindow", "window.open('printForm.aspx', '_blank', 'toolbar=no,menubar=no,status=no,location=no,scrollbars=no,resizable=1,fullscreen=yes');", True)
        End If
    End Sub

    Protected Sub map_button_Click(sender As Object, e As EventArgs)

        If ddlBlk.SelectedIndex <> -1 AndAlso ddlBlk.SelectedValue > 0 Then
            Session("selectedBlock") = ddlBlk.SelectedIndex
            Session("selectedDis") = ddlDis.SelectedIndex
            Response.Redirect("viewBBonMap.aspx?qr_text=&to_user_code=" + ddlBlk.SelectedValue)
        Else
            Session("selectedDis") = ddlDis.SelectedIndex
            Response.Redirect("viewBBonMap.aspx?qr_text=&to_user_code=" + ddlDis.SelectedValue)
        End If
    End Sub

    Protected Sub existing_grid_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)

    End Sub

    Private Sub existing_grid_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles existing_grid.RowDataBound

    End Sub


    Protected Sub ExportExcel(sender As Object, e As EventArgs)
        Dim dt As DataTable = New DataTable("existing_grid")

        ' Define only the two required columns
        dt.Columns.Add("Item Description")
        dt.Columns.Add("Item Location")

        ' Loop through rows and extract visible Label values only
        For Each row As GridViewRow In existing_grid.Rows
            Dim newRow As DataRow = dt.NewRow()

            ' Get Item Description (qr_text)
            Dim lblQrText As Label = TryCast(row.FindControl("qr_text"), Label)
            If lblQrText IsNot Nothing Then
                newRow(0) = lblQrText.Text
            End If

            ' Get Display Name only (ignore to_user_code)
            Dim lblDisplayName As Label = TryCast(row.FindControl("Display_Name"), Label)
            If lblDisplayName IsNot Nothing Then
                newRow(1) = lblDisplayName.Text
            End If

            dt.Rows.Add(newRow)
        Next

        ' Export to Excel using ClosedXML
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt, "Summary")

            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=BallotBoxSummaryReport.xlsx")

            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using
    End Sub


End Class
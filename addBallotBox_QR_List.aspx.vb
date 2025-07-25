Imports SEC_InventoryMgmt.Ballot

Public Class addBallotBox_QR_List
    Inherits System.Web.UI.Page
    Dim objCommonFunctions As New commonFunctions()
    Dim user As UserData
    Dim list_ As List(Of Ballot.Print_QR)
    Dim MyTable As New Data.DataTable
    Dim userName, userLevel, dis_code, wh_code, wh_name, wh_name_local, myLang As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        user = commonFunctions.GetUserDataFromSession()
        userName = user.Code
        dis_code = user.DistrictCode
        userLevel = user.UserLevel ' Session("UserLevel")
        wh_code = user.BlockCode 'Session("gblBlockCode")
        wh_name = user.Block_Name 'Session("gblBlockName")
        wh_name_local = user.block_name_local ' Session("gblBlockNameLocal")
        myLang = Session("MyLanguage").ToString
        lblTitle.Text = Resources.Resource.menuBBSummary
        If Not IsPostBack Then
            fill_grid(Request.QueryString("from_sr_no").ToString(), Request.QueryString("to_sr_no").ToString(), Request.QueryString("series").ToString())
        End If
        'list_ = list_
    End Sub

    Private Sub existing_grid_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles existing_grid.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
        End If
    End Sub

    ' CLASS BASED

    Protected Sub existing_grid_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim rowIndex As Integer = Integer.Parse(e.CommandArgument.ToString())

        Dim qr_text_description As String = CStr(existing_grid.DataKeys(rowIndex).Values("qr_text_description"))

        If e.CommandName = "ShowQR" Then
            Dim DBAccess = New commonFunctions
            Dim col As Int16 = 1
            Dim cnt As Int16 = 0
            Dim strText As New StringBuilder("<table style='font-family:Verdana;font-size:9pt;'>")

            Dim QRTextCode As Byte() = objCommonFunctions.getQRCODE(qr_text_description)

            cnt += 1
            If col = 1 Or col + 1 Mod 4 = 0 Then
                strText.Append("<tr>")
            End If
            strText.Append("<td style='text-align:center;'><img src ='data:image/png;base64," + Convert.ToBase64String(QRTextCode) + "' alt='" + qr_text_description + "' height='200px' width='200px' />")
            strText.Append("<br/>" + qr_text_description + "</td>")

            If (col >= 4 And col Mod 4 = 0) Then
                strText.Append("</tr>")
                col = 1
            Else
                col += 1
            End If

            If cnt > 0 Then
                strText.Append("</table>")
                Session("strText") = strText
                Session("unitType") = Resources.Resource.BallotBox

                Response.Write("<script language=JavaScript>window.open('printForm.aspx' , '_blank' ,'toolbar=no,menubar=no,status=no,location=no,scrollbars=no,resizable=1,fullscreen=yes');</script>")
            End If
        End If

    End Sub

    Private Sub fill_grid(from_sr_no As String, to_sr_no As String, series As String)
        Dim series_ = objCommonFunctions.Show_Ballot_ByID(user.Code, from_sr_no, to_sr_no, series)
        If series_.status_code = 200 Then
            ' Usign Direct Cast as I have received a list from Proc
            list_ = DirectCast(series_.data, List(Of Ballot.Print_QR))
            existing_grid.DataSource = list_
            existing_grid.DataBind()
        End If
    End Sub
End Class
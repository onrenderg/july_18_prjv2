Imports SEC_InventoryMgmt.Ballot

Public Class addBallotBox
    Inherits System.Web.UI.Page
    Dim objCommonFunctions As New commonFunctions()
    Dim user As UserData
    Dim list_ As List(Of Ballot.Ballot_Series)
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
        If Not IsPostBack Then
            lblTitle.Text = Resources.Resource.menuAddBallot
            Dim series_ = objCommonFunctions.Ballot_Series_Master_Get()
            If series_.status_code = 200 Then
                ' Usign Direct Cast as I have received a list from Proc
                list_ = DirectCast(series_.data, List(Of Ballot.Ballot_Series))
                ViewState("BallotSeriesList") = list_ ' Store list_ in ViewState
                ddlSeries.DataSource = list_
                ddlSeries.DataTextField = "series" ' The property to display
                ddlSeries.DataValueField = "series"          ' The property to use as the value
                ddlSeries.DataBind()
                ddlSeries_SelectedIndexChanged(ddlSeries, EventArgs.Empty)
            End If
            fill_grid()
        Else
            ' Retrieve list_ from ViewState on postbacks
            If ViewState("BallotSeriesList") IsNot Nothing Then
                list_ = DirectCast(ViewState("BallotSeriesList"), List(Of Ballot.Ballot_Series))
                ' You can use list_ here for other operations if needed.
            End If
        End If
        'list_ = list_
    End Sub

    Protected Sub ddlSeries_SelectedIndexChanged(sender As Object, e As EventArgs)
        If (ddlSeries.SelectedIndex <> -1) Then
            Dim mySelectedVal As Ballot.Ballot_Series = list_.ElementAt(ddlSeries.SelectedIndex)
            'Fill Model (size)
            objCommonFunctions.PopulateCommonMasterDropdown(ddlModel, "BALLOTBOXSIZE")
            Dim foundSize As ListItem = ddlModel.Items.FindByValue(mySelectedVal.box_size)
            If foundSize IsNot Nothing Then
                ddlModel.SelectedIndex = ddlModel.Items.IndexOf(foundSize)
            Else
                ' Handle the case where the value was not found
                ddlModel.SelectedIndex = 0 'sets the index to the first item.
            End If

            'Fill Month 
            objCommonFunctions.BindMonthDropdown(ddlMon)

            Dim foundmonth As ListItem = ddlMon.Items.FindByValue(CInt(mySelectedVal.series_datetime.Month.ToString()))
            If foundmonth IsNot Nothing Then
                ddlMon.SelectedIndex = ddlMon.Items.IndexOf(foundmonth)
            Else
                ' Handle the case where the value was not found
                ddlMon.SelectedIndex = 0 'sets the index to the first item.
            End If

            'Fill Year 
            objCommonFunctions.BindYearDropdown(ddlYr)

            Dim foundYear As ListItem = ddlYr.Items.FindByValue(CInt(mySelectedVal.series_datetime.Year.ToString()))
            If foundYear IsNot Nothing Then
                ddlYr.SelectedIndex = ddlYr.Items.IndexOf(foundYear)
            Else
                ' Handle the case where the value was not found
                ddlYr.SelectedIndex = 0 'sets the index to the first item.
            End If

        End If
    End Sub

    Protected Sub existing_grid_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)

    End Sub

    Protected Sub save_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save_button.Click
        Dim flag As Boolean = True
        Dim errorStr As String = ""
        Dim Item As New ListItem
        If ddlModel.SelectedIndex = -1 Or ddlModel.SelectedValue = "0" Then
            flag = False
            errorStr = errorStr + "<li>" + GetLocalResourceObject("errNoModel") + "</li>"
        End If
        If txtUnitCounts.Text.Trim = "" Then
            flag = False
            errorStr = errorStr + "<li>" + GetLocalResourceObject("errTotalRequired") + "</li>"
        End If
        If ddlSeries.SelectedIndex = -1 Or ddlSeries.SelectedValue = "0" Then
            flag = False
            errorStr = errorStr + "<li>" + GetLocalResourceObject("errNoSeries") + "</li>"
        End If
        If ddlMon.SelectedIndex = -1 Or ddlMon.SelectedValue = "0" Then
            flag = False
            errorStr = errorStr + "<li>" + GetLocalResourceObject("errNoMonth") + "</li>"
        End If
        If ddlYr.SelectedIndex = -1 Or ddlYr.SelectedValue = "0" Then
            flag = False
            errorStr = errorStr + "<li>" + GetLocalResourceObject("errNoYear") + "</li>"
        End If
        If flag Then
            Dim LoginData As UserData = commonFunctions.GetUserDataFromSession()
            Dim DBAccess = New commonFunctions()
            Dim Result As Generic_Response = DBAccess.UpdateBallotBoxStock(ddlSeries.SelectedValue, ddlModel.SelectedValue, txtUnitCounts.Text, txtFromSrNo.Text, 0, ddlMon.SelectedValue, ddlYr.SelectedValue, LoginData.Code, LoginData.Code, 1)
            If (Result.status_code = 200 Or Result.status_code = 201) Then
                fill_grid()

                txtUnitID.Text = ""
                txtUnitCounts.Text = ""
                ddlModel.SelectedIndex = 0
                ddlMon.SelectedIndex = 0
                ddlYr.SelectedIndex = 0
                txtFromSrNo.Text = ""

                lblStatus.Visible = True
                lbl_error.Visible = False
                If txtUnitID.Text.Trim <> "" Then
                    lblStatus.Text = GetLocalResourceObject("msgUpdate").ToString & "<br/>" & GetLocalResourceObject("qr_buttonResource1.Text")
                Else
                    lblStatus.Text = GetLocalResourceObject("msgSave").ToString & "<br/>" & GetLocalResourceObject("qr_buttonResource1.Text")
                End If
                txtUnitID.Text = ""
            Else
                lbl_error.Text = Result.status_message
                lbl_error.Visible = True
                lblStatus.Visible = False
                If lbl_error.Text.ToLower.Contains("invalid sr no") Then
                    SetFocus(txtFromSrNo)
                End If
            End If
            Return
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

            'Dim BBSize As Label = DirectCast(e.Row.FindControl("BBSize"), Label)
            'Dim BBCount As Label = DirectCast(e.Row.FindControl("BBCount"), Label)
            'Dim FromSrNo As Label = DirectCast(e.Row.FindControl("FromSrNo"), Label)
            'Dim ToSrNo As Label = DirectCast(e.Row.FindControl("ToSrNo"), Label)
            'Dim MonYrPurchase As Label = DirectCast(e.Row.FindControl("MonYrPurchase"), Label)
            'Dim lbllc As Label = DirectCast(e.Row.FindControl("lbllc"), Label)
            'Dim lblwh As Label = DirectCast(e.Row.FindControl("lblwh"), Label)
            'Dim btnGenerateQR As Button = DirectCast(e.Row.FindControl("btnGenerateQR"), Button)
            'Dim btnViewQR As Button = DirectCast(e.Row.FindControl("btnViewQR"), Button)
            'Dim QR_Generated As Label = DirectCast(e.Row.FindControl("QR_Generated"), Label)
            'Dim SqBracketStart As Literal = DirectCast(e.Row.FindControl("SqBracketStart"), Literal)
            'Dim hyphen As Literal = DirectCast(e.Row.FindControl("hyphen"), Literal)
            'Dim SqBracketEnd As Literal = DirectCast(e.Row.FindControl("SqBracketEnd"), Literal)
            'If Val(FromSrNo.Text) > 0 Then
            '    SqBracketStart.Visible = True
            '    hyphen.Visible = True
            '    SqBracketEnd.Visible = True
            '    FromSrNo.Visible = True
            '    ToSrNo.Visible = True
            'Else
            '    SqBracketStart.Visible = False
            '    hyphen.Visible = False
            '    SqBracketEnd.Visible = False
            '    FromSrNo.Visible = False
            '    ToSrNo.Visible = False
            'End If
            'If MonYrPurchase.Text = "01-1900" Then
            '    MonYrPurchase.Text = "Old Ballot Box"
            'End If
            'If QR_Generated.Text = "Y" Then
            '    btnViewQR.Visible = True
            'Else
            '    btnGenerateQR.Visible = True
            'End If

            'BBSize.Text = ddlModel.Items.FindByValue(BBSize.Text).Text
            'ddlModel.SelectedIndex = 0

            'txtUnitCounts.Text = ""

        End If
    End Sub

    ' CLASS BASED

    Protected Sub existing_grid_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim rowIndex As Integer = Integer.Parse(e.CommandArgument.ToString())

        ' Get the DataKeys (from_sr_no, to_sr_no, series)
        Dim fromSrNo As Int64 = CLng(existing_grid.DataKeys(rowIndex).Values("from_sr_no"))
        Dim toSrNo As Int64 = CLng(existing_grid.DataKeys(rowIndex).Values("to_sr_no"))
        Dim series As String = CStr(existing_grid.DataKeys(rowIndex).Values("series"))
        Dim box_size As String = CStr(existing_grid.DataKeys(rowIndex).Values("box_size"))

        If e.CommandName = "delete" Then
            Dim DBAccess = New commonFunctions
            Dim result = DBAccess.deleteBallotMaster(user.Code, series, box_size, fromSrNo, toSrNo)
            If result.status_code = 200 Then
                lbl_error.Visible = False
                lblStatus.Visible = True
                lblStatus.Text = result.status_message
                fill_grid()
            Else
                lbl_error.Visible = True
                lblStatus.Visible = False
                lbl_error.Text = result.status_message
            End If
        ElseIf e.CommandName = "ShowQR" Then
            Dim DBAccess = New commonFunctions

            'Response.Redirect($"addBallotBox_QR_List.aspx?from_sr_no={fromSrNo}&to_sr_no={toSrNo}&series={series}")
            Dim series_ As Generic_Response = DBAccess.Show_Ballot_ByID(user.Code, fromSrNo, toSrNo, series)
            If series_.status_code = 200 Then
                ' Usign Direct Cast as I have received a list from Proc
                Dim QR_list_ As List(Of Ballot.Print_QR) = DirectCast(series_.data, List(Of Ballot.Print_QR))
                Dim qr_text_description As String = ""
                Dim col As Int16 = 1
                Dim cnt As Int16 = 0
                Dim strText As New StringBuilder("<table style='font-family:Verdana;font-size:9pt;'>")

                For Each qlr As Ballot.Print_QR In QR_list_
                    If qlr.qr_text <> qlr.qr_text_description Then
                        qlr.qr_text = objCommonFunctions.EncryptAES(qlr.qr_text_description)
                    End If
                    qr_text_description = qlr.qr_text

                    Dim QRTextCode As Byte() = objCommonFunctions.getQRCODE(qr_text_description)
                    cnt += 1
                    If col = 1 Or col + 1 Mod 4 = 0 Then
                        strText.Append("<tr>")
                    End If
                    strText.Append("<td style='text-align:center;'><img src ='data:image/png;base64," + Convert.ToBase64String(QRTextCode) + "' alt='" + qr_text_description + "' height='200px' width='200px' />")
                    strText.Append("<br/>" + qlr.qr_text_description + "</td>")

                    If (col >= 4 And col Mod 4 = 0) Then
                        strText.Append("</tr>")
                        col = 1
                    Else
                        col += 1
                    End If
                Next

                If cnt > 0 Then
                    strText.Append("</table>")
                    Session("strText") = strText
                    Session("unitType") = Resources.Resource.BallotBox
                    Response.Write("<script language=JavaScript>window.open('printForm.aspx' , '_blank' ,'toolbar=no,menubar=no,status=no,location=no,scrollbars=no,resizable=1,fullscreen=yes');</script>")
                End If
            End If


        End If

    End Sub

    Private Sub fill_grid()
        Dim ballot_stock_owner_ = objCommonFunctions.Show_Ballot_Owner(user.Code)
        If (ballot_stock_owner_.status_code = 200) Then
            Dim available_stock_list_ As List(Of Ballot.Ballot_Stock_Owner)
            available_stock_list_ = DirectCast(ballot_stock_owner_.data, List(Of Ballot.Ballot_Stock_Owner))
            existing_grid.DataSource = available_stock_list_
            existing_grid.DataBind()
        End If
    End Sub

End Class
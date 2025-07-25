Imports System.IO
Imports ClosedXML.Excel
'Imports MessagingToolkit.QRCode
'Imports MessagingToolkit.QRCode.Codec

Public Class generateQR
    Inherits SetCulture
    Dim objCommonFunctions As New commonFunctions()
    Dim MyTable As New DataTable
    Dim userName, userLevel, dis_code, dis_name, dis_name_local, wh_code, wh_name, wh_name_local, myLang As String



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserID") Is Nothing Then
            Response.Redirect("login.aspx", True)
        End If
        userName = Session("UserID")

        myLang = Session("MyLanguage")
        dis_code = Session("gblDistrictCode")
        dis_name = Session("gblDistrictName")
        dis_name_local = Session("gblDistrictNameLocal")
        wh_code = Session("gblBlockCode")
        wh_name = Session("gblBlockName")
        wh_name_local = Session("gblBlockNameLocal")
        userLevel = Session("UserLevel")

        lblWhName.Text = "<b>" & Resources.Resource.Warehouse.ToString & ": </b>" & IIf(myLang = "hi-IN", wh_name_local, wh_name) _
            & ", " & Resources.Resource.ofDis & IIf(myLang = "hi-IN", dis_name_local, dis_name)
        lblWhAddress.Text = "<b>" & GetLocalResourceObject("lblAddrss").ToString & "</b>" & IIf(myLang = "hi-IN", Session("gblWhAddressLocal"), Session("gblWhAddress"))

        'txtlat.Text = "31.088339000"
        'txtlong.Text = "77.180438700"
        Session.Remove("mapData")


        If Not IsPostBack Then
            '& ", " & Resources.Resource.ofDis & IIf(myLang = "hi-IN", dis_name_local, dis_name)
            Dim Unit_Type As String = "AL"
            If userLevel = "012" Or userLevel = "013" Then
                If wh_code.StartsWith("1") Then
                    Unit_Type = "BB"
                ElseIf wh_code.StartsWith("0") Then
                    Unit_Type = "BC"
                End If
            End If
            objCommonFunctions.PopulateUnitTypes(rbUnitType, Unit_Type)
            Select Case rbUnitType.SelectedValue
                Case "BB"
                    lblList.Text = Resources.Resource.lblListBallotBox
                Case "BU"
                    lblList.Text = Resources.Resource.lblListBU

                Case "CU"
                    lblList.Text = Resources.Resource.lblListCU

            End Select
            MyTable = objCommonFunctions.GetEVMPendingForQRDataTable(dis_code, wh_code, rbUnitType.SelectedValue)
            If MyTable.Rows.Count > 0 Then
                existing_grid.DataSource = MyTable
                existing_grid.DataBind()
                existing_grid.Visible = True
                'lblList.Text = IIf(rbUnitType.SelectedValue = "BB", Resources.Resource.lblListBallotBox, Resources.Resource.lblListEVM)
                lblCounts.Text = "[" & MyTable.Rows.Count.ToString & "]"
                view_button.Enabled = True
            Else
                existing_grid.DataSource = New DataTable
                existing_grid.DataBind()
                existing_grid.Visible = False
                lblCounts.Text = "[0]"
                lbl_error.Visible = True
                lbl_error.Text = GetLocalResourceObject("NothingToPrint").ToString
                view_button.Enabled = False
            End If
        End If
    End Sub
    Protected Sub cancel_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_button.Click
        Response.Redirect("generateQR.aspx")
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

    Protected Sub rbUnitType_SelectedIndexChanged(sender As Object, e As EventArgs)
        lbl_error.Visible = False
        dvBBType.Visible = False
        Select Case rbUnitType.SelectedValue
            Case "BB"
                lblList.Text = Resources.Resource.lblListBallotBox
                dvBBType.Visible = True
                objCommonFunctions.PopulateCommonMasterRadio(rbMakeType, "BALLOTBOXTYPE")
            Case "BU"
                lblList.Text = Resources.Resource.lblListBU
            Case "CU"
                lblList.Text = Resources.Resource.lblListCU
        End Select

        MyTable = objCommonFunctions.GetEVMPendingForQRDataTable(dis_code, wh_code, rbUnitType.SelectedValue)
        If MyTable.Rows.Count > 0 Then
            existing_grid.DataSource = MyTable
            existing_grid.DataBind()
            existing_grid.Visible = True
            lbl_error.Visible = False
            'lblList.Text = IIf(rbUnitType.SelectedValue = "BB", Resources.Resource.lblListBallotBox, Resources.Resource.lblListEVM)
            lblCounts.Text = "[" & MyTable.Rows.Count.ToString & "]"
            view_button.Enabled = True
        Else
            existing_grid.DataSource = New DataTable
            existing_grid.DataBind()
            existing_grid.Visible = False
            lbl_error.Text = GetLocalResourceObject("errNoRecord")
            lbl_error.Visible = True
            lblCounts.Text = "[0]"
            view_button.Enabled = False
        End If
    End Sub

    Private Sub view_button_Click(sender As Object, e As EventArgs) Handles view_button.Click

        Dim cnt As Int16 = 0

        For Each row As GridViewRow In existing_grid.Rows
            Dim lblID As Label = DirectCast(row.FindControl("lblID"), Label)
            Dim lblSrs As Label = DirectCast(row.FindControl("lblSrs"), Label)
            Dim lblsn As Label = DirectCast(row.FindControl("lblsn"), Label)
            Dim lblmk As Label = DirectCast(row.FindControl("make"), Label)
            Dim lblmd As Label = DirectCast(row.FindControl("model"), Label)
            Dim lblmy As Label = DirectCast(row.FindControl("lblmy"), Label)
            Dim lblmyP As Label = DirectCast(row.FindControl("lblmyP"), Label)
            Dim lblQR As String = ""
            If rbUnitType.SelectedValue = "BB" Then
                '-- 'QR code = UnitID $ Series $ SrNo $  monthOfPurchase-yearOfPurchase $ UnitType e.g. HPSEC$M$2304$00001   1$BBL$00001$05-2015$BB
                lblQR = lblID.Text.ToString() + "$" + "HPSEC" + "$" + lblSrs.Text.ToUpper() + "$" + Right("00000" + lblsn.Text, 5) + "$" + lblmyP.Text + "$" + "BB"

            Else
                'QR code = UnitID & Series $ SrNo $ make $ model $ monthOfManufacturing-yearOfManufacturing $  monthOfPurchase-yearOfPurchase $ UnitType
                If Not (String.IsNullOrEmpty(lblID.Text) And String.IsNullOrEmpty(lblSrs.Text) And String.IsNullOrEmpty(lblsn.Text) And String.IsNullOrEmpty(lblmk.Text) And String.IsNullOrEmpty(lblmd.Text) And String.IsNullOrEmpty(lblmy.Text) And String.IsNullOrEmpty(lblmyP.Text)) Then
                    lblQR = lblID.Text.ToString() + "$" + lblSrs.Text.ToUpper() + "$" + lblsn.Text + "$" + lblmk.Text + "$" + lblmd.Text + "$" + lblmy.Text + "$" + lblmyP.Text + "$" + rbUnitType.SelectedValue
                Else
                    lblQR = ""
                End If
            End If
            If lblQR = "" Then
                Continue For
            End If
            lblQR = objCommonFunctions.EncryptAES(lblQR) 'encryptQR

            Dim mytable As New DataTable
            Dim mycommand As New SqlClient.SqlCommand
            Dim MyInterface As ErmsInterface.hbInterface
            Dim Implementer As New ErmsBLL
            '    'This is to save the EVM/Ballot Box Unit QR Code
            Try
                MyInterface = Implementer
                mycommand.Parameters.Clear()

                mycommand.Parameters.AddWithValue("@UnitID", lblID.Text)
                mycommand.Parameters.AddWithValue("@UnitType", rbUnitType.SelectedValue)
                mycommand.Parameters.AddWithValue("@QRText", lblQR)
                mytable = MyInterface.INSERTData(mycommand, "saveEVMQRCode")
            Catch ex As Exception
                lbl_error.Text = ex.Message
                lbl_error.Visible = True
                lblStatus.Visible = False
                Exit Sub
            End Try
        Next
        lblStatus.Visible = True
        lblStatus.Text = GetLocalResourceObject("msgUpdate")
        view_button.Enabled = False
    End Sub

    Private Sub existing_grid_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles existing_grid.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            If rbUnitType.SelectedValue = "BB" Then
                ' e.Row.Cells(2).Visible = False
                e.Row.Cells(5).Visible = False
                e.Row.Cells(3).Visible = False
                e.Row.Cells(4).Text = GetLocalResourceObject("Size")
            Else
                e.Row.Cells(5).Visible = True
                e.Row.Cells(3).Visible = True
                e.Row.Cells(4).Text = GetLocalResourceObject("TemplateFieldResource4.HeaderText")
            End If
        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            If rbUnitType.SelectedValue = "BB" Then
                Dim lblmyP As Label = DirectCast(e.Row.FindControl("lblmyP"), Label)
                Dim lblmyPP As Label = DirectCast(e.Row.FindControl("lblmyPP"), Label)

                If lblmyP.Text = "01-2000" Then
                    lblmyP.Visible = False
                    lblmyPP.Visible = True
                    lblmyPP.Text = GetLocalResourceObject("OldBB")
                Else
                    lblmyP.Visible = True
                    lblmyPP.Visible = False
                End If
                e.Row.Cells(5).Visible = False
                e.Row.Cells(3).Visible = False

            Else
                e.Row.Cells(5).Visible = True
                e.Row.Cells(3).Visible = True
            End If

        End If
    End Sub
    Protected Sub btnMap_Click(sender As Object, e As EventArgs)
        MyTable = objCommonFunctions.fillWarehouseMapData(wh_code)
        If MyTable.Rows.Count > 0 Then
            Session("mapData") = MyTable
            ScriptManager.RegisterStartupScript(Me, Page.GetType(), "New Window", "window.open('viewOnMap.aspx');", True)

            'rptMarkers.DataSource = MyTable
            'rptMarkers.DataBind()
        Else
            ScriptManager.RegisterStartupScript(Me, Page.GetType(), "alert", "alert('" + Resources.Resource.WarehouseLocationNotFound + "');", True)
        End If
    End Sub
End Class
Public Class allocateEVM
    Inherits SetCulture
    Dim objCommonFunctions As New commonFunctions()
    Dim MyTable As New DataTable
    Dim userName, userLevel, dis_code, blk_code, myLang As String
    Const keyValue As String = "Sec&NicHP@22"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        userName = Session("UserID")
        dis_code = Session("gblDistrictCode")
        userLevel = Session("UserLevel")

        Response.Cache.SetLastModified(DateTime.Now)
        Response.Cache.SetAllowResponseInBrowserHistory(False)
        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        Response.Cache.SetNoStore()
        myLang = Session("MyLanguage").ToString

        If Not IsPostBack Then
            Select Case userLevel
                Case "012", "013"
                    Response.Redirect("allocateEVMWH.aspx", True)
                Case Else
                    objCommonFunctions.PopulateSECCommonMasterValues(rbPriPost, "ELECTIONFOR", "003")

                    objCommonFunctions.PopulateUnitTypes(rbUnitType)
                    If userLevel <> "000" Then 'Ask state user to select district , populate district for other users
                        ddlDis.Items.Clear()
                        ddlDis.Items.Insert(0, New ListItem(IIf(myLang = "hi-IN", Session("gblDistrictNameLocal"), Session("gblDistrictName")), dis_code))
                        ddlDis.SelectedIndex = 0
                        lblBlk.Text = Resources.Resource.lblMC
                        objCommonFunctions.PopulatePriBlocks(ddlBlk, dis_code, rbPriPost.SelectedValue)
                        dvPanch.Visible = False
                        lblMappingCount.Text = ""
                        objCommonFunctions.PopulateListForEVMAllocation(lbox_unmapped_panchayat, ddlDis.SelectedValue, rbUnitType.SelectedValue, "U")
                        If lbox_unmapped_panchayat.Items.Count = 0 Then
                            lbl_error.Text = GetLocalResourceObject("NoUnitsForAllocation")
                            save_button.Enabled = False
                            lbl_error.Visible = True
                        Else
                            save_button.Enabled = True
                            lbl_error.Visible = False
                        End If
                        If ddlBlk.SelectedValue = "00" Or ddlBlk.SelectedValue = "" Then
                            lblMappingCount.Text = ""
                        Else
                            If userLevel = "002" Or userLevel = "003" Then
                                lblMappingCount.Text = ""
                            Else
                                objCommonFunctions.PopulateListForEVMAllocation(lbox_mapped_in_ward, ddlBlk.SelectedValue, rbUnitType.SelectedValue, "M")
                                lblMappingCount.Text = IIf(myLang = "hi-IN", ddlBlk.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
                            End If
                        End If

                        Select Case userLevel
                            Case "002", "003"
                                blk_code = Session("gblBlockCode")

                                Dim priType As Int16 = CInt(blk_code.Substring(4, 3))
                                If priType = 999 Then
                                    rbPriPost.SelectedValue = "002"
                                    lblBlk.Text = Resources.Resource.lblULB
                                    lblPanch.Text = Resources.Resource.lblULB
                                    lblConsWard.Text = Resources.Resource.lblWard
                                ElseIf priType > 990 And priType < 999 Then
                                    rbPriPost.SelectedValue = "003"
                                    lblBlk.Text = Resources.Resource.lblMC
                                    lblPanch.Text = Resources.Resource.lblMCWard
                                    lblConsWard.Text = Resources.Resource.lblPollStation
                                Else
                                    rbPriPost.SelectedValue = "001"
                                    lblBlk.Text = Resources.Resource.lblPanchSamiti
                                    lblPanch.Text = Resources.Resource.lblPanchayat
                                    lblConsWard.Text = Resources.Resource.lblWard
                                End If
                                objCommonFunctions.PopulatePriBlocks(ddlBlk, dis_code, rbPriPost.SelectedValue)
                                ddlBlk.SelectedIndex = ddlBlk.Items.IndexOf(ddlBlk.Items.FindByValue(blk_code))
                                rbUnitType_SelectedIndexChanged(sender, e)
                                dvPri.Visible = False
                                If rbPriPost.SelectedValue = "003" Then
                                    objCommonFunctions.populateMCWards(ddlPanch, blk_code)
                                    ddlPanch.SelectedIndex = 0

                                    dvPanch.Visible = True
                                    dvWard.Visible = True
                                End If
                            Case Else
                                dvPri.Visible = True
                                dvPanch.Visible = False
                                dvWard.Visible = False
                        End Select
                    End If
            End Select
        End If
    End Sub

    Protected Sub save_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save_button.Click
        Dim flag As Boolean = True
        Dim errorStr As String = ""
        Dim Item As New ListItem


        If ddlBlk.SelectedValue = "0" Then
            flag = False
            errorStr = errorStr + "<li>" + GetLocalResourceObject("errNoBlock") + "</li>"
        End If
        If lbox_mapped_in_ward.Items.Count <= 0 Then
            errorStr = errorStr + "<li>" + GetLocalResourceObject("errNoEVMSelected") + "</li>"
            flag = False
        End If

        Dim tbl_EvmMapping As New DataTable
        tbl_EvmMapping.Columns.Add(New DataColumn("UnitID", GetType(Integer)))
        tbl_EvmMapping.Columns.Add(New DataColumn("UnitType", GetType(String)))
        tbl_EvmMapping.Columns.Add(New DataColumn("BlkCode", GetType(String)))
        tbl_EvmMapping.Columns.Add(New DataColumn("PanchCode", GetType(String)))

        Dim drEvmMapping As DataRow = Nothing

        For Each li As ListItem In lbox_mapped_in_ward.Items

            drEvmMapping = tbl_EvmMapping.NewRow()
            drEvmMapping("UnitID") = li.Value
            drEvmMapping("UnitType") = rbUnitType.SelectedValue
            drEvmMapping("BlkCode") = IIf(Len(ddlBlk.SelectedValue > 7), ddlBlk.SelectedValue.Substring(0, 7), ddlBlk.SelectedValue)
            drEvmMapping("PanchCode") = IIf(ddlPanch.SelectedValue = "", ddlBlk.SelectedValue + "000", ddlPanch.SelectedValue)
            tbl_EvmMapping.Rows.Add(drEvmMapping)
        Next

        If flag Then
            Dim mytable As New DataTable
            Dim mycommand As New SqlClient.SqlCommand
            Dim MyInterface As ErmsInterface.hbInterface
            Dim Implementer As New ErmsBLL
            '    'This is to save the EVM Unit allocation
            Try
                MyInterface = Implementer
                mycommand.Parameters.Clear()
                ' @UnitID,@UnitType,@BlkCode,@AllottedBy
                If userLevel = "002" Or userLevel = "003" Then
                    mycommand.Parameters.AddWithValue("@WardNo", ddlConsWard.SelectedValue)
                End If
                mycommand.Parameters.AddWithValue("@tbl_EvmMapping", tbl_EvmMapping)
                mycommand.Parameters.AddWithValue("@AllottedBy", userName)

                mytable = MyInterface.INSERTData(mycommand, "saveEVMMapping")

                If mytable.Rows.Count > 0 And Not String.IsNullOrEmpty(mytable.Rows(0)(0).ToString) Then
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

            lbl_error.Visible = False

            lblStatus.Text = GetLocalResourceObject("msgSave").ToString
            lblStatus.Visible = True
        Else
            lbl_error.Text = errorStr
            lbl_error.Visible = True
            lblStatus.Visible = False
        End If
    End Sub
    Protected Sub cancel_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_button.Click
        Response.Redirect("allocateEVM.aspx")
    End Sub

    'Protected Sub rbPriPost_SelectedIndexChanged(sender As Object, e As EventArgs)
    '' ddlBlk_SelectedIndexChanged(ddlBlk, Nothing)
    'lbl_error.Visible = False
    'lblStatus.Visible = False
    'Select Case rbPriPost.SelectedValue
    '    Case "001" 'gp
    '        ' lblDis.Text = Resources.Resource.ResourceManager.GetString("lblZilaParishad")
    '        dvBlock.Visible = True
    '        lblBlk.Text = Resources.Resource.ResourceManager.GetString("lblPanchSamiti")
    '        lblPanch.Text = Resources.Resource.ResourceManager.GetString("lblPanchayat")
    '    Case "002" 'ulb
    '        ' lblDis.Text = Resources.Resource.ResourceManager.GetString("Dis")
    '        dvBlock.Visible = False
    '        lblPanch.Text = Resources.Resource.ResourceManager.GetString("Ulb")

    '        ddlBlk.SelectedIndex = -1
    '        dvPanch.Visible = True
    '        'If Len(ddlDis.SelectedValue) = 4 Then
    '        '    objCommonFunctions.PopulatePriBlocks(ddlPanch, ddlDis.SelectedValue, rbPriPost.SelectedValue)
    '        '    ddlBlk = ddlPanch
    '        '    ddlPanch.Items(0).Text = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "शहरी स्थानीय निकाय", "ULBs")
    '        'End If
    '    Case "003" 'mc
    '        ' lblDis.Text = Resources.Resource.Dis.ToString
    '        lblBlk.Text = Resources.Resource.Mc.ToString
    '        lblPanch.Text = Resources.Resource.lblMCWard.ToString
    '        dvBlock.Visible = True
    '        dvPanch.Visible = True
    '        'If Len(ddlDis.SelectedValue) = 4 Then
    '        '    objCommonFunctions.PopulatePriBlocks(ddlBlk, ddlDis.SelectedValue, rbPriPost.SelectedValue)
    '        '    If ddlBlk.Items.Count > 1 Then
    '        '        ddlBlk.Items(0).Text = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "नगर निगम वार्ड", "Municipal Corporations")
    '        '    End If
    '        '    ddlBlk.SelectedIndex = 0
    '        'End If
    '        ddlConsWard.Items.Clear()
    '        dvWard.Visible = False
    'End Select


    'If rbPriPost.SelectedValue = "001" Then
    '    dvBlock.Visible = True
    'Else
    '    dvBlock.Visible = False
    'End If
    'If rbPriPost.SelectedValue = "003" Then
    '    dvBlock.Visible = True
    '    dvWard.Visible = False
    'Else
    '    dvWard.Visible = True
    'End If
    ' End Sub

    Protected Sub rbUnitType_SelectedIndexChanged(sender As Object, e As EventArgs)
        lbox_unmapped_panchayat.Items.Clear()
        lbox_mapped_in_ward.Items.Clear()
        lblStatus.Visible = False
        lbl_error.Visible = False
        'Select Case userLevel
        '    Case "002", "003"
        '        blk_code = Session("gblBlockCode")

        '        Dim priType As Int16 = CInt(blk_code.Substring(4, 3))
        '        If priType = 999 Then
        '            rbPriPost.SelectedValue = "002"
        '            lblBlk.Text = Resources.Resource.lblULB
        '            lblPanch.Text = Resources.Resource.lblULB
        '            lblConsWard.Text = Resources.Resource.lblWard
        '        ElseIf priType > 990 And priType < 999 Then
        '            rbPriPost.SelectedValue = "003"
        '            lblBlk.Text = Resources.Resource.lblMC
        '            lblPanch.Text = Resources.Resource.lblMCWard
        '            lblConsWard.Text = Resources.Resource.lblPollStation
        '        Else
        '            rbPriPost.SelectedValue = "001"
        '            lblBlk.Text = Resources.Resource.lblPanchSamiti
        '            lblPanch.Text = Resources.Resource.lblPanchayat
        '            lblConsWard.Text = Resources.Resource.lblWard
        '        End If
        '        objCommonFunctions.PopulatePriBlocks(ddlBlk, dis_code, rbPriPost.SelectedValue)
        '        ddlBlk.SelectedIndex = ddlBlk.Items.IndexOf(ddlBlk.Items.FindByValue(blk_code))
        '        rbUnitType_SelectedIndexChanged(sender, e)
        '        dvPri.Visible = False
        '        objCommonFunctions.populateMCWards(ddlPanch, blk_code)
        '        ddlPanch.SelectedIndex = 0

        '        dvPanch.Visible = True
        '        dvWard.Visible = True
        '    Case Else
        '        dvPri.Visible = True
        '        dvPanch.Visible = False
        '        dvWard.Visible = False
        'End Select
        Select Case rbPriPost.SelectedValue
            Case "001" 'PRI
                lblBlk.Text = Resources.Resource.lblPanchSamiti
                dvBlock.Visible = True
                dvPanch.Visible = False
                dvWard.Visible = False

                ddlPanch.DataSource = New DataTable
                ddlPanch.DataBind()

                ddlConsWard.DataSource = New DataTable
                ddlConsWard.DataBind()

                objCommonFunctions.PopulateListForEVMAllocation(lbox_unmapped_panchayat, ddlDis.SelectedValue, rbUnitType.SelectedValue, "U")
                If lbox_unmapped_panchayat.Items.Count = 0 Then
                    lbl_error.Text = GetLocalResourceObject("NoUnitsForAllocation")
                    save_button.Enabled = False
                    lbl_error.Visible = True
                Else
                    save_button.Enabled = True
                    lbl_error.Visible = False
                End If
                If ddlPanch.SelectedValue = "00" Then
                    lblMappingCount.Text = ""
                Else
                    If userLevel = "002" Or userLevel = "003" Then
                        lblMappingCount.Text = ""
                        dvPanch.Visible = True
                        dvWard.Visible = True
                        ddlPanch.SelectedIndex = 0
                        ddlConsWard.SelectedIndex = 0
                    Else
                        objCommonFunctions.PopulateListForEVMAllocation(lbox_mapped_in_ward, ddlBlk.SelectedValue, rbUnitType.SelectedValue, "M")
                        lblMappingCount.Text = IIf(myLang = "hi-IN", ddlBlk.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, " " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
                    End If
                    'objCommonFunctions.PopulateListForEVMAllocation(lbox_mapped_in_ward, ddlBlk.SelectedValue, rbUnitType.SelectedValue, "M")
                    'lblMappingCount.Text = IIf(myLang = "hi-IN", ddlBlk.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
                End If
            Case "002" 'ULB
                lblPanch.Text = Resources.Resource.lblULB
                dvBlock.Visible = False
                ddlBlk.SelectedIndex = 1
                objCommonFunctions.PopulatePriBlocks(ddlPanch, ddlBlk.SelectedValue, rbPriPost.SelectedValue)

                dvPanch.Visible = True
                dvWard.Visible = False
                objCommonFunctions.PopulateListForEVMAllocation(lbox_unmapped_panchayat, ddlPanch.SelectedValue, rbUnitType.SelectedValue, "U")
                If lbox_unmapped_panchayat.Items.Count = 0 Then
                    lbl_error.Text = GetLocalResourceObject("NoUnitsForAllocation")
                    save_button.Enabled = False
                    lbl_error.Visible = True
                Else
                    save_button.Enabled = True
                    lbl_error.Visible = False
                End If
                If ddlPanch.SelectedValue = "00" Then
                    lblMappingCount.Text = ""
                Else
                    If userLevel = "002" Or userLevel = "003" Then
                        lblMappingCount.Text = ""
                        dvWard.Visible = True
                        ddlPanch.SelectedIndex = 0
                        ddlConsWard.SelectedIndex = 0
                    Else
                        objCommonFunctions.PopulateListForEVMAllocation(lbox_mapped_in_ward, ddlPanch.SelectedValue, rbUnitType.SelectedValue, "M")
                        lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, " " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
                    End If
                    'objCommonFunctions.PopulateListForEVMAllocation(lbox_mapped_in_ward, ddlPanch.SelectedValue, rbUnitType.SelectedValue, "M")
                    'lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
                End If
            Case "003" 'MC
                lblBlk.Text = Resources.Resource.lblMC
                dvBlock.Visible = True
                dvPanch.Visible = False
                dvWard.Visible = False

                objCommonFunctions.PopulateListForEVMAllocation(lbox_unmapped_panchayat, ddlBlk.SelectedValue, rbUnitType.SelectedValue, "U")
                If lbox_unmapped_panchayat.Items.Count = 0 Then
                    lbl_error.Text = GetLocalResourceObject("NoUnitsForAllocation")
                    save_button.Enabled = False
                    lbl_error.Visible = True
                Else
                    save_button.Enabled = True
                    lbl_error.Visible = False
                End If
                If ddlPanch.SelectedValue = "00" Or ddlPanch.SelectedValue = "" Then
                    lblMappingCount.Text = ""
                Else
                    If userLevel = "002" Or userLevel = "003" Then
                        lblMappingCount.Text = ""
                        dvPanch.Visible = True
                        dvWard.Visible = True
                        ddlPanch.SelectedIndex = 0
                        ddlConsWard.SelectedIndex = 0
                    Else
                        objCommonFunctions.PopulateListForEVMAllocation(lbox_mapped_in_ward, ddlBlk.SelectedValue, rbUnitType.SelectedValue, "M")
                        lblMappingCount.Text = IIf(myLang = "hi-IN", ddlBlk.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, " " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
                    End If
                    'objCommonFunctions.PopulateListForEVMAllocation(lbox_mapped_in_ward, ddlBlk.SelectedValue, rbUnitType.SelectedValue, "M")
                    'lblMappingCount.Text = IIf(myLang = "hi-IN", ddlBlk.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
                End If
        End Select
        Select Case userLevel
            Case "002", "003"
                blk_code = Session("gblBlockCode")

                Dim priType As Int16 = CInt(blk_code.Substring(4, 3))
                If priType = 999 Then
                    rbPriPost.SelectedValue = "002"
                    lblBlk.Text = Resources.Resource.lblULB
                    lblPanch.Text = Resources.Resource.lblULB
                    lblConsWard.Text = Resources.Resource.lblWard
                ElseIf priType > 990 And priType < 999 Then
                    rbPriPost.SelectedValue = "003"
                    lblBlk.Text = Resources.Resource.lblMC
                    lblPanch.Text = Resources.Resource.lblMCWard
                    lblConsWard.Text = Resources.Resource.lblPollStation
                Else
                    rbPriPost.SelectedValue = "001"
                    lblBlk.Text = Resources.Resource.lblPanchSamiti
                    lblPanch.Text = Resources.Resource.lblPanchayat
                    lblConsWard.Text = Resources.Resource.lblWard
                End If
                objCommonFunctions.PopulatePriBlocks(ddlBlk, dis_code, rbPriPost.SelectedValue)
                ddlBlk.SelectedIndex = ddlBlk.Items.IndexOf(ddlBlk.Items.FindByValue(blk_code))

                dvPri.Visible = False
                If rbPriPost.SelectedValue = "003" Then
                    objCommonFunctions.populateMCWards(ddlPanch, blk_code)
                Else
                    objCommonFunctions.PopulatePanchayats(ddlPanch, blk_code)
                End If
                ddlPanch.SelectedIndex = 0

                dvPanch.Visible = True
                dvWard.Visible = True
            Case Else
                dvPri.Visible = True
                dvPanch.Visible = False
                dvWard.Visible = False
        End Select
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

    Protected Sub right_button_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim item As New ListItem
        Dim index As Int16 = 0

        For Each item In lbox_unmapped_panchayat.Items
            If item.Selected Then
                lbox_mapped_in_ward.Items.Add(item)

            End If
        Next

        index = lbox_unmapped_panchayat.SelectedIndex
        For Each item In lbox_mapped_in_ward.Items
            lbox_unmapped_panchayat.Items.Remove(item)
            item.Selected = False
        Next
        'If lbox_unmapped_panchayat.SelectedIndex >= 0 Then
        '    item.Text = lbox_unmapped_panchayat.SelectedItem.ToString
        '    item.Value = lbox_unmapped_panchayat.SelectedValue
        '    lbox_mapped_in_ward.Items.Add(item)
        '    lbox_unmapped_panchayat.Items.Remove(item)
        'End If

        If index > 0 Then
            lbox_unmapped_panchayat.SelectedIndex = index - 1
        Else
            If lbox_unmapped_panchayat.Items.Count > 0 Then
                lbox_unmapped_panchayat.SelectedIndex = 0
            End If
        End If
        'If rbPriPost.SelectedValue = "002" Then
        '    lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
        'Else
        '    lblMappingCount.Text = IIf(myLang = "hi-IN", ddlBlk.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
        'End If
        If rbPriPost.SelectedValue = "002" Then
            If userLevel = "002" Or userLevel = "003" Then
                lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के मतदान केंद्र " + ddlConsWard.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, " " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
            Else
                lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
            End If
        Else
            If userLevel = "002" Or userLevel = "003" Then
                lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के मतदान केंद्र " + ddlConsWard.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, " " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
            Else
                lblMappingCount.Text = IIf(myLang = "hi-IN", ddlBlk.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
            End If
        End If


    End Sub
    Protected Sub left_button_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim item As New ListItem
        Dim index As Int16 = 0

        For Each item In lbox_mapped_in_ward.Items
            If item.Selected Then
                lbox_unmapped_panchayat.Items.Add(item)
            End If
        Next

        index = lbox_mapped_in_ward.SelectedIndex
        For Each item In lbox_unmapped_panchayat.Items
            lbox_mapped_in_ward.Items.Remove(item)
            item.Selected = False
        Next
        If index >= 0 Then
            lbox_mapped_in_ward.SelectedIndex = index - 1
        Else
            If lbox_mapped_in_ward.Items.Count > 0 Then
                lbox_mapped_in_ward.SelectedIndex = 0
            End If
        End If
        'If rbPriPost.SelectedValue = "002" Then
        '    lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
        'Else
        '    lblMappingCount.Text = IIf(myLang = "hi-IN", ddlBlk.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
        'End If
        If rbPriPost.SelectedValue = "002" Then
            If userLevel = "002" Or userLevel = "003" Then
                lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के मतदान केंद्र " + ddlConsWard.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, " " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
            Else
                lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
            End If
        Else
            If userLevel = "002" Or userLevel = "003" Then
                lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के मतदान केंद्र " + ddlConsWard.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, " " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
            Else
                lblMappingCount.Text = IIf(myLang = "hi-IN", ddlBlk.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
            End If
        End If


    End Sub
    Protected Sub right_button_all_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        For Each item As ListItem In lbox_unmapped_panchayat.Items
            lbox_mapped_in_ward.Items.Add(item)
        Next
        lbox_unmapped_panchayat.Items.Clear()
        If rbPriPost.SelectedValue = "002" Then
            If userLevel = "002" Or userLevel = "003" Then
                lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के मतदान केंद्र " + ddlConsWard.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, " " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
            Else
                lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
            End If
        Else
            If userLevel = "002" Or userLevel = "003" Then
                lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के मतदान केंद्र " + ddlConsWard.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, " " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
            Else
                lblMappingCount.Text = IIf(myLang = "hi-IN", ddlBlk.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
            End If
        End If
        'lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
    End Sub

    Protected Sub ddlBlk_SelectedIndexChanged(sender As Object, e As EventArgs)
        lbox_unmapped_panchayat.Items.Clear()
        lbox_mapped_in_ward.Items.Clear()

        Select Case rbPriPost.SelectedValue
            Case "001" 'PRI
                objCommonFunctions.PopulateListForEVMAllocation(lbox_unmapped_panchayat, ddlBlk.SelectedValue, rbUnitType.SelectedValue, "U")
                If lbox_unmapped_panchayat.Items.Count = 0 Then
                    lbl_error.Text = GetLocalResourceObject("NoUnitsForAllocation")
                    save_button.Enabled = False
                    lbl_error.Visible = True
                Else
                    save_button.Enabled = True
                    lbl_error.Visible = False
                End If
                If ddlPanch.SelectedValue = "00" Then
                    lblMappingCount.Text = ""
                Else
                    objCommonFunctions.PopulateListForEVMAllocation(lbox_mapped_in_ward, ddlBlk.SelectedValue, rbUnitType.SelectedValue, "M")
                    lblMappingCount.Text = IIf(myLang = "hi-IN", ddlBlk.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
                End If
            Case "002" 'ULB
                objCommonFunctions.PopulateListForEVMAllocation(lbox_unmapped_panchayat, ddlPanch.SelectedValue, rbUnitType.SelectedValue, "U")
                If lbox_unmapped_panchayat.Items.Count = 0 Then
                    lbl_error.Text = GetLocalResourceObject("NoUnitsForAllocation")
                    save_button.Enabled = False
                    lbl_error.Visible = True
                Else
                    save_button.Enabled = True
                    lbl_error.Visible = False
                End If
                If ddlPanch.SelectedValue = "00" Then
                    lblMappingCount.Text = ""
                Else
                    objCommonFunctions.PopulateListForEVMAllocation(lbox_mapped_in_ward, ddlPanch.SelectedValue, rbUnitType.SelectedValue, "M")
                    lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
                End If
            Case "003" 'MC
                objCommonFunctions.PopulateListForEVMAllocation(lbox_unmapped_panchayat, ddlBlk.SelectedValue, rbUnitType.SelectedValue, "U")
                If lbox_unmapped_panchayat.Items.Count = 0 Then
                    lbl_error.Text = GetLocalResourceObject("NoUnitsForAllocation")
                    save_button.Enabled = False
                    lbl_error.Visible = True
                Else
                    save_button.Enabled = True
                    lbl_error.Visible = False
                End If
                If ddlPanch.SelectedValue = "00" Then
                    lblMappingCount.Text = ""
                Else
                    objCommonFunctions.PopulateListForEVMAllocation(lbox_mapped_in_ward, ddlBlk.SelectedValue, rbUnitType.SelectedValue, "M")
                    lblMappingCount.Text = IIf(myLang = "hi-IN", ddlBlk.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
                End If
        End Select
    End Sub

    Protected Sub left_button_all_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles left_button_all.Click
        For Each item As ListItem In lbox_mapped_in_ward.Items
            lbox_unmapped_panchayat.Items.Add(item)
        Next
        lbox_mapped_in_ward.Items.Clear()
        'If rbPriPost.SelectedValue = "002" Then
        '    lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
        'Else
        '    lblMappingCount.Text = IIf(myLang = "hi-IN", ddlBlk.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
        'End If

        If rbPriPost.SelectedValue = "002" Then
            If userLevel = "002" Or userLevel = "003" Then
                lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के मतदान केंद्र " + ddlConsWard.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, " " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
            Else
                lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
            End If
        Else
            If userLevel = "002" Or userLevel = "003" Then
                lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के मतदान केंद्र " + ddlConsWard.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, " " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
            Else
                lblMappingCount.Text = IIf(myLang = "hi-IN", ddlBlk.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
            End If
        End If

    End Sub

    Private Sub ddlPanch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPanch.SelectedIndexChanged
        objCommonFunctions.populatePanchayatWards(ddlConsWard, ddlPanch.SelectedValue)
        objCommonFunctions.PopulateListForEVMAllocation(lbox_unmapped_panchayat, ddlPanch.SelectedValue, rbUnitType.SelectedValue, "U")
        If lbox_unmapped_panchayat.Items.Count = 0 Then
            lbl_error.Text = GetLocalResourceObject("NoUnitsForAllocation")
            save_button.Enabled = False
            lbl_error.Visible = True
        Else
            save_button.Enabled = True
            lbl_error.Visible = False
        End If
        If ddlPanch.SelectedValue = "00" Or ddlPanch.SelectedValue = "" Then
            lblMappingCount.Text = ""
        Else
            objCommonFunctions.PopulateListForEVMAllocation(lbox_mapped_in_ward, ddlPanch.SelectedValue, rbUnitType.SelectedValue, "M")
            lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
        End If
    End Sub

    Private Sub rbPriPost_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbPriPost.SelectedIndexChanged
        lbox_mapped_in_ward.Items.Clear()
        lblMappingCount.Text = ""
        objCommonFunctions.PopulatePriBlocks(ddlBlk, dis_code, rbPriPost.SelectedValue)

        Select Case rbPriPost.SelectedValue
            Case "001" 'PRI
                lblBlk.Text = Resources.Resource.lblPanchSamiti
                dvBlock.Visible = True
                If ddlBlk.Items.Count = 1 Then
                    objCommonFunctions.PopulateListForEVMAllocation(lbox_mapped_in_ward, ddlBlk.SelectedValue, rbUnitType.SelectedValue, "M")
                    lblMappingCount.Text = IIf(myLang = "hi-IN", ddlBlk.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
                End If
                dvPanch.Visible = False
                dvWard.Visible = False

                ddlPanch.DataSource = New DataTable
                ddlPanch.DataBind()

                ddlConsWard.DataSource = New DataTable
                ddlConsWard.DataBind()
            Case "002" 'ULB
                lblPanch.Text = Resources.Resource.lblULB
                dvBlock.Visible = False
                ddlBlk.SelectedIndex = 1
                objCommonFunctions.PopulatePriBlocks(ddlPanch, ddlBlk.SelectedValue, rbPriPost.SelectedValue)

                If ddlPanch.Items.Count = 1 Then
                    objCommonFunctions.PopulateListForEVMAllocation(lbox_mapped_in_ward, ddlPanch.SelectedValue, rbUnitType.SelectedValue, "M")
                    lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
                End If
                dvPanch.Visible = True
                dvWard.Visible = False
            Case "003" 'MC
                lblBlk.Text = Resources.Resource.lblMC
                dvBlock.Visible = True
                If ddlBlk.Items.Count = 1 Then
                    objCommonFunctions.PopulateListForEVMAllocation(lbox_mapped_in_ward, ddlBlk.SelectedValue, rbUnitType.SelectedValue, "M")
                    lblMappingCount.Text = IIf(myLang = "hi-IN", ddlBlk.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
                End If
                dvPanch.Visible = False
                dvWard.Visible = False
        End Select
        Select Case userLevel
            Case "002", "003", "004", "005"
                lblMappingCount.Text = IIf(myLang = "hi-IN", ddlBlk.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)

            Case Else

        End Select
    End Sub

    Private Sub ddlConsWard_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlConsWard.SelectedIndexChanged
        If lbox_unmapped_panchayat.Items.Count = 0 Then
            lbl_error.Text = GetLocalResourceObject("NoUnitsForAllocation")
            save_button.Enabled = False
            lbl_error.Visible = True
        Else
            save_button.Enabled = True
            lbl_error.Visible = False
        End If
        If ddlPanch.SelectedValue = "00" Then
            lblMappingCount.Text = ""
        Else
            objCommonFunctions.PopulateListForEVMAllocation(lbox_mapped_in_ward, ddlPanch.SelectedValue + Right("000" + ddlConsWard.SelectedValue, 3), rbUnitType.SelectedValue, "M")
            lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के मतदान केंद्र " + ddlConsWard.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, " " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
        End If
    End Sub
End Class
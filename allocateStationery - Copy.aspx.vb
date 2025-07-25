Public Class allocateStationery
    Inherits SetCulture
    Dim objCommonFunctions As New commonFunctions()
    Dim MyTable As New DataTable
    Dim userName, userLevel, dis_code, myLang As String
    Dim unitCounts As Int64
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
            Dim ddl As New DropDownList
            ' check for MC,ULB, PRI below 
            objCommonFunctions.PopulateSECCommonMasterValues(rbPriPost, "ELECTIONFOR")
            objCommonFunctions.PopulateCommonMasterDropdown(ddl, "STATIONERY", rbPriPost.SelectedValue)
            rbUnitType.DataSource = ddl.DataSource
            rbUnitType.DataTextField = ddl.DataTextField
            rbUnitType.DataValueField = ddl.DataValueField
            rbUnitType.DataBind()
            rbUnitType.SelectedIndex = 0
            lblBlk.Text = Resources.Resource.Mc.ToString
            lblPanch.Text = Resources.Resource.lblMCWard.ToString
            If userLevel <> "000" Then 'Ask state user to select district , populate district for other users
                ddlDis.Items.Clear()
                ddlDis.Items.Insert(0, New ListItem(IIf(myLang = "hi-IN", Session("gblDistrictNameLocal"), Session("gblDistrictName")), dis_code))
                ddlDis.SelectedIndex = 0
                objCommonFunctions.PopulatePriBlocks(ddlBlk, dis_code, "003")
                If ddlBlk.Items.Count = 1 Then
                    ddlBlk.SelectedIndex = 0
                    If userLevel = "001" Or userLevel = "006" Then
                        MyTable = objCommonFunctions.PopulateStationeryAllocationDataTable(ddlBlk.SelectedValue, rbUnitType.SelectedValue, "MC")
                        If MyTable.Rows.Count > 0 Then
                            existing_grid.DataSource = MyTable
                            existing_grid.DataBind()
                            lbl_error.Visible = False
                            lblMappingCount.Text = IIf(myLang = "hi-IN", ddlBlk.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + unitCounts.ToString, rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + unitCounts.ToString)
                        Else
                            lbl_error.Visible = True
                            lbl_error.Text = GetLocalResourceObject("errNoRecord")
                        End If
                        dvPanch.Visible = False
                        dvWard.Visible = False
                    Else
                        objCommonFunctions.populateMCWards(ddlPanch, ddlBlk.SelectedValue)
                        lblMappingCount.Text = ""
                        If ddlPanch.Items.Count = 1 Then
                            MyTable = objCommonFunctions.PopulateStationeryAllocationDataTable(ddlPanch.SelectedValue, rbUnitType.SelectedValue, "MC")
                            If MyTable.Rows.Count > 0 Then
                                existing_grid.DataSource = MyTable
                                existing_grid.DataBind()
                                lbl_error.Visible = False
                                lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + unitCounts.ToString, rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + unitCounts.ToString)
                            Else
                                lbl_error.Visible = True
                                lbl_error.Text = GetLocalResourceObject("errNoRecord")
                            End If
                        End If
                        dvPanch.Visible = True
                        dvWard.Visible = False

                    End If
                End If
            Else
                objCommonFunctions.PopulateDistricts(ddlDis)
                ddlDis.Enabled = True
                dvBlock.Visible = False
                dvPanch.Visible = False
                dvWard.Visible = False
                lbl_error.Visible = False
                lblStatus.Visible = False
            End If
        End If
    End Sub

    Protected Sub save_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save_button.Click
        Dim flag As Boolean = True
        Dim errorStr As String = ""
        Dim Item As New ListItem

        If userLevel = "000" Then
            If ddlDis.SelectedValue = "0" Then
                flag = False
                errorStr = errorStr + "<li>" + GetLocalResourceObject("errNoDis") + "</li>"
            End If
        ElseIf ddlBlk.SelectedValue = "0" Then
            flag = False
            errorStr = errorStr + "<li>" + GetLocalResourceObject("errNoBlock") + "</li>"
        End If
        If flag = False Then
            lbl_error.Text = errorStr
            lbl_error.Visible = True
        Else

            'saveStationeryMapping
            Dim tbl_StaioneryMapping As New DataTable
            tbl_StaioneryMapping.Columns.Add(New DataColumn("UnitID", GetType(Integer)))
            tbl_StaioneryMapping.Columns.Add(New DataColumn("AllotedQuantity", GetType(String)))

            Dim drStationeryMapping As DataRow = Nothing

            For Each dr As GridViewRow In existing_grid.Rows
                Dim id As Label = DirectCast(dr.FindControl("id"), Label)
                Dim qty As TextBox = DirectCast(dr.FindControl("lblItemCounts"), TextBox)
                If Val(qty.Text.ToString) > 0 Then
                    drStationeryMapping = tbl_StaioneryMapping.NewRow()
                    drStationeryMapping("UnitID") = id.Text
                    drStationeryMapping("AllotedQuantity") = qty.Text
                    tbl_StaioneryMapping.Rows.Add(drStationeryMapping)
                End If
            Next

            If flag Then
                Dim mytable As New DataTable
                Dim mycommand As New SqlClient.SqlCommand
                Dim MyInterface As ErmsInterface.hbInterface
                Dim Implementer As New ErmsBLL
                '    'This is to save the stationery allocation
                Try
                    MyInterface = Implementer
                    mycommand.Parameters.Clear()
                    ' @UnitID,@UnitType,@BlkCode,@AllottedBy
                    mycommand.Parameters.AddWithValue("@tbl_StaioneryMapping", tbl_StaioneryMapping)
                    mycommand.Parameters.AddWithValue("@AllottedBy", userName)
                    Dim AllotedTo As String = ""
                    Select Case userLevel
                        Case "000"
                            AllotedTo = ddlDis.SelectedValue & "000000000"
                        Case "001", "006"
                            AllotedTo = ddlBlk.SelectedValue & "000000"
                        Case "002", "003"
                            AllotedTo = ddlPanch.SelectedValue & "000"
                    End Select
                    mycommand.Parameters.AddWithValue("@AllotedTo", AllotedTo)

                    mytable = MyInterface.INSERTData(mycommand, "saveStationeryMapping")

                    If mytable.Rows.Count > 0 Then
                        If Not String.IsNullOrEmpty(mytable.Rows(0)(0).ToString) Then
                            lbl_error.Text = mytable.Rows(0)(0).ToString
                            lbl_error.Visible = True
                            lblStatus.Visible = False
                            Exit Sub
                        End If
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
        End If
    End Sub
    Protected Sub cancel_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_button.Click
        Response.Redirect("allocateStationery.aspx")
    End Sub

    Protected Sub rbPriPost_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As New DropDownList
        objCommonFunctions.PopulateCommonMasterDropdown(ddl, "STATIONERY", rbPriPost.SelectedValue)
        rbUnitType.DataSource = ddl.DataSource
        rbUnitType.DataTextField = ddl.DataTextField
        rbUnitType.DataValueField = ddl.DataValueField
        rbUnitType.DataBind()
        rbUnitType.SelectedIndex = 0

        lblMappingCount.Text = ""
        existing_grid.DataSource = New DataTable
        existing_grid.DataBind()

        '' ddlBlk_SelectedIndexChanged(ddlBlk, Nothing)
        lbl_error.Visible = False
        lblStatus.Visible = False
        If userLevel <> "000" Then

            Select Case rbPriPost.SelectedValue
                Case "001" 'gp
                    dvBlock.Visible = True
                    lblBlk.Text = Resources.Resource.lblPanchSamiti.ToString
                    lblPanch.Text = Resources.Resource.lblPanchayat.ToString
                    objCommonFunctions.PopulatePriBlocks(ddlBlk, ddlDis.SelectedValue, rbPriPost.SelectedValue)
                    If ddlBlk.Items.Count > 1 Then
                        ddlBlk.Items(0).Text = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "पंचायत समिति चुनें", "Choose Panchayat Samiti")
                    End If
                    ddlBlk_SelectedIndexChanged(sender, e)
                    dvPanch.Visible = False
                Case "002" 'ulb
                    dvBlock.Visible = False
                    lblPanch.Text = Resources.Resource.ResourceManager.GetString("Ulb")

                    ddlBlk.SelectedIndex = -1
                    dvPanch.Visible = True
                    If Len(ddlDis.SelectedValue) = 4 Then
                        objCommonFunctions.PopulatePriBlocks(ddlPanch, ddlDis.SelectedValue, rbPriPost.SelectedValue)
                        ddlBlk = ddlPanch
                        ddlPanch.Items(0).Text = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "शहरी स्थानीय निकाय चुनें", "Choose ULB")
                    End If
                Case "003" 'mc
                    lblBlk.Text = Resources.Resource.Mc.ToString
                    lblPanch.Text = Resources.Resource.lblMCWard.ToString
                    dvBlock.Visible = True
                    dvPanch.Visible = True
                    If userLevel = "001" Or userLevel = "006" Then
                        MyTable = objCommonFunctions.PopulateStationeryAllocationDataTable(ddlBlk.SelectedValue, rbUnitType.SelectedValue, "MC")
                        If MyTable.Rows.Count > 0 Then
                            existing_grid.DataSource = MyTable
                            existing_grid.DataBind()
                            lbl_error.Visible = False
                            lblMappingCount.Text = IIf(myLang = "hi-IN", ddlBlk.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + unitCounts.ToString, rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + unitCounts.ToString)
                        Else
                            lbl_error.Visible = True
                            lbl_error.Text = GetLocalResourceObject("errNoRecord")
                        End If
                        objCommonFunctions.PopulatePriBlocks(ddlBlk, ddlDis.SelectedValue, rbPriPost.SelectedValue)
                        If ddlBlk.Items.Count > 1 Then
                            ddlBlk.Items(0).Text = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "नगर निगम वार्ड चुनें", "Choose Municipal Corporations")
                        Else
                            ddlBlk_SelectedIndexChanged(sender, e)
                        End If
                        ddlBlk.SelectedIndex = 0
                        dvPanch.Visible = False
                        dvWard.Visible = False
                    Else
                        If Len(ddlDis.SelectedValue) = 4 Then
                            objCommonFunctions.PopulatePriBlocks(ddlBlk, ddlDis.SelectedValue, rbPriPost.SelectedValue)
                            If ddlBlk.Items.Count > 1 Then
                                ddlBlk.Items(0).Text = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "नगर निगम वार्ड चुनें", "Choose Municipal Corporations")
                            Else
                                ddlBlk_SelectedIndexChanged(sender, e)
                            End If
                            ddlBlk.SelectedIndex = 0
                        End If
                    End If
                    ddlConsWard.Items.Clear()
                    dvWard.Visible = False
            End Select
        End If


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
    End Sub

    Protected Sub ddlBlk_SelectedIndexChanged(sender As Object, e As EventArgs)
        If ddlPanch.Items.Count > 0 Then
            ddlPanch.Items.Clear()
            ddlConsWard.Items.Clear()
        End If

        If ddlBlk.SelectedValue <> "00" And ddlBlk.SelectedValue <> "" Then
            ddlPanch.Items.Clear()

            If rbPriPost.SelectedValue = "003" Then
                objCommonFunctions.populateMCWards(ddlPanch, ddlBlk.SelectedValue)
                ddlPanch.SelectedIndex = 0
                dvPanch.Visible = True
                Exit Sub
            ElseIf rbPriPost.SelectedValue = "002" Then
                objCommonFunctions.PopulatePanchayats(ddlPanch, ddlBlk.SelectedValue)
                dvPanch.Visible = False
            Else
                dvPanch.Visible = False
                rbUnitType_SelectedIndexChanged(sender, e)
            End If


        Else
            ddlConsWard.Items.Clear()
            ddlPanch.Items.Clear()
        End If

    End Sub

    Protected Sub rbUnitType_SelectedIndexChanged(sender As Object, e As EventArgs)
        existing_grid.DataSource = New DataTable
        existing_grid.DataBind()


        If ddlPanch.SelectedValue = "00" Then
            lblMappingCount.Text = ""
        Else
            If rbPriPost.SelectedValue = "001" Then
                MyTable = objCommonFunctions.PopulateStationeryAllocationDataTable(ddlBlk.SelectedValue, rbUnitType.SelectedValue, "GPYT")
            Else
                MyTable = objCommonFunctions.PopulateStationeryAllocationDataTable(ddlPanch.SelectedValue, rbUnitType.SelectedValue, IIf(rbPriPost.SelectedValue = "002", "NPLK", "MC"))

            End If
            If MyTable.Rows.Count > 0 Then
                existing_grid.DataSource = MyTable
                existing_grid.DataBind()
                lbl_error.Visible = False
            Else
                lbl_error.Visible = True
                lbl_error.Text = GetLocalResourceObject("errNoRecord")
            End If
            ' lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
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
    Private Sub ddlPanch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPanch.SelectedIndexChanged
        rbUnitType_SelectedIndexChanged(sender, e)
    End Sub

    Protected Sub ddlDis_SelectedIndexChanged(sender As Object, e As EventArgs)
        If userLevel = "000" Then
            dvBlock.Visible = False
            dvPanch.Visible = False
            dvWard.Visible = False

            existing_grid.DataSource = New DataTable
            existing_grid.DataBind()


            If ddlPanch.SelectedValue = "00" Then
                lblMappingCount.Text = ""
            Else
                Dim itemFor As String = ""
                Select Case rbPriPost.SelectedValue
                    Case "001"
                        itemFor = "GPYT"
                    Case "002"
                        itemFor = "NPLK"
                    Case "003"
                        itemFor = "MC"
                    Case Else
                        itemFor = rbPriPost.SelectedValue
                End Select
                If ddlDis.SelectedValue = "0" Then

                End If
                MyTable = objCommonFunctions.PopulateStationeryAllocationDataTable(ddlDis.SelectedValue, rbUnitType.SelectedValue, itemFor)

                If MyTable.Rows.Count > 0 Then
                    existing_grid.DataSource = MyTable
                    existing_grid.DataBind()
                    lbl_error.Visible = False
                Else
                    lbl_error.Visible = True
                    lbl_error.Text = GetLocalResourceObject("errNoRecord")
                End If
                ' lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
            End If

        End If
    End Sub

    Protected Sub existing_grid_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)

    End Sub

    Private Sub existing_grid_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles existing_grid.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            unitCounts = 0

        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblItemCounts As TextBox = DirectCast(e.Row.FindControl("lblItemCounts"), TextBox)
            Dim lblCnt As Label = DirectCast(e.Row.FindControl("lblCnt"), Label)


            unitCounts = unitCounts + IIf(lblItemCounts.Text = "" Or lblItemCounts.Text = "0", 0, lblItemCounts.Text)
            lblCnt.Text = unitCounts.ToString
            If Val(lblItemCounts.Text) = 0 Then
                lblItemCounts.Text = ""
            End If

            If Val(lblCnt.Text) = 0 Then
                lblCnt.Text = ""
            End If
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            If userLevel = "000" Then
                lblMappingCount.Text = IIf(myLang = "hi-IN", ddlDis.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + unitCounts.ToString, rbUnitType.SelectedItem.Text + " Allocated to " + ddlDis.SelectedItem.Text + ": " + unitCounts.ToString)
            ElseIf userLevel = "001" Or userLevel = "006" Then
                lblMappingCount.Text = IIf(myLang = "hi-IN", ddlBlk.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + unitCounts.ToString, rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + unitCounts.ToString)
            Else
                If rbPriPost.SelectedValue = "001" Then
                    lblMappingCount.Text = IIf(myLang = "hi-IN", ddlBlk.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + unitCounts.ToString, rbUnitType.SelectedItem.Text + " Allocated to " + ddlBlk.SelectedItem.Text + ": " + unitCounts.ToString)

                Else
                    lblMappingCount.Text = IIf(myLang = "hi-IN", ddlPanch.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + unitCounts.ToString, rbUnitType.SelectedItem.Text + " Allocated to " + ddlPanch.SelectedItem.Text + ": " + unitCounts.ToString)
                End If
            End If
        End If
    End Sub

    Protected Sub existing_grid_RowCommand(sender As Object, e As GridViewCommandEventArgs)

    End Sub


End Class
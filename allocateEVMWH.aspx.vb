Public Class allocateEVMWH
    Inherits SetCulture
    Dim objCommonFunctions As New commonFunctions()
    Dim MyTable As New DataTable
    Dim userName, userLevel, dis_code, myLang, wh_code As String
    Const keyValue As String = "Sec&NicHP@22"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        userName = Session("UserID")
        dis_code = Session("gblDistrictCode")
        wh_code = dis_code + Session("gblBlockCode")
        userLevel = Session("UserLevel")

        Response.Cache.SetLastModified(DateTime.Now)
        Response.Cache.SetAllowResponseInBrowserHistory(False)
        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        Response.Cache.SetNoStore()
        myLang = Session("MyLanguage").ToString
        If Not IsPostBack Then
            Dim UnitType As String = "AL"
            If userLevel = "012" Or userLevel = "013" Then
                If Session("gblBlockCode").StartsWith("1") Then
                    UnitType = "BB"
                ElseIf Session("gblBlockCode").StartsWith("0") Then
                    UnitType = "BC"
                End If
            End If
            objCommonFunctions.PopulateUnitTypes(rbUnitType, UnitType)
            objCommonFunctions.PopulateDistricts(ddlDis)
            ddlDis.SelectedIndex = ddlDis.Items.IndexOf(ddlDis.Items.FindByValue(dis_code))
            lblDis.Visible = True
            ddlDis.Visible = True

            lblWh.Visible = False
            ddlWH.Visible = False
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

            ddlDis_SelectedIndexChanged(sender, e)
        End If
    End Sub

    Protected Sub save_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save_button.Click
        Dim flag As Boolean = True
        Dim errorStr As String = ""
        Dim Item As New ListItem

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
            drEvmMapping("BlkCode") = ddlDis.SelectedValue + "000" 'wh_code 
            drEvmMapping("PanchCode") = ddlDis.SelectedValue + "000000"
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
                mycommand.Parameters.AddWithValue("@tbl_EvmMapping", tbl_EvmMapping)
                mycommand.Parameters.AddWithValue("@AllottedBy", userName)
                mycommand.Parameters.AddWithValue("@AllottedFrom", "130000000" + Right(wh_code, 4))
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
        Response.Redirect("allocateEVMWH.aspx")
    End Sub
    Protected Sub rbUnitType_SelectedIndexChanged(sender As Object, e As EventArgs)
        lbox_unmapped_panchayat.Items.Clear()
        lbox_mapped_in_ward.Items.Clear()
        ' load units below
        objCommonFunctions.PopulateListForEVMAllocation(lbox_unmapped_panchayat, ddlDis.SelectedValue, rbUnitType.SelectedValue, "U")
        If lbox_unmapped_panchayat.Items.Count = 0 Then
            lbl_error.Text = GetLocalResourceObject("NoUnitsForAllocation")
            save_button.Enabled = False
            lbl_error.Visible = True
        Else
            save_button.Enabled = True
            lbl_error.Visible = False
        End If
        ddlDis_SelectedIndexChanged(sender, e)
        rbDis_wh_SelectedIndexChanged(sender, e)
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

    Protected Sub ddlDis_SelectedIndexChanged(sender As Object, e As EventArgs)
        ' objCommonFunctions.PopulateListForEVMAllocation(lbox_unmapped_panchayat, ddlDis.SelectedValue, rbUnitType.SelectedValue, "U")
        If ddlDis.SelectedValue = "00" Or ddlDis.SelectedValue = "" Then
            lblMappingCount.Text = ""
        Else
            objCommonFunctions.PopulateListForEVMAllocation(lbox_mapped_in_ward, ddlDis.SelectedValue, rbUnitType.SelectedValue, "M")
            lblMappingCount.Text = IIf(myLang = "hi-IN", ddlDis.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlDis.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)
        End If
    End Sub

    Protected Sub rbDis_wh_SelectedIndexChanged(sender As Object, e As EventArgs)
        lblMappingCount.Text = ""
        If rbDis_wh.SelectedValue = "W" Then
            objCommonFunctions.PopulateWareHouses(ddlWH, ddlDis.SelectedValue, rbUnitType.SelectedValue, "0000") ' load all warehouses for allocation
            If ddlWH.Items.Count = 0 Then
                lbl_error.Text = GetLocalResourceObject("NoWarehouse")
                save_button.Enabled = False
                lbl_error.Visible = True
                Exit Sub
            End If
            ddlWH.SelectedIndex = 0

            ' objCommonFunctions.PopulateListForEVMAllocationAtWarehouse(lbox_unmapped_panchayat, Left(wh_code, 4), Right(wh_code, 4), ddlWH.SelectedValue, rbUnitType.SelectedValue, "U")
            lblDis.Visible = False
            ddlDis.Visible = False

            lblWh.Visible = True
            ddlWH.Visible = True
            ' objCommonFunctions.PopulateListForEVMAllocationAtWarehouse(lbox_mapped_in_ward, Left(wh_code, 4), Right(wh_code, 4), ddlWH.SelectedValue, rbUnitType.SelectedValue, "M")

        Else
            objCommonFunctions.PopulateDistricts(ddlDis)
            ddlDis.SelectedIndex = ddlDis.Items.IndexOf(ddlDis.Items.FindByValue(dis_code))

            'objCommonFunctions.PopulateListForEVMAllocation(lbox_unmapped_panchayat, ddlDis.SelectedValue, rbUnitType.SelectedValue, "U")
            objCommonFunctions.PopulateListForEVMAllocationAtWarehouse(lbox_unmapped_panchayat, Left(wh_code, 4), Right(wh_code, 4), "0000", rbUnitType.SelectedValue, "U")
            lblDis.Visible = True
            ddlDis.Visible = True

            lblWh.Visible = False
            ddlWH.Visible = False
            objCommonFunctions.PopulateListForEVMAllocationAtWarehouse(lbox_mapped_in_ward, Left(wh_code, 4), Right(wh_code, 4), "0000", rbUnitType.SelectedValue, "M")

        End If

        If lbox_unmapped_panchayat.Items.Count = 0 Then
            lbl_error.Text = GetLocalResourceObject("NoUnitsForAllocation")
            save_button.Enabled = False
            lbl_error.Visible = True
        Else
            save_button.Enabled = True
            lbl_error.Visible = False
        End If
    End Sub

    Protected Sub ddlWH_SelectedIndexChanged(sender As Object, e As EventArgs)
        If ddlWH.SelectedValue <> "00" Then
            objCommonFunctions.PopulateListForEVMAllocationAtWarehouse(lbox_mapped_in_ward, Left(wh_code, 4), Right(wh_code, 4), ddlWH.SelectedValue, rbUnitType.SelectedValue, "M")

            lblMappingCount.Text = IIf(myLang = "hi-IN", "वेयरहाउस " + ddlWH.SelectedItem.Text + " के लिए आवंटित " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, rbUnitType.SelectedItem.Text + " Allocated to Warehouse " + ddlWH.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)

        End If
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

        lblMappingCount.Text = IIf(myLang = "hi-IN", ddlDis.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlDis.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)

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
        lblMappingCount.Text = IIf(myLang = "hi-IN", ddlDis.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlDis.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)

    End Sub
    Protected Sub right_button_all_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        For Each item As ListItem In lbox_unmapped_panchayat.Items
            lbox_mapped_in_ward.Items.Add(item)
        Next
        lbox_unmapped_panchayat.Items.Clear()
        lblMappingCount.Text = IIf(myLang = "hi-IN", ddlDis.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlDis.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)

    End Sub
    Protected Sub left_button_all_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles left_button_all.Click
        For Each item As ListItem In lbox_mapped_in_ward.Items
            lbox_unmapped_panchayat.Items.Add(item)
        Next
        lbox_mapped_in_ward.Items.Clear()
        lblMappingCount.Text = IIf(myLang = "hi-IN", ddlDis.SelectedItem.Text + " के लिए आवंटित ईवीएम " + rbUnitType.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString, "EVM " + rbUnitType.SelectedItem.Text + " Allocated to " + ddlDis.SelectedItem.Text + ": " + lbox_mapped_in_ward.Items.Count.ToString)

    End Sub

End Class
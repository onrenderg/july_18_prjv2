Imports System.Drawing

Public Class updateStationeryStock
    Inherits SetCulture
    Dim objCommonFunctions As New commonFunctions()
    Dim MyTable As New DataTable
    Dim userName, userLevel, dis_code, myLang As String
    Dim unitCounts As Int64
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

        userName = Session("UserID")
        dis_code = Session("gblDistrictCode")
        userLevel = Session("UserLevel")
        myLang = Session("MyLanguage").ToString

        If Not IsPostBack Then
            Dim ddl As New DropDownList
            objCommonFunctions.PopulateSECCommonMasterValues(rbPriPost, "ELECTIONFOR", "000")
            objCommonFunctions.PopulateCommonMasterDropdown(ddl, "STATIONERY", rbPriPost.SelectedValue)
            rbUnitType.DataSource = ddl.DataSource
            rbUnitType.DataTextField = ddl.DataTextField
            rbUnitType.DataValueField = ddl.DataValueField
            rbUnitType.DataBind()
            rbUnitType.SelectedIndex = 0

            lbl_error.Visible = False
            lblStatus.Visible = False
            If userLevel = "000" Then 'Ask state user to select district , populate district for other users
                MyTable = objCommonFunctions.PopulateStationeryAvailableDataTable("13", rbUnitType.SelectedValue, "MC")
                If MyTable.Rows.Count > 0 Then
                    existing_grid.DataSource = MyTable
                    existing_grid.DataBind()
                    lbl_error.Visible = False
                Else
                    lbl_error.Visible = True
                    lbl_error.Text = GetLocalResourceObject("errNoRecord")
                End If
            ElseIf userLevel = "001" Or userLevel = "006" Then
                ddlDis.Items.Clear()
                ddlDis.Items.Insert(0, New ListItem(IIf(myLang = "hi-IN", Session("gblDistrictNameLocal"), Session("gblDistrictName")), dis_code))
                ddlDis.SelectedIndex = 0

                MyTable = objCommonFunctions.PopulateStationeryAvailableDataTable(dis_code, rbUnitType.SelectedValue, "MC")
                If MyTable.Rows.Count > 0 Then
                    existing_grid.DataSource = MyTable
                    existing_grid.DataBind()
                    lbl_error.Visible = False
                Else
                    lbl_error.Visible = True
                    lbl_error.Text = GetLocalResourceObject("errNoRecord")
                End If
            Else
                lbl_error.Visible = True
                lbl_error.Text = GetLocalResourceObject("errNotAllowed")
            End If
        End If
    End Sub

    Protected Sub save_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save_button.Click

        Dim tbl_StationeryMapping As New DataTable
        tbl_StationeryMapping.Columns.Add(New DataColumn("UnitID", GetType(Integer)))
        tbl_StationeryMapping.Columns.Add(New DataColumn("AllotedQuantity", GetType(String)))
        tbl_StationeryMapping.Columns.Add(New DataColumn("FromSrNo", GetType(Integer)))
        tbl_StationeryMapping.Columns.Add(New DataColumn("ToSrNo", GetType(Integer)))

        Dim drStationeryMapping As DataRow = Nothing

        For Each dr As GridViewRow In existing_grid.Rows
            Dim id As Label = DirectCast(dr.FindControl("id"), Label)
            Dim qty As TextBox = DirectCast(dr.FindControl("lblItemCounts"), TextBox)
            Dim FromSrNo As TextBox = DirectCast(dr.FindControl("lblFromSrNo"), TextBox)
            Dim ToSrNo As TextBox = DirectCast(dr.FindControl("lblToSrNo"), TextBox)
            If Val(qty.Text.ToString) > 0 Then
                drStationeryMapping = tbl_StationeryMapping.NewRow()
                drStationeryMapping("UnitID") = id.Text
                drStationeryMapping("AllotedQuantity") = qty.Text
                drStationeryMapping("FromSrNo") = Val(FromSrNo.Text)
                drStationeryMapping("ToSrNo") = Val(ToSrNo.Text)
                If Val(FromSrNo.Text) > 0 Then
                    If Val(ToSrNo.Text) - Val(FromSrNo.Text) + 1 <> Val(qty.Text) Then
                        lbl_error.Visible = True
                        lbl_error.Text = "Invalid Count of Items! Count of Items (From Sr.No. to To Sr.No.) must be equal to Total Available Stock. " 'GetLocalResourceObject("errInvalidCount")
                        qty.BorderColor = Color.Red
                        FromSrNo.BorderColor = Color.Red
                        ToSrNo.BorderColor = Color.Red
                        qty.Focus()
                        Exit Sub
                    Else
                        qty.BorderColor = Color.Empty
                        FromSrNo.BorderColor = Color.Empty
                        ToSrNo.BorderColor = Color.Empty
                    End If
                End If
                tbl_StationeryMapping.Rows.Add(drStationeryMapping)
            End If
        Next

        Dim mytable As New DataTable
        Dim mycommand As New SqlClient.SqlCommand
        Dim MyInterface As ErmsInterface.hbInterface
        Dim Implementer As New ErmsBLL

        Try
            MyInterface = Implementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@tbl_StaioneryMapping", tbl_StationeryMapping)
            mycommand.Parameters.AddWithValue("@itemType", rbUnitType.SelectedValue)
            mycommand.Parameters.AddWithValue("@priType", rbPriPost.SelectedValue)
            mycommand.Parameters.AddWithValue("@userlevel", userLevel)
            mycommand.Parameters.AddWithValue("@enteredBy", userName)
            If userLevel = "000" Then
                mycommand.Parameters.AddWithValue("@disCode", "13")
            Else
                mycommand.Parameters.AddWithValue("@disCode", dis_code)
            End If

            mytable = MyInterface.INSERTData(mycommand, "UpdateStationeryStock")

            If mytable.Rows.Count > 0 Then
                If mytable.Rows(0)(0).ToString = "updated" Then
                    lbl_error.Visible = False
                    lblStatus.Text = GetLocalResourceObject("msgSave").ToString
                    lblStatus.Visible = True
                Else
                    lbl_error.Text = mytable.Rows(0)(0).ToString
                    lbl_error.Visible = True
                    lblStatus.Visible = False
                End If
            End If
        Catch ex As Exception
            lbl_error.Text = ex.Message
            lbl_error.Visible = True
            lblStatus.Visible = False
        End Try

    End Sub
    Protected Sub cancel_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_button.Click
        Response.Redirect("updateStationeryStock.aspx")
    End Sub

    Protected Sub rbPriPost_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddl As New DropDownList
        objCommonFunctions.PopulateCommonMasterDropdown(ddl, "STATIONERY", rbPriPost.SelectedValue)
        rbUnitType.DataSource = ddl.DataSource
        rbUnitType.DataTextField = ddl.DataTextField
        rbUnitType.DataValueField = ddl.DataValueField
        rbUnitType.DataBind()
        rbUnitType.SelectedIndex = 0

        existing_grid.DataSource = New DataTable
        existing_grid.DataBind()

        ' ddlBlk_SelectedIndexChanged(ddlBlk, Nothing)
        lbl_error.Visible = False
        lblStatus.Visible = False

        If rbPriPost.SelectedValue = "001" Then
            MyTable = objCommonFunctions.PopulateStationeryAvailableDataTable(IIf(userLevel = "000", "13", dis_code), rbUnitType.SelectedValue, "GPYT")
        Else
            MyTable = objCommonFunctions.PopulateStationeryAvailableDataTable(IIf(userLevel = "000", "13", dis_code), rbUnitType.SelectedValue, IIf(rbPriPost.SelectedValue = "002", "NPLK", "MC"))
        End If
        If MyTable.Rows.Count > 0 Then
            existing_grid.DataSource = MyTable
            existing_grid.DataBind()
            lbl_error.Visible = False
        Else
            lbl_error.Visible = True
            lbl_error.Text = GetLocalResourceObject("errNoRecord")
        End If
    End Sub

    Protected Sub rbUnitType_SelectedIndexChanged(sender As Object, e As EventArgs)

        existing_grid.DataSource = New DataTable
        existing_grid.DataBind()

        If rbPriPost.SelectedValue = "001" Then
            MyTable = objCommonFunctions.PopulateStationeryAvailableDataTable(IIf(userLevel = "000", "13", dis_code), rbUnitType.SelectedValue, "GPYT")
        Else
            MyTable = objCommonFunctions.PopulateStationeryAvailableDataTable(IIf(userLevel = "000", "13", dis_code), rbUnitType.SelectedValue, IIf(rbPriPost.SelectedValue = "002", "NPLK", "MC"))
        End If
        If MyTable.Rows.Count > 0 Then
            existing_grid.DataSource = MyTable
            existing_grid.DataBind()
            lbl_error.Visible = False
        Else
            lbl_error.Visible = True
            lbl_error.Text = GetLocalResourceObject("errNoRecord")
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

    Protected Sub existing_grid_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)

    End Sub

    Private Sub existing_grid_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles existing_grid.RowDataBound
        If rbUnitType.SelectedValue = "PNKSEAL" Or rbUnitType.SelectedValue = "RCBOOK" Or rbUnitType.SelectedValue = "BALLOT" Then
            e.Row.Cells(3).Visible = True
            e.Row.Cells(4).Visible = True
        Else
            e.Row.Cells(3).Visible = False
            e.Row.Cells(4).Visible = False
        End If

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
        End If
    End Sub

    Protected Sub existing_grid_RowCommand(sender As Object, e As GridViewCommandEventArgs)

    End Sub
End Class
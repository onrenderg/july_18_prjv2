Public Class viewLocation
    Inherits SetCulture
    Dim objCommonFunctions As New commonFunctions()
    Dim MyTable As New DataTable
    Dim userName, userLevel, dis_code, blk_code, panch_code, myLang, wh_code As String


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Response.Cache.SetLastModified(DateTime.Now)
        Response.Cache.SetAllowResponseInBrowserHistory(False)
        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        Response.Cache.SetNoStore()
        myLang = Session("MyLanguage").ToString

        userName = Session("UserID")
        dis_code = Session("gblDistrictCode")
        blk_code = Session("gblBlockCode")
        panch_code = Session("gblPanchayatCode")
        userLevel = Session("UserLevel")

        If Not IsPostBack Then

            Select Case userLevel
                Case "000"
                    dis_code = "13"
                    wh_code = "0000"
                Case "012", "013"
                    wh_code = dis_code + Session("gblBlockCode")
               ' Case "001", "006"
                Case "002", "003"
                    dis_code = blk_code
                Case "004", "005"
                    dis_code = panch_code
                Case Else
                    wh_code = "0000"
            End Select
            objCommonFunctions.PopulateUnitTypes(rbUnitType)
            rbUnitType_SelectedIndexChanged(sender, e)
        End If
        ' AddHandler existing_grid.RowCommand, AddressOf existing_grid_RowCommand
    End Sub

    Protected Sub rbUnitType_SelectedIndexChanged(sender As Object, e As EventArgs)
        Select Case userLevel
            Case "000"
                dis_code = "13"
                wh_code = "0000"
            Case "012", "013"
                wh_code = dis_code + Session("gblBlockCode")
               ' Case "001", "006"
            Case "002", "003"
                dis_code = blk_code
            Case "004", "005"
                dis_code = panch_code
            Case Else
                wh_code = "0000"
        End Select
        MyTable = objCommonFunctions.getEVMLocationList(rbUnitType.SelectedValue, dis_code, wh_code)
        '@Device_Type varchar(3),	@dis_code char(4)='0000',	@wh_code char(4)='0000'

        If MyTable.Rows.Count > 0 Then
            existing_grid.DataSource = MyTable
            existing_grid.DataBind()
            Session("mapData") = MyTable
            lbl_error.Visible = False
        Else
            existing_grid.DataSource = Nothing
            existing_grid.DataBind()
            Session("mapData") = Nothing

            lbl_error.Text = GetLocalResourceObject("lblNoRow.Text")
            lbl_error.Visible = True
        End If
    End Sub

    'Private Sub existing_grid_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles existing_grid.RowCommand
    '    'Dim rowIndex As Int16 = Convert.ToInt32(e.CommandArgument.ToString)
    '    'If e.CommandName = "map" Then
    '    '    Dim row As GridViewRow = existing_grid.Rows(rowIndex)

    '    '    ' Get latitude and longitude from the row
    '    '    Dim latitude As String = DirectCast(row.FindControl("Latitude"), Label).Text
    '    '    Dim longitude As String = DirectCast(row.FindControl("Longitude"), Label).Text
    '    '    Dim srno As String = DirectCast(row.FindControl("srno"), Label).Text


    '    '    Dim dt As New DataTable
    '    '    dt.Columns.Add("Latitude")
    '    '    dt.Columns.Add("Longitude")
    '    '    dt.Columns.Add("title")
    '    '    Dim dr As DataRow = dt.NewRow()
    '    '    dr(0) = latitude
    '    '    dr(1) = longitude
    '    '    dr(2) = srno
    '    '    dt.Rows.Add(dr)
    '    '    dr.AcceptChanges()
    '    '    rptMarkers.DataSource = dt
    '    '    rptMarkers.DataBind()
    '    '    ShowMap(latitude, longitude)
    '    'End If
    'End Sub
    'Private Sub ShowMap(latitude As String, longitude As String)
    '    ' Use JavaScript to show the map
    '    Dim script As String = String.Format("showMap({0}, {1});", latitude, longitude)
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowMapScript", script, True)
    'End Sub

    Protected Sub btnShow_Click(sender As Object, e As EventArgs)
        Try
            rptMarkers.DataSource = Nothing
            rptMarkers.DataBind()
            MyTable = Session("mapData")
            If MyTable.Rows.Count > 0 Then
                Dim dv As DataView = MyTable.DefaultView

                dv.Sort = "Latitude"
                dv.RowFilter = "isnull(Latitude,'0.000000000')<>'0.000000000'"
                rptMarkers.DataSource = dv
                rptMarkers.DataBind()

            Else

                ScriptManager.RegisterStartupScript(Me, Page.GetType(), "alert", "alert('No Data Found !');", True)
            End If
        Catch ex As Exception
            Dim exM As String = ex.Message
        End Try
    End Sub

End Class
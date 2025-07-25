Public Class addStationary
    Inherits SetCulture

    Dim objCommonFunctions As New commonFunctions()
    Dim MyTable As New DataTable
    Dim userName, dis_code, userLevel As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        userName = Session("UserID")
        dis_code = Session("gblDistrictCode")
        userLevel = Session("UserLevel")
        If Not IsPostBack Then

            objCommonFunctions.PopulateCommonMasterDropdown(ddlUnitType, "StationaryType")
            objCommonFunctions.PopulateDistricts(ddlAvailableAt)
            objCommonFunctions.PopulateCommonMasterDropdown(ddlUnitCapacity, "StationaryCounts")
            txtUnitID.Text = ""
            txtUnitName.Focus()
            'MyTable = objCommonFunctions.GetEVMDataTable(dis_code)
            'If MyTable.Rows.Count > 0 Then
            '    existing_grid.DataSource = MyTable
            '    existing_grid.DataBind()
            '    dvList.Visible = True
            'Else
            '    existing_grid.DataSource = New DataTable
            '    existing_grid.DataBind()
            '    dvList.Visible = False
            'End If
        End If
    End Sub

End Class
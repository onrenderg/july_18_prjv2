Imports System.Data.SqlClient

Public Class index
    Inherits SetCulture
    Dim myLang, userName, userLevel, state_Code, dis_code, block_code, panch_code As String
    Dim mytable As DataTable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        myLang = Session("MyLanguage")
        Dim vv As String = GetLocalResourceObject("DashboardResource1.Text")
        userName = Session("UserID")
        dis_code = Session("gblDistrictCode")
        block_code = Session("gblBlockCode")
        userLevel = Session("UserLevel")
        If Not IsPostBack Then
            getInventoryCounts()
        End If
    End Sub
    Private Sub getInventoryCounts()
        Try
            Dim MyInterface1 As ErmsInterface.hbInterface
            Dim MyImplementer1 As New ErmsBLL
            Dim mycommand1 As New SqlCommand

            MyInterface1 = MyImplementer1
            mycommand1.Parameters.Clear()
            If userLevel = "000" Then
                mycommand1.Parameters.AddWithValue("@disBlkPanch", "13")
            ElseIf userLevel = "001" Or userLevel = "006" Or userLevel = "012" Or userLevel = "013" Then
                mycommand1.Parameters.AddWithValue("@disBlkPanch", dis_code)
            Else
                mycommand1.Parameters.AddWithValue("@disBlkPanch", block_code)
            End If
            mytable = MyInterface1.SELECTData(mycommand1, "getInventoryCounts")
            'MyGrid.DataSource = mytable
            'MyGrid.DataBind()
            If mytable.Rows.Count > 0 Then
                lblCu.Text =mytable.Rows(0)("CUCount").ToString
                lblBU.Text = mytable.Rows(0)("BUCount").ToString
                lblBB.Text = mytable.Rows(0)("BBCount").ToString
                lblVvpat.Text = mytable.Rows(0)("VVPatCount").ToString
                lblStnry.Text = mytable.Rows(0)("StnryCount").ToString
                lblFormPRI.Text = mytable.Rows(0)("frmPri").ToString
                lblFormULB.Text = mytable.Rows(0)("frmUlb").ToString
                lblFormMC.Text = mytable.Rows(0)("frmMC").ToString

                CntEvm.Text = mytable.Rows(0)("CUCount").ToString
                CntBB.Text = mytable.Rows(0)("BBCount").ToString
                CntStnry.Text = mytable.Rows(0)("StnryCount").ToString
                CntForms.Text = Convert.ToString(Val(mytable.Rows(0)("frmPri").ToString) + Val(mytable.Rows(0)("frmUlb").ToString) + Val(mytable.Rows(0)("frmMC").ToString))
                CntVvpat.Text = mytable.Rows(0)("VVPatCount").ToString
            End If



        Catch ex As Exception
            Dim exMsg As String = ex.Message
            'MyGrid.DataSource = New DataTable
            'MyGrid.DataBind()
        End Try
    End Sub
    'Private Sub MyGrid_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles MyGrid.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        Dim ElectionForName As Label = CType(e.Row.FindControl("ElectionForName"), Label)
    '        ' Dim lblTerm As Label = CType(e.Row.FindControl("lblTerm"), Label)
    '        Dim electionpost As Label = CType(e.Row.FindControl("electionpost"), Label)
    '        Dim electionPostLocal As Label = DirectCast(e.Row.FindControl("electionPostLocal"), Label)
    '        Dim NominationDate As Label = CType(e.Row.FindControl("NominationDate"), Label)
    '        Dim WithDrawalDates As Label = CType(e.Row.FindControl("WithDrawalDate"), Label)
    '        Dim ScrutinyDate As Label = CType(e.Row.FindControl("ScrutinyDate"), Label)
    '        Dim electionPostCode As Label = DirectCast(e.Row.FindControl("electionPostCode"), Label)
    '        Dim PollingDate As Label = CType(e.Row.FindControl("PollingDate"), Label)
    '        Dim PollingToDate As Label = CType(e.Row.FindControl("PollingToDate"), Label)
    '        Dim PollingDate2 As Label = CType(e.Row.FindControl("PollingDate2"), Label)
    '        Dim PollingTime2 As Label = CType(e.Row.FindControl("PollingTime2"), Label)
    '        Dim PollingTime3 As Label = CType(e.Row.FindControl("PollingTime3"), Label)
    '        Dim ResultDate As Label = CType(e.Row.FindControl("ResultDate"), Label)
    '        Dim ResultDate2 As Label = CType(e.Row.FindControl("ResultDate2"), Label)
    '        Dim ResultDate3 As Label = CType(e.Row.FindControl("ResultDate3"), Label)
    '        Dim ZPPSResultDate As Label = CType(e.Row.FindControl("ZPPSResultDate"), Label)
    '        If Left(PollingDate2.Text.Trim, 10) = "1900-01-01" Or Left(PollingDate2.Text.Trim, 10) = "01/01/1900" Then
    '            PollingDate2.Visible = False
    '            PollingTime2.Visible = False
    '            ResultDate2.Visible = False
    '        Else
    '            PollingDate2.Text = ", " + Left(PollingDate2.Text, 10)
    '            ResultDate2.Text = IIf(ResultDate2.Text.Trim = "", "", ", ") + ResultDate2.Text
    '        End If
    '        Dim PollingDate3 As Label = CType(e.Row.FindControl("PollingDate3"), Label)
    '        If Left(PollingDate3.Text.Trim, 10) = "1900-01-01" Or Left(PollingDate3.Text.Trim, 10) = "01/01/1900" Then
    '            PollingDate3.Visible = False
    '            PollingTime3.Visible = False
    '            ResultDate3.Visible = False
    '        Else
    '            PollingDate3.Text = ", " & Left(PollingDate3.Text, 10)
    '            ResultDate3.Text = IIf(ResultDate3.Text.Trim = "", "", ", ") + ResultDate3.Text

    '        End If
    '        If ZPPSResultDate.Text.Trim <> "" And (Left(ZPPSResultDate.Text.Trim, 10) = "1900-01-01" Or Left(ZPPSResultDate.Text.Trim, 10) = "01/01/1900" Or ZPPSResultDate.Text = ResultDate.Text) Then
    '            ZPPSResultDate.Visible = False
    '        Else
    '            ZPPSResultDate.Text = IIf(ZPPSResultDate.Text.Trim = "", "", ", ") & ZPPSResultDate.Text
    '        End If

    '        If (myLang = "hi-IN") Then
    '            electionpost.Visible = False
    '            electionPostLocal.Visible = True
    '        Else
    '            electionpost.Visible = True
    '            electionPostLocal.Visible = False
    '        End If
    '        Session("PostFor") = electionPostCode.Text
    '        ElectionForName.Text = IIf(myLang = "hi-IN", mytable.Rows(e.Row.RowIndex)("ElectionForNamelocal").ToString,
    '                                   mytable.Rows(e.Row.RowIndex)("ElectionForName").ToString)
    '        NominationDate.Text = IIf(myLang = "hi-IN", mytable.Rows(e.Row.RowIndex)("NominationDateslocal").ToString,
    '                                  mytable.Rows(e.Row.RowIndex)("NominationDates").ToString)
    '        ScrutinyDate.Text = IIf(myLang = "hi-IN", mytable.Rows(e.Row.RowIndex)("ScrutinyDateslocal").ToString,
    '                                mytable.Rows(e.Row.RowIndex)("ScrutinyDates").ToString)
    '        WithDrawalDates.Text = IIf(myLang = "hi-IN", mytable.Rows(e.Row.RowIndex)("WithDrawalDateslocal").ToString,
    '                                   mytable.Rows(e.Row.RowIndex)("WithDrawalDates").ToString)

    '    End If
    'End Sub
End Class
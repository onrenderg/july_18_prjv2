Public Class viewBBonMap
    Inherits System.Web.UI.Page

    Dim objCommonFunctions As New commonFunctions()
    Dim MyTable As New DataTable
    Dim userName, userLevel, dis_code, blk_code, myLang As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetLastModified(DateTime.Now)
        Response.Cache.SetAllowResponseInBrowserHistory(False)
        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        Response.Cache.SetNoStore()
        myLang = Session("MyLanguage").ToString
        If Not IsPostBack Then
            Dim qrText As String = Request.QueryString("qr_text")
            Dim toUserCode As String = Request.QueryString("to_user_code")

            If String.IsNullOrWhiteSpace(qrText) AndAlso String.IsNullOrWhiteSpace(toUserCode) Then
                ' Handle missing query params if needed
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMissingParams", "alert('Missing qr_text or to_user_code in URL');", True)

            Else
                ' Call your function using these values
                Dim jsonMarkers As String = GetMarkersJson(toUserCode, qrText)

                ' Register the JSON as JS variable
                Dim script As String = "var markers = [" & jsonMarkers & "];"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "loadMarkers", script, True)
            End If
        End If

        'fillMapData()
    End Sub
    Public Function GetMarkersJson(location_code As String, qr_code As String) As String
        Dim sb As New System.Text.StringBuilder()
        Dim stock_owner_ = objCommonFunctions.getBallotBoxListSummary_Unit_Wise(location_code, qr_code)

        If stock_owner_ IsNot Nothing AndAlso stock_owner_.status_code = 200 Then
            Dim available_bb_list_ As List(Of Ballot.BallotSummaryList) = TryCast(stock_owner_.data, List(Of Ballot.BallotSummaryList))

            If available_bb_list_ IsNot Nothing Then
                ' Filter: Remove items with empty or null lat/long
                available_bb_list_ = available_bb_list_.FindAll(Function(x) Not String.IsNullOrWhiteSpace(x.latitude) AndAlso Not String.IsNullOrWhiteSpace(x.longitude))

                If available_bb_list_.Count > 0 Then
                    For i As Integer = 0 To available_bb_list_.Count - 1
                        Dim item = available_bb_list_(i)

                        sb.Append("{")
                        sb.AppendFormat("""lat"": ""{0}"", ", item.latitude.Trim())
                        sb.AppendFormat("""lng"": ""{0}"", ", item.longitude.Trim())
                        sb.AppendFormat("""title"": ""{0}"", ", item.Display_Name.Replace("""", "\"""))
                        sb.AppendFormat("""description"": ""{0}""", item.qr_text.Replace("""", "\"""))
                        sb.Append("}")

                        If i < available_bb_list_.Count - 1 Then
                            sb.Append(",")
                        End If
                    Next
                Else
                    ScriptManager.RegisterStartupScript(Me, Page.GetType(), "alert", "Error: No Ballot Found", True)
                End If
            End If
        Else
            ScriptManager.RegisterStartupScript(Me, Page.GetType(), "alert", "Error: " & stock_owner_.status_message, True)
        End If

        Return sb.ToString()
    End Function


    'Private Sub fillMapData()
    '    Dim stock_owner_ = objCommonFunctions.getBallotBoxListSummary_Unit_Wise("13", "")
    '    If (stock_owner_.status_code = 200) Then
    '        Dim available_bb_list_ As List(Of Ballot.BallotSummaryList)
    '        available_bb_list_ = DirectCast(stock_owner_.data, List(Of Ballot.BallotSummaryList))
    '        rptMarkers.DataSource = available_bb_list_
    '        rptMarkers.DataBind()
    '    Else
    '        ScriptManager.RegisterStartupScript(Me, Page.GetType(), "alert", stock_owner_.status_message, True)
    '    End If
    'End Sub
    'Protected Sub btnGetData_Click(sender As Object, e As EventArgs)
    '    fillMapData()
    'End Sub
End Class
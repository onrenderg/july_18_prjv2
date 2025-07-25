Imports System.Data
Imports System.Configuration
Imports System.Web.UI
Imports System.Web.UI.WebControls

Partial Class BallotboxDashboard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim userlevelcode As String = CStr(Session("UserLevel"))
            Dim from_user_id As String = CStr(Session("UserID"))
            Dim gblDistrictName As String = CStr(Session("gblDistrictName"))

            ' Display User Info
            litUserId.Text = from_user_id
            litLocation.Text = If(userlevelcode = "000", "State Level", gblDistrictName)

            ' Validate session data
            If String.IsNullOrEmpty(userlevelcode) OrElse String.IsNullOrEmpty(from_user_id) Then
                litErrorMessage.Text = "Error: Session data for UserLevel or UserID not found. Please ensure you are logged in."
                Return
            End If

            Try
                Dim objCommonFunctions As New commonFunctions()
                Dim dt As DataTable = objCommonFunctions.GetBallotboxDashboardCounts(userlevelcode, from_user_id)

                If dt.Rows.Count > 0 Then
                    Dim row = dt.Rows(0)
                    litStateParked.Text = row("MyJurisdictionCount").ToString()
                    litMyStockParked.Text = row("MyStockCount").ToString()
                    litStateInTransit.Text = row("MyJurisdictionIntransitcount").ToString()
                    litInward.Text = row("InwardCount").ToString()
                    litOutward.Text = row("OutwardCount").ToString()
                Else
                    litErrorMessage.Text = "No dashboard counts found for this user. Stored procedure returned no rows."
                End If

            Catch ex As Exception
                litErrorMessage.Text = $"Error loading dashboard: {ex.Message}"
            End Try
        End If
    End Sub

    ' Navigation Click Events (Only redirect if value > 0)

    Protected Sub lnkStateParked_Click(sender As Object, e As EventArgs)
        Dim count As Integer
        If Integer.TryParse(litStateParked.Text, count) AndAlso count > 0 Then
            Session("actioncode") = 1
            Response.Redirect("BallotBoxDetailList.aspx")
        End If
    End Sub

    Protected Sub lnkStateInTransit_Click(sender As Object, e As EventArgs)
        Dim count As Integer
        If Integer.TryParse(litStateInTransit.Text, count) AndAlso count > 0 Then
            Session("actioncode") = 2
            Response.Redirect("BallotBoxDetailList.aspx")
        End If
    End Sub

    Protected Sub lnkMyStockParked_Click(sender As Object, e As EventArgs)
        Dim count As Integer
        If Integer.TryParse(litMyStockParked.Text, count) AndAlso count > 0 Then
            Response.Redirect("MiscPage.aspx")
        End If
    End Sub

    ' REMOVED: lnkMyStockInTransit_Click since In Transit is no longer shown in My Stock

    Protected Sub lnkInward_Click(sender As Object, e As EventArgs)
        Dim count As Integer
        If Integer.TryParse(litInward.Text, count) AndAlso count > 0 Then
            Response.Redirect("MiscPage.aspx")
        End If
    End Sub

    Protected Sub lnkOutward_Click(sender As Object, e As EventArgs)
        Dim count As Integer
        If Integer.TryParse(litOutward.Text, count) AndAlso count > 0 Then
            Response.Redirect("MiscPage.aspx")
        End If
    End Sub
End Class

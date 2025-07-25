Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web.UI
Imports System.Web.UI.WebControls

Partial Class BallotBoxDetailList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadGrid()
        End If
    End Sub

    Private Sub LoadGrid()
        ' 1) Get session parameters
        Dim userlevelcode As String = Convert.ToString(Session("UserLevel"))
        Dim from_user_id As String = Convert.ToString(Session("UserID"))
        Dim actioncode As Integer
        Integer.TryParse(Convert.ToString(Session("actioncode")), actioncode)

        ' 2) Set UI literals
        litUserId.Text = from_user_id
        litLocation.Text = If(actioncode = 1, "Received", If(actioncode = 2, "In Transit", String.Empty))

        ' 3) Fetch data
        Dim objCommonFunctions As New commonFunctions()
        Dim dt As DataTable = objCommonFunctions.GetBallotBoxDetailList(userlevelcode, from_user_id, actioncode)

        ' 4) Bind
        gvBallotBoxDetail.DataSource = dt
        gvBallotBoxDetail.DataBind()

        ' 5) Show message if empty
        If dt.Rows.Count = 0 Then
            litMessage.Text = "No records found for the selected criteria."
            messageRow.Visible = True
        Else
            messageRow.Visible = False
        End If
    End Sub

    Protected Sub gvBallotBoxDetail_PageIndexChanging(
            sender As Object,
            e As GridViewPageEventArgs
        ) Handles gvBallotBoxDetail.PageIndexChanging

        gvBallotBoxDetail.PageIndex = e.NewPageIndex
        LoadGrid()
    End Sub

    Protected Sub gvBallotBoxDetail_RowCommand(
            sender As Object,
            e As GridViewCommandEventArgs
        ) Handles gvBallotBoxDetail.RowCommand

        If e.CommandName = "SelectRow" Then
            ' CommandArgument format: "code|is_jurisdiction"
            Dim parts() As String = e.CommandArgument.ToString().Split("|"c)
            If parts.Length = 2 Then
                Session("SelectedCode") = parts(0)
                Session("SelectedJurisdiction") = parts(1)
                ' Redirect wherever you need next
                Response.Redirect("BallotBoxBlocksParked.aspx")
            End If
        End If
    End Sub
End Class

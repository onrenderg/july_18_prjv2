Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web.UI
Imports System.Web.UI.WebControls

Partial Class BallotBoxBlocksParked
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadBlocks()
        End If
    End Sub

    Private Sub LoadBlocks()
        ' 1) Get session parameters
        Dim userlevelcode As String = Convert.ToString(Session("UserLevel"))
        Dim prevCode As String = Convert.ToString(Session("SelectedCode"))
        Dim actioncode As Integer = 1  ' parked

        ' 2) Set UI literals
        litUserId.Text = Convert.ToString(Session("UserID"))
        litPrevCode.Text = prevCode

        ' 3) Fetch data
        Dim fn As New commonFunctions()
        Dim dt As DataTable = fn.GetBallotBoxBlocksParked(userlevelcode, prevCode, actioncode)

        ' 4) Bind to GridView
        gvBlocksParked.DataSource = dt
        gvBlocksParked.DataBind()

        ' 5) Show message if empty
        If dt.Rows.Count = 0 Then
            litMessage.Text = "No parked blocks found."
            messageRow.Visible = True
        Else
            messageRow.Visible = False
        End If
    End Sub

    Protected Sub gvBlocksParked_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvBlocksParked.PageIndexChanging
        gvBlocksParked.PageIndex = e.NewPageIndex
        LoadBlocks()
    End Sub

    ' Handle row click to redirect to size list
    Protected Sub gvBlocksParked_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvBlocksParked.RowCommand
        If e.CommandName = "SelectRow" Then
            ' CommandArgument format: "code|is_jurisdiction"
            Dim parts() As String = e.CommandArgument.ToString().Split("|"c)
            If parts.Length = 2 Then
                ' Store selected block code and jurisdiction flag
                Session("code1") = parts(0)
                Session("is_jurisdiction1") = parts(1)

                ' Redirect to size list page
                Response.Redirect("BallotBoxSizeList.aspx")
            End If
        End If
    End Sub
End Class
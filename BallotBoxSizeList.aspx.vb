Imports System
Imports System.Configuration
Imports System.Data
Imports System.Web.UI
Imports System.Web.UI.WebControls

Partial Class BallotBoxSizeList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadSizes()
        End If
    End Sub

    Private Sub LoadSizes()
        Dim userlevelcode As String = Convert.ToString(Session("UserLevel"))
        Dim usercode As String = Convert.ToString(Session("UserID"))
        Dim zonecode As String = Convert.ToString(Session("code1"))
        Dim is_jurisdiction As Integer = Convert.ToInt32(Session("is_jurisdiction1"))
        Dim actioncode As Integer = 1

        litUserId.Text = usercode
        litZone.Text = zonecode

        Dim fn As New commonFunctions()
        Dim dt As DataTable = fn.GetBallotBoxSizeList(userlevelcode, usercode, zonecode, actioncode, is_jurisdiction)

        gvBoxSizes.DataSource = dt
        gvBoxSizes.DataBind()

        messageRow.Visible = (dt.Rows.Count = 0)
        If dt.Rows.Count = 0 Then
            litMessage.Text = "No box size data found."
        End If
    End Sub

    Protected Sub gvBoxSizes_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvBoxSizes.PageIndexChanging
        gvBoxSizes.PageIndex = e.NewPageIndex
        LoadSizes()
    End Sub

    Protected Sub gvBoxSizes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvBoxSizes.RowCommand
        If e.CommandName = "SelectRow" Then
            ' Get box size, zonecode, and jurisdiction from command argument
            Dim args() As String = e.CommandArgument.ToString().Split("|"c)
            If args.Length = 3 Then
                Dim boxSizeRaw As String = args(0).Trim()
                Dim zonecode As String = args(1).Trim()
                Dim is_jurisdiction As Integer = Convert.ToInt32(args(2))

                ' Store Box Size as L or M
                Dim boxSizeCode As String = ""
                If boxSizeRaw.ToUpper().Contains("LARGE") Then
                    boxSizeCode = "L"
                ElseIf boxSizeRaw.ToUpper().Contains("MEDIUM") Then
                    boxSizeCode = "M"
                End If

                ' Save to session
                Session("boxsize") = boxSizeCode
                Session("selected_zonecode") = zonecode
                Session("selected_is_jurisdiction") = is_jurisdiction

                ' Redirect
                Response.Redirect("BallotBoxSerialList.aspx")
            End If
        End If
    End Sub
End Class

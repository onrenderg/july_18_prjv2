Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI
Imports System.Web.UI.WebControls

Partial Class BallotBoxSerialList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadSerials()
        End If
    End Sub

    Private Sub LoadSerials(Optional pageNumber As Integer = 1)
        Try
            ' Static parameters
            Dim userlevelcode As String = "000"
            Dim usercode As String = "adminstate"
            Dim zonecode As String = "130800000000000"
            Dim box_size As String = "L"
            Dim is_jurisdiction As Integer = 0
            Dim pageSize As Integer = 20
            Dim action As Integer = 1

            Dim dt As DataTable = GetBallotBoxSerialList(userlevelcode, usercode, zonecode, box_size, is_jurisdiction, pageNumber, pageSize, action)

            gvSerials.DataSource = dt
            gvSerials.DataBind()

            If dt.Rows.Count = 0 Then
                litMessage.Text = "No ballot box serials found."
                messageRow.Visible = True
            Else
                messageRow.Visible = False
            End If
        Catch ex As Exception
            litMessage.Text = "Error: " & ex.Message
            messageRow.Visible = True
        End Try
    End Sub

    Private Function GetBallotBoxSerialList(
        userlevelcode As String,
        usercode As String,
        zonecode As String,
        box_size As String,
        is_jurisdiction As Integer,
        Optional pageNumber As Integer = 1,
        Optional pageSize As Integer = 50,
        Optional action As Integer = 1
    ) As DataTable
        Dim dt As New DataTable()

        Try
            Using con As New SqlConnection(ConfigurationManager.ConnectionStrings("SecConnection").ConnectionString)
                Using cmd As New SqlCommand("Mobile_BallotBox_GetPagedItems", con)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber)
                    cmd.Parameters.AddWithValue("@PageSize", pageSize)
                    cmd.Parameters.AddWithValue("@userlevelcode", userlevelcode)
                    cmd.Parameters.AddWithValue("@usercode", usercode)
                    cmd.Parameters.AddWithValue("@action", action)
                    cmd.Parameters.AddWithValue("@zonecode", zonecode)
                    cmd.Parameters.AddWithValue("@box_size", If(String.IsNullOrEmpty(box_size), DBNull.Value, box_size))
                    cmd.Parameters.AddWithValue("@is_jurisdiction", is_jurisdiction)

                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw New ApplicationException("Error fetching ballot box serial list: " & ex.Message)
        End Try

        Return dt
    End Function

    Protected Sub gvSerials_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvSerials.PageIndexChanging
        gvSerials.PageIndex = e.NewPageIndex
        LoadSerials(e.NewPageIndex + 1)
    End Sub
End Class

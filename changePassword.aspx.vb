Imports System.Data.SqlClient

Public Class changePassword
    Inherits System.Web.UI.Page


    Dim userName, userLevel As String
    Dim MyInterface As ErmsInterface.hbInterface
    Dim MyImplementer As New ErmsBLL
    Dim mycommand As New SqlCommand
    ' Dim objActivityLog As New ActivityLog
    Dim mytable As New DataTable
    Dim Result As New SqlClient.SqlParameter("@Result", SqlDbType.VarChar, 4000)
    Dim SqlErrMsg As New SqlClient.SqlParameter("@SqlErrMsg", SqlDbType.VarChar, 3072)

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.AddHeader("Cache-control", "no-store, must-revalidate,private,no-cache")
        Response.AddHeader("PRAGMA", "NO-Cache")
        Response.Expires = -1
        Response.Expires = 0
        Response.Cache.SetNoStore()
        Response.Cache.SetExpires(DateTime.Now.AddDays(-2))
    End Sub
    Private Sub Page_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Try


        Catch ex As Exception
        End Try
    End Sub
    Private Sub changePassword_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Try
        '    Response.Cache.SetLastModified(DateTime.Now)
        '    Response.Cache.SetAllowResponseInBrowserHistory(False)
        '    Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
        '    Response.Cache.SetCacheability(HttpCacheability.NoCache)
        '    Response.Cache.SetNoStore()
        '    If Session("UserID") Is Nothing Then
        '        Response.Redirect("~/LogOut.aspx", False)
        '    Else
        '        userName = Session("UserID").ToString()
        '        userLevel = Session("UserLevel").ToString()
        '        If Session("DefultPass") = "yes" Then
        '            dvOldPwd.Visible = False
        '        Else
        '            dvOldPwd.Visible = True
        '        End If
        '    End If
        'Catch ex As Exception
        'End Try
    End Sub
    Protected Sub btnCancel_Click(sender As Object, e As EventArgs)
        ''Response.Redirect("changePassword.aspx", True)
        'If Session("DefultPass") = "yes" Then
        Response.Redirect("~/index.aspx", True)
        'End If
    End Sub

    Protected Sub btnChangePass_Click(sender As Object, e As EventArgs)
        Dim User = commonFunctions.GetUserDataFromSession()
        If Page.IsValid Then ' Client-side validation passed
            alert_div.Visible = False
            If txtNewPassword.Value = txtConfPassword.Value Then  ' Server-side check
                Dim DBAccess = New commonFunctions()
                Dim result = DBAccess.UserLogin_PasswordUpdate(User.Code, txtNewPassword.Value)
                If (result.status_code = 200) Then
                    Session.Add("alert_login", result.status_message)
                    ClientScript.RegisterStartupScript(Me.GetType(), result.status_code.ToString(), $"alert('{result.status_message}');", True)
                    'MessageBox(btnChangePass, "Password changed successfully. Redirecting to Login Page...")
                    Response.Redirect("logout.aspx", False)
                    Context.ApplicationInstance.CompleteRequest() ' This is still important
                    Return
                Else
                    alert_div.Visible = True
                    lbl_alert.Text = result.status_message
                End If
            Else
                alert_div.Visible = True
                lbl_alert.Text = "Passwords do not match."
            End If
        Else
            alert_div.Visible = True
            lbl_alert.Text = "Validation failed."
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

End Class
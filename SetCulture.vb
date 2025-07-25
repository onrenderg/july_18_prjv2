Imports System.Globalization
Imports System.IO
Imports System.IO.Compression
Imports System.Threading

Public Delegate Sub CultureChanged(ByVal Sender As Object, ByVal CurrentCulture As String)

Public Class SetCulture
    Inherits System.Web.UI.Page

    Public Event OnCultureChanged As CultureChanged

    Public Property LastCultureName() As String
        Get
            Dim strlastCultureName As String = DirectCast(Session("MyLanguage"), String)
            If strlastCultureName Is Nothing Then
                Session("MyLanguage") = Thread.CurrentThread.CurrentCulture.Name
            End If
            Return strlastCultureName
        End Get

        Set(ByVal value As String)
            Session("MyLanguage") = value
        End Set
    End Property

    Protected Overrides Sub InitializeCulture()
        'TODO: make this prettier

        Dim language As String = Request.Form("__EventTarget")
        Dim languageId As String = ""
        'Check if PostBack is caused by Language DropDownList.
        If Request.Form("__EVENTTARGET") IsNot Nothing AndAlso Request.Form("__EVENTTARGET").ToLower.Contains("ddllang") Then
            'Set the Language.
            languageId = Request.Form(Request.Form("__EVENTTARGET"))
            SetCulture(languageId)
            MyBase.InitializeCulture()
            Return
        End If
        If (((Session("MyLanguage")) IsNot Nothing)) Then
            If Not Session("MyLanguage").ToString().StartsWith(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName) Then
                SetCulture(Session("MyLanguage").ToString())
            End If
        End If

        'Dim lang As String
        'lang = "en-GB"
        'lang = Convert.ToString(Session("MyLanguage"))
        ''Session("MyLanguage") = "en-US"

        'If Session("MyLanguage") Is Nothing Then
        '    lang = LastCultureName
        '    lang = "en-GB"
        'Else
        '    lang = Convert.ToString(Session("MyLanguage"))
        'End If

        'Thread.CurrentThread.CurrentCulture = New CultureInfo(lang)
        'Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(lang)
    End Sub

    Protected Overloads Overrides Sub OnLoadComplete(ByVal e As EventArgs)
        MyBase.OnLoadComplete(e)
        If LastCultureName <> Thread.CurrentThread.CurrentCulture.Name Then
            LastCultureName = Thread.CurrentThread.CurrentCulture.Name
            RaiseEvent OnCultureChanged(Me, LastCultureName)
        End If
    End Sub
    Protected Sub SetCulture(ByVal languageId As String)
        Try
            Session("MyLanguage") = languageId
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(languageId)
            Thread.CurrentThread.CurrentUICulture = New CultureInfo(languageId)
        Catch ex1 As CultureNotFoundException
            Dim lang As String
            lang = "en-GB"
            Session("MyLanguage") = lang

            Thread.CurrentThread.CurrentCulture = New CultureInfo(lang)
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(lang)
        Catch ex As Exception

        End Try
    End Sub
    Public Shared Function Compress(ByVal data As Byte()) As Byte()
        Dim output As New MemoryStream()
        Dim gzip As New GZipStream(output, CompressionMode.Compress, True)
        gzip.Write(data, 0, data.Length)
        gzip.Close()
        Return output.ToArray()
    End Function

    Public Shared Function Decompress(ByVal data As Byte()) As Byte()
        Dim input As New MemoryStream()
        input.Write(data, 0, data.Length)
        input.Position = 0
        Dim gzip As New GZipStream(input, CompressionMode.Decompress, True)
        Dim output As New MemoryStream()
        Dim buff As Byte() = New Byte(63) {}
        Dim read As Integer = -1
        read = gzip.Read(buff, 0, buff.Length)
        While read > 0
            output.Write(buff, 0, read)
            read = gzip.Read(buff, 0, buff.Length)
        End While
        gzip.Close()
        Return output.ToArray()
    End Function


End Class

Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Threading

Public Class Site
    Inherits System.Web.UI.MasterPage
    Dim userlevel, username As String
    Dim MyInterface As ErmsInterface.hbInterface
    Dim MyImplementer As New ErmsBLL
    Dim validation As New validation
    Dim mycommand As New SqlCommand
    Dim mytable As New DataTable
    Public strRootPath As String


    Private Const AntiXsrfTokenKey As String = "__AntiXsrfToken"
    Private Const AntiXsrfUserNameKey As String = "__AntiXsrfUserName"
    Private _antiXsrfTokenValue As String

    Protected Overrides Sub FrameworkInitialize()
        'MyBase.FrameworkInitialize()
        'If (Session("MyLanguage") IsNot Nothing) Then
        '    Dim ci As New CultureInfo(Session("MyLanguage").ToString())
        '    Thread.CurrentThread.CurrentCulture = ci
        '    Thread.CurrentThread.CurrentUICulture = ci
        'End If

        If Request.Form("ddllang") IsNot Nothing Then
            Dim selectedLanguage As String = Request.Form("ddllang")
            Page.UICulture = Request.Form("ddllang")
            Page.Culture = Request.Form("ddllang")
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(selectedLanguage)
            Thread.CurrentThread.CurrentUICulture = New CultureInfo(selectedLanguage)
            Session("MyLanguage") = selectedLanguage
            Application("CultureInfo") = selectedLanguage
        ElseIf Application("CultureInfo") IsNot Nothing And Application("CultureInfo") <> "" Then
            Page.UICulture = Application("CultureInfo")
            Page.Culture = Application("CultureInfo")
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Application("CultureInfo"))
            Thread.CurrentThread.CurrentUICulture = New CultureInfo(Application("CultureInfo").ToString())
            Session("MyLanguage") = Application("CultureInfo")
        End If
        'End If
        MyBase.FrameworkInitialize()

    End Sub

    Protected Overrides Sub AddedControl(ByVal control As Control, ByVal index As Integer)
        If Request.ServerVariables("http_user_agent").IndexOf("Safari", StringComparison.CurrentCultureIgnoreCase) <> -1 Then Me.Page.ClientTarget = "uplevel"
        MyBase.AddedControl(control, index)
    End Sub
    Protected Overrides Sub OnInit(ByVal e As EventArgs)
        If Not Me.Page.EnableViewStateMac Then
            Response.Write("Invalid Request")
        End If
        Page.ViewStateUserKey = Session.SessionID
        MyBase.OnInit(e)
        If (Session("MyLanguage") IsNot Nothing) Then
            Dim ci As New CultureInfo(Session("MyLanguage").ToString())
            Thread.CurrentThread.CurrentCulture = ci
            Thread.CurrentThread.CurrentUICulture = ci
        Else
            Thread.CurrentThread.CurrentCulture = New CultureInfo("hi-IN")
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("hi-IN")
            Session("MyLanguage") = "hi-IN"
            Application("CultureInfo") = "hi-IN"

        End If
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.AddHeader("Cache-control", "no-store, must-revalidate,private,no-cache")
        Response.AddHeader("PRAGMA", "NO-Cache")
        Response.Expires = -1
        Response.Expires = 0
        Response.Cache.SetNoStore()
        Response.Cache.SetExpires(DateTime.Now.AddDays(-2))


        ''''Set Token For CSRF Prevention''''
        Dim requestCookie = Request.Cookies(AntiXsrfTokenKey)
        Dim requestCookieGuidValue As Guid
        If requestCookie IsNot Nothing AndAlso Guid.TryParse(requestCookie.Value, requestCookieGuidValue) Then
            _antiXsrfTokenValue = requestCookie.Value
            Page.ViewStateUserKey = _antiXsrfTokenValue
        Else
            _antiXsrfTokenValue = Guid.NewGuid().ToString("N")
            Page.ViewStateUserKey = _antiXsrfTokenValue
            Dim responseCookie = New HttpCookie(AntiXsrfTokenKey) With {.HttpOnly = True, .Value = _antiXsrfTokenValue, .Secure = False}
            If FormsAuthentication.RequireSSL AndAlso Request.IsSecureConnection Then responseCookie.Secure = True
            Response.Cookies.[Set](responseCookie)
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetLastModified(DateTime.Now)
        Response.Cache.SetAllowResponseInBrowserHistory(False)
        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetNoStore()

        If String.IsNullOrEmpty(Session("UserID")) Then
            Response.Redirect("login.aspx", True)
        End If

        ''''Check Token For CSRF Prevention''''
        If Not IsPostBack Then
            If Not IsPostBack Then
                litTitle.Text = Resources.Resource.PageResource1
            End If
            ViewState(AntiXsrfTokenKey) = Page.ViewStateUserKey
            ViewState(AntiXsrfUserNameKey) = If(Context.User.Identity.Name, String.Empty)

            If Session("MyLanguage") IsNot Nothing And Session("MyLanguage") <> "" Then
                Thread.CurrentThread.CurrentCulture = New CultureInfo(Trim(Session("MyLanguage")))
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(Trim(Session("MyLanguage")))
                Session("MyLanguage") = Trim(Session("MyLanguage"))
                ddllang.SelectedIndex = ddllang.Items.IndexOf(ddllang.Items.FindByValue(Trim(Session("MyLanguage"))))
                Application("CultureInfo") = Trim(Session("MyLanguage"))
            ElseIf Application("CultureInfo") IsNot Nothing And Application("CultureInfo") <> "" Then
                Thread.CurrentThread.CurrentUICulture = New CultureInfo(Application("CultureInfo").ToString())
                Thread.CurrentThread.CurrentCulture = New CultureInfo(Application("CultureInfo").ToString())
                ddllang.SelectedIndex = ddllang.Items.IndexOf(ddllang.Items.FindByValue(Application("CultureInfo").ToString()))
                FrameworkInitialize()
            Else
                Thread.CurrentThread.CurrentCulture = New CultureInfo("hi-IN")
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("hi-IN")
                Session("MyLanguage") = "hi-IN"
                Application("CultureInfo") = "hi-IN"
                ddllang.SelectedIndex = 0
            End If
        Else
            If CStr(ViewState(AntiXsrfTokenKey)) <> _antiXsrfTokenValue OrElse CStr(ViewState(AntiXsrfUserNameKey)) <> (If(Context.User.Identity.Name, String.Empty)) Then
                'Throw New InvalidOperationException("Validation of", Anti - XSRF, token, failed., ");")
                Response.Write("Invalid Request")
            End If
        End If

        If Not Request.Url.ToString().Contains("localhost") Then
            strRootPath = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + "/"
        Else
            strRootPath = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath
        End If

        Dim cookie As HttpCookie = Request.Cookies(".ASPXFORMDATA")
        Dim myLang As String = ddllang.SelectedValue
        Console.WriteLine(Request.RawUrl)
        If Session("DefultPass") = "yes" And (Request.RawUrl.ToLower.Contains("changepassword.aspx") = False) Then
            Response.Redirect("~/changePassword.aspx", True)
        Else
            Try
                'Session("UserID") = "Vandana"
                'Session("UserDesg") = "State Admin"
                'Session("UserDesgLocal") = "ज़िला प्रशासक" '"राज्य प्रशासक"
                'Session("UserLevel") = "001" ' "000"
                'Session("gblStateCode") = "13"
                'Session("gblDistrictCode") = "1309" ' "0000"
                'Session("gblDistrictName") = "Shimla"
                'Session("gblDistrictNameLocal") = "शिमला"
                'Session("gblBlockCode") = "0000000"
                'Session("gblPanchayatCode") = "0000000000"
                'Session("EmailId") = "VANDANA.DHIMAN@NIC.IN"
                'Session("MobileNo") = "9418128444"
                'Session("officerName") = "Vandana Sankhayan"
                'Session("landLine") = "0177 2625216"
                'Session("NNN") = "MC"

                If Not IsPostBack Then
                    Dim userID As String = Session("UserID").ToString.ToUpper
                    lblName.Text = Resources.Resource.welcome + " " + userID + " !"
                    Dim userlevel As String = Session("UserLevel").ToString.ToUpper
                    Dim userDesg As String = Session("UserDesg").ToString.ToUpper
                    Dim userDesgLocal As String = Session("UserDesglocal").ToString.ToUpper

                    Dim DistrictName As String = Session("gblDistrictName").ToUpper
                    Dim DistrictNameLocal As String = Session("gblDistrictNameLocal").ToUpper
                    Select Case userlevel
                        Case "000"
                            lblRole.Text = Resources.Resource.userRole + " <b>" + IIf(myLang = "hi-IN", userDesgLocal, userDesg) + "</b> " +
                                Resources.Resource.lblState + ": <b>" + Resources.Resource.lblStateName + "</b>"
                        Case "012", "013"
                            lblRole.Text = Resources.Resource.userRole + " <b>" + IIf(myLang = "hi-IN", userDesgLocal, userDesg) + "</b> " +
                                 Resources.Resource.ofDis + " <b>" + IIf(myLang = "hi-IN", DistrictNameLocal, DistrictName) + "</b>, " +
                                Resources.Resource.lblState + ": <b>" + Resources.Resource.lblStateName + "</b>"
                        Case Else
                            lblRole.Text = Resources.Resource.userRole + " <b>" + IIf(myLang = "hi-IN", userDesgLocal, userDesg) + "</b> " +
                                 Resources.Resource.ofDis + " <b>" + IIf(myLang = "hi-IN", DistrictNameLocal, DistrictName) + "</b>, " +
                                Resources.Resource.lblState + ": <b>" + Resources.Resource.lblStateName + "</b>"

                    End Select

                End If
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub ddllang_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddllang.SelectedIndexChanged
        Thread.CurrentThread.CurrentCulture = New CultureInfo(ddllang.SelectedValue)
        Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(ddllang.SelectedValue)
        Session("MyLanguage") = ddllang.SelectedValue
        Application("CultureInfo") = ddllang.SelectedValue


        'Session("MyLanguage") = ddllang.SelectedValue
        'Thread.CurrentThread.CurrentCulture = New CultureInfo(Trim(Session("MyLanguage")))
        'Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(Trim(Session("MyLanguage")))
        '' Session("MyLanguage") = Trim(Session("MyLanguage"))
        'ddllang.SelectedIndex = ddllang.Items.IndexOf(ddllang.Items.FindByValue(Trim(Session("MyLanguage"))))
        'Application("CultureInfo") = Trim(Session("MyLanguage"))
        Server.Transfer(Request.Url.PathAndQuery, False)
        MyBase.FrameworkInitialize()
    End Sub

End Class
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="login.aspx.vb" Inherits="SEC_InventoryMgmt.login" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>


<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inventory Management System - State Election Commission, Himachal Pradesh</title>

    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <!-- Tell the browser to be responsive to screen width -->
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <!-- Favicon icon -->
    <link
        rel="icon"
        type="image/png"
        sizes="16x16"
        href="assets/images/favicon.png" />
    <!-- Custom CSS -->
    <link href="dist/css/style.min.css" rel="stylesheet" />
    <%--<script src="assets/libs/popper.js/dist/umd/popper.min.js"></script>--%>
    <script src="assets/libs/bootstrap/dist/js/bootstrap.min.js"></script>
    <script src="assets/libs/jquery/dist/jquery-3.7.1.min.js"></script>

    <script src="dist/js/common.js"></script>

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
    <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
    <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
<![endif]-->
    <script type="text/javascript">
        javascript: window.history.forward(1);
        function clearids() {
            document.getElementById("txtCap").value = "";
        }
    </script>
    <script>
        $(document).ready(function () {
            $('#<%= ddllang.ClientID%>').find("option:selected").each(function () {
                var optionValue = $(this).attr("value");
                if (optionValue == 'en-GB') {
                    $("#imgHi").hide();
                    $("#imgEn").show();
                }
                else {

                    $("#imgHi").show();
                    $("#imgEn").hide();
                }
            })
        });

        //$(document).ready(function () {
        //    $("#form1").validate({
        //    });
        //});
    </script>
    <script>
        function CheckChar(ControlId) {
            //var pass = document.getElementById(ControlId.id).value;
            var pass = $(ControlId).val();
            document.getElementById("lblMessage").innerHTML = "";

            if (pass.length < 8) {
                document.getElementById("lblMessage").innerHTML = '<%= GetLocalResourceObject("PassSizeMsg")%>';
                $(ControlId).val("");
                $(ControlId).focus();
                return false;
            }
            else if (pass.search(/^(?=.*?[a-z])/) < 0) {
                document.getElementById("lblMessage").innerHTML = '<%= GetLocalResourceObject("PassLowerCaseMsg") %>';
                $(ControlId).val("");
                $(ControlId).focus();
                return false;
            }
            else if (pass.search(/^(?=.*?[A-Z])/) < 0) {
                document.getElementById("lblMessage").innerHTML = '<%= GetLocalResourceObject("PassUpperCaseMsg")%>';
                $(ControlId).val("");
                $(ControlId).focus();
                return false;
            }
            else if (pass.search(/^(?=.*?[0-9])/) < 0) {
                document.getElementById("lblMessage").innerHTML = '<%= GetLocalResourceObject("PassNumericMsg")%>';
                $(ControlId).val("");
                $(ControlId).focus();
                return false;
            }
            else if (pass.search(/(?=.*?[^\w\s])/) < 0) {
                document.getElementById("lblMessage").innerHTML = '<%= GetLocalResourceObject("PassSpecailSymbolMsg") %>';
                $(ControlId).val("");
                $(ControlId).focus();
                return false;
            }
        }

        function ValidateControls() {
            if (document.getElementById("<%=txtUserID.ClientID%>").value == "" && document.getElementById("<%=txtID.ClientID%>").value == "") {
                alert("Please Enter User Name.");
                document.getElementById("<%=txtUserID.ClientID%>").focus();
                return false;
            }
            else if (EncriptNewPass() == false) {
                return false;
            }
            else if (!ValidateControlsInvalidChar()) {
                return false;
            }
            else { return true; }
        }
    </script>
    <style>
        th {
            background-color: #000F60;
            color: whitesmoke;
        }

        input[type=radio] {
            margin-left: 30px;
        }

            input[type=radio]:first-child {
                margin-left: 10px;
            }
    </style>
</head>
<body class="bg-dark">
    <div class="main-wrapper bg-dark">
        <!-- ============================================================== -->
        <!-- Preloader - style you can find in spinners.css -->
        <!-- ============================================================== -->
        <form class="form-horizontal" runat="server" defaultbutton="btnLogin" autocomplete="off">
            <div class="preloader">
                <div class="lds-ripple">
                    <div class="lds-pos"></div>
                    <div class="lds-pos"></div>
                </div>
            </div>
            <header class="topbar" data-navbarbg="skin5">
                <nav class="navbar top-navbar navbar-expand-md navbar-dark">
                    <div class="navbar-header" style="background-color: black;">
                        <!-- ============================================================== -->
                        <!-- Logo -->
                        <!-- ============================================================== -->
                        <a class="navbar-brand" href="https://secHimachal.nic.in" target="_blank">
                            <!-- Logo icon -->
                            <b class="logo-icon p-l-10">
                                <!-- Dark Logo icon -->
                                <img src="assets/images/secLogo50.png" alt="homepage" />
                            </b>
                            <!--End Logo icon -->
                            <!-- Logo text -->
                            <span class="logo-text" style="background-color: black;">
                                <!-- dark Logo text -->
                                <img id="imgEn" src="assets/images/logo-text.png" width="178" alt="homepage" />
                                <img id="imgHi" src="assets/images/logo-text-hindi.png" alt="homepage" />
                            </span>
                        </a>
                        <!-- ============================================================== -->
                        <!-- End Logo -->
                        <!-- ============================================================== -->
                        <!-- ============================================================== -->
                        <!-- Toggle which is visible on mobile only -->
                        <!-- ============================================================== -->
                        <a
                            class="nav-toggler waves-effect waves-light d-block d-md-none"
                            href="javascript:void(0)"><i class="ti-menu ti-close"></i></a>
                    </div>
                    <!-- ============================================================== -->
                    <!-- End Logo -->
                    <!-- ============================================================== -->
                    <div
                        class="navbar-collapse collapse"
                        id="navbarSupportedContent"
                        data-navbarbg="skin5">
                        <!-- ============================================================== -->
                        <!-- toggle and nav items -->
                        <!-- ============================================================== -->
                        <ul class="navbar-nav float-start me-auto">
                            <li class="nav-item d-none d-lg-block">
                                <%--<a
                                    class="nav-link sidebartoggler waves-effect waves-light"
                                    href="javascript:void(0)"
                                    data-sidebartype="mini-sidebar"><i class="mdi mdi-menu font-24"></i></a>--%>
                            </li>
                        </ul>
                        <ul class="navbar-nav float-none me-auto">
                            <li class="nav-justified">
                                <h3 class="waves-dark bg-transparent" style="color: whitesmoke; margin: 5px; vertical-align: middle;"><%=Resources.Resource.ApplicationName%></h3>
                                <h5 class="waves-dark bg-transparent" style="text-align: center; color: whitesmoke; margin: 5px; vertical-align: middle;"><%=Resources.Resource.SEC %></h5>
                            </li>
                        </ul>
                        <ul class="navbar-nav float-end">
                            <li class="nav-item waves-effect waves-dark dropdown-menu-right">
                                <div style="float: right">
                                    <asp:DropDownList runat="server" ID="ddllang" AutoPostBack="True" CssClass="dropdown-menu-dark" meta:resourcekey="ddllangResource1">
                                        <asp:ListItem Text="हिन्दी" Value="hi-IN" Selected="True" meta:resourcekey="ListItemResource1"> </asp:ListItem>
                                        <asp:ListItem Text="English" Value="en-GB" meta:resourcekey="ListItemResource2"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </li>
                        </ul>
                        <!-- ============================================================== -->
                        <!-- Right side toggle and nav items -->
                        <!-- ============================================================== -->
                        <ul class="navbar-nav float-end">
                            <!-- ============================================================== -->
                            <!-- User profile and search -->
                            <!-- ============================================================== -->
                            <li class="nav-item dropdown">
                                <a class="nav-link dropdown-toggle text-muted waves-effect waves-dark pro-pic" href="#"
                                    id="navbarDropdown"
                                    role="button"
                                    data-bs-toggle="dropdown"
                                    aria-expanded="false">
                                    <img
                                        src="assets/images/users/1.jpg"
                                        alt="user"
                                        class="rounded-circle"
                                        width="31" />
                                </a>
                                <ul
                                    class="dropdown-menu dropdown-menu-end user-dd animated"
                                    aria-labelledby="navbarDropdown">
                                    <a class="dropdown-item" href="logout.aspx"><i class="fa fa-power-off me-1 ms-1"></i>Logout</a>

                                </ul>
                            </li>
                            <!-- ============================================================== -->
                            <!-- User profile and search -->
                            <!-- ============================================================== -->
                        </ul>
                    </div>
                </nav>
            </header>
            <div class="container-fluid bg-dark">
                <div class="row">
                    <div class="col-sm-2 col-md-4 col-lg-4" style="vertical-align: top;">
                    </div>
                    <div class="col-sm-8 col-md-4 col-lg-4">
                        <div class="card scrollable" style="min-height: 96%; overflow-y: auto;">
                            <div class="card-body">
                                <div class="auth-wrapper justify-content-center bg-light">

                                    <%-- <div class="auth-box bg-light">--%>
                                    <!-- Form -->
                                    <%--   <form class="form-horizontal" runat="server" defaultbutton="btnLogin">--%>
                                    <!-- ============================================================== -->
                                    <!-- Preloader - style you can find in spinners.css -->
                                    <!-- ============================================================== -->
                                    <!-- ============================================================== -->
                                    <!-- Login box.scss -->
                                    <!-- ============================================================== -->
                                    <div id="loginform" class="table-bordered  animated" runat="server">
                                        <div class="page-breadcrumb border-bottom border-secondary">
                                            <div class="row">
                                                <div class="col-12 d-flex no-block align-items-center">
                                                    <h4 class="page-title">
                                                        <%--  <%=Resources.Resource.ApplicationName%>--%>
                                                        <asp:Label ID="lblheader" runat="server" Text="User Login" meta:resourcekey="lblheader"></asp:Label></h4>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="container-fluid">
                                            <asp:ScriptManager ID="sm1" runat="server">
                                            </asp:ScriptManager>
                                            <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div class="card-body">
                                                        <div class="row d-flex no-block justify-content-center align-items-center">
                                                            <div class="col-sm-12">
                                                                <div class="input-group mb-2">
                                                                    <div class="input-group-prepend">
                                                                        <span class="input-group-text bg-success text-white" id="basic-addon1"><i class="ti-user"></i></span>
                                                                    </div>
                                                                    <input type="text" runat="server" id="txtUserID" autocomplete="off" maxlength="30"
                                                                        class="form-control form-control-lg" placeholder="User ID" aria-label="User ID"
                                                                        aria-describedby="basic-addon1" meta:resourcekey="lbluseridResource1" />
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <asp:RequiredFieldValidator ID="rfvUserName" CssClass="alert alert-danger" ValidationGroup="vgPassword"
                                                                        runat="server" ErrorMessage="*" ControlToValidate="txtUserID"></asp:RequiredFieldValidator>
                                                                </div>

                                                                <div class="input-group mb-2">
                                                                    <div class="input-group-prepend">
                                                                        <span class="input-group-text bg-warning text-white" id="basic-addon2"><i class="ti-pencil"></i></span>
                                                                    </div>
                                                                    <input type="password" class="form-control form-control-lg" placeholder="Password"
                                                                        runat="server" id="txtUserPass" aria-label="User Password" autocomplete="off"
                                                                        aria-describedby="basic-addon1" meta:resourcekey="lblPasswordResource1" maxlength="36" min="6" />
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <span id="lblMessage" class="badge badge-pill badge-danger"></span>
                                                                    <asp:RequiredFieldValidator ID="rfvUserPassword" CssClass="alert alert-danger" ValidationGroup="vgPassword"
                                                                        runat="server" ErrorMessage="*" ControlToValidate="txtUserPass"></asp:RequiredFieldValidator>
                                                                </div>
                                                                
                                                                <asp:UpdatePanel ID="upCaptcha" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <div class="row border-top border-secondary mb-3">
                                                                            <div class="col-8 scrollable m-10">
                                                                                <cc1:CaptchaControl ID="SearchCaptcha" runat="server" CssClass="col-12 input-group floatLeft" CaptchaBackgroundNoise="low"
                                                                                    CaptchaLength="5" CaptchaLineNoise="low" CaptchaMaxTimeout="240" CaptchaHeight="31"
                                                                                    CaptchaMinTimeout="5" CaptchaWidth="130" CaptchaFont="Verdana" />
                                                                            </div>
                                                                            <div class="row">
                                                                                <div class="col-9">
                                                                                    <asp:TextBox ID="txtCaptcha" runat="server" CssClass="input-group widthSmall float-right text-uppercase" Height="31"
                                                                                        MaxLength="5" autocomplete="off" AutoCompleteType="Disabled" meta:resourcekey="txtCapResource1" Width="100%"></asp:TextBox>
                                                                                </div>
                                                                                <div class="col-3">
                                                                                    <asp:ImageButton ID="BtbchangeCaptcha" runat="server" Height="31"
                                                                                        CausesValidation="False" title="Change Captcha" OnClick="ImageButton1_Click" ImageAlign="AbsMiddle" ImageUrl="~/assets/images/reload.png" meta:resourcekey="BtbchangeCaptchaResource1" />
                                                                                </div>
                                                                            </div>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="BtbchangeCaptcha" EventName="Click" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row border-top border-secondary p-3" >
                                                        <div>
                                                            <asp:Label ID="lblMsg" runat="server" Text="" CssClass="col-sm-12 justify-content-center align-items-center text-danger"></asp:Label>
                                                            <br />
                                                            <button class="btn btn-info" id="to-recover" type="button" meta:resourcekey="lnkForgetPasswordResource1"><i class="fa fa-lock m-r-5"></i><%=Resources.Resource.ForgetPasswordResource %></button>
                                                            <asp:Button class="btn btn-success float-right" ID="btnLogin" runat="server" Text="Login" meta:resourcekey="btnLogInResource1" />
                                                            <asp:HiddenField ID="hidHash" runat="server" />
                                                        </div>
                                                    </div>
                                                    
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnLogin" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <%--</form>--%>
                                    <div id="recoverform" runat="server" class="hide">
                                        <asp:UpdatePanel ID="upRecoverForm" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="page-breadcrumb border-bottom border-secondary">
                                                    <div class="row">
                                                        <div class="col-12 d-flex no-block align-items-center">
                                                            <h3 class="page-title">
                                                                <asp:Label ID="lblRecoverPwd" runat="server" meta:resourcekey="lnkForgetPasswordResource1"></asp:Label></h3>
                                                        </div>
                                                    </div>
                                                </div>
                                                <%-- <div class="text-center">
                                                    <span class="text-primary"><%=Resources.Resource.ForgetPasswordTitle %></span>
                                                </div>--%>
                                                <div class="row">
                                                    <!-- Form -->
                                                    <%-- <form class="col-12 d-flex no-block" action="Login.aspx">--%>
                                                    <div id="dvFP" class="col-12" runat="server">
                                                        <div class="input-group mb-3">
                                                            <div class="input-group-prepend">
                                                                <span class="input-group-text bg-info" id="basic-addon21"><i class="ti-user"></i></span>
                                                            </div>
                                                            <input type="text" class="form-control form-control-lg" aria-label="User ID" id="txtID" runat="server"
                                                                aria-describedby="basic-addon1" autocomplete="off" meta:resourcekey="lnkEmailIdResource1" />
                                                        </div>
                                                        <div class="input-group mb-3">
                                                            <asp:Label ID="lbl_msg" runat="server" class="col-sm-12 justify-content-center align-items-center text-danger" meta:resourcekey="lblFPMsg"></asp:Label>
                                                        </div>
                                                        <!-- pwd -->
                                                        <div class="row m-t-20 p-t-20 border-top border-secondary" id="dvOtpBtns" runat="server">
                                                            <div class="col-12">
                                                                <a class="btn btn-success" href="#" id="to-login" name="action"><%=Resources.Resource.BackToLogin %></a>&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:Button class="btn btn-info float-right" ID="btnSendOTP" UseSubmitBehavior="false" CausesValidation="false" runat="server" Text="" meta:resourcekey="btnsendOtpResource1" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div id="dvOTP" class="col-12" runat="server" visible="false">
                                                        <div class="input-group mb-3">
                                                            <div class="input-group-prepend">
                                                                <span class="input-group-text bg-danger"><i class="ti-ink-pen"></i></span>
                                                            </div>
                                                            <input type="text" class="form-control form-control-lg" aria-label="User ID" id="txtOTP" runat="server" maxlength="6" onkeypress="return AllowNumbers()"
                                                                aria-describedby="basic-addon1" meta:resourcekey="lnkOTPResource1" autocomplete="off" />
                                                            <asp:ImageButton ID="btnResendOTP" runat="server" Width="30" Height="40"
                                                                CssClass="border-warning" CausesValidation="False" title="Resend" ImageAlign="AbsMiddle" ImageUrl="assets/images/reload.png" meta:resourcekey="BtbResendOTPResource1" />
                                                        </div>
                                                        <div class="input-group mb-3">
                                                            <asp:Label ID="lbl_otp" runat="server" class="col-sm-12 justify-content-center align-items-center text-danger"></asp:Label>

                                                        </div>
                                                        <div class="row m-t-20 p-t-20 border-top border-secondary">
                                                            <div class="col-12">
                                                                <asp:Button class="btn btn-info float-right" ID="btnConfirmOTP" CausesValidation="False" UseSubmitBehavior="true" runat="server" meta:resourcekey="btnVerifyResource1" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <%--  </form>--%>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnSendOTP" />
                                                <asp:PostBackTrigger ControlID="btnConfirmOTP" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                    <!-- ============================================================== -->
                                    <!-- Login box.scss -->
                                    <!-- ============================================================== -->
                                </div>
                            </div>
                            <div class="card-body">
                                <h5 class="card-title m-b-0"><%=Resources.Resource.ContactUs %></h5>
                                <div class="d-flex flex-row comment-row m-t-0">
                                    <div class="comment-text w-100">
                                        <h6 class="font-medium"><%=Resources.Resource.SEC %></h6>
                                        <span class="m-b-15 d-block">
                                            <i class="mdi mdi-account-card-details"></i><%=Resources.Resource.ContactAddress %><br />
                                            <i class="mdi mdi-email"></i><%=Resources.Resource.ContactEmail %><br />
                                            <i class="mdi mdi-phone-classic"></i><%=Resources.Resource.ContactPhone %><br />
                                            <i class="mdi mdi-fax"></i><%=Resources.Resource.ContactFax %><br />
                                            <br />
                                        </span>
                                    </div>
                                </div>
                                <center><%= GetLocalResourceObject("txtFooter")%></center>
                            </div>
                        </div>

                    </div>
                    <div class="col-sm-2 col-md-4 col-lg-4" style="vertical-align: top;">
                    </div>
                </div>
            </div>
            <div class="hide footer col-sm-12 font-16 text-center">

                <div class="card scrollable" style="min-height: 17%;">
                    <div class="card-body" style="text-align: left;">
                        <div class="row">
                            <div class="comment-text w-100 justify-content-around">
                                <span class="m-b-15 d-block">
                                    <b><%=Resources.Resource.Disclaimer %></b> <%=Resources.Resource.DisclaimerText%>
                                </span>
                                <center><%= GetLocalResourceObject("txtFooter")%></center>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </form>
    </div>

    <!-- this page js -->
    <!-- ============================================================== -->
    <script type="text/javascript" src="dist/js/default.js"></script>
    <script type="text/javascript" src="dist/js/sha.min.js"></script>
    <script src="dist/js/jquery.ui.touch-punch-improved.js"></script>
    <script src="assets/libs/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <script src="dist/js/jquery-ui-1.14.1/jquery-ui.min.js"></script>
    <script src="dist/js/waves.js"></script>
    <script>
        $(".preloader").fadeOut();

        $(document).ready(function () {
            $('#<%= ddllang.ClientID%>').find("option:selected").each(function () {
                var optionValue = $(this).attr("value");
                //alert(optionValue);
                if (optionValue == 'en-GB') {
                    $("#imgHi").hide();
                    $("#imgEn").show();
                }
                else {

                    $("#imgHi").show();
                    $("#imgEn").hide();
                }
            })
        });
    </script>
    <script>


        // ============================================================== 
        // Login and Recover Password 
        // ============================================================== 
        $('#to-recover').on("click", function () {
            alert("Please visit DPMIS portal at URL: https://sechimachal.nic.in/dpmis to reset your password.");
            //$("#loginform").slideUp();
            //$("#recoverform").fadeIn();
        });
       
        $('#btnSendOTP').click(function () {
            $("#loginform").slideUp();
            $("#recoverform").fadeIn();
        });
        $('#to-login').on("click", function () {
            $("#loginform").fadeIn();
            $("#recoverform").slideUp();
        });


        $('#<%=btnLogin.ClientID%>').click(function () {
            var userName = $('#txtUserID').val();
            var userPass = $('#txtUserPass').val();
            if ($('#txtUserID').val() == '') {
                alert('<%= GetLocalResourceObject("IdEmptyMsg") %>');
                $('#txtUserID').focus();
                return false;
            }
            
            if ($('#txtUserPass').val() == '') {
                alert('<%= GetLocalResourceObject("PassEmptyMsg") %>');
                $('#txtUserPass').focus();
                return false;
            }
            
        });
    </script>

</body>

</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmError.aspx.vb" Inherits="SEC_InventoryMgmt.frmError" %>


<!DOCTYPE html>
<meta charset="utf-8">
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<!-- Tell the browser to be responsive to screen width -->
<meta name="viewport" content="width=device-width, initial-scale=1">
<meta name="description" content="">
<meta name="author" content="">
<!-- Favicon icon -->
<link rel="icon" type="image/png" sizes="16x16" href="assets/images/favicon.png">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inventory Management System - State Election Commission, Himachal Pradesh</title>
    <!-- Custom CSS -->
    <link href="dist/css/style.min.css" rel="stylesheet">
    <script src="assets/libs/jquery/dist/jquery-3.7.1.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main-wrapper">
            <!-- ============================================================== -->
            <!-- Preloader - style you can find in spinners.css -->
            <!-- ============================================================== -->
            <%--  <div class="preloader">
            <div class="lds-ripple">
                <div class="lds-pos"></div>
                <div class="lds-pos"></div>
            </div>
        </div>--%>
            <!-- ============================================================== -->
            <!-- Preloader - style you can find in spinners.css -->
            <!-- ============================================================== -->
            <!-- ============================================================== -->
            <!-- Login box.scss -->
            <!-- ============================================================== -->
            <div class="error-box">
                <div class="error-body text-center">
                    <h1 class="error-subtitle text-danger">Application Error</h1>
                    <br />
                    <h3 class="text-uppercase error-subtitle">Sorry! An Error Occured, Please Try Again.</h3>
                    <br />
                    <asp:Label ID="lblErrorCode" runat="server"></asp:Label>
                    <br />
                    <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
                    <br />
                    <asp:Literal ID="litStackTrace" runat="server"></asp:Literal>
                    <br />
                    <asp:Label ID="lblUserCode" runat="server"></asp:Label>
                    <br />
                    <a rel="noopener noreferrer" href="Login.aspx" class="btn btn-danger btn-rounded waves-effect waves-light m-b-40">Back to home</a>
                </div>
            </div>
        </div>
        <!-- ============================================================== -->
        <!-- All Required js -->
        <!-- Bootstrap tether Core JavaScript -->
        <script src="assets/libs/popper.js/dist/umd/popper.min.js"></script>
        <script src="assets/libs/bootstrap/dist/js/bootstrap.min.js"></script>
        <!-- ============================================================== -->
        <!-- This page plugin js -->
        <!-- ============================================================== -->
        <script>
            $('[data-toggle="tooltip"]').tooltip();
            $(".preloader").fadeOut();
        </script>
    </form>
</body>
</html>

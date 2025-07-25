<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="changePassword.aspx.vb" Inherits="SEC_InventoryMgmt.changePassword" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="preloader">
        <div class="lds-ripple">
            <div class="lds-pos"></div>
            <div class="lds-pos"></div>
        </div>
    </div>
    <div class="page-breadcrumb">
        <div class="row">
            <div class="col-12 d-flex no-block align-items-center">
                <h3 class="page-title">
                    <asp:Label ID="lblTitle" runat="server" Text="" meta:resourcekey="lblTitle"></asp:Label></h3>
            </div>
        </div>
    </div>
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12">
                <div class="card">
                    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="card-body">
                                <form method="post" id="changePasswordForm">  <%-- Add ID to the form --%>
                                    <div class="row p-b-15 d-flex no-block justify-content-center align-items-center">
                                        <div class="col-4">
                                            <div class="col-6 input-group mb-2" id="dvOldPwd" runat="server" visible="false">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text bg-warning text-white"><i class="ti-pencil"></i></span>
                                                </div>
                                                <input type="password" id="txtOldPassword" runat="server" tabindex="1" class="form-control col-4" title="Existing Password" maxlength="36" min="3" placeholder="Existing Password" aria-label="Existing Password" aria-describedby="basic-addon1" meta:resourcekey="txtOldPass" />
                                            </div>
                                            <div class="col-6 input-group mb-2">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text bg-warning text-white" id="basic-addon2"><i class="ti-pencil"></i></span>
                                                </div>
                                                <input type="password" id="txtNewPassword" runat="server" tabindex="2" class="form-control col-4" title="New Password" pattern="^(?=.*[A-Z])(?=.*[^\da-zA-Z]).{8,36}$" maxlength="36" min="8" placeholder="New Password" aria-label="New Password" aria-describedby="basic-addon1" meta:resourcekey="txtCurPass" oninvalid="this.setCustomValidity('Password must meet the following criteria: at least 8 characters, one uppercase letter, one lowercase letter and one special character.')"/>
                                            </div>
            
                                            <div class="col-6 input-group mb-2">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text bg-warning text-white" id="Span2"><i class="ti-pencil"></i></span>
                                                </div>
                                                <input type="password" id="txtConfPassword" runat="server" tabindex="3" class="form-control col-4" placeholder="Confirm Password" aria-label="Confirm User Password" aria-describedby="basic-addon1" meta:resourcekey="txtConfPass" title="Confirm New Password" pattern="^(?=.*[A-Z])(?=.*[^\da-zA-Z]).{8,36}$" maxlength="36" min="8" oninvalid="this.setCustomValidity('Password must meet the following criteria: at least 8 characters, one uppercase letter, one lowercase letter and one special character.')"/>
                                            </div>
                                            <div runat="server" id="alert_div" class="alert alert-danger fade show" role="alert" visible="false">
                                                <asp:Label runat="server" ID="lbl_alert"></asp:Label>
                                            </div>
                                            <div class="row form-group justify-content-center">
                                                <asp:Button ID="btnChangePass" runat="server" Text="Update Password" OnClick="btnChangePass_Click"  TabIndex="4" CssClass="col-sm-3 btn btn-lg btn-info" meta:resourcekey="btnChangePassResource1Text" />
                                                &nbsp;
                                                <asp:Button ID="btnCancel" runat="server" tabindex="99" CssClass="col-4 btn btn-lg btn-secondary" Text="Close" CausesValidation="false" UseSubmitBehavior="false" ValidationGroup="none" OnClick="btnCancel_Click" meta:Resourcekey="btnCloseResource1Text" />
                                            </div>
                                        </div>
                                    </div>
                                </form>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
     <script type="text/javascript" src="dist/js/default.js"></script>
    <script type="text/javascript" src="dist/js/sha.js"></script>
    <script>
        $(".preloader").fadeOut();
        this.setCustomValidity('Password must meet the following criteria: at least 8 characters, one uppercase letter, one lowercase letter, one digit, and one special character.');
        function CheckLen() {
            if ($('#txtNewPassword').val().length < 8) {
                alert('Password should be 8 digit long !');
                $("#lnkhelp").focus();
                return false;
            }
        }
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
    </script>
</asp:Content>

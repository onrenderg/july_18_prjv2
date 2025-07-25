<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="DeactivateMaster.aspx.vb" Inherits="SEC_InventoryMgmt.DeactivateMaster" %>
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
                <h4 class="page-title"> 
                    <asp:Label ID="lblTitle" runat="server" meta:resourcekey="lblTitleResource1"></asp:Label></h4>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="card">
            <asp:UpdatePanel runat="server" ID="panel1" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="card-body">
                        <div class="form-group row" id="dvPri" runat="server">
                            <asp:Label runat="server" CssClass="col-sm-2 text-md-right" meta:resourcekey="electionFor" />
                            <div class="col-lg-8 col-md-10 col-sm-10">
                                <asp:Label ID="lblElectionFor" runat="server"  class="col-sm-2 text-md-right fw-bold" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label for="rbUnitType"  runat="server" class="col-sm-2 text-md-right" meta:resourcekey="rbUnitType" />
                            <div class="col-lg-8 col-md-10 col-sm-10">
                                <asp:Label ID="lblrbUnitType" runat="server"  class="col-sm-2 text-md-right fw-bold"/>
                            </div>
                        </div>
                    </div>
                    <hr class="p-0 m-0"/>
                    <div id="dvList" runat="server" class="card-body">
                        <form id="Edit_Form" method="post"> 
                            <asp:HiddenField runat="server" ID="hidden_id" />
                            <asp:HiddenField runat="server" ID="hidden_itemType" />
                            <asp:HiddenField runat="server" ID="hidden_itemFor" />
                            <asp:HiddenField runat="server" ID="hidden_itemSpecification" />
                            <div class="row mb-1">
                                <div class="form-group col-12 col-sm-12 col-md-12 col-lg-4">
                                    <label for="ItemName">Item Name</label>
                                    <asp:TextBox ID="ItemName" runat="server" CssClass="form-control" placeholder="Enter Item Name" required="true" ReadOnly="true" Enabled="false"/>
                                </div>
                                <div class="form-group  col-12 col-sm-12 col-md-12 col-lg-8">
                                    <label for="ItemDescription">Item Description</label>
                                    <asp:TextBox ID="ItemDescription" runat="server" CssClass="form-control" placeholder="Enter Item Description" required="true" ReadOnly="true" Enabled="false"/>
                                </div>
                            </div>
                            <div class="row mb-1">
                                <div class="form-group  col-12 col-sm-12 col-md-12 col-lg-4">
                                    <label for="ItemNameLocal">आइटम का नाम</label>
                                    <asp:TextBox ID="ItemNameLocal" runat="server" CssClass="form-control" placeholder="आइटम का नाम दर्ज करें" required="true" ReadOnly="true" Enabled="false"/>
                                </div>
                                <div class="form-group  col-12 col-sm-12 col-md-12 col-lg-8">
                                    <label for="ItemDescriptionLocal">आइटम विवरण</label>
                                    <asp:TextBox ID="ItemDescriptionLocal" runat="server" CssClass="form-control" placeholder="आइटम विवरण दर्ज करें" required="true" ReadOnly="true" Enabled="false"/>
                                </div>
                            </div>
                            <div class="row mb-1">
                                <div class="form-group  col-12 col-sm-12 col-md-12 col-lg-12">
                                    <asp:Label runat="server" CssClass="col-sm-2 text-md-right" meta:resourcekey="lbl_deactivation_remakrs" />
                                    <asp:TextBox ID="de_activation_remakrs" runat="server" CssClass="form-control" placeholder="दर्ज करें"/>
                                </div>
                            </div>

                            <div runat="server" id="alert_div" class="alert alert-danger fade show" role="alert">
                                <asp:Label runat="server" ID="lbl_alert" ></asp:Label>
                            </div>
                            <div class="text-center">
                                <asp:Button ID="submitButton" runat="server" Text="De-Activate Master" class="btn btn-primary text-center" meta:resourcekey="submitButton"/>
                            </div>
                        </form>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>

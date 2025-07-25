<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="allocateEVMWH.aspx.vb" Inherits="SEC_InventoryMgmt.allocateEVMWH" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        input[type=radio] {
            margin-left: 30px;
        }

            input[type=radio]:first-child {
                margin-left: 10px;
            }
    </style>
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
                         <div class="form-group row">
                            <label for="rbUnitType" class="col-sm-2 text-md-right"><%= GetLocalResourceObject("rbUnitType") %></label>
                            <div class="col-lg-4 col-md-6 col-sm-8">
                                <asp:RadioButtonList ID="rbUnitType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbUnitType_SelectedIndexChanged" CssClass="col-12 select2-container--default" meta:resourcekey="rbUnitTypeResource1">
                                </asp:RadioButtonList>
                            </div>
                        </div>
                         <div class="form-group row">
                            <label for="rbDis_wh" class="col-sm-2 text-md-right"><%= GetLocalResourceObject("rbDis_wh") %></label>
                            <div class="col-lg-4 col-md-6 col-sm-8">
                                <asp:RadioButtonList ID="rbDis_wh" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbDis_wh_SelectedIndexChanged" CssClass="col-12 select2-container--default" meta:resourcekey="rbUnitTypeResource1">
                                    <asp:ListItem Text="District" Value="D" Selected="True" meta:resourcekey="lstDis" />
                                    <asp:ListItem Text="Warehouse" Value="W" meta:resourcekey="lstWarehouse" />
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label ID="lblWh" runat="server" CssClass="col-sm-2 text-md-right" meta:resourcekey="lblWh"></asp:Label>
                            <div class="col-sm-3">
                                <asp:DropDownList ID="ddlWH" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlWH_SelectedIndexChanged"  meta:resourcekey="ddlWH"
                                    ToolTip="Warehouse Name">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label ID="lblDis" runat="server" CssClass="col-sm-2 text-md-right" meta:resourcekey="lblDis"></asp:Label>
                            <div class="col-sm-3">
                                <asp:DropDownList ID="ddlDis" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlDis_SelectedIndexChanged" meta:resourcekey="lblddlDis"
                                    ToolTip="District Name">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div id="dvList" runat="server" class="card-body">
                        <div class="table-responsive">
                            <table id="Table2" class="table table-striped table-bordered">
                                <thead style="background-color: #000F60; color: whitesmoke; font-weight: bold;">
                                    <tr>
                                        <th><%= GetLocalResourceObject("lblUnmapped") %></th>
                                        <th></th>
                                        <th><%= GetLocalResourceObject("lblMapped") %></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td colspan="3" style="text-align:right;">
                                            <asp:Label ID="lblMappingCount" runat="server" Text="" CssClass="col-sm-8 text-info align-bottom"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%;">
                                            <asp:ListBox ID="lbox_unmapped_panchayat" runat="server" Rows="10" CssClass="col-sm-12" SelectionMode="Multiple"></asp:ListBox></td>
                                        <td style="text-align: center; vertical-align: middle; width: 5%;">
                                            <asp:Button ID="right_button_all" CssClass="col-sm-5 btn-primary" OnClick="right_button_all_Click" runat="server" Text=">>"></asp:Button><br />
                                            <br />
                                            <asp:Button ID="right_button" CssClass="col-sm-5 btn-primary" OnClick="right_button_Click" runat="server" Text=">"></asp:Button><br />
                                            <br />
                                            <asp:Button ID="left_button" CssClass="col-sm-5 btn-primary" OnClick="left_button_Click" runat="server" Text="<"></asp:Button><br />
                                            <br />
                                            <asp:Button ID="left_button_all" CssClass="col-sm-5 btn-primary" OnClick="left_button_all_Click" runat="server" Text="<<"></asp:Button>
                                        </td>
                                        <td style="width: 30%;">
                                            <asp:ListBox ID="lbox_mapped_in_ward" runat="server" Rows="10" CssClass="col-sm-12" SelectionMode="Multiple"></asp:ListBox></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div class="col-sm-12 row justify-content-center">
                            <asp:Label ID="lbl_error" CssClass="text-danger" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblStatus" CssClass="text-success" runat="server" Text="Record saved successfully" Visible="false"></asp:Label>
                            <asp:UpdateProgress AssociatedUpdatePanelID="panel1" ID="uprog1" runat="server">
                                <ProgressTemplate>
                                    <script>
                                          $(".preloader").fadeIn();
                                    </script>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>
                        <div class="col-sm-12 row justify-content-center">
                            <asp:Button ID="save_button" runat="server" Text="Save" CssClass="col-sm-2 btn btn-md btn-info" meta:ResourceKey="btnSave"></asp:Button>
                            &nbsp;
                            <asp:Button ID="cancel_button" runat="server" Text="Cancel" CausesValidation="false" UseSubmitBehavior="false" CssClass="col-sm-2 btn btn-md btn-secondary" meta:ResourceKey="btnCancel"></asp:Button>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="rbUnitType" />
                    <asp:PostBackTrigger ControlID="save_button" />
                    <asp:PostBackTrigger ControlID="cancel_button" />
                    <asp:PostBackTrigger ControlID="ddlDis" />

                    <asp:AsyncPostBackTrigger ControlID="right_button_all" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="right_button" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="left_button" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="left_button_all" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>

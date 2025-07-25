<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="viewWarehouse.aspx.vb" Inherits="SEC_InventoryMgmt.viewWarehouse" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

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
                <asp:HiddenField ID="hdnLangCode" runat="server" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="card">
            <asp:UpdatePanel runat="server" ID="panel1" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="card-body">
                        <div id="dvList" runat="server" class="col-12 card-body">
                            <div class="col-12">
                                <asp:GridView ID="existing_grid" runat="server" AutoGenerateColumns="false" ShowFooter="true" CssClass="table table-striped table-bordered">
                                    <Columns>
                                        <asp:TemplateField HeaderText="" meta:resourcekey="lblDist">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblDisNameL" Text='<%# Bind("District_Name_local") %>' />
                                                <asp:Label runat="server" ID="lblDisName" Text='<%# Bind("District_Name") %>' Visible="false" />
                                                <asp:Label runat="server" ID="lbldiscode" Visible="false" Text='<%# Bind("District_Code") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" meta:resourcekey="lblWarehouse">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblWareHouseL" Text='<%# Bind("WareHouse_Name_local") %>' />
                                                <asp:Label runat="server" ID="lblWareHouse" Text='<%# Bind("WareHouse_Name") %>' Visible="false" />
                                                <asp:Label runat="server" ID="lblWareHousecode" Visible="false" Text='<%# Bind("WareHouse_Code") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" meta:resourcekey="lblWarehouseAddress">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblWareHouseAddressL" Text='<%# Bind("WareHouse_Address_local") %>' />
                                                <asp:Label runat="server" ID="lblWareHouseAddress" Text='<%# Bind("WareHouse_Address") %>' Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" meta:resourcekey="lblIncharge">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblIncharge" Text='<%# Bind("WH_Incharge_Name") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" meta:resourcekey="lblEmail">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmail" Text='<%# Bind("WH_Incharge_Email") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" meta:resourcekey="lblMobile">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblMobile" Text='<%# Bind("WH_Incharge_Mobile") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    
                                       <%-- <asp:TemplateField HeaderText="CU Units" meta:resourcekey="lblCU">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblCU" Text='<%# Bind("CuUnits") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BU Units" meta:resourcekey="lblBU">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblBU" Text='<%# Bind("BuUnits") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="VVPAT Units" meta:resourcekey="lblVvpatUnits">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblTotalUnits" Text='<%# Bind("VvpatUnits") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>

                                    </Columns>
                                    <HeaderStyle BackColor="#000F60" ForeColor="White" BorderColor="#000F60" />
                                </asp:GridView>
                            </div>
                        </div>

                        <div class="row offset-3">
                            <asp:Label ID="lbl_error" CssClass="col-9 text-danger" runat="server" meta:resourcekey="lbl_errorResource1"></asp:Label>
                            <asp:Label ID="lblStatus" CssClass="col-9 text-success" runat="server" meta:resourcekey="lblStatusResource1"></asp:Label>
                            <asp:UpdateProgress AssociatedUpdatePanelID="panel1" ID="uprog1" runat="server">
                                <ProgressTemplate>
                                    <script>
                                        $(".preloader").fadeIn();
                                    </script>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>
                        <div class="row offset-3 text-center">
                            <asp:Button ID="view_button" runat="server" CausesValidation="False" UseSubmitBehavior="False" Text="View On Map"
                                CssClass="col-sm-2 btn btn-dropbox" OnClick="view_button_Click" meta:resourcekey="view_buttonResource1"></asp:Button>
                            <asp:Button ID="btnExport" runat="server" CssClass="col-sm-2 btn btn-info" Text="Export to Excel" OnClick="ExportExcel" />
                            &nbsp;<asp:Button ID="cancel_button" runat="server" CausesValidation="False" UseSubmitBehavior="False"
                                CssClass="col-sm-2 btn btn-secondary" meta:resourcekey="cancel_buttonResource1"></asp:Button>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <%--<asp:AsyncPostBackTrigger ControlID="rbUnitType" EventName="SelectedIndexChanged" />--%>
                    <asp:PostBackTrigger ControlID="existing_grid" />
                    <asp:PostBackTrigger ControlID="cancel_button" />
                    <asp:PostBackTrigger ControlID="btnExport" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>

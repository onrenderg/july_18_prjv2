<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="updateStationeryStock.aspx.vb" Inherits="SEC_InventoryMgmt.updateStationeryStock" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="assets/libs/datatables.net-bs4/css/dataTables.bootstrap4.css" rel="stylesheet" />
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
                        <div class="form-group row" id="dvPri" runat="server">
                            <asp:Label ID="lblElectionFor" runat="server" CssClass="col-sm-2 text-md-right" Font-Bold="true" Text="Election Type" meta:resourcekey="electionFor" />
                            <div class="col-lg-8 col-md-10 col-sm-10">
                                <asp:RadioButtonList ID="rbPriPost" CssClass="col-12 select2-container--default" runat="server"
                                    RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbPriPost_SelectedIndexChanged">
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="rbUnitType" class="col-sm-2 text-md-right"><%= GetLocalResourceObject("rbUnitType") %></label>
                            <div class="col-lg-8 col-md-10 col-sm-10">
                                <asp:RadioButtonList ID="rbUnitType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbUnitType_SelectedIndexChanged" CssClass="col-12 select2-container--default" meta:resourcekey="rbUnitTypeResource1">
                                </asp:RadioButtonList>
                            </div>
                        </div>

                        <div class="form-group row">
                            <asp:Label ID="lblDis" runat="server" CssClass="col-sm-2 text-md-right" meta:resourcekey="lblDis" Visible="false"></asp:Label>
                            <div class="col-lg-5 col-md-6 col-sm-6">
                                <asp:DropDownList ID="ddlDis" Visible="false" runat="server" CssClass="form-control" Enabled="false" meta:resourcekey="lblddlDis"
                                    ToolTip="Your District">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div id="dvList" runat="server" class="card-body">
                        <div class="table-responsive">
                            <asp:GridView ID="existing_grid" runat="server" AutoGenerateColumns="false" OnRowCommand="existing_grid_RowCommand"
                                CssClass="table table-striped table-bordered" OnRowDeleting="existing_grid_RowDeleting">
                                <Columns>
                                    <asp:TemplateField HeaderText="Item Name" meta:resourcekey="TemplateFieldResource0">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="id" Text='<%# Bind("id") %>' Visible="false" />
                                            <asp:Label runat="server" ID="lblName" Text='<%# Bind("ItemName") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Description" meta:resourcekey="TemplateFieldResource1">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="Itemdescription" Text='<%# Bind("Itemdescription") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="30%" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Available Stock" meta:resourcekey="TemplateFieldResource2">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="lblItemCounts" Text='<%# Bind("ItemInStock") %>' TextMode="Number" CssClass="form-control" PlaceHolder="No. of Items" />
                                            <asp:Label runat="server" ID="lblCnt" CssClass="row" Visible="false" />
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="From Sr. No." meta:resourcekey="TemplateFieldResource3">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="lblFromSrNo" TextMode="Number" CssClass="form-control" PlaceHolder="From Sr. No." Text='<%# Bind("FromSrNo") %>' />
                                            <asp:Label runat="server" ID="lblFrmSrNo" CssClass="row" Visible="false" Text='<%# Bind("FromSrNo") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="To Sr. No." meta:resourcekey="TemplateFieldResource4">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="lblToSrNo" TextMode="Number" CssClass="form-control" PlaceHolder="To Sr. No." Text='<%# Bind("ToSrNo") %>' />
                                            <asp:Label runat="server" ID="lblToSr" CssClass="row" Visible="false" Text='<%# Bind("ToSrNo") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#000F60" ForeColor="White" BorderColor="#000F60" />
                            </asp:GridView>
                        </div>
                        <div class="col-12 row justify-content-center">
                            <asp:Label ID="lbl_error" CssClass="col-12 align-content-center text-danger" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblStatus" CssClass="text-success" runat="server" Text="Record saved successfully" Visible="false"></asp:Label>
                            <asp:UpdateProgress AssociatedUpdatePanelID="panel1" ID="uprog1" runat="server">
                                <ProgressTemplate>
                                    <script>
                                        $(".preloader").fadeIn();
                                    </script>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>
                        <div class="col-12 row justify-content-center">
                            <asp:Button ID="save_button" runat="server" Text="Save" CssClass="col-sm-2 btn btn-md btn-info" meta:ResourceKey="btnSave"></asp:Button>
                            &nbsp;
                            <asp:Button ID="cancel_button" runat="server" Text="Cancel" CausesValidation="false" UseSubmitBehavior="false" CssClass="col-sm-2 btn btn-md btn-secondary" meta:ResourceKey="btnCancel"></asp:Button>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="rbPriPost" />
                    <asp:PostBackTrigger ControlID="rbUnitType" />
                    <asp:PostBackTrigger ControlID="existing_grid" />
                    <asp:PostBackTrigger ControlID="save_button" />
                    <asp:PostBackTrigger ControlID="cancel_button" />

                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
    <script src="assets/extra-libs/DataTables/datatables.min.js"></script>
    <script>
        $(document).ready(function () {
            var gv = $('#<%=existing_grid.ClientID%>');
            var thead = $('<thead/>');
            thead.append(gv.find('tr:eq(0)'));
            gv.append(thead);
            gv.dataTable({
                "sPaginationType": "full_numbers",
                "order": [],
                "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
                "bDestroy": true
            });

        });
    </script>
</asp:Content>

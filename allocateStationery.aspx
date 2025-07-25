<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="allocateStationery.aspx.vb" Inherits="SEC_InventoryMgmt.allocateStationery" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

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
                            <asp:Label ID="lblElectionFor" runat="server" CssClass="col-sm-2 text-md-right" Text="Election Type" meta:resourcekey="electionFor" />
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
                            <asp:Label ID="lblDis" runat="server" CssClass="col-sm-2 text-md-right" meta:resourcekey="lblDis"></asp:Label>
                            <div class="col-lg-5 col-md-6 col-sm-6">
                                <asp:DropDownList ID="ddlDis" runat="server" CssClass="form-control" Enabled="false" meta:resourcekey="lblddlDis"
                                    ToolTip="District Name" AutoPostBack="true" OnSelectedIndexChanged="ddlDis_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group row" id="dvBlock" runat="server">
                            <asp:Label ID="lblBlk" runat="server" CssClass="col-sm-2 text-md-right"></asp:Label>
                            <div class="col-lg-5 col-md-6 col-sm-6">
                                <asp:DropDownList ID="ddlBlk" runat="server" CssClass="form-control" meta:resourcekey="lblLevelPS" ToolTip="Choose Block"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlBlk_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>

                    </div>
                    <div id="dvList" runat="server" visible="false">
                        <div class="col-12 text-end">
                            <asp:Label ID="lblMappingCount" runat="server" Text="" class="text-end text-info me-3"></asp:Label>
                        </div>
                        <div class="table-responsive">
                            <asp:GridView ID="existing_grid" runat="server" AutoGenerateColumns="false" OnRowCommand="existing_grid_RowCommand"
                                Class="table table-striped table-bordered" OnRowDeleting="existing_grid_RowDeleting">
                                <Columns>
                                    <asp:TemplateField HeaderText="Item Name" meta:resourcekey="TemplateFieldResource0">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="id" Text='<%# Bind("id") %>' Visible="false" />
                                            <asp:Label runat="server" ID="lblName" Text='<%# Bind("ItemName") %>' />
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Description" meta:resourcekey="TemplateFieldResource1">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="Itemdescription" Text='<%# Bind("Itemdescription") %>' />
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User Stock" meta:resourcekey="TemplateFieldResource5">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lbl_user_stock" Text='<%# Bind("items_allocated_To_selected_user") %>' CssClass="col-4" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Stock Available [From SrNo - To SrNo]" meta:resourcekey="TemplateFieldResource2">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblStockCnt" Text='<%# Bind("total_available_stock") %>' CssClass="col-4" />
                                            &nbsp;<asp:Label runat="server" ID="lblFrmSrNo" CssClass="col-2" Visible="false" ToolTip="Starting SrNo Available to allocate" Text='<%# Bind("FromSrNo") %>' />
                                            <asp:Label runat="server" ID="lblToSr" CssClass="col-2" Visible="false" ToolTip="Ending SrNo Available to allocate" Text='<%# Bind("ToSrNo") %>' />
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="From Sr. No." meta:resourcekey="TemplateFieldResource3">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="lblFromSrNo" TextMode="Number" CssClass="form-control" PlaceHolder="From Sr. No." Text='<%# Bind("AllotedFromSrNo")  %>' />
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="To Sr. No." meta:resourcekey="TemplateFieldResource4">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="lblToSrNo" TextMode="Number" CssClass="form-control" PlaceHolder="To Sr. No." Text='<%# Bind("AllotedToSrNo")  %>' />
                                        </ItemTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Item Allotted" meta:resourcekey="TemplateFieldResource6">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="lblItemCounts" TextMode="Number" CssClass="form-control" PlaceHolder="No. if Items" MaxLength='<%# Bind("total_available_stock") %>' />
                                            <asp:Label runat="server" ID="lblCnt" CssClass="row" Visible="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#000F60" ForeColor="White" BorderColor="#000F60" />
                            </asp:GridView>
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
                    <asp:PostBackTrigger ControlID="rbPriPost" />
                    <asp:PostBackTrigger ControlID="ddlDis" />
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
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(function () {
            });
        });
    </script>

</asp:Content>

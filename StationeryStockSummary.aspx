<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="StationeryStockSummary.aspx.vb" Inherits="SEC_InventoryMgmt.StationeryStockSummary" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

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
                                <asp:DropDownList ID="rbUnitType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbUnitType_SelectedIndexChanged" CssClass="select2 form-control custom-select select2-hidden-accessible" meta:resourcekey="rbUnitTypeResource1">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group row" id="DV" runat="server" visible="false">
                            <label for="rbBallotBoxStationery" runat="server" class="col-sm-2 text-md-right"><%= GetLocalResourceObject("rbBallotBoxStationery") %></label>
                            <div class="col-lg-8 col-md-10 col-sm-10">
                                <asp:RadioButtonList ID="rbBallotBoxStationery" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbBallotBoxStationery_SelectedIndexChanged" CssClass="col-12 select2-container--default" meta:resourcekey="rbBallotBoxStationery">
                                </asp:RadioButtonList>
                            </div>
                        </div>

                        <div class="form-group row">
                            <asp:Label ID="lblDis" runat="server"  Font-Bold="true" CssClass="col-sm-2 text-md-right" meta:resourcekey="lblDis"></asp:Label>
                            <div class="col-lg-5 col-md-6 col-sm-6">
                                <asp:DropDownList ID="ddlDis" runat="server" CssClass="form-control" meta:resourcekey="lblddlDis"
                                    ToolTip="District Name" AutoPostBack="true" OnSelectedIndexChanged="ddlDis_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group row" id="dvBlock" runat="server">
                            <asp:Label ID="lblBlk" runat="server"  Font-Bold="true" CssClass="col-sm-2 text-md-right"></asp:Label>
                            <div class="col-lg-5 col-md-6 col-sm-6">
                                <asp:DropDownList ID="ddlBlk" runat="server" CssClass="form-control" meta:resourcekey="lblLevelPS" ToolTip="Choose Block"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlBlk_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div id="dvList" runat="server">
                        <div class="table-responsive">
                            <asp:GridView ID="existing_grid" runat="server" AutoGenerateColumns="false" OnRowCommand="existing_grid_RowCommand"
                                CssClass="table table-striped table-bordered" OnRowDeleting="existing_grid_RowDeleting" HeaderStyle-VerticalAlign="Top">
                                <Columns>
                                    <asp:TemplateField HeaderText="Item Name" meta:resourcekey="TemplateFieldResource0">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="id" Text='<%# Bind("id") %>' Visible="false" />
                                            <asp:Label runat="server" ID="lblName" Text='<%# Bind("ItemName") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Description" meta:resourcekey="TemplateFieldResource1">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="Itemdescription" Text='<%# Bind("Itemdescription") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>

                                    
                                    <asp:TemplateField HeaderText="Item Name" meta:resourcekey="TemplateFieldResource2">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="MyEnteredStock" Text='<%# Bind("MyEnteredStock") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Name" meta:resourcekey="TemplateFieldResource3">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="AllocatedToMe" Text='<%# Bind("AllocatedToMe") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Name" meta:resourcekey="TemplateFieldResource8">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="MyTotalStock" Text='<%# Bind("MyTotalStock") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Item Name" meta:resourcekey="TemplateFieldResource5">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="AllocatedToOthers" Text='<%# Bind("AllocatedToOthers") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Name" meta:resourcekey="TemplateFieldResource6">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="TotalConsumed" Text='<%# Bind("TotalConsumed") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Name" meta:resourcekey="TemplateFieldResource7">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="Disposed" Text='<%# Bind("Disposed") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Name" meta:resourcekey="TemplateFieldResource4">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="AvailableForAllocation" Text='<%# Bind("AvailableForAllocation") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
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
                        <div class="col-12 row justify-content-center mb-2">
                            <asp:Button ID="btnExport" runat="server" CssClass="col-sm-2 btn btn-info" Text="Export to Excel" OnClick="ExportExcel"  meta:ResourceKey="export_buttonResource"/>
                            &nbsp;
                            <%--<asp:Button ID="save_button" runat="server" Text="Save" CssClass="col-sm-2 btn btn-md btn-info" meta:ResourceKey="save_buttonResource1"></asp:Button>
                            &nbsp;--%>
                            <asp:Button ID="cancel_button" runat="server" Text="Cancel" CausesValidation="false" UseSubmitBehavior="false" CssClass="col-sm-2 btn btn-md btn-secondary" meta:ResourceKey="cancel_buttonResource1"></asp:Button>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="rbPriPost" />
                    <asp:PostBackTrigger ControlID="rbUnitType" />
                    <%--<asp:AsyncPostBackTrigger ControlID="existing_grid" />
                    <asp:PostBackTrigger ControlID="save_button" />--%>
                    <asp:PostBackTrigger ControlID="cancel_button" />
                    <asp:PostBackTrigger ControlID="btnExport" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
    <script src="assets/extra-libs/DataTables/datatables.min.js"></script>
    <script type="text/javascript">
        function pageLoad() {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(function () {
                initializeDataTable(); // Call your DataTable function
            });
            initializeDataTable(); // Call on initial page load
        }

        function initializeDataTable() {
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
        }

    </script>
</asp:Content>

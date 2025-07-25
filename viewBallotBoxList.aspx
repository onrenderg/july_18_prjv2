<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="viewBallotBoxList.aspx.vb" Inherits="SEC_InventoryMgmt.viewBallotBoxList" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

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
                    <div id="dvList" runat="server">
                        <div class="table-responsive">
                            <asp:GridView ID="existing_grid" runat="server" AutoGenerateColumns="false" OnRowCommand="existing_grid_RowCommand"
                                CssClass="table table-striped table-bordered" HeaderStyle-VerticalAlign="Top">
                                <Columns>
                                    <asp:TemplateField HeaderText="Item Description" meta:resourcekey="TemplateFieldResource0">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="qr_text" Text='<%# Bind("qr_text") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="40%" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Item Location" meta:resourcekey="TemplateFieldResource1">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="to_user_code" Text='<%# Bind("to_user_code") %>' Visible="false" />
                                            <asp:Label runat="server" ID="Display_Name" Text='<%# Bind("Display_Name") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="30%" />
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HyperLink runat="server" ID="lnkDetails"
                                                Text='<%# Bind("button_text") %>'
                                                NavigateUrl='<%# String.Format("viewBBonMap.aspx?qr_text={0}&to_user_code={1}", Eval("qr_text"), Eval("to_user_code")) %>'
                                                Target="_blank"
                                                Enabled='<%# Not (String.IsNullOrEmpty(Eval("latitude")?.ToString()) OrElse Convert.ToDecimal(Eval("latitude")) <= 1) %>'></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button runat="server" ID="btnViewQR"  CssClass="btn btn-sm btn-success grid-button" CausesValidation="false" Text="View QR" CommandName="ShowQR" CommandArgument='<%# Bind("qr_text") %>'  meta:Resourcekey="btnViewQR"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <HeaderStyle BackColor="#000F60" ForeColor="White" BorderColor="#000F60" />
                            </asp:GridView>
                        </div>
                        
                        <div class="col-12 row justify-content-center mb-2">
                            <asp:Button ID="btnExport" runat="server" CssClass="col-sm-2 btn btn-info" Text="Export to Excel" OnClick="ExportExcel"  meta:ResourceKey="export_buttonResource"/>
                            &nbsp;
                            <asp:Button ID="map_button" runat="server" Text="View All On Map" CssClass="col-sm-2 btn btn-primary" meta:ResourceKey="map_buttonResource" OnClick="map_button_Click"></asp:Button>
                            &nbsp;
                            <asp:Button ID="cancel_button" runat="server" Text="Cancel" CausesValidation="false" UseSubmitBehavior="false" CssClass="col-sm-2 btn btn-md btn-secondary" meta:ResourceKey="cancel_buttonResource1"></asp:Button>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
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

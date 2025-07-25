<%@ Page Title="" Language="vb" AutoEventWireup="false"  MasterPageFile="~/Site.Master"  CodeBehind="updateStationeryMasters.aspx.vb" Inherits="SEC_InventoryMgmt.updateStationeryMasters"  Culture="auto" meta:resourcekey="PageResource1" UICulture="auto"  %>

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

                        <div class="form-group row" >
                            <asp:Label ID="lblDis" runat="server" CssClass="col-sm-2 text-md-right" meta:resourcekey="lblDis" Visible="false"></asp:Label>
                            <div class="col-lg-5 col-md-6 col-sm-6">
                                <asp:DropDownList ID="ddlDis" Visible="false" runat="server" CssClass="form-control" Enabled="false" meta:resourcekey="lblddlDis"
                                    ToolTip="Your District">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-12 row justify-content-center">
                            <asp:Button ID="btn_add_new" runat="server" OnClick="btn_add_new_Click" CssClass="col-sm-2 btn btn-md btn-info" meta:ResourceKey="btn_add_new"></asp:Button>
                        </div>
                    <div id="dvList" runat="server" >
                        <div class="table-responsive">
                            <asp:GridView ID="existing_grid" runat="server" AutoGenerateColumns="false" OnRowCommand="existing_grid_RowCommand"
                                CssClass="table table-striped table-bordered" OnRowDeleting="existing_grid_RowDeleting">
                                <Columns>
                                    <asp:TemplateField HeaderText="Item Name" meta:resourcekey="TemplateFieldResource0">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="id" Text='<%# Bind("id") %>' Visible="false" />
                                            <asp:Label runat="server" ID="lblName" Text='<%# Bind("ItemName") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Description" meta:resourcekey="TemplateFieldResource1">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="Itemdescription" Text='<%# Bind("Itemdescription") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="60%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actions">
    <ItemTemplate>
        <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" CommandArgument='<%# Bind("id") %>' Text='<i class="mdi mdi-pencil-box-outline"></i>'><%# Resources.Resource.UpdateStockMaster %></asp:LinkButton>
        <br />
       
        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="text-danger" CommandName='<%# IIf(Eval("is_active") = False, "ReActivate", "DeActivate") %>' CommandArgument='<%# Bind("id") %>' Text='<i class="mdi mdi-delete-empty"></i>'>
           <%# 
                IIf(Eval("is_active") = False, Resources.Resource.ReActivateStockMaster, Resources.Resource.DeActivateStockMaster)
            %>
        </asp:LinkButton>
    </ItemTemplate>
    <ItemStyle Width="20%" />
</asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#000F60" ForeColor="White" BorderColor="#000F60" />
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="rbPriPost" />
                    <asp:PostBackTrigger ControlID="rbUnitType" />
                    <asp:PostBackTrigger ControlID="existing_grid" />
                    <%--<asp:PostBackTrigger ControlID="btn_add_new" />
                    <asp:PostBackTrigger ControlID="cancel_button" />--%>

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

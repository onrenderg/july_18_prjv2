<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="BallotBoxAllocation.aspx.vb" Inherits="SEC_InventoryMgmt.BallotBoxAllocation" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

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
                    <asp:Label ID="lblTitle" runat="server" meta:resourcekey="lblTitleResource1"></asp:Label>
                </h4>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="card">
            <asp:UpdatePanel runat="server" ID="panel1" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="card-body">
                        <div class="form-group row" id="div_state" runat="server">
                            <asp:Label ID="lblState" runat="server" CssClass="col-sm-2 text-md-right" meta:resourcekey="lblState"></asp:Label>
                            <div class="col-lg-5 col-md-6 col-sm-6">
                                <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control"
                                    ToolTip="State" AutoPostBack="true" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" >
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group row" id="div_district" runat="server" visible="false">
                            <asp:Label ID="lblDis" runat="server" CssClass="col-sm-2 text-md-right" meta:resourcekey="lblDis"></asp:Label>
                            <div class="col-lg-5 col-md-6 col-sm-6">
                                <asp:DropDownList ID="ddlDis" runat="server" CssClass="form-control" meta:resourcekey="lblddlDis"
                                    ToolTip="District Name" AutoPostBack="true" OnSelectedIndexChanged="ddlDis_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <%--<div class="form-group row" id="div_warehouse" runat="server" visible="false" >
                            <asp:Label ID="lblWerehouse" runat="server" CssClass="col-sm-2 text-md-right" meta:resourcekey="lblWarehouse"></asp:Label>
                            <div class="col-lg-5 col-md-6 col-sm-6">
                                <asp:DropDownList ID="ddlWarehouse" runat="server" CssClass="form-control"
                                    ToolTip="Warehouse" AutoPostBack="true" OnSelectedIndexChanged="ddlWarehouse_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>--%>
                        <div class="form-group row" id="div_block" runat="server" visible="false">
                            <asp:Label ID="lblBlk" runat="server" CssClass="col-sm-2 text-md-right" Text="Block" meta:resourcekey="lblBlock" ></asp:Label>
                            <div class="col-lg-5 col-md-6 col-sm-6">
                                <asp:DropDownList ID="ddlBlk" runat="server" CssClass="form-control" ToolTip="Choose Block"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlBlk_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group row" id="div_Polparty" runat="server" visible="false">
                            <asp:Label ID="lblPolparty" runat="server" CssClass="col-sm-2 text-md-right" meta:resourcekey="lblPolparty" ></asp:Label>
                            <div class="col-lg-5 col-md-6 col-sm-6">
                                <asp:DropDownList ID="ddlPolparty" runat="server" CssClass="form-control" ToolTip="Choose Pol Party"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlPolparty_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div id="dvList" runat="server">
                        <div class="row">
                            <div class="col-12 col-sm-12 col-md-12 col-lg-6">
                                <div class="table-responsive">
                            <asp:GridView ID="existing_grid" runat="server" AutoGenerateColumns="false" OnRowCommand="existing_grid_RowCommand"
                                CssClass="table table-striped table-bordered" OnRowDeleting="existing_grid_RowDeleting" DataKeyNames="qr_text_description,to_user_id,from_user_id,is_received">
                                <Columns>
                                    <asp:BoundField DataField="qr_text_description" meta:resourcekey="TemplateFieldResource1"  ItemStyle-Width="100%" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="RowButton" runat="server" Text="Allocate" class="btn btn-primary" CommandName="AllocateQR" CommandArgument='<%# Eval("qr_text_description") + "," + Eval("to_user_id") + "," + Eval("from_user_id") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblNoRow" runat="server" Text="No Ballot Box For Allocation" />
                                </EmptyDataTemplate>
                                <HeaderStyle BackColor="#000F60" ForeColor="White" BorderColor="#000F60" />
                                <EmptyDataRowStyle BackColor="#000F60" ForeColor="White" BorderColor="#000F60" />
                            </asp:GridView>
                        </div>
                            </div> 
                            <div class="col-12 col-sm-12 col-md-12 col-lg-6">
                                <div class="table-responsive">
                            <asp:GridView ID="existing_grid1" runat="server" AutoGenerateColumns="false" OnRowCommand="existing_grid1_RowCommand"
                                CssClass="table table-striped table-bordered" OnRowDeleting="existing_grid_RowDeleting" DataKeyNames="qr_text_description,to_user_id,from_user_id,is_received">
                                <Columns>
                                    <asp:BoundField DataField="qr_text_description" meta:resourcekey="TemplateFieldResource2" ItemStyle-Width="100%" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="Button1" runat="server" Text="Received" class="btn btn-success" CommandName="AlreadyReceived" CommandArgument='<%# Eval("qr_text_description") + "," + Eval("to_user_id") + "," + Eval("from_user_id") %>' Visible='<%# Convert.ToBoolean(Eval("is_received")) %>' />
                                            <asp:Button ID="RowButton" runat="server" Text="De-Allocate" class="btn btn-danger" CommandName="DeAllocateQR" CommandArgument='<%# Eval("qr_text_description") + "," + Eval("to_user_id") + "," + Eval("from_user_id") %>' Visible='<%# Not Convert.ToBoolean(Eval("is_received")) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblNoRow" runat="server" Text="No Ballot Box Alloted" />
                                </EmptyDataTemplate>
                                <HeaderStyle BackColor="#000F60" ForeColor="White" BorderColor="#000F60" />
                                <EmptyDataRowStyle BackColor="#000F60" ForeColor="White" BorderColor="#000F60" />
                            </asp:GridView>
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
                        <div class="col-12 row justify-content-center mb-2" >
                            <asp:Button ID="save_button" runat="server" Text="Save" CssClass="col-sm-2 btn btn-md btn-info" meta:ResourceKey="save_buttonResource1" style="display:none"></asp:Button>
                            &nbsp;
                            <asp:Button ID="cancel_button" runat="server" Text="Cancel" CausesValidation="false" UseSubmitBehavior="false" CssClass="col-sm-2 btn btn-md btn-secondary" meta:ResourceKey="cancel_buttonResource1"></asp:Button>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="ddlDis" />
                    <asp:PostBackTrigger ControlID="ddlBlk" />
                    <asp:PostBackTrigger ControlID="save_button" />
                    <asp:PostBackTrigger ControlID="cancel_button" />
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
                initialize1DataTable(); // Call your DataTable function
            });
            initializeDataTable(); // Call on initial page load
            initialize1DataTable(); // Call on initial page load
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
        function initialize1DataTable() {
            var gv = $('#<%=existing_grid1.ClientID%>');
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

<%--  
        $(document).ready(function () {
            var gv = $('#<%=existing_grid1.ClientID%>');
              var thead = $('<thead/>');
              thead.append(gv.find('tr:eq(0)'));
              gv.append(thead);
              gv.dataTable();
        });--%>

    </script>
</asp:Content>

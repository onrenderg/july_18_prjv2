<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="addBallotBox_QR_List.aspx.vb" Inherits="SEC_InventoryMgmt.addBallotBox_QR_List" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        th {
            background-color: #000F60;
            color: whitesmoke;
        }

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
                    <asp:Label ID="lblTitle" runat="server"></asp:Label></h4>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="card">
            <%--<asp:UpdatePanel runat="server" ID="panel1" UpdateMode="Conditional">
                <ContentTemplate>--%>
                    <div id="dvList" runat="server">
                        <%--<h4 class="card-title"><%= Resources.Resource.lblListBallotBox %></h4>--%>
                        <div class="table-responsive">
                            <asp:GridView ID="existing_grid" runat="server" AutoGenerateColumns="false" OnRowCommand="existing_grid_RowCommand" CssClass="p-0 table table-striped table-bordered"
                                DataKeyNames="qr_text_description">
                                <Columns>
                                    <asp:BoundField DataField="qr_text_description" HeaderText="QR" meta:resourcekey="TemplateFieldResource2"/>
                                    
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Button runat="server" ID="btnViewQR"  CssClass="btn btn-sm btn-success grid-button" CausesValidation="false" Text="View QR" CommandName="ShowQR" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" meta:Resourcekey="btnViewQR"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                </Columns>
                                <HeaderStyle BackColor="#000F60" ForeColor="White" />
                            </asp:GridView>
                        </div>
                    </div>
               <%-- </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>--%>
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

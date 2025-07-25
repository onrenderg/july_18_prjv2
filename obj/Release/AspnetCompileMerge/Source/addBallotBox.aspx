<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="addBallotBox.aspx.vb" Inherits="SEC_InventoryMgmt.addBallotBox" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

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
                    <asp:Label ID="lblTitle" runat="server" meta:resourcekey="lblTitleResource1"></asp:Label></h4>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="card">
            <asp:UpdatePanel runat="server" ID="panel1" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="card-body" style="display:none">
                        <div id="formNom" runat="server">
                            <div class="form-group row">
                                <asp:Label ID="txtUnitID" runat="server" Visible="false"></asp:Label>
                                <div class="table-responsive">
                                    <table id="zero_config" class="table table-striped table-bordered">
                                        <thead style="background-color: #000F60; color: whitesmoke;">
                                            <tr style="vertical-align: top;">
                                                <%-- --%>
                                                <th><%= GetLocalResourceObject("lblChooseSeries") %>  </th>
                                                <th><%= GetLocalResourceObject("lblModel") %> </th>
                                                <th colspan="2">
                                                    <%= GetLocalResourceObject("lblPurchasedOn") %>
                                                </th>
                                                <th><%= GetLocalResourceObject("lblUnitCount") %>  </th>
                                                <th><%= GetLocalResourceObject("lblFromSrNo") %> </th>
                                                <th><%= GetLocalResourceObject("lblToSrNo") %>  </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td >
                                                    <asp:DropDownList ID="ddlSeries" runat="server" CssClass="col-6 form-control" meta:resourcekey="ddlMonResource1" AutoPostBack="true" OnSelectedIndexChanged="ddlSeries_SelectedIndexChanged" ></asp:DropDownList>
                                                </td>
                                                <td >
                                                    <asp:DropDownList ID="ddlModel" runat="server" CssClass="form-control" meta:resourcekey="ddlModelResource1"></asp:DropDownList></td>
                                                <td >
                                                    <asp:DropDownList ID="ddlMon" runat="server" CssClass="col-6 form-control" meta:resourcekey="ddlMonResource1"></asp:DropDownList>
                                                </td>
                                                <td >
                                                    <asp:DropDownList ID="ddlYr" runat="server" CssClass="col-6 form-control" meta:resourcekey="ddlYrResource1"></asp:DropDownList>
                                                </td>
                                                <td >
                                                    <asp:TextBox ID="txtUnitCounts" runat="server" TextMode="Number" MaxLength="5" CssClass="required form-control" meta:resourcekey="txtUnitSrNoResource1"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" CssClass="text-danger" ControlToValidate="txtUnitCounts" SetFocusOnError="true" ValidationExpression="^[0-9]{1,5}$" ErrorMessage="Only Numbers are allowed"></asp:RegularExpressionValidator>
                                                </td>
                                                <td >
                                                    <asp:TextBox ID="txtFromSrNo" runat="server" TextMode="Number" MaxLength="5" min="1" CssClass="form-control" meta:resourcekey="txtFromSrNoResource1"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" CssClass="text-danger" ControlToValidate="txtFromSrNo" SetFocusOnError="true" ValidationExpression="^[0-9]+$" ErrorMessage="Only Numbers are allowed"></asp:RegularExpressionValidator>
                                                </td>
                                                <td >
                                                    <asp:TextBox ID="txtToSrNo" runat="server" TextMode="Number" MaxLength="5" ReadOnly="true" CssClass="form-control" meta:resourcekey="txtToSrNoResource1"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
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
                            <div class="col-sm-12 row justify-content-center gap-2">
                                <asp:Button ID="save_button" runat="server" CssClass="col-sm-2 btn btn-md btn-info" meta:resourcekey="save_buttonResource1"></asp:Button>
                                <%--<asp:Button ID="cancel_button" runat="server" CausesValidation="False" UseSubmitBehavior="False"
                                    CssClass="col-sm-2 btn btn-md btn-secondary" meta:resourcekey="cancel_buttonResource1"></asp:Button>&nbsp;--%>
                            </div>
                        </div>
                    </div>
                    <div id="dvList" runat="server" class="pt-3">
                        <%--<h4 class="card-title"><%= Resources.Resource.lblListBallotBox %></h4>--%>
                        <div class="table-responsive">
                            <asp:GridView ID="existing_grid" runat="server" AutoGenerateColumns="false" OnRowCommand="existing_grid_RowCommand" 
                                CssClass="table table-striped table-bordered" OnRowDeleting="existing_grid_RowDeleting"
                                DataKeyNames="from_sr_no, to_sr_no, series,box_size">
                                <Columns>
                                    <asp:TemplateField HeaderText="" Visible="false">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkDelete" CssClass="btn btn-sm btn-danger grid-button"
                                                CommandName="delete" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CausesValidation="False"
                                                OnClientClick="return confirmDelete();" ToolTip="Delete" meta:resourcekey="lnkDeleteResource1">
                                                <i class="mdi mdi-lg-4 mdi-delete"></i>&nbsp;<%=Resources.Resource.lnkDelete %></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="series" HeaderText="Series Name" meta:resourcekey="TemplateFieldResource2" />
                                    <asp:BoundField DataField="box_size" HeaderText="Size" meta:resourcekey="TemplateFieldResource3" />
                                    <asp:BoundField DataField="MonYrPurchase" HeaderText="Mon-Year of Purchase" meta:resourcekey="TemplateFieldResource8" />

                                    <asp:BoundField DataField="total_qty" HeaderText="No. of Ballot Boxes" meta:resourcekey="TemplateFieldResource4" />

                                    <asp:TemplateField HeaderText="Sr. No. Range" meta:resourcekey="TemplateFieldResource5">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="sr_no_range" Text='<%# Bind("sr_no_range") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Button runat="server" ID="btnViewQR"  CssClass="btn btn-sm btn-success grid-button" CausesValidation="false" Text="View QR" CommandName="ShowQR" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" meta:Resourcekey="btnViewQR"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="lot_id" Visible="false" />
                                    <asp:BoundField DataField="QR_Generated" Visible="false" />
                                </Columns>
                                <HeaderStyle BackColor="#000F60" ForeColor="White" />
                            </asp:GridView>
                        </div>
                        <div class="col-sm-12 row justify-content-center gap-2">
                                <asp:Button ID="cancel_button" runat="server" CausesValidation="False" UseSubmitBehavior="False"
                                    CssClass="col-sm-2 btn btn-md btn-secondary" meta:resourcekey="cancel_buttonResource1"></asp:Button>&nbsp;
                            </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="ddlSeries" />
                    <asp:PostBackTrigger ControlID="existing_grid" />
                    <asp:PostBackTrigger ControlID="save_button" />
                    <asp:PostBackTrigger ControlID="cancel_button" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">

    <script src="https://code.jquery.com/jquery-3.7.0.min.js"></script>
    <script>
        $(document).ready(function () {

            function calculateToSrNo() {
                var fromSrNo = parseInt($('#<%= txtFromSrNo.ClientID %>').val());
                var unitCounts = parseInt($('#<%= txtUnitCounts.ClientID %>').val());

                // Check if both values are valid numbers
                if (!isNaN(fromSrNo) && !isNaN(unitCounts)) {
                    // Set the calculated value to txtToSrNo (e.g., fromSrNo + unitCounts)
                    var toSrNo = fromSrNo + unitCounts - 1;

                    // Update txtToSrNo value
                    $('#<%= txtToSrNo.ClientID %>').val(toSrNo);
                }
                else {
                    // Clear txtToSrNo if values are invalid
                    $('#<%= txtToSrNo.ClientID %>').val('');
                }
            }
            // Bind the input events to both txtFromSrNo and txtUnitCounts
            $('#<%= txtFromSrNo.ClientID %>, #<%= txtUnitCounts.ClientID %>').on('input', function () {
                calculateToSrNo();
            });
        });
    </script>


    <script type="text/javascript">
        function confirmDelete() {
            var strDel = '<%=Resources.Resource.lblConfirmDelete %>';
            return confirm(strDel);
        }
    </script> 
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

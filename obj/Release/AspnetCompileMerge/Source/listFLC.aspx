<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="listFLC.aspx.vb" Inherits="SEC_InventoryMgmt.listFLC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link
        href="assets/libs/datatables.net-bs4/css/dataTables.bootstrap4.css"
        rel="stylesheet" />
   
    <style>
        th {
            background-color: #000F60;
            color: whitesmoke;
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
                    <asp:Label ID="lblTitle" runat="server" Text="Update FLC Details" meta:resourcekey="lblTitleResource1"></asp:Label></h4>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="card">
            <asp:UpdatePanel runat="server" ID="panel1" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="card-body">
                        <div id="dvList" runat="server" visible="false" class="card-body">
                            <h4 class="card-title"><%= Resources.Resource.lblListFLC %></h4>
                            <div class="table-responsive">
                                <asp:GridView ID="existing_grid" runat="server" AutoGenerateColumns="false" OnRowCommand="existing_grid_RowCommand"
                                    CssClass="table table-striped table-bordered" OnRowDeleting="existing_grid_RowDeleting">
                                    <Columns>
                                        <asp:TemplateField HeaderText="FLC Date" meta:resourcekey="TemplateFieldResource1">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="FLC_Date" Text='<%# Bind("FLC_Date") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Hall Name (In English)" meta:resourcekey="TemplateFieldResource2">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="Hall_Name" Text='<%# Bind("Hall_Name") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Hall Name (In Hindi)" meta:resourcekey="TemplateFieldResource3">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="Hall_Name_Local" Text='<%# Bind("Hall_Name_Local") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Hall Address (In English)" meta:resourcekey="TemplateFieldResource4">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="Hall_Address" Text='<%# Bind("Hall_Address") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Hall Address (In Hindi)" meta:resourcekey="TemplateFieldResource5">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="Hall_Address_Local" Text='<%# Bind("Hall_Address_Local") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" >
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lnkEdit" ForeColor="#FDBE02" CommandName="edit" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" 
                                                    CausesValidation="False" ToolTip="Edit" meta:resourcekey="lnkEditResource1">
                                            <i class="mdi mdi-lg-4 mdi-table-edit"></i><%=Resources.Resource.lnkEdit %></asp:LinkButton>
                                                &nbsp;
                                            <asp:LinkButton runat="server" ID="lnkDelete" ForeColor="#E34724"
                                                CommandName="delete" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CausesValidation="False"
                                                OnClientClick="return confirmDelete();" ToolTip="Delete" meta:resourcekey="lnkDeleteResource1">
                                                <i class="mdi mdi-lg-4 mdi-delete"></i><%=Resources.Resource.lnkDelete %></asp:LinkButton>
                                                <asp:Label runat="server" ID="lblID" Text='<%# Bind("FLC_ID") %>' Visible="false"  />
                                      
                                                </ItemTemplate>
                                        </asp:TemplateField>

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
                        <div id="form1" runat="server" visible="false">
                            <div class="form-group row">
                                <div class="table-responsive">
                                    <table id="zero_config" class="table table-striped table-bordered">
                                        <thead style="background-color: #000F60; color: whitesmoke;">
                                            <tr>
                                                <th><%= GetLocalResourceObject("lblFlcDate") %>  </th>
                                                <th colspan="2"><%= GetLocalResourceObject("lblHallName") %> </th>
                                                <th colspan="2"><%= GetLocalResourceObject("lblHallAddress") %> </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td style="width: 10%;">
                                                    <asp:Label ID="txtFlcID" runat="server" Visible="false" />
                                                    <asp:TextBox ID="txtFlcdate" runat="server" CssClass="required form-control" meta:resourcekey="txtFlcDate"></asp:TextBox>

                                                </td>
                                                <td style="width: 10%;">
                                                    <asp:TextBox ID="txtHallName" runat="server" MaxLength="50" CssClass="required form-control" meta:resourcekey="txtHallName"></asp:TextBox>

                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" CssClass="text-danger"
                                                        ControlToValidate="txtHallName" SetFocusOnError="true" ValidationExpression="^[A-Z0-9., \s]+$" ErrorMessage="Only Text, Numbers are allowed"></asp:RegularExpressionValidator>
                                                </td>
                                                <td style="width: 10%">
                                                    <asp:TextBox ID="txtHallNameL" runat="server" MaxLength="50" CssClass="required form-control" meta:resourcekey="txtHallNameL"></asp:TextBox>

                                                </td>
                                                <td style="width: 10%;">
                                                    <asp:TextBox ID="txtHallAddress" runat="server" MaxLength="250" TextMode="MultiLine" CssClass="form-control" meta:resourcekey="txtHallAddress"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" CssClass="text-danger"
                                                        ControlToValidate="txtHallAddress" SetFocusOnError="true" ValidationExpression="^[A-Za-z0-9., \s ]+$" ErrorMessage="Only Text, Numbers are allowed"></asp:RegularExpressionValidator>
                                                </td>
                                                <td style="width: 10%;">
                                                    <asp:TextBox ID="txtHallAddressL" runat="server" MaxLength="250" TextMode="MultiLine" CssClass="form-control" meta:resourcekey="txtHallAddressL"></asp:TextBox></td>

                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                        </div>
                        <div class="col-sm-12 row offset-3">
                            <asp:Button ID="save_button" runat="server" Visible="false"  CssClass="col-sm-2 btn btn-info" meta:resourcekey="save_buttonResource1"></asp:Button>
                            <asp:Button ID="cancel_button" runat="server" CausesValidation="False" UseSubmitBehavior="False"
                                CssClass="col-sm-2 btn btn-secondary" meta:resourcekey="cancel_buttonResource1"></asp:Button>&nbsp;
                                <asp:Button ID="btnExport" runat="server" CausesValidation="False" UseSubmitBehavior="False" Text="Export to Excel"
                                    CssClass="col-sm-2 btn btn-success" OnClick="ExportExcel"></asp:Button>
                        </div>
                    </div>


                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="existing_grid" />
                    <asp:PostBackTrigger ControlID="save_button" />
                    <asp:PostBackTrigger ControlID="cancel_button" />
                    <asp:PostBackTrigger ControlID="btnExport" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
    <!-- this page js -->
    <script src="assets/extra-libs/DataTables/datatables.min.js"></script>
    <script src="assets/extra-libs/multicheck/datatable-checkbox-init.js"></script>
    <script src="assets/extra-libs/multicheck/jquery.multicheck.js"></script>
    
    <script src="dist/js/google-jsapi.js"></script>
    <script src="dist/js/googletranslitration.js"></script>
    <script src="dist/js/transliteration.I.js"></script>

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
    
    <script>
        $(document).ready(function () {
            var ids = ['<%=txtHallNameL.ClientID%>', '<%=txtHallAddressL.ClientID %>'];
            googleTransliterate(ids);

            // 1 Capitalize string - convert textbox user entered text to uppercase
            jQuery('#<%= txtHallName.ClientID%>').keyup(function () {
                $(this).val($(this).val().toUpperCase());
            });
        });
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(function () {

            });
    </script>
    <script type="text/javascript">
        function confirmDelete() {
            var strDel = '<%=Resources.Resource.lblConfirmDelete %>';
            return confirm(strDel);
        }
    </script>
</asp:Content>

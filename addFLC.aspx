<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="addFLC.aspx.vb" Inherits="SEC_InventoryMgmt.addFLC" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- All Jquery -->
    <!-- ============================================================== -->
    <script src="assets/libs/jquery/dist/jquery.min.js"></script>
    <!-- Bootstrap tether Core JavaScript -->
    <script src="assets/libs/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <!-- slimscrollbar scrollbar JavaScript -->
    <script src="assets/libs/perfect-scrollbar/dist/perfect-scrollbar.jquery.min.js"></script>
    <script src="assets/extra-libs/sparkline/sparkline.js"></script>
    <!--Wave Effects -->
    <script src="dist/js/waves.js"></script>
    <!--Menu sidebar -->
    <script src="dist/js/sidebarmenu.js"></script>
    <!--Custom JavaScript -->
    <script src="dist/js/custom.min.js"></script>
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
                            <div class="table-responsive">
                                <table id="zero_config" class="table table-striped table-bordered">
                                    <thead>
                                        <tr>
                                            <th><%= GetLocalResourceObject("District_Name") %>  </th>
                                            <th><%= GetLocalResourceObject("FLC_Date") %>  </th>
                                            <th><%= GetLocalResourceObject("Hall_Name") %> </th>
                                            <th><%= GetLocalResourceObject("Hall_Name_Local") %> </th>
                                            <th><%= GetLocalResourceObject("Hall_Address") %> </th>
                                            <th><%= GetLocalResourceObject("Hall_Address_Local") %> </th>
                                            <th><%= GetLocalResourceObject("FLC_By") %></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td style="width: 10%;">
                                                <asp:DropDownList ID="ddldis" runat="server" CssClass="form-control" meta:resourcekey="ddlDisResource1"></asp:DropDownList></td>
                                            <td style="width: 10%;">
                                                <asp:TextBox ID="txtFLC_date" runat="server" TextMode="Date" required="required" CssClass="required form-control" meta:resourcekey="txtUnitSrNoResource1"></asp:TextBox>
                                                <asp:RangeValidator ID="RangeValidatorDt1" runat="server" CssClass="text-danger"
                                                    ControlToValidate="txtFLC_date" SetFocusOnError="true" ErrorMessage="Invalid Date"></asp:RangeValidator>
                                            </td>
                                            <td style="width: 10%;">
                                                <asp:TextBox ID="txtHallName" runat="server" MaxLength="50" required="required" placeHolder="In English" CssClass="required form-control" meta:resourcekey="txtUnitSrNoResource1"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" CssClass="text-danger"
                                                    ControlToValidate="txtHallName" SetFocusOnError="true" ValidationExpression="^[A-Z0-9]+$" ErrorMessage="Only Text, Numbers are allowed"></asp:RegularExpressionValidator>
                                            </td>
                                            <td style="width: 10%;">
                                                <asp:TextBox ID="txtHallNameLocal" runat="server" MaxLength="50" placeHolder="In Hindi" CssClass="required form-control" meta:resourcekey="txtUnitSrNoResource1"></asp:TextBox>
                                            </td>
                                            <td style="width: 10%;">
                                                <asp:TextBox ID="txtHallAddress" runat="server" MaxLength="250" TextMode="MultiLine" required="required" placeHolder="In English" CssClass="required form-control" meta:resourcekey="txtUnitSrNoResource1"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" CssClass="text-danger"
                                                    ControlToValidate="txtHallAddress" SetFocusOnError="true" ValidationExpression="^[A-Z0-9 -]+$" ErrorMessage="Only Text, Numbers are allowed"></asp:RegularExpressionValidator>
                                            </td>
                                            <td style="width: 10%;">
                                                <asp:TextBox ID="txtHallAddressLocal" runat="server" MaxLength="250" TextMode="MultiLine" placeHolder="In Hindi" CssClass="required form-control" meta:resourcekey="txtUnitSrNoResource1"></asp:TextBox>
                                            </td>
                                            <td style="width: 10%;">
                                                <asp:DropDownList ID="ddlFlcBy" runat="server" CssClass="form-control" meta:resourcekey="ddlFLCBy"></asp:DropDownList>
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
                        <div class="col-sm-12 row offset-3">
                            <asp:Button ID="save_button" runat="server" CssClass="col-sm-2 btn btn-info" meta:resourcekey="save_buttonResource1"></asp:Button>
                            <asp:Button ID="cancel_button" runat="server" CausesValidation="False" UseSubmitBehavior="False"
                                CssClass="col-sm-2 btn btn-secondary" meta:resourcekey="cancel_buttonResource1"></asp:Button>&nbsp;
                        </div>

                    </div>
                    <div id="dvList" runat="server" visible="false" class="card-body">
                        <h4 class="card-title"><%= Resources.Resource.lblListEVM %></h4>
                        <div class="table-responsive">
                            <asp:GridView ID="existing_grid" runat="server" AutoGenerateColumns="false" OnRowCommand="existing_grid_RowCommand"
                                CssClass="table table-striped table-bordered" OnRowDeleting="existing_grid_RowDeleting">
                                <Columns>
                                    <asp:TemplateField HeaderText="" meta:resourcekey="TemplateFieldResource3" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="FLC_ID" Text='<%# Bind("FLC_ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FLC Date" meta:resourcekey="TemplateFieldResource10">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="FLC_Date" Text='<%# Bind("FLC_Date") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="District" meta:resourcekey="TemplateFieldResource4">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="District_Name" Text='<%# Bind("District_Name") %>' />
                                            <asp:Label runat="server" ID="District_Code" Text='<%# Bind("District_Code") %>' Visible="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Hall_Name (English)" meta:resourcekey="TemplateFieldResource6">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="Hall_Name" Text='<%# Bind("Hall_Name") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Hall_Name (Hindi)" meta:resourcekey="TemplateFieldResource7">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="Hall_Name_Local" Text='<%# Bind("Hall_Name_Local") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Hall_Address (English)" meta:resourcekey="TemplateFieldResource8">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="Hall_Address" Text='<%# Bind("Hall_Address") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Hall_Address (Hindi)" meta:resourcekey="TemplateFieldResource3">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="Hall_Address_Local" Text='<%# Bind("Hall_Address_Local") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Working Status" meta:resourcekey="TemplateFieldResource9">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="FLC_By_Name" Text='<%# Bind("FLC_By_Name") %>' />
                                            <asp:Label runat="server" ID="FLC_By" Text='<%# Bind("FLC_By") %>' Visible="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" meta:resourcekey="TemplateFieldResource1">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkEdit" ForeColor="#FDBE02" CommandName="editWard" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CausesValidation="False" ToolTip="Edit" meta:resourcekey="lnkEditResource1">
                                            <i class="mdi mdi-lg-4 mdi-table-edit"></i><%=Resources.Resource.lnkEdit %></asp:LinkButton>
                                            &nbsp;
                                            <asp:LinkButton runat="server" ID="lnkDelete" ForeColor="#E34724"
                                                CommandName="delete" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CausesValidation="False"
                                                OnClientClick="return confirmDelete();" ToolTip="Delete" meta:resourcekey="lnkDeleteResource1">
                                                <i class="mdi mdi-lg-4 mdi-delete"></i><%=Resources.Resource.lnkDelete %></asp:LinkButton>
                                            &nbsp;
                                             <asp:LinkButton runat="server" ID="lnkQR" ForeColor="#000F60" CommandName="viewQR" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CausesValidation="False" ToolTip="Download QR Code">
                                            <i class="mdi mdi-lg-4 mdi-download"></i><%=Resources.Resource.lnkQR %></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <HeaderStyle BackColor="#000F60" ForeColor="White" BorderColor="#000F60" />
                            </asp:GridView>
                        </div>
                    </div>

                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="existing_grid" />
                    <asp:PostBackTrigger ControlID="save_button" />
                    <asp:PostBackTrigger ControlID="cancel_button" />

                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">

    <script>
        $(document).ready(function () {

            // 1 Capitalize string - convert textbox user entered text to uppercase
            jQuery('#<%= txtHallName.ClientID%>').keyup(function () {
                 $(this).val($(this).val().toUpperCase());
             });

             jQuery('#<%= txtHalladdress.ClientID%>').keyup(function () {
                 $(this).val($(this).val().toUpperCase());
             });
         });
    </script>
    <script type="text/javascript">
        function confirmDelete() {
            var strDel = '<%=Resources.Resource.lblConfirmDelete %>';
            return confirm(strDel);
        }
    </script>
</asp:Content>

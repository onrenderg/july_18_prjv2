<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="false" MasterPageFile="~/Site.Master" CodeBehind="addEVM.aspx.vb" Inherits="SEC_InventoryMgmt.addEVM" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <!-- All Jquery -->
    
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
                    <div class="card-body">
                        <div id="formNom" runat="server">
                            <div class="form-group row">
                                <label for="rbUnitType" class="col-sm-2 text-md-right"><%= GetLocalResourceObject("rbUnitType") %></label>
                                <div class="col-lg-2 col-md-4 col-sm-6">
                                    <asp:RadioButtonList ID="rbUnitType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbUnitType_SelectedIndexChanged" CssClass="col-12 select2-container--default" meta:resourcekey="rbUnitTypeResource1">
                                    </asp:RadioButtonList>
                                    <asp:Label ID="txtUnitID" runat="server" Visible="false"></asp:Label>
                                </div>
                                <div class="table-responsive">
                                    <table id="zero_config" class="table table-striped table-bordered">
                                        <thead>
                                            <tr>
                                                <th><%= GetLocalResourceObject("lblUnitSeries") %>  </th>
                                                <th><%= GetLocalResourceObject("lblUnitSrNo") %>  </th>
                                                <th><%= GetLocalResourceObject("lblMake") %> </th>
                                                <th><%= GetLocalResourceObject("lblModel") %> </th>
                                                <th colspan="2"><%= GetLocalResourceObject("lblManfacturedOn") %> </th>
                                                <th colspan="3"><%= GetLocalResourceObject("lblAvailableAt") %>:&nbsp;<%= GetLocalResourceObject("lblLat") %>&nbsp;
                                                <%= GetLocalResourceObject("lblLong") %> </th>
                                                <th><%= GetLocalResourceObject("lblWorkingStatus") %> </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td style="width: 10%;">
                                                    <asp:TextBox ID="txtUnitSeries" runat="server" MaxLength="5" required="required" CssClass="required form-control" meta:resourcekey="txtUnitSrNoResource1"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" CssClass="text-danger"
                                                        ControlToValidate="txtUnitSeries" SetFocusOnError="true" ValidationExpression="^[A-Z]+$" ErrorMessage="Only Text is allowed"></asp:RegularExpressionValidator>
                                                </td>
                                                <td style="width: 10%;">
                                                    <asp:TextBox ID="txtUnitSrNo" runat="server" MaxLength="12" required="required" CssClass="required form-control" meta:resourcekey="txtUnitSrNoResource1"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" CssClass="text-danger"
                                                        ControlToValidate="txtUnitSrNo" SetFocusOnError="true" ValidationExpression="^[A-Z0-9]+$" ErrorMessage="Only Text, Numbers are allowed"></asp:RegularExpressionValidator>
                                                </td>
                                                <td style="width: 10%;">
                                                    <asp:DropDownList ID="ddlMake" runat="server" CssClass="form-control" meta:resourcekey="ddlMakeResource1"></asp:DropDownList></td>
                                                <td style="width: 10%;">
                                                    <asp:DropDownList ID="ddlModel" runat="server" CssClass="form-control" meta:resourcekey="ddlModelResource1"></asp:DropDownList></td>
                                                <td style="width: 10%;">
                                                    <asp:DropDownList ID="ddlMon" runat="server" CssClass="col-6 form-control" meta:resourcekey="ddlMonResource1"></asp:DropDownList>
                                                </td>
                                                <td style="width: 10%;">
                                                    <asp:DropDownList ID="ddlYr" runat="server" CssClass="col-6 form-control" meta:resourcekey="ddlYrResource1"></asp:DropDownList>
                                                </td>
                                                <td style="width: 10%;">
                                                    <asp:DropDownList ID="ddlAvailableAt" runat="server" CssClass="form-control" meta:resourcekey="ddlAvailableAtResource1"></asp:DropDownList></td>
                                                <td style="width: 10%;">
                                                    <asp:TextBox ID="txtLat" runat="server" CssClass="form-control" MaxLength="12" placeHolder="Latitude" ToolTip="Latitude of EVM's Current Location" meta:resourcekey="txtLatResource1"></asp:TextBox>
                                                    <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtLat" MinimumValue="30.36" MaximumValue="33.22" CssClass="text-danger" ErrorMessage="Out of Himachal Pradesh"></asp:RangeValidator>
                                                </td>
                                                <td style="width: 10%;">
                                                    <asp:TextBox ID="txtLong" runat="server" CssClass="form-control" MaxLength="12" placeHolder="Longitude" ToolTip="Longitude of EVM's Current Location" meta:resourcekey="txtLongResource1"></asp:TextBox>
                                                    <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtLong" MinimumValue="75.74" MaximumValue="79.07" CssClass="text-danger" ErrorMessage="Out of Himachal Pradesh"></asp:RangeValidator>
                                                </td>
                                                <td style="width: 10%;">
                                                    <asp:DropDownList ID="ddlOperationStatus" runat="server" CssClass="form-control" meta:resourcekey="ddlOperationStatusResource1"></asp:DropDownList>
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
                                <asp:Button ID="view_button" runat="server" CausesValidation="False" UseSubmitBehavior="False" Text="Print All QR Code"
                                    CssClass="col-sm-2 btn btn-info" meta:resourcekey="view_buttonResource1"></asp:Button>
                            </div>
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
                                            <asp:Label runat="server" ID="lblID" Text='<%# Bind("ID") %>' />
                                            <asp:Label runat="server" ID="lblut" Text='<%# Bind("UnitType") %>' />
                                            <asp:Label runat="server" ID="make" Text='<%# Bind("Make") %>' />
                                            <asp:Label runat="server" ID="model" Text='<%# Bind("Model") %>' />
                                            <asp:Label runat="server" ID="mon" Text='<%# Bind("MonManufacturing") %>' />
                                            <asp:Label runat="server" ID="yr" Text='<%# Bind("YrManufacturing") %>' />
                                            <asp:Label runat="server" ID="dis" Text='<%# Bind("LocatedAt") %>' />
                                            <asp:Label runat="server" ID="wstatus" Text='<%# Bind("workingStatus") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Series" meta:resourcekey="TemplateFieldResource10">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblSrs" Text='<%# Bind("Series") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SrNo" meta:resourcekey="TemplateFieldResource4">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblsn" Text='<%# Bind("SrNo") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Make" meta:resourcekey="TemplateFieldResource6">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblmk" Text='<%# Bind("Make") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Model" meta:resourcekey="TemplateFieldResource7">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblmd" Text='<%# Bind("Model") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mon-Year of Manufacturing" meta:resourcekey="TemplateFieldResource8">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblmy" Text='<%# Bind("MonYrManufacturing") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Current Location: Latitude Longitude" meta:resourcekey="TemplateFieldResource3">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lbllc" Text='<%# Bind("LocatedAt") %>' />
                                            &nbsp;
                                            <asp:Label runat="server" ID="lbllt" Text='<%# Bind("Latitude") %>' />
                                            &nbsp;
                                            <asp:Label runat="server" ID="lblln" Text='<%# Bind("Longitude") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Working Status" meta:resourcekey="TemplateFieldResource9">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblws" Text='<%# Bind("workingStatus") %>' />
                                             <asp:Label runat="server" ID="QRText" Text='<%# Bind("QRText") %>' Visible="false" />
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
                    <asp:AsyncPostBackTrigger ControlID="rbUnitType" EventName="SelectedIndexChanged" />
                    <asp:PostBackTrigger ControlID="existing_grid" />
                    <asp:PostBackTrigger ControlID="save_button" />
                    <asp:PostBackTrigger ControlID="cancel_button" />
                    <asp:PostBackTrigger ControlID="view_button" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
 
    
    <script>
        $(document).ready(function () {

            // 1 Capitalize string - convert textbox user entered text to uppercase
            jQuery('#<%= txtUnitSrNo.ClientID%>').keyup(function () {
                $(this).val($(this).val().toUpperCase());
            });

            jQuery('#<%= txtUnitSeries.ClientID%>').keyup(function () {
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

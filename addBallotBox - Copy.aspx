<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="addBallotBox.aspx.vb" Inherits="SEC_InventoryMgmt.addBallotBox" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <!-- All Jquery -->
        <!-- ============================================================== -->
         <script src="assets/libs/jquery/dist/jquery.min.js"></script>
        <!-- slimscrollbar scrollbar JavaScript -->
        <script src="assets/libs/perfect-scrollbar/dist/perfect-scrollbar.jquery.min.js"></script>
        <script src="assets/extra-libs/sparkline/sparkline.js"></script>
        <!--Wave Effects -->
        <script src="dist/js/waves.js"></script>
        <!--Menu sidebar -->
        <script src="dist/js/sidebarmenu.js"></script>
        <!--Custom JavaScript -->
        <script src="dist/js/custom.min.js"></script>

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
                                <asp:Label ID="txtUnitID" runat="server" Visible="false"></asp:Label>
                                <div class="table-responsive">
                                    <table id="zero_config" class="table table-striped table-bordered">
                                        <thead style="background-color:#000F60; color: whitesmoke;">
                                            <tr>
                                                <th><%= GetLocalResourceObject("lblAvailableAt") %>  </th>
                                                <th><%= GetLocalResourceObject("lblWarehouseAt") %>  </th>
                                                <th><%= GetLocalResourceObject("lblUnitCount") %>  </th>
                                                <th><%= GetLocalResourceObject("lblModel") %> </th>
                                                <th colspan="2"><%= GetLocalResourceObject("lblPurchasedOn") %> </th>
                                              <%--  <th colspan="2"><%= GetLocalResourceObject("lblLat") %>&nbsp;
                                                <%= GetLocalResourceObject("lblLong") %> </th>
                                                <th><%= GetLocalResourceObject("lblWorkingStatus") %> </th> --%>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td style="width: 10%;">
                                                    <asp:DropDownList ID="ddlAvailableAt" runat="server" CssClass="form-control" meta:resourcekey="ddlAvailableAtResource1"></asp:DropDownList></td>
                                                <td style="width: 10%;">
                                                    <asp:DropDownList ID="ddlWarehouseAt" runat="server" CssClass="form-control" meta:resourcekey="ddlWarehouseAtResource1"></asp:DropDownList></td>
                                                <td style="width: 10%;">
                                                    <asp:TextBox ID="txtUnitCounts" runat="server" TextMode="Number" MaxLength="5" required="required" CssClass="required form-control" meta:resourcekey="txtUnitSrNoResource1"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" CssClass="text-danger"
                                                        ControlToValidate="txtUnitCounts" SetFocusOnError="true" ValidationExpression="^[0-9]+$" ErrorMessage="Only Numbers are allowed"></asp:RegularExpressionValidator>
                                                </td>
                                                <td style="width: 10%;">
                                                    <asp:DropDownList ID="ddlModel" runat="server" CssClass="form-control" meta:resourcekey="ddlModelResource1"></asp:DropDownList></td>
                                                <td style="width: 10%;">
                                                    <asp:DropDownList ID="ddlMon" runat="server" CssClass="col-6 form-control" meta:resourcekey="ddlMonResource1"></asp:DropDownList>
                                                </td>
                                                <td style="width: 10%;">
                                                    <asp:DropDownList ID="ddlYr" runat="server" CssClass="col-6 form-control" meta:resourcekey="ddlYrResource1"></asp:DropDownList>
                                                </td>
                                               <%-- <td style="width: 10%;">
                                                    <asp:TextBox ID="txtLat" runat="server" CssClass="form-control" MaxLength="12" placeHolder="Latitude" ToolTip="Latitude of EVM's Current Location" meta:resourcekey="txtLatResource1"></asp:TextBox>
                                                    <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtLat" MinimumValue="30.36" MaximumValue="33.22" CssClass="text-danger" ErrorMessage="Out of Himachal Pradesh"></asp:RangeValidator>
                                                </td>
                                                <td style="width: 10%;">
                                                    <asp:TextBox ID="txtLong" runat="server" CssClass="form-control" MaxLength="12" placeHolder="Longitude" ToolTip="Longitude of EVM's Current Location" meta:resourcekey="txtLongResource1"></asp:TextBox>
                                                    <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtLong" MinimumValue="75.74" MaximumValue="79.07" CssClass="text-danger" ErrorMessage="Out of Himachal Pradesh"></asp:RangeValidator>
                                                </td>
                                                <td style="width: 10%;">
                                                    <asp:DropDownList ID="ddlOperationStatus" runat="server" CssClass="form-control" meta:resourcekey="ddlOperationStatusResource1"></asp:DropDownList>
                                                </td>--%>
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
                                <%--  <%Dim str As String%><%Dim str1 As String%><%str1 = txtUnitID.ClientID%><%str = "javascript:return checkOldSrNo('" + str1 + "','" + txtUnitSrNo.Text + "');"%><%save_button.OnClientClick = str%>--%>
                                <asp:Button ID="save_button" runat="server" CssClass="col-sm-2 btn btn-md btn-info" meta:resourcekey="save_buttonResource1"></asp:Button>
                                <asp:Button ID="cancel_button" runat="server" CausesValidation="False" UseSubmitBehavior="False"
                                    CssClass="col-sm-2 btn btn-md btn-secondary" meta:resourcekey="cancel_buttonResource1"></asp:Button>&nbsp;
                            </div>
                        </div>
                    </div>
                    <div id="dvList" runat="server" visible="false" class="card-body">
                        <h4 class="card-title"><%= Resources.Resource.lblListBallotBox %></h4>
                        <div class="table-responsive">
                            <asp:GridView ID="existing_grid" runat="server" AutoGenerateColumns="false" OnRowCommand="existing_grid_RowCommand"
                                CssClass="table table-striped table-bordered">
                                <Columns>
                                   <%-- <asp:TemplateField HeaderText="" meta:resourcekey="TemplateFieldResource1">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkEdit" ForeColor="#FFC107" CommandName="editWard" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CausesValidation="False" ToolTip="Edit" meta:resourcekey="lnkEditResource1">
                                            <i class="mdi mdi-lg-4 mdi-table-edit"></i><%=Resources.Resource.lnkEdit %></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" meta:resourcekey="TemplateFieldResource2">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkDelete" ForeColor="#E34724"
                                                CommandName="delete" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CausesValidation="False"
                                                OnClientClick="return confirmDelete();" ToolTip="Delete" meta:resourcekey="lnkDeleteResource1">
                                                <i class="mdi mdi-lg-4 mdi-delete"></i><%=Resources.Resource.lnkDelete %></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    
                                      <asp:TemplateField HeaderText="Current Location[District Warehouse]" meta:resourcekey="TemplateFieldResource3">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lbllc" Text='<%# Bind("DistrictCode") %>' />&nbsp;
                                            <asp:Label runat="server" ID="lblwh" Text='<%# Bind("WarehouseCode") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Size" meta:resourcekey="TemplateFieldResource4">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="BBSize" Text='<%# Bind("BBSize") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="No. of Ballot Boxes" meta:resourcekey="TemplateFieldResource4">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="BBCount" Text='<%# Bind("BBCount") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   
                                    <asp:TemplateField HeaderText="Mon-Year of Purchase" meta:resourcekey="TemplateFieldResource8">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="MonYrPurchase" Text='<%# Bind("MonYrPurchase") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   
                                   <%-- <asp:TemplateField HeaderText="Working Status" meta:resourcekey="TemplateFieldResource9">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblws" Text='<%# Bind("workingStatus") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                </Columns>
                                <HeaderStyle BackColor="#000F60" ForeColor="White" BorderColor="#000F60" />
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="ddlAvailableAt" />
                    <asp:PostBackTrigger ControlID="existing_grid" />
                    <asp:PostBackTrigger ControlID="save_button" />
                    <asp:PostBackTrigger ControlID="cancel_button" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
    <%-- <script>
        $(document).ready(function () {

          // 1 Capitalize string - convert textbox user entered text to uppercase
            jQuery('#<%= txtUnitSrNo.ClientID%>').keyup(function () {
                $(this).val($(this).val().toUpperCase());
            });
        });
    </script>--%>
    <script type="text/javascript">
        function confirmDelete() {
            var strDel = '<%=Resources.Resource.lblConfirmDelete %>';
            return confirm(strDel);
        }
    </script>
</asp:Content>

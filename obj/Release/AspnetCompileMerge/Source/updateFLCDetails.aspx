<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="updateFLCDetails.aspx.vb" Inherits="SEC_InventoryMgmt.updateFLCDetails" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link
        href="assets/libs/datatables.net-bs4/css/dataTables.bootstrap4.css"
        rel="stylesheet" />
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
                        <div class="form-group row">
                            <label for="rbUnitType" class="col-sm-3 text-md-right"><%= GetLocalResourceObject("rbUnitType") %></label>
                            <div class="col-lg-3 col-md-5 col-sm-6">
                                <asp:RadioButtonList ID="rbUnitType" runat="server" OnSelectedIndexChanged="rbUnitType_SelectedIndexChanged"
                                    RepeatDirection="Horizontal" AutoPostBack="true" CssClass="col-12 list-inline-item" meta:resourcekey="rbUnitTypeResource1">
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="ddlFLC" class="col-sm-3 text-md-right"><%= GetLocalResourceObject("flcType") %></label>
                            <div class="col-lg-4 col-md-8 col-sm-6">
                                <asp:DropDownList ID="ddlFLC" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="ddlFLC_SelectedIndexChanged" CssClass="dropdown-header">
                                </asp:DropDownList>
                            </div>
                        </div>

                    </div>
                    <div id="dvEVM" runat="server" visible="false" class="card-body">
                        <div class="table-responsive">
                            <table id="zero_config" class="table table-striped table-bordered">
                                <thead style="background-color: #000F60; color: whitesmoke;">
                                    <tr>
                                        <th><%= GetLocalResourceObject("lblUnitId") %>  </th>
                                        <th><%= GetLocalResourceObject("lblUnitSeries") %>  </th>
                                        <th><%= GetLocalResourceObject("lblUnitSrNo") %>  </th>
                                        <th><%= GetLocalResourceObject("lblWorkingStatus") %> </th>
                                        <th><%= GetLocalResourceObject("lblRemark") %> </th>
                                        <th><%= GetLocalResourceObject("lblTraining") %> </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td style="width: 10%;">
                                            <asp:TextBox ID="txtUnitID" runat="server" ReadOnly="true" CssClass="required form-control" meta:resourcekey="txtUnitSrNoResource1"></asp:TextBox>
                                        </td>
                                        <td style="width: 10%;">
                                            <asp:TextBox ID="txtUnitSeries" runat="server" ReadOnly="true" CssClass="required form-control" meta:resourcekey="txtUnitSrNoResource1"></asp:TextBox>
                                        </td>
                                        <td style="width: 10%;">
                                            <asp:TextBox ID="txtUnitSrNo" runat="server" ReadOnly="true" CssClass="required form-control" meta:resourcekey="txtUnitSrNoResource1"></asp:TextBox>

                                        </td>
                                        <td style="width: 10%;">
                                            <asp:DropDownList ID="ddlOperationStatus" runat="server" CssClass="form-control" meta:resourcekey="ddlOperationStatusResource1"></asp:DropDownList>

                                        </td>
                                        <td style="width: 10%;">
                                            <asp:TextBox ID="txtRemark" runat="server" CssClass="form-control" meta:resourcekey="txtRemark"></asp:TextBox>
                                        </td>
                                        <td style="width: 10%;">
                                            <asp:RadioButtonList ID="ddlForTraining" runat="server" RepeatDirection="Horizontal" CssClass="list-inline-item" meta:resourcekey="ddlForTraining"></asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div class="col-sm-12 row offset-3">
                            <asp:Button ID="save_button" runat="server" CssClass="col-sm-2 btn btn-info" meta:resourcekey="save_buttonResource1"></asp:Button>
                            <asp:Button ID="cancel_button" runat="server" CausesValidation="False" UseSubmitBehavior="False"
                                CssClass="col-sm-2 btn btn-secondary" meta:resourcekey="cancel_buttonResource1"></asp:Button>

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
                    <div class="row">
                        <%--   <h4 class="card-title"><%= Resources.Resource.lblListEVM %></h4>--%>
                        <div class="table-responsive">
                            <asp:GridView ID="existing_grid" runat="server" AutoGenerateColumns="false"
                                OnRowCommand="existing_grid_RowCommand" OnRowDeleting="existing_grid_RowDeleting"
                                CssClass="table table-striped table-bordered" HeaderStyle-ForeColor="WhiteSmoke">
                                <Columns>
                                    <asp:TemplateField HeaderText="QR Id" meta:resourcekey="TemplateFieldResource1">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblID" Text='<%# Bind("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Series" meta:resourcekey="TemplateFieldResource10">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblSrs" Text='<%# Bind("Series") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SrNo" meta:resourcekey="TemplateFieldResource2">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblsn" Text='<%# Bind("SrNo") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Make" meta:resourcekey="TemplateFieldResource3">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblmk" Text='<%# Bind("MakeName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Model" meta:resourcekey="TemplateFieldResource4">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblmd" Text='<%# Bind("ModelName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Working Status" meta:resourcekey="TemplateFieldResource5">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblwsL" Text='<%# Bind("wsNameLocal") %>' />
                                            <asp:Label runat="server" ID="lblws" Text='<%# Bind("wsName") %>' Visible="false" />
                                            <asp:Label runat="server" ID="lblWsCode" Text='<%# Bind("workingstatus") %>' Visible="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remarks/Pink Seal No. [Used For Training]" meta:resourcekey="TemplateFieldResource6">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPinkSeal" Text='<%# Bind("PinkSealNumber") %>' />
                                            <asp:Label runat="server" ID="lblRemark" Visible="false" Text='<%# Bind("Remarks") %>' />
                                            &nbsp;[<asp:Label runat="server" ID="UsedForTraining" Text='<%# Bind("UsedForTraining") %>' />]
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkEdit" ForeColor="#FDBE02" CommandName="edit" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CausesValidation="False" ToolTip="Edit" meta:resourcekey="lnkEditResource1">
                                            <i class="mdi mdi-lg-4 mdi-table-edit"></i><%=Resources.Resource.lnkEdit %></asp:LinkButton>
                                            &nbsp;
                                            <asp:LinkButton runat="server" ID="lnkDelete" ForeColor="#E34724"
                                                CommandName="delete" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CausesValidation="False"
                                                OnClientClick="return confirmDelete();" ToolTip="Delete" meta:resourcekey="lnkDeleteResource1">
                                                <i class="mdi mdi-lg-4 mdi-delete"></i><%=Resources.Resource.lnkDelete %></asp:LinkButton>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <asp:Label ID="lbl_blank_grid" CssClass="col-9 text-danger" runat="server" meta:resourcekey="lbl_blank_grid"></asp:Label>
                                </EmptyDataTemplate>
                                <HeaderStyle BackColor="#000F60" ForeColor="White" BorderColor="#000F60" />
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="col-sm-12 row offset-3">
                        <asp:Button ID="cancel_button1" runat="server" CausesValidation="False" UseSubmitBehavior="False"
                            CssClass="col-sm-2 btn btn-secondary" meta:resourcekey="cancel_buttonResource1"></asp:Button>
                        &nbsp;<asp:Button ID="btnExport" runat="server" CausesValidation="False" UseSubmitBehavior="False" Text="Export to Excel"
                            CssClass="col-sm-2 btn btn-success" OnClick="ExportExcel" Visible="false"></asp:Button>
                    </div>

                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="rbUnitType" />
                    <asp:PostBackTrigger ControlID="ddlFLC" />
                    <asp:PostBackTrigger ControlID="existing_grid" />
                    <asp:PostBackTrigger ControlID="save_button" />
                    <asp:PostBackTrigger ControlID="cancel_button" />
                    <asp:PostBackTrigger ControlID="cancel_button1" />
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

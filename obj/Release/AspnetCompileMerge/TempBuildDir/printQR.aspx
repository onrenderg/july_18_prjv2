<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="printQR.aspx.vb" Inherits="SEC_InventoryMgmt.printQR" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

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

        .selected {
            background-color: #A1DCF2;
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
    <h4 class="page-title">
        <asp:Label ID="lblTitle" runat="server" meta:resourcekey="lblTitleResource1"></asp:Label></h4>
    <div class="row">
        <div class="card">
            <asp:UpdatePanel runat="server" ID="panel1" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="card-body">
                        <div class="form-group row">
                            <asp:Label ID="lblDistName" runat="server" class="col-lg-1 col-md-2 col-sm-2 text-md-right"><%= Resources.Resource.Dis %></asp:Label>
                            <div class="col-lg-2 col-md-3 col-sm-6">
                                <asp:DropDownList ID="ddlDistName" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="ddlDistName_SelectedIndexChanged" CssClass="col-12 dropdown-header">
                                </asp:DropDownList>
                            </div>
                            <asp:Label ID="lblWareHouse" runat="server" class="col-lg-1 col-md-2 col-sm-2 text-md-end text-lg-end"><%= Resources.Resource.Warehouse %></asp:Label>
                            <div class="col-lg-4 col-md-4 col-sm-6">
                                <asp:DropDownList ID="ddlWareHouse" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbUnitType_SelectedIndexChanged" CssClass="col-12 dropdown-header">
                                </asp:DropDownList>
                            <asp:Button ID="btnMap" runat="server" Text="View On Map"
                                OnClick="btnMap_Click" CssClass="btn btn-dropbox text-md-end" meta:resourcekey="btnMap" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="rbUnitType" class="col-lg-3 col-md-3 col-sm-12 text-md-right"><%= GetLocalResourceObject("rbUnitType") %></label>
                            <div class="col-lg-5 col-md-6 col-sm-12">
                                <asp:RadioButtonList ID="rbUnitType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbUnitType_SelectedIndexChanged" CssClass="col-12 select2-container--default" meta:resourcekey="rbUnitTypeResource1">
                                </asp:RadioButtonList>
                            </div>
                        </div>


                        <div class="row">
                            <%--   <h4 class="card-title"><%= Resources.Resource.lblListEVM %></h4>--%>
                            <div class="table-responsive">
                                <asp:GridView ID="existing_grid" runat="server" AutoGenerateColumns="false"
                                    CssClass="table table-striped table-bordered" HeaderStyle-ForeColor="WhiteSmoke">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkHeader" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <label class="customcheckbox">
                                                    <asp:CheckBox ID="grdCheckBox" runat="server" class="listCheckbox" />
                                                    <span class="checkmark"></span>
                                                </label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Id" meta:resourcekey="TemplateFieldResource1">
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
                                        <asp:TemplateField HeaderText="Make" meta:resourcekey="TemplateFieldResource3">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblmk" Text='<%# Bind("MakeName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Model" meta:resourcekey="TemplateFieldResource4">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblmd" Text='<%# Bind("ModelName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mon-Year of Manufacturing" meta:resourcekey="TemplateFieldResource5">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblmy" Text='<%# Bind("MonYrManufacturing") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mon-Year of Purchase" meta:resourcekey="TemplateFieldResource6">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblmyP" Text='<%# Bind("MonYrPurchase") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Working Status" meta:resourcekey="TemplateFieldResource7">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblwsL" Text='<%# Bind("wsNameLocal") %>' />
                                                <asp:Label runat="server" ID="lblws" Text='<%# Bind("wsName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CodeVals" Visible="false">
                                            <ItemTemplate>
                                                <%--                   <asp:Label runat="server" ID="lblID" Text='<%# Bind("ID") %>' />--%>
                                                <asp:Label runat="server" ID="lblut" Text='<%# Bind("UnitType") %>' />
                                                <asp:Label runat="server" ID="make" Text='<%# Bind("Make") %>' />
                                                <asp:Label runat="server" ID="model" Text='<%# Bind("Model") %>' />
                                                <asp:Label runat="server" ID="mon" Text='<%# Bind("MonManufacturing") %>' />
                                                <asp:Label runat="server" ID="yr" Text='<%# Bind("YrManufacturing") %>' />
                                                <asp:Label runat="server" ID="Label1" Text='<%# Bind("MonPurchase") %>' />
                                                <asp:Label runat="server" ID="Label2" Text='<%# Bind("YrPurchase") %>' />
                                                <asp:Label runat="server" ID="QRText" Text='<%# Bind("QRText") %>' />
                                                <asp:Label runat="server" ID="dis" Text='<%# Bind("LocatedAt") %>' />
                                                <asp:Label runat="server" ID="wstatus" Text='<%# Bind("workingStatus") %>' />
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
                        <div class="col-12 row offset-1">
                            <asp:Button ID="view_on_map" runat="server" CausesValidation="False" UseSubmitBehavior="False" Text="View On Map"
                                CssClass="col-sm-2 btn btn-dropbox" OnClick="view_on_map_Click" meta:resourcekey="view_on_map_buttonResource1"></asp:Button>

                            &nbsp;<asp:Button ID="view_button" runat="server" CausesValidation="False" UseSubmitBehavior="False" Text="Print All QR Code"
                                CssClass="col-sm-2 btn btn-info" meta:resourcekey="view_buttonResource1"></asp:Button>
                            &nbsp;<asp:Button ID="btnSrId" runat="server" CausesValidation="False" UseSubmitBehavior="False" Text="QR Code List"
                                CssClass="col-sm-2 btn btn-primary" meta:resourcekey="btnSrIdResource1" OnClick="btnSrId_Click"></asp:Button>
                            &nbsp;<asp:Button ID="btnExport" runat="server" CausesValidation="False" UseSubmitBehavior="False" Text="Export to Excel"
                                CssClass="col-sm-2 btn btn-success" OnClick="ExportExcel"></asp:Button>&nbsp;<asp:Button ID="cancel_button" runat="server" CausesValidation="False" UseSubmitBehavior="False"
                                    CssClass="col-sm-2 btn btn-secondary" meta:resourcekey="cancel_buttonResource1"></asp:Button>&nbsp;
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <%--<asp:AsyncPostBackTrigger ControlID="ddlWareHouse" EventName="SelectedIndexChanged" />--%>
                    <asp:PostBackTrigger ControlID="ddlDistName" />
                    <asp:PostBackTrigger ControlID="ddlWareHouse" />
                    <%--<asp:AsyncPostBackTrigger ControlID="existing_grid" EventName="RowCommand" />--%>
                    <asp:PostBackTrigger ControlID="rbUnitType" />
                    <asp:PostBackTrigger ControlID="existing_grid" />
                    <asp:PostBackTrigger ControlID="cancel_button" />
                    <asp:PostBackTrigger ControlID="view_button" />
                    <asp:PostBackTrigger ControlID="btnSrId" />
                    <asp:PostBackTrigger ControlID="btnExport" />
                    <asp:PostBackTrigger ControlID="btnMap" />
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
    </script>
    <script type="text/javascript">
        $("[id*=ContentPlaceHolder1_existing_grid_chkHeader]").on("click", function () {

            var chkHeader = $(this);
            var grid = $(this).closest("table");
            $("input[type=checkbox]", grid).each(function () {
                if (chkHeader.is(":checked")) {
                    $(this).attr("checked", "checked");
                    $("td", $(this).closest("tr")).addClass("selected");
                } else {
                    $(this).removeAttr("checked");
                    $("td", $(this).closest("tr")).removeClass("selected");
                }
            });
        });
        $("[id*=ContentPlaceHolder1_existing_grid_grdCheckBox]").on("click", function () {

            var grid = $(this).closest("table");
            var chkHeader = $("[id*=ContentPlaceHolder1_existing_grid_chkHeader]", grid);
            if (!$(this).is(":checked")) {
                $("td", $(this).closest("tr")).removeClass("selected");
                chkHeader.removeAttr("checked");
            } else {
                $("td", $(this).closest("tr")).addClass("selected");
                if ($("[id*=ContentPlaceHolder1_existing_grid_grdCheckBox]", grid).length == $("[id*=ContentPlaceHolder1_existing_grid_grdCheckBox]:checked", grid).length) {
                    chkHeader.attr("checked", "checked");
                }
            }
        });
    </script>
    <script>
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {

        });
    </script>
</asp:Content>

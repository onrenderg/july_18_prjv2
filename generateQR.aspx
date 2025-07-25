<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="generateQR.aspx.vb" Inherits="SEC_InventoryMgmt.generateQR" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

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
                        <div class="row alert alert-info">
                            <asp:Label ID="lblWhName" runat="server" CssClass="col-5" />
                            <asp:Label ID="lblWhAddress" runat="server" CssClass="col-5" />
                            <%--<asp:ImageMap ID="imgLoc" runat="server" AlternateText="View On Map" ImageUrl="~/assets/images/geolocation.jpg" Height="50" Width="50" />--%>
                            <asp:Button ID="btnMap" runat="server" Text="View On Map"
                                OnClick="btnMap_Click" CssClass="btn btn-dropbox col-2" />
                        </div>
                        <div class="form-group row">
                            <label for="rbUnitType" class="col-lg-3 col-md-4 col-sm-12 text-md-right"><%= GetLocalResourceObject("rbUnitType") %></label>
                            <div class="col-lg-5 col-md-6 col-sm-12">
                                <asp:RadioButtonList ID="rbUnitType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbUnitType_SelectedIndexChanged" CssClass="col-12 select2-container--default" meta:resourcekey="rbUnitTypeResource1">
                                </asp:RadioButtonList>
                            </div>
                        </div>
                         <div id="dvBBType" runat="server" class="form-group row" visible="false">
                            <label for="rbUnitType" class="col-lg-3 col-md-4 col-sm-12 text-md-right"><%= GetLocalResourceObject("rbUnitType") %></label>
                            <div class="col-lg-5 col-md-6 col-sm-12">
                                <asp:RadioButtonList ID="rbMakeType" runat="server" RepeatDirection="Horizontal" AutoPostBack="false" CssClass="col-12 select2-container--default" meta:resourcekey="rbUnitTypeResource1">
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="row">
                            <h4 class="card-title">
                                <asp:Label ID="lblList" runat="server" />
                                &nbsp;
                            <asp:Label ID="lblCounts" runat="server" Text=""></asp:Label></h4>
                            <div class="table-responsive">

                                <asp:GridView ID="existing_grid" runat="server" AutoGenerateColumns="false"
                                    CssClass="table table-striped table-bordered" HeaderStyle-ForeColor="WhiteSmoke">
                                    <Columns>
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
                                                <asp:Label runat="server" ID="lblmyPP" Text='<%# Bind("MonYrPurchase") %>' Visible="false"  />
                                                <asp:Label runat="server" ID="lblmyP" Text='<%# Bind("MonYrPurchase") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" Visible="false">
                                            <ItemTemplate>
                                               
                                                <asp:Label runat="server" ID="lblut" Text='<%# Bind("UnitType") %>' />
                                                <asp:Label runat="server" ID="make" Text='<%# Bind("Make") %>' />
                                                <asp:Label runat="server" ID="model" Text='<%# Bind("Model") %>' />
                                                <asp:Label runat="server" ID="mon" Text='<%# Bind("MonManufacturing") %>' />
                                                <asp:Label runat="server" ID="yr" Text='<%# Bind("YrManufacturing") %>' />
                                                <asp:Label runat="server" ID="monP" Text='<%# Bind("MonPurchase") %>' />
                                                <asp:Label runat="server" ID="yrP" Text='<%# Bind("YrPurchase") %>' />
                                                <asp:Label runat="server" ID="QRText" Text='<%# Bind("QRText") %>' />
                                                <asp:Label runat="server" ID="dis" Text='<%# Bind("LocatedAt") %>' />
                                                <asp:Label runat="server" ID="wstatus" Text='<%# Bind("workingStatus") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#000F60" ForeColor="White" BorderColor="#000F60" />
                                </asp:GridView>
                                <div class="help-block">
                                    <asp:Label runat="server" ID="lblHelptext" Text="QR Code will have all the field values listed above. Please note correctness of record against each unit as the QRCodes once generated will not be editable"
                                        CssClass="text text-body small float-end" meta:resourcekey="lblHelptext" />
                                </div>
                            </div>
                        </div>

                        <div class="row offset-3">
                            <asp:Label ID="lbl_error" CssClass="col-12 text-danger" runat="server" meta:resourcekey="lbl_errorResource1"></asp:Label>
                            <asp:Label ID="lblStatus" CssClass="col-12 text-success" runat="server" meta:resourcekey="lblStatusResource1"></asp:Label>
                            <asp:UpdateProgress AssociatedUpdatePanelID="panel1" ID="uprog1" runat="server">
                                <ProgressTemplate>
                                    <script>
                                        $(".preloader").fadeIn();
                                    </script>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>
                        <div class="col-sm-12 row offset-3">
                            <asp:Button ID="view_button" runat="server" CausesValidation="False" UseSubmitBehavior="False" Text="Generate QR Code"
                                CssClass="col-sm-2 btn btn-info" meta:resourcekey="view_buttonResource1"></asp:Button>
                            <asp:Button ID="cancel_button" runat="server" CausesValidation="False" UseSubmitBehavior="False"
                                CssClass="col-sm-2 btn btn-secondary" meta:resourcekey="cancel_buttonResource1"></asp:Button>&nbsp;
                        
                        </div>
                    </div>
                    
                </ContentTemplate>
                <Triggers>
                    <%--<asp:AsyncPostBackTrigger ControlID="rbUnitType" EventName="SelectedIndexChanged" />--%>
                    <%--<asp:PostBackTrigger ControlID="ddlDistName" />--%>
                    <%--<asp:AsyncPostBackTrigger ControlID="existing_grid" EventName="RowCommand" />--%>
                    <asp:PostBackTrigger ControlID="rbUnitType" />
                    <asp:PostBackTrigger ControlID="existing_grid" />
                    <asp:PostBackTrigger ControlID="cancel_button" />
                    <asp:PostBackTrigger ControlID="view_button" />
                    <asp:PostBackTrigger ControlID="btnMap" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
  
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
</asp:Content>

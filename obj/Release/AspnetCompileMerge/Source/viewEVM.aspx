<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="viewEVM.aspx.vb" Inherits="SEC_InventoryMgmt.viewEVM" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="dist/js/loaderGoogle.js"></script>


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
                <asp:HiddenField ID="hdnLangCode" runat="server" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="card">
            <asp:UpdatePanel runat="server" ID="panel1" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="card-body">
                        <div id="dvList" runat="server" class="col-12 card-body">
                            <div class="col-12">
                                <div id="lineChart" style="border: 1px solid #ccc; width: 100%; min-height: 350px;"></div>
                            </div>
                            <div class="col-12">
                                <asp:GridView ID="existing_grid" runat="server" AutoGenerateColumns="false" ShowFooter="true" CssClass="table table-striped table-bordered">
                                    <Columns>
                                        <asp:TemplateField HeaderText="" meta:resourcekey="lblDist">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblDisName" Text='<%# Bind("District_Name") %>'  />
                                                <asp:Label runat="server" ID="lbldiscode" Visible="false" Text='<%# Bind("District_Code") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CU Units" meta:resourcekey="lblCU">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblCU" Text='<%# Bind("CuUnits") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BU Units" meta:resourcekey="lblBU">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblBU" Text='<%# Bind("BuUnits") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="VVPAT Units" meta:resourcekey="lblVvpatUnits">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblTotalUnits" Text='<%# Bind("VvpatUnits") %>' />
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
                        <div class="row offset-3 text-center">
                            <%--  <asp:Button ID="view_button" runat="server" CausesValidation="False" UseSubmitBehavior="False" Text="Print All QR Code"
                                CssClass="col-sm-2 btn btn-info" meta:resourcekey="view_buttonResource1"></asp:Button>--%>
                            <asp:Button ID="btnExport" runat="server" CssClass="col-sm-2 btn btn-info" Text="Export to Excel" OnClick="ExportExcel" />
                            &nbsp;<asp:Button ID="cancel_button" runat="server" CausesValidation="False" UseSubmitBehavior="False"
                                CssClass="col-sm-2 btn btn-secondary" meta:resourcekey="cancel_buttonResource1"></asp:Button>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <%--<asp:AsyncPostBackTrigger ControlID="rbUnitType" EventName="SelectedIndexChanged" />--%>
                    <asp:PostBackTrigger ControlID="existing_grid" />
                    <asp:PostBackTrigger ControlID="cancel_button" />
                    <asp:PostBackTrigger ControlID="btnExport" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
    <script type="text/javascript">
        $(".preloader").fadeOut();
        google.charts.load('current', { 'packages': ['corechart'] });

        google.charts.setOnLoadCallback(drawVisualizationNew);
        function drawVisualizationNew() {
            var langCode = $("[id*='<%= hdnLangCode.ClientID %>']");
            //  alert(langCode.val());

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "viewEVM.aspx/getChartData",
                dataType: "json",
                success: function (jsonData, textStatus, jQxhr) {
                    var jsonData = $.parseJSON(jsonData.d);
                    var data3 = new google.visualization.DataTable();

                    var options = {
                        title: 'EVM Units Availability',
                        vAxis: { title: 'Place of EVM Unit' },
                        hAxis: {
                            title: 'EVM Unit Count',
                            gridlines: { count: 701 }, //+1 is importent for the origingridline
                            viewWindow: {
                                min: 0,
                                max: 700
                            }
                        },
                        seriesType: 'bars',
                        series: { 3: { type: 'line' } }
                    };

                    if (langCode.val() == 'hi-IN') {
                        //  alert('hindi');
                        data3.addColumn('string', 'जिले का नाम');
                        data3.addColumn('number', 'कंट्रोल इकाई', { role: 'annotation' });
                        data3.addColumn('number', 'बैलट इकाई', { role: 'annotation' });
                        data3.addColumn('number', 'वीवीपैट इकाई', { role: 'annotation' });


                        options = {
                            title: 'ईवीएम इकाइयों की उपलब्धता की स्थिति',
                            vAxis: { title: 'ईवीएम इकाई का स्थान' },
                            hAxis: {
                                title: 'ईवीएम इकाइयों की संख्या',
                                gridlines: { count: 701 }, //+1 is importent for the origingridline
                                viewWindow: {
                                    min: 0,
                                    max: 700
                                }
                            },
                            seriesType: 'bars',
                            series: { 3: { type: 'line' } }
                        };
                    }
                    else {
                        // alert('english');
                        data3.addColumn('string', 'District Name');
                        data3.addColumn('number', 'Control Unit', { role: 'annotation' });
                        data3.addColumn('number', 'Ballot Unit', { role: 'annotation' });
                        data3.addColumn('number', 'VVPAT Unit', { role: 'annotation' });
                    }

                    jsonData.forEach(function (row) {
                        data3.addRow([
                            row.place,
                            row.CuVal,
                            row.BuVal,
                            row.Vvpat
                        ]);
                    });

                    //var view = new google.visualization.DataView(data);
                    //view.setColumns([0, 1,
                    //    {
                    //        calc: "stringify",
                    //        sourceColumn: 1,
                    //        type: "string",
                    //        role: "annotation"
                    //    }, 2,
                    //    {
                    //        calc: "stringify",
                    //        sourceColumn: 2,
                    //        type: "string",
                    //        role: "annotation"
                    //    }, 3,
                    //    {
                    //        calc: "stringify",
                    //        sourceColumn: 3,
                    //        type: "string",
                    //        role: "annotation"
                    //    }]);

                    var chart = new google.visualization.BarChart(document.getElementById('lineChart'));
                    //var chart = new google.visualization.ComboChart(document.getElementById('lineChart'));
                    chart.draw(data3, options);

                    //chart.draw(view, options);
                },
                err: function () {
                    alert('Error');
                }
            });
        }
    </script>

</asp:Content>

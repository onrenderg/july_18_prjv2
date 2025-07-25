<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="viewBallotBox.aspx.vb" Inherits="SEC_InventoryMgmt.viewBallotBox" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

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
                        <div id="dvList" runat="server" class="col-12">
                            <div class="col-12">
                                <div id="lineChart" style="border: 1px solid #ccc; width: 100%; min-height: 350px;"></div>
                            </div>
                            <div class="col-12">
                                <asp:GridView ID="existing_grid" runat="server" AutoGenerateColumns="false" ShowFooter="true" CssClass="table table-striped table-bordered">
                                    <Columns>
                                        <asp:TemplateField HeaderText="" meta:resourcekey="lblAvailableAt">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblDisName" Text='<%# Bind("Display_Name") %>' />
                                                <asp:Label runat="server" ID="lbldiscode" Visible="false" Text='<%# Bind("to_user_code") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="No. of Ballot Boxes" meta:resourcekey="lblBU">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblBU" Text='<%# Bind("No_Of_Units") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#000F60" ForeColor="White" BorderColor="#000F60" />
                                </asp:GridView>
                            </div>
                        </div>

                        <div class="row">
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
                        <div class="row text-center justify-content-center">
                            <asp:Button ID="btnExport" runat="server" CssClass="col-sm-2 btn btn-info" Text="Export to Excel" OnClick="ExportExcel" />
                            &nbsp;<asp:Button ID="cancel_button" runat="server" CausesValidation="False" UseSubmitBehavior="False"
                                CssClass="col-sm-2 btn btn-secondary" meta:resourcekey="cancel_buttonResource1"></asp:Button>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="existing_grid" />
                    <asp:PostBackTrigger ControlID="cancel_button" />
                    <asp:PostBackTrigger ControlID="btnExport" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
    <style>
        #lineChart {
    width: 100% !important;
    height: 400px !important;
}

    </style>
    <script type="text/javascript">
        $(".preloader").fadeOut();

        google.charts.load('current', { 'packages': ['corechart'] });

        google.charts.setOnLoadCallback(drawVisualizationNew);

        function drawVisualizationNew() {
            var langCode = $("[id*='hdnLangCode']").val();
            var data3 = new google.visualization.DataTable();

            if (langCode == 'hi-IN') {
                data3.addColumn('string', 'जिले का नाम'); // District Name in Hindi
                data3.addColumn('number', 'बैलट बॉक्स'); // Ballot Box Count in Hindi
            } else {
                data3.addColumn('string', 'District Name');
                data3.addColumn('number', 'Ballot Box');
            }

            var dataRows = []; // Store rows to exclude first one

            // Loop through existing_grid rows, skipping the first row (Total row)
            $("#<%= existing_grid.ClientID %> tbody tr").each(function (index) {
                if (index === 0) return; // Ignore the Header row
                if (index === 1) return; // Ignore the first row
                var place = $(this).find("td:eq(0)").text().trim(); // First column: Place/District Name
                var BuVal = parseInt($(this).find("td:eq(1)").text().trim(), 10) || 0; // Second column: Ballot Box count
                dataRows.push([place, BuVal]); // Store data
            });
            // Add the selected rows to DataTable
            if (dataRows.length > 0) {
                data3.addRows(dataRows);
            }

            var options = {
                title: (langCode == 'hi-IN') ? 'बैलट बॉक्स वितरण' : 'Ballot Box Distribution',
                chartArea: { width: '85%', height: '75%' }, // Expand chart area
                hAxis: {
                    title: (langCode == 'hi-IN') ? 'जिले का नाम' : 'District Name',
                    maxAlternation: 2,
                    slantedText: true, // Rotate text for better visibility
                    slantedTextAngle: 30, // Angle of text rotation
                    textStyle: {
                        fontSize: 10, // Adjust font size
                        maxLines: 2 // Allow wrapping (some browsers might still truncate)
                    }
                },
                vAxis: {
                    title: (langCode == 'hi-IN') ? 'बैलट बॉक्स की संख्या' : 'Ballot Box Count',
                    minValue: 0 // Ensures positive numbers
                },
                bars: 'vertical', // Vertical column chart
                legend: { position: "none" } // No legend for better clarity
            };
            


            var chart = new google.visualization.ColumnChart(document.getElementById('lineChart'));
            chart.draw(data3, options);
        }
    </script>
</asp:Content>

<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="viewLocation.aspx.vb" Inherits="SEC_InventoryMgmt.viewLocation" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="assets/libs/datatables.net-bs4/css/dataTables.bootstrap4.css" rel="stylesheet">
    <style>
        th {
            background-color: #000F60;
            color: whitesmoke;
        }

        input[type=radio]:first-child {
            margin-left: 0px;
        }

        input[type=radio]:nth-child(1) {
            margin-left: 30px;
        }

        input[type=checkbox] {
            margin-right: 10px;
            margin-left: 5px;
        }
    </style>
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCUUBXxLSV44eyMedFG5LBazcKIpW5Qlts"></script>
    <script type="text/javascript">
        var markers = [
            <asp:Repeater ID="rptMarkers" runat="server">
                <ItemTemplate>
                    {
                        "lat": '<%# Eval("Latitude")%>',
                     "lng": '<%# Eval("Longitude") %>',
                     "title": '<%# Eval("srno") %>'
                 
                 }
                </ItemTemplate>
                <SeparatorTemplate>
                    ,
                </SeparatorTemplate>
            </asp:Repeater>
        ];
    </script>
    <script type="text/javascript">
        window.onload = function () {
            document.getElementById("dvMap").style.visibility = "hidden";
            var mapOptions = {
                center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
                zoom: 10,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            var infoWindow = new google.maps.InfoWindow();
            var bounds = new google.maps.LatLngBounds();
            var map = new google.maps.Map(document.getElementById("dvMap"), mapOptions);
            for (i = 0; i < markers.length; i++) {
                var data = markers[i]

                var myLatlng = new google.maps.LatLng(data.lat, data.lng);
                bounds.extend(myLatlng);
                map.fitBounds(bounds);
                var marker = new google.maps.Marker({
                    position: myLatlng,
                    map: map,
                    title: '<%# Eval("srno") %>'
                });
                (function (marker, data) {
                    google.maps.event.addListener(marker, "click", function (e) {
                        infoWindow.setContent(data.description);
                        infoWindow.open(map, marker);
                    });
                })(marker, data);
                document.getElementById("dvMap").style.visibility = "visible";
            }
            // map.fitBounds (bounds);
        }
    </script>
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
                            <label for="rbUnitType" class="col-sm-2 text-md-right"><%= GetLocalResourceObject("rbUnitType") %></label>
                            <div class="col-lg-4 col-md-6 col-sm-8">
                                <asp:RadioButtonList ID="rbUnitType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbUnitType_SelectedIndexChanged" CssClass="col-12 select2-container--default" meta:resourcekey="rbUnitTypeResource1">
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="col-sm-12 row justify-content-center">
                            <asp:Label ID="lbl_error" CssClass="text-danger" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblStatus" CssClass="text-success" runat="server" Text="Record saved successfully" Visible="false"></asp:Label>
                            <asp:UpdateProgress AssociatedUpdatePanelID="panel1" ID="uprog1" runat="server">
                                <ProgressTemplate>
                                    <script>
                                        $(".preloader").fadeIn();
                                    </script>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>
                        <div class="col-sm-12 row justify-content-center">
                            <asp:Button ID="btnShow" runat="server" Text="View All" OnClick="btnShow_Click" CssClass="col-sm-2 btn btn-md btn-info" meta:ResourceKey="btnShow"></asp:Button>
                            &nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" PostBackUrl="~/viewLocation.aspx" UseSubmitBehavior="false" CssClass="col-sm-2 btn btn-md btn-secondary" meta:ResourceKey="btnCancel"></asp:Button>
                        </div>
                    </div>
                    <div id="dvList" runat="server" class="card-body">
                        <div class="table-responsive">
                            <%-- <span style="text-align: right; width: 80%;">
                                <asp:Label ID="lblMappingCount" runat="server" Text="" CssClass="col-sm-8 text-info align-bottom"></asp:Label>
                            </span>--%>
                            <asp:GridView ID="existing_grid" runat="server" AutoGenerateColumns="false"
                                CssClass="table table-striped table-bordered">
                                <Columns>
                                    <asp:TemplateField HeaderText="Item SrNo" meta:resourcekey="TemplateFieldResource10">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="id" Text='<%# Bind("id") %>' Visible="false" />
                                            <asp:Label runat="server" ID="srno" Text='<%# Bind("SrNo") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Current Location" meta:resourcekey="TemplateFieldResource4">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="CurLocName" Text='<%# Bind("CurLocName") %>' />
                                          </ItemTemplate>
                                        <ItemStyle Width="30%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="View On Map" meta:resourcekey="TemplateFieldResource3">
                                        <ItemTemplate>
                                             <asp:LinkButton ID="lnkMap" runat="server" Text="View" CommandName="map"
                                                CommandArgument='<%# Bind("Latitude") + "," + Bind("Longitude") %>'
                                                OnClientClick='<%# "showMapModal(" & Eval("Latitude") & "," & Eval("Longitude") & "); return false;" %>'
                                                data-bs-toggle="modal"
                                                data-bs-target="#myMapModal">
                                                <i class="fas fa-map-marker-alt alert-danger"></i> View on Map</asp:LinkButton> 

                                            <%--   <asp:LinkButton ID="lnkMap" runat="server" Text="View" CommandName="map" 
                                                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" data-bs-toggle="modal" data-bs-target="#myMapModal" />--%>
                                      </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false" HeaderText="Location">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="Latitude" Text='<%# Bind("Latitude") %>' CssClass="row" />
                                            <asp:Label runat="server" ID="Longitude" Text='<%# Bind("Longitude") %>' CssClass="row" />
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <span class="text-left text-info">
                                        <p class="m-t-30">
                                            <asp:Label ID="lblNoRow" runat="server" Text="No Records Found" />
                                        </p>
                                    </span>
                                </EmptyDataTemplate>
                                <HeaderStyle BackColor="#000F60" ForeColor="White" BorderColor="#000F60" />
                            </asp:GridView>
                        </div>
                    </div>
                     <div id="dvMap" style="width: 100%; min-height: 300px">
                    </div>
                    <!-- Modal -->
                    <div class="modal fade" id="myMapModal" tabindex="-1" role="dialog" aria-labelledby="myMapModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-lg" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title" id="myMapModalLabel">Location Map</h5>
                                    <button type="button" class="close" data-bs-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body">
                                    <!-- The map container -->
                                    <div id="dvMapModal" style="width: 100%; height: 600px;"></div>
                                </div>
                            </div>
                        </div>
                    </div>

                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="rbUnitType" />
                    <asp:PostBackTrigger ControlID="existing_grid" />
                    <asp:PostBackTrigger ControlID="btnShow" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
    <script src="assets/extra-libs/DataTables/datatables.min.js"></script>
    <script>
        $(".preloader").fadeOut();
        $(document).ready(function () {
            var gv = $('#<%=existing_grid.ClientID%>');
            var thead = $('<thead/>');
            thead.append(gv.find('tr:eq(0)'));
            gv.append(thead);
            gv.dataTable();
        });
    </script>
    <script>
        function showMapModal(lat, lng) {
            // Set up the map options
            var mapOptions = {
                center: new google.maps.LatLng(lat, lng),
                zoom: 15,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };

            // Create a new map inside the modal
            var map = new google.maps.Map(document.getElementById("dvMapModal"), mapOptions);

            // Add a marker at the selected location
            var marker = new google.maps.Marker({
                position: new google.maps.LatLng(lat, lng),
                map: map,
                title: "Selected Location"
            });

            // Adjust map bounds to include the marker
            var bounds = new google.maps.LatLngBounds();
            bounds.extend(marker.getPosition());
            map.fitBounds(bounds);

            // Open the modal
            $('#myMapModal').modal('show');
        }
    </script>

</asp:Content>

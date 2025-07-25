<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="viewBBonMap.aspx.vb" Inherits="SEC_InventoryMgmt.viewBBonMap" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCUUBXxLSV44eyMedFG5LBazcKIpW5Qlts"></script>
    <script type="text/javascript">
        var markers = [];

        window.onload = function () {
            if (markers.length === 0) {
                var defaultCenter = new google.maps.LatLng(31.1048, 77.1734); // Shimla, HP-IN
                var map = new google.maps.Map(document.getElementById("dvMap"), {
                    center: defaultCenter,
                    zoom: 7,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                });
                return;
            }

            var mapOptions = {
                center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
                zoom: 10,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };

            var infoWindow = new google.maps.InfoWindow();
            var bounds = new google.maps.LatLngBounds();
            var map = new google.maps.Map(document.getElementById("dvMap"), mapOptions);

            for (var i = 0; i < markers.length; i++) {
                var data = markers[i];
                var myLatlng = new google.maps.LatLng(data.lat, data.lng);
                bounds.extend(myLatlng);

                var marker = new google.maps.Marker({
                    position: myLatlng,
                    map: map,
                    title: data.title + "\n" + data.description
                });

                (function (marker, data) {
                    google.maps.event.addListener(marker, "click", function () {
                        infoWindow.setContent(
                            "<strong>" + data.title + "</strong><br />" +
                            data.description + "<br /><br />" +
                            "<a href='https://www.google.com/maps/dir/?api=1&destination=" + data.lat + "," + data.lng + "' target='_blank'>🚘 Navigate</a>"
                        );
                        infoWindow.open(map, marker);
                    });
                })(marker, data);
            }

            map.fitBounds(bounds);
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel panel-primary panel-horizontal" id="dvManageBlocks">
        <div class="panel-heading text-center">
            <strong></strong>
        </div>
        <div class="panel-body">
            <div id="dvMap" style="width: 100%; height: 550px;" class="scrollable"></div>
            <div class="col-sm-12 row justify-content-center">
                <asp:Button ID="close" runat="server"
                    OnClientClick="history.back(); return false;"
                    Text="Back"
                    CssClass="col-sm-2 mt-3 btn btn-primary"
                    meta:resourcekey="btnGetDataResource1" />

            </div>
            
        </div>
    </div>
    <%--<div id="dvMap" style="width: 1170px; height: 900px">--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>

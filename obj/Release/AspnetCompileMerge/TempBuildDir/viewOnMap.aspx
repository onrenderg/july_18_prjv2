<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="viewOnMap.aspx.vb" Inherits="SEC_InventoryMgmt.viewOnMap" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCUUBXxLSV44eyMedFG5LBazcKIpW5Qlts"></script>
    <script type="text/javascript">
        var markers = [
            <asp:Repeater ID="rptMarkers" runat="server">
                <ItemTemplate>
                    {
                        "lat": '<%# Eval("Latitude")%>',
                     "lng": '<%# Eval("Longitude") %>',
                     "title": '<%# Eval("title") %>'
                 
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
                    title: data.title
                });
                (function (marker, data) {
                    google.maps.event.addListener(marker, "click", function (e) {
                        infoWindow.setContent(data.description);
                        infoWindow.open(map, marker);
                    });
                })(marker, data);
            }
            // map.fitBounds (bounds);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel panel-primary panel-horizontal" id="dvManageBlocks">
        <div class="panel-heading text-center">
            <strong></strong>
        </div>
        <div class="panel-body">
            <div id="dvMap" style="width: 100%; height: 550px;" class="scrollable">
            </div>
            <div class="form-group row">
                <div class="col-sm-5" style="text-align: right">
                    <asp:Button ID="close" runat="server" OnClientClick="javascript:window.close()" Text="Close" CssClass="btn btn-primary " meta:resourcekey="btnGetDataResource1" />
                    <%--<asp:Button runat="server" ID="btnGetData" OnClientClick=""  Text="Close" CssClass="btn btn-primary " meta:resourcekey="btnGetDataResource1" />--%>
                </div>
            </div>

        </div>
    </div>
    <%--<div id="dvMap" style="width: 1170px; height: 900px">--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>

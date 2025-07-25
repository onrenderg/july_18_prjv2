<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="printForm.aspx.vb" Inherits="SEC_InventoryMgmt.printForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="dist/css/style.min.css" rel="stylesheet" />
    <title></title>
    <style>
        #btn {
            text-align: center;
        }

        .mydatagrid {
            font-family: Mangal;
            font-size: 12px;
        }
        .lbl {
            font-family: Verdana;
            font-size: 9px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                <div class="col-md-12 col-sm-12 col-lg-12">
                    <!-- Div to print -->
                    <div id="printgrid" runat="server" style="font-family: Mangal; margin-left: 5px; margin-right: 5px; margin-top: 5px; margin-bottom:0px; text-align:center">
                        <h2>
                            <asp:Label ID="lblTitle" runat="server" Text="राज्य निर्वाचन आयोग, हिमाचल प्रदेश" CssClass="col-sm-12 col-form-label font-bold" />
                          </h2>
                        <!-- Header In Table -->
                        <p class="col-md-12">
                            <asp:Literal runat="server" ID="liheader"></asp:Literal>
                        </p>
                        <!-- Header In Table -->
                        <!-- Grid Table For Print-->
                        <div class="table table-responsive-sm table-responsive" style="overflow: unset; text-align:left; font-family:Verdana;">
                            <asp:Literal runat="server" ID="litable"></asp:Literal>
                        </div>
                        <!-- End Of Table -->
                     <%--   <br />
                        <div id="foot" class="hide row">
                            <asp:Label ID="lblplace" runat="server" Text="" class="hide col-sm-12 col-form-label"></asp:Label><br />
                            <div id="date" class="col-sm-12">
                                <asp:Label ID="lbldate" runat="server" Text="" class="col-form-label"></asp:Label>
                            </div>
                        </div>--%>
                    </div>
                    <!--End Of Print -->
                    <asp:Label ID="lbl_error" CssClass="row text-danger text-center font-bold" runat="server" Text="No Record Found"></asp:Label>
                    <div id="btn" class="col-sm-12 row justify-content-center">
                        <asp:Button ID="btnPrint" runat="server" Text="Print" OnClientClick="javascript:printList('printgrid');" CssClass="col-2 btn btn-primary" />
                    </div>
                </div>
            </div>
        </div>
    </form>

    <!-- All Jquery -->
    <!-- ============================================================== -->
    <script src="assets/libs/jquery/dist/jquery-3.7.1.min.js"></script>
    <script src="dist/js/common.js"></script>

    <script src="dist/js/jquery-ui-1.14.1/jquery-ui.min.js"></script>
    <script src="assets/libs/bootstrap/dist/js/bootstrap.min.js"></script>
    <!-- slimscrollbar scrollbar JavaScript -->
    <script src="assets/libs/perfect-scrollbar/dist/perfect-scrollbar.jquery.min.js"></script>
    <script src="assets/extra-libs/sparkline/sparkline.js"></script>
    <!--Wave Effects -->
    <script src="dist/js/waves.js"></script>
   <%-- <!--Menu sidebar -->
    <script src="dist/js/sidebarmenu.js"></script>--%>
    <!--Custom JavaScript -->
    <script src="dist/js/custom.min.js"></script>

    <script type="text/javascript">
        function printList(divID) {
            var divElements = document.getElementById(divID).innerHTML;
            //Get the HTML of whole page
            var oldPage = document.body.innerHTML;
            document.body.innerHTML =
                "<html><head><title></title></head><body style='width:100%;'><div style='width:100%;'>" +
                divElements + "</div></body></html>";

            //Print Page
            window.print();
            //Restore orignal HTML
            document.body.innerHTML = oldPage;
        }
    </script>
</body>
</html>

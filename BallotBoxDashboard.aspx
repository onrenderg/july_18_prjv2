<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="BallotboxDashboard.aspx.vb" Inherits="SEC_InventoryMgmt.BallotboxDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Ballot Box Dashboard</title>
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Ballot Box Inventory Header -->
    <div class="card mb-4">
        <div class="card-body">
            <div class="row align-items-center">
                <div class="col-10">
                    <h5 class="card-title mb-1">Ballot Box Inventory</h5>
                    <p class="card-text mb-0">User Id : <asp:Literal ID="litUserId" runat="server"></asp:Literal></p>
                    <p class="card-text">Location : <asp:Literal ID="litLocation" runat="server"></asp:Literal></p>
                </div>
                <div class="col-2 text-end">
                    <span class="material-icons h4">search</span>
                </div>
            </div>
        </div>
    </div>

    <!-- State Total Section -->
    <h4 class="text-center my-4">State Total</h4>
    <div class="row">
        <div class="col-md-6">
            <asp:LinkButton ID="lnkStateParked" runat="server" CssClass="text-decoration-none text-reset" OnClick="lnkStateParked_Click">
                <div class="card text-center mb-3">
                    <div class="card-header bg-dark text-white">
                        <span class="material-icons align-middle me-1">apartment</span> Parked
                    </div>
                    <div class="card-body">
                        <h1 class="card-title"><asp:Literal ID="litStateParked" runat="server" /></h1>
                    </div>
                </div>
            </asp:LinkButton>
        </div>

        <div class="col-md-6">
            <asp:LinkButton ID="lnkStateInTransit" runat="server" CssClass="text-decoration-none text-reset" OnClick="lnkStateInTransit_Click">
                <div class="card text-center mb-3">
                    <div class="card-header bg-dark text-white">
                        <span class="material-icons align-middle me-1">local_shipping</span> In Transit
                    </div>
                    <div class="card-body">
                        <h1 class="card-title"><asp:Literal ID="litStateInTransit" runat="server" /></h1>
                    </div>
                </div>
            </asp:LinkButton>
        </div>
    </div>

    <!-- My Stock Section (Parked, Inward, Outward only) -->
    <h4 class="text-center my-4">My Stock</h4>
    <div class="row text-center">
        <!-- Parked -->
        <div class="col-md-4">
            <asp:LinkButton ID="lnkMyStockParked" runat="server" CssClass="text-decoration-none text-reset" OnClick="lnkMyStockParked_Click">
                <div class="card text-center mb-3">
                    <div class="card-header bg-dark text-white">
                        <span class="material-icons align-middle me-1">apartment</span> Parked
                    </div>
                    <div class="card-body">
                        <h1 class="card-title"><asp:Literal ID="litMyStockParked" runat="server" /></h1>
                    </div>
                </div>
            </asp:LinkButton>
        </div>

        <!-- Inward -->
        <div class="col-md-4">
            <asp:LinkButton ID="lnkInward" runat="server" CssClass="text-decoration-none text-reset" OnClick="lnkInward_Click">
                <div class="card text-center mb-3">
                    <div class="card-header bg-dark text-white">Inward</div>
                    <div class="card-body">
                        <h1 class="card-title"><asp:Literal ID="litInward" runat="server" /></h1>
                    </div>
                </div>
            </asp:LinkButton>
        </div>

        <!-- Outward -->
        <div class="col-md-4">
            <asp:LinkButton ID="lnkOutward" runat="server" CssClass="text-decoration-none text-reset" OnClick="lnkOutward_Click">
                <div class="card text-center mb-3">
                    <div class="card-header bg-dark text-white">Outward</div>
                    <div class="card-body">
                        <h1 class="card-title"><asp:Literal ID="litOutward" runat="server" /></h1>
                    </div>
                </div>
            </asp:LinkButton>
        </div>
    </div>

    <!-- Error Message -->
    <div class="row">
        <div class="col-12">
            <div class="alert alert-danger" role="alert">
                <asp:Literal ID="litErrorMessage" runat="server" />
            </div>
        </div>
    </div>

</asp:Content>

<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="index.aspx.vb" Inherits="SEC_InventoryMgmt.index" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h4 class="page-title">
        <asp:Label ID="Dashboard" runat="server" meta:resourcekey="DashboardResource1"></asp:Label></h4>
    <div class="row">
        <!-- Column -->
        <div class="col-md-6 col-lg-2 col-xlg-3">
            <div class="card card-hover">
                <div class="box bg-cyan text-center">
                    <h1 class="font-light text-white">
                        <i class="mdi mdi-server"></i>
                    </h1>
                    <h6 class="text-white">
                        <asp:Label ID="CntEvm" runat="server" Text="1000" />
                        <asp:Label ID="EVM" runat="server" meta:resourcekey="EVMResource1"></asp:Label></h6>
                </div>
            </div>
        </div>
        <!-- Column -->
        <div class="col-md-6 col-lg-4 col-xlg-3">
            <div class="card card-hover">
                <div class="box bg-success text-center">
                    <h1 class="font-light text-white">
                        <i class="mdi mdi-treasure-chest"></i>
                    </h1>
                    <h6 class="text-white">
                        <asp:Label ID="CntBB" runat="server" Text="22000" />
                        <asp:Label ID="BallotBox" runat="server" Text="Ballot Box" meta:resourcekey="BallotBoxResource1"></asp:Label></h6>
                </div>
            </div>
        </div>
        <!-- Column -->
        <div class="col-md-6 col-lg-2 col-xlg-3">
            <div class="card card-hover">
                <div class="box bg-warning text-center">
                    <h1 class="font-light text-white">
                        <i class="mdi mdi-printer"></i>
                    </h1>
                    <h6 class="text-white">
                        <asp:Label ID="CntVvpat" runat="server" Text="0" />
                        <asp:Label ID="VVPAT" runat="server" Text="VVPAT" meta:resourcekey="VVPATResource1"></asp:Label></h6>
                </div>
            </div>
        </div>
        <!-- Column -->
        <div class="col-md-6 col-lg-2 col-xlg-3">
            <div class="card card-hover">
                <div class="box bg-danger text-center">
                    <h1 class="font-light text-white">
                        <i class="mdi mdi-pencil-box"></i>
                    </h1>
                    <h6 class="text-white">
                        <asp:Label ID="CntStnry" runat="server" Text="30" />
                        <asp:Label ID="StationaryItems" runat="server" Text="Stationery Items" meta:resourcekey="StationaryItemsResource1"></asp:Label></h6>
                </div>
            </div>
        </div>
        <!-- Column -->
        <div class="col-md-6 col-lg-2 col-xlg-3">
            <div class="card card-hover">
                <div class="box bg-info text-center">
                    <h1 class="font-light text-white">
                        <i class="mdi mdi-receipt"></i>
                    </h1>
                    <h6 class="text-white">
                        <asp:Label ID="CntForms" runat="server" Text="169" />
                        <asp:Label ID="Forms" runat="server" Text="Forms" meta:resourcekey="FormsResource1"></asp:Label></h6>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-12">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">
                            <asp:Label ID="lblEvmUnits" runat="server" Text="EVM Units" meta:resourcekey="lblEvmUnits" /></h5>
                        <p>
                            <asp:Label ID="lblEvmUnitsdesc" runat="server" Text="Electronic voting is the standard means of conducting elections using Electronic Voting Machines (EVMs) in India. The system was developed and tested by the state-owned Electronics Corporation of India and Bharat Electronics in the 1990s. They were introduced in Indian elections between 1998 and 2001, in a phased manner." meta:resourcekey="lblEvmUnitsDesc" />
                        </p>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title"><asp:Label ID="lblControlUnits" runat="server" Text="Control Unit" meta:resourcekey="lblControlUnits" /> (<asp:Label ID="lblCu" runat="server" Text="1000" />)</h5>
                      
                        <p>
                            <img src="assets/images/Evmcontrolunit.jpg" height="90%" width="80%" style="min-height: 394px;" alt="EvmControlUnit" />
                        </p>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title"><asp:Label ID="lblBallotUnits" runat="server" Text="Ballot Unit" meta:resourcekey="lblBallotUnits" />(<asp:Label ID="lblBU" runat="server" Text="1000" />)</h5>
                        <p>
                            <img src="assets/images/EvmBallotunit.png" height="50%" width="90%" style="max-height: 395px;" alt="EvmBallotUnit" />
                        </p>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title"><asp:Label ID="lblVVPatUnits" runat="server" Text="VVPAT Unit" meta:resourcekey="lblVVPatUnits" /> (<asp:Label ID="lblVvpat" runat="server" Text="0" />)</h5>
                        <p>
                            <img src="assets/images/EvmVVPAT.jpg" height="100%" width="100%" style="min-height: 394px;" alt="EvmBallotUnit" />
                        </p>
                    </div>
                </div>
            </div>

            <div class="col-md-8">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title"><asp:Label ID="lblBallotBoxes" runat="server" Text="Ballot Box" meta:resourcekey="lblBallotBoxes" /> (<asp:Label ID="lblBB" runat="server" Text="22000" />)</h5>
                        <p>
                            <asp:Label ID="lblBallotBoxesDesc" runat="server" Text="A ballot box is a temporarily sealed container, usually a square box though sometimes a tamper resistant bag, with a narrow slot in the top sufficient to accept a ballot paper in an election but which prevents anyone from accessing the votes cast until the close of the voting period." meta:resourcekey="lblBallotBoxesDesc" />
                 
                        </p>
                    </div>
                </div>
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title"><asp:Label ID="lblStationeryItems" runat="server" Text="Stationery Items" meta:resourcekey="lblStationeryItems" /> (<asp:Label ID="lblStnry" runat="server" Text="30" />)</h5>
                        <p>
                            <asp:Label ID="lblStationeryItemsDesc" runat="server" Text="Various Stationery items related to elctions used by the State Election Commission, Himachal Pradesh" meta:resourcekey="lblStationeryItemsDesc" />
                           
                            <ul>
                                <li><asp:Label ID="lblStationeryItems1" runat="server" Text="Stationery Articles" meta:resourcekey="lblStationeryItems1" /></li>
                                <li><asp:Label ID="lblStationeryItems2" runat="server" Text="Envelopes (Material to be printed on the envelops)" meta:resourcekey="lblStationeryItems2" /></li>
                                <li><asp:Label ID="lblStationeryItems3" runat="server" Text="Sign Boards" meta:resourcekey="lblStationeryItems3" /></li>
                                <li><asp:Label ID="lblStationeryItems4" runat="server" Text="Address Tags" meta:resourcekey="lblStationeryItems4" /></li>
                                <li><asp:Label ID="lblStationeryItems5" runat="server" Text="Poster Symbols" meta:resourcekey="lblStationeryItems5" /></li>
                                <li><asp:Label ID="lblStationeryItems6" runat="server" Text="Ballot Papers" meta:resourcekey="lblStationeryItems6" /></li>
                                <li><asp:Label ID="lblStationeryItems7" runat="server" Text="Misc. Stationery Items" meta:resourcekey="lblStationeryItems7" /></li>

                            </ul>
                        </p>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title"><asp:Label ID="lblBallotBoxes1" runat="server" Text="Ballot Box" meta:Resourcekey="lblBallotBoxes" /></h5>
                        <p>
                            <img src="assets/images/BallotBox.jpg" height="70%" width="90%" style="min-height: 194px;" alt="BallotBox" />
                        </p>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title"><asp:Label ID="lblFormsPRI" runat="server" Text="Forms PRI" meta:Resourcekey="lblFormsPRI" /> (<asp:Label ID="lblFormPRI" runat="server" Text="54" />)</h5>
                        <p>
                            <asp:Label ID="lblFormsPRIDesc" runat="server" Text="Various forms used in elections for Panchayati Raj Institutions." meta:Resourcekey="lblFormsPRIDesc" />
                        </p>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title"><asp:Label ID="lblFormsUlb1" runat="server" Text="Forms Municipalities" meta:Resourcekey="lblFormsUlb1" />  (<asp:Label ID="lblFormULB" runat="server" Text="55" />)</h5>
                        <p>
                            <asp:Label ID="lblFormsUlb1Desc" runat="server" Text="Various forms used in elections for Municipal Committies." meta:Resourcekey="lblFormsUlb1Desc" />
                 
                        </p>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title"><asp:Label ID="lblFormsUlb2" runat="server" Text="Forms MC" meta:Resourcekey="lblFormsUlb2" /> (<asp:Label ID="lblFormMC" runat="server" Text="60" />)</h5>
                        <p>
                            <asp:Label ID="lblFormsUlb2Desc" runat="server" Text="Various forms used in elections for Municipal Corporations." meta:Resourcekey="lblFormsUlb2Desc" />
                               
                        </p>
                    </div>
                </div>
            </div>
            <div class="hide col-md-12">
                <a id="aModal" class='btn btn-info' data-bs-toggle='modal' 
                    data-bs-target='#Modal1'>Open Modal</a>
            </div>
        </div>

        <!-- Modal -->
        <div class="modal fade" id="Modal1" tabindex="-1" role="dialog" aria-hidden="true ">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="ModalLabel">
                            <asp:Label ID="lblModTitle" runat="server" Text=""></asp:Label></h5>
                        <button type="button" class="close" data-bs-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true ">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                       test modal content
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>

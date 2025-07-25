<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BallotBoxSerialList.aspx.vb" Inherits="SEC_InventoryMgmt.BallotBoxSerialList" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Ballot Box Serial List</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Header -->
    <div class="card mb-4">
        <div class="card-body">
            <h5 class="card-title mb-1">Ballot Box Serial Numbers</h5>
        </div>
    </div>

    <!-- Message Row -->
    <div class="row" id="messageRow" runat="server" visible="false">
        <div class="col-12">
            <div class="alert alert-info" role="alert">
                <asp:Literal ID="litMessage" runat="server" />
            </div>
        </div>
    </div>

    <!-- GridView -->
    <div class="table-responsive">
        <asp:GridView ID="gvSerials" runat="server"
                      CssClass="table table-bordered table-hover table-striped"
                      AutoGenerateColumns="True"
                      AllowPaging="True"
                      PageSize="10"
                      OnPageIndexChanging="gvSerials_PageIndexChanging"
                      EmptyDataText="No records found.">
        </asp:GridView>
    </div>

</asp:Content>

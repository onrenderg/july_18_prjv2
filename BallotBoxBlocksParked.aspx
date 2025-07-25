<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="BallotBoxBlocksParked.aspx.vb" Inherits="SEC_InventoryMgmt.BallotBoxBlocksParked" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Blocks Parked</title>
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Header -->
    <div class="card mb-4">
        <div class="card-body">
            <div class="row align-items-center">
                <div class="col-10">
                    <h5 class="card-title mb-1">Blocks Parked</h5>
                    <p class="card-text mb-0">User Id: <asp:Literal ID="litUserId" runat="server" /></p>
                    <p class="card-text">District/Block Code: <asp:Literal ID="litPrevCode" runat="server" /></p>
                </div>
                <div class="col-2 text-end">
                    <span class="material-icons h4">location_city</span>
                </div>
            </div>
        </div>
    </div>

    <!-- Status Message -->
    <div class="row" id="messageRow" runat="server" Visible="false">
        <div class="col-12">
            <div class="alert alert-info" role="alert">
                <asp:Literal ID="litMessage" runat="server" />
            </div>
        </div>
    </div>

    <!-- GridView Styled with Bootstrap -->
    <div class="table-responsive">
        <asp:GridView ID="gvBlocksParked" runat="server"
            CssClass="table table-bordered table-striped table-hover"
            AutoGenerateColumns="False"
            EmptyDataText="No parked blocks found."
            AllowPaging="True"
            PageSize="10"
            OnRowCommand="gvBlocksParked_RowCommand"
            OnPageIndexChanging="gvBlocksParked_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="ddname" HeaderText="Block Name" />
                <asp:BoundField DataField="code" HeaderText="Block Code" />
                <asp:BoundField DataField="total_boxes" HeaderText="Total Boxes" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="is_jurisdiction" HeaderText="Jurisdiction?" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton
                            ID="lnkSelectBlock"
                            runat="server"
                            CssClass="btn btn-sm btn-outline-primary"
                            Text="Select"
                            CommandName="SelectRow"
                            CommandArgument='<%# Eval("code") & "|" & Eval("is_jurisdiction") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

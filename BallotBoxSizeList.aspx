<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BallotBoxSizeList.aspx.vb" Inherits="SEC_InventoryMgmt.BallotBoxSizeList" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Ballot Box Size List</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Header -->
    <div class="card mb-4">
        <div class="card-body">
            <div class="row align-items-center">
                <div class="col-10">
                    <h5 class="card-title mb-1">Ballot Box Sizes</h5>
                    <p class="card-text mb-0">User Id: <asp:Literal ID="litUserId" runat="server" /></p>
                    <p class="card-text">Zone Code: <asp:Literal ID="litZone" runat="server" /></p>
                </div>
                <div class="col-2 text-end">
                    <span class="material-icons h4">inventory</span>
                </div>
            </div>
        </div>
    </div>

    <!-- Status Message -->
    <div class="row" id="messageRow" runat="server" visible="false">
        <div class="col-12">
            <div class="alert alert-info" role="alert">
                <asp:Literal ID="litMessage" runat="server" />
            </div>
        </div>
    </div>

    <!-- GridView to show box size data -->
    <div class="table-responsive">
        <asp:GridView ID="gvBoxSizes" runat="server"
                      CssClass="table table-bordered table-hover table-striped"
                      AutoGenerateColumns="False"
                      AllowPaging="True"
                      PageSize="10"
                      OnPageIndexChanging="gvBoxSizes_PageIndexChanging"
                      OnRowCommand="gvBoxSizes_RowCommand"
                      EmptyDataText="No records found.">
            <Columns>
                <asp:BoundField DataField="zonecode" HeaderText="Zone Code" />
                <asp:BoundField DataField="box_size" HeaderText="Box Size" />
                <asp:BoundField DataField="total_boxes" HeaderText="Total Boxes" />
                <asp:BoundField DataField="is_jurisdiction" HeaderText="Jurisdiction?" />

                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnSelect" runat="server" 
                                    Text="Select" 
                                    CommandName="SelectRow" 
                                    CommandArgument='<%# Eval("box_size") & "|" & Eval("zonecode") & "|" & Eval("is_jurisdiction") %>' 
                                    CssClass="btn btn-sm btn-primary" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>

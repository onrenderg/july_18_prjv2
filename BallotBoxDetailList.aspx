<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="BallotBoxDetailList.aspx.vb"
    Inherits="SEC_InventoryMgmt.BallotBoxDetailList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Ballot Box Details</title>
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Header -->
    <div class="card mb-4">
        <div class="card-body">
            <div class="row align-items-center">
                <div class="col-10">
                    <h5 class="card-title mb-1">Ballot Box Details</h5>
                    <p class="card-text mb-0">User Id: <asp:Literal ID="litUserId" runat="server" /></p>
                    <p class="card-text">Location: <asp:Literal ID="litLocation" runat="server" /></p>
                </div>
                <div class="col-2 text-end">
                    <span class="material-icons h4">list</span>
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
        <asp:GridView ID="gvBallotBoxDetail" runat="server"
            CssClass="table table-bordered table-striped table-hover"
            AutoGenerateColumns="False"
            EmptyDataText="No records found for the selected criteria."
            AllowPaging="True"
            PageSize="10"
            OnRowCommand="gvBallotBoxDetail_RowCommand">
            <Columns>
                <asp:BoundField DataField="ddname" HeaderText="Name" />
                <asp:BoundField DataField="code"   HeaderText="Code" />
                <asp:BoundField DataField="total_boxes" HeaderText="Total Boxes" DataFormatString="{0:N0}" />
                <asp:BoundField DataField="is_jurisdiction" HeaderText="Jurisdiction?" />

                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton
                            ID="lnkSelect"
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

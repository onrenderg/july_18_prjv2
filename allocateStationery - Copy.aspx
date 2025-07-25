<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="allocateStationery.aspx.vb" Inherits="SEC_InventoryMgmt.allocateStationery" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        input[type=radio] {
            margin-left: 30px;
        }

            input[type=radio]:first-child {
                margin-left: 10px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="preloader">
        <div class="lds-ripple">
            <div class="lds-pos"></div>
            <div class="lds-pos"></div>
        </div>
    </div>

    <div class="page-breadcrumb">
        <div class="row">
            <div class="col-12 d-flex no-block align-items-center">
                <h4 class="page-title">
                    <asp:Label ID="lblTitle" runat="server" meta:resourcekey="lblTitleResource1"></asp:Label></h4>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="card">
            <asp:UpdatePanel runat="server" ID="panel1" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="card-body">
                        <div class="form-group row" id="dvPri" runat="server">
                            <asp:Label ID="lblElectionFor" runat="server" CssClass="col-sm-2 text-md-right" Text="Election Type" meta:resourcekey="electionFor" />
                            <div class="col-lg-8 col-md-10 col-sm-10">
                                <asp:RadioButtonList ID="rbPriPost" CssClass="col-12 select2-container--default" runat="server" 
                                    RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbPriPost_SelectedIndexChanged">
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="rbUnitType" class="col-sm-2 text-md-right"><%= GetLocalResourceObject("rbUnitType") %></label>
                            <div class="col-lg-8 col-md-10 col-sm-10">
                                <asp:RadioButtonList ID="rbUnitType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbUnitType_SelectedIndexChanged" CssClass="col-12 select2-container--default" meta:resourcekey="rbUnitTypeResource1">
                                </asp:RadioButtonList>
                            </div>
                        </div>

                        <div class="form-group row">
                            <asp:Label ID="lblDis" runat="server" CssClass="col-sm-2 text-md-right" meta:resourcekey="lblDis"></asp:Label>
                            <div class="col-lg-5 col-md-6 col-sm-6">
                                <asp:DropDownList ID="ddlDis" runat="server" CssClass="form-control" Enabled="false" meta:resourcekey="lblddlDis"
                                    ToolTip="District Name" AutoPostBack="true" OnSelectedIndexChanged="ddlDis_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group row" id="dvBlock" runat="server">
                            <asp:Label ID="lblBlk" runat="server" CssClass="col-sm-2 text-md-right"></asp:Label>
                            <div class="col-lg-5 col-md-6 col-sm-6">
                                <asp:DropDownList ID="ddlBlk" runat="server" CssClass="form-control" meta:resourcekey="lblLevelPS" ToolTip="Choose Block"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlBlk_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group row" id="dvPanch" runat="server">
                            <asp:Label ID="lblPanch" runat="server" class="col-sm-2 text-md-right control-label"></asp:Label>
                            <div class="col-sm-3">
                                <asp:DropDownList ID="ddlPanch" CssClass="form-control" runat="server" AutoPostBack="true" />
                            </div>
                        </div>
                        <div class="form-group row" id="dvWard" runat="server" visible="false">
                            <asp:Label ID="lblConsWard" runat="server" class="col-sm-2 text-md-right control-label" meta:resourcekey="lblWard"></asp:Label>
                            <div class="col-sm-3">
                                <asp:DropDownList ID="ddlConsWard" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                    </div>
                    <div id="dvList" runat="server" class="card-body">
                        <div class="table-responsive">
                            <span style="text-align: right; width: 80%;">
                                <asp:Label ID="lblMappingCount" runat="server" Text="" CssClass="col-sm-8 text-info align-bottom"></asp:Label>
                            </span>
                            <asp:GridView ID="existing_grid" runat="server" AutoGenerateColumns="false" OnRowCommand="existing_grid_RowCommand"
                                CssClass="table table-striped table-bordered" OnRowDeleting="existing_grid_RowDeleting">
                                <Columns>
                                    <asp:TemplateField HeaderText="Item Name" meta:resourcekey="TemplateFieldResource10">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="id" Text='<%# Bind("id") %>' Visible="false" />
                                            <asp:Label runat="server" ID="lblName" Text='<%# Bind("ItemName") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Description" meta:resourcekey="TemplateFieldResource4">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="Itemdescription" Text='<%# Bind("Itemdescription") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="30%" />
                                    </asp:TemplateField>
                                     <asp:TemplateField  HeaderText="Stock Available" meta:resourcekey="TemplateFieldResource4">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblStockCnt" Text='<%# Bind("ItemInStock") %>' CssClass="row"  />
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                   <%-- <asp:TemplateField  HeaderText="Items Already Alloted" meta:resourcekey="TemplateFieldResource4">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblItemsAlloted" Text='<%# Bind("ItemInStock") %>' CssClass="row"  />
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Item Allot" meta:resourcekey="TemplateFieldResource4">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="lblItemCounts"  TextMode="Number" CssClass="form-control" PlaceHolder="No. if Items" />
                                            <asp:Label runat="server" ID="lblCnt" CssClass="row" Visible="false"  />
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" meta:resourcekey="TemplateFieldResource1" Visible="false">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkEdit" ForeColor="#FDBE02" CommandName="editWard" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CausesValidation="False" ToolTip="Edit" meta:resourcekey="lnkEditResource1">
                                            <i class="mdi mdi-lg-4 mdi-table-edit"></i><%=Resources.Resource.lnkEdit %></asp:LinkButton>
                                            &nbsp;
                                            <asp:LinkButton runat="server" ID="lnkDelete" ForeColor="#E34724"
                                                CommandName="delete" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CausesValidation="False"
                                                OnClientClick="return confirmDelete();" ToolTip="Delete" meta:resourcekey="lnkDeleteResource1">
                                                <i class="mdi mdi-lg-4 mdi-delete"></i><%=Resources.Resource.lnkDelete %></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#000F60" ForeColor="White" BorderColor="#000F60" />
                            </asp:GridView>
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
                            <asp:Button ID="save_button" runat="server" Text="Save" CssClass="col-sm-2 btn btn-md btn-info" meta:ResourceKey="btnSave"></asp:Button>
                            &nbsp;
                            <asp:Button ID="cancel_button" runat="server" Text="Cancel" CausesValidation="false" UseSubmitBehavior="false" CssClass="col-sm-2 btn btn-md btn-secondary" meta:ResourceKey="btnCancel"></asp:Button>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="rbPriPost" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ddlBlk" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ddlpanch" EventName="SelectedIndexChanged" />
                    <asp:PostBackTrigger ControlID="rbUnitType" />
                    <%--<asp:PostBackTrigger ControlID="existing_grid" />--%>
                    <asp:PostBackTrigger ControlID="save_button" />
                    <asp:PostBackTrigger ControlID="cancel_button" />

                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>

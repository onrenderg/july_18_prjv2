<%@ Page Title="Edit Master" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="EditMaster.aspx.vb" Inherits="SEC_InventoryMgmt.EditMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        async function transliterateHinglishToHindi(inputId) {
            let inputElement = document.getElementById(inputId);
            let text = inputElement.value;

            if (!text.trim()) return; // Skip if empty

            let apiUrl = `https://inputtools.google.com/request?itc=hi-t-i0-und&num=1&cp=0&text=${encodeURIComponent(text)}&ie=utf-8&oe=utf-8`;

            try {
                let response = await fetch(apiUrl);
                let data = await response.json();

                if (data[0] === "SUCCESS" && data[1]?.[0]?.[1]?.length > 0) {
                    let translatedText = data[1][0][1][0]; // Get first transliterated suggestion
                    inputElement.value = translatedText; // Replace the entire text with the transliterated Hindi text
                }
            } catch (error) {
                console.error("Google Transliteration API Error:", error);
            }
        }

        function attachTransliterationListeners() {
            let itemNameLocal = document.getElementById('<%= ItemNameLocal.ClientID %>');
        let itemDescriptionLocal = document.getElementById('<%= ItemDescriptionLocal.ClientID %>');

        function handleKeyPress(event, inputId) {
            if (event.keyCode === 32) { // Space key is pressed
                transliterateHinglishToHindi(inputId);
            }
        }

        itemNameLocal.addEventListener("keyup", function (event) {
            handleKeyPress(event, '<%= ItemNameLocal.ClientID %>');
        });

        itemDescriptionLocal.addEventListener("keyup", function (event) {
            handleKeyPress(event, '<%= ItemDescriptionLocal.ClientID %>');
        });
        }

        window.onload = attachTransliterationListeners;
    </script>
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
                            <asp:Label runat="server" CssClass="col-sm-2 text-md-right" meta:resourcekey="electionFor" />
                            <div class="col-lg-8 col-md-10 col-sm-10">
                                <asp:Label ID="lblElectionFor" runat="server"  class="col-sm-2 text-md-right fw-bold" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label for="rbUnitType"  runat="server" class="col-sm-2 text-md-right" meta:resourcekey="rbUnitType" />
                            <div class="col-lg-8 col-md-10 col-sm-10">
                                <asp:Label ID="lblrbUnitType" runat="server"  class="col-sm-2 text-md-right fw-bold"/>
                            </div>
                        </div>
                    </div>
                    <hr class="p-0 m-0"/>
                    <div id="dvList" runat="server" class="card-body">
                        <form id="Edit_Form" method="post"> 
                            <asp:HiddenField runat="server" ID="hidden_id" />
                            <asp:HiddenField runat="server" ID="hidden_itemType" />
                            <asp:HiddenField runat="server" ID="hidden_itemFor" />
                            <asp:HiddenField runat="server" ID="hidden_itemSpecification" />
                            <div class="row mb-1">
                                <div class="form-group col-12 col-sm-12 col-md-12 col-lg-4">
                                    <label for="ItemName">Item Name</label>
                                    <asp:TextBox ID="ItemName" runat="server" CssClass="form-control" placeholder="Enter Item Name" required="true"/>
                                </div>
                                <div class="form-group  col-12 col-sm-12 col-md-12 col-lg-8">
                                    <label for="ItemDescription">Item Description</label>
                                    <asp:TextBox ID="ItemDescription" runat="server" CssClass="form-control" placeholder="Enter Item Description" required="true"/>
                                </div>
                            </div>
                            <div class="row mb-1">
                                <div class="form-group  col-12 col-sm-12 col-md-12 col-lg-4">
                                    <label for="ItemNameLocal">आइटम का नाम</label>
                                    <asp:TextBox ID="ItemNameLocal" runat="server" CssClass="form-control" placeholder="आइटम का नाम दर्ज करें" required="true"/>
                                </div>
                                <div class="form-group  col-12 col-sm-12 col-md-12 col-lg-8">
                                    <label for="ItemDescriptionLocal">आइटम विवरण</label>
                                    <asp:TextBox ID="ItemDescriptionLocal" runat="server" CssClass="form-control" placeholder="आइटम विवरण दर्ज करें" required="true"/>
                                </div>
                            </div>
                            <div runat="server" id="alert_div" class="alert alert-danger fade show" role="alert">
                                <asp:Label runat="server" ID="lbl_alert" ></asp:Label>
                            </div>
                            <div class="text-center">
                                <asp:Button ID="submitButton" runat="server" Text="Update (अपडेट करें)" class="btn btn-primary text-center" />
                            </div>
                        </form>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>

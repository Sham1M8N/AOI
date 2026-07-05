<%@ Page Title="Product Details" Language="C#" MasterPageFile="~/Navbar.Master" AutoEventWireup="true" CodeBehind="ProductDetails.aspx.cs" Inherits="AOI.Webforms.ProductDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .product-img {
            max-height: 320px;
            object-fit: contain;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-5 text-center">
            <asp:Image ID="imgProduct" runat="server" CssClass="img-fluid product-img mb-3" />
        </div>
        <div class="col-md-7">
            <h2><asp:Literal ID="litProductName" runat="server" /></h2>
            <p class="text-muted"><asp:Literal ID="litCategory" runat="server" /></p>
            <h4 class="text-primary">RM <asp:Literal ID="litPrice" runat="server" /></h4>
            <p><strong>Availability:</strong> <asp:Literal ID="litAvailability" runat="server" /></p>

            <div class="d-flex align-items-center mb-3">
                <label for="<%= txtQuantity.ClientID %>" class="me-2 mb-0">Qty:</label>
                <asp:TextBox ID="txtQuantity" runat="server" Text="1" CssClass="form-control" style="width:80px" />
            </div>

            <asp:Button ID="btnAddToCart" runat="server" Text="Add to Cart" CssClass="btn btn-primary" OnClick="btnAddToCart_Click" />
            <a href="ProductListing.aspx" class="btn btn-secondary mt-3">Back to Products</a>
            <asp:Literal ID="litAddedMessage" runat="server" />
        </div>
    </div>
</asp:Content>

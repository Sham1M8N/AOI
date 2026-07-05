<%@ Page Title="Laptop Listing" Language="C#" MasterPageFile="~/Navbar.Master" AutoEventWireup="true" CodeBehind="LaptopListing.aspx.cs" Inherits="AOI.Webforms.LaptopListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .product-card {
            border: 1px solid #ddd;
            padding: 15px;
            border-radius: 8px;
            margin-bottom: 20px;
            transition: transform 0.3s;
        }
        .product-card:hover {
            transform: translateY(-5px);
        }
        .product-img {
            max-height: 180px;
            object-fit: contain;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="mb-4">Laptops</h2>
    <div class="row">
        <asp:Repeater ID="rptLaptops" runat="server" OnItemCommand="rptLaptops_ItemCommand">
            <ItemTemplate>
                <div class="col-md-3 mb-4">
                    <div class="product-card text-center">
                        <img src='<%# Eval("ImageUrl") %>' alt='<%# Eval("ProductName") %>' class="img-fluid product-img mb-3" />
                        <h5><%# Eval("ProductName") %></h5>
                        <p class="text-muted">RM <%# Eval("Price", "{0:N2}") %></p>
                        <a href='ProductDetails.aspx?ProductId=<%# Eval("ProductId") %>' class="btn btn-primary">View Details</a>
                        <asp:LinkButton runat="server" CommandName="AddToCart" CommandArgument='<%# Eval("ProductId") %>' CssClass="btn btn-outline-primary mt-2">Add to Cart</asp:LinkButton>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>

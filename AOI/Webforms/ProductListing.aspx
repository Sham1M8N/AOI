<%@ Page Title="All Products" Language="C#" MasterPageFile="~/Navbar.Master" AutoEventWireup="true" CodeBehind="ProductListing.aspx.cs" Inherits="AOI.Webforms.ProductListing" %>

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
    <h2 class="mb-4">All Products</h2>
    <asp:Literal ID="litCartMessage" runat="server" />
    <div class="mb-4">
        <p>Filter by: </p>
    <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
        <asp:ListItem Text="All Categories" Value=""></asp:ListItem>
        <asp:ListItem Text="Laptop" Value="Laptop"></asp:ListItem>
        <asp:ListItem Text="Desktop" Value="Desktop"></asp:ListItem>
        <asp:ListItem Text="Accessories" Value="Accessories"></asp:ListItem>
    </asp:DropDownList>

    <asp:DropDownList ID="ddlSort" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSort_SelectedIndexChanged">
        <asp:ListItem Text="Sort by Price" Value=""></asp:ListItem>
        <asp:ListItem Text="Low to High" Value="ASC"></asp:ListItem>
        <asp:ListItem Text="High to Low" Value="DESC"></asp:ListItem>
    </asp:DropDownList>
</div>

    <div class="row">
        <asp:Repeater ID="rptProducts" runat="server" OnItemCommand="rptProducts_ItemCommand">
            <ItemTemplate>
                <div class="col-md-3 mb-4">
                    <div class="product-card text-center">
                        <img src='<%# Eval("ImageUrl") %>' alt='<%# Eval("ProductName") %>' class="img-fluid product-img mb-3" />
                        <h5><%# Eval("ProductName") %></h5>
                        <p class="text-muted">RM <%# Eval("Price", "{0:N2}") %></p>
                        <p><strong>Availability:</strong> <%# Eval("Availability") %></p>
                        <a href='ProductDetails.aspx?ProductId=<%# Eval("ProductId") %>' class="btn btn-primary">View Details</a>
                        <asp:LinkButton runat="server" CommandName="AddToCart" CommandArgument='<%# Eval("ProductId") %>' CssClass="btn btn-outline-primary mt-2">Add to Cart</asp:LinkButton>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>

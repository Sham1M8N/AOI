<%@ Page Title="Your Cart" Language="C#" MasterPageFile="~/Navbar.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="AOI.Webforms.Cart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .cart-img {
            max-height: 80px;
            object-fit: contain;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="mb-4">Your Cart</h2>

    <asp:Literal ID="litMessage" runat="server" />

    <asp:Panel ID="pnlCart" runat="server">
        <table class="table align-middle">
            <thead>
                <tr>
                    <th>Product</th>
                    <th>Price</th>
                    <th>Quantity</th>
                    <th>Subtotal</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptCart" runat="server" OnItemCommand="rptCart_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <td class="d-flex align-items-center gap-2">
                                <img src='<%#: Eval("ImageUrl") %>' alt='<%#: Eval("ProductName") %>' class="cart-img" />
                                <span><%#: Eval("ProductName") %></span>
                            </td>
                            <td>RM <%# Eval("Price", "{0:N2}") %></td>
                            <td style="width:120px">
                                <asp:TextBox runat="server" ID="txtQty" Text='<%# Eval("Quantity") %>' CssClass="form-control" />
                            </td>
                            <td>RM <%# Eval("Subtotal", "{0:N2}") %></td>
                            <td>
                                <asp:LinkButton runat="server" CommandName="UpdateQty" CommandArgument='<%# Eval("ProductId") %>' CssClass="btn btn-sm btn-outline-primary me-1">Update</asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="Remove" CommandArgument='<%# Eval("ProductId") %>' CssClass="btn btn-sm btn-outline-danger">Remove</asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>

        <div class="row mb-3">
            <div class="col-md-4">
                <div class="input-group">
                    <asp:TextBox ID="txtPromoCode" runat="server" CssClass="form-control" placeholder="Promo code" />
                    <asp:Button ID="btnApplyPromo" runat="server" Text="Apply" CssClass="btn btn-outline-secondary" OnClick="btnApplyPromo_Click" CausesValidation="false" />
                </div>
                <asp:Literal ID="litPromoMessage" runat="server" />
            </div>
        </div>

        <div class="text-end mb-2">
            <div>Subtotal: RM <asp:Literal ID="litSubtotal" runat="server" /></div>
            <asp:Panel ID="pnlPromoApplied" runat="server">
                <div class="text-success">Promo (<asp:Literal ID="litPromoCode" runat="server" />): -RM <asp:Literal ID="litDiscount" runat="server" /></div>
            </asp:Panel>
        </div>

        <div class="d-flex justify-content-between align-items-center">
            <h4>Total: RM <asp:Literal ID="litTotal" runat="server" /></h4>
            <asp:Button ID="btnCheckout" runat="server" Text="Checkout" CssClass="btn btn-success" OnClick="btnCheckout_Click" />
        </div>
    </asp:Panel>
</asp:Content>

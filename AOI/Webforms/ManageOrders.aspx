<%@ Page Title="Manage Orders" Language="C#" MasterPageFile="~/Navbar.Master" AutoEventWireup="true" CodeBehind="ManageOrders.aspx.cs" Inherits="AOI.Webforms.ManageOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h2>Manage Orders</h2>

    <div class="gridview mb-4">
        <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False" DataKeyNames="OrderID"
            OnRowEditing="gvOrders_RowEditing" OnRowUpdating="gvOrders_RowUpdating"
            OnRowCancelingEdit="gvOrders_RowCancelingEdit" OnRowCommand="gvOrders_RowCommand"
            CssClass="table table-bordered table-hover">

            <Columns>
                <asp:BoundField DataField="OrderID" HeaderText="Order ID" ReadOnly="True" />
                <asp:BoundField DataField="Username" HeaderText="Customer" ReadOnly="True" />
                <asp:BoundField DataField="OrderDate" HeaderText="Date" DataFormatString="{0:g}" ReadOnly="True" />
                <asp:BoundField DataField="TotalAmount" HeaderText="Total (RM)" DataFormatString="{0:N2}" ReadOnly="True" />
                <asp:BoundField DataField="PromoCode" HeaderText="Promo" ReadOnly="True" NullDisplayText="-" />
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <%# Eval("Status") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlEditStatus" runat="server">
                            <asp:ListItem Text="Pending" Value="Pending" />
                            <asp:ListItem Text="Processing" Value="Processing" />
                            <asp:ListItem Text="Completed" Value="Completed" />
                            <asp:ListItem Text="Cancelled" Value="Cancelled" />
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="True" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton runat="server" CommandName="ViewItems" CommandArgument='<%# Eval("OrderID") %>' CssClass="btn btn-sm btn-outline-secondary">View Items</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>
    </div>

    <asp:Panel ID="pnlOrderItems" runat="server" Visible="false">
        <h4><asp:Literal ID="litOrderItemsTitle" runat="server" /></h4>
        <asp:GridView ID="gvOrderItems" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered">
            <Columns>
                <asp:BoundField DataField="ProductName" HeaderText="Product" />
                <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                <asp:BoundField DataField="Price" HeaderText="Unit Price (RM)" DataFormatString="{0:N2}" />
                <asp:BoundField DataField="LineTotal" HeaderText="Line Total (RM)" DataFormatString="{0:N2}" />
            </Columns>
        </asp:GridView>
    </asp:Panel>

</asp:Content>

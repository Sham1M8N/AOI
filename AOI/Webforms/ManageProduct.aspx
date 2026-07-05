<%@ Page Title="Manage Products" Language="C#" MasterPageFile="~/Navbar.Master" AutoEventWireup="true" CodeBehind="ManageProduct.aspx.cs" Inherits="AOI.Webforms.ManageProduct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h2>Manage Products</h2>
    <!-- Add New Product Form -->
    <asp:Panel ID="pnlAddProduct" runat="server" CssClass="mb-3">
        <asp:TextBox ID="txtProductName" runat="server" Placeholder="Product Name"></asp:TextBox>
        <asp:TextBox ID="txtPrice" runat="server" Placeholder="Price"></asp:TextBox>
        <asp:TextBox ID="txtImageUrl" runat="server" Placeholder="Image URL"></asp:TextBox>
        <asp:TextBox ID="txtQuantity" runat="server" Placeholder="Quantity"></asp:TextBox>

        <!-- Category Dropdown -->
        <asp:DropDownList ID="ddlCategory" runat="server">
            <asp:ListItem Text="Select Category" Value="" />
            <asp:ListItem Text="Laptop" Value="Laptop" />
            <asp:ListItem Text="Desktop" Value="Desktop" />
            <asp:ListItem Text="Accessories" Value="Accessories" />
        </asp:DropDownList>
    <asp:Button ID="btnAddProduct" runat="server" Text="Add Product" OnClick="btnAddProduct_Click" CssClass="btn btn-primary ms-2" />

</asp:Panel>

    <!-- Gridview -->
    <div class="gridview">
        <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False" DataKeyNames="ProductID"
            OnRowEditing="gvProducts_RowEditing" OnRowUpdating="gvProducts_RowUpdating"
            OnRowCancelingEdit="gvProducts_RowCancelingEdit" OnRowDeleting="gvProducts_RowDeleting"
            CssClass="table table-bordered table-hover">

            <Columns>
                <asp:BoundField DataField="ProductID" HeaderText="ID" ReadOnly="True" />
                <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                <asp:TemplateField HeaderText="Category">
                    <ItemTemplate>
                        <%# Eval("Category") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlEditCategory" runat="server">
                            <asp:ListItem Text="Laptop" Value="Laptop" />
                            <asp:ListItem Text="Desktop" Value="Desktop" />
                            <asp:ListItem Text="Accessories" Value="Accessories" />
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Price" HeaderText="Price" />
                <asp:BoundField DataField="ImageUrl" HeaderText="Image URL" />
                <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
            </Columns>

        </asp:GridView>
    </div>

</asp:Content>

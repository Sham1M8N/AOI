<%@ Page Title="Manage Users" Language="C#" MasterPageFile="~/Navbar.Master" AutoEventWireup="true" CodeBehind="ManageUsers.aspx.cs" Inherits="AOI.Webforms.ManageUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- You can add page-specific CSS or scripts here -->
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
<script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Manage Users</h2>
    <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" CssClass="table table-striped"
    OnRowEditing="gvUsers_RowEditing"
    OnRowUpdating="gvUsers_RowUpdating"
    OnRowCancelingEdit="gvUsers_RowCancelingEdit"
    OnRowDeleting="gvUsers_RowDeleting"
    DataKeyNames="UserId">
    <Columns>
        <asp:BoundField DataField="UserId" HeaderText="User ID" ReadOnly="true" />

        <asp:BoundField DataField="Username" HeaderText="Username" />

        <asp:BoundField DataField="Email" HeaderText="Email" />

        <asp:TemplateField HeaderText="Role">

            <ItemTemplate>
                <%# Eval("Role") %>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList ID="ddlRole" runat="server">
                    <asp:ListItem Text="Admin" Value="Admin" />
                    <asp:ListItem Text="Customer" Value="Customer" />
                </asp:DropDownList>
            </EditItemTemplate>
        </asp:TemplateField>

        <%-- Reset Password Button --%>
        <asp:TemplateField HeaderText="Actions">
            <ItemTemplate>
                <asp:Button ID="btnResetPassword" runat="server"
                    CommandName="ResetPassword"
                    CommandArgument='<%# Eval("UserId") %>'
                    Text="Reset Password"
                    CssClass="btn btn-warning btn-sm" />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
    </Columns>
</asp:GridView>

</asp:Content>

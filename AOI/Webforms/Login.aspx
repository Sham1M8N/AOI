<%@ Page Title="Login" Language="C#" MasterPageFile="~/Navbar.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AOI.Webforms.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/custom.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="register-container shadow">
        <div class="container mt-5" style="max-width: 400px;">
            <h2 class="text-center mb-4">Login</h2>
            <asp:Label ID="lblMessage" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
            <asp:Panel runat="server" CssClass="form-group">
                <asp:Label runat="server" AssociatedControlID="txtUsername" Text="Username:" />
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
            </asp:Panel>
            <asp:Panel runat="server" CssClass="form-group mt-3">
                <asp:Label runat="server" AssociatedControlID="txtPassword" Text="Password:" />
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
            </asp:Panel>
            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary mt-4 w-100" OnClick="btnLogin_Click" />
            <div class="mt-3 text-center">
                <a href="/Webforms/Register.aspx">Don`t have an account? Register</a>
            </div>
        </div>
    </div>
</asp:Content>

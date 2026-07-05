<%@ Page Title="Page Not Found" Language="C#" MasterPageFile="~/Navbar.Master" AutoEventWireup="true" CodeBehind="NotFound.aspx.cs" Inherits="AOI.Webforms.NotFound" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .notfound-container {
            text-align: center;
            padding: 80px 20px;
        }
        .notfound-container h1 {
            font-size: 120px;
            color: #dc3545;
        }
        .notfound-container p {
            font-size: 24px;
        }
        .notfound-image {
            margin-top: 20px;
            max-width: 300px;
            width: 100%;
            height: auto;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="notfound-container">
        <h1>404</h1>
        <p>Sorry, the page you're looking for doesn't exist.</p>
        <a href="/Webforms/Homepage.aspx" class="btn btn-primary mt-3">Return Home</a>
        <br />
        <!-- GIF ERROR -->
        <img src="/Image/usagi_error.gif" alt="Error Image" class="notfound-image" />
    </div>
</asp:Content>

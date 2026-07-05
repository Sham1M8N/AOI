<%@ Page Title="" Language="C#" MasterPageFile="~/Navbar.Master" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="AOI.Webforms.AdminDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Admin Dashboard - AOI</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h1 class="mb-4">Admin Dashboard</h1>

        <div class="row">
            <!-- Card 1: Users -->
            <div class="col-md-4 mb-3">
                <div class="card text-white bg-primary h-100">
                    <div class="card-body">
                        <h5 class="card-title">Users</h5>
                        <p class="card-text">Manage all registered users.</p>
                        <a href="/Webforms/ManageUsers.aspx" class="btn btn-light btn-sm">Go to Users</a>
                    </div>
                </div>
            </div>

            <!-- Card 2: Products -->
            <div class="col-md-4 mb-3">
                <div class="card text-white bg-success h-100">
                    <div class="card-body">
                        <h5 class="card-title">Products</h5>
                        <p class="card-text">Add, edit, or remove products.</p>
                        <a href="/Webforms/ManageProduct.aspx" class="btn btn-light btn-sm">Go to Products</a>
                    </div>
                </div>
            </div>

            <!-- Card 3: Orders -->
            <div class="col-md-4 mb-3">
                <div class="card text-white bg-warning h-100">
                    <div class="card-body">
                        <h5 class="card-title">Orders</h5>
                        <p class="card-text">Review and process orders.</p>
                        <a href="/Webforms/ManageOrders.aspx" class="btn btn-light btn-sm">Go to Orders</a>
                    </div>
                </div>
            </div>
        </div>

        <!-- Optional stats section -->
        <div class="mt-4">
            <h3>Quick Stats</h3>
            <ul class="list-group">
                <li class="list-group-item d-flex justify-content-between align-items-center">Total Users
            <asp:Label ID="lblTotalUsers" runat="server" CssClass="badge bg-primary rounded-pill">0</asp:Label>
                </li>
                <li class="list-group-item d-flex justify-content-between align-items-center">Total Products
            <asp:Label ID="lblTotalProducts" runat="server" CssClass="badge bg-success rounded-pill">0</asp:Label>
                </li>
                <li class="list-group-item d-flex justify-content-between align-items-center">Pending Orders
            <asp:Label ID="lblPendingOrders" runat="server" CssClass="badge bg-warning rounded-pill">0</asp:Label>
                </li>
            </ul>
        </div>
    </div>
</asp:Content>


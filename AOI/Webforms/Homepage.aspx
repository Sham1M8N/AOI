<%@ Page Title="Home" Language="C#" MasterPageFile="~/Navbar.Master" AutoEventWireup="true" CodeBehind="Homepage.aspx.cs" Inherits="AOI.Webforms.Homepage" %>

<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">
    <link href="/css/custom.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <body class="bg-black text-light">
    <!-- Hero Section -->
    <div class="text-center mt-5">
        <h1>Welcome to AOI</h1>
        <p class="lead">Your one-stop shop for awesome tech products.</p>
    </div>

    <!-- Carousel Section -->
    <div id="carouselExampleIndicators" class="carousel slide mt-5" data-bs-ride="carousel">
        <div class="carousel-indicators">
            <button type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide-to="0" class="active" aria-current="true" aria-label="Slide 1"></button>
            <button type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide-to="1" aria-label="Slide 2"></button>
        </div>
        <div class="carousel-inner">
            <div class="carousel-item active">
                <img src="/Image/AOIbanner1.gif" class="d-block w-100 img-fluid" alt="First slide">
            </div>
            <div class="carousel-item">
                <img src="/Image/Banner2.jpg" class="d-block w-100 img-fluid" alt="Second slide">
            </div>
        </div>
        <button class="carousel-control-prev" type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide="prev">
            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
            <span class="visually-hidden">Previous</span>
        </button>
        <button class="carousel-control-next" type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide="next">
            <span class="carousel-control-next-icon" aria-hidden="true"></span>
            <span class="visually-hidden">Next</span>
        </button>
    </div>

    <!-- Featured Products Section -->
    <div class="container mt-5">
        <h2 class="text-center mb-4">Featured Products</h2>
        <div class="row">
            <!-- Product 1 -->
            <div class="col-lg-4 col-md-6 col-sm-12 mb-4">
                <div class="card h-100 text-center">
                    <img src="/Image/Products/itx-build.jpg" class="card-img-top img-fluid" alt="Ultimate ITX Build">
                    <div class="card-body">
                        <h5 class="card-title">Ultimate ITX Build</h5>
                        <p class="card-text">Compact yet powerful.</p>
                        <a href="#" class="btn btn-primary" aria-label="View details for ITX Build">View Details</a>
                    </div>
                </div>
            </div>

            <!-- Product 2 -->
            <div class="col-lg-4 col-md-6 col-sm-12 mb-4">
                <div class="card h-100 text-center">
                    <img src="/Image/Products/rtx4070.png" class="card-img-top img-fluid" alt="NVIDIA RTX 4070">
                    <div class="card-body">
                        <h5 class="card-title">NVIDIA RTX 4070</h5>
                        <p class="card-text">Next-gen graphics.</p>
                        <a href="#" class="btn btn-primary" aria-label="View details for RTX 4070">View Details</a>
                    </div>
                </div>
            </div>

            <!-- Product 3 -->
            <div class="col-lg-4 col-md-6 col-sm-12 mb-4">
                <div class="card h-100 text-center">
                    <img src="/Image/Products/lianli.jpg" class="card-img-top img-fluid" alt="Liquid Cooler">
                    <div class="card-body">
                        <h5 class="card-title">Liquid Cooler</h5>
                        <p class="card-text">Silent and efficient.</p>
                        <a href="#" class="btn btn-primary" aria-label="View details for Liquid Cooler">View Details</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </body>
</asp:Content>

-- AOI database schema + seed data.
-- Run via setup-database.ps1, or manually against a database created at
-- AOI/App_Data/AOIdb.mdf (matches the connection string in Web.config).

CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL,
    Role NVARCHAR(50) NOT NULL DEFAULT ('Customer')
);

CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(255) NOT NULL,
    Category NVARCHAR(100) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    ImageUrl NVARCHAR(500) NULL,
    Quantity INT NOT NULL,
    CreatedDate DATETIME NULL DEFAULT (GETDATE())
);

CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    OrderDate DATETIME NOT NULL DEFAULT (GETDATE()),
    TotalAmount DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    PromoCode NVARCHAR(50) NULL,
    CONSTRAINT FK_Orders_Users FOREIGN KEY (UserID) REFERENCES Users(UserId)
);

CREATE TABLE OrderItems (
    OrderItemID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    CONSTRAINT FK_OrderItems_Products FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

CREATE TABLE PromoCodes (
    Code NVARCHAR(50) NOT NULL PRIMARY KEY,
    DiscountPercent DECIMAL(5,2) NOT NULL,
    IsActive BIT NOT NULL DEFAULT (1),
    ExpiryDate DATETIME NULL
);

-- Seed: product catalog
INSERT INTO Products (ProductName, Category, Price, ImageUrl, Quantity) VALUES
('Acer Aspire 15', 'Laptop', 2999.00, '/Image/ProductListing/Laptop/Acer Aspire 15/Acer Aspire 15.jpg', 10),
('Acer Aspire GO 15', 'Laptop', 4499.00, '/Image/ProductListing/Laptop/Acer Aspire GO 15/Acer Aspire GO 15.jpg', 5),
('Asus TUF Dash F15', 'Laptop', 5499.00, '/Image/ProductListing/Laptop/Asus TUF Dash F15/Asus TUF Dash F15.jpg', 8),
('HP OMEN MAX 16', 'Laptop', 4199.00, '/Image/ProductListing/Laptop/HP OMEN MAX 16/HP OMEN MAX 16.jpg', 7),
('Lenovo IdeaPad 1', 'Laptop', 4999.00, '/Image/ProductListing/Laptop/Lenovo IdeaPad 1/Lenovo IdeaPad 1.jpg', 12),
('ESSENTIAL PACKAGE 1', 'Desktop', 999.00, '/Image/ProductListing/Desktop/ESSENTIAL PACKAGE 1.jpg', 2),
('ESSENTIAL PACKAGE 2', 'Desktop', 1500.00, '/Image/ProductListing/Desktop/ESSENTIAL PACKAGE 2.jpg', 5),
('EXODUS PACKAGE', 'Desktop', 6000.00, '/Image/ProductListing/Desktop/EXODUS PACKAGE.jpg', 5),
('NEUTRON PACKAGE', 'Desktop', 7000.00, '/Image/ProductListing/Desktop/NEUTRON PACKAGE.jpg', 5),
('PHANTOM PACKAGE', 'Desktop', 10000.00, '/Image/ProductListing/Desktop/PHANTOM PACKAGE.jpg', 5),
('Logitech G213 PRODIGY', 'Accessories', 200.00, '/Image/ProductListing/Accessories/Keyboard/Logitech G213 PRODIGY.jpg', 5),
('Logitech G512 Carbon', 'Accessories', 250.00, '/Image/ProductListing/Accessories/Keyboard/Logitech G512 Carbon.jpg', 5),
('GIGABYTE G24F 2', 'Accessories', 699.00, '/Image/ProductListing/Accessories/Monitor/GIGABYTE G24F 2.jpg', 5),
('Gigabyte M34WQ', 'Accessories', 999.00, '/Image/ProductListing/Accessories/Monitor/Gigabyte M34WQ.jpg', 5),
('Logitech G502 HERO', 'Accessories', 150.00, '/Image/ProductListing/Accessories/Mouse/Logitech G502 HERO.jpg', 5);

-- Seed: promo codes
INSERT INTO PromoCodes (Code, DiscountPercent, IsActive, ExpiryDate) VALUES
('SAVE10', 10.00, 1, NULL),
('WELCOME15', 15.00, 1, NULL);

-- Seed: default admin account. Username: admin | Password: Admin123!
-- CHANGE THIS PASSWORD (via Manage Users > Reset Password) before using this
-- anywhere beyond local development - this hash is public in this repo.
INSERT INTO Users (Username, Email, PasswordHash, Role) VALUES
('admin', 'admin@example.com', 'PBKDF2$100000$iyecbC/NoI7NyiJWoDCBLQ==$z57vkmImSqN6Xcqij7GJ0J8NhwcCxoMc0NNSONqcruo=', 'Admin');

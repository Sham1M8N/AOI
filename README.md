# AOI

A full-stack e-commerce web app for computer parts and laptops, built with ASP.NET Web Forms and SQL Server. Customer-facing storefront with cart, stock-aware checkout, and promo codes; an admin panel for managing products, orders, and users.

## Screenshots

**Homepage**
![Homepage](screenshots/homepage.png)

**Product Listing** — filter by category, sort by price
![Product Listing](screenshots/productlisting.png)

**Product Details** — live stock display, add to cart
![Product Details](screenshots/productdetails.png)

## Features

**Customer**
- Browse products by category (Laptop / Desktop / Accessories), sort by price
- Cart with live stock validation — can't add more than what's in stock, re-checked again at checkout
- Promo codes with percentage discounts
- Register / login with secure password storage

**Admin**
- Dashboard with live counts (users, products, pending orders)
- Product management (add / edit / delete, with order-history protection on delete)
- User management (edit, delete, reset password)
- Order management (view items, update status)

## Tech stack

- ASP.NET Web Forms, C#, .NET Framework 4.7.2
- SQL Server LocalDB
- Bootstrap 5

## Getting started

1. Clone the repo
2. Run the database setup script (creates the LocalDB database and seeds product data + a default admin account):
   ```powershell
   Database/setup-database.ps1
   ```
3. Open `AOI.sln` in Visual Studio and press F5

Default admin login: `admin` / `Admin123!` — **change this** (Manage Users → Reset Password) before using the app beyond local development, since this hash is public in this repo.

## Security notes

Passwords are hashed with salted PBKDF2 (100,000 iterations). All SQL is parameterized. Output is HTML-encoded at every render sink to prevent stored XSS. Stock levels are validated both when adding to cart and again at checkout to prevent overselling.

## License

MIT — see [LICENSE](LICENSE).

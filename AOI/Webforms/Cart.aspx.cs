using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;

namespace AOI.Webforms
{
    public partial class Cart : System.Web.UI.Page
    {
        private string ConnectionString => ConfigurationManager.ConnectionStrings["AOIDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCart();
            }
        }

        private void LoadCart()
        {
            var lines = AOI.Models.Cart.GetLines(Session);
            if (lines.Count == 0)
            {
                pnlCart.Visible = false;
                litMessage.Text = "<p>Your cart is empty.</p>";
                return;
            }

            pnlCart.Visible = true;
            litMessage.Text = "";

            DataTable dt = new DataTable();
            dt.Columns.Add("ProductId", typeof(int));
            dt.Columns.Add("ProductName", typeof(string));
            dt.Columns.Add("Price", typeof(decimal));
            dt.Columns.Add("ImageUrl", typeof(string));
            dt.Columns.Add("Quantity", typeof(int));
            dt.Columns.Add("Subtotal", typeof(decimal));

            decimal subtotalTotal = 0;

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                foreach (var line in lines)
                {
                    string query = "SELECT ProductName, Price, ImageUrl FROM Products WHERE ProductID = @id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", line.ProductId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            decimal price = Convert.ToDecimal(reader["Price"]);
                            decimal subtotal = price * line.Quantity;
                            subtotalTotal += subtotal;

                            dt.Rows.Add(line.ProductId, reader["ProductName"].ToString(), price,
                                reader["ImageUrl"].ToString(), line.Quantity, subtotal);
                        }
                    }
                }
            }

            rptCart.DataSource = dt;
            rptCart.DataBind();

            decimal discountPercent = Session["PromoDiscount"] as decimal? ?? 0;
            decimal discount = subtotalTotal * discountPercent / 100m;
            decimal grandTotal = subtotalTotal - discount;

            litSubtotal.Text = subtotalTotal.ToString("N2");
            if (discountPercent > 0)
            {
                pnlPromoApplied.Visible = true;
                litPromoCode.Text = Session["PromoCode"] as string;
                litDiscount.Text = discount.ToString("N2");
            }
            else
            {
                pnlPromoApplied.Visible = false;
            }
            litTotal.Text = grandTotal.ToString("N2");
        }

        protected void btnApplyPromo_Click(object sender, EventArgs e)
        {
            string code = txtPromoCode.Text.Trim();
            if (string.IsNullOrEmpty(code))
            {
                litPromoMessage.Text = "<p class='text-danger'>Please enter a promo code.</p>";
                LoadCart();
                return;
            }

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT DiscountPercent FROM PromoCodes WHERE Code = @Code AND IsActive = 1 AND (ExpiryDate IS NULL OR ExpiryDate >= @Now)",
                    con);
                cmd.Parameters.AddWithValue("@Code", code);
                cmd.Parameters.AddWithValue("@Now", DateTime.Now);

                object result = cmd.ExecuteScalar();
                if (result == null)
                {
                    Session.Remove("PromoCode");
                    Session.Remove("PromoDiscount");
                    litPromoMessage.Text = "<p class='text-danger'>Invalid or expired promo code.</p>";
                }
                else
                {
                    Session["PromoCode"] = code;
                    Session["PromoDiscount"] = Convert.ToDecimal(result);
                    litPromoMessage.Text = "<p class='text-success'>Promo code applied!</p>";
                }
            }

            LoadCart();
        }

        protected void rptCart_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int productId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "UpdateQty")
            {
                TextBox txtQty = (TextBox)e.Item.FindControl("txtQty");
                if (int.TryParse(txtQty.Text.Trim(), out int qty))
                {
                    if (qty > 0)
                    {
                        using (SqlConnection con = new SqlConnection(ConnectionString))
                        {
                            con.Open();
                            SqlCommand cmd = new SqlCommand("SELECT Quantity FROM Products WHERE ProductID = @id", con);
                            cmd.Parameters.AddWithValue("@id", productId);
                            int stock = Convert.ToInt32(cmd.ExecuteScalar());
                            if (qty > stock) qty = stock;
                        }
                    }
                    AOI.Models.Cart.UpdateQuantity(Session, productId, qty);
                }
            }
            else if (e.CommandName == "Remove")
            {
                AOI.Models.Cart.RemoveItem(Session, productId);
            }

            LoadCart();
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("~/Webforms/Login.aspx");
                return;
            }

            var lines = AOI.Models.Cart.GetLines(Session);
            if (lines.Count == 0) return;

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    SqlCommand userCmd = new SqlCommand("SELECT UserId FROM Users WHERE Username = @Username", con, transaction);
                    userCmd.Parameters.AddWithValue("@Username", Session["Username"].ToString());
                    int userId = (int)userCmd.ExecuteScalar();

                    decimal subtotal = 0;
                    var itemsToInsert = new System.Collections.Generic.List<(int ProductId, int Quantity, decimal Price)>();

                    foreach (var line in lines)
                    {
                        SqlCommand stockCheckCmd = new SqlCommand("SELECT Price, Quantity FROM Products WHERE ProductID = @id", con, transaction);
                        stockCheckCmd.Parameters.AddWithValue("@id", line.ProductId);
                        using (SqlDataReader reader = stockCheckCmd.ExecuteReader())
                        {
                            if (!reader.Read() || Convert.ToInt32(reader["Quantity"]) < line.Quantity)
                            {
                                transaction.Rollback();
                                litMessage.Text = "<p class='text-danger'>Some items no longer have enough stock. Please review your cart.</p>";
                                LoadCart();
                                return;
                            }

                            decimal price = Convert.ToDecimal(reader["Price"]);
                            subtotal += price * line.Quantity;
                            itemsToInsert.Add((line.ProductId, line.Quantity, price));
                        }
                    }

                    string promoCode = Session["PromoCode"] as string;
                    decimal discountPercent = Session["PromoDiscount"] as decimal? ?? 0;
                    decimal total = subtotal - (subtotal * discountPercent / 100m);

                    SqlCommand orderCmd = new SqlCommand(
                        "INSERT INTO Orders (UserID, OrderDate, TotalAmount, Status, PromoCode) OUTPUT INSERTED.OrderID VALUES (@UserId, @OrderDate, @Total, @Status, @PromoCode)",
                        con, transaction);
                    orderCmd.Parameters.AddWithValue("@UserId", userId);
                    orderCmd.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                    orderCmd.Parameters.AddWithValue("@Total", total);
                    orderCmd.Parameters.AddWithValue("@Status", "Pending");
                    orderCmd.Parameters.AddWithValue("@PromoCode", (object)promoCode ?? DBNull.Value);
                    int orderId = (int)orderCmd.ExecuteScalar();

                    foreach (var item in itemsToInsert)
                    {
                        SqlCommand itemCmd = new SqlCommand(
                            "INSERT INTO OrderItems (OrderID, ProductID, Quantity, Price) VALUES (@OrderId, @ProductId, @Quantity, @Price)",
                            con, transaction);
                        itemCmd.Parameters.AddWithValue("@OrderId", orderId);
                        itemCmd.Parameters.AddWithValue("@ProductId", item.ProductId);
                        itemCmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                        itemCmd.Parameters.AddWithValue("@Price", item.Price);
                        itemCmd.ExecuteNonQuery();

                        SqlCommand stockCmd = new SqlCommand(
                            "UPDATE Products SET Quantity = Quantity - @Qty WHERE ProductID = @id",
                            con, transaction);
                        stockCmd.Parameters.AddWithValue("@Qty", item.Quantity);
                        stockCmd.Parameters.AddWithValue("@id", item.ProductId);
                        stockCmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }

            AOI.Models.Cart.Clear(Session);
            Session.Remove("PromoCode");
            Session.Remove("PromoDiscount");
            pnlCart.Visible = false;
            litMessage.Text = "<p class='text-success'>Order placed successfully! Thank you for shopping with AOI.</p>";
        }
    }
}

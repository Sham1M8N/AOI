using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AOI.Webforms
{
    public partial class ProductDetails : System.Web.UI.Page
    {
        private int productId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Request.QueryString["ProductId"], out productId))
            {
                Response.Redirect("~/Webforms/NotFound.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadProduct(productId);
            }
        }

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtQuantity.Text.Trim(), out int quantity) || quantity <= 0)
                quantity = 1;

            string cs = ConfigurationManager.ConnectionStrings["AOIDb"].ConnectionString;
            int stock;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Quantity FROM Products WHERE ProductID = @id", con);
                cmd.Parameters.AddWithValue("@id", productId);
                stock = Convert.ToInt32(cmd.ExecuteScalar());
            }

            int alreadyInCart = AOI.Models.Cart.GetLines(Session)
                .Where(l => l.ProductId == productId)
                .Sum(l => l.Quantity);

            if (stock - alreadyInCart <= 0)
            {
                litAddedMessage.Text = "<p class='text-danger mt-2'>Sorry, this item is out of stock.</p>";
                return;
            }

            if (quantity > stock - alreadyInCart)
                quantity = stock - alreadyInCart;

            AOI.Models.Cart.AddItem(Session, productId, quantity);
            litAddedMessage.Text = "<p class='text-success mt-2'>Added to cart!</p>";
        }

        private void LoadProduct(int productId)
        {
            string cs = ConfigurationManager.ConnectionStrings["AOIDb"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ProductID, ProductName, Category, Price, Quantity, ImageUrl FROM Products WHERE ProductID = @id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", productId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    Response.Redirect("~/Webforms/NotFound.aspx");
                    return;
                }

                DataRow row = dt.Rows[0];
                litProductName.Text = Server.HtmlEncode(row["ProductName"].ToString());
                litCategory.Text = Server.HtmlEncode(row["Category"].ToString());
                litPrice.Text = Convert.ToDecimal(row["Price"]).ToString("N2");
                imgProduct.ImageUrl = row["ImageUrl"].ToString();

                int quantity = 0;
                if (row["Quantity"] != DBNull.Value)
                    int.TryParse(row["Quantity"].ToString(), out quantity);

                litAvailability.Text = quantity > 0 ? $"In Stock ({quantity})" : "Out of Stock";
            }
        }
    }
}

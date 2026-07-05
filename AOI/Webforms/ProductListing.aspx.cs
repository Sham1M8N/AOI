using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AOI.Webforms
{
    public partial class ProductListing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
            }
        }
        protected void LoadProducts()
        {
            string categoryFilter = ddlCategory.SelectedValue;
            string priceSort = ddlSort.SelectedValue;

            string query = "SELECT ProductID, ProductName, Price, Quantity, Category, ImageUrl FROM Products WHERE 1=1";

            if (!string.IsNullOrEmpty(categoryFilter))
                query += " AND Category = @Category";

            if (!string.IsNullOrEmpty(priceSort))
                query += $" ORDER BY Price {priceSort}";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AOIDb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                if (!string.IsNullOrEmpty(categoryFilter))
                    cmd.Parameters.AddWithValue("@Category", categoryFilter);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Add Availability column if it doesn't exist
                if (!dt.Columns.Contains("Availability"))
                    dt.Columns.Add("Availability", typeof(string));

                // Calculate Availability based on Quantity
                foreach (DataRow row in dt.Rows)
                {
                    int quantity = 0;
                    if (row["Quantity"] != DBNull.Value)
                    {
                        int.TryParse(row["Quantity"].ToString(), out quantity);
                    }
                    row["Availability"] = quantity > 0 ? $"In Stock ({quantity})" : "Out of Stock";
                }

                rptProducts.DataSource = dt;
                rptProducts.DataBind();
            }
        }



        protected void rptProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "AddToCart")
            {
                int productId = Convert.ToInt32(e.CommandArgument);

                int stock;
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AOIDb"].ConnectionString))
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
                    litCartMessage.Text = "<p class='text-danger'>Sorry, this item is out of stock.</p>";
                else
                    AOI.Models.Cart.AddItem(Session, productId);

                LoadProducts();
            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }
    }
}
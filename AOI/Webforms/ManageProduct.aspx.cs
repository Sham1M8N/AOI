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
    public partial class ManageProduct : System.Web.UI.Page
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["AOIDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("~/Webforms/NotFound.aspx");
            }

            if (!IsPostBack)
            {
                BindProducts();
            }
        }

        private void BindProducts()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        ProductID, 
                        ProductName, 
                        Category, 
                        Price, 
                        ImageUrl, 
                        Quantity
                    FROM Products";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvProducts.DataSource = dt;
                gvProducts.DataBind();
            }
        }

        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            string name = txtProductName.Text.Trim();
            string category = ddlCategory.SelectedValue;
            string imageUrl = txtImageUrl.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(category)) return;

            if (!decimal.TryParse(txtPrice.Text.Trim(), out decimal price) ||
                !int.TryParse(txtQuantity.Text.Trim(), out int quantity)) return;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO Products (ProductName, Category, Price, ImageUrl, Quantity) VALUES (@name, @category, @price, @imageUrl, @quantity)";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@imageUrl", imageUrl);
                cmd.Parameters.AddWithValue("@quantity", quantity);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            // Clear inputs
            txtProductName.Text = txtPrice.Text = txtImageUrl.Text = txtQuantity.Text = "";
            ddlCategory.SelectedIndex = 0;

            BindProducts();
        }

        protected void gvProducts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvProducts.EditIndex = e.NewEditIndex;
            BindProducts();
        }

        protected void gvProducts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvProducts.EditIndex = -1;
            BindProducts();
        }

        protected void gvProducts_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int productId = Convert.ToInt32(gvProducts.DataKeys[e.RowIndex].Value);

            var row = gvProducts.Rows[e.RowIndex];
            string name = ((TextBox)row.Cells[1].Controls[0]).Text.Trim();
            string priceText = ((TextBox)row.Cells[3].Controls[0]).Text.Trim();
            string imageUrl = ((TextBox)row.Cells[4].Controls[0]).Text.Trim();
            string quantityText = ((TextBox)row.Cells[5].Controls[0]).Text.Trim();
            var ddlCategory = (DropDownList)row.FindControl("ddlEditCategory");

            if (ddlCategory == null) return;

            string category = ddlCategory.SelectedValue;

            if (!decimal.TryParse(priceText, out decimal price) ||
                !int.TryParse(quantityText, out int quantity)) return;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "UPDATE Products SET ProductName=@name, Category=@category, Price=@price, ImageUrl=@imageUrl, Quantity=@quantity WHERE ProductID=@id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@imageUrl", imageUrl);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@id", productId);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            gvProducts.EditIndex = -1;
            BindProducts();
        }

        protected void gvProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int productId = Convert.ToInt32(gvProducts.DataKeys[e.RowIndex].Value);

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sql = "DELETE FROM Products WHERE ProductID=@id";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@id", productId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                litMessage.Text = "";
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                litMessage.Text = "<p class='text-danger'>Can't delete this product - it appears in existing orders.</p>";
            }

            BindProducts();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AOI.Webforms
{
	public partial class LaptopListing : System.Web.UI.Page
	{
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadLaptops();
            }
        }
        private void LoadLaptops()
        {
            string cs = ConfigurationManager.ConnectionStrings["AOIDb"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("SELECT ProductId, ProductName, Price, ImageUrl FROM Products WHERE Category = 'Laptop'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                rptLaptops.DataSource = dt;
                rptLaptops.DataBind();
            }
        }

        protected void rptLaptops_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "AddToCart")
            {
                int productId = Convert.ToInt32(e.CommandArgument);

                int stock;
                string cs = ConfigurationManager.ConnectionStrings["AOIDb"].ConnectionString;
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

                if (stock - alreadyInCart > 0)
                    AOI.Models.Cart.AddItem(Session, productId);

                LoadLaptops();
            }
        }
    }
}
        
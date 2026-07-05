using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AOI.Webforms
{
	public partial class AdminDashboard : System.Web.UI.Page
	{
        protected void Page_Load(object sender, EventArgs e)
        {
            SecurityHelper.RequireAdmin(this);

            if (!IsPostBack)
            {
                LoadDashboardStats();
            }
        }
        public static class SecurityHelper
        {
            public static void RequireAdmin(Page page)
            {
                if (HttpContext.Current.Session["Username"] == null ||
                    HttpContext.Current.Session["Role"] == null ||
                    HttpContext.Current.Session["Role"].ToString() != "Admin")
                {
                    page.Response.Redirect("~/Webforms/Homepage.aspx");
                }
            }

            public static void RequireLogin(Page page)
            {
                if (HttpContext.Current.Session["Username"] == null)
                {
                    page.Response.Redirect("~/Webforms/Login.aspx");
                }
            }
        }

        private void LoadDashboardStats()
                {
            string connString = ConfigurationManager.ConnectionStrings["AOIDb"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                lblTotalUsers.Text = GetCount(conn, "Users").ToString();
                lblTotalProducts.Text = GetCount(conn, "Products").ToString();
                lblPendingOrders.Text = GetCount(conn, "Orders", "Status = 'Pending'").ToString();
            }
        }

        private int GetCount(SqlConnection conn, string tableName, string whereClause = null)
        {
            string query = $"SELECT COUNT(*) FROM {tableName}";

            if (!string.IsNullOrEmpty(whereClause))
                query += " WHERE " + whereClause;

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                return (int)cmd.ExecuteScalar();
            }
        }
    }
	
}
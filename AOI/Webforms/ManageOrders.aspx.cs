using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace AOI.Webforms
{
    public partial class ManageOrders : System.Web.UI.Page
    {
        private string ConnectionString => ConfigurationManager.ConnectionStrings["AOIDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("~/Webforms/NotFound.aspx");
                return;
            }

            if (!IsPostBack)
            {
                BindOrders();
            }
        }

        private void BindOrders()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query = @"
                    SELECT o.OrderID, u.Username, o.OrderDate, o.TotalAmount, o.Status, o.PromoCode
                    FROM Orders o
                    JOIN Users u ON o.UserID = u.UserId
                    ORDER BY o.OrderDate DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvOrders.DataSource = dt;
                gvOrders.DataBind();
            }
        }

        protected void gvOrders_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvOrders.EditIndex = e.NewEditIndex;
            BindOrders();
        }

        protected void gvOrders_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvOrders.EditIndex = -1;
            BindOrders();
        }

        protected void gvOrders_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int orderId = Convert.ToInt32(gvOrders.DataKeys[e.RowIndex].Value);
            var row = gvOrders.Rows[e.RowIndex];
            var ddlStatus = (DropDownList)row.FindControl("ddlEditStatus");

            if (ddlStatus == null) return;

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string sql = "UPDATE Orders SET Status = @Status WHERE OrderID = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
                cmd.Parameters.AddWithValue("@id", orderId);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            gvOrders.EditIndex = -1;
            BindOrders();
        }

        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewItems")
            {
                int orderId = Convert.ToInt32(e.CommandArgument);
                LoadOrderItems(orderId);
            }
        }

        private void LoadOrderItems(int orderId)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query = @"
                    SELECT p.ProductName, oi.Quantity, oi.Price, (oi.Quantity * oi.Price) AS LineTotal
                    FROM OrderItems oi
                    JOIN Products p ON oi.ProductID = p.ProductID
                    WHERE oi.OrderID = @OrderId";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@OrderId", orderId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvOrderItems.DataSource = dt;
                gvOrderItems.DataBind();
            }

            litOrderItemsTitle.Text = $"Items for Order #{orderId}";
            pnlOrderItems.Visible = true;
        }
    }
}

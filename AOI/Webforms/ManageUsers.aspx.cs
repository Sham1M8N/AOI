using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace AOI.Webforms
{
	public partial class ManageUsers : System.Web.UI.Page
	{
        protected void Page_Load(object sender, EventArgs e)
        {
            SecurityHelper.RequireAdmin(this);

            if (!IsPostBack)
            {
                LoadUsers();
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
                    // Not an admin — redirect away
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
        

        private void LoadUsers()
        {
            string connString = ConfigurationManager.ConnectionStrings["AOIDb"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT UserId, Username, Email, Role FROM Users";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvUsers.DataSource = dt;
                gvUsers.DataBind();
            }
        }
        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ResetPassword")
            {
                int userId = Convert.ToInt32(e.CommandArgument);
                string newPassword = GenerateRandomPassword(10);
                string hashedPassword = AOI.Models.PasswordHasher.Hash(newPassword);

                string connString = ConfigurationManager.ConnectionStrings["AOIDb"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "UPDATE Users SET PasswordHash = @PasswordHash WHERE UserId = @UserId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                // Show new password to admin
                string script = $"$('#newPasswordContent').text('New Password for User ID {userId}: {newPassword}'); $('#passwordModal').modal('show');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowPasswordModal", script, true);


                LoadUsers();
            }
        }
        private string GenerateRandomPassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            Random rnd = new Random();
            return new string(Enumerable.Repeat(validChars, length)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

        protected void gvUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            LoadUsers();
        }

        protected void gvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            LoadUsers();
        }

        protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvUsers.Rows[e.RowIndex];

            string username = ((TextBox)row.Cells[1].Controls[0]).Text;
            string email = ((TextBox)row.Cells[2].Controls[0]).Text;

            // Get selected role from dropdown
            DropDownList ddlRole = (DropDownList)row.FindControl("ddlRole");
            string role = ddlRole.SelectedValue;

            string connString = ConfigurationManager.ConnectionStrings["AOIDb"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "UPDATE Users SET Username = @Username, Email = @Email, Role = @Role WHERE UserId = @UserId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Role", role);
                cmd.Parameters.AddWithValue("@UserId", userId);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            gvUsers.EditIndex = -1;
            LoadUsers();
        }


        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);
            string connString = ConfigurationManager.ConnectionStrings["AOIDb"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "DELETE FROM Users WHERE UserId = @UserId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            LoadUsers();
        }

    }
}

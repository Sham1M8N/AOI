using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AOI.Webforms
{
	public partial class Login : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

            lblMessage.Visible = false;
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowMessage("Please enter both username and password.");
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["AOIDb"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT UserId, Username, PasswordHash, Role FROM Users WHERE Username = @Username";

                string storedHash = null;
                string actualUsername = null;
                string role = null;
                int userId = 0;

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userId = Convert.ToInt32(reader["UserId"]);
                            actualUsername = reader["Username"].ToString();
                            storedHash = reader["PasswordHash"].ToString();
                            role = reader["Role"].ToString();
                        }
                    }
                }

                if (storedHash == null || !AOI.Models.PasswordHasher.Verify(password, storedHash))
                {
                    ShowMessage("Invalid username or password.");
                    return;
                }

                if (AOI.Models.PasswordHasher.IsLegacyHash(storedHash))
                {
                    string upgradedHash = AOI.Models.PasswordHasher.Hash(password);
                    using (SqlCommand upgradeCmd = new SqlCommand("UPDATE Users SET PasswordHash = @Hash WHERE UserId = @UserId", conn))
                    {
                        upgradeCmd.Parameters.AddWithValue("@Hash", upgradedHash);
                        upgradeCmd.Parameters.AddWithValue("@UserId", userId);
                        upgradeCmd.ExecuteNonQuery();
                    }
                }

                Session["Username"] = actualUsername;
                Session["Role"] = role;

                if (role == "Admin")
                    Response.Redirect("AdminDashboard.aspx");
                else
                    Response.Redirect("Homepage.aspx");
            }
        }

        private void ShowMessage(string message)
        {
            lblMessage.Text = message;
            lblMessage.Visible = true;
        }
    }
}
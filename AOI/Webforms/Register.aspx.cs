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
	public partial class Register : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (password != confirmPassword)
            {
                ShowMessage("Passwords do not match!");
                return;
            }

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ShowMessage("Please fill all fields.");
                return;
            }

            string passwordHash = AOI.Models.PasswordHasher.Hash(password);

            string connString = ConfigurationManager.ConnectionStrings["AOIDb"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                // Check if username or email exists
                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username OR Email = @Email";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Username", username);
                    checkCmd.Parameters.AddWithValue("@Email", email);

                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        ShowMessage("Username or Email already exists.");
                        return;
                    }
                }

                // Insert user
                string insertQuery = "INSERT INTO Users (Username, Email, PasswordHash) VALUES (@Username, @Email, @PasswordHash)";
                using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@Username", username);
                    insertCmd.Parameters.AddWithValue("@Email", email);
                    insertCmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

                    int rows = insertCmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        Session["Username"] = username;
                        Session["Role"] = "Customer";
                        Response.Redirect("Homepage.aspx");

                    }
                    else
                    {
                        ShowMessage("Error occurred during registration. Please try again.");
                    }
                }
            }
        }

        private void ShowMessage(string message)
        {
            lblMessage.Visible = true;
            lblMessage.Text = message;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AOI
{
	public partial class Navbar : System.Web.UI.MasterPage
	{
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] != null)
                {
                    lblGreeting.Text = "Hello, " + Session["Username"].ToString();
                    btnLogout.Visible = true;
                    Button1.Visible = false;  // Hide Login button

                    if (Session["Role"] != null && Session["Role"].ToString() == "Admin")
                    {
                        adminLink.Visible = true;
                    }
                    else
                    {
                        adminLink.Visible = false;
                    }
                }
                else
                {
                    lblGreeting.Text = "Hello, Guest!";
                    btnLogout.Visible = false;
                    Button1.Visible = true;   // Show Login button
                    adminLink.Visible = false;
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            cartItemCount.InnerText = AOI.Models.Cart.GetItemCount(Session).ToString();
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Webforms/Login.aspx");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear all session data
            Session.Clear();
            Session.Abandon();

            // Optionally clear authentication cookies if used
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                var cookie = new HttpCookie("ASP.NET_SessionId")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(cookie);
            }

            // Show alert and redirect
            string script = "alert('You have been successfully logged out.'); window.location='/Webforms/Homepage.aspx';";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "LogoutAlert", script, true);
        }

    }
}
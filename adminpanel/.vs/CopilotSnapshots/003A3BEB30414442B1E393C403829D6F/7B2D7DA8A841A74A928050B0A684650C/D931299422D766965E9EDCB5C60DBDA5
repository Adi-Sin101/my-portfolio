using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace adminpanel
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is authenticated, if not hide logout button
            if (Session["AdminUser"] == null)
            {
                // Hide logout button if user is not logged in
                lnkLogout.Visible = false;
            }
            else
            {
                // Show logout button if user is logged in
                lnkLogout.Visible = true;
            }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            // Clear session
            Session.Clear();
            Session.Abandon();
            
            // Redirect to login page
            Response.Redirect("Login.aspx");
        }
    }
}
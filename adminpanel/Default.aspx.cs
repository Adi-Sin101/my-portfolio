using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace adminpanel
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is authenticated
            if (Session["AdminUser"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadDashboardStats();
            }
        }

        private void LoadDashboardStats()
        {
            string cs = ConfigurationManager.ConnectionStrings["AdminPanelDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();

                    // Get project count
                    SqlCommand cmdProjects = new SqlCommand("SELECT COUNT(*) FROM Projects", con);
                    object projectCountResult = cmdProjects.ExecuteScalar();
                    lblProjectCount.Text = projectCountResult != null ? projectCountResult.ToString() : "0";

                    // Get skills count (assuming you have a Skills table)
                    try
                    {
                        SqlCommand cmdSkills = new SqlCommand("SELECT COUNT(*) FROM Skills", con);
                        object skillCountResult = cmdSkills.ExecuteScalar();
                        lblSkillCount.Text = skillCountResult != null ? skillCountResult.ToString() : "0";
                    }
                    catch
                    {
                        lblSkillCount.Text = "N/A";
                    }

                    // Get experience count (assuming you have an Experience table)
                    try
                    {
                        SqlCommand cmdExperience = new SqlCommand("SELECT COUNT(*) FROM Experience", con);
                        object expCountResult = cmdExperience.ExecuteScalar();
                        lblExperienceCount.Text = expCountResult != null ? expCountResult.ToString() : "0";
                    }
                    catch
                    {
                        lblExperienceCount.Text = "N/A";
                    }

                    // Get last update date from Projects table
                    try
                    {
                        SqlCommand cmdLastUpdate = new SqlCommand("SELECT MAX(CreatedDate) FROM Projects", con);
                        object lastUpdateResult = cmdLastUpdate.ExecuteScalar();
                        if (lastUpdateResult != null && lastUpdateResult != DBNull.Value)
                        {
                            DateTime lastUpdate = (DateTime)lastUpdateResult;
                            lblLastUpdate.Text = lastUpdate.ToString("MMM dd, yyyy");
                        }
                        else
                        {
                            lblLastUpdate.Text = "Never";
                        }
                    }
                    catch
                    {
                        lblLastUpdate.Text = "Unknown";
                    }
                }
                catch (Exception ex)
                {
                    // Handle database connection errors gracefully
                    lblProjectCount.Text = "Error";
                    lblSkillCount.Text = "Error";
                    lblExperienceCount.Text = "Error";
                    lblLastUpdate.Text = "Error";
                    
                    // You might want to log this error
                    Response.Write("<script>console.log('Dashboard stats error: " + ex.Message + "');</script>");
                }
            }
        }
    }
}
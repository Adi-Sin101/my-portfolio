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
                    System.Diagnostics.Debug.WriteLine($"Database error in LoadDashboardStats: {ex.Message}");
                }
            }
            
            // Load visitor tracking information from cookies
            LoadVisitorTrackingInfo();
        }

        #region Visitor Tracking Cookie Display

        private void LoadVisitorTrackingInfo()
        {
            try
            {
                // Read visitor tracking cookie (no Skills page tracking)
                HttpCookie visitorCookie = Request.Cookies["AdminVisitorInfo"];
                
                if (visitorCookie != null)
                {
                    // Display visitor information
                    pnlVisitorInfo.Visible = true;
                    pnlNoVisitorInfo.Visible = false;
                    
                    // Visitor Name
                    string visitorName = HttpUtility.UrlDecode(visitorCookie.Values["Username"] ?? "Admin User");
                    lblVisitorName.Text = visitorName;
                    
                    // Visit Count
                    string visitCountStr = visitorCookie.Values["VisitCount"] ?? "1";
                    lblVisitCount.Text = visitCountStr;
                    
                    // First Visit Date
                    string firstVisitStr = visitorCookie.Values["FirstVisit"];
                    if (DateTime.TryParse(firstVisitStr, out DateTime firstVisit))
                    {
                        lblFirstVisit.Text = firstVisit.ToString("MMM dd, yyyy");
                    }
                    else
                    {
                        lblFirstVisit.Text = "Today";
                    }
                    
                    // Last Visit Date
                    string lastVisitStr = visitorCookie.Values["LastVisit"];
                    if (DateTime.TryParse(lastVisitStr, out DateTime lastVisit))
                    {
                        TimeSpan timeDiff = DateTime.Now.Subtract(lastVisit);
                        if (timeDiff.TotalMinutes < 1)
                        {
                            lblLastVisit.Text = "Just now";
                        }
                        else if (timeDiff.TotalHours < 1)
                        {
                            lblLastVisit.Text = $"{(int)timeDiff.TotalMinutes} min ago";
                        }
                        else if (timeDiff.TotalDays < 1)
                        {
                            lblLastVisit.Text = lastVisit.ToString("HH:mm");
                        }
                        else
                        {
                            lblLastVisit.Text = lastVisit.ToString("MMM dd");
                        }
                    }
                    else
                    {
                        lblLastVisit.Text = "Now";
                    }
                    
                    // Generate welcome message
                    GenerateWelcomeMessage(visitorName, int.Parse(visitCountStr), firstVisit);
                }
                else
                {
                    // No visitor tracking data available
                    pnlVisitorInfo.Visible = false;
                    pnlNoVisitorInfo.Visible = true;
                }
            }
            catch (Exception ex)
            {
                // Error reading cookies - show no visitor info panel
                pnlVisitorInfo.Visible = false;
                pnlNoVisitorInfo.Visible = true;
                System.Diagnostics.Debug.WriteLine($"Error loading visitor tracking info: {ex.Message}");
            }
        }

        private void GenerateWelcomeMessage(string visitorName, int visitCount, DateTime firstVisit)
        {
            string message = "";
            string messageClass = "alert alert-info";
            
            if (visitCount == 1)
            {
                message = $"🎉 Welcome to the admin panel, {visitorName}! This is your first visit.";
                messageClass = "alert alert-success";
            }
            else if (visitCount <= 5)
            {
                int daysSinceFirst = (DateTime.Now - firstVisit).Days;
                message = $"👋 Welcome back, {visitorName}! Visit #{visitCount}";
                if (daysSinceFirst > 0)
                {
                    message += $" (member for {daysSinceFirst} day{(daysSinceFirst == 1 ? "" : "s")})";
                }
                messageClass = "alert alert-primary";
            }
            else if (visitCount <= 20)
            {
                message = $"⭐ Hello {visitorName}! You're a regular user with {visitCount} visits.";
                messageClass = "alert alert-info";
            }
            else
            {
                message = $"🏆 Welcome back, {visitorName}! You're a power user with {visitCount} visits!";
                messageClass = "alert alert-warning";
            }
            
            lblWelcomeMessage.Text = $"<div class='{messageClass}' style='margin-top: 15px; padding: 12px; border-radius: 8px;'>{message}</div>";
        }

        #endregion
    }
}
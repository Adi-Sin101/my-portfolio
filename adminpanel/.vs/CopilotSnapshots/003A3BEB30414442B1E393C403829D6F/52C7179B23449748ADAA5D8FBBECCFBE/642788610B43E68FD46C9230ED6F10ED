using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

namespace adminpanel
{
    public partial class Contact : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["AdminPanelDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is authenticated
            if (Session["AdminUser"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadContactInfo();
            }
        }

        private void LoadContactInfo()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    string query = "SELECT TOP 1 * FROM PersonalInfo WHERE IsActive = 1 ORDER BY CreatedDate DESC";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    if (reader.Read())
                    {
                        StringBuilder html = new StringBuilder();
                        
                        // Current contact info display
                        html.Append("<div class='row'>");
                        html.Append("<div class='col-md-6'>");
                        
                        html.Append("<div class='contact-info-item'>");
                        html.Append("<div class='contact-info-label'>Email Address</div>");
                        html.AppendFormat("<div>{0}</div>", reader["Email"]);
                        html.Append("</div>");
                        
                        html.Append("<div class='contact-info-item'>");
                        html.Append("<div class='contact-info-label'>Phone Number</div>");
                        html.AppendFormat("<div>{0}</div>", reader["Phone"]);
                        html.Append("</div>");
                        
                        html.Append("</div>");
                        html.Append("<div class='col-md-6'>");
                        
                        html.Append("<div class='contact-info-item'>");
                        html.Append("<div class='contact-info-label'>GitHub Profile</div>");
                        html.AppendFormat("<div><a href='{0}' target='_blank'>{1}</a></div>", reader["GitHubUrl"], reader["GitHubText"]);
                        html.Append("</div>");
                        
                        html.Append("<div class='contact-info-item'>");
                        html.Append("<div class='contact-info-label'>LinkedIn Profile</div>");
                        html.AppendFormat("<div><a href='{0}' target='_blank'>{1}</a></div>", reader["LinkedInUrl"], reader["LinkedInText"]);
                        html.Append("</div>");
                        
                        html.Append("</div>");
                        html.Append("</div>");
                        
                        ltlContactInfo.Text = html.ToString();
                        
                        // Portfolio preview
                        StringBuilder previewHtml = new StringBuilder();
                        previewHtml.Append("<div style='text-align: center; margin-bottom: 20px;'>");
                        previewHtml.Append("<h3 style='margin-bottom: 10px;'>Contact Me</h3>");
                        previewHtml.Append("<div style='width: 60px; height: 4px; background: #2563eb; margin: 0 auto 20px;'></div>");
                        previewHtml.Append("</div>");
                        
                        previewHtml.Append("<div class='row'>");
                        
                        // Email box
                        previewHtml.Append("<div class='col-md-6 mb-3'>");
                        previewHtml.Append("<div class='contact-box-preview'>");
                        previewHtml.Append("<div class='contact-icon-preview'>📧</div>");
                        previewHtml.Append("<strong>Email:</strong><br>");
                        previewHtml.AppendFormat("<a href='mailto:{0}'>{0}</a>", reader["Email"]);
                        previewHtml.Append("</div>");
                        previewHtml.Append("</div>");
                        
                        // Phone box
                        previewHtml.Append("<div class='col-md-6 mb-3'>");
                        previewHtml.Append("<div class='contact-box-preview'>");
                        previewHtml.Append("<div class='contact-icon-preview'>📞</div>");
                        previewHtml.Append("<strong>Phone:</strong><br>");
                        previewHtml.AppendFormat("<a href='tel:{0}'>{0}</a>", reader["Phone"]);
                        previewHtml.Append("</div>");
                        previewHtml.Append("</div>");
                        
                        // LinkedIn box
                        previewHtml.Append("<div class='col-md-6 mb-3'>");
                        previewHtml.Append("<div class='contact-box-preview'>");
                        previewHtml.Append("<div class='contact-icon-preview'>💼</div>");
                        previewHtml.Append("<strong>LinkedIn:</strong><br>");
                        previewHtml.AppendFormat("<a href='{0}' target='_blank'>{1}</a>", reader["LinkedInUrl"], reader["LinkedInText"]);
                        previewHtml.Append("</div>");
                        previewHtml.Append("</div>");
                        
                        // GitHub box
                        previewHtml.Append("<div class='col-md-6 mb-3'>");
                        previewHtml.Append("<div class='contact-box-preview'>");
                        previewHtml.Append("<div class='contact-icon-preview'>💻</div>");
                        previewHtml.Append("<strong>GitHub:</strong><br>");
                        previewHtml.AppendFormat("<a href='{0}' target='_blank'>{1}</a>", reader["GitHubUrl"], reader["GitHubText"]);
                        previewHtml.Append("</div>");
                        previewHtml.Append("</div>");
                        
                        previewHtml.Append("</div>");
                        
                        ltlContactPreview.Text = previewHtml.ToString();
                    }
                    else
                    {
                        ltlContactInfo.Text = "<div class='alert alert-warning'><strong>No contact information found.</strong><br/>Please go to the About section to add your personal information.</div>";
                        ltlContactPreview.Text = "<p class='text-center text-muted'>No contact information available for preview.</p>";
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    ltlContactInfo.Text = "<div class='alert alert-danger'>Error loading contact information: " + ex.Message + "</div>";
                    ltlContactPreview.Text = "<p class='text-center text-danger'>Error loading preview.</p>";
                }
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadContactInfo();
            ShowMessage("Contact information refreshed successfully!", "success");
        }

        private void ShowMessage(string message, string type)
        {
            string alertClass = type == "success" ? "alert-success" : "alert-danger";
            string script = $@"
                var alertDiv = document.createElement('div');
                alertDiv.className = 'alert {alertClass} alert-dismissible fade show';
                alertDiv.innerHTML = '{message}<button type=""button"" class=""btn-close"" data-bs-dismiss=""alert""></button>';
                document.querySelector('.container').insertBefore(alertDiv, document.querySelector('.container').firstChild);
                setTimeout(function() {{
                    if (alertDiv.parentNode) {{
                        alertDiv.parentNode.removeChild(alertDiv);
                    }}
                }}, 5000);
            ";
            ClientScript.RegisterStartupScript(this.GetType(), "ShowMessage", script, true);
        }
    }
}
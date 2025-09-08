using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Text;

namespace adminpanel
{
    public partial class ContactInfo : System.Web.UI.Page
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
                EnsureContactInfoTable();
                LoadContactInfo();
                LoadContactInfoPreview();
            }
        }

        #region ContactInfo CRUD Operations

        private void EnsureContactInfoTable()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    
                    // Create ContactInfo table if it doesn't exist
                    string createTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ContactInfo' AND xtype='U')
                        BEGIN
                            CREATE TABLE ContactInfo (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                Email VARCHAR(255),
                                LinkedIn VARCHAR(255),
                                GitHub VARCHAR(255),
                                Phone VARCHAR(50),
                                icon_class VARCHAR(100) DEFAULT 'fas fa-envelope',
                                display_order INT DEFAULT 0
                            )
                            
                            -- Insert default contact info
                            INSERT INTO ContactInfo (Email, LinkedIn, GitHub, Phone, icon_class, display_order)
                            VALUES ('your.email@example.com', 'https://linkedin.com/in/your-profile', 'https://github.com/your-username', '+1 (555) 123-4567', 'fas fa-envelope', 1)
                        END";
                    
                    SqlCommand createCmd = new SqlCommand(createTableQuery, con);
                    createCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error creating ContactInfo table: {ex.Message}");
                }
            }
        }

        private void LoadContactInfo()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    
                    string query = @"SELECT Id, Email, LinkedIn, GitHub, Phone, icon_class, display_order
                                   FROM ContactInfo 
                                   ORDER BY display_order, Id";
                    
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    
                    gvContactInfo.DataSource = dt;
                    gvContactInfo.DataBind();
                    
                    // Update contact info count
                    lblContactInfoCount.Text = $"Total Contact Records: {dt.Rows.Count}";
                    lblContactInfoCount.CssClass = dt.Rows.Count > 0 ? "contact-info-card-label contact-info-stats" : "contact-info-card-label contact-info-stats empty";
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading contact information: " + ex.Message, "error");
                    lblContactInfoCount.Text = "Error loading contact info";
                    lblContactInfoCount.CssClass = "contact-info-card-label contact-info-stats error";
                }
            }
        }

        protected void btnRefreshContactInfo_Click(object sender, EventArgs e)
        {
            LoadContactInfo();
            LoadContactInfoPreview();
            ShowMessage("Contact information refreshed successfully!", "success");
        }

        protected void btnAddNewContactInfo_Click(object sender, EventArgs e)
        {
            ClearModalForm();
            ltlModalTitle.Text = "Add New Contact Information";
            pnlContactInfoForm.Visible = true;
            pnlContactInfoView.Visible = false;
            btnSaveContactInfo.Visible = true;
            btnSaveContactInfo.Text = "Add Contact Info";
            ViewState["EditingContactInfoId"] = null;
            
            ShowModal();
        }

        protected void gvContactInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvContactInfo.PageIndex = e.NewPageIndex;
            LoadContactInfo();
        }

        protected void gvContactInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewContactInfo")
            {
                int contactInfoId = Convert.ToInt32(e.CommandArgument);
                ViewContactInfoDetails(contactInfoId);
            }
        }

        protected void gvContactInfo_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvContactInfo.EditIndex = e.NewEditIndex;
            LoadContactInfo();
        }

        protected void gvContactInfo_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int contactInfoId = Convert.ToInt32(gvContactInfo.DataKeys[e.RowIndex].Value);
                GridViewRow row = gvContactInfo.Rows[e.RowIndex];
                
                string email = ((TextBox)row.FindControl("txtEmail")).Text.Trim();
                string linkedin = ((TextBox)row.FindControl("txtLinkedIn")).Text.Trim();
                string github = ((TextBox)row.FindControl("txtGitHub")).Text.Trim();
                string phone = ((TextBox)row.FindControl("txtPhone")).Text.Trim();
                string iconClass = ((TextBox)row.FindControl("txtIconClass")).Text.Trim();
                string displayOrderStr = ((TextBox)row.FindControl("txtDisplayOrder")).Text.Trim();

                int displayOrder = string.IsNullOrEmpty(displayOrderStr) ? 0 : Convert.ToInt32(displayOrderStr);

                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    string query = @"UPDATE ContactInfo 
                                   SET Email = @email, LinkedIn = @linkedin, GitHub = @github, 
                                       Phone = @phone, icon_class = @iconClass, display_order = @displayOrder 
                                   WHERE Id = @id";
                    
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@linkedin", linkedin);
                    cmd.Parameters.AddWithValue("@github", github);
                    cmd.Parameters.AddWithValue("@phone", phone);
                    cmd.Parameters.AddWithValue("@iconClass", iconClass);
                    cmd.Parameters.AddWithValue("@displayOrder", displayOrder);
                    cmd.Parameters.AddWithValue("@id", contactInfoId);
                    
                    cmd.ExecuteNonQuery();
                }

                gvContactInfo.EditIndex = -1;
                LoadContactInfo();
                LoadContactInfoPreview();
                ShowMessage("Contact information updated successfully!", "success");
            }
            catch (Exception ex)
            {
                ShowMessage("Error updating contact information: " + ex.Message, "error");
            }
        }

        protected void gvContactInfo_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvContactInfo.EditIndex = -1;
            LoadContactInfo();
        }

        protected void gvContactInfo_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int contactInfoId = Convert.ToInt32(gvContactInfo.DataKeys[e.RowIndex].Value);
                
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    string query = "DELETE FROM ContactInfo WHERE Id = @id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", contactInfoId);
                    
                    cmd.ExecuteNonQuery();
                }

                LoadContactInfo();
                LoadContactInfoPreview();
                ShowMessage("Contact information deleted successfully!", "success");
            }
            catch (Exception ex)
            {
                ShowMessage("Error deleting contact information: " + ex.Message, "error");
            }
        }

        private void ViewContactInfoDetails(int contactInfoId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    string query = @"SELECT Id, Email, LinkedIn, GitHub, Phone, icon_class, display_order
                                   FROM ContactInfo WHERE Id = @id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", contactInfoId);
                    
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    if (reader.Read())
                    {
                        StringBuilder html = new StringBuilder();
                        
                        // Header with contact info
                        html.Append("<div class='contact-info-details-header'>");
                        html.Append($"<div class='contact-info-icon-large'><i class='{reader["icon_class"]}' style='font-size: 48px; color: #a78bfa;'></i></div>");
                        html.Append($"<div class='contact-info-info'>");
                        html.Append($"<h4>Contact Information #{reader["Id"]}</h4>");
                        html.Append($"<div class='contact-info-type'>{GetContactType(reader["icon_class"].ToString())}</div>");
                        html.Append("</div>");
                        html.Append("</div>");
                        
                        // Contact details
                        html.Append("<div class='contact-info-details-content'>");
                        
                        if (!string.IsNullOrEmpty(reader["Email"].ToString()))
                        {
                            html.Append("<div class='detail-item'>");
                            html.Append("<strong>Email:</strong>");
                            html.Append($"<p><a href='mailto:{reader["Email"]}' style='color: #a78bfa;'>{reader["Email"]}</a></p>");
                            html.Append("</div>");
                        }
                        
                        if (!string.IsNullOrEmpty(reader["LinkedIn"].ToString()))
                        {
                            html.Append("<div class='detail-item'>");
                            html.Append("<strong>LinkedIn:</strong>");
                            html.Append($"<p><a href='{reader["LinkedIn"]}' target='_blank' style='color: #a78bfa;'>{reader["LinkedIn"]}</a></p>");
                            html.Append("</div>");
                        }
                        
                        if (!string.IsNullOrEmpty(reader["GitHub"].ToString()))
                        {
                            html.Append("<div class='detail-item'>");
                            html.Append("<strong>GitHub:</strong>");
                            html.Append($"<p><a href='{reader["GitHub"]}' target='_blank' style='color: #a78bfa;'>{reader["GitHub"]}</a></p>");
                            html.Append("</div>");
                        }
                        
                        if (!string.IsNullOrEmpty(reader["Phone"].ToString()))
                        {
                            html.Append("<div class='detail-item'>");
                            html.Append("<strong>Phone:</strong>");
                            html.Append($"<p><a href='tel:{reader["Phone"]}' style='color: #a78bfa;'>{reader["Phone"]}</a></p>");
                            html.Append("</div>");
                        }
                        
                        html.Append($"<div class='detail-item'><strong>Icon Class:</strong> <code style='background: rgba(60,16,80,0.6); padding: 4px 8px; border-radius: 4px;'>{reader["icon_class"]}</code></div>");
                        html.Append($"<div class='detail-item'><strong>Display Order:</strong> {reader["display_order"]}</div>");
                        
                        html.Append("</div>");
                        
                        ltlContactInfoDetails.Text = html.ToString();
                        
                        // Set up modal for viewing
                        ltlModalTitle.Text = $"Contact Information Details";
                        pnlContactInfoForm.Visible = false;
                        pnlContactInfoView.Visible = true;
                        btnSaveContactInfo.Visible = false;
                        
                        ShowModal();
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading contact information details: " + ex.Message, "error");
                }
            }
        }

        protected void btnSaveContactInfo_Click(object sender, EventArgs e)
        {
            try
            {
                string email = txtModalEmail.Text.Trim();
                string linkedin = txtModalLinkedIn.Text.Trim();
                string github = txtModalGitHub.Text.Trim();
                string phone = txtModalPhone.Text.Trim();
                string iconClass = ddlModalIconClass.SelectedValue;
                string displayOrderStr = txtModalDisplayOrder.Text.Trim();

                int displayOrder = string.IsNullOrEmpty(displayOrderStr) ? 0 : Convert.ToInt32(displayOrderStr);

                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    
                    if (ViewState["EditingContactInfoId"] == null)
                    {
                        // Add new contact info
                        string query = @"INSERT INTO ContactInfo (Email, LinkedIn, GitHub, Phone, icon_class, display_order) 
                                       VALUES (@email, @linkedin, @github, @phone, @iconClass, @displayOrder)";
                        
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@linkedin", linkedin);
                        cmd.Parameters.AddWithValue("@github", github);
                        cmd.Parameters.AddWithValue("@phone", phone);
                        cmd.Parameters.AddWithValue("@iconClass", iconClass);
                        cmd.Parameters.AddWithValue("@displayOrder", displayOrder);
                        
                        cmd.ExecuteNonQuery();
                        ShowMessage("Contact information added successfully!", "success");
                    }
                    else
                    {
                        // Update existing contact info
                        int contactInfoId = Convert.ToInt32(ViewState["EditingContactInfoId"]);
                        string query = @"UPDATE ContactInfo 
                                       SET Email = @email, LinkedIn = @linkedin, GitHub = @github, 
                                           Phone = @phone, icon_class = @iconClass, display_order = @displayOrder 
                                       WHERE Id = @id";
                        
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@linkedin", linkedin);
                        cmd.Parameters.AddWithValue("@github", github);
                        cmd.Parameters.AddWithValue("@phone", phone);
                        cmd.Parameters.AddWithValue("@iconClass", iconClass);
                        cmd.Parameters.AddWithValue("@displayOrder", displayOrder);
                        cmd.Parameters.AddWithValue("@id", contactInfoId);
                        
                        cmd.ExecuteNonQuery();
                        ShowMessage("Contact information updated successfully!", "success");
                    }
                }

                LoadContactInfo();
                LoadContactInfoPreview();
                HideModal();
            }
            catch (Exception ex)
            {
                ShowMessage("Error saving contact information: " + ex.Message, "error");
            }
        }

        #endregion

        #region Helper Methods

        protected string GetContactType(string iconClass)
        {
            if (string.IsNullOrEmpty(iconClass)) return "Unknown";
            
            if (iconClass.Contains("envelope")) return "Email";
            if (iconClass.Contains("phone")) return "Phone";
            if (iconClass.Contains("linkedin")) return "LinkedIn";
            if (iconClass.Contains("github")) return "GitHub";
            if (iconClass.Contains("twitter")) return "Twitter";
            if (iconClass.Contains("instagram")) return "Instagram";
            if (iconClass.Contains("globe")) return "Website";
            if (iconClass.Contains("map")) return "Location";
            
            return "Contact";
        }

        private void LoadContactInfoPreview()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    string query = @"SELECT Email, LinkedIn, GitHub, Phone, icon_class, display_order
                                   FROM ContactInfo 
                                   ORDER BY display_order, Id";
                    
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    StringBuilder html = new StringBuilder();
                    html.Append("<div class='contact-info-preview-grid'>");
                    
                    while (reader.Read())
                    {
                        string iconClass = reader["icon_class"].ToString();
                        string contactType = GetContactType(iconClass);
                        string contactValue = "";
                        
                        // Determine the primary contact value to display
                        if (!string.IsNullOrEmpty(reader["Email"].ToString()))
                            contactValue = reader["Email"].ToString();
                        else if (!string.IsNullOrEmpty(reader["Phone"].ToString()))
                            contactValue = reader["Phone"].ToString();
                        else if (!string.IsNullOrEmpty(reader["LinkedIn"].ToString()))
                            contactValue = "LinkedIn Profile";
                        else if (!string.IsNullOrEmpty(reader["GitHub"].ToString()))
                            contactValue = "GitHub Profile";
                        else
                            contactValue = "Contact Information";
                        
                        html.Append("<div class='contact-info-preview-item'>");
                        html.Append($"<div class='contact-info-preview-icon'><i class='{iconClass}'></i></div>");
                        html.Append($"<div class='contact-info-preview-type'>{contactType}</div>");
                        html.Append($"<div class='contact-info-preview-value'>{contactValue}</div>");
                        html.Append("</div>");
                    }
                    
                    html.Append("</div>");
                    
                    if (!reader.HasRows)
                    {
                        html.Clear();
                        html.Append("<p class='text-center text-muted'>No contact information to preview. Add some contact info first!</p>");
                    }
                    
                    ltlContactInfoPreview.Text = html.ToString();
                    reader.Close();
                }
                catch (Exception ex)
                {
                    ltlContactInfoPreview.Text = "<p class='text-center text-danger'>Error loading preview.</p>";
                }
            }
        }

        public string GetPrimaryEmail()
        {
            // Get the first email from ContactInfo for use in contact form
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    string query = @"SELECT TOP 1 Email FROM ContactInfo 
                                   WHERE Email IS NOT NULL AND Email != '' 
                                   ORDER BY display_order, Id";
                    
                    SqlCommand cmd = new SqlCommand(query, con);
                    object result = cmd.ExecuteScalar();
                    
                    return result?.ToString() ?? "your.email@example.com";
                }
                catch
                {
                    return "your.email@example.com";
                }
            }
        }

        private void ClearModalForm()
        {
            txtModalEmail.Text = "";
            txtModalLinkedIn.Text = "";
            txtModalGitHub.Text = "";
            txtModalPhone.Text = "";
            txtModalDisplayOrder.Text = "";
            ddlModalIconClass.SelectedIndex = 0;
        }

        private void ShowModal()
        {
            string script = @"
                document.getElementById('contactInfoModal').style.display = 'flex';
            ";
            ClientScript.RegisterStartupScript(this.GetType(), "ShowModal", script, true);
        }

        private void HideModal()
        {
            string script = @"
                document.getElementById('contactInfoModal').style.display = 'none';
            ";
            ClientScript.RegisterStartupScript(this.GetType(), "HideModal", script, true);
        }

        private void ShowMessage(string message, string type)
        {
            string alertClass = type == "success" ? "alert-success" : 
                               type == "error" ? "alert-danger" : 
                               type == "info" ? "alert-info" : "alert-warning";
            string bgColor = type == "success" ? "rgba(34, 197, 94, 0.1)" : 
                            type == "error" ? "rgba(239, 68, 68, 0.1)" : 
                            type == "info" ? "rgba(59, 130, 246, 0.1)" : "rgba(245, 158, 11, 0.1)";
            string borderColor = type == "success" ? "#22c55e" : 
                                type == "error" ? "#ef4444" : 
                                type == "info" ? "#3b82f6" : "#f59e0b";
            string textColor = type == "success" ? "#4ade80" : 
                              type == "error" ? "#f87171" : 
                              type == "info" ? "#60a5fa" : "#fbbf24";
            
            string script = $@"
                document.addEventListener('DOMContentLoaded', function() {{
                    var alertDiv = document.createElement('div');
                    alertDiv.className = 'portfolio-alert {alertClass}';
                    alertDiv.style.cssText = 'position: fixed; top: 20px; right: 20px; z-index: 10000; padding: 16px 20px; border-radius: 12px; max-width: 400px; box-shadow: 0 8px 32px rgba(0,0,0,0.3); background: {bgColor}; color: {textColor}; border: 1px solid {borderColor}; font-family: Poppins, sans-serif; font-weight: 500; backdrop-filter: blur(10px);';
                    alertDiv.innerHTML = '{message.Replace("'", "\\'")}';
                    document.body.appendChild(alertDiv);
                    
                    // Fade in
                    alertDiv.style.opacity = '0';
                    alertDiv.style.transform = 'translateX(100%)';
                    alertDiv.style.transition = 'opacity 0.4s ease, transform 0.4s ease';
                    
                    setTimeout(function() {{
                        alertDiv.style.opacity = '1';
                        alertDiv.style.transform = 'translateX(0)';
                    }}, 100);
                    
                    // Fade out after 4 seconds
                    setTimeout(function() {{
                        alertDiv.style.opacity = '0';
                        alertDiv.style.transform = 'translateX(100%)';
                        setTimeout(function() {{
                            if (document.body.contains(alertDiv)) {{
                                document.body.removeChild(alertDiv);
                            }}
                        }}, 400);
                    }}, 4000);
                }});
            ";
            
            ClientScript.RegisterStartupScript(this.GetType(), "ShowMessage", script, true);
        }

        #endregion
    }
}
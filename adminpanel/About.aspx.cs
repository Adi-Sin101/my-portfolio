using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;

namespace adminpanel
{
    public partial class About : System.Web.UI.Page
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
                LoadPersonalInfo();
                LoadCurrentInfoPreview();
            }
        }

        private void LoadPersonalInfo()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    string query = "SELECT TOP 1 * FROM HomeContent ORDER BY Id DESC";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    if (reader.Read())
                    {
                        hdnPersonalInfoId.Value = reader["Id"].ToString();
                        txtFullName.Text = reader["HeroHeading"].ToString();
                        txtRole.Text = reader["RoleText"].ToString();
                        txtEmail.Text = reader["Email"].ToString();
                        txtPhone.Text = reader["Phone"].ToString();
                        txtHeroDescription.Text = reader["Description"].ToString();
                        txtAboutDescription.Text = reader["Description"].ToString(); // Using same description for both
                        txtGitHubUrl.Text = reader["GitHubUrl"].ToString();
                        txtGitHubText.Text = "GitHub Profile"; // Default text since not in table
                        txtLinkedInUrl.Text = reader["LinkedInUrl"].ToString();
                        txtLinkedInText.Text = "LinkedIn Profile"; // Default text since not in table
                        
                        // Handle Resume column
                        if (reader["Resume"] != DBNull.Value)
                        {
                            txtResumeUrl.Text = reader["Resume"].ToString();
                        }
                        
                        string profileImagePath = reader["ImagePath"].ToString();
                        hdnCurrentProfileImagePath.Value = profileImagePath;
                        
                        if (!string.IsNullOrEmpty(profileImagePath))
                        {
                            imgCurrentProfile.ImageUrl = profileImagePath;
                            currentProfileDiv.Visible = true;
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading personal information: " + ex.Message, "danger");
                }
            }
        }

        private void LoadCurrentInfoPreview()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    string query = "SELECT TOP 1 * FROM HomeContent ORDER BY Id DESC";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    if (reader.Read())
                    {
                        StringBuilder html = new StringBuilder();
                        html.Append("<div class='row'>");
                        html.Append("<div class='col-md-6'>");
                        html.AppendFormat("<div class='info-item'><span class='info-label'>Name:</span> {0}</div>", reader["HeroHeading"]);
                        html.AppendFormat("<div class='info-item'><span class='info-label'>Role:</span> {0}</div>", reader["RoleText"]);
                        html.AppendFormat("<div class='info-item'><span class='info-label'>Email:</span> {0}</div>", reader["Email"]);
                        html.AppendFormat("<div class='info-item'><span class='info-label'>Phone:</span> {0}</div>", reader["Phone"]);
                        html.AppendFormat("<div class='info-item'><span class='info-label'>GitHub:</span> <a href='{0}' target='_blank'>{0}</a></div>", reader["GitHubUrl"]);
                        html.AppendFormat("<div class='info-item'><span class='info-label'>LinkedIn:</span> <a href='{0}' target='_blank'>{0}</a></div>", reader["LinkedInUrl"]);
                        
                        // Add Resume info
                        string resumeUrl = reader["Resume"].ToString();
                        if (!string.IsNullOrEmpty(resumeUrl))
                        {
                            html.AppendFormat("<div class='info-item'><span class='info-label'>Resume:</span> <a href='{0}' target='_blank'>Download Resume</a></div>", resumeUrl);
                        }
                        
                        html.Append("</div>");
                        html.Append("<div class='col-md-6'>");
                        html.AppendFormat("<div class='info-item'><span class='info-label'>Description:</span><br/>{0}</div>", reader["Description"]);
                        string imagePath = reader["ImagePath"].ToString();
                        if (!string.IsNullOrEmpty(imagePath))
                        {
                            html.AppendFormat("<div class='info-item'><span class='info-label'>Profile Image:</span><br/><img src='{0}' style='max-width: 150px; max-height: 100px; object-fit: cover;' /></div>", imagePath);
                        }
                        html.Append("</div>");
                        html.Append("</div>");
                        
                        ltlCurrentInfo.Text = html.ToString();
                    }
                    else
                    {
                        ltlCurrentInfo.Text = "<p class='text-muted'>No personal information found. Please fill out the form above.</p>";
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    ltlCurrentInfo.Text = "<p class='text-danger'>Error loading preview: " + ex.Message + "</p>";
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string heroHeading = txtFullName.Text.Trim();
                string roleText = txtRole.Text.Trim();
                string email = txtEmail.Text.Trim();
                string phone = txtPhone.Text.Trim();
                string description = txtHeroDescription.Text.Trim();
                string gitHubUrl = txtGitHubUrl.Text.Trim();
                string linkedInUrl = txtLinkedInUrl.Text.Trim();
                string resumeUrl = txtResumeUrl.Text.Trim();
                string profileImagePath = hdnCurrentProfileImagePath.Value;

                // Debug info
                string debugInfo = $"Debug Info:\\nID: {hdnPersonalInfoId.Value}\\nHero Heading: {heroHeading}\\nRole: {roleText}\\nEmail: {email}";

                // Handle profile image upload
                if (FileUploadProfile.HasFile)
                {
                    string newImagePath = HandleImageUpload();
                    if (newImagePath != null)
                    {
                        // Delete old image
                        if (!string.IsNullOrEmpty(profileImagePath))
                        {
                            DeleteImageFile(profileImagePath);
                        }
                        profileImagePath = newImagePath;
                    }
                    else
                    {
                        return; // Error occurred during upload
                    }
                }

                using (SqlConnection con = new SqlConnection(cs))
                {
                    try
                    {
                        con.Open();
                        
                        // Check if we want to update existing record or always insert new
                        bool updateExisting = !string.IsNullOrEmpty(hdnPersonalInfoId.Value);
                        
                        // Verify the record still exists if we're trying to update
                        if (updateExisting)
                        {
                            string checkQuery = "SELECT COUNT(*) FROM HomeContent WHERE Id = @id";
                            SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                            checkCmd.Parameters.AddWithValue("@id", Convert.ToInt32(hdnPersonalInfoId.Value));
                            int recordExists = (int)checkCmd.ExecuteScalar();
                            
                            if (recordExists == 0)
                            {
                                updateExisting = false; // Record doesn't exist, insert instead
                                hdnPersonalInfoId.Value = ""; // Clear the hidden field
                                ShowMessage($"Record with ID {hdnPersonalInfoId.Value} not found. Creating new record instead.", "warning");
                            }
                            else
                            {
                                ShowMessage($"Found existing record with ID {hdnPersonalInfoId.Value}. Updating...", "info");
                            }
                        }
                        
                        if (updateExisting)
                        {
                            // Update existing record in HomeContent table
                            string query = @"UPDATE HomeContent SET 
                                           HeroHeading = @heroHeading, 
                                           RoleText = @roleText, 
                                           Description = @description,
                                           ImagePath = @imagePath,
                                           GitHubUrl = @gitHubUrl,
                                           LinkedInUrl = @linkedInUrl,
                                           Email = @email,
                                           Phone = @phone,
                                           Resume = @resume
                                           WHERE Id = @id";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(hdnPersonalInfoId.Value));
                            cmd.Parameters.AddWithValue("@heroHeading", heroHeading);
                            cmd.Parameters.AddWithValue("@roleText", roleText);
                            cmd.Parameters.AddWithValue("@description", description);
                            cmd.Parameters.AddWithValue("@imagePath", profileImagePath ?? "");
                            cmd.Parameters.AddWithValue("@gitHubUrl", gitHubUrl);
                            cmd.Parameters.AddWithValue("@linkedInUrl", linkedInUrl);
                            cmd.Parameters.AddWithValue("@email", email);
                            cmd.Parameters.AddWithValue("@phone", phone);
                            cmd.Parameters.AddWithValue("@resume", resumeUrl);
                            
                            int result = cmd.ExecuteNonQuery();
                            
                            if (result > 0)
                            {
                                ShowMessage($"Personal information updated successfully! Rows affected: {result}\\n\\n{debugInfo}", "success");
                                hdnCurrentProfileImagePath.Value = profileImagePath;
                                
                                // Verify the update by reading back the data
                                string verifyQuery = "SELECT HeroHeading, RoleText FROM HomeContent WHERE Id = @id";
                                SqlCommand verifyCmd = new SqlCommand(verifyQuery, con);
                                verifyCmd.Parameters.AddWithValue("@id", Convert.ToInt32(hdnPersonalInfoId.Value));
                                SqlDataReader verifyReader = verifyCmd.ExecuteReader();
                                if (verifyReader.Read())
                                {
                                    string updatedHero = verifyReader["HeroHeading"].ToString();
                                    string updatedRole = verifyReader["RoleText"].ToString();
                                    ShowMessage($"Verification: Hero Heading is now '{updatedHero}', Role is now '{updatedRole}'", "info");
                                }
                                verifyReader.Close();
                            }
                            else
                            {
                                ShowMessage($"Failed to update personal information! No rows affected.\\n\\n{debugInfo}", "danger");
                            }
                        }
                        else
                        {
                            // Insert new record into HomeContent table
                            string query = @"INSERT INTO HomeContent 
                                           (HeroHeading, RoleText, Description, ImagePath, GitHubUrl, LinkedInUrl, Email, Phone, Resume)
                                           VALUES 
                                           (@heroHeading, @roleText, @description, @imagePath, @gitHubUrl, @linkedInUrl, @email, @phone, @resume)";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@heroHeading", heroHeading);
                            cmd.Parameters.AddWithValue("@roleText", roleText);
                            cmd.Parameters.AddWithValue("@description", description);
                            cmd.Parameters.AddWithValue("@imagePath", profileImagePath ?? "");
                            cmd.Parameters.AddWithValue("@gitHubUrl", gitHubUrl);
                            cmd.Parameters.AddWithValue("@linkedInUrl", linkedInUrl);
                            cmd.Parameters.AddWithValue("@email", email);
                            cmd.Parameters.AddWithValue("@phone", phone);
                            cmd.Parameters.AddWithValue("@resume", resumeUrl);
                            
                            int result = cmd.ExecuteNonQuery();
                            if (result > 0)
                            {
                                ShowMessage($"Personal information saved successfully! (New record created)\\n\\n{debugInfo}", "success");
                                
                                // Get the new record ID for future updates
                                string getIdQuery = "SELECT TOP 1 Id FROM HomeContent ORDER BY Id DESC";
                                SqlCommand getIdCmd = new SqlCommand(getIdQuery, con);
                                object newId = getIdCmd.ExecuteScalar();
                                if (newId != null)
                                {
                                    hdnPersonalInfoId.Value = newId.ToString();
                                }
                            }
                            else
                            {
                                ShowMessage($"Failed to save personal information!\\n\\n{debugInfo}", "danger");
                            }
                        }
                        
                        LoadPersonalInfo();
                        LoadCurrentInfoPreview();
                    }
                    catch (Exception ex)
                    {
                        ShowMessage($"Error: {ex.Message}\\n\\n{debugInfo}", "danger");
                    }
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearForm();
            LoadPersonalInfo();
        }

        protected void btnSaveNew_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                // Force creation of new record by clearing the ID
                hdnPersonalInfoId.Value = "";
                
                // Call the regular save method which will now do an INSERT
                btnSave_Click(sender, e);
            }
        }

        private string HandleImageUpload()
        {
            try
            {
                string fileName = Path.GetFileName(FileUploadProfile.FileName);
                string fileExtension = Path.GetExtension(fileName).ToLower();
                
                // Validate file type
                if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png" || fileExtension == ".gif")
                {
                    // Create uploads directory if it doesn't exist
                    string uploadDir = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }
                    
                    // Create unique filename
                    string uniqueFileName = "profile_" + Guid.NewGuid().ToString() + fileExtension;
                    string filePath = uploadDir + uniqueFileName;
                    
                    FileUploadProfile.SaveAs(filePath);
                    return "Uploads/" + uniqueFileName;
                }
                else
                {
                    ShowMessage("Please select a valid image file (jpg, jpeg, png, gif)!", "danger");
                    return null;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error uploading file: " + ex.Message, "danger");
                return null;
            }
        }

        private void DeleteImageFile(string imagePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(imagePath))
                {
                    string physicalPath = Server.MapPath("~/" + imagePath);
                    if (File.Exists(physicalPath))
                    {
                        File.Delete(physicalPath);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error but don't show to user as it's not critical
            }
        }

        private void ClearForm()
        {
            txtFullName.Text = "";
            txtRole.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtHeroDescription.Text = "";
            txtAboutDescription.Text = "";
            txtGitHubUrl.Text = "";
            txtGitHubText.Text = "";
            txtLinkedInUrl.Text = "";
            txtLinkedInText.Text = "";
            txtResumeUrl.Text = "";
            hdnPersonalInfoId.Value = "";
            hdnCurrentProfileImagePath.Value = "";
            currentProfileDiv.Visible = false;
        }

        private void ShowMessage(string message, string type)
        {
            string alertClass = "";
            switch (type)
            {
                case "success":
                    alertClass = "alert-success";
                    break;
                case "warning":
                    alertClass = "alert-warning";
                    break;
                case "info":
                    alertClass = "alert-info";
                    break;
                case "danger":
                default:
                    alertClass = "alert-danger";
                    break;
            }
            
            string script = $@"
                var alertDiv = document.createElement('div');
                alertDiv.className = 'alert {alertClass} alert-dismissible fade show';
                alertDiv.innerHTML = '{message.Replace("'", "\\'")}' + '<button type=""button"" class=""btn-close"" data-bs-dismiss=""alert""></button>';
                document.querySelector('.container').insertBefore(alertDiv, document.querySelector('.container').firstChild);
                setTimeout(function() {{
                    if (alertDiv.parentNode) {{
                        alertDiv.parentNode.removeChild(alertDiv);
                    }}
                }}, 8000);
            ";
            ClientScript.RegisterStartupScript(this.GetType(), "ShowMessage", script, true);
        }
    }
}
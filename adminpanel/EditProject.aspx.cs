using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace adminpanel
{
    public partial class EditProject : System.Web.UI.Page
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
                LoadProject();
            }
        }

        private void LoadProject()
        {
            string projectIdParam = Request.QueryString["id"];
            if (string.IsNullOrEmpty(projectIdParam) || !int.TryParse(projectIdParam, out int projectId))
            {
                ShowMessage("Invalid project ID.", "danger");
                return;
            }

            hdnProjectId.Value = projectId.ToString();

            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    string query = "SELECT * FROM Projects WHERE Id = @id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", projectId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtTitle.Text = reader["Title"].ToString();
                        txtDescription.Text = reader["Description"].ToString();
                        txtTechUsed.Text = reader["TechUsed"].ToString();
                        txtGitUrl.Text = reader["GitUrl"].ToString();

                        string imagePath = reader["ImagePath"].ToString();
                        hdnCurrentImagePath.Value = imagePath;

                        if (!string.IsNullOrEmpty(imagePath))
                        {
                            imgCurrent.ImageUrl = imagePath;
                            currentImageDiv.Visible = true;
                        }
                    }
                    else
                    {
                        ShowMessage("Project not found.", "danger");
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading project: " + ex.Message, "danger");
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid && !string.IsNullOrEmpty(hdnProjectId.Value))
            {
                int projectId = Convert.ToInt32(hdnProjectId.Value);
                string title = txtTitle.Text.Trim();
                string description = txtDescription.Text.Trim();
                string techUsed = txtTechUsed.Text.Trim();
                string gitUrl = txtGitUrl.Text.Trim();
                string imagePath = hdnCurrentImagePath.Value; // Keep current image by default

                // Handle file upload
                if (FileUpload1.HasFile)
                {
                    string newImagePath = HandleImageUpload();
                    if (newImagePath != null)
                    {
                        // Delete old image
                        if (!string.IsNullOrEmpty(imagePath))
                        {
                            DeleteImageFile(imagePath);
                        }
                        imagePath = newImagePath;
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
                        string query = @"UPDATE Projects SET 
                                       Title = @title, 
                                       Description = @description, 
                                       ImagePath = @imagePath, 
                                       TechUsed = @techUsed, 
                                       GitUrl = @gitUrl 
                                       WHERE Id = @id";

                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@imagePath", imagePath);
                        cmd.Parameters.AddWithValue("@techUsed", techUsed);
                        cmd.Parameters.AddWithValue("@gitUrl", gitUrl);
                        cmd.Parameters.AddWithValue("@id", projectId);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            ShowMessage("Project updated successfully!", "success");
                            // Update the hidden field with new image path
                            hdnCurrentImagePath.Value = imagePath;
                            
                            // Update the displayed image
                            if (!string.IsNullOrEmpty(imagePath))
                            {
                                imgCurrent.ImageUrl = imagePath;
                                currentImageDiv.Visible = true;
                            }
                        }
                        else
                        {
                            ShowMessage("Failed to update project!", "danger");
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowMessage("Error: " + ex.Message, "danger");
                    }
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdnProjectId.Value))
            {
                int projectId = Convert.ToInt32(hdnProjectId.Value);
                string imagePath = hdnCurrentImagePath.Value;

                using (SqlConnection con = new SqlConnection(cs))
                {
                    try
                    {
                        con.Open();
                        string query = "DELETE FROM Projects WHERE Id = @id";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@id", projectId);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            // Delete image file
                            if (!string.IsNullOrEmpty(imagePath))
                            {
                                DeleteImageFile(imagePath);
                            }

                            ShowMessage("Project deleted successfully!", "success");
                            
                            // Redirect to projects page after a delay
                            string script = "setTimeout(function() { window.location.href = 'Projects.aspx'; }, 2000);";
                            ClientScript.RegisterStartupScript(this.GetType(), "RedirectScript", script, true);
                        }
                        else
                        {
                            ShowMessage("Failed to delete project!", "danger");
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowMessage("Error: " + ex.Message, "danger");
                    }
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Projects.aspx");
        }

        private string HandleImageUpload()
        {
            try
            {
                string fileName = Path.GetFileName(FileUpload1.FileName);
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
                    string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    string filePath = uploadDir + uniqueFileName;

                    FileUpload1.SaveAs(filePath);
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

        private void ShowMessage(string message, string type)
        {
            lblMessage.Text = message;
            pnlMessage.Visible = true;

            // Add appropriate CSS class based on message type
            switch (type)
            {
                case "success":
                    pnlMessage.CssClass = "alert alert-success";
                    break;
                case "warning":
                    pnlMessage.CssClass = "alert alert-warning";
                    break;
                case "danger":
                default:
                    pnlMessage.CssClass = "alert alert-danger";
                    break;
            }
        }
    }
}
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
    public partial class Skills : System.Web.UI.Page
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
                LoadSkills();
                LoadSkillsPreview();
                LoadSkillsStats();
            }
        }

        #region Skills CRUD Operations

        private void LoadSkills()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    
                    // First ensure the table has the columns we need
                    EnsureSkillsTableStructure(con);
                    
                    // Load skills with calculated percentage and display order
                    string query = @"SELECT Id, Name, 
                                   COALESCE(IconUrl, 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/html5/html5-original.svg') as IconUrl,
                                   COALESCE(Percentage, 75) as Percentage,
                                   COALESCE(DisplayOrder, ROW_NUMBER() OVER (ORDER BY Name)) as DisplayOrder
                                   FROM Skills 
                                   ORDER BY COALESCE(DisplayOrder, 999), Name";
                    
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    
                    // Add missing columns if they don't exist in the data
                    if (!dt.Columns.Contains("Percentage"))
                    {
                        dt.Columns.Add("Percentage", typeof(int));
                        foreach (DataRow row in dt.Rows)
                        {
                            row["Percentage"] = 75; // Default percentage
                        }
                    }
                    
                    if (!dt.Columns.Contains("DisplayOrder"))
                    {
                        dt.Columns.Add("DisplayOrder", typeof(int));
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i]["DisplayOrder"] = i + 1;
                        }
                    }
                    
                    gvSkills.DataSource = dt;
                    gvSkills.DataBind();
                    
                    // Update skills count
                    lblSkillsCount.Text = $"Total Skills: {dt.Rows.Count}";
                    lblSkillsCount.CssClass = dt.Rows.Count > 0 ? "skills-card-label skills-stats" : "skills-card-label skills-stats empty";
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading skills: " + ex.Message, "error");
                    lblSkillsCount.Text = "Error loading skills";
                    lblSkillsCount.CssClass = "skills-card-label skills-stats error";
                }
            }
        }

        private void EnsureSkillsTableStructure(SqlConnection connection)
        {
            try
            {
                // Check if additional columns exist and add them if they don't
                string checkColumns = @"
                    SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'Skills' AND COLUMN_NAME IN ('Percentage', 'DisplayOrder', 'Category')";
                
                SqlCommand checkCmd = new SqlCommand(checkColumns, connection);
                int existingColumns = (int)checkCmd.ExecuteScalar();
                
                if (existingColumns < 3)
                {
                    // Add missing columns
                    string addColumns = @"
                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Skills' AND COLUMN_NAME = 'Percentage')
                            ALTER TABLE Skills ADD Percentage INT DEFAULT 75;
                        
                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Skills' AND COLUMN_NAME = 'DisplayOrder')
                            ALTER TABLE Skills ADD DisplayOrder INT DEFAULT 0;
                        
                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Skills' AND COLUMN_NAME = 'Category')
                            ALTER TABLE Skills ADD Category NVARCHAR(50) DEFAULT 'General';";
                    
                    SqlCommand addCmd = new SqlCommand(addColumns, connection);
                    addCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error ensuring table structure: {ex.Message}");
            }
        }

        // Refresh Skills
        protected void btnRefreshSkills_Click(object sender, EventArgs e)
        {
            LoadSkills();
            LoadSkillsPreview();
            LoadSkillsStats();
            ShowMessage("Skills refreshed successfully!", "success");
        }

        // Add New Skill
        protected void btnAddNewSkill_Click(object sender, EventArgs e)
        {
            ClearModalForm();
            ltlModalTitle.Text = "Add New Skill";
            pnlSkillForm.Visible = true;
            pnlSkillView.Visible = false;
            btnSaveSkill.Visible = true;
            btnSaveSkill.Text = "Add Skill";
            ViewState["EditingSkillId"] = null;
            
            ShowModal();
        }

        // Bulk Actions
        protected void btnBulkActions_Click(object sender, EventArgs e)
        {
            // Implement bulk actions like reorder, delete multiple, etc.
            ShowMessage("Bulk actions feature - coming soon!", "info");
        }

        // GridView Event Handlers
        protected void gvSkills_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSkills.PageIndex = e.NewPageIndex;
            LoadSkills();
        }

        protected void gvSkills_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewSkill")
            {
                int skillId = Convert.ToInt32(e.CommandArgument);
                ViewSkillDetails(skillId);
            }
        }

        protected void gvSkills_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvSkills.EditIndex = e.NewEditIndex;
            LoadSkills();
        }

        protected void gvSkills_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int skillId = Convert.ToInt32(gvSkills.DataKeys[e.RowIndex].Value);
                GridViewRow row = gvSkills.Rows[e.RowIndex];
                
                string name = ((TextBox)row.FindControl("txtName")).Text.Trim();
                string iconUrl = ((TextBox)row.FindControl("txtIconUrl")).Text.Trim();
                string percentageStr = ((TextBox)row.FindControl("txtPercentage")).Text.Trim();
                string displayOrderStr = ((TextBox)row.FindControl("txtDisplayOrder")).Text.Trim();

                int percentage = string.IsNullOrEmpty(percentageStr) ? 75 : Convert.ToInt32(percentageStr);
                int displayOrder = string.IsNullOrEmpty(displayOrderStr) ? 0 : Convert.ToInt32(displayOrderStr);

                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    string query = @"UPDATE Skills 
                                   SET Name = @name, IconUrl = @iconUrl, Percentage = @percentage, DisplayOrder = @displayOrder 
                                   WHERE Id = @id";
                    
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@iconUrl", iconUrl);
                    cmd.Parameters.AddWithValue("@percentage", percentage);
                    cmd.Parameters.AddWithValue("@displayOrder", displayOrder);
                    cmd.Parameters.AddWithValue("@id", skillId);
                    
                    cmd.ExecuteNonQuery();
                }

                gvSkills.EditIndex = -1;
                LoadSkills();
                LoadSkillsPreview();
                ShowMessage("Skill updated successfully!", "success");
            }
            catch (Exception ex)
            {
                ShowMessage("Error updating skill: " + ex.Message, "error");
            }
        }

        protected void gvSkills_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvSkills.EditIndex = -1;
            LoadSkills();
        }

        protected void gvSkills_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                // Get the skill ID from DataKeys - this is the correct way
                int skillId = Convert.ToInt32(gvSkills.DataKeys[e.RowIndex].Value);
                
                System.Diagnostics.Debug.WriteLine($"Attempting to delete skill with ID: {skillId}");
                
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    
                    // First check if the skill exists
                    string checkQuery = "SELECT COUNT(*) FROM Skills WHERE Id = @id";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                    checkCmd.Parameters.AddWithValue("@id", skillId);
                    int exists = (int)checkCmd.ExecuteScalar();
                    
                    if (exists == 0)
                    {
                        ShowMessage("Skill not found in database!", "error");
                        return;
                    }
                    
                    // Now delete the skill
                    string deleteQuery = "DELETE FROM Skills WHERE Id = @id";
                    SqlCommand deleteCmd = new SqlCommand(deleteQuery, con);
                    deleteCmd.Parameters.AddWithValue("@id", skillId);
                    
                    int rowsAffected = deleteCmd.ExecuteNonQuery();
                    
                    System.Diagnostics.Debug.WriteLine($"Rows affected by delete: {rowsAffected}");
                    
                    if (rowsAffected > 0)
                    {
                        LoadSkills();
                        LoadSkillsPreview();
                        LoadSkillsStats();
                        ShowMessage($"Skill deleted successfully! (ID: {skillId})", "success");
                    }
                    else
                    {
                        ShowMessage("No rows were deleted. Please try again.", "error");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in delete: {ex.Message}");
                ShowMessage("Error deleting skill: " + ex.Message, "error");
            }
        }

        // Modal Operations
        private void ViewSkillDetails(int skillId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    string query = @"SELECT Id, Name, IconUrl, 
                                   COALESCE(Percentage, 75) as Percentage,
                                   COALESCE(DisplayOrder, 0) as DisplayOrder,
                                   COALESCE(Category, 'General') as Category
                                   FROM Skills WHERE Id = @id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", skillId);
                    
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    if (reader.Read())
                    {
                        StringBuilder html = new StringBuilder();
                        
                        // Header with skill info
                        html.Append("<div class='skill-details-header'>");
                        html.Append($"<div class='skill-icon-large'><img src='{reader["IconUrl"]}' alt='{reader["Name"]}' /></div>");
                        html.Append($"<div class='skill-info'>");
                        html.Append($"<h4>{reader["Name"]}</h4>");
                        html.Append($"<div class='skill-category'>{reader["Category"]}</div>");
                        html.Append("</div>");
                        html.Append("</div>");
                        
                        // Skill details
                        html.Append("<div class='skill-details-content'>");
                        
                        html.Append("<div class='detail-item'>");
                        html.Append("<strong>Proficiency Level:</strong>");
                        html.Append($"<div class='skill-progress-large'>");
                        html.Append($"<div class='progress-bar-large' style='width: {reader["Percentage"]}%;'>");
                        html.Append($"<span>{reader["Percentage"]}% - {GetSkillLevel(Convert.ToInt32(reader["Percentage"]))}</span>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");
                        
                        html.Append($"<div class='detail-item'><strong>Display Order:</strong> {reader["DisplayOrder"]}</div>");
                        html.Append($"<div class='detail-item'><strong>Icon URL:</strong> <a href='{reader["IconUrl"]}' target='_blank'>View Icon</a></div>");
                        
                        html.Append("</div>");
                        
                        ltlSkillDetails.Text = html.ToString();
                        
                        // Set up modal for viewing
                        ltlModalTitle.Text = $"Skill Details - {reader["Name"]}";
                        pnlSkillForm.Visible = false;
                        pnlSkillView.Visible = true;
                        btnSaveSkill.Visible = false;
                        
                        ShowModal();
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading skill details: " + ex.Message, "error");
                }
            }
        }

        protected void btnSaveSkill_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtModalSkillName.Text.Trim();
                string iconUrl = txtModalIconUrl.Text.Trim();
                string percentageStr = txtModalPercentage.Text.Trim();
                string displayOrderStr = txtModalDisplayOrder.Text.Trim();
                string category = ddlModalCategory.SelectedValue;

                int percentage = string.IsNullOrEmpty(percentageStr) ? 75 : Convert.ToInt32(percentageStr);
                int displayOrder = string.IsNullOrEmpty(displayOrderStr) ? 0 : Convert.ToInt32(displayOrderStr);

                // Set default icon if empty
                if (string.IsNullOrEmpty(iconUrl))
                {
                    iconUrl = GetDefaultIcon(name);
                }

                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    
                    if (ViewState["EditingSkillId"] == null)
                    {
                        // Add new skill
                        string query = @"INSERT INTO Skills (Name, IconUrl, Percentage, DisplayOrder, Category) 
                                       VALUES (@name, @iconUrl, @percentage, @displayOrder, @category)";
                        
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@iconUrl", iconUrl);
                        cmd.Parameters.AddWithValue("@percentage", percentage);
                        cmd.Parameters.AddWithValue("@displayOrder", displayOrder);
                        cmd.Parameters.AddWithValue("@category", category);
                        
                        cmd.ExecuteNonQuery();
                        ShowMessage("Skill added successfully!", "success");
                    }
                    else
                    {
                        // Update existing skill
                        int skillId = Convert.ToInt32(ViewState["EditingSkillId"]);
                        string query = @"UPDATE Skills 
                                       SET Name = @name, IconUrl = @iconUrl, Percentage = @percentage, 
                                           DisplayOrder = @displayOrder, Category = @category 
                                       WHERE Id = @id";
                        
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@iconUrl", iconUrl);
                        cmd.Parameters.AddWithValue("@percentage", percentage);
                        cmd.Parameters.AddWithValue("@displayOrder", displayOrder);
                        cmd.Parameters.AddWithValue("@category", category);
                        cmd.Parameters.AddWithValue("@id", skillId);
                        
                        cmd.ExecuteNonQuery();
                        ShowMessage("Skill updated successfully!", "success");
                    }
                }

                LoadSkills();
                LoadSkillsPreview();
                LoadSkillsStats();
                HideModal();
            }
            catch (Exception ex)
            {
                ShowMessage("Error saving skill: " + ex.Message, "error");
            }
        }

        #endregion

        #region Helper Methods

        protected string GetSkillLevel(int percentage)
        {
            if (percentage >= 90) return "Expert";
            if (percentage >= 75) return "Advanced";
            if (percentage >= 60) return "Intermediate";
            if (percentage >= 40) return "Beginner";
            return "Learning";
        }

        private string GetDefaultIcon(string skillName)
        {
            string name = skillName.ToLower();
            
            if (name.Contains("html")) return "https://cdn.jsdelivr.net/gh/devicons/devicon/icons/html5/html5-original.svg";
            if (name.Contains("css")) return "https://cdn.jsdelivr.net/gh/devicons/devicon/icons/css3/css3-original.svg";
            if (name.Contains("javascript") || name.Contains("js")) return "https://cdn.jsdelivr.net/gh/devicons/devicon/icons/javascript/javascript-original.svg";
            if (name.Contains("react")) return "https://cdn.jsdelivr.net/gh/devicons/devicon/icons/react/react-original.svg";
            if (name.Contains("python")) return "https://cdn.jsdelivr.net/gh/devicons/devicon/icons/python/python-original.svg";
            if (name.Contains("java")) return "https://cdn.jsdelivr.net/gh/devicons/devicon/icons/java/java-original.svg";
            if (name.Contains("c#") || name.Contains("csharp")) return "https://cdn.jsdelivr.net/gh/devicons/devicon/icons/csharp/csharp-original.svg";
            if (name.Contains("node")) return "https://cdn.jsdelivr.net/gh/devicons/devicon/icons/nodejs/nodejs-original.svg";
            if (name.Contains("sql")) return "https://cdn.jsdelivr.net/gh/devicons/devicon/icons/mysql/mysql-original.svg";
            
            return "https://cdn.jsdelivr.net/gh/devicons/devicon/icons/html5/html5-original.svg";
        }

        private void LoadSkillsPreview()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    string query = @"SELECT TOP 6 Name, IconUrl, 
                                   COALESCE(Percentage, 75) as Percentage
                                   FROM Skills 
                                   ORDER BY COALESCE(DisplayOrder, 999), Name";
                    
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    StringBuilder html = new StringBuilder();
                    html.Append("<div class='skills-preview-grid'>");
                    
                    while (reader.Read())
                    {
                        html.Append("<div class='skill-preview-item'>");
                        html.Append($"<div class='skill-preview-icon'><img src='{reader["IconUrl"]}' alt='{reader["Name"]}' /></div>");
                        html.Append($"<div class='skill-preview-name'>{reader["Name"]}</div>");
                        html.Append("<div class='skill-preview-progress'>");
                        html.Append($"<div class='progress-preview' style='width: {reader["Percentage"]}%;'></div>");
                        html.Append("</div>");
                        html.Append($"<div class='skill-preview-percent'>{reader["Percentage"]}%</div>");
                        html.Append("</div>");
                    }
                    
                    html.Append("</div>");
                    
                    if (!reader.HasRows)
                    {
                        html.Clear();
                        html.Append("<p class='text-center text-muted'>No skills to preview. Add some skills first!</p>");
                    }
                    
                    ltlSkillsPreview.Text = html.ToString();
                    reader.Close();
                }
                catch (Exception ex)
                {
                    ltlSkillsPreview.Text = "<p class='text-center text-danger'>Error loading preview.</p>";
                }
            }
        }

        private void LoadSkillsStats()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    string query = @"SELECT 
                                   COUNT(*) as TotalSkills,
                                   AVG(CAST(COALESCE(Percentage, 75) AS FLOAT)) as AvgProficiency,
                                   MAX(COALESCE(Percentage, 75)) as MaxProficiency,
                                   COUNT(CASE WHEN COALESCE(Percentage, 75) >= 75 THEN 1 END) as AdvancedSkills
                                   FROM Skills";
                    
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    if (reader.Read())
                    {
                        int total = Convert.ToInt32(reader["TotalSkills"]);
                        double avgProf = reader["AvgProficiency"] != DBNull.Value ? Convert.ToDouble(reader["AvgProficiency"]) : 0;
                        int maxProf = reader["MaxProficiency"] != DBNull.Value ? Convert.ToInt32(reader["MaxProficiency"]) : 0;
                        int advanced = Convert.ToInt32(reader["AdvancedSkills"]);
                        
                        StringBuilder html = new StringBuilder();
                        html.Append($"<p><strong>Total Skills:</strong> {total}</p>");
                        html.Append($"<p><strong>Average Proficiency:</strong> {avgProf:F1}%</p>");
                        html.Append($"<p><strong>Highest Proficiency:</strong> {maxProf}%</p>");
                        html.Append($"<p><strong>Advanced Skills:</strong> {advanced}</p>");
                        
                        ltlSkillsStats.Text = html.ToString();
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    ltlSkillsStats.Text = "<p>Error loading statistics.</p>";
                }
            }
        }

        private void ClearModalForm()
        {
            txtModalSkillName.Text = "";
            txtModalIconUrl.Text = "";
            txtModalPercentage.Text = "";
            txtModalDisplayOrder.Text = "";
            ddlModalCategory.SelectedIndex = 0;
        }

        private void ShowModal()
        {
            string script = @"
                document.getElementById('skillModal').style.display = 'flex';
            ";
            ClientScript.RegisterStartupScript(this.GetType(), "ShowModal", script, true);
        }

        private void HideModal()
        {
            string script = @"
                document.getElementById('skillModal').style.display = 'none';
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
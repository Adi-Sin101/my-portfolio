using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

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
            }
        }

        private void LoadSkills()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    string query = "SELECT Id, Name, IconUrl, Percentage, DisplayOrder, IsActive FROM Skills WHERE IsActive = 1 ORDER BY DisplayOrder, Name";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    
                    gvSkills.DataSource = dt;
                    gvSkills.DataBind();
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading skills: " + ex.Message, "danger");
                }
            }
        }

        protected void btnAddSkill_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string name = txtSkillName.Text.Trim();
                string iconUrl = txtIconUrl.Text.Trim();
                int percentage = string.IsNullOrEmpty(txtPercentage.Text) ? 0 : Convert.ToInt32(txtPercentage.Text);
                int displayOrder = string.IsNullOrEmpty(txtDisplayOrder.Text) ? 0 : Convert.ToInt32(txtDisplayOrder.Text);

                using (SqlConnection con = new SqlConnection(cs))
                {
                    try
                    {
                        con.Open();
                        string query = "INSERT INTO Skills (Name, IconUrl, Percentage, DisplayOrder, IsActive, CreatedDate) VALUES (@name, @icon, @percentage, @order, 1, GETDATE())";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@icon", iconUrl);
                        cmd.Parameters.AddWithValue("@percentage", percentage);
                        cmd.Parameters.AddWithValue("@order", displayOrder);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            ShowMessage("Skill added successfully!", "success");
                            ClearForm();
                            LoadSkills();
                        }
                        else
                        {
                            ShowMessage("Failed to add skill!", "danger");
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowMessage("Error: " + ex.Message, "danger");
                    }
                }
            }
        }

        protected void btnUpdateSkill_Click(object sender, EventArgs e)
        {
            if (Page.IsValid && !string.IsNullOrEmpty(hdnSkillId.Value))
            {
                int skillId = Convert.ToInt32(hdnSkillId.Value);
                string name = txtSkillName.Text.Trim();
                string iconUrl = txtIconUrl.Text.Trim();
                int percentage = string.IsNullOrEmpty(txtPercentage.Text) ? 0 : Convert.ToInt32(txtPercentage.Text);
                int displayOrder = string.IsNullOrEmpty(txtDisplayOrder.Text) ? 0 : Convert.ToInt32(txtDisplayOrder.Text);

                using (SqlConnection con = new SqlConnection(cs))
                {
                    try
                    {
                        con.Open();
                        string query = "UPDATE Skills SET Name = @name, IconUrl = @icon, Percentage = @percentage, DisplayOrder = @order WHERE Id = @id";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@icon", iconUrl);
                        cmd.Parameters.AddWithValue("@percentage", percentage);
                        cmd.Parameters.AddWithValue("@order", displayOrder);
                        cmd.Parameters.AddWithValue("@id", skillId);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            ShowMessage("Skill updated successfully!", "success");
                            ClearForm();
                            LoadSkills();
                            SwitchToAddMode();
                        }
                        else
                        {
                            ShowMessage("Failed to update skill!", "danger");
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowMessage("Error: " + ex.Message, "danger");
                    }
                }
            }
        }

        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            ClearForm();
            SwitchToAddMode();
        }

        protected void gvSkills_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int skillId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditSkill")
            {
                LoadSkillForEdit(skillId);
            }
            else if (e.CommandName == "DeleteSkill")
            {
                DeleteSkill(skillId);
            }
        }

        private void LoadSkillForEdit(int skillId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    string query = "SELECT Id, Name, IconUrl, Percentage, DisplayOrder FROM Skills WHERE Id = @id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", skillId);
                    
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        hdnSkillId.Value = reader["Id"].ToString();
                        txtSkillName.Text = reader["Name"].ToString();
                        txtIconUrl.Text = reader["IconUrl"].ToString();
                        txtPercentage.Text = reader["Percentage"].ToString();
                        txtDisplayOrder.Text = reader["DisplayOrder"].ToString();
                        
                        SwitchToEditMode();
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading skill for edit: " + ex.Message, "danger");
                }
            }
        }

        private void DeleteSkill(int skillId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    // Soft delete - just mark as inactive
                    string query = "UPDATE Skills SET IsActive = 0 WHERE Id = @id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", skillId);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        ShowMessage("Skill deleted successfully!", "success");
                        LoadSkills();
                    }
                    else
                    {
                        ShowMessage("Failed to delete skill!", "danger");
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Error: " + ex.Message, "danger");
                }
            }
        }

        private void ClearForm()
        {
            txtSkillName.Text = "";
            txtIconUrl.Text = "";
            txtPercentage.Text = "";
            txtDisplayOrder.Text = "";
            hdnSkillId.Value = "";
        }

        private void SwitchToEditMode()
        {
            btnAddSkill.Visible = false;
            btnUpdateSkill.Visible = true;
            btnCancelEdit.Visible = true;
        }

        private void SwitchToAddMode()
        {
            btnAddSkill.Visible = true;
            btnUpdateSkill.Visible = false;
            btnCancelEdit.Visible = false;
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
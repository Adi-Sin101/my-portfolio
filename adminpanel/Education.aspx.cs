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
    public partial class Education : System.Web.UI.Page
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
                EnsureEducationTableExists();
                LoadEducation();
                LoadEducationStats();
            }
        }

        #region Database Table Management

        private void EnsureEducationTableExists()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();

                    // Check if Education table exists
                    string checkTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Education' AND xtype='U')
                        BEGIN
                            CREATE TABLE Education (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                Degree NVARCHAR(200) NOT NULL,
                                Institution NVARCHAR(200) NOT NULL,
                                Year NVARCHAR(50),
                                Grade NVARCHAR(100)
                            );
                        END";

                    SqlCommand cmd = new SqlCommand(checkTableQuery, con);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    ShowMessage("Error ensuring Education table exists: " + ex.Message, "error");
                }
            }
        }

        #endregion

        #region Education CRUD Operations

        private void LoadEducation()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    string query = "SELECT Id, Degree, Institution, Year, Grade FROM Education ORDER BY Id DESC";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvEducation.DataSource = dt;
                    gvEducation.DataBind();

                    // Load preview data
                    LoadEducationPreview();
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading education records: " + ex.Message, "error");
                }
            }
        }

        private void LoadEducationPreview()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    string query = "SELECT Degree, Institution, Year, Grade FROM Education ORDER BY Id DESC";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    rptEducationPreview.DataSource = dt;
                    rptEducationPreview.DataBind();
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading education preview: " + ex.Message, "error");
                }
            }
        }

        private void LoadEducationStats()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();

                    // Get total count
                    string countQuery = "SELECT COUNT(*) FROM Education";
                    SqlCommand countCmd = new SqlCommand(countQuery, con);
                    int totalCount = (int)countCmd.ExecuteScalar();
                    lblEducationCount.Text = totalCount.ToString();

                    // Get degree count (only degree-level education)
                    string degreeQuery = "SELECT COUNT(*) FROM Education WHERE Degree LIKE '%B.%' OR Degree LIKE '%M.%' OR Degree LIKE '%Ph.%' OR Degree LIKE '%Bachelor%' OR Degree LIKE '%Master%' OR Degree LIKE '%Doctor%'";
                    SqlCommand degreeCmd = new SqlCommand(degreeQuery, con);
                    int degreeCount = (int)degreeCmd.ExecuteScalar();
                    lblDegreeCount.Text = degreeCount.ToString();

                    // Get last updated (for simplicity, we'll show current date if records exist)
                    if (totalCount > 0)
                    {
                        lblLastUpdated.Text = DateTime.Now.ToString("MMM dd, yyyy");
                    }
                    else
                    {
                        lblLastUpdated.Text = "Never";
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading education statistics: " + ex.Message, "error");
                }
            }
        }

        private void SaveEducation()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    string query;
                    SqlCommand cmd;

                    if (string.IsNullOrEmpty(hdnEducationId.Value))
                    {
                        // Insert new record
                        query = @"INSERT INTO Education (Degree, Institution, Year, Grade) 
                                 VALUES (@Degree, @Institution, @Year, @Grade)";
                        cmd = new SqlCommand(query, con);
                    }
                    else
                    {
                        // Update existing record
                        query = @"UPDATE Education SET Degree = @Degree, Institution = @Institution, 
                                 Year = @Year, Grade = @Grade WHERE Id = @Id";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@Id", hdnEducationId.Value);
                    }

                    cmd.Parameters.AddWithValue("@Degree", txtDegree.Text.Trim());
                    cmd.Parameters.AddWithValue("@Institution", txtInstitution.Text.Trim());
                    cmd.Parameters.AddWithValue("@Year", string.IsNullOrEmpty(txtYear.Text.Trim()) ? (object)DBNull.Value : txtYear.Text.Trim());
                    cmd.Parameters.AddWithValue("@Grade", string.IsNullOrEmpty(txtGrade.Text.Trim()) ? (object)DBNull.Value : txtGrade.Text.Trim());

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        string message = string.IsNullOrEmpty(hdnEducationId.Value) ? 
                            "Education record added successfully!" : 
                            "Education record updated successfully!";
                        ShowMessage(message, "success");
                        
                        ClearForm();
                        LoadEducation();
                        LoadEducationStats();
                        pnlAddEdit.Visible = false;
                    }
                    else
                    {
                        ShowMessage("Failed to save education record. Please try again.", "error");
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Error saving education record: " + ex.Message, "error");
                }
            }
        }

        private void LoadEducationForEdit(int educationId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    string query = "SELECT * FROM Education WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", educationId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        hdnEducationId.Value = reader["Id"].ToString();
                        txtDegree.Text = reader["Degree"].ToString();
                        txtInstitution.Text = reader["Institution"].ToString();
                        txtYear.Text = reader["Year"].ToString();
                        txtGrade.Text = reader["Grade"].ToString();

                        lblFormMode.Text = "Edit Education";
                        pnlAddEdit.Visible = true;
                        pnlPreview.Visible = false;
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading education record for editing: " + ex.Message, "error");
                }
            }
        }

        private void DeleteEducation(int educationId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    string query = "DELETE FROM Education WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", educationId);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        ShowMessage("Education record deleted successfully!", "success");
                        LoadEducation();
                        LoadEducationStats();
                    }
                    else
                    {
                        ShowMessage("Failed to delete education record. Please try again.", "error");
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Error deleting education record: " + ex.Message, "error");
                }
            }
        }

        #endregion

        #region Button Event Handlers

        protected void BtnAddNew_Click(object sender, EventArgs e)
        {
            ClearForm();
            lblFormMode.Text = "Add New Education";
            pnlAddEdit.Visible = true;
            pnlPreview.Visible = false;
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveEducation();
            }
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            pnlAddEdit.Visible = false;
            pnlPreview.Visible = false;
        }

        protected void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadEducation();
            LoadEducationStats();
            ShowMessage("Education records refreshed successfully!", "success");
        }

        protected void BtnPreview_Click(object sender, EventArgs e)
        {
            LoadEducationPreview();
            pnlPreview.Visible = true;
            pnlAddEdit.Visible = false;
        }

        #endregion

        #region GridView Event Handlers

        protected void GvEducation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditEducation")
            {
                int educationId = Convert.ToInt32(e.CommandArgument);
                LoadEducationForEdit(educationId);
            }
            else if (e.CommandName == "DeleteEducation")
            {
                int educationId = Convert.ToInt32(e.CommandArgument);
                DeleteEducation(educationId);
            }
        }

        protected void GvEducation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Add hover effects or other row customizations if needed
            }
        }

        #endregion

        #region Helper Methods

        private void ClearForm()
        {
            hdnEducationId.Value = "";
            txtDegree.Text = "";
            txtInstitution.Text = "";
            txtYear.Text = "";
            txtGrade.Text = "";
        }

        private void ShowMessage(string message, string type)
        {
            lblMessage.Text = message;
            pnlMessage.CssClass = $"education-message {type}";
            pnlMessage.Visible = true;

            // Auto-hide message after 5 seconds
            Page.ClientScript.RegisterStartupScript(this.GetType(), "HideMessage",
                "setTimeout(function(){ document.querySelector('.education-message').style.display = 'none'; }, 5000);", true);
        }

        // Helper method for formatting grades in preview
        protected string FormatGrade(string grade, string degree)
        {
            if (string.IsNullOrEmpty(grade)) return "";

            // Format based on degree type
            if (degree.ToLower().Contains("b.sc") || degree.ToLower().Contains("bachelor"))
            {
                return $"CGPA: {grade}";
            }
            else if (degree.ToUpper().Contains("HSC") || degree.ToUpper().Contains("SSC"))
            {
                return $"GPA: {grade}";
            }
            else
            {
                return grade;
            }
        }

        #endregion
    }
}
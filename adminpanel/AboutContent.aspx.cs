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
    public partial class AboutContent : System.Web.UI.Page
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
                EnsureAboutContentTable();
                LoadAboutContent();
            }
        }

        #region AboutContent CRUD Operations

        private void EnsureAboutContentTable()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    
                    // Check if AboutContent table exists, create if not
                    string checkTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AboutContent' AND xtype='U')
                        BEGIN
                            CREATE TABLE AboutContent (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                AboutText NVARCHAR(MAX) NOT NULL
                            )
                            
                            -- Insert sample about content
                            INSERT INTO AboutContent (AboutText) VALUES 
                            ('Welcome to my portfolio! I am a passionate web developer with expertise in modern technologies and frameworks. I enjoy creating innovative solutions and bringing ideas to life through code.')
                        END
                        ELSE
                        BEGIN
                            -- Check if we need to add timestamp columns to existing table
                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AboutContent') AND name = 'CreatedDate')
                            BEGIN
                                ALTER TABLE AboutContent ADD CreatedDate DATETIME DEFAULT GETDATE()
                            END
                            
                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AboutContent') AND name = 'ModifiedDate')
                            BEGIN
                                ALTER TABLE AboutContent ADD ModifiedDate DATETIME DEFAULT GETDATE()
                            END
                        END";
                    
                    SqlCommand createCmd = new SqlCommand(checkTableQuery, con);
                    createCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    ShowMessage("Error initializing AboutContent table: " + ex.Message, "danger");
                }
            }
        }

        private void LoadAboutContent()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    
                    // Check which columns exist
                    bool hasTimestamps = CheckIfTimestampColumnsExist(con);
                    
                    string query;
                    if (hasTimestamps)
                    {
                        query = @"SELECT Id, AboutText, CreatedDate, ModifiedDate 
                                FROM AboutContent 
                                ORDER BY COALESCE(ModifiedDate, GETDATE()) DESC, Id DESC";
                    }
                    else
                    {
                        query = @"SELECT Id, AboutText 
                                FROM AboutContent 
                                ORDER BY Id DESC";
                    }
                    
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    
                    gvAboutContent.DataSource = dt;
                    gvAboutContent.DataBind();
                    
                    // Update content count
                    lblContentCount.Text = $"Total Records: {dt.Rows.Count}";
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading about content: " + ex.Message, "danger");
                    lblContentCount.Text = "Error loading content";
                }
            }
        }

        private bool CheckIfTimestampColumnsExist(SqlConnection con)
        {
            try
            {
                string checkQuery = @"SELECT COUNT(*) FROM sys.columns 
                                    WHERE object_id = OBJECT_ID('AboutContent') 
                                    AND name IN ('CreatedDate', 'ModifiedDate')";
                
                SqlCommand cmd = new SqlCommand(checkQuery, con);
                int columnCount = (int)cmd.ExecuteScalar();
                
                return columnCount == 2; // Both columns exist
            }
            catch
            {
                return false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                InsertAboutContent();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                UpdateAboutContent();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void InsertAboutContent()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    
                    // Check if timestamp columns exist
                    bool hasTimestamps = CheckIfTimestampColumnsExist(con);
                    
                    string query;
                    if (hasTimestamps)
                    {
                        query = @"INSERT INTO AboutContent (AboutText, CreatedDate, ModifiedDate) 
                                VALUES (@AboutText, GETDATE(), GETDATE())";
                    }
                    else
                    {
                        query = @"INSERT INTO AboutContent (AboutText) 
                                VALUES (@AboutText)";
                    }
                    
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@AboutText", txtAboutText.Text.Trim());
                    
                    int result = cmd.ExecuteNonQuery();
                    
                    if (result > 0)
                    {
                        ShowMessage("About content saved successfully!", "success");
                        ClearForm();
                        LoadAboutContent();
                    }
                    else
                    {
                        ShowMessage("Failed to save about content.", "danger");
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Error saving about content: " + ex.Message, "danger");
                }
            }
        }

        private void UpdateAboutContent()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    
                    // Check if timestamp columns exist
                    bool hasTimestamps = CheckIfTimestampColumnsExist(con);
                    
                    string query;
                    if (hasTimestamps)
                    {
                        query = @"UPDATE AboutContent 
                                SET AboutText = @AboutText, ModifiedDate = GETDATE() 
                                WHERE Id = @Id";
                    }
                    else
                    {
                        query = @"UPDATE AboutContent 
                                SET AboutText = @AboutText 
                                WHERE Id = @Id";
                    }
                    
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@AboutText", txtAboutText.Text.Trim());
                    cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(hdnEditId.Value));
                    
                    int result = cmd.ExecuteNonQuery();
                    
                    if (result > 0)
                    {
                        ShowMessage("About content updated successfully!", "success");
                        ClearForm();
                        LoadAboutContent();
                    }
                    else
                    {
                        ShowMessage("Failed to update about content.", "danger");
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Error updating about content: " + ex.Message, "danger");
                }
            }
        }

        private void DeleteAboutContent(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    
                    string query = "DELETE FROM AboutContent WHERE Id = @Id";
                    
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", id);
                    
                    int result = cmd.ExecuteNonQuery();
                    
                    if (result > 0)
                    {
                        ShowMessage("About content deleted successfully!", "success");
                        LoadAboutContent();
                    }
                    else
                    {
                        ShowMessage("Failed to delete about content.", "danger");
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Error deleting about content: " + ex.Message, "danger");
                }
            }
        }

        protected void gvAboutContent_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName)
            {
                case "EditContent":
                    EditContent(id);
                    break;
                case "DeleteContent":
                    DeleteAboutContent(id);
                    break;
                case "ViewContent":
                    ViewContent(id);
                    break;
            }
        }

        protected void gvAboutContent_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Add click event to view full content
                Button btnView = e.Row.FindControl("btnView") as Button;
                HiddenField hdnFullText = e.Row.FindControl("hdnFullText") as HiddenField;
                
                if (btnView != null && hdnFullText != null)
                {
                    string fullText = hdnFullText.Value.Replace("'", "\\'").Replace("\r\n", "\\n").Replace("\n", "\\n");
                    btnView.OnClientClick = $"showPreview('{fullText}'); return false;";
                }
            }
        }

        private void EditContent(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    
                    string query = "SELECT AboutText FROM AboutContent WHERE Id = @Id";
                    
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", id);
                    
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    if (reader.Read())
                    {
                        txtAboutText.Text = reader["AboutText"].ToString();
                        hdnEditId.Value = id.ToString();
                        
                        // Change form mode to edit
                        lblFormTitle.Text = "Edit About Content";
                        btnSave.Visible = false;
                        btnUpdate.Visible = true;
                        
                        ShowMessage("Content loaded for editing. Make your changes and click 'Update Content'.", "info");
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading content for editing: " + ex.Message, "danger");
                }
            }
        }

        private void ViewContent(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    
                    string query = "SELECT AboutText FROM AboutContent WHERE Id = @Id";
                    
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", id);
                    
                    object result = cmd.ExecuteScalar();
                    
                    if (result != null)
                    {
                        string aboutText = result.ToString();
                        ltlPreviewContent.Text = aboutText.Replace("\n", "<br/>");
                        
                        // Trigger modal display via JavaScript
                        ScriptManager.RegisterStartupScript(this, GetType(), "ShowPreviewModal",
                            $"showPreview('{aboutText.Replace("'", "\\'").Replace("\r\n", "\\n").Replace("\n", "\\n")}');", true);
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Error viewing content: " + ex.Message, "danger");
                }
            }
        }

        private void ClearForm()
        {
            txtAboutText.Text = "";
            hdnEditId.Value = "";
            lblFormTitle.Text = "Add New About Content";
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            pnlMessage.Visible = false;
        }

        #endregion

        #region Helper Methods

        private void ShowMessage(string message, string type)
        {
            ltlMessage.Text = message;
            pnlMessage.CssClass = $"alert alert-{type}";
            pnlMessage.Visible = true;
        }

        protected string GetTruncatedText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text))
                return "";

            if (text.Length <= maxLength)
                return text;

            return text.Substring(0, maxLength);
        }

        #endregion
    }
}
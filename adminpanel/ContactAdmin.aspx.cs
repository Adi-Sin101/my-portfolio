using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Data;

namespace adminpanel
{
    public partial class ContactAdmin : System.Web.UI.Page
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
                LoadContactMessages();
            }
        }

        #region Contact Messages CRUD Operations

        private void LoadContactMessages()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    
                    // Check if ContactMessages table exists, create if not
                    EnsureContactMessagesTableExists(con);
                    
                    // Get message statistics
                    string countQuery = "SELECT COUNT(*) as Total, SUM(CASE WHEN IsRead = 0 THEN 1 ELSE 0 END) as Unread FROM ContactMessages";
                    SqlCommand countCmd = new SqlCommand(countQuery, con);
                    SqlDataReader countReader = countCmd.ExecuteReader();
                    
                    if (countReader.Read())
                    {
                        int total = Convert.ToInt32(countReader["Total"]);
                        int unread = Convert.ToInt32(countReader["Unread"] ?? 0);
                        lblMessageCount.Text = $"Total: {total} messages | New: {unread} unread";
                        lblMessageCount.CssClass = unread > 0 ? "message-stats" : "message-stats";
                    }
                    countReader.Close();
                    
                    // Load messages for GridView
                    string query = @"SELECT Id, Name, Email, Subject, Message, DateReceived, IsRead 
                                   FROM ContactMessages 
                                   ORDER BY DateReceived DESC";
                    
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    
                    gvContactMessages.DataSource = dt;
                    gvContactMessages.DataBind();
                }
                catch (Exception ex)
                {
                    lblMessageCount.Text = "Error loading messages: " + ex.Message;
                    lblMessageCount.CssClass = "message-stats";
                    ShowMessage("Error loading contact messages: " + ex.Message, "error");
                }
            }
        }

        private void EnsureContactMessagesTableExists(SqlConnection con)
        {
            string createTableQuery = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ContactMessages' AND xtype='U')
                CREATE TABLE ContactMessages (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Name NVARCHAR(100) NOT NULL,
                    Email NVARCHAR(100) NOT NULL,
                    Subject NVARCHAR(200) NOT NULL,
                    Message NVARCHAR(MAX) NOT NULL,
                    DateReceived DATETIME DEFAULT GETDATE(),
                    IsRead BIT DEFAULT 0
                )";
            
            SqlCommand createCmd = new SqlCommand(createTableQuery, con);
            createCmd.ExecuteNonQuery();
        }

        // Refresh Messages
        protected void btnRefreshMessages_Click(object sender, EventArgs e)
        {
            LoadContactMessages();
            ShowMessage("Messages refreshed successfully!", "success");
        }

        // Add New Message
        protected void btnAddMessage_Click(object sender, EventArgs e)
        {
            // This would open a modal for adding a new message (for testing purposes)
            ShowMessage("Add new message functionality would open here", "info");
        }

        // Mark All as Read
        protected void btnMarkAllRead_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    string query = "UPDATE ContactMessages SET IsRead = 1 WHERE IsRead = 0";
                    SqlCommand cmd = new SqlCommand(query, con);
                    int affected = cmd.ExecuteNonQuery();
                    
                    LoadContactMessages(); // Refresh the display
                    ShowMessage($"Marked {affected} messages as read!", "success");
                }
                catch (Exception ex)
                {
                    ShowMessage("Error marking messages as read: " + ex.Message, "error");
                }
            }
        }

        // GridView Event Handlers
        protected void gvContactMessages_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvContactMessages.PageIndex = e.NewPageIndex;
            LoadContactMessages();
        }

        protected void gvContactMessages_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewMessage")
            {
                int messageId = Convert.ToInt32(e.CommandArgument);
                ViewMessageDetails(messageId);
            }
        }

        protected void gvContactMessages_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvContactMessages.EditIndex = e.NewEditIndex;
            LoadContactMessages();
        }

        protected void gvContactMessages_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int messageId = Convert.ToInt32(gvContactMessages.DataKeys[e.RowIndex].Value);
                GridViewRow row = gvContactMessages.Rows[e.RowIndex];
                
                string name = ((TextBox)row.FindControl("txtName")).Text.Trim();
                string email = ((TextBox)row.FindControl("txtEmail")).Text.Trim();
                string subject = ((TextBox)row.FindControl("txtSubject")).Text.Trim();
                string message = ((TextBox)row.FindControl("txtMessage")).Text.Trim();
                bool isRead = Convert.ToBoolean(((DropDownList)row.FindControl("ddlIsRead")).SelectedValue);

                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    string query = @"UPDATE ContactMessages 
                                   SET Name = @name, Email = @email, Subject = @subject, 
                                       Message = @message, IsRead = @isRead 
                                   WHERE Id = @id";
                    
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@subject", subject);
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters.AddWithValue("@isRead", isRead);
                    cmd.Parameters.AddWithValue("@id", messageId);
                    
                    cmd.ExecuteNonQuery();
                }

                gvContactMessages.EditIndex = -1;
                LoadContactMessages();
                ShowMessage("Message updated successfully!", "success");
            }
            catch (Exception ex)
            {
                ShowMessage("Error updating message: " + ex.Message, "error");
            }
        }

        protected void gvContactMessages_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvContactMessages.EditIndex = -1;
            LoadContactMessages();
        }

        protected void gvContactMessages_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int messageId = Convert.ToInt32(e.Values["Id"]);
                
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    string query = "DELETE FROM ContactMessages WHERE Id = @id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", messageId);
                    
                    cmd.ExecuteNonQuery();
                }

                LoadContactMessages();
                ShowMessage("Message deleted successfully!", "success");
            }
            catch (Exception ex)
            {
                ShowMessage("Error deleting message: " + ex.Message, "error");
            }
        }

        #endregion

        #region Helper Methods

        protected string TruncateText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return "";
            if (text.Length <= maxLength) return text;
            return text.Substring(0, maxLength) + "...";
        }

        private void ViewMessageDetails(int messageId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    string query = "SELECT * FROM ContactMessages WHERE Id = @id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", messageId);
                    
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    if (reader.Read())
                    {
                        string message = $"From: {reader["Name"]} ({reader["Email"]})\n";
                        message += $"Subject: {reader["Subject"]}\n";
                        message += $"Date: {reader["DateReceived"]}\n\n";
                        message += $"Message:\n{reader["Message"]}";
                        
                        ShowMessage("Message Details:\n" + message, "info");
                        
                        // Mark as read if it wasn't already
                        bool isRead = Convert.ToBoolean(reader["IsRead"]);
                        if (!isRead)
                        {
                            reader.Close();
                            string updateQuery = "UPDATE ContactMessages SET IsRead = 1 WHERE Id = @id";
                            SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                            updateCmd.Parameters.AddWithValue("@id", messageId);
                            updateCmd.ExecuteNonQuery();
                            
                            LoadContactMessages(); // Refresh to show updated status
                        }
                    }
                    else
                    {
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading message details: " + ex.Message, "error");
                }
            }
        }

        private void ShowMessage(string message, string type)
        {
            string alertClass = type == "success" ? "alert-success" : 
                               type == "error" ? "alert-danger" : 
                               type == "info" ? "alert-info" : "alert-warning";
            
            string script = $@"
                alert('{message.Replace("'", "\\'")}');
            ";
            ClientScript.RegisterStartupScript(this.GetType(), "ShowMessage", script, true);
        }

        #endregion
    }
}
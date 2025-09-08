using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using System.Net;

namespace adminpanel
{
    public partial class Contact : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["AdminPanelDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // No authentication required for contact page - it's public
            if (!IsPostBack)
            {
                LoadContactInfo();
            }
        }

        protected void BtnSendMessage_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtContactName.Text.Trim();
                string email = txtContactEmail.Text.Trim();
                string subject = txtContactSubject.Text.Trim();
                string message = txtContactMessage.Text.Trim();

                // Basic validation
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(message))
                {
                    ShowMessage("Please fill in all required fields.", "error");
                    return;
                }

                // Save to database
                bool saved = SaveContactMessage(name, email, subject, message);

                if (saved)
                {
                    // Clear form
                    txtContactName.Text = "";
                    txtContactEmail.Text = "";
                    txtContactSubject.Text = "";
                    txtContactMessage.Text = "";

                    ShowMessage("Thank you for your message! I'll get back to you soon.", "success");
                }
                else
                {
                    ShowMessage("There was an error sending your message. Please try again.", "error");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("An error occurred: " + ex.Message, "error");
            }
        }

        private bool SaveContactMessage(string name, string email, string subject, string message)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    // Ensure ContactMessages table exists
                    EnsureContactMessagesTableExists(con);

                    // Insert the message
                    string query = @"INSERT INTO ContactMessages (Name, Email, Subject, Message, DateReceived, IsRead) 
                                   VALUES (@name, @email, @subject, @message, GETDATE(), 0)";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@subject", subject);
                    cmd.Parameters.AddWithValue("@message", message);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                // Log error but don't expose details to user
                System.Diagnostics.Debug.WriteLine("Error saving contact message: " + ex.Message);
                return false;
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
                    Subject NVARCHAR(200) NULL,
                    Message NVARCHAR(MAX) NOT NULL,
                    DateReceived DATETIME DEFAULT GETDATE(),
                    IsRead BIT DEFAULT 0
                )";

            SqlCommand createCmd = new SqlCommand(createTableQuery, con);
            createCmd.ExecuteNonQuery();
        }

        private void LoadContactInfo()
        {
            // Load contact information from database to populate the contact cards
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();

                    // Try to get from HomeContent table
                    string query = "SELECT TOP 1 * FROM HomeContent ORDER BY Id DESC";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Update contact card links with database values
                        hlnkContactEmailCard.NavigateUrl = "mailto:" + reader["Email"].ToString();
                        hlnkContactEmailCard.Text = reader["Email"].ToString();

                        hlnkContactLinkedInCard.NavigateUrl = reader["LinkedInUrl"].ToString();
                        // Extract LinkedIn username from URL for display
                        string linkedInUrl = reader["LinkedInUrl"].ToString();
                        if (linkedInUrl.Contains("linkedin.com/in/"))
                        {
                            string username = linkedInUrl.Substring(linkedInUrl.LastIndexOf("/") + 1);
                            hlnkContactLinkedInCard.Text = username;
                        }

                        hlnkContactGitHubCard.NavigateUrl = reader["GitHubUrl"].ToString();
                        // Extract GitHub username from URL for display
                        string gitHubUrl = reader["GitHubUrl"].ToString();
                        if (gitHubUrl.Contains("github.com/"))
                        {
                            string username = gitHubUrl.Substring(gitHubUrl.LastIndexOf("/") + 1);
                            hlnkContactGitHubCard.Text = username;
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // If there's an error, use default values
                    System.Diagnostics.Debug.WriteLine("Error loading contact info: " + ex.Message);
                }
            }
        }

        private void ShowMessage(string message, string type)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = "contact-form-message " + type;
            lblMessage.Visible = true;

            // Auto-hide success messages after 5 seconds
            if (type == "success")
            {
                string script = @"
                    setTimeout(function() {
                        var msg = document.getElementById('" + lblMessage.ClientID + @"');
                        if (msg) {
                            msg.style.display = 'none';
                        }
                    }, 5000);
                ";
                ClientScript.RegisterStartupScript(this.GetType(), "HideMessage", script, true);
            }
        }
    }
}
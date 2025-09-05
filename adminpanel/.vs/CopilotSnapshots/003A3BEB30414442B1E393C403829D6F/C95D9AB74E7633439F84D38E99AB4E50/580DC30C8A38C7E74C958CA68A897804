using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace adminpanel
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // If already logged in, redirect to dashboard
            if (Session["AdminUser"] != null)
            {
                Response.Redirect("Default.aspx");
            }
        }
        
        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            // Clear previous message
            lblMessage.Visible = false;
            lblMessage.Text = "";
            
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowMessage("Please enter both username and password.", "danger");
                return;
            }

            string cs = ConfigurationManager.ConnectionStrings["AdminPanelDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    
                    // Query your AdminUsers table with Password column
                    string query = "SELECT Id FROM AdminUsers WHERE Username=@u AND Password=@p";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", password);

                    object result = cmd.ExecuteScalar();
                    
                    if (result != null)
                    {
                        // Login successful - update LastLogin
                        string updateQuery = "UPDATE AdminUsers SET LastLogin = GETDATE() WHERE Username = @u";
                        SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                        updateCmd.Parameters.AddWithValue("@u", username);
                        updateCmd.ExecuteNonQuery();
                        
                        // Set session and redirect
                        Session["AdminUser"] = username;
                        Response.Redirect("Default.aspx");
                        return;
                    }
                    else
                    {
                        ShowMessage("Invalid username or password!", "danger");
                    }
                }
                catch (SqlException sqlEx)
                {
                    if (sqlEx.Message.Contains("Invalid object name"))
                    {
                        ShowMessage("AdminUsers table not found. Please check your database setup.", "warning");
                    }
                    else
                    {
                        ShowMessage("Database error: " + sqlEx.Message, "danger");
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Connection error: " + ex.Message, "danger");
                }
            }
        }
        
        private void ShowMessage(string message, string type)
        {
            lblMessage.Text = message;
            lblMessage.Visible = true;
            
            // Add appropriate CSS class based on message type
            switch (type)
            {
                case "success":
                    lblMessage.CssClass = "alert alert-success";
                    break;
                case "warning":
                    lblMessage.CssClass = "alert alert-warning";
                    break;
                case "danger":
                default:
                    lblMessage.CssClass = "alert alert-danger";
                    break;
            }
        }
    }
}
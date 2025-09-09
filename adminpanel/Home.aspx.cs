using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Net.Mail;
using System.Net;

namespace adminpanel
{
    public partial class Home : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["AdminPanelDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPersonalInfo();
                LoadEducation();
                LoadSkills();
                LoadExperience();
                LoadProjects();
            }
        }

        // Contact form submit handler
        protected void BtnSendMessage_Click(object sender, EventArgs e)
        {
            try
            {
                // Get form data - check if controls exist first
                string name = "";
                string email = "";
                string subject = "";
                string message = "";

                // Safely get control values
                var nameControl = FindControl("txtContactName") as TextBox;
                var emailControl = FindControl("txtContactEmail") as TextBox;
                var subjectControl = FindControl("txtContactSubject") as TextBox;
                var messageControl = FindControl("txtContactMessage") as TextBox;

                if (nameControl != null) name = nameControl.Text.Trim();
                if (emailControl != null) email = emailControl.Text.Trim();
                if (subjectControl != null) subject = subjectControl.Text.Trim();
                if (messageControl != null) message = messageControl.Text.Trim();

                // Basic validation
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || 
                    string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(message))
                {
                    ShowMessage("Please fill in all fields.", "error");
                    return;
                }
                
                // Email validation
                if (!IsValidEmail(email))
                {
                    ShowMessage("Please enter a valid email address.", "error");
                    return;
                }
                
                // Get recipient email from ContactInfo table
                string recipientEmail = GetPrimaryEmailFromContactInfo();
                
                // Save to database
                bool dbSaved = SaveContactMessage(name, email, subject, message);
                
                // Send email notification
                bool emailSent = SendEmailNotification(name, email, subject, message, recipientEmail);
                
                // Clear form if controls exist
                if (nameControl != null) nameControl.Text = "";
                if (emailControl != null) emailControl.Text = "";
                if (subjectControl != null) subjectControl.Text = "";
                if (messageControl != null) messageControl.Text = "";
                
                if (emailSent)
                {
                    ShowMessage("Thank you for your message! I'll get back to you soon.", "success");
                }
                else if (dbSaved)
                {
                    ShowMessage("Your message has been saved. Email sending is not configured yet.", "warning");
                }
                else
                {
                    ShowMessage("Sorry, there was an error processing your message. Please try again.", "error");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Sorry, there was an error sending your message. Please try again.", "error");
                System.Diagnostics.Debug.WriteLine("Contact form error: " + ex.Message);
            }
        }

        private string GetPrimaryEmailFromContactInfo()
        {
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
                    
                    if (result != null && !string.IsNullOrEmpty(result.ToString()))
                    {
                        return result.ToString();
                    }
                    
                    // Fallback to HomeContent email
                    string fallbackQuery = "SELECT TOP 1 Email FROM HomeContent WHERE Email IS NOT NULL AND Email != '' ORDER BY Id DESC";
                    SqlCommand fallbackCmd = new SqlCommand(fallbackQuery, con);
                    object fallbackResult = fallbackCmd.ExecuteScalar();
                    
                    return fallbackResult?.ToString() ?? "";
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error getting primary email: " + ex.Message);
                    return "";
                }
            }
        }
        
        private bool SaveContactMessage(string name, string email, string subject, string message)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    
                    // Create ContactMessages table if it doesn't exist
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
                    
                    // Insert the message
                    string insertQuery = @"INSERT INTO ContactMessages (Name, Email, Subject, Message) 
                                         VALUES (@Name, @Email, @Subject, @Message)";
                    
                    SqlCommand cmd = new SqlCommand(insertQuery, con);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Subject", subject);
                    cmd.Parameters.AddWithValue("@Message", message);
                    
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error saving contact message: " + ex.Message);
                    return false;
                }
            }
        }
        
        private bool SendEmailNotification(string name, string senderEmail, string subject, string message, string recipientEmail)
        {
            try
            {
                // Check if recipient email is configured
                if (string.IsNullOrEmpty(recipientEmail) || recipientEmail == "your.email@example.com")
                {
                    System.Diagnostics.Debug.WriteLine("No valid recipient email configured in ContactInfo table");
                    return false;
                }

                // Get email configuration from web.config or use default values
                string smtpServer = ConfigurationManager.AppSettings["SMTPServer"] ?? "smtp.gmail.com";
                string smtpPort = ConfigurationManager.AppSettings["SMTPPort"] ?? "587";
                string adminEmail = ConfigurationManager.AppSettings["AdminEmail"] ?? recipientEmail;
                string adminPassword = ConfigurationManager.AppSettings["AdminPassword"] ?? "";
                
                // If no admin password is configured, return false
                if (string.IsNullOrEmpty(adminPassword))
                {
                    System.Diagnostics.Debug.WriteLine("No email password configured in web.config");
                    return false;
                }
                
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(adminEmail, "Portfolio Contact Form");
                mail.To.Add(recipientEmail); // Send to email from ContactInfo table
                mail.Subject = $"Portfolio Contact: {subject}";
                
                string emailBody = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif;'>
                        <div style='max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px;'>
                            <h2 style='color: #a78bfa; text-align: center;'>New Contact Form Message</h2>
                            <div style='background: #f8f9fa; padding: 20px; border-radius: 8px; margin: 20px 0;'>
                                <p><strong>Name:</strong> {name}</p>
                                <p><strong>Email:</strong> {senderEmail}</p>
                                <p><strong>Subject:</strong> {subject}</p>
                                <p><strong>Sent to:</strong> {recipientEmail}</p>
                            </div>
                            <div style='background: #fff; padding: 20px; border: 1px solid #e9ecef; border-radius: 8px;'>
                                <h4>Message:</h4>
                                <p style='line-height: 1.6;'>{message.Replace("\n", "<br>")}</p>
                            </div>
                            <div style='text-align: center; margin-top: 20px; color: #6c757d; font-size: 12px;'>
                                <p>This message was sent from your portfolio contact form.</p>
                                <p>Reply directly to: {senderEmail}</p>
                            </div>
                        </div>
                    </body>
                    </html>";
                
                mail.Body = emailBody;
                mail.IsBodyHtml = true;
                mail.ReplyToList.Add(senderEmail); // Allow easy reply to sender
                
                SmtpClient smtp = new SmtpClient(smtpServer, int.Parse(smtpPort));
                smtp.Credentials = new NetworkCredential(adminEmail, adminPassword);
                smtp.EnableSsl = true;
                
                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error sending email: " + ex.Message);
                return false;
            }
        }
        
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        
        private void ShowMessage(string message, string type)
        {
            // Add a script to show a message to the user
            string alertClass = type == "success" ? "alert-success" : 
                               type == "warning" ? "alert-warning" : "alert-danger";
            string bgColor = type == "success" ? "#d4edda" : 
                            type == "warning" ? "#fff3cd" : "#f8d7da";
            string textColor = type == "success" ? "#155724" : 
                              type == "warning" ? "#856404" : "#721c24";
            string borderColor = type == "success" ? "#c3e6cb" : 
                                type == "warning" ? "#ffeaa7" : "#f5c6cb";
            
            string script = $@"
                document.addEventListener('DOMContentLoaded', function() {{
                    var alertDiv = document.createElement('div');
                    alertDiv.className = 'alert {alertClass}';
                    alertDiv.style.cssText = 'position: fixed; top: 100px; right: 20px; z-index: 10000; padding: 15px; border-radius: 8px; max-width: 400px; box-shadow: 0 4px 12px rgba(0,0,0,0.15); background-color: {bgColor}; color: {textColor}; border: 1px solid {borderColor}; font-family: Arial, sans-serif;';
                    alertDiv.innerHTML = '{message.Replace("'", "\\'")}';
                    document.body.appendChild(alertDiv);
                    
                    // Fade in
                    alertDiv.style.opacity = '0';
                    alertDiv.style.transform = 'translateX(100%)';
                    alertDiv.style.transition = 'opacity 0.3s ease, transform 0.3s ease';
                    
                    setTimeout(function() {{
                        alertDiv.style.opacity = '1';
                        alertDiv.style.transform = 'translateX(0)';
                    }}, 100);
                    
                    // Fade out after 5 seconds
                    setTimeout(function() {{
                        alertDiv.style.opacity = '0';
                        alertDiv.style.transform = 'translateX(100%)';
                        setTimeout(function() {{
                            if (document.body.contains(alertDiv)) {{
                                document.body.removeChild(alertDiv);
                            }}
                        }}, 300);
                    }}, 5000);
                }});
            ";
            
            ClientScript.RegisterStartupScript(this.GetType(), "ShowMessage", script, true);
        }

        private void LoadPersonalInfo()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    
                    // Load from HomeContent table where About.aspx saves the data
                    string query = "SELECT TOP 1 * FROM HomeContent ORDER BY Id DESC";
                    
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    if (reader.Read())
                    {
                        // Map HomeContent columns to display fields
                        string heroHeading = reader["HeroHeading"].ToString();
                        string roleText = reader["RoleText"].ToString();
                        string description = reader["Description"].ToString();
                        string email = reader["Email"].ToString();
                        string phone = reader["Phone"].ToString();
                        string gitHubUrl = reader["GitHubUrl"].ToString();
                        string linkedInUrl = reader["LinkedInUrl"].ToString();
                        string imagePath = reader["ImagePath"].ToString();
                        string resumeUrl = reader["Resume"] != DBNull.Value ? reader["Resume"].ToString() : "";
                        
                        // Populate all the display controls
                        ltlNavName.Text = heroHeading;
                        ltlHeroName.Text = heroHeading;
                        ltlName.Text = heroHeading;
                        ltlRole.Text = roleText;
                        ltlHeroDescription.Text = description;
                        
                        // Social links
                        hlnkGitHub.NavigateUrl = !string.IsNullOrEmpty(gitHubUrl) ? gitHubUrl : "#";
                        hlnkLinkedIn.NavigateUrl = !string.IsNullOrEmpty(linkedInUrl) ? linkedInUrl : "#";
                        hlnkEmail.NavigateUrl = !string.IsNullOrEmpty(email) ? "mailto:" + email : "#";
                        hlnkPhone.NavigateUrl = !string.IsNullOrEmpty(phone) ? "tel:" + phone : "#";
                        hlnkResume.NavigateUrl = !string.IsNullOrEmpty(resumeUrl) ? resumeUrl : "#";
                        
                        // Profile image
                        imgHero.ImageUrl = !string.IsNullOrEmpty(imagePath) ? imagePath : "Images/default-profile.jpg";
                        
                        reader.Close();
                        
                        // Load About Me content from AboutContent table
                        LoadAboutContent(con);
                        
                        // Load contact information from ContactInfo table
                        LoadContactInfoFromDatabase(con, email, phone, linkedInUrl, gitHubUrl);
                    }
                    else
                    {
                        reader.Close();
                        // No data found in HomeContent, use defaults
                        SetDefaultPersonalInfo();
                    }
                }
                catch (Exception ex)
                {
                    // Use default values if database query fails
                    SetDefaultPersonalInfo();
                }
            }
        }

        private void LoadAboutContent(SqlConnection con)
        {
            try
            {
                // Load About Me content from AboutContent table
                string aboutQuery = "SELECT TOP 1 AboutText FROM AboutContent ORDER BY ModifiedDate DESC, Id DESC";
                
                SqlCommand aboutCmd = new SqlCommand(aboutQuery, con);
                object aboutResult = aboutCmd.ExecuteScalar();
                
                if (aboutResult != null && !string.IsNullOrEmpty(aboutResult.ToString()))
                {
                    // Use content from AboutContent table
                    ltlAboutDescription.Text = aboutResult.ToString();
                    System.Diagnostics.Debug.WriteLine("About content loaded from AboutContent table");
                }
                else
                {
                    // Fall back to HomeContent description if AboutContent is empty
                    string fallbackQuery = "SELECT TOP 1 Description FROM HomeContent ORDER BY Id DESC";
                    SqlCommand fallbackCmd = new SqlCommand(fallbackQuery, con);
                    object fallbackResult = fallbackCmd.ExecuteScalar();
                    
                    if (fallbackResult != null && !string.IsNullOrEmpty(fallbackResult.ToString()))
                    {
                        ltlAboutDescription.Text = fallbackResult.ToString();
                        System.Diagnostics.Debug.WriteLine("About content loaded from HomeContent table (fallback)");
                    }
                    else
                    {
                        ltlAboutDescription.Text = "Please add your About Me content through the Admin Panel > About Content section.";
                        System.Diagnostics.Debug.WriteLine("Using default About content message");
                    }
                }
            }
            catch (Exception ex)
            {
                // If there's an error, use a default message
                ltlAboutDescription.Text = "Please add your About Me content through the Admin Panel > About Content section.";
                System.Diagnostics.Debug.WriteLine("Error loading About content: " + ex.Message);
            }
        }
        
        private void LoadContactInfoFromDatabase(SqlConnection con, string fallbackEmail, string fallbackPhone, string fallbackLinkedIn, string fallbackGitHub)
        {
            try
            {
                // Load contact information from ContactInfo table
                string contactQuery = @"SELECT Email, LinkedIn, GitHub, Phone 
                                      FROM ContactInfo 
                                      ORDER BY display_order, Id";
                
                SqlCommand contactCmd = new SqlCommand(contactQuery, con);
                SqlDataReader contactReader = contactCmd.ExecuteReader();
                
                // Default values
                string primaryEmail = fallbackEmail;
                string primaryPhone = fallbackPhone;
                string primaryLinkedIn = fallbackLinkedIn;
                string primaryGitHub = fallbackGitHub;
                
                // Use first record from ContactInfo if available
                if (contactReader.Read())
                {
                    if (!string.IsNullOrEmpty(contactReader["Email"].ToString()))
                        primaryEmail = contactReader["Email"].ToString();
                    if (!string.IsNullOrEmpty(contactReader["Phone"].ToString()))
                        primaryPhone = contactReader["Phone"].ToString();
                    if (!string.IsNullOrEmpty(contactReader["LinkedIn"].ToString()))
                        primaryLinkedIn = contactReader["LinkedIn"].ToString();
                    if (!string.IsNullOrEmpty(contactReader["GitHub"].ToString()))
                        primaryGitHub = contactReader["GitHub"].ToString();
                }
                contactReader.Close();
                
                // If we still don't have good data, use sensible defaults
                if (string.IsNullOrEmpty(primaryEmail) || primaryEmail == "your.email@example.com")
                    primaryEmail = "adiba0tahsin@gmail.com";
                if (string.IsNullOrEmpty(primaryLinkedIn))
                    primaryLinkedIn = "https://www.linkedin.com/in/adiba-tahsin-985b452a2";
                if (string.IsNullOrEmpty(primaryGitHub))
                    primaryGitHub = "https://github.com/Adi-Sin101";
                if (string.IsNullOrEmpty(primaryPhone))
                    primaryPhone = "+1 (555) 123-4567";
                
                // Safely populate contact cards - check if controls exist first
                PopulateContactCard("hlnkContactEmailCard", primaryEmail, "mailto:");
                PopulateContactCard("hlnkContactLinkedInCard", ExtractLinkedInUsername(primaryLinkedIn), primaryLinkedIn);
                PopulateContactCard("hlnkContactGitHubCard", ExtractGitHubUsername(primaryGitHub), primaryGitHub);
                
                // Safely populate contact details
                PopulateContactDetail("ltlContactEmailDetail", primaryEmail);
                PopulateContactDetail("ltlContactPhoneDetail", primaryPhone);
                
                System.Diagnostics.Debug.WriteLine($"Contact Info Loaded - Email: {primaryEmail}, LinkedIn: {primaryLinkedIn}, GitHub: {primaryGitHub}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error loading contact info: " + ex.Message);
                
                // Set default data if there's an error
                PopulateContactCard("hlnkContactEmailCard", "adiba0tahsin@gmail.com", "mailto:");
                PopulateContactCard("hlnkContactLinkedInCard", "adiba-tahsin-985b452a2", "https://www.linkedin.com/in/adiba-tahsin-985b452a2");
                PopulateContactCard("hlnkContactGitHubCard", "Adi-Sin101", "https://github.com/Adi-Sin101");
                PopulateContactDetail("ltlContactEmailDetail", "adiba0tahsin@gmail.com");
                PopulateContactDetail("ltlContactPhoneDetail", "+1 (555) 123-4567");
            }
        }

        private string ExtractLinkedInUsername(string linkedInUrl)
        {
            if (string.IsNullOrEmpty(linkedInUrl)) return "LinkedIn Profile";
            
            // Extract username from LinkedIn URL
            if (linkedInUrl.Contains("/in/"))
            {
                string[] parts = linkedInUrl.Split('/');
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i] == "in" && i + 1 < parts.Length)
                    {
                        return parts[i + 1];
                    }
                }
            }
            
            return "LinkedIn Profile";
        }

        private string ExtractGitHubUsername(string gitHubUrl)
        {
            if (string.IsNullOrEmpty(gitHubUrl)) return "GitHub Profile";
            
            // Extract username from GitHub URL
            if (gitHubUrl.Contains("github.com/"))
            {
                string[] parts = gitHubUrl.Split('/');
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i] == "github.com" && i + 1 < parts.Length)
                    {
                        return parts[i + 1];
                    }
                }
            }
            
            return "GitHub Profile";
        }

        private void PopulateContactCard(string controlId, string value, string urlPrefix)
        {
            var control = FindControl(controlId) as HyperLink;
            if (control != null)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    control.Text = value;
                    // For LinkedIn and GitHub, the urlPrefix might be the full URL
                    if (urlPrefix.StartsWith("http"))
                    {
                        control.NavigateUrl = urlPrefix;
                    }
                    else
                    {
                        control.NavigateUrl = urlPrefix + value;
                    }
                }
                else
                {
                    control.Text = GetDefaultText(controlId);
                    control.NavigateUrl = "#";
                }
                
                System.Diagnostics.Debug.WriteLine($"Populated {controlId}: Text='{control.Text}', URL='{control.NavigateUrl}'");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Control {controlId} not found!");
            }
        }

        private void LoadEducation()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    
                    // Query your Education table with the correct columns: Id, Degree, Institution, Year, Grade
                    string query = "SELECT Id, Degree, Institution, Year, Grade FROM Education ORDER BY Id DESC";
                    
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    
                    // Add YearInfo column for display formatting
                    if (!dt.Columns.Contains("YearInfo"))
                    {
                        dt.Columns.Add("YearInfo", typeof(string));
                    }
                    
                    // Process the data for correct display format
                    foreach (DataRow row in dt.Rows)
                    {
                        string degree = row["Degree"].ToString();
                        string institution = row["Institution"].ToString();
                        string year = row["Year"].ToString();
                        string grade = row["Grade"].ToString();
                        
                        // Format based on degree type
                        if (degree.Contains("B.Sc") || degree.Contains("Bachelor"))
                        {
                            // For ongoing Bachelor's degree - show Expected year
                            if (year.Contains("Expected") || year.Contains("2027") || string.IsNullOrEmpty(year))
                            {
                                row["YearInfo"] = "2027"; // Expected year only
                                row["Grade"] = grade.StartsWith("CGPA") ? grade : "CGPA: " + grade;
                            }
                            else
                            {
                                row["YearInfo"] = "";
                                row["Grade"] = grade.StartsWith("CGPA") ? grade : "CGPA: " + grade;
                            }
                        }
                        else if (degree.Contains("HSC") || degree.Contains("SSC"))
                        {
                            // For HSC/SSC - add year to institution
                            if (!institution.Contains(year) && !string.IsNullOrEmpty(year))
                            {
                                row["Institution"] = institution + " | " + year;
                            }
                            row["YearInfo"] = ""; // No separate year info
                            row["Grade"] = grade.StartsWith("GPA") ? grade : "GPA: " + grade;
                        }
                        else
                        {
                            // For other degrees
                            row["YearInfo"] = "";
                            row["Grade"] = grade.StartsWith("GPA") ? grade : "GPA: " + grade;
                        }
                    }
                    
                    if (dt.Rows.Count == 0)
                    {
                        // Add sample education data if no records found
                        dt = CreateSampleEducationData();
                    }
                    
                    rptEducation.DataSource = dt;
                    rptEducation.DataBind();
                }
                catch (Exception ex)
                {
                    // Handle error with sample data
                    DataTable errorDt = CreateSampleEducationData();
                    rptEducation.DataSource = errorDt;
                    rptEducation.DataBind();
                    
                    System.Diagnostics.Debug.WriteLine("Education error: " + ex.Message);
                }
            }
        }

        private DataTable CreateSampleEducationData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Degree");
            dt.Columns.Add("Institution");
            dt.Columns.Add("Year");
            dt.Columns.Add("Grade");
            dt.Columns.Add("YearInfo");
            
            // B.Sc in Computer Science (Ongoing)
            DataRow row1 = dt.NewRow();
            row1["Id"] = 1;
            row1["Degree"] = "B.Sc in Computer Science";
            row1["Institution"] = "Khulna University of Engineering & Technology";
            row1["Year"] = "Expected: 2027";
            row1["Grade"] = "CGPA: 3.28 / 4.00";
            row1["YearInfo"] = "2027";
            dt.Rows.Add(row1);
            
            // HSC (Completed)
            DataRow row2 = dt.NewRow();
            row2["Id"] = 2;
            row2["Degree"] = "HSC";
            row2["Institution"] = "Khulna Govt. Girls' College | 2021";
            row2["Year"] = "2021";
            row2["Grade"] = "GPA: 5.00";
            row2["YearInfo"] = "";
            dt.Rows.Add(row2);
            
            // SSC (Completed)
            DataRow row3 = dt.NewRow();
            row3["Id"] = 3;
            row3["Degree"] = "SSC";
            row3["Institution"] = "Govt. Coronation Secondary Girls' School | 2019";
            row3["Year"] = "2019";
            row3["Grade"] = "GPA: 5.00";
            row3["YearInfo"] = "";
            dt.Rows.Add(row3);
            
            return dt;
        }

        private void LoadSkills()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    // Check if columns exist first
                    string checkColumnsQuery = @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS 
                                       WHERE TABLE_NAME = 'Skills' AND COLUMN_NAME IN ('IsActive', 'DisplayOrder', 'Percentage', 'IconUrl')";
                    SqlCommand checkCmd = new SqlCommand(checkColumnsQuery, con);
                    int columnsExist = (int)checkCmd.ExecuteScalar();
                    
                    string query;
                    if (columnsExist >= 2) // At least some of the columns exist
                    {
                        query = @"SELECT Name, 
                                COALESCE(IconUrl, 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/html5/html5-original.svg') as IconUrl, 
                                COALESCE(Percentage, 75) as Percentage 
                        FROM Skills 
                        ORDER BY COALESCE(DisplayOrder, 0), Name";
                    }
                    else
                    {
                        // Basic query for original table structure
                        query = "SELECT Name FROM Skills ORDER BY Name";
                    }
                    
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    
                    // Add missing columns if needed
                    if (!dt.Columns.Contains("IconUrl"))
                    {
                        dt.Columns.Add("IconUrl", typeof(string));
                    }
                    if (!dt.Columns.Contains("Percentage"))
                    {
                        dt.Columns.Add("Percentage", typeof(int));
                    }
                    
                    // Fill missing data
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["IconUrl"] == DBNull.Value || string.IsNullOrEmpty(row["IconUrl"].ToString()))
                        {
                            string skillName = row["Name"].ToString().ToLower();
                            if (skillName.Contains("html"))
                                row["IconUrl"] = "https://cdn.jsdelivr.net/gh/devicons/devicon/icons/html5/html5-original.svg";
                            else if (skillName.Contains("css"))
                                row["IconUrl"] = "https://cdn.jsdelivr.net/gh/devicons/devicon/icons/css3/css3-original.svg";
                            else if (skillName.Contains("javascript") || skillName.Contains("js"))
                                row["IconUrl"] = "https://cdn.jsdelivr.net/gh/devicons/devicon/icons/javascript/javascript-original.svg";
                            else if (skillName.Contains("react"))
                                row["IconUrl"] = "https://cdn.jsdelivr.net/gh/devicons/devicon/icons/react/react-original.svg";
                            else if (skillName.Contains("python"))
                                row["IconUrl"] = "https://cdn.jsdelivr.net/gh/devicons/devicon/icons/python/python-original.svg";
                            else if (skillName.Contains("java"))
                                row["IconUrl"] = "https://cdn.jsdelivr.net/gh/devicons/devicon/icons/java/java-original.svg";
                            else
                                row["IconUrl"] = "https://cdn.jsdelivr.net/gh/devicons/devicon/icons/html5/html5-original.svg";
                        }
                        
                        if (row["Percentage"] == DBNull.Value || Convert.ToInt32(row["Percentage"]) == 0)
                        {
                            row["Percentage"] = 75; // Default percentage
                        }
                    }
                    
                    if (dt.Rows.Count == 0)
                    {
                        // Add sample skills data
                        dt.Columns.Add("Name");
                        dt.Columns.Add("IconUrl");
                        dt.Columns.Add("Percentage");
                        
                        var skills = new[]
                        {
                            new { Name = "HTML5", Icon = "https://cdn.jsdelivr.net/gh/devicons/devicon/icons/html5/html5-original.svg", Percentage = 90 },
                            new { Name = "CSS3", Icon = "https://cdn.jsdelivr.net/gh/devicons/devicon/icons/css3/css3-original.svg", Percentage = 85 },
                            new { Name = "JavaScript", Icon = "https://cdn.jsdelivr.net/gh/devicons/devicon/icons/javascript/javascript-original.svg", Percentage = 75 },
                            new { Name = "React", Icon = "https://cdn.jsdelivr.net/gh/devicons/devicon/icons/react/react-original.svg", Percentage = 70 },
                            new { Name = "Python", Icon = "https://cdn.jsdelivr.net/gh/devicons/devicon/icons/python/python-original.svg", Percentage = 80 }
                        };
                        
                        foreach (var skill in skills)
                        {
                            DataRow row = dt.NewRow();
                            row["Name"] = skill.Name;
                            row["IconUrl"] = skill.Icon;
                            row["Percentage"] = skill.Percentage;
                            dt.Rows.Add(row);
                        }
                    }
                    
                    rptSkills.DataSource = dt;
                    rptSkills.DataBind();
                }
                catch (Exception ex)
                {
                    // Handle error with sample data
                    DataTable errorDt = new DataTable();
                    errorDt.Columns.Add("Name");
                    errorDt.Columns.Add("IconUrl");
                    errorDt.Columns.Add("Percentage");
                    
                    DataRow errorRow = errorDt.NewRow();
                    errorRow["Name"] = "Error: " + ex.Message;
                    errorRow["IconUrl"] = "https://cdn.jsdelivr.net/gh/devicons/devicon/icons/html5/html5-original.svg";
                    errorRow["Percentage"] = 0;
                    errorDt.Rows.Add(errorRow);
                    
                    rptSkills.DataSource = errorDt;
                    rptSkills.DataBind();
                }
            }
        }

        private void LoadExperience()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    
                    string query = @"SELECT Title, Company, StartDate, EndDate, Description, ImagePath 
                                   FROM Experience 
                                   ORDER BY Id DESC";
                    
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    
                    // Process the data for display
                    DataTable displayTable = new DataTable();
                    displayTable.Columns.Add("Title");
                    displayTable.Columns.Add("Company");
                    displayTable.Columns.Add("Duration");
                    displayTable.Columns.Add("Description");
                    displayTable.Columns.Add("ImagePath");
                    
                    foreach (DataRow row in dt.Rows)
                    {
                        DataRow newRow = displayTable.NewRow();
                        newRow["Title"] = row["Title"]?.ToString() ?? "";
                        newRow["Company"] = row["Company"]?.ToString() ?? "";
                        newRow["Description"] = row["Description"]?.ToString() ?? "";
                        
                        // Handle ImagePath
                        string imagePath = row["ImagePath"]?.ToString();
                        newRow["ImagePath"] = string.IsNullOrEmpty(imagePath) ? "default-experience.png" : imagePath;
                        
                        // Calculate Duration from dates
                        string startDateStr = row["StartDate"]?.ToString();
                        string endDateStr = row["EndDate"]?.ToString();
                        string duration = "Duration not specified";
                        
                        if (!string.IsNullOrEmpty(startDateStr))
                        {
                            try
                            {
                                DateTime startDate = DateTime.Parse(startDateStr);
                                duration = startDate.ToString("MMM yyyy");
                                
                                if (!string.IsNullOrEmpty(endDateStr))
                                {
                                    DateTime endDate = DateTime.Parse(endDateStr);
                                    duration += " - " + endDate.ToString("MMM yyyy");
                                }
                                else
                                {
                                    duration += " - Present";
                                }
                            }
                            catch
                            {
                                duration = startDateStr + (!string.IsNullOrEmpty(endDateStr) ? " - " + endDateStr : " - Present");
                            }
                        }
                        
                        newRow["Duration"] = duration;
                        displayTable.Rows.Add(newRow);
                    }
                    
                    // Always bind the data
                    rptExperience.DataSource = displayTable;
                    rptExperience.DataBind();
                    
                    // Show/hide the no data message
                    noExperience.Visible = displayTable.Rows.Count == 0;
                }
            }
            catch (Exception)
            {
                // If there's an error, show the no data message
                rptExperience.DataSource = null;
                rptExperience.DataBind();
                noExperience.Visible = true;
            }
        }

        private void LoadProjects()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    
                    // Query with correct column name ImagePath
                    string query = "SELECT Title, Description, ImagePath, TechUsed, GitUrl FROM Projects ORDER BY Id DESC";
                    
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    
                    // Fill missing values and ensure proper image paths
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["ImagePath"] == DBNull.Value || string.IsNullOrEmpty(row["ImagePath"].ToString()))
                            row["ImagePath"] = "default-project.png";
                        if (row["TechUsed"] == DBNull.Value || string.IsNullOrEmpty(row["TechUsed"].ToString()))
                            row["TechUsed"] = "Various Technologies";
                        if (row["GitUrl"] == DBNull.Value || string.IsNullOrEmpty(row["GitUrl"].ToString()))
                            row["GitUrl"] = "#";
                    }
                    
                    if (dt.Rows.Count == 0)
                    {
                        CreateSampleProjects();
                    }
                    else
                    {
                        rptProjects.DataSource = dt;
                        rptProjects.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    // Show error with debug info
                    DataTable errorDt = new DataTable();
                    errorDt.Columns.Add("Title");
                    errorDt.Columns.Add("Description");
                    errorDt.Columns.Add("ImagePath");
                    errorDt.Columns.Add("TechUsed");
                    errorDt.Columns.Add("GitUrl");
                    
                    DataRow errorRow = errorDt.NewRow();
                    errorRow["Title"] = "Database Connection Error";
                    errorRow["Description"] = "Error loading projects from database: " + ex.Message;
                    errorRow["ImagePath"] = "default-project.png";
                    errorRow["TechUsed"] = "Debug Info";
                    errorRow["GitUrl"] = "#";
                    errorDt.Rows.Add(errorRow);
                    
                    rptProjects.DataSource = errorDt;
                    rptProjects.DataBind();
                }
            }
        }

        private void CreateSampleProjects()
        {
            // Add sample project data
            DataTable dt = new DataTable();
            dt.Columns.Add("Title");
            dt.Columns.Add("Description");
            dt.Columns.Add("ImagePath");
            dt.Columns.Add("TechUsed");
            dt.Columns.Add("GitUrl");
            
            var projects = new[]
            {
                new { Title = "Portfolio Website", 
                      Description = "A personal portfolio website to showcase my projects, skills, and experience. Built with HTML, CSS, and JavaScript for a modern, responsive design.", 
                      Image = "portfilo.png", Tech = "HTML, CSS, JavaScript", Git = "https://github.com/Adi-Sin101/my-portfolio.git" },
                new { Title = "PawPal (Petcare App)", 
                      Description = "PawPal is an Android app that helps pet owners manage daily activities, health records, expenses, and vet contacts.", 
                      Image = "pawpal.jpeg", Tech = "Android, Java, Firebase", Git = "https://github.com/Adi-Sin101/PawPal_Android.git" },
                new { Title = "Database Test", 
                      Description = "If you see this, your Projects table is empty or doesn't exist. Add projects through the admin panel!", 
                      Image = "default-project.png", Tech = "Database, Debugging", Git = "#" }
            };
            
            foreach (var project in projects)
            {
                DataRow row = dt.NewRow();
                row["Title"] = project.Title;
                row["Description"] = project.Description;
                row["ImagePath"] = project.Image;
                row["TechUsed"] = project.Tech;
                row["GitUrl"] = project.Git;
                dt.Rows.Add(row);
            }
            
            rptProjects.DataSource = dt;
            rptProjects.DataBind();
        }

        private bool TableExists(SqlConnection connection, string tableName)
        {
            string query = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @tableName";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@tableName", tableName);
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }

        private void SetDefaultPersonalInfo()
        {
            ltlNavName.Text = "Your Name";
            ltlHeroName.Text = "Your Name";
            ltlName.Text = "Your Name";
            ltlRole.Text = "Your Role";
            ltlHeroDescription.Text = "Please update your personal information in the Admin Panel > About section.";
            
            // Load About Me content from AboutContent table even when using defaults
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    LoadAboutContent(con);
                }
                catch (Exception ex)
                {
                    ltlAboutDescription.Text = "Please add your About Me content through the Admin Panel > About Content section.";
                    System.Diagnostics.Debug.WriteLine("Error loading About content in SetDefaultPersonalInfo: " + ex.Message);
                }
            }
            
            // Default social links
            hlnkGitHub.NavigateUrl = "#";
            hlnkLinkedIn.NavigateUrl = "#";
            hlnkEmail.NavigateUrl = "#";
            hlnkPhone.NavigateUrl = "#";
            hlnkResume.NavigateUrl = "#";
            
            // Default profile image
            imgHero.ImageUrl = "Images/default-profile.jpg";
            
            // Ensure ContactInfo table exists and has default data
            EnsureContactInfoTableWithDefaults();
            
            // Safely populate contact cards with defaults from your VS Code portfolio
            PopulateContactCard("hlnkContactEmailCard", "adiba0tahsin@gmail.com", "mailto:");
            PopulateContactCard("hlnkContactLinkedInCard", "adiba-tahsin-985b452a2", "https://www.linkedin.com/in/adiba-tahsin-985b452a2");
            PopulateContactCard("hlnkContactGitHubCard", "Adi-Sin101", "https://github.com/Adi-Sin101");
            
            // Safely populate contact details with defaults
            PopulateContactDetail("ltlContactEmailDetail", "adiba0tahsin@gmail.com");
            PopulateContactDetail("ltlContactPhoneDetail", "+1 (555) 123-4567");
        }

        private void EnsureContactInfoTableWithDefaults()
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
                        END";
                    
                    SqlCommand createCmd = new SqlCommand(createTableQuery, con);
                    createCmd.ExecuteNonQuery();
                    
                    // Check if there's any data in the table
                    string checkDataQuery = "SELECT COUNT(*) FROM ContactInfo";
                    SqlCommand checkCmd = new SqlCommand(checkDataQuery, con);
                    int recordCount = (int)checkCmd.ExecuteScalar();
                    
                    // If no data exists, insert default data matching your VS Code portfolio
                    if (recordCount == 0)
                    {
                        string insertDefaultQuery = @"
                            INSERT INTO ContactInfo (Email, LinkedIn, GitHub, Phone, icon_class, display_order)
                            VALUES 
                            ('adiba0tahsin@gmail.com', 'https://www.linkedin.com/in/adiba-tahsin-985b452a2', 'https://github.com/Adi-Sin101', '+1 (555) 123-4567', 'fas fa-envelope', 1)";
                        
                        SqlCommand insertCmd = new SqlCommand(insertDefaultQuery, con);
                        insertCmd.ExecuteNonQuery();
                        
                        System.Diagnostics.Debug.WriteLine("Default contact info inserted into ContactInfo table");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error ensuring ContactInfo table: {ex.Message}");
                }
            }
        }

        private void PopulateContactDetail(string controlId, string value)
        {
            var control = FindControl(controlId) as Literal;
            if (control != null)
            {
                control.Text = !string.IsNullOrEmpty(value) ? value : GetDefaultText(controlId);
            }
        }

        private string GetDefaultText(string controlId)
        {
            switch (controlId)
            {
                case "hlnkContactEmailCard":
                case "ltlContactEmailDetail":
                    return "adiba0tahsin@gmail.com";
                case "hlnkContactPhoneCard":
                case "ltlContactPhoneDetail":
                    return "+1 (555) 123-4567";
                case "hlnkContactLinkedInCard":
                    return "adiba-tahsin-985b452a2";
                case "hlnkContactGitHubCard":
                    return "Adi-Sin101";
                default:
                    return "Update Contact Info";
            }
        }
    }
}
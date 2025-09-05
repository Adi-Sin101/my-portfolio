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
                        ltlAboutDescription.Text = description;
                        
                        // Social links
                        hlnkGitHub.NavigateUrl = !string.IsNullOrEmpty(gitHubUrl) ? gitHubUrl : "#";
                        hlnkLinkedIn.NavigateUrl = !string.IsNullOrEmpty(linkedInUrl) ? linkedInUrl : "#";
                        hlnkEmail.NavigateUrl = !string.IsNullOrEmpty(email) ? "mailto:" + email : "#";
                        hlnkPhone.NavigateUrl = !string.IsNullOrEmpty(phone) ? "tel:" + phone : "#";
                        hlnkResume.NavigateUrl = !string.IsNullOrEmpty(resumeUrl) ? resumeUrl : "#";
                        
                        // Contact section
                        hlnkContactEmail.Text = !string.IsNullOrEmpty(email) ? email : "Contact Email";
                        hlnkContactEmail.NavigateUrl = !string.IsNullOrEmpty(email) ? "mailto:" + email : "#";
                        hlnkContactLinkedIn.Text = !string.IsNullOrEmpty(linkedInUrl) ? "LinkedIn Profile" : "LinkedIn";
                        hlnkContactLinkedIn.NavigateUrl = !string.IsNullOrEmpty(linkedInUrl) ? linkedInUrl : "#";
                        hlnkContactGitHub.Text = !string.IsNullOrEmpty(gitHubUrl) ? "GitHub Profile" : "GitHub";
                        hlnkContactGitHub.NavigateUrl = !string.IsNullOrEmpty(gitHubUrl) ? gitHubUrl : "#";
                        hlnkContactPhone.Text = !string.IsNullOrEmpty(phone) ? phone : "Contact Phone";
                        hlnkContactPhone.NavigateUrl = !string.IsNullOrEmpty(phone) ? "tel:" + phone : "#";
                        
                        // Profile image
                        imgHero.ImageUrl = !string.IsNullOrEmpty(imagePath) ? imagePath : "Images/default-profile.jpg";
                    }
                    else
                    {
                        // No data found in HomeContent, use defaults
                        SetDefaultPersonalInfo();
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Use default values if database query fails
                    SetDefaultPersonalInfo();
                }
            }
        }

        private void LoadEducation()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    string query = "SELECT Degree, Institution, Year, Grade FROM Education WHERE IsActive = 1 ORDER BY StartYear DESC";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    
                    if (dt.Rows.Count == 0)
                    {
                        // Add sample education data
                        dt.Columns.Add("Degree");
                        dt.Columns.Add("Institution");
                        dt.Columns.Add("Year");
                        dt.Columns.Add("Grade");
                        
                        DataRow row1 = dt.NewRow();
                        row1["Degree"] = "B.Sc in Computer Science";
                        row1["Institution"] = "Khulna University of Engineering & Technology";
                        row1["Year"] = "Expected: 2027";
                        row1["Grade"] = "CGPA: 3.28 / 4.00";
                        dt.Rows.Add(row1);
                        
                        DataRow row2 = dt.NewRow();
                        row2["Degree"] = "HSC";
                        row2["Institution"] = "Khulna Govt. Girls' College";
                        row2["Year"] = "2021";
                        row2["Grade"] = "GPA: 5.00";
                        dt.Rows.Add(row2);
                        
                        DataRow row3 = dt.NewRow();
                        row3["Degree"] = "SSC";
                        row3["Institution"] = "Govt. Coronation Secondary Girls' School";
                        row3["Year"] = "2019";
                        row3["Grade"] = "GPA: 5.00";
                        dt.Rows.Add(row3);
                    }
                    
                    rptEducation.DataSource = dt;
                    rptEducation.DataBind();
                }
                catch (Exception ex)
                {
                    // Handle error
                }
            }
        }

        private void LoadSkills()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    string query = "SELECT Name, IconUrl, Percentage FROM Skills WHERE IsActive = 1 ORDER BY DisplayOrder, Name";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    
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
                    // Handle error
                }
            }
        }

        private void LoadExperience()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    string query = "SELECT Title, Company, Duration, Description, ImagePath FROM Experience WHERE IsActive = 1 ORDER BY StartDate DESC";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    
                    if (dt.Rows.Count == 0)
                    {
                        // Add sample experience data
                        dt.Columns.Add("Title");
                        dt.Columns.Add("Company");
                        dt.Columns.Add("Duration");
                        dt.Columns.Add("Description");
                        dt.Columns.Add("ImagePath");
                        
                        var experiences = new[]
                        {
                            new { Title = "Campus Ambassador", Company = "She-STEM", Duration = "Aug 2025 - Present", 
                                  Description = "Serving as the She-STEM Campus Ambassador at KUET, promoting STEM education among female students.", 
                                  Image = "SheStem.jpg" },
                            new { Title = "Assistant Operations Secretary", Company = "KUET Business and Entrepreneurship Club", Duration = "April 2024 – Present", 
                                  Description = "Responsible for strategic planning and execution of club initiatives, events, workshops, and competitions.", 
                                  Image = "KBEC_member.jpg" }
                        };
                        
                        foreach (var exp in experiences)
                        {
                            DataRow row = dt.NewRow();
                            row["Title"] = exp.Title;
                            row["Company"] = exp.Company;
                            row["Duration"] = exp.Duration;
                            row["Description"] = exp.Description;
                            row["ImagePath"] = exp.Image;
                            dt.Rows.Add(row);
                        }
                    }
                    
                    rptExperience.DataSource = dt;
                    rptExperience.DataBind();
                }
                catch (Exception ex)
                {
                    // Handle error
                }
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
            ltlAboutDescription.Text = "Please update your personal information in the Admin Panel > About section.";
            
            // Default social links
            hlnkGitHub.NavigateUrl = "#";
            hlnkLinkedIn.NavigateUrl = "#";
            hlnkEmail.NavigateUrl = "#";
            hlnkPhone.NavigateUrl = "#";
            hlnkResume.NavigateUrl = "#";
            
            // Default contact section
            hlnkContactEmail.Text = "Update Email";
            hlnkContactEmail.NavigateUrl = "#";
            hlnkContactLinkedIn.Text = "Update LinkedIn";
            hlnkContactLinkedIn.NavigateUrl = "#";
            hlnkContactGitHub.Text = "Update GitHub";
            hlnkContactGitHub.NavigateUrl = "#";
            hlnkContactPhone.Text = "Update Phone";
            hlnkContactPhone.NavigateUrl = "#";
            
            // Default profile image
            imgHero.ImageUrl = "Images/default-profile.jpg";
        }
    }
}
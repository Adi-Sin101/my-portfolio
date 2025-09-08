<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="adminpanel.Home" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title><asp:Literal ID="ltlName" runat="server" Text="Your Name" /> | Portfolio</title>
    <link rel="stylesheet" href="Content/portfolio-style.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700;800&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
        <!-- Navbar -->
        <nav class="navbar">
            <div class="nav-container">
                <div class="logo"><asp:Literal ID="ltlNavName" runat="server" Text="Adiba Tahsin" /></div>
                <ul class="nav-links">
                    <li><a href="#home">Home</a></li>
                    <li><a href="#about">About</a></li>
                    <li><a href="#skills">Skills</a></li>
                    <li><a href="#experience">Experience</a></li>
                    <li><a href="#projects">Projects</a></li>
                    <li><a href="#contact">Contact</a></li>
                </ul>
            </div>
        </nav>

        <!-- Hero Section -->
        <section class="hero-section" id="home">
            <div class="hero-content">
                <div class="hero-text">
                    <h1>Hi, I'm <span class="highlight"><asp:Literal ID="ltlHeroName" runat="server" Text="Adiba Tahsin" /></span></h1>
                    <h2 id="role-text"><asp:Literal ID="ltlRole" runat="server" Text="Web Developer" /></h2>
                    <p><asp:Literal ID="ltlHeroDescription" runat="server" Text="A Computer Science student with a strong foundation in frontend development and programming..." /></p>
                    <div class="hero-buttons">
                        <a href="#projects" class="btn light">View My Work</a>
                        <asp:HyperLink ID="hlnkResume" runat="server" CssClass="btn dark" Target="_blank" Text="Resume" NavigateUrl="#" />
                    </div>
                    <div class="social-icons">
                        <asp:HyperLink ID="hlnkGitHub" runat="server" Target="_blank" aria-label="GitHub">
                            <i class="fab fa-github fa-2x"></i>
                        </asp:HyperLink>
                        <asp:HyperLink ID="hlnkLinkedIn" runat="server" Target="_blank" aria-label="LinkedIn">
                            <i class="fab fa-linkedin fa-2x"></i>
                        </asp:HyperLink>
                        <asp:HyperLink ID="hlnkEmail" runat="server" aria-label="Email">
                            <i class="fas fa-envelope fa-2x"></i>
                        </asp:HyperLink>
                        <asp:HyperLink ID="hlnkPhone" runat="server" aria-label="Phone">
                            <i class="fas fa-phone fa-2x"></i>
                        </asp:HyperLink>
                    </div>
                </div>
                <div class="hero-image">
                    <asp:Image ID="imgHero" runat="server" AlternateText="Profile Photo" ImageUrl="~/Uploads/profile.jpg" />
                </div>
            </div>
        </section>

        <!-- About Section -->
        <section id="about" class="about-section fade-in">
            <div class="about-container">
                <div class="about-me">
                    <h3>About Me</h3>
                    <p><asp:Literal ID="ltlAboutDescription" runat="server" /></p>
                </div>
                <div class="education">
                    <h3>Education</h3>
                    <div class="timeline vertical-right">
                        <asp:Repeater ID="rptEducation" runat="server">
                            <ItemTemplate>
                                <div class="timeline-item">
                                    <div class="timeline-dot"></div>
                                    <div class="timeline-content">
                                        <h4><%# Eval("Degree") %></h4>
                                        <p class="school"><%# Eval("Institution") %></p>
                                        <p><%# Eval("Year") %></p>
                                        <p><%# Eval("Grade") %></p>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </section>

        <!-- Skills Section -->
        <section id="skills" class="skills-section fade-in">
            <h3>Skills</h3>
            <div class="skills-grid">
                <asp:Repeater ID="rptSkills" runat="server">
                    <ItemTemplate>
                        <div class="skill">
                            <img src="<%# Eval("IconUrl") %>" alt="<%# Eval("Name") %>" class="skill-icon" />
                            <div class="skill-name"><%# Eval("Name") %></div>
                            <div class="progress-bar" style='<%# Convert.ToInt32(Eval("Percentage")) > 0 ? "" : "display:none;" %>'>
                                <div class="progress-fill" style="width: 0%;" data-percentage="<%# Eval("Percentage") %>%"></div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </section>

        <!-- Experience Section -->
        <section id="experience" class="experience-section fade-in">
            <h3>Experience</h3>
            <div class="experience-grid">
                <asp:Repeater ID="rptExperience" runat="server">
                    <ItemTemplate>
                        <div class="experience-card">
                            <img src="<%# Eval("ImagePath") %>" alt="<%# Eval("Title") %>" onerror="this.src='default-experience.png';">
                            <div class="experience-content">
                                <h4><%# Eval("Title") %></h4>
                                <p class="company"><%# Eval("Company") %> | <%# Eval("Duration") %></p>
                                <p><%# Eval("Description") %></p>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <!-- Fallback if no data -->
            <div id="noExperience" runat="server" visible="false" style="text-align: center; padding: 40px; color: #a78bfa;">
                <h4>No Experience Records Found</h4>
                <p>Experience data will appear here once added through the admin panel.</p>
            </div>
        </section>

        <!-- Projects Section -->
        <section id="projects" class="projects-section fade-in">
            <h3>Projects</h3>
            <div class="projects-grid">
                <asp:Repeater ID="rptProjects" runat="server">
                    <ItemTemplate>
                        <div class="project-card">
                            <img src="<%# Eval("ImagePath") %>" alt="<%# Eval("Title") %>">
                            <div class="project-content">
                                <h4><%# Eval("Title") %></h4>
                                <p class="project-desc"><%# Eval("Description") %></p>
                                <div class="project-tools">
                                    <strong>Tech Used:</strong>
                                    <%# Eval("TechUsed") %>
                                </div>
                                <a href="<%# Eval("GitUrl") %>" target="_blank" class="project-link">View on GitHub</a>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </section>

       <section id="contact" class="contact-section">
    <h3>Contact</h3>
    <div class="contact-cards-row">
        <!-- Email Card -->
        <div class="contact-card">
            <div class="contact-card-icon" aria-label="Email">
                <i class="fas fa-envelope"></i>
            </div>
            <asp:HyperLink ID="hlnkContactEmailCard" runat="server" CssClass="contact-card-link" Text="adiba0tahsin@gmail.com" NavigateUrl="mailto:adiba0tahsin@gmail.com" />
            <div class="contact-card-label">Email Me</div>
        </div>

        <!-- LinkedIn Card -->
        <div class="contact-card">
            <div class="contact-card-icon" aria-label="LinkedIn">
                <i class="fab fa-linkedin"></i>
            </div>
            <asp:HyperLink ID="hlnkContactLinkedInCard" runat="server" CssClass="contact-card-link" Text="adiba-tahsin-985b452a2" NavigateUrl="https://www.linkedin.com/in/adiba-tahsin-985b452a2" Target="_blank" />
            <div class="contact-card-label">LinkedIn</div>
        </div>

        <!-- GitHub Card -->
        <div class="contact-card">
            <div class="contact-card-icon" aria-label="GitHub">
                <i class="fab fa-github"></i>
            </div>
            <asp:HyperLink ID="hlnkContactGitHubCard" runat="server" CssClass="contact-card-link" Text="Adi-Sin101" NavigateUrl="https://github.com/Adi-Sin101" Target="_blank" />
            <div class="contact-card-label">GitHub</div>
        </div>
    </div>

    <!-- Contact Form -->
    <div class="contact-form" id="contactForm">
        <h4 style="text-align:center;color:#a78bfa;margin-bottom:18px;letter-spacing:0.5px;">Contact Form</h4>
        <div class="form-row form-row-flex">
            <asp:TextBox ID="txtContactName" runat="server" CssClass="form-input" placeholder="Your Name *" />
            <asp:TextBox ID="txtContactEmail" runat="server" CssClass="form-input" placeholder="Your Email *" TextMode="Email" />
        </div>
        <div class="form-row">
            <asp:TextBox ID="txtContactSubject" runat="server" CssClass="form-input" placeholder="Your Subject.." />
        </div>
        <div class="form-row">
            <asp:TextBox ID="txtContactMessage" runat="server" CssClass="form-input" placeholder="Your message..." TextMode="MultiLine" Rows="4" />
        </div>
        <div class="form-row" style="text-align:center;">
            <asp:Button ID="btnSendMessage" runat="server" Text="Send Message" CssClass="btn light" OnClick="BtnSendMessage_Click" />
        </div>
    </div>
</section>


        <!-- Modal Popup -->
        <div id="experienceModal" class="modal">
            <div class="modal-content">
                <span class="close-modal">&times;</span>
                <img id="modalImage" src="" alt="Experience Image" />
                <div id="modalText"></div>
            </div>
        </div>
    </form>

    <script src="Scripts/portfolio.js"></script>
</body>
</html>

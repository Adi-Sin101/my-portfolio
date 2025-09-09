<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="adminpanel._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section class="row" aria-labelledby="dashboardTitle">
            <div class="col-12">
                <h1 id="dashboardTitle">Portfolio Admin Dashboard</h1>
                <p class="lead">Welcome to your portfolio administration panel. Manage all aspects of your portfolio from here.</p>
            </div>
        </section>

        <div class="row mt-4">
            <section class="col-md-4 mb-3">
                <div class="card">
                    <div class="card-header bg-primary text-white">
                        <h3 class="card-title">Projects Management</h3>
                    </div>
                    <div class="card-body">
                        <p>Add, edit, and manage your portfolio projects. Upload images and add project details.</p>
                        <a href="Projects.aspx" class="btn btn-primary">Manage Projects</a>
                    </div>
                </div>
            </section>
            
            <section class="col-md-4 mb-3">
                <div class="card">
                    <div class="card-header bg-success text-white">
                        <h3 class="card-title">Skills & Experience</h3>
                    </div>
                    <div class="card-body">
                        <p>Update your skills, experience, and professional background information.</p>
                        <a href="Skills.aspx" class="btn btn-success me-2">Skills</a>
                        <a href="Experience.aspx" class="btn btn-success">Experience</a>
                    </div>
                </div>
            </section>
            
            <section class="col-md-4 mb-3">
                <div class="card">
                    <div class="card-header bg-info text-white">
                        <h3 class="card-title">Profile Information</h3>
                    </div>
                    <div class="card-body">
                        <p>Manage your personal information, about section, and contact details.</p>
                        <a href="About.aspx" class="btn btn-info me-2">About</a>
                        <a href="Contact.aspx" class="btn btn-info">Contact</a>
                    </div>
                </div>
            </section>
        </div>

        <div class="row mt-4">
            <section class="col-12">
                <div class="card">
                    <div class="card-header bg-dark text-white">
                        <h3 class="card-title">Quick Stats</h3>
                    </div>
                    <div class="card-body">
                        <div class="row text-center">
                            <div class="col-md-3">
                                <h4 class="text-primary">
                                    <asp:Label ID="lblProjectCount" runat="server" Text="0"></asp:Label>
                                </h4>
                                <p>Total Projects</p>
                            </div>
                            <div class="col-md-3">
                                <h4 class="text-success">
                                    <asp:Label ID="lblSkillCount" runat="server" Text="0"></asp:Label>
                                </h4>
                                <p>Skills Listed</p>
                            </div>
                            <div class="col-md-3">
                                <h4 class="text-info">
                                    <asp:Label ID="lblExperienceCount" runat="server" Text="0"></asp:Label>
                                </h4>
                                <p>Work Experiences</p>
                            </div>
                            <div class="col-md-3">
                                <h4 class="text-warning">
                                    <asp:Label ID="lblLastUpdate" runat="server" Text="Never"></asp:Label>
                                </h4>
                                <p>Last Update</p>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>

        <!-- Visitor Tracking Information Section -->
        <div class="row mt-4">
            <section class="col-12">
                <div class="card visitor-tracking-card">
                    <div class="card-header bg-gradient-purple text-white">
                        <h3 class="card-title">
                            <i class="fas fa-user-clock me-2"></i>Your Admin Activity Tracking
                        </h3>
                    </div>
                    <div class="card-body">
                        <asp:Panel ID="pnlVisitorInfo" runat="server" Visible="true">
                            <div class="row">
                                <div class="col-md-3 text-center visitor-stat">
                                    <div class="stat-icon">
                                        <i class="fas fa-user text-primary"></i>
                                    </div>
                                    <h5>Welcome</h5>
                                    <p class="stat-value">
                                        <asp:Label ID="lblVisitorName" runat="server" Text="Admin User" CssClass="fw-bold"></asp:Label>
                                    </p>
                                </div>
                                <div class="col-md-3 text-center visitor-stat">
                                    <div class="stat-icon">
                                        <i class="fas fa-chart-line text-success"></i>
                                    </div>
                                    <h5>Total Visits</h5>
                                    <p class="stat-value">
                                        <asp:Label ID="lblVisitCount" runat="server" Text="1" CssClass="fw-bold text-success"></asp:Label>
                                    </p>
                                </div>
                                <div class="col-md-3 text-center visitor-stat">
                                    <div class="stat-icon">
                                        <i class="fas fa-calendar-plus text-info"></i>
                                    </div>
                                    <h5>First Visit</h5>
                                    <p class="stat-value">
                                        <asp:Label ID="lblFirstVisit" runat="server" Text="Today" CssClass="fw-bold text-info"></asp:Label>
                                    </p>
                                </div>
                                <div class="col-md-3 text-center visitor-stat">
                                    <div class="stat-icon">
                                        <i class="fas fa-clock text-warning"></i>
                                    </div>
                                    <h5>Last Visit</h5>
                                    <p class="stat-value">
                                        <asp:Label ID="lblLastVisit" runat="server" Text="Now" CssClass="fw-bold text-warning"></asp:Label>
                                    </p>
                                </div>
                            </div>
                            
                            <div class="row mt-3">
                                <div class="col-12 text-center">
                                    <asp:Label ID="lblWelcomeMessage" runat="server" CssClass="welcome-message"></asp:Label>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnlNoVisitorInfo" runat="server" Visible="false">
                            <div class="text-center">
                                <i class="fas fa-info-circle text-muted" style="font-size: 2rem;"></i>
                                <p class="text-muted mt-2">No visitor tracking data available. Visit tracking starts after login.</p>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </section>
        </div>
    </main>

    <style>
        .card {
            border: none;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            margin-bottom: 20px;
        }
        .card-header {
            border-bottom: none;
            font-weight: bold;
        }
        .card-title {
            margin: 0;
            font-size: 1.1rem;
        }
        .btn {
            margin: 2px;
        }
        .me-2 {
            margin-right: 0.5rem;
        }
        .mt-4 {
            margin-top: 1.5rem;
        }
        .mb-3 {
            margin-bottom: 1rem;
        }

        /* Visitor Tracking Styles */
        .visitor-tracking-card {
            background: linear-gradient(135deg, #f8fafc 0%, #e2e8f0 100%);
            border: 1px solid #e2e8f0;
            border-radius: 12px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
        }

        .bg-gradient-purple {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        }

        .visitor-stat {
            padding: 20px;
            margin-bottom: 10px;
        }

        .stat-icon {
            width: 60px;
            height: 60px;
            border-radius: 50%;
            background: rgba(255,255,255,0.1);
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 0 auto 15px auto;
            font-size: 24px;
        }

        .visitor-stat h5 {
            color: #495057;
            font-weight: 600;
            margin-bottom: 10px;
            font-size: 14px;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }

        .stat-value {
            font-size: 18px;
            margin: 0;
        }

        .fw-bold {
            font-weight: bold;
        }

        .text-purple {
            color: #667eea;
        }

        .welcome-message {
            font-size: 16px;
            line-height: 1.5;
        }

        .welcome-message .alert {
            border: none;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        }

        /* Responsive adjustments */
        @media (max-width: 768px) {
            .visitor-stat {
                padding: 15px;
                margin-bottom: 15px;
            }
            
            .stat-icon {
                width: 50px;
                height: 50px;
                font-size: 20px;
            }
            
            .stat-value {
                font-size: 16px;
            }
        }

        /* Animation for stats */
        .visitor-stat {
            transition: transform 0.2s ease;
        }

        .visitor-stat:hover {
            transform: translateY(-2px);
        }
    </style>

</asp:Content>

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
    </style>

</asp:Content>

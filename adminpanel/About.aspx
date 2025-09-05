<%@ Page Title="About Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="adminpanel.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <div class="row">
            <div class="col-12">
                <h2 class="mb-4">Personal Information Management</h2>
                
                <!-- Personal Info Form -->
                <div class="card mb-4">
                    <div class="card-header bg-info text-white">
                        <h4 class="mb-0">Personal Information</h4>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label for="txtFullName">Hero Heading (Name):</label>
                                    <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" placeholder="Your Full Name" />
                                    <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName" 
                                        ErrorMessage="Hero heading is required" CssClass="text-danger" ValidationGroup="PersonalInfo" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label for="txtRole">Role Text:</label>
                                    <asp:TextBox ID="txtRole" runat="server" CssClass="form-control" placeholder="e.g., Web Developer" />
                                    <asp:RequiredFieldValidator ID="rfvRole" runat="server" ControlToValidate="txtRole" 
                                        ErrorMessage="Role is required" CssClass="text-danger" ValidationGroup="PersonalInfo" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label for="txtEmail">Email:</label>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="your.email@example.com" />
                                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" 
                                        ErrorMessage="Email is required" CssClass="text-danger" ValidationGroup="PersonalInfo" />
                                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                        ErrorMessage="Please enter a valid email address" CssClass="text-danger" ValidationGroup="PersonalInfo"
                                        ValidationExpression="^[^\s@]+@[^\s@]+\.[^\s@]+$" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label for="txtPhone">Phone:</label>
                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="+1234567890" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group mb-3">
                                    <label for="txtHeroDescription">Main Description:</label>
                                    <asp:TextBox ID="txtHeroDescription" runat="server" CssClass="form-control" TextMode="MultiLine" 
                                        Rows="4" placeholder="Main description for your portfolio..." />
                                    <asp:RequiredFieldValidator ID="rfvAboutDescription" runat="server" ControlToValidate="txtHeroDescription" 
                                        ErrorMessage="Description is required" CssClass="text-danger" ValidationGroup="PersonalInfo" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group mb-3">
                                    <label for="txtAboutDescription">Additional About Info:</label>
                                    <asp:TextBox ID="txtAboutDescription" runat="server" CssClass="form-control" TextMode="MultiLine" 
                                        Rows="3" placeholder="Additional information (optional)..." />
                                    <small class="form-text text-muted">This field is for display purposes only - the main description above will be saved to the database.</small>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Social Links -->
                <div class="card mb-4">
                    <div class="card-header bg-primary text-white">
                        <h4 class="mb-0">Social Links</h4>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label for="txtGitHubUrl">GitHub URL:</label>
                                    <asp:TextBox ID="txtGitHubUrl" runat="server" CssClass="form-control" placeholder="https://github.com/yourusername" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label for="txtGitHubText">GitHub Display Text:</label>
                                    <asp:TextBox ID="txtGitHubText" runat="server" CssClass="form-control" placeholder="yourusername" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label for="txtLinkedInUrl">LinkedIn URL:</label>
                                    <asp:TextBox ID="txtLinkedInUrl" runat="server" CssClass="form-control" placeholder="https://linkedin.com/in/yourprofile" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label for="txtLinkedInText">LinkedIn Display Text:</label>
                                    <asp:TextBox ID="txtLinkedInText" runat="server" CssClass="form-control" placeholder="yourprofile" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label for="txtResumeUrl">Resume URL/Path:</label>
                                    <asp:TextBox ID="txtResumeUrl" runat="server" CssClass="form-control" placeholder="path/to/your/resume.pdf" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label for="FileUploadProfile">Profile Image:</label>
                                    <asp:FileUpload ID="FileUploadProfile" runat="server" CssClass="form-control" accept="image/*" />
                                </div>
                            </div>
                        </div>
                        <div class="row" id="currentProfileDiv" runat="server" visible="false">
                            <div class="col-md-12">
                                <div class="form-group mb-3">
                                    <label>Current Profile Image:</label><br />
                                    <asp:Image ID="imgCurrentProfile" runat="server" CssClass="img-thumbnail" style="max-width: 200px; max-height: 150px;" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Action Buttons -->
                <div class="card">
                    <div class="card-body text-center">
                        <asp:Button ID="btnSave" runat="server" Text="Save Information" OnClick="btnSave_Click" 
                            CssClass="btn btn-success btn-lg me-3" ValidationGroup="PersonalInfo" />
                        <asp:Button ID="btnSaveNew" runat="server" Text="Save as New Record" OnClick="btnSaveNew_Click" 
                            CssClass="btn btn-primary btn-lg me-3" ValidationGroup="PersonalInfo" />
                        <asp:Button ID="btnReset" runat="server" Text="Reset Form" OnClick="btnReset_Click" 
                            CssClass="btn btn-secondary btn-lg" />
                        <asp:HiddenField ID="hdnPersonalInfoId" runat="server" />
                        <asp:HiddenField ID="hdnCurrentProfileImagePath" runat="server" />
                    </div>
                </div>

                <!-- Current Information Display -->
                <div class="card mt-4">
                    <div class="card-header bg-secondary text-white">
                        <h4 class="mb-0">Current Information Preview</h4>
                    </div>
                    <div class="card-body">
                        <asp:Literal ID="ltlCurrentInfo" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <style>
        .form-group label {
            font-weight: 600;
            color: #495057;
        }
        .card {
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            border: none;
        }
        .me-3 {
            margin-right: 1rem;
        }
        .img-thumbnail {
            border: 1px solid #dee2e6;
        }
        .info-item {
            margin-bottom: 10px;
        }
        .info-label {
            font-weight: bold;
            color: #495057;
        }
    </style>
</asp:Content>

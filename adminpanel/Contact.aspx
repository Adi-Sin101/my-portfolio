<%@ Page Title="Contact Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="adminpanel.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <div class="row">
            <div class="col-12">
                <h2 class="mb-4">Contact Information Management</h2>
                
                <div class="alert alert-info">
                    <h5><i class="fas fa-info-circle"></i> Information</h5>
                    <p>Contact information is managed through the <strong>About</strong> section. 
                       This page provides a summary view of your current contact details.</p>
                    <a href="About.aspx" class="btn btn-primary">Go to Personal Information Management</a>
                </div>

                <!-- Current Contact Information Display -->
                <div class="card">
                    <div class="card-header bg-primary text-white">
                        <h4 class="mb-0">Current Contact Information</h4>
                    </div>
                    <div class="card-body">
                        <asp:Literal ID="ltlContactInfo" runat="server" />
                        
                        <div class="mt-4">
                            <asp:Button ID="btnRefresh" runat="server" Text="Refresh Information" OnClick="btnRefresh_Click" 
                                CssClass="btn btn-secondary me-3" />
                            <a href="About.aspx" class="btn btn-primary">Edit Contact Information</a>
                        </div>
                    </div>
                </div>

                <!-- Contact Preview -->
                <div class="card mt-4">
                    <div class="card-header bg-info text-white">
                        <h4 class="mb-0">How It Appears on Portfolio</h4>
                    </div>
                    <div class="card-body">
                        <div class="contact-preview">
                            <asp:Literal ID="ltlContactPreview" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <style>
        .card {
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            border: none;
        }
        .me-3 {
            margin-right: 1rem;
        }
        .contact-info-item {
            margin-bottom: 15px;
            padding: 10px;
            background-color: #f8f9fa;
            border-left: 4px solid #007bff;
        }
        .contact-info-label {
            font-weight: bold;
            color: #495057;
        }
        .contact-preview {
            background-color: #1f2937;
            color: white;
            padding: 30px;
            border-radius: 8px;
        }
        .contact-box-preview {
            background: rgba(255, 255, 255, 0.05);
            border: 1px solid rgba(255, 255, 255, 0.1);
            border-radius: 8px;
            padding: 20px;
            margin-bottom: 15px;
            text-align: center;
        }
        .contact-icon-preview {
            color: #2563eb;
            margin-bottom: 10px;
        }
        .contact-preview a {
            color: #60a5fa;
            text-decoration: none;
        }
        .contact-preview a:hover {
            color: #93c5fd;
        }
    </style>
</asp:Content>

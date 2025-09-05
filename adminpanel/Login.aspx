<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="adminpanel.Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-6">
                <div class="card shadow">
                    <div class="card-header bg-primary text-white text-center">
                        <h3 class="mb-0">Admin Login</h3>
                    </div>
                    <div class="card-body">
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" CssClass="alert alert-danger" Visible="false"></asp:Label>
                        
                        <div class="form-group mb-3">
                            <label for="txtUsername">Username:</label>
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Enter username"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvUsername" runat="server" 
                                ControlToValidate="txtUsername" 
                                ErrorMessage="Username is required" 
                                CssClass="text-danger" 
                                ValidationGroup="Login" />
                        </div>
                        
                        <div class="form-group mb-3">
                            <label for="txtPassword">Password:</label>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Enter password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                                ControlToValidate="txtPassword" 
                                ErrorMessage="Password is required" 
                                CssClass="text-danger" 
                                ValidationGroup="Login" />
                        </div>
                        
                        <div class="d-grid">
                            <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="BtnLogin_Click" 
                                CssClass="btn btn-primary btn-lg" ValidationGroup="Login" />
                        </div>
                    </div>
                    <div class="card-footer text-center text-muted">
                        <small>Default credentials: admin / admin123</small>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <style>
        .container {
            max-width: 500px;
        }
        .card {
            border: none;
            border-radius: 10px;
        }
        .card-header {
            border-radius: 10px 10px 0 0;
        }
        .form-control:focus {
            border-color: #007bff;
            box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
        }
    </style>
</asp:Content>

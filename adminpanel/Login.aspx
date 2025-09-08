<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="adminpanel.Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Portfolio-style Login Page - Fixed for Form Submission -->
    <div class="login-container">
        <div class="login-background"></div>
        
        <div class="login-wrapper">
            <div class="login-card">
                <!-- Header -->
                <div class="login-header">
                    <div class="login-icon">
                        <i class="fas fa-lock"></i>
                    </div>
                    <h2>Admin Portal</h2>
                    <div class="login-divider"></div>
                </div>

                <!-- Form Content -->
                <div class="login-body">
                    <asp:Label ID="lblMessage" runat="server" CssClass="login-alert" Visible="false"></asp:Label>
                    
                    <div class="form-group">
                        <div class="input-wrapper">
                            <i class="fas fa-user input-icon"></i>
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="login-input" placeholder="Username" />
                        </div>
                        <asp:RequiredFieldValidator ID="rfvUsername" runat="server" 
                            ControlToValidate="txtUsername" 
                            ErrorMessage="Username is required" 
                            CssClass="validation-error" 
                            ValidationGroup="Login" />
                    </div>
                    
                    <div class="form-group">
                        <div class="input-wrapper">
                            <i class="fas fa-key input-icon"></i>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="login-input" placeholder="Password" />
                        </div>
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                            ControlToValidate="txtPassword" 
                            ErrorMessage="Password is required" 
                            CssClass="validation-error" 
                            ValidationGroup="Login" />
                    </div>
                    
                    <div class="login-actions">
                        <asp:Button ID="btnLogin" runat="server" Text="Sign In" OnClick="BtnLogin_Click" 
                            CssClass="login-btn" ValidationGroup="Login" />
                    </div>
                </div>
                
                <!-- Footer -->
                <div class="login-footer">
                    <div class="default-credentials">
                        <i class="fas fa-info-circle"></i>
                        <span>Default: admin / admin123</span>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <style>
        /* Portfolio-style Login Design - Fixed for Form Submission */
        .login-container {
            position: relative; /* Changed from fixed to relative */
            min-height: 100vh;
            background: linear-gradient(135deg, #180322 0%, #3d1555 100%);
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 20px;
            box-sizing: border-box;
            margin: -20px; /* Compensate for container padding */
        }

        .login-background {
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: radial-gradient(circle at 70% 20%, rgba(120, 53, 150, 0.13) 0%, transparent 60%),
                        radial-gradient(circle at 20% 80%, rgba(168, 85, 247, 0.09) 0%, transparent 70%);
            pointer-events: none;
        }

        .login-wrapper {
            position: relative;
            z-index: 1;
            width: 100%;
            max-width: 420px;
        }

        .login-card {
            background: rgba(36, 10, 39, 0.95);
            border: 1px solid rgba(167, 139, 250, 0.3);
            border-radius: 18px;
            box-shadow: 0 8px 32px rgba(120, 53, 150, 0.15), 
                        0 0 24px rgba(147, 51, 234, 0.1);
            backdrop-filter: blur(15px);
            -webkit-backdrop-filter: blur(15px);
            overflow: hidden;
            transition: transform 0.3s ease, box-shadow 0.3s ease;
        }

        .login-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 12px 40px rgba(120, 53, 150, 0.2), 
                        0 0 32px rgba(147, 51, 234, 0.15);
        }

        /* Header */
        .login-header {
            text-align: center;
            padding: 40px 30px 20px;
            background: linear-gradient(135deg, rgba(167, 139, 250, 0.1) 0%, rgba(244, 114, 182, 0.05) 100%);
        }

        .login-icon {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 60px;
            height: 60px;
            background: linear-gradient(135deg, #a78bfa 0%, #f472b6 100%);
            border-radius: 50%;
            margin-bottom: 20px;
            box-shadow: 0 4px 16px rgba(167, 139, 250, 0.3);
        }

        .login-icon i {
            font-size: 24px;
            color: #ffffff;
        }

        .login-header h2 {
            color: #ffffff;
            font-size: 1.8rem;
            font-weight: 600;
            margin: 0 0 15px 0;
            letter-spacing: 0.5px;
        }

        .login-divider {
            width: 60px;
            height: 3px;
            background: linear-gradient(90deg, #a78bfa 0%, #f472b6 100%);
            border-radius: 2px;
            margin: 0 auto;
        }

        /* Body */
        .login-body {
            padding: 30px;
        }

        .form-group {
            margin-bottom: 25px;
        }

        .input-wrapper {
            position: relative;
            display: flex;
            align-items: center;
        }

        .input-icon {
            position: absolute;
            left: 15px;
            color: #a78bfa;
            font-size: 16px;
            z-index: 2;
            pointer-events: none; /* Prevent icon from blocking input */
        }

        .login-input {
            width: 100%;
            padding: 15px 15px 15px 45px;
            background: rgba(60, 16, 80, 0.6);
            border: 1.5px solid rgba(167, 139, 250, 0.3);
            border-radius: 12px;
            color: #f3e8ff;
            font-size: 16px;
            font-family: 'Poppins', sans-serif;
            outline: none;
            transition: all 0.3s ease;
            box-sizing: border-box;
        }

        .login-input::placeholder {
            color: rgba(243, 232, 255, 0.6);
        }

        .login-input:focus {
            border-color: #a78bfa;
            box-shadow: 0 0 0 3px rgba(167, 139, 250, 0.1);
            background: rgba(60, 16, 80, 0.8);
        }

        .validation-error {
            display: block;
            color: #ff6b6b;
            font-size: 14px;
            margin-top: 8px;
            margin-left: 5px;
        }

        .login-alert {
            display: block !important;
            padding: 12px 16px;
            border-radius: 8px;
            margin-bottom: 20px;
            font-size: 14px;
            border-left: 4px solid;
        }

        .login-alert.alert-danger {
            background-color: rgba(255, 107, 107, 0.1);
            border-left-color: #ff6b6b;
            color: #ff9999;
        }

        .login-alert.alert-warning {
            background-color: rgba(255, 193, 7, 0.1);
            border-left-color: #ffc107;
            color: #ffda6a;
        }

        .login-alert.alert-success {
            background-color: rgba(34, 197, 94, 0.1);
            border-left-color: #22c55e;
            color: #4ade80;
        }

        .login-actions {
            margin-top: 35px;
        }

        .login-btn {
            width: 100%;
            padding: 15px;
            background: linear-gradient(135deg, #a78bfa 0%, #f472b6 100%);
            border: none;
            border-radius: 12px;
            color: #ffffff;
            font-size: 16px;
            font-weight: 600;
            font-family: 'Poppins', sans-serif;
            cursor: pointer;
            transition: all 0.3s ease;
            box-shadow: 0 4px 16px rgba(167, 139, 250, 0.3);
            letter-spacing: 0.5px;
        }

        .login-btn:hover {
            transform: translateY(-2px);
            box-shadow: 0 8px 25px rgba(167, 139, 250, 0.4);
            background: linear-gradient(135deg, #8b5cf6 0%, #ec4899 100%);
        }

        .login-btn:active {
            transform: translateY(0);
            box-shadow: 0 4px 16px rgba(167, 139, 250, 0.3);
        }

        /* Ensure button can be clicked */
        .login-btn {
            position: relative;
            z-index: 10;
        }

        /* Footer */
        .login-footer {
            padding: 20px 30px;
            background: rgba(20, 5, 25, 0.4);
            border-top: 1px solid rgba(167, 139, 250, 0.1);
        }

        .default-credentials {
            display: flex;
            align-items: center;
            justify-content: center;
            color: rgba(243, 232, 255, 0.7);
            font-size: 14px;
            gap: 8px;
        }

        .default-credentials i {
            color: #a78bfa;
        }

        /* Responsive Design */
        @media (max-width: 480px) {
            .login-container {
                padding: 15px;
                margin: -15px;
            }

            .login-card {
                border-radius: 15px;
            }

            .login-header {
                padding: 30px 20px 15px;
            }

            .login-header h2 {
                font-size: 1.6rem;
            }

            .login-body {
                padding: 25px 20px;
            }

            .login-footer {
                padding: 15px 20px;
            }

            .default-credentials {
                font-size: 13px;
            }
        }

        /* Animation Effects */
        @keyframes fadeInUp {
            from {
                opacity: 0;
                transform: translateY(30px);
            }
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        .login-card {
            animation: fadeInUp 0.6s ease;
        }

        /* Focus and Accessibility */
        .login-input:focus + .input-icon,
        .login-input:not(:placeholder-shown) + .input-icon {
            color: #f472b6;
        }

        /* Ensure proper font loading */
        body {
            font-family: 'Poppins', 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }

        /* Override master page styles if needed */
        .login-container * {
            box-sizing: border-box;
        }

        /* Fix any potential z-index issues */
        form {
            position: relative;
            z-index: 1;
        }
    </style>

    <!-- Font Awesome for icons (if not already loaded) -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
    <!-- Google Fonts for Poppins (if not already loaded) -->
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap" rel="stylesheet">

    <!-- Simple JavaScript for Enter key support - No conflicts -->
    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function() {
            // Add Enter key support
            var inputs = document.querySelectorAll('.login-input');
            for (var i = 0; i < inputs.length; i++) {
                inputs[i].addEventListener('keypress', function(e) {
                    if (e.key === 'Enter' || e.keyCode === 13) {
                        document.getElementById('<%= btnLogin.ClientID %>').click();
                    }
                });
            }
        });
    </script>
</asp:Content>

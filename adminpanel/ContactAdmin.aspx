<%@ Page Title="Contact Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ContactAdmin.aspx.cs" Inherits="adminpanel.ContactAdmin" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Portfolio-style Contact Management -->
    <div class="contact-admin-container">
        <div class="contact-admin-background"></div>
        
        <div class="contact-admin-wrapper">
            <!-- Header Section -->
            <div class="admin-hero-section">
                <div class="admin-hero-content">
                    <h1>Contact Management <span class="highlight">Dashboard</span></h1>
                    <div class="contact-divider-posh"></div>
                    <p>Manage your portfolio contact messages and information</p>
                </div>
            </div>

            <!-- Contact Messages CRUD Section -->
            <section class="contact-section admin-section">
                <div class="contact-messages-card">
                    <div class="card-header-modern">
                        <h3><i class="fas fa-envelope"></i> Contact Messages</h3>
                        <div class="header-actions">
                            <asp:Button ID="btnRefreshMessages" runat="server" Text="Refresh" OnClick="btnRefreshMessages_Click" 
                                CssClass="btn light admin-btn" />
                            <asp:Button ID="btnAddMessage" runat="server" Text="Add New Message" OnClick="btnAddMessage_Click" 
                                CssClass="btn dark admin-btn" />
                            <asp:Button ID="btnMarkAllRead" runat="server" Text="Mark All Read" OnClick="btnMarkAllRead_Click" 
                                CssClass="btn light admin-btn" />
                        </div>
                    </div>
                    
                    <div class="card-body-modern">
                        <asp:Label ID="lblMessageCount" runat="server" CssClass="contact-card-label message-stats" Text="Loading messages..." />
                        
                        <div class="messages-grid-container">
                            <asp:GridView ID="gvContactMessages" runat="server" 
                                CssClass="modern-table" 
                                AutoGenerateColumns="false" 
                                AllowPaging="true" 
                                PageSize="10"
                                OnPageIndexChanging="gvContactMessages_PageIndexChanging"
                                OnRowCommand="gvContactMessages_RowCommand"
                                OnRowEditing="gvContactMessages_RowEditing"
                                OnRowUpdating="gvContactMessages_RowUpdating"
                                OnRowCancelingEdit="gvContactMessages_RowCancelingEdit"
                                OnRowDeleting="gvContactMessages_RowDeleting"
                                EmptyDataText="No contact messages found."
                                DataKeyNames="Id">
                                <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="ID" Visible="false" />
                                    
                                    <asp:TemplateField HeaderText="Status" ItemStyle-CssClass="status-column">
                                        <ItemTemplate>
                                            <span class='<%# Convert.ToBoolean(Eval("IsRead")) ? "status-badge read" : "status-badge new" %>'>
                                                <%# Convert.ToBoolean(Eval("IsRead")) ? "Read" : "New" %>
                                            </span>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlIsRead" runat="server" CssClass="modern-select">
                                                <asp:ListItem Value="0" Text="New" />
                                                <asp:ListItem Value="1" Text="Read" />
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Name" ItemStyle-CssClass="name-column">
                                        <ItemTemplate>
                                            <div class="sender-name"><%# Eval("Name") %></div>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("Name") %>' 
                                                CssClass="modern-input" MaxLength="100" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Email" ItemStyle-CssClass="email-column">
                                        <ItemTemplate>
                                            <a href="mailto:<%# Eval("Email") %>" class="email-link">
                                                <%# Eval("Email") %>
                                            </a>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEmail" runat="server" Text='<%# Eval("Email") %>' 
                                                CssClass="modern-input" MaxLength="100" TextMode="Email" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Subject" ItemStyle-CssClass="subject-column">
                                        <ItemTemplate>
                                            <div class="message-subject"><%# Eval("Subject") %></div>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtSubject" runat="server" Text='<%# Eval("Subject") %>' 
                                                CssClass="modern-input" MaxLength="200" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Message" ItemStyle-CssClass="message-column">
                                        <ItemTemplate>
                                            <div class="message-preview-modern" title="<%# Eval("Message") %>">
                                                <%# TruncateText(Eval("Message").ToString(), 100) %>
                                            </div>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtMessage" runat="server" Text='<%# Eval("Message") %>' 
                                                CssClass="modern-textarea" TextMode="MultiLine" Rows="3" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:BoundField DataField="DateReceived" HeaderText="Date" DataFormatString="{0:MMM dd, yyyy}" 
                                        ItemStyle-CssClass="date-column" ReadOnly="true" />
                                    
                                    <asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="actions-column">
                                        <ItemTemplate>
                                            <div class="action-buttons">
                                                <asp:Button ID="btnView" runat="server" Text="View" CommandName="ViewMessage" 
                                                    CommandArgument='<%# Eval("Id") %>' CssClass="action-btn view-btn" />
                                                <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="Edit" 
                                                    CssClass="action-btn edit-btn" />
                                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="Delete" 
                                                    CommandArgument='<%# Eval("Id") %>' CssClass="action-btn delete-btn"
                                                    OnClientClick="return confirm('Are you sure you want to delete this message?');" />
                                            </div>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <div class="action-buttons">
                                                <asp:Button ID="btnUpdate" runat="server" Text="Update" CommandName="Update" 
                                                    CssClass="action-btn update-btn" />
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CommandName="Cancel" 
                                                    CssClass="action-btn cancel-btn" />
                                            </div>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </section>

            <!-- Information Section -->
            <section class="contact-section admin-section">
                <div class="info-card">
                    <div class="info-content">
                        <div class="info-icon">
                            <i class="fas fa-lightbulb"></i>
                        </div>
                        <div class="info-text">
                            <h4>Quick Tip</h4>
                            <p>This is the admin panel for managing contact messages. Your public contact page is available at <strong>Contact.aspx</strong>.</p>
                            <a href="Contact.aspx" class="btn light admin-btn" target="_blank">View Public Contact Page</a>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>

    <!-- Contact Management Styles -->
    <style>
        /* Portfolio-style Contact Admin Design */
        .contact-admin-container {
            min-height: 100vh;
            background: linear-gradient(135deg, #180322 0%, #3d1555 100%);
            color: #f3e8ff;
            padding: 20px 0;
            position: relative;
        }

        .contact-admin-background {
            position: fixed;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: radial-gradient(circle at 70% 20%, rgba(120, 53, 150, 0.13) 0%, transparent 60%),
                        radial-gradient(circle at 20% 80%, rgba(168, 85, 247, 0.09) 0%, transparent 70%);
            pointer-events: none;
            z-index: 0;
        }

        .contact-admin-wrapper {
            position: relative;
            z-index: 1;
            max-width: 1200px;
            margin: 0 auto;
            padding: 0 20px;
        }

        /* Hero Section */
        .admin-hero-section {
            text-align: center;
            padding: 40px 0 60px;
        }

        .admin-hero-content h1 {
            font-size: 3rem;
            margin-bottom: 20px;
            color: #f3e8ff;
            font-weight: 700;
        }

        .highlight {
            color: #a78bfa;
            font-weight: 800;
        }

        .contact-divider-posh {
            width: 48px;
            height: 3px;
            background: linear-gradient(90deg, #a78bfa 0%, #f472b6 100%);
            border-radius: 2px;
            margin: 0 auto 20px auto;
        }

        .admin-hero-content p {
            font-size: 1.2rem;
            color: #cbd5e1;
            margin: 0;
        }

        /* Sections */
        .admin-section {
            margin-bottom: 40px;
        }

        /* Cards */
        .contact-messages-card,
        .info-card {
            background: rgba(36, 10, 39, 0.97);
            border-radius: 18px;
            box-shadow: 0 8px 32px rgba(36, 10, 39, 0.18);
            overflow: hidden;
            transition: transform 0.2s, box-shadow 0.2s;
        }

        .contact-messages-card:hover {
            transform: translateY(-2px);
            box-shadow: 0 12px 40px rgba(120, 53, 150, 0.2);
        }

        .card-header-modern {
            background: linear-gradient(135deg, rgba(167, 139, 250, 0.1) 0%, rgba(244, 114, 182, 0.05) 100%);
            padding: 25px 30px;
            border-bottom: 1px solid rgba(167, 139, 250, 0.1);
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .card-header-modern h3 {
            color: #a78bfa;
            font-size: 1.5rem;
            font-weight: 600;
            margin: 0;
            display: flex;
            align-items: center;
            gap: 12px;
        }

        .header-actions {
            display: flex;
            gap: 10px;
            align-items: center;
        }

        .card-body-modern {
            padding: 30px;
        }

        /* Buttons */
        .admin-btn {
            padding: 8px 16px;
            border-radius: 8px;
            font-weight: 500;
            font-size: 14px;
            border: none;
            cursor: pointer;
            transition: all 0.3s ease;
            text-decoration: none;
            display: inline-block;
        }

        .btn.light {
            background: #ffffff;
            color: #2e1131;
        }

        .btn.light:hover {
            background: #a78bfa;
            color: #ffffff;
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(167, 139, 250, 0.3);
        }

        .btn.dark {
            background: #a78bfa;
            color: #ffffff;
        }

        .btn.dark:hover {
            background: #8b5cf6;
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(139, 92, 246, 0.4);
        }

        /* Message Stats */
        .message-stats {
            display: inline-block;
            background: linear-gradient(135deg, #a78bfa 0%, #f472b6 100%);
            color: #ffffff;
            padding: 8px 16px;
            border-radius: 20px;
            font-size: 14px;
            font-weight: 600;
            margin-bottom: 25px;
        }

        /* Modern Table */
        .messages-grid-container {
            background: rgba(60, 16, 80, 0.6);
            border-radius: 12px;
            padding: 20px;
            overflow-x: auto;
        }

        .modern-table {
            width: 100%;
            border-collapse: separate;
            border-spacing: 0;
            color: #f3e8ff;
        }

        .modern-table th {
            background: rgba(167, 139, 250, 0.2);
            color: #a78bfa;
            padding: 15px 12px;
            font-weight: 600;
            text-align: left;
            border: none;
            font-size: 14px;
        }

        .modern-table th:first-child {
            border-radius: 8px 0 0 0;
        }

        .modern-table th:last-child {
            border-radius: 0 8px 0 0;
        }

        .modern-table td {
            padding: 12px;
            border-bottom: 1px solid rgba(167, 139, 250, 0.1);
            vertical-align: middle;
        }

        .modern-table tr:hover td {
            background: rgba(167, 139, 250, 0.05);
        }

        /* Status Badges */
        .status-badge {
            padding: 4px 10px;
            border-radius: 12px;
            font-size: 12px;
            font-weight: 600;
            text-transform: uppercase;
        }

        .status-badge.new {
            background: rgba(34, 197, 94, 0.2);
            color: #4ade80;
            border: 1px solid rgba(34, 197, 94, 0.3);
        }

        .status-badge.read {
            background: rgba(107, 114, 126, 0.2);
            color: #9ca3af;
            border: 1px solid rgba(107, 114, 126, 0.3);
        }

        /* Column Styles */
        .sender-name,
        .message-subject {
            color: #f3e8ff;
            font-weight: 500;
        }

        .email-link {
            color: #a78bfa;
            text-decoration: none;
            transition: color 0.2s;
        }

        .email-link:hover {
            color: #f472b6;
        }

        .message-preview-modern {
            max-width: 250px;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
            color: #cbd5e1;
            cursor: pointer;
        }

        /* Action Buttons */
        .action-buttons {
            display: flex;
            gap: 5px;
            flex-wrap: wrap;
        }

        .action-btn {
            padding: 4px 8px;
            border: none;
            border-radius: 4px;
            font-size: 12px;
            cursor: pointer;
            transition: all 0.2s ease;
        }

        .view-btn {
            background: rgba(59, 130, 246, 0.2);
            color: #60a5fa;
        }

        .view-btn:hover {
            background: #3b82f6;
            color: white;
        }

        .edit-btn {
            background: rgba(245, 158, 11, 0.2);
            color: #fbbf24;
        }

        .edit-btn:hover {
            background: #f59e0b;
            color: white;
        }

        .delete-btn {
            background: rgba(239, 68, 68, 0.2);
            color: #f87171;
        }

        .delete-btn:hover {
            background: #ef4444;
            color: white;
        }

        .update-btn {
            background: rgba(34, 197, 94, 0.2);
            color: #4ade80;
        }

        .update-btn:hover {
            background: #22c55e;
            color: white;
        }

        .cancel-btn {
            background: rgba(107, 114, 126, 0.2);
            color: #9ca3af;
        }

        .cancel-btn:hover {
            background: #6b7280;
            color: white;
        }

        /* Form Elements */
        .modern-input,
        .modern-textarea,
        .modern-select {
            width: 100%;
            padding: 8px 12px;
            background: rgba(60, 16, 80, 0.8);
            border: 1px solid rgba(167, 139, 250, 0.3);
            border-radius: 6px;
            color: #f3e8ff;
            font-size: 14px;
            outline: none;
            transition: border-color 0.2s;
        }

        .modern-input:focus,
        .modern-textarea:focus,
        .modern-select:focus {
            border-color: #a78bfa;
            box-shadow: 0 0 0 2px rgba(167, 139, 250, 0.2);
        }

        /* Info Card */
        .info-card {
            background: linear-gradient(135deg, rgba(59, 130, 246, 0.1) 0%, rgba(147, 51, 234, 0.1) 100%);
            border: 1px solid rgba(59, 130, 246, 0.2);
        }

        .info-content {
            padding: 25px 30px;
            display: flex;
            align-items: center;
            gap: 20px;
        }

        .info-icon {
            width: 50px;
            height: 50px;
            background: rgba(59, 130, 246, 0.2);
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            color: #60a5fa;
            font-size: 20px;
            flex-shrink: 0;
        }

        .info-text h4 {
            color: #a78bfa;
            margin: 0 0 10px 0;
            font-size: 1.2rem;
        }

        .info-text p {
            color: #cbd5e1;
            margin: 0 0 15px 0;
            line-height: 1.6;
        }

        /* Responsive Design */
        @media (max-width: 1024px) {
            .header-actions {
                flex-direction: column;
                gap: 8px;
            }
        }

        @media (max-width: 768px) {
            .contact-admin-wrapper {
                padding: 0 15px;
            }
            
            .admin-hero-content h1 {
                font-size: 2rem;
            }
            
            .card-header-modern {
                padding: 20px;
                flex-direction: column;
                gap: 15px;
                align-items: flex-start;
            }
            
            .card-body-modern {
                padding: 20px;
            }
            
            .messages-grid-container {
                padding: 15px;
            }
            
            .modern-table th,
            .modern-table td {
                padding: 8px 6px;
                font-size: 13px;
            }
            
            .action-buttons {
                flex-direction: column;
            }
            
            .info-content {
                flex-direction: column;
                text-align: center;
                gap: 15px;
            }
        }

        /* Animation for page load */
        .admin-section {
            animation: fadeInUp 0.6s ease forwards;
            opacity: 0;
        }

        .admin-section:nth-child(1) { animation-delay: 0.1s; }
        .admin-section:nth-child(2) { animation-delay: 0.2s; }

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
    </style>
</asp:Content>
<%@ Page Title="Contact Information Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ContactInfo.aspx.cs" Inherits="adminpanel.ContactInfo" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Portfolio-style Contact Info Management -->
    <div class="contact-info-admin-container">
        <div class="contact-info-admin-background"></div>
        
        <div class="contact-info-admin-wrapper">
            <!-- Hero Section -->
            <div class="admin-hero-section">
                <div class="admin-hero-content">
                    <h1>Contact Information <span class="highlight">Management</span></h1>
                    <div class="contact-info-divider-posh"></div>
                    <p>Manage your contact information and social links</p>
                </div>
            </div>

            <!-- Contact Info CRUD Section -->
            <section class="contact-info-section admin-section">
                <div class="contact-info-crud-card">
                    <div class="card-header-modern">
                        <h3><i class="fas fa-address-card"></i> Contact Information Dashboard</h3>
                        <div class="header-actions">
                            <asp:Button ID="btnRefreshContactInfo" runat="server" Text="Refresh" OnClick="btnRefreshContactInfo_Click" 
                                CssClass="btn light admin-btn" />
                            <asp:Button ID="btnAddNewContactInfo" runat="server" Text="Add New Contact" OnClick="btnAddNewContactInfo_Click" 
                                CssClass="btn dark admin-btn" />
                        </div>
                    </div>
                    
                    <div class="card-body-modern">
                        <asp:Label ID="lblContactInfoCount" runat="server" CssClass="contact-info-card-label contact-info-stats" Text="Loading contact info..." />
                        
                        <div class="contact-info-grid-container">
                            <asp:GridView ID="gvContactInfo" runat="server" 
                                CssClass="modern-table" 
                                AutoGenerateColumns="false" 
                                AllowPaging="true" 
                                PageSize="10"
                                OnPageIndexChanging="gvContactInfo_PageIndexChanging"
                                OnRowCommand="gvContactInfo_RowCommand"
                                OnRowEditing="gvContactInfo_RowEditing"
                                OnRowUpdating="gvContactInfo_RowUpdating"
                                OnRowCancelingEdit="gvContactInfo_RowCancelingEdit"
                                OnRowDeleting="gvContactInfo_RowDeleting"
                                EmptyDataText="No contact information found. Add your first contact info!"
                                DataKeyNames="Id">
                                <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="ID" Visible="false" />
                                    
                                    <asp:TemplateField HeaderText="Icon" ItemStyle-CssClass="icon-column">
                                        <ItemTemplate>
                                            <div class="contact-info-icon-preview">
                                                <i class="<%# Eval("icon_class") %>" style="font-size: 24px; color: #a78bfa;"></i>
                                            </div>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtIconClass" runat="server" Text='<%# Eval("icon_class") %>' 
                                                CssClass="modern-input" placeholder="fas fa-envelope" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Type" ItemStyle-CssClass="type-column">
                                        <ItemTemplate>
                                            <div class="contact-type"><%# GetContactType(Eval("icon_class").ToString()) %></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Email" ItemStyle-CssClass="email-column">
                                        <ItemTemplate>
                                            <div class="contact-email"><%# Eval("Email") %></div>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEmail" runat="server" Text='<%# Eval("Email") %>' 
                                                CssClass="modern-input" TextMode="Email" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="LinkedIn" ItemStyle-CssClass="linkedin-column">
                                        <ItemTemplate>
                                            <div class="contact-linkedin">
                                                <asp:HyperLink ID="hlnkLinkedIn" runat="server" 
                                                    NavigateUrl='<%# Eval("LinkedIn") %>' Target="_blank"
                                                    Text='<%# !string.IsNullOrEmpty(Eval("LinkedIn").ToString()) ? "View Profile" : "Not Set" %>'
                                                    Visible='<%# !string.IsNullOrEmpty(Eval("LinkedIn").ToString()) %>' />
                                                <span runat="server" visible='<%# string.IsNullOrEmpty(Eval("LinkedIn").ToString()) %>'>Not Set</span>
                                            </div>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtLinkedIn" runat="server" Text='<%# Eval("LinkedIn") %>' 
                                                CssClass="modern-input" placeholder="https://linkedin.com/in/username" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="GitHub" ItemStyle-CssClass="github-column">
                                        <ItemTemplate>
                                            <div class="contact-github">
                                                <asp:HyperLink ID="hlnkGitHub" runat="server" 
                                                    NavigateUrl='<%# Eval("GitHub") %>' Target="_blank"
                                                    Text='<%# !string.IsNullOrEmpty(Eval("GitHub").ToString()) ? "View Profile" : "Not Set" %>'
                                                    Visible='<%# !string.IsNullOrEmpty(Eval("GitHub").ToString()) %>' />
                                                <span runat="server" visible='<%# string.IsNullOrEmpty(Eval("GitHub").ToString()) %>'>Not Set</span>
                                            </div>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtGitHub" runat="server" Text='<%# Eval("GitHub") %>' 
                                                CssClass="modern-input" placeholder="https://github.com/username" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Phone" ItemStyle-CssClass="phone-column">
                                        <ItemTemplate>
                                            <div class="contact-phone"><%# Eval("Phone") %></div>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtPhone" runat="server" Text='<%# Eval("Phone") %>' 
                                                CssClass="modern-input" placeholder="+1 (555) 123-4567" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Order" ItemStyle-CssClass="order-column">
                                        <ItemTemplate>
                                            <div class="display-order"><%# Eval("display_order") %></div>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDisplayOrder" runat="server" Text='<%# Eval("display_order") %>' 
                                                CssClass="modern-input small" TextMode="Number" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="actions-column">
                                        <ItemTemplate>
                                            <div class="action-buttons">
                                                <asp:Button ID="btnView" runat="server" Text="View" CommandName="ViewContactInfo" 
                                                    CommandArgument='<%# Eval("Id") %>' CssClass="action-btn view-btn" />
                                                <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="Edit" 
                                                    CssClass="action-btn edit-btn" />
                                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="Delete" 
                                                    CssClass="action-btn delete-btn"
                                                    OnClientClick="return confirm('Are you sure you want to delete this contact info?');" />
                                            </div>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <div class="action-buttons">
                                                <asp:Button ID="btnUpdate" runat="server" Text="Save" CommandName="Update" 
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

            <!-- Contact Preview Section -->
            <section class="contact-info-section admin-section">
                <div class="contact-info-preview-card">
                    <div class="card-header-modern">
                        <h3><i class="fas fa-eye"></i> Portfolio Preview</h3>
                    </div>
                    <div class="card-body-modern">
                        <div class="portfolio-preview-container">
                            <h4 style="text-align: center; margin-bottom: 30px; color: #ffffff;">How Your Contact Info Appears on Portfolio</h4>
                            <asp:Literal ID="ltlContactInfoPreview" runat="server" />
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>

    <!-- Modal for Contact Info Details/Add/Edit -->
    <div class="modal-overlay" id="contactInfoModal" style="display: none;">
        <div class="modal-container">
            <div class="modal-header-modern">
                <h3>
                    <asp:Literal ID="ltlModalTitle" runat="server" Text="Contact Information Details" />
                </h3>
                <button type="button" class="modal-close-btn" onclick="closeContactInfoModal()">
                    <i class="fas fa-times"></i>
                </button>
            </div>
            
            <div class="modal-body-modern">
                <asp:Panel ID="pnlContactInfoForm" runat="server" Visible="false">
                    <div class="form-group">
                        <label class="modern-label">Email Address *</label>
                        <asp:TextBox ID="txtModalEmail" runat="server" CssClass="modern-input" TextMode="Email" placeholder="your.email@example.com" />
                        <asp:RequiredFieldValidator ID="rfvModalEmail" runat="server" 
                            ControlToValidate="txtModalEmail" ErrorMessage="Email is required" 
                            CssClass="validation-error" ValidationGroup="ContactInfoForm" />
                        <small class="form-hint">This email will be used for contact form submissions</small>
                    </div>
                    
                    <div class="form-grid">
                        <div class="form-group">
                            <label class="modern-label">LinkedIn Profile URL</label>
                            <asp:TextBox ID="txtModalLinkedIn" runat="server" CssClass="modern-input" 
                                placeholder="https://linkedin.com/in/your-profile" />
                        </div>
                        <div class="form-group">
                            <label class="modern-label">GitHub Profile URL</label>
                            <asp:TextBox ID="txtModalGitHub" runat="server" CssClass="modern-input" 
                                placeholder="https://github.com/your-username" />
                        </div>
                    </div>
                    
                    <div class="form-grid">
                        <div class="form-group">
                            <label class="modern-label">Phone Number</label>
                            <asp:TextBox ID="txtModalPhone" runat="server" CssClass="modern-input" 
                                placeholder="+1 (555) 123-4567" />
                        </div>
                        <div class="form-group">
                            <label class="modern-label">Display Order</label>
                            <asp:TextBox ID="txtModalDisplayOrder" runat="server" CssClass="modern-input" 
                                TextMode="Number" placeholder="1" />
                            <small class="form-hint">Lower numbers appear first</small>
                        </div>
                    </div>
                    
                    <div class="form-group">
                        <label class="modern-label">Icon Class</label>
                        <asp:DropDownList ID="ddlModalIconClass" runat="server" CssClass="modern-select">
                            <asp:ListItem Value="fas fa-envelope" Text="Email (fas fa-envelope)" />
                            <asp:ListItem Value="fas fa-phone" Text="Phone (fas fa-phone)" />
                            <asp:ListItem Value="fab fa-linkedin" Text="LinkedIn (fab fa-linkedin)" />
                            <asp:ListItem Value="fab fa-github" Text="GitHub (fab fa-github)" />
                            <asp:ListItem Value="fas fa-map-marker-alt" Text="Location (fas fa-map-marker-alt)" />
                            <asp:ListItem Value="fab fa-twitter" Text="Twitter (fab fa-twitter)" />
                            <asp:ListItem Value="fab fa-instagram" Text="Instagram (fab fa-instagram)" />
                            <asp:ListItem Value="fas fa-globe" Text="Website (fas fa-globe)" />
                        </asp:DropDownList>
                        <div class="icon-preview-container" style="margin-top: 10px; text-align: center;">
                            <i id="iconPreview" class="fas fa-envelope" style="font-size: 32px; color: #a78bfa;"></i>
                        </div>
                    </div>
                </asp:Panel>
                
                <asp:Panel ID="pnlContactInfoView" runat="server" Visible="false">
                    <asp:Literal ID="ltlContactInfoDetails" runat="server" />
                </asp:Panel>
            </div>
            
            <div class="modal-footer-modern">
                <button type="button" class="btn light admin-btn" onclick="closeContactInfoModal()">Close</button>
                <asp:Button ID="btnSaveContactInfo" runat="server" Text="Save Contact Info" OnClick="btnSaveContactInfo_Click" 
                    CssClass="btn dark admin-btn" ValidationGroup="ContactInfoForm" Visible="false" />
            </div>
        </div>
    </div>

    <style>
        /* Portfolio-style Contact Info Admin Design */
        .contact-info-admin-container {
            min-height: 100vh;
            background: linear-gradient(135deg, #180322 0%, #3d1555 100%);
            color: #f3e8ff;
            padding: 20px 0;
            position: relative;
        }

        .contact-info-admin-background {
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

        .contact-info-admin-wrapper {
            position: relative;
            z-index: 1;
            max-width: 1400px;
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

        .contact-info-divider-posh {
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
        .contact-info-crud-card,
        .contact-info-preview-card {
            background: rgba(36, 10, 39, 0.97);
            border-radius: 18px;
            box-shadow: 0 8px 32px rgba(36, 10, 39, 0.18);
            overflow: hidden;
            transition: transform 0.2s, box-shadow 0.2s;
        }

        .contact-info-crud-card:hover,
        .contact-info-preview-card:hover {
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

        /* Contact Info Stats */
        .contact-info-stats {
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
        .contact-info-grid-container {
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

        /* Contact Info Display Elements */
        .contact-info-icon-preview {
            display: flex;
            align-items: center;
            justify-content: center;
        }

        .contact-type {
            color: #f3e8ff;
            font-weight: 500;
            font-size: 14px;
        }

        .contact-email,
        .contact-linkedin,
        .contact-github,
        .contact-phone {
            color: #f3e8ff;
            font-size: 14px;
        }

        .contact-linkedin a,
        .contact-github a {
            color: #a78bfa;
            text-decoration: none;
        }

        .contact-linkedin a:hover,
        .contact-github a:hover {
            color: #f472b6;
        }

        .display-order {
            background: rgba(59, 130, 246, 0.2);
            color: #60a5fa;
            padding: 4px 8px;
            border-radius: 12px;
            font-size: 12px;
            font-weight: 600;
            text-align: center;
            min-width: 30px;
            display: inline-block;
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
            min-width: 50px;
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
        .modern-select {
            width: 100%;
            padding: 12px;
            background: rgba(60, 16, 80, 0.8);
            border: 1px solid rgba(167, 139, 250, 0.3);
            border-radius: 8px;
            color: #f3e8ff;
            font-size: 14px;
            outline: none;
            transition: border-color 0.2s;
        }

        .modern-input.small {
            padding: 8px;
            font-size: 13px;
        }

        .modern-input:focus,
        .modern-select:focus {
            border-color: #a78bfa;
            box-shadow: 0 0 0 2px rgba(167, 139, 250, 0.2);
        }

        .modern-input::placeholder {
            color: rgba(243, 232, 255, 0.6);
        }

        /* Modal Styles */
        .modal-overlay {
            position: fixed;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: rgba(0, 0, 0, 0.8);
            backdrop-filter: blur(4px);
            z-index: 1000;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 20px;
        }

        .modal-container {
            background: rgba(36, 10, 39, 0.98);
            border-radius: 16px;
            max-width: 700px;
            width: 100%;
            max-height: 90vh;
            overflow: hidden;
            box-shadow: 0 20px 60px rgba(0, 0, 0, 0.5);
        }

        .modal-header-modern {
            padding: 25px 30px;
            background: linear-gradient(135deg, rgba(167, 139, 250, 0.1) 0%, rgba(244, 114, 182, 0.05) 100%);
            border-bottom: 1px solid rgba(167, 139, 250, 0.1);
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .modal-header-modern h3 {
            color: #a78bfa;
            margin: 0;
            font-size: 1.4rem;
        }

        .modal-close-btn {
            background: rgba(239, 68, 68, 0.2);
            border: none;
            color: #f87171;
            width: 32px;
            height: 32px;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            cursor: pointer;
            transition: all 0.2s;
        }

        .modal-close-btn:hover {
            background: #ef4444;
            color: white;
        }

        .modal-body-modern {
            padding: 30px;
            max-height: 60vh;
            overflow-y: auto;
        }

        .modal-footer-modern {
            padding: 20px 30px;
            background: rgba(20, 5, 25, 0.4);
            border-top: 1px solid rgba(167, 139, 250, 0.1);
            display: flex;
            gap: 10px;
            justify-content: flex-end;
        }

        /* Form Grid */
        .form-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 20px;
            margin-bottom: 20px;
        }

        .form-group {
            margin-bottom: 20px;
        }

        .modern-label {
            display: block;
            color: #a78bfa;
            font-weight: 500;
            margin-bottom: 8px;
            font-size: 14px;
        }

        .form-hint {
            color: #9ca3af;
            font-size: 12px;
            margin-top: 5px;
            display: block;
        }

        .validation-error {
            color: #f87171;
            font-size: 12px;
            margin-top: 5px;
            display: block;
        }

        .icon-preview-container {
            background: rgba(60, 16, 80, 0.6);
            border-radius: 8px;
            padding: 15px;
            border: 1px solid rgba(167, 139, 250, 0.2);
        }

        /* Portfolio Preview Styles */
        .portfolio-preview-container {
            background: #1f2937;
            border-radius: 12px;
            padding: 30px;
            color: white;
        }

        /* Contact Info Preview Grid */
        .contact-info-preview-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 20px;
            padding: 20px 0;
        }

        .contact-info-preview-item {
            background: rgba(255, 255, 255, 0.05);
            border: 1px solid rgba(255, 255, 255, 0.1);
            border-radius: 12px;
            padding: 20px;
            text-align: center;
            transition: transform 0.2s;
        }

        .contact-info-preview-item:hover {
            transform: translateY(-5px);
            background: rgba(255, 255, 255, 0.1);
        }

        .contact-info-preview-icon {
            font-size: 32px;
            color: #a78bfa;
            margin-bottom: 10px;
        }

        .contact-info-preview-type {
            font-weight: 600;
            margin-bottom: 8px;
            color: #f3e8ff;
        }

        .contact-info-preview-value {
            font-size: 12px;
            color: #cbd5e1;
            word-break: break-word;
        }

        /* Responsive Design */
        @media (max-width: 1024px) {
            .form-grid {
                grid-template-columns: 1fr;
            }
            
            .header-actions {
                flex-direction: column;
                gap: 8px;
            }
        }

        @media (max-width: 768px) {
            .contact-info-admin-wrapper {
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
            
            .contact-info-grid-container {
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
            
            .modal-container {
                margin: 10px;
                max-height: 95vh;
            }
            
            .modal-body-modern {
                padding: 20px;
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

    <script type="text/javascript">
        function closeContactInfoModal() {
            document.getElementById('contactInfoModal').style.display = 'none';
        }
        
        // Close modal when clicking outside
        document.addEventListener('click', function(e) {
            if (e.target.classList.contains('modal-overlay')) {
                closeContactInfoModal();
            }
        });
        
        // Icon preview functionality
        document.addEventListener('DOMContentLoaded', function() {
            var iconSelect = document.getElementById('<%= ddlModalIconClass.ClientID %>');
            var iconPreview = document.getElementById('iconPreview');
            
            if (iconSelect && iconPreview) {
                iconSelect.addEventListener('change', function() {
                    iconPreview.className = this.value;
                });
            }
        });
    </script>
</asp:Content>
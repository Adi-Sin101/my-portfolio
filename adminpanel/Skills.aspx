<%@ Page Title="Skills Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Skills.aspx.cs" Inherits="adminpanel.Skills" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Portfolio-style Skills Management -->
    <div class="skills-admin-container">
        <div class="skills-admin-background"></div>
        
        <div class="skills-admin-wrapper">
            <!-- Hero Section -->
            <div class="admin-hero-section">
                <div class="admin-hero-content">
                    <h1>Skills <span class="highlight">Management</span></h1>
                    <div class="skills-divider-posh"></div>
                    <p>Manage your technical skills and expertise levels</p>
                </div>
            </div>

            <!-- Skills CRUD Section -->
            <section class="skills-section admin-section">
                <div class="skills-crud-card">
                    <div class="card-header-modern">
                        <h3><i class="fas fa-code"></i> Skills Dashboard</h3>
                        <div class="header-actions">
                            <asp:Button ID="btnRefreshSkills" runat="server" Text="Refresh" OnClick="btnRefreshSkills_Click" 
                                CssClass="btn light admin-btn" />
                            <asp:Button ID="btnAddNewSkill" runat="server" Text="Add New Skill" OnClick="btnAddNewSkill_Click" 
                                CssClass="btn dark admin-btn" />
                            <asp:Button ID="btnBulkActions" runat="server" Text="Bulk Actions" OnClick="btnBulkActions_Click" 
                                CssClass="btn light admin-btn" />
                        </div>
                    </div>
                    
                    <div class="card-body-modern">
                        <asp:Label ID="lblSkillsCount" runat="server" CssClass="skills-card-label skills-stats" Text="Loading skills..." />
                        
                        <div class="skills-grid-container">
                            <asp:GridView ID="gvSkills" runat="server" 
                                CssClass="modern-table" 
                                AutoGenerateColumns="false" 
                                AllowPaging="true" 
                                PageSize="10"
                                OnPageIndexChanging="gvSkills_PageIndexChanging"
                                OnRowCommand="gvSkills_RowCommand"
                                OnRowEditing="gvSkills_RowEditing"
                                OnRowUpdating="gvSkills_RowUpdating"
                                OnRowCancelingEdit="gvSkills_RowCancelingEdit"
                                OnRowDeleting="gvSkills_RowDeleting"
                                EmptyDataText="No skills found. Add your first skill!"
                                DataKeyNames="Id">
                                <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="ID" Visible="false" />
                                    
                                    <asp:TemplateField HeaderText="Icon" ItemStyle-CssClass="icon-column">
                                        <ItemTemplate>
                                            <div class="skill-icon-preview">
                                                <img src="<%# Eval("IconUrl") %>" alt="<%# Eval("Name") %>" class="skill-icon" onerror="this.src='https://cdn.jsdelivr.net/gh/devicons/devicon/icons/html5/html5-original.svg'" />
                                            </div>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtIconUrl" runat="server" Text='<%# Eval("IconUrl") %>' 
                                                CssClass="modern-input" placeholder="Icon URL" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Skill Name" ItemStyle-CssClass="name-column">
                                        <ItemTemplate>
                                            <div class="skill-name"><%# Eval("Name") %></div>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("Name") %>' 
                                                CssClass="modern-input" MaxLength="100" />
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" 
                                                ControlToValidate="txtName" ErrorMessage="*" 
                                                CssClass="validation-error" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Proficiency" ItemStyle-CssClass="level-column">
                                        <ItemTemplate>
                                            <div class="skill-level-display">
                                                <div class="progress-container">
                                                    <div class="progress-bar-modern" style="width: <%# Eval("Percentage") %>%;">
                                                        <span class="progress-text"><%# Eval("Percentage") %>%</span>
                                                    </div>
                                                </div>
                                                <div class="skill-level-label"><%# GetSkillLevel(Convert.ToInt32(Eval("Percentage"))) %></div>
                                            </div>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtPercentage" runat="server" Text='<%# Eval("Percentage") %>' 
                                                CssClass="modern-input small" TextMode="Number" min="0" max="100" placeholder="75" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Order" ItemStyle-CssClass="order-column">
                                        <ItemTemplate>
                                            <div class="display-order"><%# Eval("DisplayOrder") %></div>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDisplayOrder" runat="server" Text='<%# Eval("DisplayOrder") %>' 
                                                CssClass="modern-input small" TextMode="Number" placeholder="1" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="actions-column">
                                        <ItemTemplate>
                                            <div class="action-buttons">
                                                <asp:Button ID="btnView" runat="server" Text="View" CommandName="ViewSkill" 
                                                    CommandArgument='<%# Eval("Id") %>' CssClass="action-btn view-btn" />
                                                <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="Edit" 
                                                    CssClass="action-btn edit-btn" />
                                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="Delete" 
                                                    CssClass="action-btn delete-btn"
                                                    OnClientClick="return confirm('Are you sure you want to delete this skill?');" />
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

            <!-- Skills Preview Section -->
            <section class="skills-section admin-section">
                <div class="skills-preview-card">
                    <div class="card-header-modern">
                        <h3><i class="fas fa-eye"></i> Portfolio Preview</h3>
                    </div>
                    <div class="card-body-modern">
                        <div class="portfolio-preview-container">
                            <h4 style="text-align: center; margin-bottom: 30px; color: #ffffff;">How Your Skills Appear on Portfolio</h4>
                            <asp:Literal ID="ltlSkillsPreview" runat="server" />
                        </div>
                    </div>
                </div>
            </section>

            <!-- Quick Stats Section -->
            <section class="skills-section admin-section">
                <div class="info-card">
                    <div class="info-content">
                        <div class="info-icon">
                            <i class="fas fa-chart-bar"></i>
                        </div>
                        <div class="info-text">
                            <h4>Skills Statistics</h4>
                            <asp:Literal ID="ltlSkillsStats" runat="server" />
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>

    <!-- Modal for Skill Details/Add/Edit -->
    <div class="modal-overlay" id="skillModal" style="display: none;">
        <div class="modal-container">
            <div class="modal-header-modern">
                <h3>
                    <asp:Literal ID="ltlModalTitle" runat="server" Text="Skill Details" />
                </h3>
                <button type="button" class="modal-close-btn" onclick="closeSkillModal()">
                    <i class="fas fa-times"></i>
                </button>
            </div>
            
            <div class="modal-body-modern">
                <asp:Panel ID="pnlSkillForm" runat="server" Visible="false">
                    <div class="form-grid">
                        <div class="form-group">
                            <label class="modern-label">Skill Name *</label>
                            <asp:TextBox ID="txtModalSkillName" runat="server" CssClass="modern-input" MaxLength="100" placeholder="e.g., JavaScript" />
                            <asp:RequiredFieldValidator ID="rfvModalSkillName" runat="server" 
                                ControlToValidate="txtModalSkillName" ErrorMessage="Skill name is required" 
                                CssClass="validation-error" ValidationGroup="SkillForm" />
                        </div>
                        <div class="form-group">
                            <label class="modern-label">Proficiency Level (%)</label>
                            <asp:TextBox ID="txtModalPercentage" runat="server" CssClass="modern-input" TextMode="Number" 
                                min="0" max="100" placeholder="75" />
                            <asp:RangeValidator ID="rvModalPercentage" runat="server" 
                                ControlToValidate="txtModalPercentage" MinimumValue="0" MaximumValue="100" 
                                Type="Integer" ErrorMessage="Percentage must be between 0 and 100" 
                                CssClass="validation-error" ValidationGroup="SkillForm" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="modern-label">Icon URL</label>
                        <asp:TextBox ID="txtModalIconUrl" runat="server" CssClass="modern-input" 
                            placeholder="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/javascript/javascript-original.svg" />
                        <div class="icon-preview" id="iconPreview" style="display: none; margin-top: 10px; text-align: center;">
                            <img id="previewImg" src="" alt="Icon Preview" style="width: 48px; height: 48px; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.2);" />
                        </div>
                        <small class="form-hint">Leave empty to auto-generate based on skill name</small>
                    </div>
                    <div class="form-grid">
                        <div class="form-group">
                            <label class="modern-label">Display Order</label>
                            <asp:TextBox ID="txtModalDisplayOrder" runat="server" CssClass="modern-input" 
                                TextMode="Number" placeholder="1" />
                            <small class="form-hint">Lower numbers appear first</small>
                        </div>
                        <div class="form-group">
                            <label class="modern-label">Category</label>
                            <asp:DropDownList ID="ddlModalCategory" runat="server" CssClass="modern-select">
                                <asp:ListItem Value="Frontend" Text="Frontend Development" />
                                <asp:ListItem Value="Backend" Text="Backend Development" />
                                <asp:ListItem Value="Database" Text="Database & Storage" />
                                <asp:ListItem Value="Tools" Text="Tools & Frameworks" />
                                <asp:ListItem Value="General" Text="General Programming" Selected="True" />
                            </asp:DropDownList>
                        </div>
                    </div>
                </asp:Panel>
                
                <asp:Panel ID="pnlSkillView" runat="server" Visible="false">
                    <asp:Literal ID="ltlSkillDetails" runat="server" />
                </asp:Panel>
            </div>
            
            <div class="modal-footer-modern">
                <button type="button" class="btn light admin-btn" onclick="closeSkillModal()">Close</button>
                <asp:Button ID="btnSaveSkill" runat="server" Text="Save Skill" OnClick="btnSaveSkill_Click" 
                    CssClass="btn dark admin-btn" ValidationGroup="SkillForm" Visible="false" />
            </div>
        </div>
    </div>

    <style>
        /* Portfolio-style Skills Admin Design */
        .skills-admin-container {
            min-height: 100vh;
            background: linear-gradient(135deg, #180322 0%, #3d1555 100%);
            color: #f3e8ff;
            padding: 20px 0;
            position: relative;
        }

        .skills-admin-background {
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

        .skills-admin-wrapper {
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

        .skills-divider-posh {
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
        .skills-crud-card,
        .skills-preview-card,
        .info-card,
        .cookie-demo-card {
            background: rgba(36, 10, 39, 0.97);
            border-radius: 18px;
            box-shadow: 0 8px 32px rgba(36, 10, 39, 0.18);
            overflow: hidden;
            transition: transform 0.2s, box-shadow 0.2s;
        }

        .skills-crud-card:hover,
        .skills-preview-card:hover {
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

        /* Skills Stats */
        .skills-stats {
            display: inline-block;
            background: linear-gradient(135deg, #a78bfa 0%, #f472b6 100%);
            color: #ffffff;
            padding: 8px 16px;
            border-radius: 20px;
            font-size: 14px;
            font-weight: 600;
            margin-bottom: 25px;
        }

        .skills-stats.empty {
            background: rgba(107, 114, 126, 0.3);
            color: #9ca3af;
        }

        .skills-stats.error {
            background: rgba(239, 68, 68, 0.3);
            color: #f87171;
        }

        /* Modern Table */
        .skills-grid-container {
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

        /* Skill Display Elements */
        .skill-icon-preview {
            display: flex;
            align-items: center;
            justify-content: center;
        }

        .skill-icon {
            width: 32px;
            height: 32px;
            border-radius: 4px;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
        }

        .skill-name {
            color: #f3e8ff;
            font-weight: 500;
            font-size: 14px;
        }

        .skill-level-display {
            width: 180px;
        }

        .progress-container {
            background: rgba(107, 114, 126, 0.3);
            border-radius: 20px;
            height: 18px;
            position: relative;
            overflow: hidden;
            margin-bottom: 5px;
        }

        .progress-bar-modern {
            background: linear-gradient(90deg, #a78bfa 0%, #f472b6 100%);
            height: 100%;
            border-radius: 20px;
            display: flex;
            align-items: center;
            justify-content: center;
            transition: width 0.3s ease;
            position: relative;
            min-width: 20px;
        }

        .progress-text {
            color: #ffffff;
            font-size: 11px;
            font-weight: 600;
            text-shadow: 0 1px 2px rgba(0, 0, 0, 0.5);
        }

        .skill-level-label {
            font-size: 11px;
            color: #cbd5e1;
            text-align: center;
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

        /* Skills Preview Styles */
        .skills-preview-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(150px, 1fr));
            gap: 20px;
            padding: 20px 0;
        }

        .skill-preview-item {
            background: rgba(255, 255, 255, 0.05);
            border: 1px solid rgba(255, 255, 255, 0.1);
            border-radius: 12px;
            padding: 20px;
            text-align: center;
            transition: transform 0.2s;
        }

        .skill-preview-item:hover {
            transform: translateY(-5px);
            background: rgba(255, 255, 255, 0.1);
        }

        .skill-preview-icon img {
            width: 40px;
            height: 40px;
            border-radius: 8px;
            margin-bottom: 10px;
        }

        .skill-preview-name {
            font-weight: 600;
            margin-bottom: 10px;
            color: #f3e8ff;
        }

        .skill-preview-progress {
            background: rgba(107, 114, 126, 0.3);
            border-radius: 10px;
            height: 8px;
            margin-bottom: 5px;
            overflow: hidden;
        }

        .progress-preview {
            background: linear-gradient(90deg, #60a5fa 0%, #a78bfa 100%);
            height: 100%;
            border-radius: 10px;
            transition: width 0.3s ease;
        }

        .skill-preview-percent {
            font-size: 12px;
            color: #cbd5e1;
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

        /* Skill Details Styles */
        .skill-details-header {
            display: flex;
            align-items: center;
            gap: 20px;
            margin-bottom: 30px;
            padding: 20px;
            background: rgba(167, 139, 250, 0.1);
            border-radius: 12px;
        }

        .skill-icon-large img {
            width: 64px;
            height: 64px;
            border-radius: 12px;
            box-shadow: 0 4px 16px rgba(0, 0, 0, 0.2);
        }

        .skill-info h4 {
            color: #f3e8ff;
            margin: 0 0 5px 0;
            font-size: 1.5rem;
        }

        .skill-category {
            color: #a78bfa;
            font-size: 14px;
            font-weight: 500;
        }

        .skill-details-content .detail-item {
            margin-bottom: 20px;
        }

        .skill-details-content .detail-item strong {
            color: #a78bfa;
            display: block;
            margin-bottom: 10px;
        }

        .skill-progress-large {
            background: rgba(107, 114, 126, 0.3);
            border-radius: 25px;
            height: 30px;
            overflow: hidden;
        }

        .progress-bar-large {
            background: linear-gradient(90deg, #a78bfa 0%, #f472b6 100%);
            height: 100%;
            display: flex;
            align-items: center;
            justify-content: center;
            border-radius: 25px;
            transition: width 0.3s ease;
        }

        .progress-bar-large span {
            color: #ffffff;
            font-weight: 600;
            text-shadow: 0 1px 2px rgba(0, 0, 0, 0.5);
        }

        /* Info Card */
        .info-card {
            background: linear-gradient(135deg, rgba(59, 130, 246, 0.1) 0%, rgba(147, 51, 234, 0.1) 100%);
            border: 1px solid rgba(59, 130, 246, 0.2);
        }

        .info-content {
            padding: 25px 30px;
            display: flex;
            align-items: flex-start;
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
            margin: 0 0 15px 0;
            font-size: 1.2rem;
        }

        .info-text p {
            color: #cbd5e1;
            margin: 5px 0;
            line-height: 1.6;
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

            .skills-preview-grid {
                grid-template-columns: repeat(auto-fit, minmax(120px, 1fr));
            }
        }

        @media (max-width: 768px) {
            .skills-admin-wrapper {
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
            
            .skills-grid-container {
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
            
            .modal-container {
                margin: 10px;
                max-height: 95vh;
            }
            
            .modal-body-modern {
                padding: 20px;
            }

            .skill-level-display {
                width: 120px;
            }

            .skill-details-header {
                flex-direction: column;
                text-align: center;
            }
        }

        /* Animation for page load */
        .admin-section {
            animation: fadeInUp 0.6s ease forwards;
            opacity: 0;
        }

        .admin-section:nth-child(1) { animation-delay: 0.1s; }
        .admin-section:nth-child(2) { animation-delay: 0.2s; }
        .admin-section:nth-child(3) { animation-delay: 0.3s; }

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
        function closeSkillModal() {
            document.getElementById('skillModal').style.display = 'none';
        }
        
        // Close modal when clicking outside
        document.addEventListener('click', function(e) {
            if (e.target.classList.contains('modal-overlay')) {
                closeSkillModal();
            }
        });
        
        // Icon preview functionality
        document.addEventListener('DOMContentLoaded', function() {
            var iconInput = document.getElementById('<%= txtModalIconUrl.ClientID %>');
            if (iconInput) {
                iconInput.addEventListener('input', function() {
                    var url = this.value.trim();
                    var preview = document.getElementById('iconPreview');
                    var img = document.getElementById('previewImg');
                    
                    if (url && url !== '') {
                        img.src = url;
                        preview.style.display = 'block';
                        
                        img.onerror = function() {
                            preview.style.display = 'none';
                        };
                    } else {
                        preview.style.display = 'none';
                    }
                });
            }
            
            // Enhanced table interactions
            const tableRows = document.querySelectorAll('.modern-table tbody tr');
            tableRows.forEach(row => {
                row.addEventListener('mouseenter', function() {
                    this.style.transform = 'translateX(5px)';
                });
                row.addEventListener('mouseleave', function() {
                    this.style.transform = 'translateX(0)';
                });
            });
        });
    </script>
</asp:Content>

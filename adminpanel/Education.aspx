<%@ Page Title="Education Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Education.aspx.cs" Inherits="adminpanel.Education" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Education Management Panel -->
    <div class="education-management-container">
        <!-- Header Section -->
        <div class="education-header">
            <div class="education-title">
                <h2><i class="fas fa-graduation-cap"></i> Education Management</h2>
                <p>Manage your educational background and qualifications</p>
            </div>
            <div class="education-actions">
                <asp:Button ID="btnAddNew" runat="server" Text="Add New Education" CssClass="btn btn-primary" OnClick="BtnAddNew_Click" />
                <asp:Button ID="btnPreview" runat="server" Text="Preview Portfolio" CssClass="btn btn-secondary" OnClick="BtnPreview_Click" />
            </div>
        </div>

        <!-- Statistics Cards -->
        <div class="education-stats-row">
            <div class="education-stat-card">
                <div class="education-stat-icon">
                    <i class="fas fa-list"></i>
                </div>
                <div class="education-stat-content">
                    <asp:Label ID="lblEducationCount" runat="server" CssClass="education-stat-number" Text="0" />
                    <span class="education-stat-label">Total Records</span>
                </div>
            </div>
            
            <div class="education-stat-card">
                <div class="education-stat-icon">
                    <i class="fas fa-university"></i>
                </div>
                <div class="education-stat-content">
                    <asp:Label ID="lblDegreeCount" runat="server" CssClass="education-stat-number" Text="0" />
                    <span class="education-stat-label">Degrees</span>
                </div>
            </div>
            
            <div class="education-stat-card">
                <div class="education-stat-icon">
                    <i class="fas fa-clock"></i>
                </div>
                <div class="education-stat-content">
                    <asp:Label ID="lblLastUpdated" runat="server" CssClass="education-stat-number" Text="Never" />
                    <span class="education-stat-label">Last Updated</span>
                </div>
            </div>
        </div>

        <!-- Message Display -->
        <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="education-message">
            <asp:Label ID="lblMessage" runat="server" />
        </asp:Panel>

        <!-- Add/Edit Form -->
        <asp:Panel ID="pnlAddEdit" runat="server" Visible="false" CssClass="education-form-panel">
            <div class="education-form-header">
                <h3><asp:Label ID="lblFormMode" runat="server" Text="Add New Education" /></h3>
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-light" OnClick="BtnCancel_Click" />
            </div>
            
            <asp:HiddenField ID="hdnEducationId" runat="server" />
            
            <div class="education-form-body">
                <div class="form-row">
                    <div class="form-group">
                        <label for="<%= txtDegree.ClientID %>">Degree/Level *</label>
                        <asp:TextBox ID="txtDegree" runat="server" CssClass="form-control" placeholder="e.g., B.Sc in Computer Science, HSC, SSC" MaxLength="200" />
                        <asp:RequiredFieldValidator ID="rfvDegree" runat="server" ControlToValidate="txtDegree" 
                            ErrorMessage="Degree is required" CssClass="field-validation-error" ValidationGroup="Education" />
                    </div>
                    
                    <div class="form-group">
                        <label for="<%= txtInstitution.ClientID %>">Institution *</label>
                        <asp:TextBox ID="txtInstitution" runat="server" CssClass="form-control" placeholder="e.g., Khulna University of Engineering & Technology" MaxLength="200" />
                        <asp:RequiredFieldValidator ID="rfvInstitution" runat="server" ControlToValidate="txtInstitution" 
                            ErrorMessage="Institution is required" CssClass="field-validation-error" ValidationGroup="Education" />
                    </div>
                </div>
                
                <div class="form-row">
                    <div class="form-group">
                        <label for="<%= txtYear.ClientID %>">Year</label>
                        <asp:TextBox ID="txtYear" runat="server" CssClass="form-control" placeholder="e.g., 2021, Expected: 2027" MaxLength="50" />
                        <small class="form-text">Enter year or expected year (e.g., "2021" or "Expected: 2027")</small>
                    </div>
                    
                    <div class="form-group">
                        <label for="<%= txtGrade.ClientID %>">Grade/CGPA</label>
                        <asp:TextBox ID="txtGrade" runat="server" CssClass="form-control" placeholder="e.g., 3.28 / 4.00, 5.00" MaxLength="100" />
                        <small class="form-text">Enter GPA, CGPA, or grade (e.g., "3.28 / 4.00" or "5.00")</small>
                    </div>
                </div>
                
                <div class="form-actions">
                    <asp:Button ID="btnSave" runat="server" Text="Save Education" CssClass="btn btn-success" OnClick="BtnSave_Click" ValidationGroup="Education" />
                    <asp:Button ID="btnCancelForm" runat="server" Text="Cancel" CssClass="btn btn-secondary" OnClick="BtnCancel_Click" />
                </div>
            </div>
        </asp:Panel>

        <!-- Education Grid -->
        <div class="education-grid-panel">
            <div class="education-grid-header">
                <h3>Current Education Records</h3>
                <div class="education-grid-controls">
                    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="btn btn-outline-primary btn-sm" OnClick="BtnRefresh_Click" />
                </div>
            </div>
            
            <div class="education-grid-container">
                <asp:GridView ID="gvEducation" runat="server" AutoGenerateColumns="False" CssClass="education-grid"
                    DataKeyNames="Id" OnRowCommand="GvEducation_RowCommand" OnRowDataBound="GvEducation_RowDataBound"
                    EmptyDataText="No education records found. Click 'Add New Education' to get started.">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" ItemStyle-Width="60px" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" />
                        
                        <asp:TemplateField HeaderText="Degree" ItemStyle-Width="200px">
                            <ItemTemplate>
                                <div class="education-degree">
                                    <strong><%# Eval("Degree") %></strong>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Institution" ItemStyle-Width="250px">
                            <ItemTemplate>
                                <div class="education-institution">
                                    <%# Eval("Institution") %>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Year" ItemStyle-Width="120px">
                            <ItemTemplate>
                                <div class="education-year">
                                    <%# Eval("Year") %>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Grade" ItemStyle-Width="120px">
                            <ItemTemplate>
                                <div class="education-grade">
                                    <%# Eval("Grade") %>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px" ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <div class="education-actions">
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditEducation" CommandArgument='<%# Eval("Id") %>'
                                        CssClass="btn btn-sm btn-outline-primary" ToolTip="Edit">
                                        <i class="fas fa-edit"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteEducation" CommandArgument='<%# Eval("Id") %>'
                                        CssClass="btn btn-sm btn-outline-danger" ToolTip="Delete"
                                        OnClientClick="return confirm('Are you sure you want to delete this education record?');">
                                        <i class="fas fa-trash"></i>
                                    </asp:LinkButton>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="education-empty-state">
                            <i class="fas fa-graduation-cap fa-3x"></i>
                            <h4>No Education Records</h4>
                            <p>You haven't added any education records yet.</p>
                            <asp:Button ID="btnAddFirst" runat="server" Text="Add Your First Education" CssClass="btn btn-primary" OnClick="BtnAddNew_Click" />
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>

        <!-- Preview Section -->
        <asp:Panel ID="pnlPreview" runat="server" Visible="false" CssClass="education-preview-panel">
            <div class="education-preview-header">
                <h3>Portfolio Preview</h3>
                <p>This is how your education section will appear on your portfolio website</p>
            </div>
            
            <div class="education-preview-content">
                <!-- Education Timeline Preview -->
                <div class="education-timeline-preview">
                    <div class="timeline-line"></div>
                    <asp:Repeater ID="rptEducationPreview" runat="server">
                        <ItemTemplate>
                            <div class="education-item-preview">
                                <div class="timeline-dot"></div>
                                <div class="education-card-preview">
                                    <h4><%# Eval("Degree") %></h4>
                                    <p class="institution"><%# Eval("Institution") %></p>
                                    <%# !string.IsNullOrEmpty(Eval("Year").ToString()) ? "<p class='year-info'>" + Eval("Year") + "</p>" : "" %>
                                    <%# !string.IsNullOrEmpty(Eval("Grade").ToString()) ? "<p class='grade'>" + FormatGrade(Eval("Grade").ToString(), Eval("Degree").ToString()) + "</p>" : "" %>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </asp:Panel>
    </div>

    <!-- Education Management Styles -->
    <style>
        .education-management-container {
            padding: 20px;
            background: #f8f9fa;
            min-height: calc(100vh - 200px);
        }

        .education-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            background: white;
            padding: 25px;
            border-radius: 12px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            margin-bottom: 25px;
        }

        .education-title h2 {
            color: #a78bfa;
            margin: 0 0 5px 0;
            font-size: 1.8rem;
            font-weight: 600;
        }

        .education-title p {
            color: #6b7280;
            margin: 0;
            font-size: 1rem;
        }

        .education-actions {
            display: flex;
            gap: 10px;
        }

        .education-stats-row {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
            gap: 20px;
            margin-bottom: 25px;
        }

        .education-stat-card {
            background: white;
            padding: 20px;
            border-radius: 12px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            display: flex;
            align-items: center;
            gap: 15px;
        }

        .education-stat-icon {
            width: 50px;
            height: 50px;
            background: linear-gradient(135deg, #a78bfa 0%, #f472b6 100%);
            border-radius: 12px;
            display: flex;
            align-items: center;
            justify-content: center;
            color: white;
            font-size: 20px;
        }

        .education-stat-number {
            font-size: 1.5rem;
            font-weight: 700;
            color: #1f2937;
            display: block;
        }

        .education-stat-label {
            color: #6b7280;
            font-size: 0.9rem;
        }

        .education-message {
            margin-bottom: 20px;
            padding: 15px 20px;
            border-radius: 8px;
            font-weight: 500;
        }

        .education-message.success {
            background-color: #d1fae5;
            color: #065f46;
            border-left: 4px solid #10b981;
        }

        .education-message.error {
            background-color: #fee2e2;
            color: #991b1b;
            border-left: 4px solid #ef4444;
        }

        .education-form-panel {
            background: white;
            border-radius: 12px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            margin-bottom: 25px;
            overflow: hidden;
        }

        .education-form-header {
            background: linear-gradient(135deg, #a78bfa 0%, #f472b6 100%);
            color: white;
            padding: 20px 25px;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .education-form-header h3 {
            margin: 0;
            font-size: 1.3rem;
            font-weight: 600;
        }

        .education-form-body {
            padding: 25px;
        }

        .form-row {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 20px;
            margin-bottom: 20px;
        }

        .form-group {
            display: flex;
            flex-direction: column;
        }

        .form-group label {
            font-weight: 600;
            color: #374151;
            margin-bottom: 8px;
            font-size: 0.95rem;
        }

        .form-control {
            padding: 12px 15px;
            border: 2px solid #e5e7eb;
            border-radius: 8px;
            font-size: 1rem;
            transition: all 0.3s ease;
        }

        .form-control:focus {
            outline: none;
            border-color: #a78bfa;
            box-shadow: 0 0 0 3px rgba(167, 139, 250, 0.1);
        }

        .form-text {
            color: #6b7280;
            font-size: 0.85rem;
            margin-top: 5px;
        }

        .field-validation-error {
            color: #ef4444;
            font-size: 0.85rem;
            margin-top: 5px;
        }

        .form-actions {
            display: flex;
            gap: 10px;
            margin-top: 25px;
            padding-top: 20px;
            border-top: 1px solid #e5e7eb;
        }

        .education-grid-panel {
            background: white;
            border-radius: 12px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            overflow: hidden;
        }

        .education-grid-header {
            background: #f8fafc;
            padding: 20px 25px;
            border-bottom: 1px solid #e5e7eb;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .education-grid-header h3 {
            margin: 0;
            color: #1f2937;
            font-size: 1.2rem;
            font-weight: 600;
        }

        .education-grid-container {
            overflow-x: auto;
        }

        .education-grid {
            width: 100%;
            border-collapse: collapse;
        }

        .education-grid th {
            background: #f8fafc;
            padding: 15px 12px;
            text-align: left;
            font-weight: 600;
            color: #374151;
            border-bottom: 2px solid #e5e7eb;
            font-size: 0.9rem;
        }

        .education-grid td {
            padding: 15px 12px;
            border-bottom: 1px solid #f3f4f6;
            vertical-align: middle;
        }

        .education-grid tr:hover {
            background-color: #f9fafb;
        }

        .education-degree strong {
            color: #1f2937;
            font-weight: 600;
        }

        .education-institution {
            color: #6b7280;
            font-size: 0.95rem;
        }

        .education-year {
            color: #374151;
            font-weight: 500;
        }

        .education-grade {
            color: #059669;
            font-weight: 600;
        }

        .education-actions {
            display: flex;
            gap: 5px;
        }

        .education-empty-state {
            text-align: center;
            padding: 60px 20px;
            color: #6b7280;
        }

        .education-empty-state i {
            color: #d1d5db;
            margin-bottom: 20px;
        }

        .education-empty-state h4 {
            color: #374151;
            margin-bottom: 10px;
        }

        .education-preview-panel {
            background: white;
            border-radius: 12px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            margin-top: 25px;
            overflow: hidden;
        }

        .education-preview-header {
            background: linear-gradient(135deg, #374151 0%, #4b5563 100%);
            color: white;
            padding: 20px 25px;
            text-align: center;
        }

        .education-preview-header h3 {
            margin: 0 0 5px 0;
            font-size: 1.3rem;
        }

        .education-preview-header p {
            margin: 0;
            opacity: 0.9;
        }

        .education-preview-content {
            padding: 40px 25px;
        }

        /* Timeline Preview Styles */
        .education-timeline-preview {
            position: relative;
            max-width: 800px;
            margin: 0 auto;
        }

        .timeline-line {
            position: absolute;
            left: 20px;
            top: 0;
            bottom: 0;
            width: 2px;
            background: linear-gradient(to bottom, #a78bfa, #f472b6);
        }

        .education-item-preview {
            position: relative;
            margin-bottom: 30px;
            padding-left: 60px;
        }

        .timeline-dot {
            position: absolute;
            left: -20px;
            top: 10px;
            width: 12px;
            height: 12px;
            background: #a78bfa;
            border-radius: 50%;
            border: 3px solid white;
            box-shadow: 0 0 0 3px #a78bfa;
        }

        .education-card-preview {
            background: linear-gradient(135deg, rgba(167, 139, 250, 0.1) 0%, rgba(244, 114, 182, 0.05) 100%);
            padding: 20px;
            border-radius: 12px;
            border: 1px solid rgba(167, 139, 250, 0.2);
        }

        .education-card-preview h4 {
            color: #1f2937;
            margin: 0 0 8px 0;
            font-size: 1.1rem;
            font-weight: 600;
        }

        .education-card-preview .institution {
            color: #6b7280;
            margin: 0 0 8px 0;
            font-style: italic;
        }

        .education-card-preview .year-info {
            color: #059669;
            margin: 0 0 8px 0;
            font-weight: 500;
        }

        .education-card-preview .grade {
            color: #7c3aed;
            margin: 0;
            font-weight: 600;
        }

        /* Button Styles */
        .btn {
            padding: 10px 20px;
            border-radius: 8px;
            font-weight: 500;
            text-decoration: none;
            display: inline-block;
            border: none;
            cursor: pointer;
            transition: all 0.3s ease;
            font-size: 0.9rem;
        }

        .btn-primary {
            background: linear-gradient(135deg, #a78bfa 0%, #f472b6 100%);
            color: white;
        }

        .btn-primary:hover {
            transform: translateY(-2px);
            box-shadow: 0 8px 25px rgba(167, 139, 250, 0.4);
        }

        .btn-secondary {
            background: #6b7280;
            color: white;
        }

        .btn-secondary:hover {
            background: #4b5563;
        }

        .btn-success {
            background: #10b981;
            color: white;
        }

        .btn-success:hover {
            background: #059669;
        }

        .btn-light {
            background: white;
            color: #374151;
            border: 1px solid #d1d5db;
        }

        .btn-light:hover {
            background: #f9fafb;
        }

        .btn-outline-primary {
            background: transparent;
            color: #a78bfa;
            border: 1px solid #a78bfa;
        }

        .btn-outline-primary:hover {
            background: #a78bfa;
            color: white;
        }

        .btn-outline-danger {
            background: transparent;
            color: #ef4444;
            border: 1px solid #ef4444;
        }

        .btn-outline-danger:hover {
            background: #ef4444;
            color: white;
        }

        .btn-sm {
            padding: 6px 12px;
            font-size: 0.8rem;
        }

        /* Responsive Design */
        @media (max-width: 768px) {
            .education-header {
                flex-direction: column;
                gap: 15px;
                text-align: center;
            }

            .form-row {
                grid-template-columns: 1fr;
            }

            .education-stats-row {
                grid-template-columns: 1fr;
            }

            .education-grid-header {
                flex-direction: column;
                gap: 10px;
                text-align: center;
            }

            .form-actions {
                flex-direction: column;
            }
        }
    </style>
</asp:Content>
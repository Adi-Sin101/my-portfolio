<%@ Page Title="Experience Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Experience.aspx.cs" Inherits="adminpanel.Experience" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <div class="row">
            <div class="col-12">
                <h2 class="mb-4">Experience Management</h2>
                
                <!-- Add/Edit Experience Form -->
                <div class="card mb-4">
                    <div class="card-header bg-success text-white">
                        <h4 class="mb-0" id="formTitle" runat="server">Add New Experience</h4>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label for="txtTitle">Job Title:</label>
                                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" placeholder="e.g., Software Developer" />
                                    <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtTitle" 
                                        ErrorMessage="Job title is required" CssClass="text-danger" ValidationGroup="AddExperience" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label for="txtCompany">Company/Organization:</label>
                                    <asp:TextBox ID="txtCompany" runat="server" CssClass="form-control" placeholder="e.g., Tech Solutions Inc." />
                                    <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ControlToValidate="txtCompany" 
                                        ErrorMessage="Company is required" CssClass="text-danger" ValidationGroup="AddExperience" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group mb-3">
                                    <label for="txtDuration">Duration (Display):</label>
                                    <asp:TextBox ID="txtDuration" runat="server" CssClass="form-control" placeholder="Jan 2024 - Present" />
                                    <small class="text-muted">Optional: Auto-calculated from dates if left empty</small>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group mb-3">
                                    <label for="txtStartDate">Start Date:</label>
                                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="Date" />
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group mb-3">
                                    <label for="txtEndDate">End Date (Optional):</label>
                                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" TextMode="Date" />
                                    <small class="text-muted">Leave empty if current position</small>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group mb-3">
                                    <label for="txtDescription">Description:</label>
                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" 
                                        Rows="4" placeholder="Describe your role, responsibilities, and achievements..." />
                                    <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription" 
                                        ErrorMessage="Description is required" CssClass="text-danger" ValidationGroup="AddExperience" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label for="FileUpload1">Experience Image:</label>
                                    <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control" accept="image/*" />
                                    <small class="text-muted">Optional: Company logo or related image</small>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group mb-3" id="currentImageDiv" runat="server" visible="false">
                                    <label>Current Image:</label><br />
                                    <asp:Image ID="imgCurrent" runat="server" CssClass="img-thumbnail" style="max-width: 150px; max-height: 100px;" />
                                    <br /><small class="text-muted">Leave file upload empty to keep current image</small>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Button ID="btnAddExperience" runat="server" Text="Add Experience" OnClick="btnAddExperience_Click" 
                                CssClass="btn btn-success me-3" ValidationGroup="AddExperience" />
                            <asp:Button ID="btnUpdateExperience" runat="server" Text="Update Experience" OnClick="btnUpdateExperience_Click" 
                                CssClass="btn btn-primary me-3" ValidationGroup="AddExperience" Visible="false" />
                            <asp:Button ID="btnCancelEdit" runat="server" Text="Cancel Edit" OnClick="btnCancelEdit_Click" 
                                CssClass="btn btn-secondary" Visible="false" />
                        </div>
                        <asp:HiddenField ID="hdnExperienceId" runat="server" />
                        <asp:HiddenField ID="hdnCurrentImagePath" runat="server" />
                    </div>
                </div>

                <!-- Experience Grid -->
                <div class="card">
                    <div class="card-header bg-info text-white">
                        <h4 class="mb-0">Work Experience</h4>
                    </div>
                    <div class="card-body">
                        <asp:GridView ID="gvExperience" runat="server" CssClass="table table-striped table-hover" 
                            AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="gvExperience_RowCommand"
                            EmptyDataText="No experience records found. Add your first experience above!">
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="ID" ItemStyle-Width="50px" />
                                <asp:TemplateField HeaderText="Image" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <img src="<%# Eval("ImagePath") %>" alt="<%# Eval("Title") %>" 
                                            style="width: 60px; height: 40px; object-fit: cover;" class="img-thumbnail" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Title" HeaderText="Job Title" ItemStyle-Width="200px" />
                                <asp:BoundField DataField="Company" HeaderText="Company" ItemStyle-Width="200px" />
                                <asp:BoundField DataField="Duration" HeaderText="Duration" ItemStyle-Width="150px" />
                                <asp:TemplateField HeaderText="Description" ItemStyle-Width="300px">
                                    <ItemTemplate>
                                        <div style="max-height: 60px; overflow: hidden; text-overflow: ellipsis;">
                                            <%# Eval("Description") %>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px">
                                    <ItemTemplate>
                                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="EditExperience" 
                                            CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-warning me-1" />
                                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="DeleteExperience" 
                                            CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-danger"
                                            OnClientClick="return confirm('Are you sure you want to delete this experience?');" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
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
        .table th {
            background-color: #f8f9fa;
            border-top: none;
        }
        .me-1 {
            margin-right: 0.25rem;
        }
        .img-thumbnail {
            border: 1px solid #dee2e6;
        }
    </style>
</asp:Content>

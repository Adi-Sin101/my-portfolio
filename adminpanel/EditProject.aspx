<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditProject.aspx.cs" Inherits="adminpanel.EditProject" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Edit Project - Admin Panel</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <div class="row">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header bg-primary text-white">
                        <h4 class="mb-0">
                            <i class="fas fa-edit me-2"></i>Edit Project
                        </h4>
                    </div>
                    <div class="card-body">
                        <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert">
                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        </asp:Panel>

                        <div class="row">
                            <div class="col-md-8">
                                <div class="mb-3">
                                    <label for="txtTitle" class="form-label">Project Title *</label>
                                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" placeholder="Enter project title" />
                                    <asp:RequiredFieldValidator ID="rfvTitle" runat="server" 
                                        ControlToValidate="txtTitle" 
                                        ErrorMessage="Project title is required" 
                                        CssClass="text-danger" 
                                        Display="Dynamic" />
                                </div>

                                <div class="mb-3">
                                    <label for="txtDescription" class="form-label">Description *</label>
                                    <asp:TextBox ID="txtDescription" runat="server" 
                                        TextMode="MultiLine" 
                                        Rows="4" 
                                        CssClass="form-control" 
                                        placeholder="Enter project description" />
                                    <asp:RequiredFieldValidator ID="rfvDescription" runat="server" 
                                        ControlToValidate="txtDescription" 
                                        ErrorMessage="Description is required" 
                                        CssClass="text-danger" 
                                        Display="Dynamic" />
                                </div>

                                <div class="mb-3">
                                    <label for="txtTechUsed" class="form-label">Technologies Used</label>
                                    <asp:TextBox ID="txtTechUsed" runat="server" 
                                        CssClass="form-control" 
                                        placeholder="e.g., C#, ASP.NET, SQL Server" />
                                </div>

                                <div class="mb-3">
                                    <label for="txtGitUrl" class="form-label">GitHub URL</label>
                                    <asp:TextBox ID="txtGitUrl" runat="server" 
                                        CssClass="form-control" 
                                        placeholder="https://github.com/username/repository" />
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="mb-3">
                                    <label class="form-label">Current Image</label>
                                    <div id="currentImageDiv" runat="server" visible="false" class="mb-2">
                                        <asp:Image ID="imgCurrent" runat="server" 
                                            CssClass="img-thumbnail" 
                                            Width="200px" 
                                            Height="150px" 
                                            style="object-fit: cover;" />
                                        <br />
                                        <small class="text-muted">Current project image</small>
                                    </div>
                                </div>

                                <div class="mb-3">
                                    <label for="FileUpload1" class="form-label">Update Image</label>
                                    <asp:FileUpload ID="FileUpload1" runat="server" 
                                        CssClass="form-control" 
                                        accept="image/*" />
                                    <div class="form-text">Leave empty to keep current image</div>
                                </div>
                            </div>
                        </div>

                        <div class="row mt-4">
                            <div class="col-md-12">
                                <asp:Button ID="btnUpdate" runat="server" 
                                    Text="Update Project" 
                                    OnClick="btnUpdate_Click" 
                                    CssClass="btn btn-success me-2" />
                                <asp:Button ID="btnCancel" runat="server" 
                                    Text="Cancel" 
                                    OnClick="btnCancel_Click" 
                                    CssClass="btn btn-secondary" 
                                    CausesValidation="false" />
                                <asp:Button ID="btnDelete" runat="server" 
                                    Text="Delete Project" 
                                    OnClick="btnDelete_Click" 
                                    CssClass="btn btn-danger float-end" 
                                    OnClientClick="return confirm('Are you sure you want to delete this project? This action cannot be undone.');" 
                                    CausesValidation="false" />
                            </div>
                        </div>

                        <asp:HiddenField ID="hdnProjectId" runat="server" />
                        <asp:HiddenField ID="hdnCurrentImagePath" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
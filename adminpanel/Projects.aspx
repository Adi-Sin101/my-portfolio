<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Projects.aspx.cs" Inherits="adminpanel.Projects" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Projects - Admin Panel</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
            background-color: #f5f5f5;
        }
        .container {
            max-width: 1200px;
            margin: 0 auto;
            background-color: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }
        .header {
            background-color: #007bff;
            color: white;
            padding: 15px;
            margin: -20px -20px 20px -20px;
            border-radius: 8px 8px 0 0;
        }
        .form-section {
            background-color: #f8f9fa;
            padding: 20px;
            border-radius: 5px;
            margin-bottom: 30px;
        }
        .form-group {
            margin-bottom: 15px;
        }
        .form-group label {
            display: block;
            margin-bottom: 5px;
            font-weight: bold;
            color: #333;
        }
        .form-control {
            width: 100%;
            padding: 8px;
            border: 1px solid #ddd;
            border-radius: 4px;
            font-size: 14px;
        }
        .btn {
            padding: 10px 20px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 14px;
        }
        .btn-primary {
            background-color: #007bff;
            color: white;
        }
        .btn-primary:hover {
            background-color: #0056b3;
        }
        .grid-container {
            margin-top: 20px;
        }
        .grid-view {
            width: 100%;
            border-collapse: collapse;
            margin-top: 10px;
        }
        .grid-view th, .grid-view td {
            padding: 12px;
            text-align: left;
            border-bottom: 1px solid #ddd;
        }
        .grid-view th {
            background-color: #f8f9fa;
            font-weight: bold;
            color: #495057;
        }
        .grid-view tr:hover {
            background-color: #f5f5f5;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="header">
                <h1>Projects Management</h1>
                <p>Manage your portfolio projects</p>
            </div>

            <!-- Add/Edit Project Form -->
            <div class="form-section">
                <h2 id="formTitle" runat="server">Add New Project</h2>
                <div class="form-group">
                    <label for="txtTitle">Project Title:</label>
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" placeholder="Enter project title" />
                </div>
                <div class="form-group">
                    <label for="txtDescription">Description:</label>
                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" placeholder="Enter project description" />
                </div>
                <div class="form-group">
                    <label for="txtTech">Technologies Used:</label>
                    <asp:TextBox ID="txtTech" runat="server" CssClass="form-control" placeholder="e.g., C#, ASP.NET, SQL Server" />
                </div>
                <div class="form-group">
                    <label for="txtGitUrl">GitHub URL:</label>
                    <asp:TextBox ID="txtGitUrl" runat="server" CssClass="form-control" placeholder="https://github.com/username/repository" />
                </div>
                <div class="form-group">
                    <label for="FileUpload1">Project Image:</label>
                    <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control" accept="image/*" />
                    <div id="currentImageDiv" runat="server" visible="false" style="margin-top: 10px;">
                        <small>Current image:</small><br />
                        <asp:Image ID="imgCurrentEdit" runat="server" Width="100px" Height="75px" style="object-fit: cover; border-radius: 4px;" />
                        <br /><small class="text-muted">Leave file upload empty to keep current image</small>
                    </div>
                </div>
                <asp:Button ID="btnAdd" runat="server" Text="Add Project" OnClick="btnAdd_Click" CssClass="btn btn-primary" />
                <asp:Button ID="btnUpdate" runat="server" Text="Update Project" OnClick="btnUpdate_Click" CssClass="btn btn-primary" Visible="false" style="margin-right: 10px;" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel Edit" OnClick="btnCancel_Click" CssClass="btn" Visible="false" style="background-color: #6c757d; color: white;" />
                
                <asp:HiddenField ID="hdnEditingProjectId" runat="server" />
                <asp:HiddenField ID="hdnCurrentImagePath" runat="server" />
            </div>

            <!-- Projects Grid -->
            <div class="grid-container">
                <h2>Existing Projects</h2>
                <asp:GridView ID="gvProjects" runat="server" 
                    AutoGenerateColumns="False" 
                    DataKeyNames="Id" 
                    OnRowCommand="gvProjects_RowCommand"
                    CssClass="grid-view"
                    EmptyDataText="No projects found. Add your first project above!">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="Title" HeaderText="Title" ItemStyle-Width="200px" />
                        <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="300px" />
                        <asp:BoundField DataField="TechUsed" HeaderText="Technologies" ItemStyle-Width="150px" />
                        <asp:BoundField DataField="GitUrl" HeaderText="GitHub URL" ItemStyle-Width="200px" />
                        <asp:TemplateField HeaderText="Image" ItemStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Image ID="imgProject" runat="server" 
                                    ImageUrl='<%# Eval("ImagePath") %>' 
                                    Width="80px" Height="60px" 
                                    style="object-fit: cover; border-radius: 4px;"
                                    AlternateText="Project Image" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" 
                                    Text="Edit" 
                                    CommandName="EditProject" 
                                    CommandArgument='<%# Container.DataItemIndex %>'
                                    CssClass="btn btn-primary"
                                    style="margin-right: 5px; padding: 5px 10px; font-size: 12px;" />
                                <asp:Button ID="btnDelete" runat="server" 
                                    Text="Delete" 
                                    CommandName="DeleteProject" 
                                    CommandArgument='<%# Container.DataItemIndex %>'
                                    CssClass="btn"
                                    style="background-color: #dc3545; color: white; padding: 5px 10px; font-size: 12px;"
                                    OnClientClick="return confirm('Are you sure you want to delete this project?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>

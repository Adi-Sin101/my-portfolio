<%@ Page Title="Skills Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Skills.aspx.cs" Inherits="adminpanel.Skills" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <div class="row">
            <div class="col-12">
                <h2 class="mb-4">Skills Management</h2>
                
                <!-- Add New Skill Form -->
                <div class="card mb-4">
                    <div class="card-header bg-primary text-white">
                        <h4 class="mb-0">Add New Skill</h4>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label for="txtSkillName">Skill Name:</label>
                                    <asp:TextBox ID="txtSkillName" runat="server" CssClass="form-control" placeholder="e.g., JavaScript" />
                                    <asp:RequiredFieldValidator ID="rfvSkillName" runat="server" ControlToValidate="txtSkillName" 
                                        ErrorMessage="Skill name is required" CssClass="text-danger" ValidationGroup="AddSkill" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label for="txtIconUrl">Icon URL:</label>
                                    <asp:TextBox ID="txtIconUrl" runat="server" CssClass="form-control" 
                                        placeholder="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/javascript/javascript-original.svg" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label for="txtPercentage">Percentage (0-100):</label>
                                    <asp:TextBox ID="txtPercentage" runat="server" CssClass="form-control" placeholder="85" TextMode="Number" />
                                    <asp:RangeValidator ID="rvPercentage" runat="server" ControlToValidate="txtPercentage" 
                                        MinimumValue="0" MaximumValue="100" Type="Integer" 
                                        ErrorMessage="Percentage must be between 0 and 100" CssClass="text-danger" ValidationGroup="AddSkill" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group mb-3">
                                    <label for="txtDisplayOrder">Display Order:</label>
                                    <asp:TextBox ID="txtDisplayOrder" runat="server" CssClass="form-control" placeholder="1" TextMode="Number" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Button ID="btnAddSkill" runat="server" Text="Add Skill" OnClick="btnAddSkill_Click" 
                                CssClass="btn btn-primary" ValidationGroup="AddSkill" />
                            <asp:Button ID="btnUpdateSkill" runat="server" Text="Update Skill" OnClick="btnUpdateSkill_Click" 
                                CssClass="btn btn-success" ValidationGroup="AddSkill" Visible="false" />
                            <asp:Button ID="btnCancelEdit" runat="server" Text="Cancel" OnClick="btnCancelEdit_Click" 
                                CssClass="btn btn-secondary" Visible="false" />
                        </div>
                        <asp:HiddenField ID="hdnSkillId" runat="server" />
                    </div>
                </div>

                <!-- Skills Grid -->
                <div class="card">
                    <div class="card-header bg-info text-white">
                        <h4 class="mb-0">Current Skills</h4>
                    </div>
                    <div class="card-body">
                        <asp:GridView ID="gvSkills" runat="server" CssClass="table table-striped table-hover" 
                            AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="gvSkills_RowCommand"
                            EmptyDataText="No skills found. Add your first skill above!">
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="ID" ItemStyle-Width="50px" />
                                <asp:TemplateField HeaderText="Icon" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <img src="<%# Eval("IconUrl") %>" alt="<%# Eval("Name") %>" style="width: 32px; height: 32px;" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Name" HeaderText="Skill Name" ItemStyle-Width="200px" />
                                <asp:BoundField DataField="Percentage" HeaderText="Percentage" ItemStyle-Width="100px" />
                                <asp:BoundField DataField="DisplayOrder" HeaderText="Order" ItemStyle-Width="80px" />
                                <asp:TemplateField HeaderText="Progress" ItemStyle-Width="150px">
                                    <ItemTemplate>
                                        <div class="progress" style="height: 20px;">
                                            <div class="progress-bar bg-primary" role="progressbar" 
                                                style="width: <%# Eval("Percentage") %>%;" 
                                                aria-valuenow="<%# Eval("Percentage") %>" aria-valuemin="0" aria-valuemax="100">
                                                <%# Eval("Percentage") %>%
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px">
                                    <ItemTemplate>
                                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="EditSkill" 
                                            CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-warning me-1" />
                                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="DeleteSkill" 
                                            CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-danger"
                                            OnClientClick="return confirm('Are you sure you want to delete this skill?');" />
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
        .progress {
            background-color: #e9ecef;
        }
    </style>
</asp:Content>

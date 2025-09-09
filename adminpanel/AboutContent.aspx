<%@ Page Title="About Content Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AboutContent.aspx.cs" Inherits="adminpanel.AboutContent" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <div class="row">
            <div class="col-12">
                <h2 class="mb-4">About Me Content Management</h2>
                
                <!-- Success/Error Messages -->
                <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert" role="alert">
                    <asp:Literal ID="ltlMessage" runat="server" />
                </asp:Panel>

                <!-- Add/Edit About Content Form -->
                <div class="card mb-4">
                    <div class="card-header bg-primary text-white">
                        <h4 class="mb-0">
                            <asp:Label ID="lblFormTitle" runat="server" Text="Add New About Content" />
                        </h4>
                    </div>
                    <div class="card-body">
                        <div class="form-group mb-3">
                            <label for="txtAboutText">About Me Text:</label>
                            <asp:TextBox ID="txtAboutText" runat="server" CssClass="form-control" TextMode="MultiLine" 
                                Rows="6" placeholder="Enter your about me content here..." />
                            <asp:RequiredFieldValidator ID="rfvAboutText" runat="server" ControlToValidate="txtAboutText" 
                                ErrorMessage="About text is required" CssClass="text-danger" ValidationGroup="AboutContentGroup" />
                            <small class="form-text text-muted">This will be displayed in the About Me section of your portfolio.</small>
                        </div>
                        
                        <div class="form-group">
                            <asp:Button ID="btnSave" runat="server" Text="Save Content" OnClick="btnSave_Click" 
                                CssClass="btn btn-success me-2" ValidationGroup="AboutContentGroup" />
                            <asp:Button ID="btnUpdate" runat="server" Text="Update Content" OnClick="btnUpdate_Click" 
                                CssClass="btn btn-warning me-2" ValidationGroup="AboutContentGroup" Visible="false" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" 
                                CssClass="btn btn-secondary me-2" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear Form" OnClick="btnClear_Click" 
                                CssClass="btn btn-outline-secondary" />
                        </div>
                        
                        <asp:HiddenField ID="hdnEditId" runat="server" />
                    </div>
                </div>

                <!-- Current About Content List -->
                <div class="card">
                    <div class="card-header bg-info text-white d-flex justify-content-between align-items-center">
                        <h4 class="mb-0">Current About Content</h4>
                        <asp:Label ID="lblContentCount" runat="server" CssClass="badge bg-light text-dark" />
                    </div>
                    <div class="card-body">
                        <asp:GridView ID="gvAboutContent" runat="server" CssClass="table table-striped table-hover" 
                            AutoGenerateColumns="false" OnRowCommand="gvAboutContent_RowCommand" 
                            OnRowDataBound="gvAboutContent_RowDataBound" DataKeyNames="Id" EmptyDataText="No about content found.">
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="ID" ItemStyle-Width="80px" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" />
                                <asp:TemplateField HeaderText="About Text" ItemStyle-Width="60%">
                                    <ItemTemplate>
                                        <div class="about-text-preview">
                                            <%# GetTruncatedText(Eval("AboutText").ToString(), 150) %>
                                            <%# Eval("AboutText").ToString().Length > 150 ? "<span class='text-muted'>...</span>" : "" %>
                                        </div>
                                        <asp:HiddenField ID="hdnFullText" runat="server" Value='<%# Eval("AboutText") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Character Count" ItemStyle-Width="120px" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <span class="badge bg-secondary"><%# Eval("AboutText").ToString().Length %> chars</span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actions" ItemStyle-Width="200px" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <asp:Button ID="btnView" runat="server" Text="View" CommandName="ViewContent" 
                                            CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-outline-primary me-1" />
                                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="EditContent" 
                                            CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-warning me-1" />
                                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="DeleteContent" 
                                            CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-danger" 
                                            OnClientClick="return confirm('Are you sure you want to delete this about content?');" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="table-dark" />
                            <EmptyDataRowStyle CssClass="text-center text-muted" />
                        </asp:GridView>
                    </div>
                </div>

                <!-- Preview Modal -->
                <div class="modal fade" id="previewModal" tabindex="-1" aria-labelledby="previewModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="previewModalLabel">About Content Preview</h5>
                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>
                            <div class="modal-body">
                                <asp:Literal ID="ltlPreviewContent" runat="server" />
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Custom Styles -->
    <style>
        .me-2 {
            margin-right: 0.5rem;
        }
        
        .about-text-preview {
            word-wrap: break-word;
            white-space: pre-wrap;
            line-height: 1.4;
        }
        
        .card {
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            border: none;
        }
        
        .table th {
            border-top: none;
        }
        
        .badge {
            font-size: 0.75em;
        }
        
        .alert {
            border-radius: 0.25rem;
        }
        
        .form-group label {
            font-weight: 600;
            color: #495057;
        }
        
        textarea.form-control {
            resize: vertical;
            min-height: 120px;
        }
        
        .btn-group-sm > .btn, .btn-sm {
            padding: 0.25rem 0.5rem;
            font-size: 0.875rem;
        }
    </style>

    <!-- JavaScript for Modal -->
    <script type="text/javascript">
        function showPreview(content) {
            document.getElementById('<%= ltlPreviewContent.ClientID %>').innerHTML = content.replace(/\n/g, '<br/>');
            var modal = new bootstrap.Modal(document.getElementById('previewModal'));
            modal.show();
        }
    </script>
</asp:Content>
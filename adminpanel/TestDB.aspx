<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestDB.aspx.cs" Inherits="adminpanel.TestDB" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Database Test</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 20px; }
        .result { background: #f8f9fa; padding: 15px; margin: 10px 0; border-radius: 5px; }
        .error { background: #f8d7da; color: #721c24; }
        .success { background: #d4edda; color: #155724; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Database Connection Test</h1>
        
        <asp:Button ID="btnTestDB" runat="server" Text="Test Database" OnClick="btnTestDB_Click" 
            style="padding: 10px 20px; background: #007bff; color: white; border: none; border-radius: 4px;" />
        
        <div id="results" runat="server"></div>
        
        <hr style="margin: 30px 0;" />
        
        <h2>Quick Add Test Project</h2>
        <p>Title: <asp:TextBox ID="txtTestTitle" runat="server" placeholder="Test Project" /></p>
        <p>Description: <asp:TextBox ID="txtTestDesc" runat="server" placeholder="Test Description" /></p>
        <p><asp:Button ID="btnAddTest" runat="server" Text="Add Test Project" OnClick="btnAddTest_Click" 
               style="padding: 8px 16px; background: #28a745; color: white; border: none; border-radius: 4px;" /></p>
        
        <div id="testResults" runat="server"></div>
    </form>
</body>
</html>
# Login Troubleshooting Guide

## ?? **Diagnosing Login Issues**

### **Step 1: Check Database Connection**
1. Open **SQL Server Management Studio**
2. Connect to `DESKTOP-A9TQCCD\SQLEXPRESS`
3. Check if database `Admin_Panel` exists
4. Verify the `AdminUsers` table exists with correct structure

### **Step 2: Verify AdminUsers Table**
Run this SQL query to check/create the table:

```sql
-- Check if AdminUsers table exists
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AdminUsers' AND xtype='U')
BEGIN
    CREATE TABLE AdminUsers (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Username NVARCHAR(50) NOT NULL UNIQUE,
        Password NVARCHAR(50) NOT NULL,
        Email NVARCHAR(100),
        CreatedDate DATETIME DEFAULT GETDATE(),
        LastLogin DATETIME
    )
    
    -- Insert default admin user
    INSERT INTO AdminUsers (Username, Password, Email) 
    VALUES ('admin', 'admin123', 'admin@example.com')
END
ELSE
BEGIN
    -- Check if default user exists
    IF NOT EXISTS (SELECT * FROM AdminUsers WHERE Username = 'admin')
    BEGIN
        INSERT INTO AdminUsers (Username, Password, Email) 
        VALUES ('admin', 'admin123', 'admin@example.com')
    END
END
```

### **Step 3: Common Issues**

#### **A. Database Connection Fails**
- **Symptoms**: Page loads but button doesn't work, or shows database error
- **Solution**: 
  1. Check if SQL Server is running
  2. Verify connection string in Web.config
  3. Make sure Windows Authentication is enabled

#### **B. Table Doesn't Exist**
- **Symptoms**: "Invalid object name 'AdminUsers'" error
- **Solution**: Run the SQL script above

#### **C. JavaScript Conflicts**
- **Symptoms**: Button click doesn't trigger server-side event
- **Solution**: Check browser console for JavaScript errors

#### **D. Validation Issues**
- **Symptoms**: Form validates but doesn't submit
- **Solution**: Check if all validation groups match

### **Step 4: Testing Credentials**
- **Default Username**: `admin`
- **Default Password**: `admin123`

### **Step 5: Browser Console Check**
1. Open browser Developer Tools (F12)
2. Go to Console tab
3. Try clicking Sign In
4. Look for any JavaScript errors

## ?? **Quick Fix Commands**

### **Create Database and Table (if missing):**
```sql
-- Create database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Admin_Panel')
BEGIN
    CREATE DATABASE Admin_Panel
END

USE Admin_Panel

-- Create AdminUsers table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AdminUsers' AND xtype='U')
CREATE TABLE AdminUsers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETDATE(),
    LastLogin DATETIME
)

-- Insert default user if not exists
IF NOT EXISTS (SELECT * FROM AdminUsers WHERE Username = 'admin')
INSERT INTO AdminUsers (Username, Password, Email) 
VALUES ('admin', 'admin123', 'admin@example.com')
```

### **Test Connection String:**
Replace your connection string with this if needed:
```xml
<add name="AdminPanelDB" 
     connectionString="Data Source=DESKTOP-A9TQCCD\SQLEXPRESS;Initial Catalog=Admin_Panel;Integrated Security=True;Connect Timeout=30;" 
     providerName="System.Data.SqlClient"/>
```
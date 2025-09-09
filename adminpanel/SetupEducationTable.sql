-- Education Table Setup for Admin Panel
-- This script creates the Education table and inserts sample data

USE Admin_Panel;
GO

-- Create Education table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Education' AND xtype='U')
BEGIN
    CREATE TABLE Education (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Degree NVARCHAR(200) NOT NULL,
        Institution NVARCHAR(200) NOT NULL,
        Year NVARCHAR(50),
        Grade NVARCHAR(100)
    );
    
    PRINT 'Education table created successfully.';
END
ELSE
BEGIN
    PRINT 'Education table already exists.';
END

-- Clear existing data (optional - comment out if you want to keep existing data)
-- DELETE FROM Education;

-- Insert sample education data
IF NOT EXISTS (SELECT * FROM Education)
BEGIN
    INSERT INTO Education (Degree, Institution, Year, Grade) VALUES
    ('B.Sc in Computer Science', 'Khulna University of Engineering & Technology', 'Expected: 2027', '3.28 / 4.00'),
    ('HSC', 'Khulna Govt. Girls'' College', '2021', '5.00'),
    ('SSC', 'Govt. Coronation Secondary Girls'' School', '2019', '5.00');
    
    PRINT 'Sample education data inserted successfully.';
END
ELSE
BEGIN
    PRINT 'Education table already contains data.';
END

-- Display the data
SELECT * FROM Education ORDER BY Id;

PRINT 'Education table setup completed!';
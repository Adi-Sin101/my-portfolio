-- Setup script for AboutContent table
-- This creates the AboutContent table if it doesn't exist and optionally adds timestamp columns

USE [Admin_Panel]
GO

-- Create AboutContent table if it doesn't exist (with basic structure)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AboutContent]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AboutContent](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [AboutText] [nvarchar](max) NOT NULL,
        CONSTRAINT [PK_AboutContent] PRIMARY KEY CLUSTERED ([Id] ASC)
    )
    
    PRINT 'AboutContent table created successfully.'
END
ELSE
BEGIN
    PRINT 'AboutContent table already exists.'
END
GO

-- Optionally add timestamp columns if they don't exist
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AboutContent]') AND type in (N'U'))
BEGIN
    -- Check and add CreatedDate column
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AboutContent') AND name = 'CreatedDate')
    BEGIN
        ALTER TABLE [dbo].[AboutContent] ADD [CreatedDate] [datetime] NOT NULL DEFAULT (getdate())
        PRINT 'CreatedDate column added to AboutContent table.'
    END
    
    -- Check and add ModifiedDate column
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AboutContent') AND name = 'ModifiedDate')
    BEGIN
        ALTER TABLE [dbo].[AboutContent] ADD [ModifiedDate] [datetime] NOT NULL DEFAULT (getdate())
        PRINT 'ModifiedDate column added to AboutContent table.'
    END
END
GO

-- Insert sample data if table is empty
IF NOT EXISTS (SELECT 1 FROM [dbo].[AboutContent])
BEGIN
    INSERT INTO [dbo].[AboutContent] ([AboutText])
    VALUES 
    (
        'Welcome to my portfolio! I am a passionate web developer with expertise in modern technologies and frameworks. I enjoy creating innovative solutions and bringing ideas to life through code. 

With a strong foundation in both front-end and back-end development, I specialize in creating responsive, user-friendly applications that deliver exceptional user experiences. My journey in web development has equipped me with skills in various programming languages, databases, and development methodologies.

I am constantly learning and staying updated with the latest trends in technology to ensure that I can provide cutting-edge solutions to complex problems. When I''m not coding, you can find me exploring new technologies, contributing to open-source projects, or sharing knowledge with the developer community.'
    )
    
    PRINT 'Sample AboutContent data inserted successfully.'
END
ELSE
BEGIN
    PRINT 'AboutContent table already contains data.'
END
GO

-- Display current content
SELECT 
    Id,
    LEFT(AboutText, 100) + '...' AS AboutText_Preview,
    LEN(AboutText) AS Character_Count,
    CASE 
        WHEN EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AboutContent') AND name = 'CreatedDate')
        THEN CreatedDate 
        ELSE NULL 
    END AS CreatedDate,
    CASE 
        WHEN EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AboutContent') AND name = 'ModifiedDate')
        THEN ModifiedDate 
        ELSE NULL 
    END AS ModifiedDate
FROM [dbo].[AboutContent]
ORDER BY Id DESC

PRINT 'AboutContent setup completed.'
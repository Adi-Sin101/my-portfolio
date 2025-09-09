-- Quick fix for existing AboutContent table
-- Run this if you're getting column errors with the AboutContent table

USE [Admin_Panel]
GO

-- Check if AboutContent table exists and show its structure
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AboutContent]') AND type in (N'U'))
BEGIN
    PRINT 'Current AboutContent table structure:'
    
    SELECT 
        COLUMN_NAME,
        DATA_TYPE,
        IS_NULLABLE,
        COLUMN_DEFAULT
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'AboutContent'
    ORDER BY ORDINAL_POSITION
    
    -- Add missing columns if they don't exist
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AboutContent') AND name = 'CreatedDate')
    BEGIN
        ALTER TABLE [dbo].[AboutContent] ADD [CreatedDate] [datetime] NULL
        UPDATE [dbo].[AboutContent] SET [CreatedDate] = GETDATE() WHERE [CreatedDate] IS NULL
        ALTER TABLE [dbo].[AboutContent] ALTER COLUMN [CreatedDate] [datetime] NOT NULL
        ALTER TABLE [dbo].[AboutContent] ADD CONSTRAINT [DF_AboutContent_CreatedDate] DEFAULT (getdate()) FOR [CreatedDate]
        PRINT 'Added CreatedDate column with default value'
    END
    ELSE
    BEGIN
        PRINT 'CreatedDate column already exists'
    END
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AboutContent') AND name = 'ModifiedDate')
    BEGIN
        ALTER TABLE [dbo].[AboutContent] ADD [ModifiedDate] [datetime] NULL
        UPDATE [dbo].[AboutContent] SET [ModifiedDate] = GETDATE() WHERE [ModifiedDate] IS NULL
        ALTER TABLE [dbo].[AboutContent] ALTER COLUMN [ModifiedDate] [datetime] NOT NULL
        ALTER TABLE [dbo].[AboutContent] ADD CONSTRAINT [DF_AboutContent_ModifiedDate] DEFAULT (getdate()) FOR [ModifiedDate]
        PRINT 'Added ModifiedDate column with default value'
    END
    ELSE
    BEGIN
        PRINT 'ModifiedDate column already exists'
    END
    
    PRINT 'AboutContent table structure updated successfully!'
    
    -- Show updated structure
    PRINT 'Updated AboutContent table structure:'
    SELECT 
        COLUMN_NAME,
        DATA_TYPE,
        IS_NULLABLE,
        COLUMN_DEFAULT
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'AboutContent'
    ORDER BY ORDINAL_POSITION
    
END
ELSE
BEGIN
    PRINT 'AboutContent table does not exist. Creating new table...'
    
    CREATE TABLE [dbo].[AboutContent](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [AboutText] [nvarchar](max) NOT NULL,
        [CreatedDate] [datetime] NOT NULL DEFAULT (getdate()),
        [ModifiedDate] [datetime] NOT NULL DEFAULT (getdate()),
        CONSTRAINT [PK_AboutContent] PRIMARY KEY CLUSTERED ([Id] ASC)
    )
    
    -- Insert sample content
    INSERT INTO [dbo].[AboutContent] ([AboutText])
    VALUES ('Welcome to my portfolio! I am a passionate web developer with expertise in modern technologies and frameworks. I enjoy creating innovative solutions and bringing ideas to life through code.')
    
    PRINT 'AboutContent table created successfully with sample data!'
END

-- Display current data
SELECT 
    Id,
    LEFT(AboutText, 50) + '...' AS AboutText_Preview,
    LEN(AboutText) AS Character_Count,
    CreatedDate,
    ModifiedDate
FROM [dbo].[AboutContent]
ORDER BY Id DESC

PRINT 'Fix completed successfully!'
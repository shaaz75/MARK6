
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/18/2023 09:54:38
-- Generated from EDMX file: C:\Users\hp\source\repos\MARK6\MARK6\Models\DBModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MARK6DB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[MediaGallery]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MediaGallery];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'MediaGalleries'
CREATE TABLE [dbo].[MediaGalleries] (
    [MediaId] int IDENTITY(1,1) NOT NULL,
    [Title] varchar(250)  NULL,
    [MediaPath] varchar(max)  NULL,
    [MediaType] varchar(20)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [MediaId] in table 'MediaGalleries'
ALTER TABLE [dbo].[MediaGalleries]
ADD CONSTRAINT [PK_MediaGalleries]
    PRIMARY KEY CLUSTERED ([MediaId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
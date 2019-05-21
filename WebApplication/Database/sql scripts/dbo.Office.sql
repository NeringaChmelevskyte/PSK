USE [AppDB]
GO

/****** Object: Table [dbo].[Office] Script Date: 5/21/2019 8:24:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Office] (
    [Id]      INT           IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (50) NOT NULL,
    [Address] NVARCHAR (80) NOT NULL,
    [City]    NVARCHAR (50) NOT NULL
);



USE [AppDB]
GO

/****** Object: Table [dbo].[Events] Script Date: 5/21/2019 8:25:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Events] (
    [EventId]     INT            IDENTITY (1, 1) NOT NULL,
    [Subject]     NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Start]       DATETIME       NOT NULL,
    [End]         DATETIME       NOT NULL,
    [ThemeColor]  NVARCHAR (50)  NOT NULL,
    [IsFullDay]   BIT            NOT NULL,
    [UserId]      INT            NOT NULL
);



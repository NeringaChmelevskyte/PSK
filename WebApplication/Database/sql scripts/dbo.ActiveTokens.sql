USE [AppDB]
GO

/****** Object: Table [dbo].[Users] Script Date: 5/21/2019 8:23:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ActiveTokens] (
    [Id]       INT         IDENTITY (1, 1) NOT NULL,
    [Token]     NVARCHAR(200)  NOT NULL,
    [UserId]   INT	NOT NULL
);



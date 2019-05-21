USE [AppDB]
GO

/****** Object: Table [dbo].[Users] Script Date: 5/21/2019 8:23:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users] (
    [Id]       INT         IDENTITY (1, 1) NOT NULL,
    [Name]     NCHAR (20)  NOT NULL,
    [Surname]  NCHAR (25)  NOT NULL,
    [Password] NCHAR (200) NOT NULL,
    [Email]    NCHAR (30)  NOT NULL,
    [Role]     INT         NOT NULL
);



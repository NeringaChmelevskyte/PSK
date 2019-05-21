USE [AppDB]
GO

/****** Object: Table [dbo].[Calendar] Script Date: 5/21/2019 8:25:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Calendar] (
    [Id]          INT      IDENTITY (1, 1) NOT NULL,
    [StartDate]   DATETIME NOT NULL,
    [EndDate]     DATETIME NOT NULL,
    [BookingType] INT      NOT NULL,
    [BookerId]    INT      NOT NULL
);



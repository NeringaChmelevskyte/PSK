USE [AppDB]
GO

/****** Object: Table [dbo].[Trip] Script Date: 5/21/2019 8:24:21 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Trip] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [Title]      NVARCHAR (90) NOT NULL,
    [Start]      DATETIME      NOT NULL,
    [End]        DATETIME      NOT NULL,
    [FromOffice] INT           NOT NULL,
    [ToOffice]   INT           NOT NULL,
    [TripStatus] INT           NOT NULL
);



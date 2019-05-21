USE [AppDB]
GO

/****** Object: Table [dbo].[Apartment] Script Date: 5/21/2019 8:25:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Apartment] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Title]     NVARCHAR (50) NOT NULL,
    [OfficeId]  INT           NOT NULL,
    [RoomCount] INT           NOT NULL
);



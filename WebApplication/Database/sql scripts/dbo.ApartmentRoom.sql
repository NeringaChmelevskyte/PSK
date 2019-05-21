USE [AppDB]
GO

/****** Object: Table [dbo].[ApartmentRoom] Script Date: 5/21/2019 8:25:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ApartmentRoom] (
    [Id]          INT IDENTITY (1, 1) NOT NULL,
    [RoomNumber]  INT NOT NULL,
    [ApartmentId] INT NOT NULL
);



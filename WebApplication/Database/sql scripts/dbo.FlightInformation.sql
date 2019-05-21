USE [AppDB]
GO

/****** Object: Table [dbo].[FlightInformation] Script Date: 5/21/2019 8:25:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FlightInformation] (
    [Id]                 INT      IDENTITY (1, 1) NOT NULL,
    [TripId]             INT      NOT NULL,
    [Cost]               REAL     NOT NULL,
    [Start]              DATETIME NOT NULL,
    [End]                DATETIME NOT NULL,
    [FlightTicketStatus] INT      NOT NULL
);



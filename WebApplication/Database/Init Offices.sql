/****** Script for SelectTopNRows command from SSMS  ******/
USE [AppDB]
GO

INSERT INTO [dbo].[Office]
           ([Name]
           ,[City]
           ,[Address])
     VALUES
           (N'Vilniaus ofisas'
           ,N'Vilnius'
           ,N'Vilnius'),
           (N'Kauno ofisas'
           ,N'Kaunas'
           ,N'Kaunas'),
           (N'Klaipėdos ofisas'
           ,N'Klaipėda'
           ,N'Klaipėda'),
           (N'Panevėžio ofisas'
           ,N'Panevėžys'
           ,N'Panevėžys')
GO
/****** Script for SelectTopNRows command from SSMS  ******/
USE [AppDB]
GO
INSERT INTO [dbo].[Users]
           ([Name]
           ,[Surname]
           ,[Password]
           ,[Email]
           ,[Role])
     VALUES
           (N'Tomas'
           ,N'Ukrinas'
           ,'wurW1F3d9NdndayJy4GKsq7HhPhaeZyI8zExmgGprAFJd24L'
           ,'tomas.ukrinas1997@gmail.com'
           ,0)
		   ,(N'Dar'
           ,N'Vienas'
           ,'2FSpoGjpRln9Red9yXrmsgsz6ezJeqqgGKJN2+TmxOighkfs'
           ,'user@gmail.com'
           ,1)
		   ,(N'Test'
           ,N'User'
           ,'2FSpoGjpRln9Red9yXrmsgsz6ezJeqqgGKJN2+TmxOighkfs'
           ,'TestUser@email.com'
           ,2)
GO
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
           (N'admin'
           ,N'admin'
           ,'wurW1F3d9NdndayJy4GKsq7HhPhaeZyI8zExmgGprAFJd24L'
           ,'admin@mail.com'
           ,0)
		   ,(N'emp'
           ,N'emp'
           ,'2FSpoGjpRln9Red9yXrmsgsz6ezJeqqgGKJN2+TmxOighkfs'
           ,'emp@mail.com'
           ,1)
		   ,(N'org'
           ,N'org'
           ,'2FSpoGjpRln9Red9yXrmsgsz6ezJeqqgGKJN2+TmxOighkfs'
           ,'orgz@mail.com'
           ,2)
GO
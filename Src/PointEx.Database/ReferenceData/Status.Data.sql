SET IDENTITY_INSERT [dbo].[Status] ON 

INSERT INTO [dbo].[Status] ([Id], [Name])
SELECT  1, N'Pendiente' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Status] WHERE Id = 1)

INSERT INTO [dbo].[Status] ([Id], [Name])
SELECT  2, N'Aprobado' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Status] WHERE Id = 2)

INSERT INTO [dbo].[Status] ([Id], [Name])
SELECT  3, N'Rechazado' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Status] WHERE Id = 3)

SET IDENTITY_INSERT [dbo].[Status] OFF
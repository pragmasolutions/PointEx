SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT INTO [dbo].[Category] ([Id], [Name])
SELECT  1, N'Indumentaria' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 1)

INSERT INTO [dbo].[Category] ([Id], [Name])
SELECT  2, N'Librería/juguetes' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 2)

INSERT INTO [dbo].[Category] ([Id], [Name])
SELECT  3, N'Regalos' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 3)

INSERT INTO [dbo].[Category] ([Id], [Name])
SELECT  4, N'Servicios' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 4)

INSERT INTO [dbo].[Category] ([Id], [Name])
SELECT  5, N'Supermercados' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 5)

INSERT INTO [dbo].[Category] ([Id], [Name])
SELECT  6, N'Tiempo Libre' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 6)

INSERT INTO [dbo].[Category] ([Id], [Name])
SELECT  7, N'Viajes y Turismo' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 7)

INSERT INTO [dbo].[Category] ([Id], [Name])
SELECT  8, N'Belleza Y Salud' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 8)

INSERT INTO [dbo].[Category] ([Id], [Name])
SELECT  9, N'Electrónica' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 9)

INSERT INTO [dbo].[Category] ([Id], [Name])
SELECT  10, N'Entretenimientos' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 10)

INSERT INTO [dbo].[Category] ([Id], [Name])
SELECT  11, N'Fotografía/óptica' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 11)

INSERT INTO [dbo].[Category] ([Id], [Name])
SELECT  12, N'Gastronomía' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 12)

INSERT INTO [dbo].[Category] ([Id], [Name])
SELECT  13, N'Joyerías' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 13)

INSERT INTO [dbo].[Category] ([Id], [Name])
SELECT  14, N'Todo Para El Hogar' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 14)

INSERT INTO [dbo].[Category] ([Id], [Name])
SELECT  15, N'Todo Para Tu Auto' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 15)

INSERT INTO [dbo].[Category] ([Id], [Name])
SELECT  16, N'Combustible' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 16)

INSERT INTO [dbo].[Category] ([Id], [Name])
SELECT  17, N'Construcción' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 17)

INSERT INTO [dbo].[Category] ([Id], [Name])
SELECT  18, N'Alquiler De Autos' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 18)

INSERT INTO [dbo].[Category] ([Id], [Name])
SELECT  19, N'Pinturerías' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 19)

INSERT INTO [dbo].[Category] ([Id], [Name])
SELECT  20, N'Otros' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 20)

SET IDENTITY_INSERT [dbo].[Category] OFF
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT INTO [dbo].[Category] ([Id], [Name], [IconClass])
SELECT  1, N'Indumentaria', 'fa fa-female' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 1)

INSERT INTO [dbo].[Category] ([Id], [Name], [IconClass])
SELECT  2, N'Fotocopiadora / Impresiones', 'fa fa-print' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 2)

INSERT INTO [dbo].[Category] ([Id], [Name], [IconClass])
SELECT  3, N'Regalos', 'fa fa-gift' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 3)

INSERT INTO [dbo].[Category] ([Id], [Name], [IconClass])
SELECT  4, N'Servicios', 'fa fa-star' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 4)

INSERT INTO [dbo].[Category] ([Id], [Name], [IconClass])
SELECT  5, N'Supermercados', 'fa fa-shopping-cart' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 5)

INSERT INTO [dbo].[Category] ([Id], [Name], [IconClass])
SELECT  6, N'Tiempo Libre', 'fa fa-child' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 6)

INSERT INTO [dbo].[Category] ([Id], [Name], [IconClass])
SELECT  7, N'Viajes y Turismo', 'fa fa-plane' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 7)

INSERT INTO [dbo].[Category] ([Id], [Name], [IconClass])
SELECT  8, N'Belleza Y Salud', 'fa fa-medkit' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 8)

INSERT INTO [dbo].[Category] ([Id], [Name], [IconClass])
SELECT  9, N'Electrónica', 'fa fa-laptop' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 9)

INSERT INTO [dbo].[Category] ([Id], [Name], [IconClass])
SELECT  10, N'Entretenimientos', 'fa fa-video-camera' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 10)

INSERT INTO [dbo].[Category] ([Id], [Name], [IconClass])
SELECT  11, N'Fotografía/óptica', 'fa fa-camera' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 11)

INSERT INTO [dbo].[Category] ([Id], [Name], [IconClass])
SELECT  12, N'Gastronomía', 'fa fa-cutlery' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 12)

INSERT INTO [dbo].[Category] ([Id], [Name], [IconClass])
SELECT  13, N'Joyerías', 'fa fa-diamond' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 13)

INSERT INTO [dbo].[Category] ([Id], [Name], [IconClass])
SELECT  14, N'Todo Para El Hogar', 'fa fa-home' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 14)

INSERT INTO [dbo].[Category] ([Id], [Name], [IconClass])
SELECT  15, N'Todo Para Tu Auto', 'fa fa-car' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 15)

INSERT INTO [dbo].[Category] ([Id], [Name], [IconClass])
SELECT  16, N'Combustible', 'fa fa-automobile' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 16)

INSERT INTO [dbo].[Category] ([Id], [Name], [IconClass])
SELECT  17, N'Lavandería', 'fa fa-tachometer' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 17)

INSERT INTO [dbo].[Category] ([Id], [Name], [IconClass])
SELECT  18, N'Espectáculos', 'fa fa-thumb-tack' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 18)

INSERT INTO [dbo].[Category] ([Id], [Name], [IconClass])
SELECT  19, N'Cine', 'fa fa-film'  WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 19)

INSERT INTO [dbo].[Category] ([Id], [Name], [IconClass])
SELECT  20, N'Otros', 'fa fa-question-circle' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 20)

INSERT INTO [dbo].[Category] ([Id], [Name], [IconClass])
SELECT  21, N'Recitales', 'fa fa-music' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 21)

INSERT INTO [dbo].[Category] ([Id], [Name], [IconClass])
SELECT  22, N'Boliches', 'fa fa-moon-o' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 22)

INSERT INTO [dbo].[Category] ([Id], [Name], [IconClass])
SELECT  23, N'Festivales', 'fa fa-microphone' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 23)

INSERT INTO [dbo].[Category] ([Id], [Name], [IconClass])
SELECT  24, N'Entradas a la cancha', 'fa fa-futbol-o' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 24)

INSERT INTO [dbo].[Category] ([Id], [Name], [IconClass])
SELECT  25, N'Farmacia', 'fa fa-plus-square' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Category] WHERE Id = 25)


SET IDENTITY_INSERT [dbo].[Category] OFF


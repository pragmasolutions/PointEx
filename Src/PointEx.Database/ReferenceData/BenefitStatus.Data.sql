SET IDENTITY_INSERT [dbo].[BenefitStatus] ON 

INSERT INTO [dbo].[BenefitStatus] ([Id], [Name])
SELECT  1, N'Pendiente' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[BenefitStatus] WHERE Id = 1)

INSERT INTO [dbo].[BenefitStatus] ([Id], [Name])
SELECT  2, N'Aprobado' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[BenefitStatus] WHERE Id = 2)

INSERT INTO [dbo].[BenefitStatus] ([Id], [Name])
SELECT  3, N'Rechazado' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[BenefitStatus] WHERE Id = 3)

SET IDENTITY_INSERT [dbo].[BenefitStatus] OFF
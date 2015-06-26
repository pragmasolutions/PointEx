SET IDENTITY_INSERT [dbo].[BenefitTypes] ON 

INSERT INTO [dbo].[BenefitTypes] ([Id], [Name])
SELECT  1, N'Descuento' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[BenefitTypes] WHERE Id = 1)

INSERT INTO [dbo].[BenefitTypes] ([Id], [Name])
SELECT  2, N'2x1' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[BenefitTypes] WHERE Id = 2)

INSERT INTO [dbo].[BenefitTypes] ([Id], [Name])
SELECT  3, N'3x2' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[BenefitTypes] WHERE Id = 3)

SET IDENTITY_INSERT [dbo].[BenefitTypes] OFF
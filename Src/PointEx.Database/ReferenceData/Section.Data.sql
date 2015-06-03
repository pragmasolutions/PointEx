SET IDENTITY_INSERT [dbo].[Section] ON 

INSERT INTO [dbo].[Section] ([Id], [Name], MaxNumberOfItems)
SELECT  1, N'SliderHome' , 3 WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Section] WHERE Id = 1)

SET IDENTITY_INSERT [dbo].[Section] OFF
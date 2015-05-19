CREATE TABLE [dbo].[ShopCategory] (
	[Id]     INT NOT NULL IDENTITY,
    [ShopId]     INT NOT NULL,
    [CategoryId] INT NOT NULL,
    CONSTRAINT [FK_ShopCategory_Category] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category] ([Id]),
    CONSTRAINT [FK_ShopCategory_Shop] FOREIGN KEY ([ShopId]) REFERENCES [dbo].[Shop] ([Id]), 
    CONSTRAINT [PK_ShopCategory] PRIMARY KEY ([Id]) 
);


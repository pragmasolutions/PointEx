CREATE TABLE [dbo].[Benefit] (
    [Id]           INT           NOT NULL,
    [Description]  VARCHAR (250) NOT NULL,
    [ShopId]       INT           NOT NULL,
    [CreatedDate]  DATETIME2 (7) NOT NULL,
    [ModifiedDate] DATETIME2 (7) NULL,
    CONSTRAINT [FK_Benefit_Shop] FOREIGN KEY ([ShopId]) REFERENCES [dbo].[Shop] ([Id])
);




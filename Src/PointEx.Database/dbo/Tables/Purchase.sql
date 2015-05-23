CREATE TABLE [dbo].[Purchase] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [ShopId]       INT           NOT NULL,
    [CardId]       INT           NOT NULL,
	[BenefitId]    INT           NOT NULL,
    [PurchaseDate] DATETIME2 (7) NOT NULL,
	[Amount]	   MONEY		 NOT NULL,
    CONSTRAINT [PK_Purchase] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Purchase_Card] FOREIGN KEY ([CardId]) REFERENCES [dbo].[Card] ([Id]),
    CONSTRAINT [FK_Purchase_Shop] FOREIGN KEY ([ShopId]) REFERENCES [dbo].[Shop] ([Id]),
	CONSTRAINT [FK_Purchase_Benefit] FOREIGN KEY ([BenefitId]) REFERENCES [dbo].[Benefit] ([Id])
);




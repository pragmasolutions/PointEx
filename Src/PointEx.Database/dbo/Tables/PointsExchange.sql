CREATE TABLE [dbo].[PointsExchange] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [PrizeId]      INT           NOT NULL,
    [BeneficiaryId]    INT           NOT NULL,
    [ExchangeDate] DATETIME2 (7) NOT NULL,
	[PointsUsed]		INT		NOT NULL,
	CONSTRAINT [PK_PointsExchange] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PointsExchange_Prize] FOREIGN KEY ([PrizeId]) REFERENCES [dbo].[Prize] ([Id]),
    CONSTRAINT [FK_PointsExchange_Beneficiary] FOREIGN KEY ([BeneficiaryId]) REFERENCES [dbo].[Beneficiary] ([Id])
);




CREATE TABLE [dbo].[PointsExchange] (
    [Id]           INT           NOT NULL,
    [PrizeId]      INT           NOT NULL,
    [BeneficiaryId]    INT           NOT NULL,
    [ExchangeDate] DATETIME2 (7) NOT NULL,
    CONSTRAINT [FK_PointsExchange_Prize] FOREIGN KEY ([PrizeId]) REFERENCES [dbo].[Prize] ([Id]),
    CONSTRAINT [FK_PointsExchange_Beneficiary] FOREIGN KEY ([BeneficiaryId]) REFERENCES [dbo].[Beneficiary] ([Id])
);




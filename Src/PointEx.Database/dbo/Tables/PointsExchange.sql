CREATE TABLE [dbo].[PointsExchange] (
    [Id]           INT           NOT NULL,
    [PrizeId]      INT           NOT NULL,
    [StudentId]    INT           NOT NULL,
    [ExchangeDate] DATETIME2 (7) NOT NULL,
    CONSTRAINT [FK_PointsExchange_Reward] FOREIGN KEY ([PrizeId]) REFERENCES [dbo].[Reward] ([Id]),
    CONSTRAINT [FK_PointsExchange_Student] FOREIGN KEY ([StudentId]) REFERENCES [dbo].[Student] ([Id])
);




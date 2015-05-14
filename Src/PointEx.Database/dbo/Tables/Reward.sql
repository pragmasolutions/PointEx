CREATE TABLE [dbo].[Reward] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [RewardCatalogId] INT           NOT NULL,
    [Name]            VARCHAR (250) NOT NULL,
    [CreatedDate]     DATETIME2 (7) NOT NULL,
    [ModifiedDate]    DATETIME2 (7) NULL,
    CONSTRAINT [PK_Reward] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Reward_RewardCatalog] FOREIGN KEY ([RewardCatalogId]) REFERENCES [dbo].[RewardCatalog] ([Id])
);




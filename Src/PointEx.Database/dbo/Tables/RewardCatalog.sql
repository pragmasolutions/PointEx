CREATE TABLE [dbo].[RewardCatalog] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [CreatedDate]  DATETIME2 (7) NOT NULL,
    [ModifiedDate] DATETIME2 (7) NULL,
    CONSTRAINT [PK_RewardCatalog] PRIMARY KEY CLUSTERED ([Id] ASC)
);




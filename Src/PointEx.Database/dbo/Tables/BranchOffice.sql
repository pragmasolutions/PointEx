CREATE TABLE [dbo].[BranchOffice] (
    [Id]           INT               IDENTITY (1, 1) NOT NULL,
	[ShopId]       INT               NOT NULL,
    [Name]         VARCHAR (250)     NOT NULL,
	[TownId]       INT               NOT NULL,
    [Address]      VARCHAR (250)     NOT NULL,
	[Phone]        VARCHAR (250)     NOT NULL,
    [Location]     [sys].[geography] NULL,
    [CreatedDate]  DATETIME2 (7)     NOT NULL,
    [ModifiedDate] DATETIME2 (7)     NULL,
    CONSTRAINT [PK_BranchOffice] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_BranchOffice_Shop] FOREIGN KEY ([ShopId]) REFERENCES [dbo].[Shop] ([Id]),
    CONSTRAINT [FK_BranchOffice_Town] FOREIGN KEY ([TownId]) REFERENCES [dbo].[Town] ([Id])
);




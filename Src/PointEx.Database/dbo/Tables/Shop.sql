CREATE TABLE [dbo].[Shop] (
    [Id]           INT               IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (250)     NOT NULL,
    [Address]      VARCHAR (250)     NOT NULL,
    [TownId]       INT               NOT NULL,
    [UserId]       NVARCHAR (128)    NOT NULL,
    [Location]     [sys].[geography] NULL,
	[Phone]        VARCHAR (250)     NULL,
    [CreatedDate]  DATETIME2 (7)     NOT NULL,
    [ModifiedDate] DATETIME2 (7)     NULL,
    CONSTRAINT [PK_Shop] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Shop_Town] FOREIGN KEY ([TownId]) REFERENCES [dbo].[Town] ([Id]), 
    CONSTRAINT [FK_Shop_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers]([Id])
);




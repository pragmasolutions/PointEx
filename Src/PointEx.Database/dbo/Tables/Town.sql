CREATE TABLE [dbo].[Town] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (250) NOT NULL,
    CONSTRAINT [PK_Town] PRIMARY KEY CLUSTERED ([Id] ASC)
);


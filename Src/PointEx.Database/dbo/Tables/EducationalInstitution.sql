CREATE TABLE [dbo].[EducationalInstitution] (
    [Id]           INT               IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (250)     NOT NULL,
    [TownId]       INT               NOT NULL,
	[Address]				   VARCHAR (250)     NULL,
    [Location]     [sys].[geography] NULL,
    [CreatedDate]  DATETIME2 (7)     NOT NULL,
    [ModifiedDate] DATETIME2 (7)     NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_EducationalInstitution] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EducationalInstitution_Town] FOREIGN KEY ([TownId]) REFERENCES [dbo].[Town] ([Id])
);




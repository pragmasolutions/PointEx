CREATE TABLE [dbo].[Beneficiary] (
    [Id]                       INT            IDENTITY (1, 1) NOT NULL,
    [EducationalInstitutionId] INT            NOT NULL,
    [TownId]                   INT            NOT NULL,
    [UserId]                   NVARCHAR (128) NOT NULL,
    [CreatedDate]              DATETIME2 (7)  NOT NULL,
    [ModifiedDate]             DATETIME2 (7)  NULL,
    CONSTRAINT [PK_Beneficiary] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Beneficiary_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_Beneficiary_EducationalInstitution] FOREIGN KEY ([EducationalInstitutionId]) REFERENCES [dbo].[EducationalInstitution] ([Id]),
    CONSTRAINT [FK_Beneficiary_Town] FOREIGN KEY ([TownId]) REFERENCES [dbo].[Town] ([Id])
);




CREATE TABLE [dbo].[BenefitFile] (
	[Id]     INT NOT NULL IDENTITY,
    [BenefitId]     INT NOT NULL,
    [FileId] INT NOT NULL,
	[Order]     INT NOT NULL,
    CONSTRAINT [FK_BenefitFile_File] FOREIGN KEY ([FileId]) REFERENCES [dbo].[File] ([Id]),
    CONSTRAINT [FK_BenefitFile_Benefit] FOREIGN KEY ([BenefitId]) REFERENCES [dbo].[Benefit] ([Id]), 
    CONSTRAINT [PK_BenefitFile] PRIMARY KEY ([Id]) 
);


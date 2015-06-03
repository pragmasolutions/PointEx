CREATE TABLE [dbo].[BenefitBranchOffice]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [BenefitId] INT NOT NULL, 
    [BranchOfficeId] INT NOT NULL, 
    CONSTRAINT [FK_BenefitByBranchOffice_Benefit] FOREIGN KEY (BenefitId) REFERENCES [Benefit]([Id]), 
    CONSTRAINT [FK_BenefitByBranchOffice_BranchOffice] FOREIGN KEY ([BranchOfficeId]) REFERENCES [BranchOffice]([Id])
)

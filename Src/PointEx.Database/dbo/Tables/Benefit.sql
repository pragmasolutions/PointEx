CREATE TABLE [dbo].[Benefit] (
    [Id]           INT           NOT NULL IDENTITY,
	[Name]		   VARCHAR (250) NOT NULL,
    [Description]  VARCHAR (MAX) NOT NULL,
	[DiscountPercentage] DECIMAL(18,2) NULL,
	[DiscountPercentageCeiling] MONEY NULL,
    [ShopId]       INT           NOT NULL,
	[DateFrom]  DATETIME2 (7)    NOT NULL DEFAULT GETDATE(),
	[DateTo]  DATETIME2 (7)		 NULL,
    [CreatedDate]  DATETIME2 (7) NOT NULL,
    [ModifiedDate] DATETIME2 (7) NULL,
	[IsDeleted]	   BIT NOT NULL DEFAULT 0,
    [BenefitTypeId] INT NOT NULL, 
    [BenefitStatusId] INT NOT NULL DEFAULT 2, 
    CONSTRAINT [FK_Benefit_Shop] FOREIGN KEY ([ShopId]) REFERENCES [dbo].[Shop] ([Id]), 
	CONSTRAINT [FK_Benefit_BenefitTypes] FOREIGN KEY ([BenefitTypeId]) REFERENCES [dbo].[BenefitTypes] ([Id]), 
    CONSTRAINT [PK_Benefit] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Benefit_BenefitStatus] FOREIGN KEY ([BenefitStatusId]) REFERENCES [BenefitStatus]([Id])
);




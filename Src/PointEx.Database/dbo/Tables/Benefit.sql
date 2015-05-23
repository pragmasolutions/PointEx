﻿CREATE TABLE [dbo].[Benefit] (
    [Id]           INT           NOT NULL IDENTITY,
	[Name]		   VARCHAR (250) NOT NULL,
    [Description]  VARCHAR (MAX) NOT NULL,
	[DiscountPercentage] DECIMAL(18,2) NULL,
	[DiscountPercentageCeiling] MONEY NULL,
    [ShopId]       INT           NOT NULL,
    [CreatedDate]  DATETIME2 (7) NOT NULL,
    [ModifiedDate] DATETIME2 (7) NULL,
    CONSTRAINT [FK_Benefit_Shop] FOREIGN KEY ([ShopId]) REFERENCES [dbo].[Shop] ([Id]), 
    CONSTRAINT [PK_Benefit] PRIMARY KEY ([Id])
);




CREATE TABLE [dbo].[Section] (
    [Id]           INT           NOT NULL IDENTITY,
	[Name]		   VARCHAR (250) NOT NULL,
    [MaxNumberOfItems] INT NULL, 
    CONSTRAINT [PK_Section] PRIMARY KEY ([Id])
);




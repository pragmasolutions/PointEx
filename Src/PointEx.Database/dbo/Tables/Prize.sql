CREATE TABLE [dbo].[Prize] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [Name]            VARCHAR (250) NOT NULL,
	[Description]     NVARCHAR (MAX) NULL,
	[PointsNeeded]    INT NOT NULL,
	[ImageFileId]     INT NULL,
    [CreatedDate]     DATETIME2 (7) NOT NULL,
    [ModifiedDate]    DATETIME2 (7) NULL,
    CONSTRAINT [PK_Prize] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Prize_File] FOREIGN KEY ([ImageFileId]) REFERENCES [dbo].[File] ([Id])
);




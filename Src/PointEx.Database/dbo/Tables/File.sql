CREATE TABLE [dbo].[File] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [Name]            VARCHAR (250) NOT NULL,
	[ContentType]     VARCHAR (250) NOT NULL,
	[Content]	      VARBINARY (MAX) NOT NULL,
    [CreatedDate]     DATETIME2 (7) NOT NULL,
    [ModifiedDate]    DATETIME2 (7) NULL,
    CONSTRAINT [PK_File] PRIMARY KEY CLUSTERED ([Id] ASC)
);




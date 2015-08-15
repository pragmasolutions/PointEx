CREATE TABLE [dbo].[SliderImage]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,
	[Name] VARCHAR(100) NOT NULL, 
    [FileId] INT NOT NULL,
	[CreatedDate]     DATETIME2 (7) NOT NULL,
    [ModifiedDate]    DATETIME2 (7) NULL,
    CONSTRAINT [PK_SliderImage] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_SliderImage_File] FOREIGN KEY ([FileId]) REFERENCES [File]([Id]),
)

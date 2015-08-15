CREATE TABLE [dbo].[SectionItem] (
    [Id]           INT           NOT NULL IDENTITY,
	[SectionId]    INT    NOT NULL,
	[BenefitId]    INT    NULL,
	[PrizeId]      INT    NULL,
	[SliderImageId]		   INT NULL,
	[Order]		   INT NOT NULL,
    CONSTRAINT [PK_SectionItem] PRIMARY KEY ([Id]), 
	CONSTRAINT [FK_SectionItem_Section] FOREIGN KEY ([SectionId]) REFERENCES [Section]([Id]),
    CONSTRAINT [FK_SectionItem_Benefit] FOREIGN KEY ([BenefitId]) REFERENCES [Benefit]([Id]),
	CONSTRAINT [FK_SectionItem_Prize] FOREIGN KEY ([PrizeId]) REFERENCES [Prize]([Id]), 
    CONSTRAINT [FK_SectionItem_SliderImage] FOREIGN KEY ([SliderImageId]) REFERENCES [SliderImage]([Id])
);




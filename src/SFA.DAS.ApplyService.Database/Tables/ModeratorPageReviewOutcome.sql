CREATE TABLE [dbo].[ModeratorPageReviewOutcome](
	[Id] [uniqueidentifier] NOT NULL,
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[SequenceNumber] [int] NOT NULL,
	[SectionNumber] [int] NOT NULL,
	[PageId] [nvarchar](50) NOT NULL,
	[ModeratorUserId] NVARCHAR(256) NULL,     
	[ModeratorReviewStatus] NVARCHAR(20) NULL, 
    [ModeratorReviewComment] NVARCHAR(MAX) NULL,
	[CreatedAt] DATETIME2 NOT NULL, 
    [CreatedBy] NVARCHAR(256) NOT NULL, 
    [UpdatedAt] DATETIME2 NULL, 
    [UpdatedBy] NVARCHAR(256) NULL, 
 CONSTRAINT [PK_ModeratorPageReviewOutcome] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY], 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].ModeratorPageReviewOutcome ADD  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].ModeratorPageReviewOutcome ADD DEFAULT (getutcdate()) FOR [CreatedAt]
GO

CREATE INDEX [IX_ModeratorPageReviewOutcome_ApplicationId] ON [ModeratorPageReviewOutcome] ([ApplicationId])
GO
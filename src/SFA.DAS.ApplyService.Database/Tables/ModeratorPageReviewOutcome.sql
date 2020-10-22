CREATE TABLE [dbo].[ModeratorPageReviewOutcome](
	[Id] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[SequenceNumber] [int] NOT NULL,
	[SectionNumber] [int] NOT NULL,
	[PageId] [nvarchar](50) NOT NULL,
	[ModeratorUserId] NVARCHAR(256) NULL,     
	[ModeratorReviewStatus] NVARCHAR(20) NULL, 
    [ModeratorReviewComment] NVARCHAR(MAX) NULL,
	[ClarificationUserId] NVARCHAR(256) NULL,
	[ClarificationStatus] NVARCHAR(20) NULL, 
    [ClarificationComment] NVARCHAR(MAX) NULL,
	[ClarificationResponse] NVARCHAR(MAX) NULL,
	[CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] NVARCHAR(256) NOT NULL, 
    [UpdatedAt] DATETIME2 NULL, 
    [UpdatedBy] NVARCHAR(256) NULL, 
 CONSTRAINT [PK_ModeratorPageReviewOutcome] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY], 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE UNIQUE INDEX [UX_ModeratorPageReviewOutcome_ApplicationId] ON [ModeratorPageReviewOutcome] ([ApplicationId], [PageId]) INCLUDE ([SequenceNumber], [SectionNumber])
GO
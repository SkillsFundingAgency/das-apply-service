CREATE TABLE [dbo].[AssessorPageReviewOutcome](
	[Id] [uniqueidentifier] NOT NULL DEFAULT NEWSEQUENTIALID(),
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[SequenceNumber] [int] NOT NULL,
	[SectionNumber] [int] NOT NULL,
	[PageId] [nvarchar](50) NOT NULL,
	[Assessor1UserId] NVARCHAR(256) NULL,     
	[Assessor1ReviewStatus] NVARCHAR(20) NULL, 
    [Assessor1ReviewComment] NVARCHAR(MAX) NULL,
	[Assessor2UserId] NVARCHAR(256) NULL, 
	[Assessor2ReviewStatus] NVARCHAR(20) NULL,
	[Assessor2ReviewComment] NVARCHAR(MAX) NULL,
	[CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] NVARCHAR(256) NOT NULL, 
    [UpdatedAt] DATETIME2 NULL, 
    [UpdatedBy] NVARCHAR(256) NULL, 
 CONSTRAINT [PK_AssessorPageReviewOutcome] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY], 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE INDEX [IX_AssessorPageReviewOutcome_ApplicationId] ON [AssessorPageReviewOutcome] ([ApplicationId])
GO
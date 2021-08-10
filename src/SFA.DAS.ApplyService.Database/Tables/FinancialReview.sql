CREATE TABLE [dbo].[FinancialReview]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY CLUSTERED DEFAULT NEWID(),
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[Status] nvarchar(20)  NOT NULL,
	[SelectedGrade] [nvarchar](20) NULL,
	[FinancialDueDate] [datetime2](7) NULL,
	[GradedBy] [nvarchar](256) NULL,
	[GradedOn] [datetime2](7) NULL,
	[Comments] [nvarchar](max) NULL,
	[ExternalComments] [nvarchar](max) NULL,
	FinancialEvidences [nvarchar] (max) Null,
	[ClarificationRequestedOn] [datetime2](7) NULL,
	[ClarificationRequestedBy] [nvarchar](256) NULL,
	[ClarificationResponse] [nvarchar](max) NULL
)

GO

CREATE INDEX [IX_FinancialReview_ApplicationId] ON [dbo].[FinancialReview] (ApplicationId)

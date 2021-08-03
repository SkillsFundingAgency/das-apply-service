CREATE TABLE [dbo].[FinancialReviewClarificationFile]
(
		 [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY  DEFAULT NEWID(),
	 [ApplicationId] [uniqueidentifier] NOT NULL,
	[Filename] [nvarchar](max) NOT NULL
)

GO

CREATE INDEX [IX_FinancialReviewClarificationFile_ApplicationId] ON [dbo].[FinancialReviewClarificationFile] (ApplicationId)

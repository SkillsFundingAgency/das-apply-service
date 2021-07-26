CREATE TABLE [dbo].[FinancialReviewEvidenceFile]
(
	 [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY CLUSTERED DEFAULT NEWID(),
	 [ApplicationId] [uniqueidentifier] NOT NULL,
	[Filename] [nvarchar](max) NOT NULL
)

GO

CREATE INDEX [IX_FinancialReviewEvidenceFile_ApplicationId] ON [dbo].[FinancialReviewEvidenceFile] (ApplicationId)

CREATE TABLE [dbo].[Apply]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [ApplicationId] UNIQUEIDENTIFIER NOT NULL, 
    [OrganisationId] UNIQUEIDENTIFIER NOT NULL, 
    [ApplicationStatus] NVARCHAR(20) NOT NULL DEFAULT 'New', 
    [ApplyData] NVARCHAR(MAX) NULL, 
    [GatewayReviewStatus] NVARCHAR(20) NOT NULL DEFAULT 'Draft',
    [AssessorReviewStatus] NVARCHAR(20) NOT NULL DEFAULT 'Draft',
    [FinancialReviewStatus] NVARCHAR(20) NOT NULL DEFAULT 'Draft',
    [FinancialGrade] NVARCHAR(MAX) NULL, 
    [CreatedAt] DATETIME2 NOT NULL, 
    [CreatedBy] NVARCHAR(256) NOT NULL, 
    [UpdatedAt] DATETIME2 NULL, 
    [UpdatedBy] NVARCHAR(256) NULL, 
    [DeletedAt] DATETIME2 NULL, 
    [DeletedBy] NVARCHAR(256) NULL,
    [UKPRN] AS JSON_VALUE(ApplyData, '$.ApplyDetails.UKPRN') PERSISTED,
    [NotRequiredOverrides] NVARCHAR(MAX) NULL
)
GO
CREATE INDEX [IX_Apply_ApplicationId] ON [Apply] ([ApplicationId])
GO
CREATE INDEX [IX_Apply_OrganisationId] ON [Apply] ([OrganisationId])
GO
CREATE INDEX [IX_Apply_CreatedBy] ON [Apply] ([CreatedBy])
GO
CREATE INDEX [IX_Apply_ApplicationStatus] ON [Apply] ([ApplicationStatus])
GO
CREATE INDEX [IX_Apply_UKPRN] ON [Apply] ([UKPRN])
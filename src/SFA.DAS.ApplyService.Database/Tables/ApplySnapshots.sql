CREATE TABLE [dbo].[ApplySnapshots]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [ApplicationId] UNIQUEIDENTIFIER NOT NULL, 
    [SnapshotApplicationId] UNIQUEIDENTIFIER NOT NULL,
    [SnapshotDate] DATETIME2 NOT NULL, 
    [OrganisationId] UNIQUEIDENTIFIER NOT NULL, 
    [ApplicationStatus] NVARCHAR(20) NOT NULL, 
    [ApplyData] NVARCHAR(MAX) NULL, 
    [GatewayReviewStatus] NVARCHAR(20) NOT NULL,
    [AssessorReviewStatus] NVARCHAR(20) NOT NULL,
    [FinancialReviewStatus] NVARCHAR(20) NOT NULL,
    [FinancialGrade] NVARCHAR(MAX) NULL
)
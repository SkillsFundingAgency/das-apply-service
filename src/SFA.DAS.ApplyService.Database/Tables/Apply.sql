CREATE TABLE [dbo].[Apply]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [ApplicationId] UNIQUEIDENTIFIER NOT NULL, 
    [OrganisationId] UNIQUEIDENTIFIER NOT NULL, 
    [ApplicationStatus] NVARCHAR(20) NOT NULL DEFAULT 'New', 
    [ApplyData] NVARCHAR(MAX) NULL, 
    [ReviewStatus] NVARCHAR(20) NOT NULL DEFAULT 'Draft', 
    [GatewayReviewStatus] NVARCHAR(20) NOT NULL DEFAULT 'Draft',
    [CreatedAt] DATETIME2 NOT NULL, 
    [CreatedBy] NVARCHAR(256) NOT NULL, 
    [UpdatedAt] DATETIME2 NULL, 
    [UpdatedBy] NVARCHAR(256) NULL, 
    [DeletedAt] DATETIME2 NULL, 
    [DeletedBy] NVARCHAR(256) NULL
)
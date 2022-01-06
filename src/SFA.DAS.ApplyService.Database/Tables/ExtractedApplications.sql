CREATE TABLE [dbo].[ExtractedApplications]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
    [ApplicationId] UNIQUEIDENTIFIER NOT NULL,  
    [ExtractedDate] DATETIME2 NOT NULL,
    [GatewayFilesExtracted] BIT NOT NULL,
    [AssessorFilesExtracted] BIT NOT NULL,
    [FinanceFilesExtracted] BIT NOT NULL,
    [AppealFilesExtracted] BIT NOT NULL DEFAULT 0,
	
    CONSTRAINT PK_ExtractedApplications PRIMARY KEY (Id),
    INDEX IX_ExtractedApplications_ApplicationId (ApplicationId)
)
GO
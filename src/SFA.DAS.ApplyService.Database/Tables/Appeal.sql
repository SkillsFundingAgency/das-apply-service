CREATE TABLE [dbo].[Appeal]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    [ApplicationId] UNIQUEIDENTIFIER NOT NULL,
    [Status] TINYINT NOT NULL,
    [HowFailedOnPolicyOrProcesses] NVARCHAR(MAX) NULL,
    [HowFailedOnEvidenceSubmitted] NVARCHAR(MAX) NULL,
    [AppealSubmittedDate] DATETIME2 NULL,
    [AppealDeterminedDate] DATETIME2 NULL,
    [InternalComments] NVARCHAR(MAX) NULL,
    [ExternalComments] NVARCHAR(MAX) NULL,
    [UserId] NVARCHAR(256) NULL,
    [UserName] NVARCHAR(256) NULL,
    [InProgressDate] DATETIME2 NULL,
    [InProgressUserId] NVARCHAR(256) NULL,
    [InProgressUserName] NVARCHAR(256) NULL,
    [InProgressInternalComments] NVARCHAR(MAX) NULL,
    [InProgressExternalComments] NVARCHAR(MAX) NULL,
    [CreatedOn] DATETIME2 NOT NULL,
    [UpdatedOn] DATETIME2 NULL
)
GO

ALTER TABLE [dbo].[Appeal] ADD CONSTRAINT [FK_Appeal_Apply] FOREIGN KEY(ApplicationId)
REFERENCES [dbo].[Apply] ([ApplicationId])
GO


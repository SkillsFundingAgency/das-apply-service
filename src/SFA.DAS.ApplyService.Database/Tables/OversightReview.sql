CREATE TABLE [dbo].[OversightReview]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
	[ApplicationId] UNIQUEIDENTIFIER NOT NULL,
    [GatewayApproved] BIT NULL,
    [ModerationApproved] BIT NULL,
    [Status] TINYINT NOT NULL, 
    [ApplicationDeterminedDate] DATETIME2 NULL,
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

ALTER TABLE [dbo].[OversightReview] ADD CONSTRAINT [FK_OversightReview_Apply] FOREIGN KEY(ApplicationId)
REFERENCES [dbo].[Apply] ([ApplicationId])
GO

CREATE UNIQUE INDEX [IX_OversightReview_ApplicationId] ON [OversightReview] ([ApplicationId])
GO

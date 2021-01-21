CREATE TABLE [dbo].[OversightReview]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
	[ApplicationId] UNIQUEIDENTIFIER NOT NULL,
    [Status] NVARCHAR(30) NOT NULL DEFAULT 'New', 
    [ApplicationDeterminedDate] DATETIME2 NULL,
    [InternalComments] NVARCHAR(MAX) NULL,
    [ExternalComments] NVARCHAR(MAX) NULL,
    [UserId] NVARCHAR(256) NULL,
    [UserName] NVARCHAR(256) NULL,
    [CreatedOn] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
)
GO

ALTER TABLE [dbo].[OversightReview] ADD CONSTRAINT [FK_OversightReview_Apply] FOREIGN KEY(ApplicationId)
REFERENCES [dbo].[Apply] ([ApplicationId])
GO
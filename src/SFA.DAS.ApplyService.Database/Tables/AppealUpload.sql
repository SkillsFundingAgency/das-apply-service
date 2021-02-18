CREATE TABLE [dbo].[AppealUpload]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [ApplicationId] UNIQUEIDENTIFIER NOT NULL,
    [AppealId] UNIQUEIDENTIFIER NULL,
    [FileId] UNIQUEIDENTIFIER NOT NULL,
    [Filename] NVARCHAR(256) NOT NULL,
    [ContentType] NVARCHAR(256) NOT NULL,
    [Size] INT NOT NULL,
    [UserId] NVARCHAR(256) NULL,
    [UserName] NVARCHAR(256) NULL,
    [CreatedOn] DATETIME2 NOT NULL
)
GO

ALTER TABLE [dbo].[AppealUpload] ADD CONSTRAINT [FK_AppealUpload_Apply] FOREIGN KEY(ApplicationId)
REFERENCES [dbo].[Apply] ([ApplicationId])
GO


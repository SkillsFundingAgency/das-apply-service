CREATE TABLE [dbo].[Appeal]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[ApplicationId] UNIQUEIDENTIFIER NOT NULL,
    [Message] NVARCHAR(MAX) NOT NULL,
    [UserId] NVARCHAR(256) NULL,
    [UserName] NVARCHAR(256) NULL,
    [CreatedOn] DATETIME2 NOT NULL	
)
GO

ALTER TABLE [dbo].[Appeal] ADD CONSTRAINT [FK_Appeal_Apply] FOREIGN KEY(ApplicationId)
REFERENCES [dbo].[Apply] ([ApplicationId])
GO

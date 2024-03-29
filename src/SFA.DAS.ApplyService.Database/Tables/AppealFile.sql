﻿CREATE TABLE [dbo].[AppealFile]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    [ApplicationId] UNIQUEIDENTIFIER NOT NULL,
    [FileName] NVARCHAR(256) NOT NULL,
    [ContentType] NVARCHAR(256) NOT NULL,
    [Size] INT NOT NULL,
    [UserId] NVARCHAR(256) NULL,
    [UserName] NVARCHAR(256) NULL,
    [CreatedOn] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
)
GO

ALTER TABLE [dbo].[AppealFile] ADD CONSTRAINT [FK_AppealFile_Apply] FOREIGN KEY(ApplicationId)
REFERENCES [dbo].[Apply] ([ApplicationId])
GO

CREATE INDEX [IX_AppealFile_ApplicationId] ON [AppealFile] ([ApplicationId])
GO

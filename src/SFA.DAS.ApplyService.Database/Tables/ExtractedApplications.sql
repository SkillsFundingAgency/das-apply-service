CREATE TABLE [dbo].[ExtractedApplications]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
    [ApplicationId] UNIQUEIDENTIFIER NOT NULL,  
    [ExtractedDate] DATETIME2 NOT NULL,

    CONSTRAINT PK_ExtractedApplications PRIMARY KEY (Id),
    CONSTRAINT [FK_ExtractedApplications_Apply] FOREIGN KEY([ApplicationId])
    REFERENCES [dbo].[Apply]([ApplicationId])
)
GO
CREATE TABLE [dbo].[ApplicationSequences](
	[Id] [uniqueidentifier] NOT NULL,
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[SequenceId] [int] NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL DEFAULT 0, 
    [SequenceData] NVARCHAR(MAX) NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ApplicationSequences] ADD  CONSTRAINT [DF_ApplicationSequences_Id_1]  DEFAULT (newid()) FOR [Id]
GO



CREATE TABLE [dbo].[WorkflowSections](
	[Id] [uniqueidentifier] NOT NULL,
	[WorkflowId] [uniqueidentifier] NOT NULL,
	[SequenceId] [int] NOT NULL,
	[SectionId] [int] NOT NULL,
	[QnAData] [nvarchar](max) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[LinkTitle] [nvarchar](50) NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
	[DisplayType] [nvarchar](50) NOT NULL,
	[DisallowedOrgTypes] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[WorkflowSections] ADD  CONSTRAINT [DF_WorkflowSections_Id]  DEFAULT (newid()) FOR [Id]
GO



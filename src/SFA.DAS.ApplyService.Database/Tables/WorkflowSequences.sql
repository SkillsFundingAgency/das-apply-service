CREATE TABLE [dbo].[WorkflowSequences](
	[Id] [uniqueidentifier] NOT NULL,
	[WorkflowId] [uniqueidentifier] NOT NULL,
	[SequenceId] [int] NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL DEFAULT 0,
	[Description] NVARCHAR(255) NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[WorkflowSequences] ADD  CONSTRAINT [DF_WorkflowSequences_Id]  DEFAULT (newid()) FOR [Id]
GO


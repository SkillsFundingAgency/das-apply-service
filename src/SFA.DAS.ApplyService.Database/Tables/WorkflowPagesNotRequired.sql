CREATE TABLE [dbo].[WorkflowPagesNotRequired](
	[Id] [uniqueidentifier] NOT NULL,
	[SequenceId] [int] NOT NULL,
	[SectionId] [int] NOT NULL,
	[PageId] [int] NOT NULL,
	[OrganisationType] [nvarchar](50) NOT NULL,
	[Status] [nvarchar](50) NOT NULL, 
    CONSTRAINT [PK_WorkflowPagesNotRequired] PRIMARY KEY ([Id])
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[WorkflowPagesNotRequired] ADD  CONSTRAINT [DF_WorkflowPagesNotRequired_Id]  DEFAULT (newid()) FOR [Id]
GO



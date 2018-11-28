CREATE TABLE [dbo].[Applications](
	[Id] [uniqueidentifier] NOT NULL,
	[ApplyingOrganisationId] [uniqueidentifier] NOT NULL,
	[ApplicationStatus] [nvarchar](20) NOT NULL,
	[ApplicationData] [nvarchar(max)] NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](120) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](120) NULL,
	[WithdrawnAt] [datetime2](7) NULL,
	[WithdrawnBy] [nvarchar](120) NULL,
	[DeletedAt] [datetime2](7) NULL,
	[DeletedBy] [nvarchar](120) NULL,
	[CreatedFromWorkflowId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Applications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Applications] ADD  CONSTRAINT [Applications_Id]  DEFAULT (newid()) FOR [Id]
GO



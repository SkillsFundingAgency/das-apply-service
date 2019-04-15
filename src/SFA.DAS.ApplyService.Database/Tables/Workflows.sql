CREATE TABLE [dbo].[Workflows](
	[Id] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](200) NOT NULL,
	[Version] [nvarchar](10) NOT NULL,
	[Type] [nvarchar](10) NOT NULL,
	[Status] [nvarchar](20) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](256) NULL,
	[DeletedAt] [datetime2](7) NULL,
	[DeletedBy] [nvarchar](256) NULL,
 [ReferenceFormat] NVARCHAR(10) NULL, 
    CONSTRAINT [PK_Workflows] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Workflows] ADD  DEFAULT (newid()) FOR [Id]
GO


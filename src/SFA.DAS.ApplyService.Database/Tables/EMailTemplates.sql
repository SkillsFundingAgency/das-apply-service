CREATE TABLE [dbo].[EmailTemplates](
	[Id] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
	[Status] [nvarchar](20)	NOT NULL,
	[TemplateName] [nvarchar](max) NOT NULL,
	[TemplateId] [nvarchar](max) NOT NULL,
	[Recipients] [nvarchar](max) NOT NULL,
	[RecipientTemplateId] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[CreatedBy]	[nvarchar](120)	NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[UpdatedBy]	[nvarchar](120)	NULL,
	[DeletedAt] [datetime2](7) NULL,
	[DeletedBy]	[nvarchar](120)	NULL,

    CONSTRAINT [PK_EmailTemplates] PRIMARY KEY ([Id]),
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


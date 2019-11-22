CREATE TABLE [review].[Audit](
 [Id] [bigint] IDENTITY(1,1) NOT NULL,
 [Action] NVARCHAR(50) NOT NULL,
 [ApplicationId] [uniqueidentifier] NOT NULL,
 [UserId] [nvarchar](100) NOT NULL,
 [ChangedAt] [datetime2](7) NOT NULL,
 [ChangeDelta] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Audit] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

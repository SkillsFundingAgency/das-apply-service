CREATE TABLE [dbo].[GatewayAnswer](
	[Id] [uniqueidentifier] NOT NULL,
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[PageId] [nvarchar](20) NOT NULL,
	[Status] [nvarchar](20) NULL,
	[GatewayPageData] nvarchar(max) NULL,
	[CreatedAt] DATETIME2 NOT NULL, 
    [CreatedBy] NVARCHAR(256) NOT NULL, 
    [UpdatedAt] DATETIME2 NULL, 
    [UpdatedBy] NVARCHAR(256) NULL, 
 CONSTRAINT [PK_GatewayStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY], 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].GatewayAnswer ADD  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].GatewayAnswer ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO

CREATE UNIQUE INDEX GatewayAnswer_pk
    ON GatewayAnswer (ApplicationId, PageId) INCLUDE (Status);
GO


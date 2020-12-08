CREATE TABLE [dbo].[Audit]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[EntityType] NVARCHAR(256) NOT NULL,
	[EntityId] UNIQUEIDENTIFIER NOT NULL,
	[UserId] NVARCHAR(256),
	[UserName] NVARCHAR(256),
	[UserAction] NVARCHAR(256),
	[AuditDate] DATETIME2,
	[InitialState] [nvarchar](max) NULL,
	[UpdatedState] [nvarchar](max) NULL,
	[Diff] [nvarchar](max) NULL,
	[CorrelationId] [uniqueidentifier] NULL
)

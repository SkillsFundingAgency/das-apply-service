CREATE TABLE [dbo].[ApplicationWorkflow]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[ApplicationId] UNIQUEIDENTIFIER,
	[ApplicationSectionId] UNIQUEIDENTIFIER,
	[Completed] BIT
)

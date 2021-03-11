CREATE TABLE [dbo].[SubmittedApplicationAnswers]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
    [ApplicationId] UNIQUEIDENTIFIER NOT NULL,  
    [PageId] NVARCHAR(10) NOT NULL,
	[QuestionId] NVARCHAR(10) NOT NULL,
	[QuestionType] NVARCHAR(10) NOT NULL,
	[Answer] NVARCHAR(MAX) NULL,
	[ColumnHeading] NVARCHAR(100) NULL,

	CONSTRAINT PK_SubmittedApplicationAnswers PRIMARY KEY (Id),
	INDEX IX_SubmittedApplicationAnswers_ByApplication (ApplicationId)
)
GO
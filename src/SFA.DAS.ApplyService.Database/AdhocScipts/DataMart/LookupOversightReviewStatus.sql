DROP TABLE IF EXISTS LookupOversightReviewStatus
GO

CREATE TABLE LookupOversightReviewStatus
(
	[Status] INT NOT NULL PRIMARY KEY,
	[Description] NVARCHAR(50)
)
GO

INSERT INTO LookupOversightReviewStatus ([Status],[Description]) VALUES (1, 'Successful')
INSERT INTO LookupOversightReviewStatus ([Status],[Description]) VALUES (2, 'Successful - Already Active')
INSERT INTO LookupOversightReviewStatus ([Status],[Description]) VALUES (3, 'Successful - Fitness For Funding')
INSERT INTO LookupOversightReviewStatus ([Status],[Description]) VALUES (4, 'Unsuccessful')
INSERT INTO LookupOversightReviewStatus ([Status],[Description]) VALUES (5, 'In Progress')
INSERT INTO LookupOversightReviewStatus ([Status],[Description]) VALUES (6, 'Rejected')
INSERT INTO LookupOversightReviewStatus ([Status],[Description]) VALUES (7, 'Withdrawn')
INSERT INTO LookupOversightReviewStatus ([Status],[Description]) VALUES (8, 'Removed')
GO


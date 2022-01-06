DROP TABLE IF EXISTS LookupSequenceTitles
GO

CREATE TABLE LookupSequenceTitles
(
	[SequenceNumber] INT NOT NULL,
	[Title] NVARCHAR(100)
)
GO

INSERT INTO LookupSequenceTitles ([SequenceNumber],[Title]) VALUES
(0, 'Preamble'),
(1, 'Your organisation'),
(2, 'Financial evidence'),
(3, 'Criminal and compliance checks'),
(4, 'Protecting your apprentices'),
(5, 'Readiness to engage'),
(6, 'Planning apprenticeship training'),
(7, 'Delivering apprenticeship training'),
(8, 'Evaluating apprenticeship training'),
(9, 'Finish');
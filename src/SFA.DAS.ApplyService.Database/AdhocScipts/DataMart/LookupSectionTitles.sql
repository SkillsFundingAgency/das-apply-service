DROP TABLE IF EXISTS LookupSectionTitles
GO

CREATE TABLE LookupSectionTitles
(
	[SequenceNumber] INT NOT NULL,
	[SectionNumber] INT NOT NULL,
	[Title] NVARCHAR(100)
)
GO

INSERT INTO LookupSectionTitles ([SequenceNumber],[SectionNumber],[Title]) VALUES
(0, 1, 'Preamble'),

(1, 1, 'Introduction and what you''ll need'),
(1, 2, 'Organisation information'),
(1, 3, 'Confirm who''s in control'),
(1, 4, 'Describe your organisation'),
(1, 5, 'Experience and accreditation'),

(2, 1, 'Introduction and what you''ll need'),
(2, 2, 'Your organisation''s financial evidence'),
(2, 3, 'Your UK ultimate parent company''s financial evidence'),

(3, 1, 'Introduction and what you''ll need'),
(3, 2, 'Checks on your organisation'),
(3, 3, 'Introduction and what you''ll need'),
(3, 4, 'Checks on who''s in control of your organisation'),

(4, 1, 'Introduction and what you''ll need'),
(4, 2, 'Continuity plan for apprenticeship training'),
(4, 3, 'Equality and diversity policy'),
(4, 4, 'Safeguarding and Prevent duty policy'),
(4, 5, 'Health and safety policy'),
(4, 6, 'Acting as a subcontractor'),

(5, 1, 'Introduction and what you''ll need'),
(5, 2, 'Engaging with employers'),
(5, 3, 'Complaints policy'),
(5, 4, 'Contract for services template'),
(5, 5, 'Commitment statement template'),
(5, 6, 'Prior learning of apprentices'),
(5, 7, 'English and maths assessments'),
(5, 8, 'Working with subcontractors'),

(6, 1, 'Introduction and what you''ll need'),
(6, 2, 'Type of apprenticeship training'),
(6, 3, 'Training apprentices'),
(6, 4, 'Supporting apprentices'),
(6, 5, 'Forecasting starts'),
(6, 6, 'Off the job training'),
(6, 7, 'Where will your apprentices be trained'),

(7, 1, 'Introduction and what you''ll need'),
(7, 2, 'Overall accountability for apprenticeships'),
(7, 3, 'Management hierarchy for apprenticeships'),
(7, 4, 'Quality and high standards in apprenticeship training'),
(7, 5, 'Developing and delivering training'),
(7, 6, 'Sectors and employee experience'),
(7, 7, 'Policy for professional development of employees'),

(8, 1, 'Introduction and what you''ll need'),
(8, 2, 'Process for evaluating the quality of training delivered'),
(8, 3, 'Process of evaluating the quality of apprenticeship training'),
(8, 4, 'Systems and processes to collect apprenticeship data'),

(9, 1, 'Application permissions and checks'),
(9, 2, 'Quality statement'),
(9, 3, 'Post application tasks'),
(9, 4, 'Submit application');
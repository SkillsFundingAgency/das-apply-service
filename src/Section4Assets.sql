--SECTION 4
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-T-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-T-1', '', 'Your policies and procedures', 'Live', GETUTCDATE(), 'Import')
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-LT-1', '', '', 'Live', GETUTCDATE(), 'Import')

-- page 24 question 1
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-01-L-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-01-L-1', '', 'Information Commisioner''s Office (ICO) registration number', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-01-SL-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-01-SL-1', '', 'Information Commisoner''s Office Registration', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-01-QB-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-01-QB-1', '','Provide your Information Commisioner''s Office (ICO) registration number', 'Live', GETUTCDATE(), 'Import')
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-01-H-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-01-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-BT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-BT-1', '', '', 'Live', GETUTCDATE(), 'Import')

-- page 24 question 2
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-02-L-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-02-L-1', '', 'Internal audit policy', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-02-SL-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-02-SL-1', '', 'Internal Audit Policy', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-02-QB-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-02-QB-1', '','Upload a PDF of your organisation''s internal audit policy in respect to fraud and financial irregularity', 'Live', GETUTCDATE(), 'Import')
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-02-H-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-02-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

-- page 24 question 3
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-03-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-03-L-1', '', 'Public Liability Insurance', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-03-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-03-SL-1', '', 'Public Liability Insurance', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-03-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-03-QB-1', '','Upload a PDF of your public liability certificate of insurance', 'Live', GETUTCDATE(), 'Import')
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-03-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-03-H-1', '', 'If you are providing any form of training or consultancy, you must have public liability insurance.', 'Live', GETUTCDATE(), 'Import')


-- page 24 question 4
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-04-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-04-L-1', '', 'Professional indemnity insurance', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-04-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-04-SL-1', '', 'Professional indemnity insurance', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-04-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-04-QB-1', '','Upload a PDF of your professional indemnity certificate of insurance', 'Live', GETUTCDATE(), 'Import')
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-04-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-04-H-1', '', 'If you are providing any form of training or consultancy, you must have professional indemnity insurance.', 'Live', GETUTCDATE(), 'Import')

-- page 24 question 5
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-05-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-05-L-1', '', 'Employers liability insurance', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-05-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-05-SL-1', '', 'Employers liability insurance', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-05-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-05-QB-1', '','Upload a PDF of your employers liability certificate of insurance (optional)', 'Live', GETUTCDATE(), 'Import')
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-05-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-05-H-1', '', 'If you have any employees, you must have employers liability insurance. ', 'Live', GETUTCDATE(), 'Import')

  -- page 24 question 6
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-06-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-06-L-1', '', 'Safeguarding policy', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-06-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-06-SL-1', '', 'Safeguarding policy', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-06-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-06-QB-1', '','Upload a PDF of your safeguarding policy', 'Live', GETUTCDATE(), 'Import')
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-06-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-06-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

-- page 24 question 7
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-07-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-07-L-1', '', 'Prevent Agenda policy', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-07-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-07-SL-1', '', 'Prevent Agenda policy', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-07-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-07-QB-1', '','Upload your PDF conflict of interest policy document', 'Live', GETUTCDATE(), 'Import')
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-07-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-07-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

  -- page 24 question 8
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-08-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-08-L-1', '', 'Conflict of interest policy', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-08-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-08-SL-1', '', 'Conflict of interest policy', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-08-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-08-QB-1', '','Upload a PDF of your conflict of interest policy', 'Live', GETUTCDATE(), 'Import')
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-08-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-08-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

-- page 24 question 9
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-09-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-09-L-1', '', 'Monitoring procedures', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-09-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-09-SL-1', '', 'Monitoring procedures', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-09-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-09-QB-1', '','Upload a PDF of your procedures for monitoring assessor practices and decisions', 'Live', GETUTCDATE(), 'Import')
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-09-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-09-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

-- page 24 question 10
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-10-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-10-L-1', '', 'Moderation processes', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-10-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-10-SL-1', '', 'Moderation processes', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-10-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-10-QB-1', '','Upload a PDF describing your standardisation and moderation activities, including how you sample assessment decisions', 'Live', GETUTCDATE(), 'Import')
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-10-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-10-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

-- page 24 question 11
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-11-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-11-L-1', '', 'Complaints and appeals policy', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-11-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-11-SL-1', '', 'Employers liability insurance', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-11-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-11-QB-1', '','Upload a PDF of your complaints and appeals policy', 'Live', GETUTCDATE(), 'Import')
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-11-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-11-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

  -- page 24 question 12
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-12-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-12-L-1', '', 'Fair Access', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-12-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-12-SL-1', '', 'Fair Access', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-12-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-12-QB-1', '','Upload a PDF of your Fair Access policy', 'Live', GETUTCDATE(), 'Import')
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-12-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-12-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

  -- page 24 question 13
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-13-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-13-L-1', '', 'Consistency assurance', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-13-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-13-SL-1', '', 'Consistency assurance', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-13-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-13-QB-1', '','Upload a PDF of your strategy for ensuring comparability and consistency of assessment decisions', 'Live', GETUTCDATE(), 'Import')
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-13-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-13-H-1', '', '', 'Live', GETUTCDATE(), 'Import')
  

  -- page 24 question 14
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-14-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-14-L-1', '', 'How do you continuously improve the quality of your assessment practice?', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-14-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-14-SL-1', '', 'Continuous quality assurance', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-14-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-14-QB-1', '','Upload a PDF of your employers liability certificate of insurance (optional)', 'Live', GETUTCDATE(), 'Import')
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-14-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-14-H-1', '', 'If you have any employees, you must have employers liability insurance. ', 'Live', GETUTCDATE(), 'Import')

--  -- page 24 question 15
--DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-15-L-1';  -- Row L
--INSERT INTO Assets
--  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
--VALUES
--  (NEWID(), 'SQ-2-SE-4-PG-24-CC-15-L-1', '', 'Employers liability insurance', 'Live', GETUTCDATE(), 'Import')

--DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-15-SL-1'; -- Row J
--INSERT INTO Assets
--  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
--VALUES
--  (NEWID(), 'SQ-2-SE-4-PG-24-CC-15-SL-1', '', 'Employers liability insurance', 'Live', GETUTCDATE(), 'Import')

--DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-15-QB-1';   -- Row P
--INSERT INTO Assets
--  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
--VALUES
--  (NEWID(), 'SQ-2-SE-4-PG-24-CC-15-QB-1', '','Upload a PDF of your employers liability certificate of insurance (optional)', 'Live', GETUTCDATE(), 'Import')
--DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-15-H-1';   -- Row AA
--INSERT INTO Assets
--  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
--VALUES
--  (NEWID(), 'SQ-2-SE-4-PG-24-CC-15-H-1', '', 'If you have any employees, you must have employers liability insurance. ', 'Live', GETUTCDATE(), 'Import')



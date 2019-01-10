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




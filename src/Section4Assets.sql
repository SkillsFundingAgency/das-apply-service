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
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-01-L-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-01-L-1', '', 'Internal audit policy', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-01-SL-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-01-SL-1', '', 'Internal Audit Policy', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-01-QB-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-01-QB-1', '','Upload a PDF of your organisation''s internal audit policy in respect to fraud and financial irregularity.', 'Live', GETUTCDATE(), 'Import')
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

-- page 25
--DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-T-1';
--INSERT INTO Assets
--  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
--VALUES
--  (NEWID(), 'SQ-2-SE-4-PG-24-T-1', '', 'Internal audit policy', 'Live', GETUTCDATE(), 'Import')

--DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-LT-1';
--INSERT INTO Assets
--  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
--VALUES
--  (NEWID(), 'SQ-2-SE-4-PG-24-LT-1', '', 'Information Commisoner''s Office Registration', 'Live', GETUTCDATE(), 'Import')
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-02-L-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-02-L-1', '', 'Information Commisioner''s Office (ICO) registration number', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-02-SL-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-02-SL-1', '', 'Information Commisoner''s Office Registration', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-02-QB-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-02-QB-1', '','Provide your Information Commisioner''s Office (ICO) registration number', 'Live', GETUTCDATE(), 'Import')
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-02-H-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-02-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-BT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-BT-1', '', '', 'Live', GETUTCDATE(), 'Import')


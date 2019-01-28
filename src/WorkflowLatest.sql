--USE [SFA.DAS.ApplyService]
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-1-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-1-1', '', 'Trading name', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-1-2';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-1-2', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-1-3';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-1-3', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-1-4';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-1-4', '', 'Trading name', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-1-CD-30-5';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-1-CD-30-5', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-1-CD-30-6';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-1-CD-30-6', '', 'Does your organisation have a trading name?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-1-CD-30-7';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-1-CD-30-7', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-1-CD-30-8';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-1-CD-30-8', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-2-9';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-2-9', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-2-10';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-2-10', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-2-11';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-2-11', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-2-12';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-2-12', '', 'Name to use on the register', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-2-CD-01-13';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-2-CD-01-13', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-2-CD-01-14';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-2-CD-01-14', '', 'Do you want to use your trading name on the register?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-2-CD-01-15';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-2-CD-01-15', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-2-CD-01-16';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-2-CD-01-16', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-2-CD-01.1-17';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-2-CD-01.1-17', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-2-CD-01.1-18';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-2-CD-01.1-18', '', 'What is your trading name?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-2-CD-01.1-19';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-2-CD-01.1-19', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-2-CD-01.1-20';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-2-CD-01.1-20', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-3-21';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-3-21', '', 'Enter contact details', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-3-22';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-3-22', '', '<p class="govuk-body">This information will be published on the Register of end point assessment organisations and will be made available to the public.</p>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-3-23';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-3-23', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-3-24';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-3-24', '', 'Contact details', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-3-CD-02-25';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-3-CD-02-25', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-3-CD-02-26';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-3-CD-02-26', '', 'Full name', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-3-CD-02-27';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-3-CD-02-27', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-3-CD-02-28';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-3-CD-02-28', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-3-CD-03-29';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-3-CD-03-29', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-3-CD-03-30';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-3-CD-03-30', '', 'Address', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-3-CD-03-31';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-3-CD-03-31', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-3-CD-03-32';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-3-CD-03-32', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-3-CD-04-33';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-3-CD-04-33', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-3-CD-04-34';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-3-CD-04-34', '', 'Postcode', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-3-CD-04-35';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-3-CD-04-35', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-3-CD-04-36';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-3-CD-04-36', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-3-CD-05-37';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-3-CD-05-37', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-3-CD-05-38';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-3-CD-05-38', '', 'Email address', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-3-CD-05-39';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-3-CD-05-39', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-3-CD-05-40';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-3-CD-05-40', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-3-CD-06-41';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-3-CD-06-41', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-3-CD-06-42';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-3-CD-06-42', '', 'Telephone', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-3-CD-06-43';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-3-CD-06-43', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-3-CD-06-44';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-3-CD-06-44', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-4-45';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-4-45', '', 'Who should we send the contract notice to', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-4-46';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-4-46', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-4-47';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-4-47', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-4-48';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-4-48', '', 'Contract notice contact details', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-4-CD-07-49';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-4-CD-07-49', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-4-CD-07-50';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-4-CD-07-50', '', 'Full name', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-4-CD-07-51';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-4-CD-07-51', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-4-CD-07-52';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-4-CD-07-52', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-4-CD-08-53';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-4-CD-08-53', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-4-CD-08-54';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-4-CD-08-54', '', 'Address', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-4-CD-08-55';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-4-CD-08-55', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-4-CD-08-56';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-4-CD-08-56', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-4-CD-09-57';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-4-CD-09-57', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-4-CD-09-58';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-4-CD-09-58', '', 'Postcode', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-4-CD-09-59';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-4-CD-09-59', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-4-CD-09-60';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-4-CD-09-60', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-4-CD-10-61';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-4-CD-10-61', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-4-CD-10-62';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-4-CD-10-62', '', 'Email address', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-4-CD-10-63';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-4-CD-10-63', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-4-CD-10-64';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-4-CD-10-64', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-4-CD-11-65';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-4-CD-11-65', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-4-CD-11-66';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-4-CD-11-66', '', 'Telephone', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-4-CD-11-67';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-4-CD-11-67', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-4-CD-11-68';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-4-CD-11-68', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-5-69';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-5-69', '', 'UK provider registration number (UKPRN)', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-5-70';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-5-70', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-5-71';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-5-71', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-5-72';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-5-72', '', 'UK provider registration number (UKPRN)', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-5-CD-12-73';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-5-CD-12-73', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-5-CD-12-74';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-5-CD-12-74', '', 'Do you have a UK provider registration number (UKPRN)?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-5-CD-12-75';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-5-CD-12-75', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-5-CD-12-76';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-5-CD-12-76', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-5-CD-12.1-77';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-5-CD-12.1-77', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-5-CD-12.1-78';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-5-CD-12.1-78', '', 'Provide your UKPRN', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-5-CD-12.1-79';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-5-CD-12.1-79', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-5-CD-12.1-80';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-5-CD-12.1-80', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-6-81';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-6-81', '', 'Who has responsibility for the overall executive management of your organisation?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-6-82';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-6-82', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-6-83';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-6-83', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-6-84';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-6-84', '', 'Overall executive management', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-6-CD-13-85';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-6-CD-13-85', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-6-CD-13-86';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-6-CD-13-86', '', 'Full name', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-6-CD-13-87';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-6-CD-13-87', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-6-CD-13-88';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-6-CD-13-88', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-6-CD-14-89';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-6-CD-14-89', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-6-CD-14-90';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-6-CD-14-90', '', 'Do they hold any other positions or directorships of other organisations?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-6-CD-14-91';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-6-CD-14-91', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-6-CD-14-92';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-6-CD-14-92', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-6-CD-14.1-93';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-6-CD-14.1-93', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-6-CD-14.1-94';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-6-CD-14.1-94', '', 'Provide details of other positions or directorships ', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-6-CD-14.1-95';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-6-CD-14.1-95', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-6-CD-14.1-96';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-6-CD-14.1-96', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-7-97';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-7-97', '', 'Ofqual recognition number', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-7-98';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-7-98', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-7-99';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-7-99', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-7-100';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-7-100', '', 'Ofqual recognition number', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-7-CD-15-101';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-7-CD-15-101', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-7-CD-15-102';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-7-CD-15-102', '', 'Do you have an Ofqual recognition number?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-7-CD-15-103';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-7-CD-15-103', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-7-CD-15-104';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-7-CD-15-104', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-7-CD-15.1-105';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-7-CD-15.1-105', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-7-CD-15.1-106';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-7-CD-15.1-106', '', 'Provide us with your Ofqual recognition number', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-7-CD-15.1-107';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-7-CD-15.1-107', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-7-CD-15.1-108';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-7-CD-15.1-108', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-8-109';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-8-109', '', 'Trading status', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-8-110';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-8-110', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-8-111';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-8-111', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-8-112';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-8-112', '', 'Trading status', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-8-CD-16-113';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-8-CD-16-113', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-8-CD-16-114';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-8-CD-16-114', '', 'What''s your trading status?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-8-CD-16-115';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-8-CD-16-115', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-8-CD-16-116';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-8-CD-16-116', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-8-CD-16.1-117';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-8-CD-16.1-117', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-8-CD-16.1-118';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-8-CD-16.1-118', '', 'Describe your trading status', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-8-CD-16.1-119';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-8-CD-16.1-119', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-8-CD-16.1-120';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-8-CD-16.1-120', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-9-121';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-9-121', '', 'Company number', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-9-122';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-9-122', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-9-123';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-9-123', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-9-124';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-9-124', '', 'Company number', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-9-CD-17-125';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-9-CD-17-125', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-9-CD-17-126';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-9-CD-17-126', '', 'Do you have a company number?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-9-CD-17-127';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-9-CD-17-127', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-9-CD-17-128';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-9-CD-17-128', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-9-CD-17.1-129';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-9-CD-17.1-129', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-9-CD-17.1-130';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-9-CD-17.1-130', '', 'What is your number', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-9-CD-17.1-131';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-9-CD-17.1-131', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-9-CD-17.1-132';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-9-CD-17.1-132', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-10-133';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-10-133', '', 'Part of a group of companies?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-10-134';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-10-134', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-10-135';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-10-135', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-10-136';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-10-136', '', 'Part of a group of companies?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-10-CD-18-137';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-10-CD-18-137', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-10-CD-18-138';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-10-CD-18-138', '', 'Is your parent company registered overseas?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-10-CD-18-139';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-10-CD-18-139', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-10-CD-18-140';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-10-CD-18-140', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-10-CD-18.1-141';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-10-CD-18.1-141', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-10-CD-18.1-142';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-10-CD-18.1-142', '', 'Which country?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-10-CD-18.1-143';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-10-CD-18.1-143', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-10-CD-18.1-144';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-10-CD-18.1-144', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-10-CD-18.2-145';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-10-CD-18.2-145', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-10-CD-18.2-146';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-10-CD-18.2-146', '', 'Registration number', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-10-CD-18.2-147';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-10-CD-18.2-147', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-10-CD-18.2-148';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-10-CD-18.2-148', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-11-149';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-11-149', '', 'Director details', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-11-150';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-11-150', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-11-151';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-11-151', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-11-152';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-11-152', '', 'Directors', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-11-CD-19-153';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-11-CD-19-153', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-11-CD-19-154';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-11-CD-19-154', '', 'Full name', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-11-CD-19-155';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-11-CD-19-155', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-11-CD-19-156';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-11-CD-19-156', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-11-CD-20-157';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-11-CD-20-157', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-11-CD-20-158';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-11-CD-20-158', '', 'Date of birth', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-11-CD-20-159';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-11-CD-20-159', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-11-CD-20-160';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-11-CD-20-160', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-11-CD-21-161';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-11-CD-21-161', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-11-CD-21-162';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-11-CD-21-162', '', 'How many shares does the director hold?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-11-CD-21-163';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-11-CD-21-163', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-11-CD-21-164';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-11-CD-21-164', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-12-165';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-12-165', '', 'Director data', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-12-166';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-12-166', '', '<ul class="govuk-list govuk-list--bullet"><li>Un-discharged bankruptcy</li><li>Composition with creditors</li><li>Any form of dispute</li></ul>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-12-167';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-12-167', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-12-168';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-12-168', '', 'Directors data', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-12-CD-22-169';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-12-CD-22-169', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-12-CD-22-170';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-12-CD-22-170', '', 'Has any director, or any other person with significant control of your organisation, had one or more of the following?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-12-CD-22-171';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-12-CD-22-171', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-12-CD-22-172';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-12-CD-22-172', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-13-173';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-13-173', '', 'Further detail of incident', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-13-174';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-13-174', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-13-175';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-13-175', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-13-176';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-13-176', '', 'Further detail of incident', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-13-CD-23-177';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-13-CD-23-177', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-13-CD-23-178';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-13-CD-23-178', '', 'Date of incident', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-13-CD-23-179';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-13-CD-23-179', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-13-CD-23-180';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-13-CD-23-180', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-13-CD-24-181';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-13-CD-24-181', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-13-CD-24-182';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-13-CD-24-182', '', 'Brief summary', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-13-CD-24-183';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-13-CD-24-183', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-13-CD-24-184';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-13-CD-24-184', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-13-CD-25-185';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-13-CD-25-185', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-13-CD-25-186';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-13-CD-25-186', '', 'Any outstanding court action or legal proceedings', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-13-CD-25-187';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-13-CD-25-187', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-13-CD-25-188';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-13-CD-25-188', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-14-189';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-14-189', '', 'Registered charity', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-14-190';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-14-190', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-14-191';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-14-191', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-14-192';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-14-192', '', 'Registered charity', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-14-CD-26-193';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-14-CD-26-193', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-14-CD-26-194';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-14-CD-26-194', '', 'Is your organisation a registered charity?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-14-CD-26-195';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-14-CD-26-195', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-14-CD-26-196';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-14-CD-26-196', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-14-CD-26.1-197';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-14-CD-26.1-197', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-14-CD-26.1-198';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-14-CD-26.1-198', '', 'What is the registered charity number?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-14-CD-26.1-199';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-14-CD-26.1-199', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-14-CD-26.1-200';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-14-CD-26.1-200', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-15-201';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-15-201', '', 'Register of removed trustees', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-15-202';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-15-202', '', '<p class="govuk-body">Has any director, or any other person with significant control of your organisation, been removed from the charities commission or appear on the register of removed trustees?</p>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-15-203';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-15-203', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-15-204';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-15-204', '', 'Register of removed trustees', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-15-CD-27-205';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-15-CD-27-205', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-15-CD-27-206';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-15-CD-27-206', '', 'Register of removed trustees', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-15-CD-27-207';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-15-CD-27-207', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-15-CD-27-208';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-15-CD-27-208', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-15-CD-27.1-209';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-15-CD-27.1-209', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-15-CD-27.1-210';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-15-CD-27.1-210', '', 'Full name', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-15-CD-27.1-211';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-15-CD-27.1-211', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-1-PG-15-CD-27.1-212';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-1-PG-15-CD-27.1-212', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-15-213';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-15-213', '', 'Authoriser details', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-15-214';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-15-214', '', '<p class="govuk-body">Who is signing your application?</p><p class="govuk-body">Include the name and job title of the person named as authoriser.</p>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-15-215';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-15-215', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-15-216';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-15-216', '', 'Invitation to apply', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-15-W_DEL-01-217';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-15-W_DEL-01-217', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-15-W_DEL-01-218';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-15-W_DEL-01-218', '', 'Name', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-15-W_DEL-01-219';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-15-W_DEL-01-219', '', 'Name', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-15-W_DEL-01-220';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-15-W_DEL-01-220', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-15-W_DEL-02-221';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-15-W_DEL-02-221', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-15-W_DEL-02-222';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-15-W_DEL-02-222', '', 'Job title', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-15-W_DEL-02-223';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-15-W_DEL-02-223', '', 'Job title', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-15-W_DEL-02-224';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-15-W_DEL-02-224', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-17-225';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-17-225', '', 'Terms and conditions', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-17-226';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-17-226', '', '<p class="govuk-body">If you make an application to be added to the register, you''re agreeing to the terms and conditions in the legal documentation.</p><p class="govuk-body">You must be able to truthfully answer ''yes'' to every question on this page for your application to be considered eligible.</p><p class="govuk-body">If you can''t answer ''yes'' to every question on this page, it''s very unlikely that your application will be accepted.</p>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-17-227';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-17-227', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-17-228';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-17-228', '', 'Terms and conditions', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-17-W_DEL-03-229';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-17-W_DEL-03-229', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-17-W_DEL-03-230';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-17-W_DEL-03-230', '', 'Do you agree to comply with the terms of the conditions for organisations on the register of end-point assessment organisations (link opens in a new tab) and sign and return the ''Conditions for organisations on the register of end-point assessment organisations''?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-17-W_DEL-03-231';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-17-W_DEL-03-231', '', 'Terms and conditions', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-17-W_DEL-03-232';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-17-W_DEL-03-232', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-18-233';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-18-233', '', 'Providing services straight away', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-18-234';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-18-234', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-18-235';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-18-235', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-18-236';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-18-236', '', 'Providing services straight away', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-18-W_DEL-04-237';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-18-W_DEL-04-237', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-18-W_DEL-04-238';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-18-W_DEL-04-238', '', 'If your application is successful, can you start an end-point assessment on the day you join the register?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-18-W_DEL-04-239';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-18-W_DEL-04-239', '', 'Providing services straight away', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-18-W_DEL-04-240';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-18-W_DEL-04-240', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-18-W_DEL-04.1-241';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-18-W_DEL-04.1-241', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-18-W_DEL-04.1-242';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-18-W_DEL-04.1-242', '', 'When will you be ready to do your first assessments?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-18-W_DEL-04.1-243';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-18-W_DEL-04.1-243', '', 'When will you be ready to do your first assessments?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-18-W_DEL-04.1-244';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-18-W_DEL-04.1-244', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-245';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-245', '', 'Grounds for Mandatory exclusion', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-246';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-246', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-247';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-247', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-248';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-248', '', 'Grounds for Mandatory exclusion', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-05-249';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-05-249', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-05-250';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-05-250', '', 'I understand and accept that:', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-05-251';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-05-251', '', 'Conditions for application', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-05-252';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-05-252', '', '<p class="govuk-body">Information cannot be amended after the application has been submitted and reviewed,the Education and Skills Funding Agency may at its sole discretion use information it already holds, obtains from other Government bodies or which is already in the public domain to validate part or all of any answer we have given in this submission and the information obtained may be shared with employers to assist them in their selection process,the Education and Skills Funding Agency may seek additional assurances from my organisation based on this information.</p>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-06-253';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-06-253', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-06-254';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-06-254', '', 'I confirm that the information to be uploaded in response to the Financial Health Assessment complies with the requirements stated in the ''Guidance for Applicants'' document.', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-06-255';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-06-255', '', 'Conditions for financial health assessment', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-06-256';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-06-256', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-07-257';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-07-257', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-07-258';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-07-258', '', 'Do you agree to inform the Education and Skills Funding Agency as soon as possible if there are any changes to the information you''re providing in this declaration?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-07-259';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-07-259', '', 'Changes of circumstance', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-07-260';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-07-260', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-08-261';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-08-261', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-08-262';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-08-262', '', 'I can confirm that we have read the conditions of acceptance and that we would be able to agree to these if our application is successful?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-08-263';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-08-263', '', 'Conditions of acceptance', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-08-264';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-08-264', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-09-265';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-09-265', '', 'Answer ''yes'' if anyone who represents, supervises or has control in your organisation or a partner or parent organisation has been convicted of any of the above.', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-09-266';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-09-266', '', 'Criminal convictions', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-09-267';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-09-267', '', 'Criminal convictions', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-09-268';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-09-268', '', '<p class="govuk-body">Confirm whether, within the past 5 years, anyone who represents, supervises or has control in your organisation or a partner or parent organisations has been convicted of:</p><ul class="govuk-list govuk-list--bullet"><li>any offence under sections 44 to 46 of the Serious Crime Act 2007 which relates to an offence covered by subparagraph (f)</li><li>conspiracy within the meaning of section 1 or 1A of the Criminal Law Act 1977</li><li>conspiracy within the meaning of article 9 or 9A of the Criminal Attempts and Conspiracy (Northern Ireland) Order 1983 where that conspiracy relates to participation in a criminal organisation as defined in Article 2 of Council Framework Decision 2008/842/JHA on the fight against organised crime</li></ul>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-10-269';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-10-269', '', 'Answer ''yes'' if anyone who represents, supervises or has control in your organisation or a partner or parent organisation has been convicted of any of the above.', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-10-270';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-10-270', '', 'Financial convictions', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-10-271';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-10-271', '', 'Financial convictions', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-10-272';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-10-272', '', '<p class="govuk-body">Confirm whether, within the past 5 years, anyone who represents, supervises or has control in your organisation or a parent or partner organisations has been convicted of any offence that relates to fraud affecting the European Communities'' financial interests as defined by article 1 of the Convention of the Protection of the Financial Interests of the European Communities, including:</p><ul class="govuk-list govuk-list--bullet"><li>the common law offence of cheating the Revenue (HMRC)</li><li>the common law offence of conspiracy to defraud</li><li>fraud or theft within the meaning of the Theft Act 1968, the Theft Act (Northern Ireland) 1969, the Theft Act 1978 or the Theft (Northern Ireland) Order 1978</li><li>fraudulent trading within the meaning of section 458 of the Companies Act 1985, article 451 of the Companies (Northern Ireland) Order 1986 or section 993 of the Companies Act 2006</li><li>fraudulent evasion within the meaning of section 170 of the Customs and Excise Management Act 1979 or section 72 of the Value Added Tax Act 1994</li><li>an offence in connection with taxation in the European Union within the meaning of section 71 of the Criminal Justice Act 1993</li><li>destroying, defacing or concealing of documents or procuring the execution of a valuable security within the meaning of section 20 of the Theft Act 1968 or section 19 of the Theft Act (Northern Ireland) 1969</li><li>fraud within the meaning of section 2, 3 or 4 of the Fraud Act 2006</li><li>the possession of articles for use in frauds within the meaning of section 6 of the Fraud Act 2006, or the making, adapting, supplying or offering to supply articles for use in frauds within the meaning of section 7 of that Act</li></ul>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-11-273';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-11-273', '', 'Answer ''yes'' if anyone who represents, supervises or has control in your organisation or a partner or parent organisation has been convicted of any of the above.', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-11-274';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-11-274', '', 'Acts of terrorism', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-11-275';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-11-275', '', 'Acts of terrorism', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-11-276';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-11-276', '', '<p class="govuk-body">Confirm whether, within the past 5 years, anyone who represents, supervises or has control in your organisation or a partner or parent organisation has been convicted of:</p><ul class="govuk-list govuk-list--bullet"><li>section 41 of the Counter Terrorism Act 2008</li><li>schedule 2 of the Counter Terrorism Act 2008 where the court has determined that there is a terrorist connection</li><li>any offence under sections 44 to 46 of the Serious Crime Act 2007</li></ul>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-12-277';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-12-277', '', 'Answer ''yes'' if anyone who represents, supervises or has control in your organisation or a partner or parent organisation has been convicted of any of the above.', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-12-278';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-12-278', '', 'Parent or partner organisations', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-12-279';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-12-279', '', 'Parent or partner organisations', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-19-M_DEL-12-280';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-19-M_DEL-12-280', '', '<p class="govuk-body">Confirm whether, within the past 5 years, anyone who represents, supervises or has control in your organisation or a partner or parent organisation has been convicted of:</p><ul class="govuk-list govuk-list--bullet"><li>money laundering within the meaning of sections 340(11) and 415 of the Proceeds of Crime Act 2002</li><li>an offence in connection with the proceeds of criminal conduct within the meaning of section 93A, 93B or 93C of the Criminal Justice Act 1988 or article 45, 46 or 47 of the Proceeds of Crime (Northern Ireland) Order 1996</li><li>an offence under section 4 of the Asylum and Immigration (Treatment of Claimants etc) Act 2004</li><li>an offence under section 59A of the Sexual Offences Act 2003</li><li>an offence under section 71 of the Coroners and Justice Act 2009</li><li>an offence in connection with the proceeds of drug trafficking within the meaning of section 49, 50 and 51 of the Drug Trafficking Act 1994</li></ul>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20-281';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20-281', '', 'Grounds for discretionary exclusion', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20-282';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20-282', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20-283';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20-283', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20-284';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20-284', '', 'Grounds for discretionary exclusion', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20-D_DEL-13-285';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20-D_DEL-13-285', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20-D_DEL-13-286';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20-D_DEL-13-286', '', 'Tax and social security irregularities', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20-D_DEL-13-287';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20-D_DEL-13-287', '', 'Cessation of trading', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20-D_DEL-13-288';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20-D_DEL-13-288', '', '<p class="govuk-body">Have members of your organisation or a partner organisation been legally found to be in breach of tax payments or social security contributions?</p><p class="govuk-body">If you do answer ''yes'', you must provide full details of any subsequent event or remedial action that you think the Education and Skills Funding Agency (ESFA) should take into consideration. ESFA will use the information you provide to consider whether or not you will be able to proceed any further with this application.</p><p class="govuk-body">''A partner organisation'' includes members of your group of economic operators or their proposed subcontractors.</p><p class="govuk-body">ESFA can also exclude you if you are guilty of serious misrepresentation in providing any information referred to within regulations 23, 24, 25, 26 or 27 of the ''Public Contracts Regulations 2015'' or if you fail to provide any such information it requests.</p>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20-D_DEL-13.1-289';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20-D_DEL-13.1-289', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20-D_DEL-13.1-290';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20-D_DEL-13.1-290', '', 'Provide details whether you''ve paid, or have entered into a binding arrangement with a view to paying, including, where applicable, any accrued interest and/or fines.', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20-D_DEL-13.1-291';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES

  (NEWID(), 'SQ-1-SE-2-PG-20-D_DEL-13.1-291', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20-D_DEL-13.1-292';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20-D_DEL-13.1-292', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20-D_DEL-14-293';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20-D_DEL-14-293', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20-D_DEL-14-294';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20-D_DEL-14-294', '', 'Bankruptcy and insolvency', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20-D_DEL-14-295';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20-D_DEL-14-295', '', 'Tax and social security irregularities', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20-D_DEL-14-296';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20-D_DEL-14-296', '', '<p class="govuk-body">Confirm whether, within the past 3 years, your organisation or any of your partner organisations:</p><ul class="govuk-list govuk-list--bullet"><li>has been made bankrupt or the subject of insolvency or winding-up proceedings</li></ul>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20.1-297';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20.1-297', '', 'Bankruptcy and insolvency details 1', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20.1-298';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20.1-298', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20.1-299';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20.1-299', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20.1-300';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20.1-300', '', 'Bankruptcy and insolvency details', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-301';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-301', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-302';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-302', '', 'Type of proceeding', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-303';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-303', '', 'Type of proceeding', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-304';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-304', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-305';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-305', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-306';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-306', '', 'Date of proceedings', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-307';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-307', '', 'Date of proceedings', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-308';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-308', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-309';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-309', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-310';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-310', '', 'If repaying debts, how you are repaying the debt?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-311';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-311', '', 'If repaying debts, how you are repaying the debt?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-312';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-312', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-313';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-313', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-314';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-314', '', 'Date the debt will be cleared', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-315';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-315', '', 'Date the debt will be cleared', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-316';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20.1-D_DEL-13-1-316', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-317';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-317', '', 'NO TITLE FOR THIS IN SPREADSHEET', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-318';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-318', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-319';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-319', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-320';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-320', '', 'NO TITLE FOR THIS IN SPREADSHEET', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-21-321';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-21-321', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-21-322';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-21-322', '', 'Cessation of trading', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-21-323';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-21-323', '', 'Cessation of trading', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-21-324';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-21-324', '', '<p class="govuk-body">Confirm whether your organisation or any of your partner organisations is in:</p><ul class="govuk-list govuk-list--bullet"><li>voluntary administration or company voluntary arrangement</li><li>compulsory winding up</li><li>receivership</li><li>composition with creditors</li></ul>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-21.1-325';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-21.1-325', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-21.1-326';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-21.1-326', '', 'Provide details of any mitigating factors that you think should be taken into consideration', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-21.1-327';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-21.1-327', '', 'Provide details of any mitigating factors that you think should be taken into consideration', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-21.1-328';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-21.1-328', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-22-329';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-22-329', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-22-330';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-22-330', '', 'Incorrect tax returns', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-22-331';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-22-331', '', 'Incorrect tax returns', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-22-332';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-22-332', '', '<p class="govuk-body">Confirm whether any of your organisation''s tax returns, submitted on or after 1 October 2012, have:</p><ul class="govuk-list govuk-list--bullet"><li>been found to be incorrect as of 1 April 2013</li><li>given rise to a criminal conviction for tax-related offences which is unspent, or to a civil penalty for fraud or evasion</li></ul>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-22.1-333';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-22.1-333', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-22.1-334';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-22.1-334', '', 'Provide details of any mitigating factors that you think should be taken into consideration', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-22.1-335';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-22.1-335', '', 'Provide details of any mitigating factors that you think should be taken into consideration', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-22.1-336';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-22.1-336', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-23-337';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-23-337', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-23-338';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-23-338', '', 'HMRC challenges to tax returns', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-23-339';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-23-339', '', 'HMRC challenges to tax returns', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-23-340';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-23-340', '', '<p class="govuk-body">Confirm whether any of your organisation''s tax returns, submitted on or after 1 October 2012, have been found to be incorrect on or after 1 April 2013 because:</p><ul class="govuk-list govuk-list--bullet"><li>HMRC successfully challenged it under the General Anti-Abuse Rule (GAAR) or Halifax abuse principle OR</li><li>a tax authority (in a jurisdiction in which your organisation is established) successfully challenging it under any tax rules or legislation that have an effect equivalent or similar to the GAAR or Halifax abuse principle</li><li>given rise to a criminal conviction for tax-related offences which is unspent, or to a civil penalty for fraud or evasion</li></ul>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-23.1-341';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-23.1-341', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-23.1-342';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-23.1-342', '', 'Provide details of any mitigating factors that you think should be taken into consideration', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-23.1-343';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-23.1-343', '', 'Provide details of any mitigating factors that you think should be taken into consideration', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-23.1-344';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-23.1-344', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-24-345';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-24-345', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-24-346';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-24-346', '', 'Contracts withdrawn from you', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-24-347';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-24-347', '', 'Contracts withdrawn from you', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-24-348';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-24-348', '', '<p class="govuk-body">Has your organisation had any contract for the delivery of services withdrawn within the last 3 financial years?</p><ul class="govuk-list govuk-list--bullet"><li>HMRC successfully challenged it under the General Anti-Abuse Rule (GAAR) or Halifax abuse principle OR</li><li>a tax authority (in a jurisdiction in which your organisation is established) successfully challenging it under any tax rules or legislation that have an effect equivalent or similar to the GAAR or Halifax abuse principle</li><li>given rise to a criminal conviction for tax-related offences which is unspent, or to a civil penalty for fraud or evasion</li></ul>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-24.1-349';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-24.1-349', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-24.1-350';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-24.1-350', '', 'Provide details of any mitigating factors that you think should be taken into consideration', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-24.1-351';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-24.1-351', '', 'Provide details of any mitigating factors that you think should be taken into consideration', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-24.1-352';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-24.1-352', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-25-353';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-25-353', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-25-354';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-25-354', '', 'Contracts you have withdrawn from', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-25-355';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-25-355', '', 'Contracts you have withdrawn from', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-25-356';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-25-356', '', '<p class="govuk-body">Has your organisation withdrawn from a contract for the delivery of services within the last 3 years?</p><ul class="govuk-list govuk-list--bullet"><li>HMRC successfully challenged it under the General Anti-Abuse Rule (GAAR) or Halifax abuse principle OR</li><li>a tax authority (in a jurisdiction in which your organisation is established) successfully challenging it under any tax rules or legislation that have an effect equivalent or similar to the GAAR or Halifax abuse principle</li><li>given rise to a criminal conviction for tax-related offences which is unspent, or to a civil penalty for fraud or evasion</li></ul>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-25.1-357';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-25.1-357', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-25.1-358';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-25.1-358', '', 'Provide details of any mitigating factors that you think should be taken into consideration', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-25.1-359';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-25.1-359', '', 'Provide details of any mitigating factors that you think should be taken into consideration', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-25.1-360';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-25.1-360', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-25.2-361';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-25.2-361', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-25.2-362';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-25.2-362', '', 'Full name', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-25.2-363';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-25.2-363', '', 'Full name', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-25.2-364';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-25.2-364', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-25.3-365';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-25.3-365', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-25.3-366';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-25.3-366', '', 'Dates involved', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-25.3-367';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-25.3-367', '', 'Dates involved', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-25.3-368';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-25.3-368', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-26-369';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-26-369', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-26-370';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-26-370', '', 'Organisation removed from registers', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-26-371';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-26-371', '', 'Organisation removed from registers', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-26-372';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-26-372', '', '<p class="govuk-body">Has your organisation been removed from any of the following registers?</p><ul class="govuk-list govuk-list--bullet"><li>Education and skills funding agency''s register of training organisations RoATP</li><li>EPAO register</li><li>Ofqual''s register</li><li>Other professional or trade registers</li></ul>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-26.1-373';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-26.1-373', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-26.1-374';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-26.1-374', '', 'Date of removal', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-26.1-375';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-26.1-375', '', 'Date of removal', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-26.1-376';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-26.1-376', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-26.2-377';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-26.2-377', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-26.2-378';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-26.2-378', '', 'Reasons why', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-26.2-379';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-26.2-379', '', 'Reasons why', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-26.2-380';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-26.2-380', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-27-381';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-27-381', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-27-382';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-27-382', '', 'Directions and sanctions', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-27-383';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-27-383', '', 'Directions and sanctions', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-27-384';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-27-384', '', '<p class="govuk-body">Has your organisation received direction or sanctions from any of the following?</p><ul class="govuk-list govuk-list--bullet"><li>Ofqual</li><li>The QAA</li><li>Awarding organisations</li><li>Other similar bodies</li></ul>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-27.1-385';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-27.1-385', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-27.1-386';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-27.1-386', '', 'Date of sanction', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-27.1-387';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-27.1-387', '', 'Date of sanction', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-27.1-388';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-27.1-388', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-27.2-389';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-27.2-389', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-27.2-390';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-27.2-390', '', 'Reasons why', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-27.2-391';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-27.2-391', '', 'Reasons why', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-27.2-392';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-27.2-392', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-28-393';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-28-393', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-28-394';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-28-394', '', 'Has your organisation ever had to repay public money?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-28-395';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-28-395', '', 'Repayment of public money', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-28-396';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-28-396', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-28.1-397';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-28.1-397', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-28.1-398';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-28.1-398', '', 'Provide details of any mitigating factors that you think should be taken into consideration', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-28.1-399';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-28.1-399', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-28.1-400';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-28.1-400', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-29-401';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-29-401', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-29-402';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-29-402', '', 'Public body funds and contracts', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-29-403';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-29-403', '', 'Public body funds and contracts', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-29-404';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-29-404', '', '<p class="govuk-body">Have any directors, shareholders, senior employees or someone that has powers of representation, decision or control of your organisation had any of the following?</p><ul class="govuk-list govuk-list--bullet"><li>Failure to repay funding due to any public body</li><li>Early termination of a contract with a public body</li><li>Early withdrawal from a contract with a public body</li></ul>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-29.1-405';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-29.1-405', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-29.1-406';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-29.1-406', '', 'Provide details of any mitigating factors that you think should be taken into consideration', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-29.1-407';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-29.1-407', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-29.1-408';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-29.1-408', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-30-409';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-30-409', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-30-410';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-30-410', '', 'Does your organisation have any outstanding or ongoing legal dispute that could prevent you from conducting end-point assessments?', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-30-411';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-30-411', '', 'Legal dispute', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-30-412';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-30-412', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-30.1-413';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-30.1-413', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-30.1-414';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-30.1-414', '', 'Date', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-30.1-415';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-30.1-415', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-30.1-416';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-30.1-416', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-30.2-417';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-30.2-417', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-30.2-418';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-30.2-418', '', 'Details of the dispute', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-30.2-419';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-30.2-419', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-30.2-420';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-30.2-420', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-30.3-421';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-30.3-421', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-30.3-422';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-30.3-422', '', 'Current status of the dispute', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-30.3-423';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-30.3-423', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-21-A_DEL-30.3-424';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-21-A_DEL-30.3-424', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-22-425';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-22-425', '', 'Application accuracy', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-22-426';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-22-426', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-22-427';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-22-427', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-22-428';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-22-428', '', 'Application accuracy', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-22-A_DEL-28-429';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-22-A_DEL-28-429', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-22-A_DEL-28-430';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-22-A_DEL-28-430', '', 'False declarations', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-22-A_DEL-28-431';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-22-A_DEL-28-431', '', 'False declarations', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-22-A_DEL-28-432';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-22-A_DEL-28-432', '', '<p class="govuk-body">I certify that the information provided is accurate and accept the conditions and undertakings requested in this application. It''s understood that false information may result in:</p><ul class="govuk-list govuk-list--bullet"><li>exclusion from this and future registers</li><li>the removal from the Register of End-point Assessments Organisations</li><li>the withdrawal of contracts with employers</li><li>civil or criminal proceedings</li></ul>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-22-A_DEL-29-433';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-22-A_DEL-29-433', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-22-A_DEL-29-434';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-22-A_DEL-29-434', '', 'Accurate and true representation', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-22-A_DEL-29-435';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-22-A_DEL-29-435', '', 'Accurate and true representation', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-22-A_DEL-29-436';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-22-A_DEL-29-436', '', '<p class="govuk-body">Will your applications to deliver end-point assessments for standards be accurate and true representations?</p>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-22-A_DEL-30-437';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-22-A_DEL-30-437', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-22-A_DEL-30-438';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-22-A_DEL-30-438', '', 'Agreement to appear on the register', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-22-A_DEL-30-439';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-22-A_DEL-30-439', '', 'Agreement to appear on the register', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-22-A_DEL-30-440';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-22-A_DEL-30-440', '', '<p class="govuk-body">Do you agree your organisation details will be added to the register of end-point assessment organisations if your application is successful?</p>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-3-PG-23-441';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-3-PG-23-441', '', 'Financial health assessment', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-3-PG-23-442';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-3-PG-23-442', '', '<p class="govuk-body">You will fail the financial health assessment process if you do not upload financial statements where available. Management accounts must only be submitted where financial statements/accounts are yet to be produced.</p><p class="govuk-body">For further information on what you must include if you select ''a'', ''b'' or ''c'' please check <a class="govuk-link" href="https://www.gov.uk/government/publications/esfa-financial-health-assessment/">https://www.gov.uk/government/publications/esfa-financial-health-assessment</a>.</p>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-3-PG-23-443';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-3-PG-23-443', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-3-PG-23-444';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-3-PG-23-444', '', 'Financial health assessment', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-3-PG-23-FHA-01-445';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-3-PG-23-FHA-01-445', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-3-PG-23-FHA-01-446';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-3-PG-23-FHA-01-446', '', 'Upload your financial evidences', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-3-PG-23-FHA-01-447';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-3-PG-23-FHA-01-447', '', 'Upload your financial evidences', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-3-PG-23-FHA-01-448';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-3-PG-23-FHA-01-448', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-3-PG-23-FHA-02-449';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-3-PG-23-FHA-02-449', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-3-PG-23-FHA-02-450';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-3-PG-23-FHA-02-450', '', 'Attach the latest available accounts for the UK ultimate parent company.', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-3-PG-23-FHA-02-451';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-3-PG-23-FHA-02-451', '', 'Attach the latest available accounts for the UK ultimate parent company', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-1-SE-3-PG-23-FHA-02-452';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-3-PG-23-FHA-02-452', '', '<p class="govuk-body">Organisations that are subject to the financial health assessment must submit their UK parent company accounts or they will fail the process.</p>', 'Live', GETUTCDATE(), 'Import')

GO

DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-15-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-15-LT-1', '', 'Name and job title', 'Live', GETUTCDATE(), 'Import')
GO

DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-18-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-18-LT-1', '', 'Providing services straight away', 'Live', GETUTCDATE(), 'Import')
GO

DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-193-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-193-LT-1', '', 'Criminal convictions', 'Live', GETUTCDATE(), 'Import')
GO

DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-194-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-194-LT-1', '', 'Financial convictions', 'Live', GETUTCDATE(), 'Import')
GO


DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-195-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-195-LT-1', '', 'Acts of terrorism', 'Live', GETUTCDATE(), 'Import')
GO

DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-196-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-196-LT-1', '', 'Parent or partner organisations', 'Live', GETUTCDATE(), 'Import')
GO

DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20-LT-1', '', 'Tax and social security irregularities', 'Live', GETUTCDATE(), 'Import')
GO

DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-201-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-201-LT-1', '', 'Bankruptcy and insolvency', 'Live', GETUTCDATE(), 'Import')
GO


DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-20.1-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-20.1-LT-1', '', 'Bankruptcy and insolvency details', 'Live', GETUTCDATE(), 'Import')
GO


DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-202-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-202-LT-1', '', 'Cessation of trading', 'Live', GETUTCDATE(), 'Import')
GO

DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-203-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-203-LT-1', '', 'Incorrect tax returns', 'Live', GETUTCDATE(), 'Import')
GO

DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-204-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-204-LT-1', '', 'HMRC challenges to tax returns', 'Live', GETUTCDATE(), 'Import')
GO

DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-205-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-205-LT-1', '', 'Contracts withdrawn from you', 'Live', GETUTCDATE(), 'Import')
GO

DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-206-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-206-LT-1', '', 'Contracts you have withdrawn from', 'Live', GETUTCDATE(), 'Import')
GO

DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-207-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-207-LT-1', '', 'Organisation removed from registers', 'Live', GETUTCDATE(), 'Import')
GO


DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-208-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-208-LT-1', '', 'Directions and sanctions', 'Live', GETUTCDATE(), 'Import')
GO


DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-209-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-209-LT-1', '', 'Repayment of public money', 'Live', GETUTCDATE(), 'Import')
GO

DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-210-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-210-LT-1', '', 'Public body funds and contracts', 'Live', GETUTCDATE(), 'Import')
GO

DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-211-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-211-LT-1', '', 'Legal dispute', 'Live', GETUTCDATE(), 'Import')
GO

DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-22-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-22-LT-1', '', 'False declarations', 'Live', GETUTCDATE(), 'Import')
GO

DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-221-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-221-LT-1', '', 'Accurate and true representation', 'Live', GETUTCDATE(), 'Import')
GO

DELETE FROM Assets WHERE Reference = 'SQ-1-SE-2-PG-222-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-1-SE-2-PG-222-LT-1', '', 'Agreement to appear on the register', 'Live', GETUTCDATE(), 'Import')
GO


DELETE FROM workflows where Description='Epao Workflow' and Version='1.0'

GO

if not exists(select * from workflows where Description='Epao Workflow' and Version='1.0')
	INSERT [dbo].[Workflows]
	  ([Id], [Description], [Version], [Type], [Status], [CreatedAt], [CreatedBy], [UpdatedAt], [UpdatedBy], [DeletedAt], [DeletedBy], [ReferenceFormat])
	VALUES
	  (N'83b35024-8aef-440d-8f59-8c1cc459c350', N'EPAO Workflow', N'1.0', N'EPAO', N'Live', CAST(N'2018-12-12T14:41:46.0600000' AS DateTime2), N'Import', NULL, NULL, NULL, NULL,N'AAD')
else
begin
	UPDATE [dbo].[Workflows] SET  [Description] = N'EPAO Workflow', [Version] =  N'1.0', [Type] = N'EPAO', 
	[Status] = N'Live', [CreatedAt] = CAST(N'2018-12-12T14:41:46.0600000' AS DateTime2), 
	[CreatedBy] = N'Import', [UpdatedBy] = N'Import', [UpdatedAt] = GETDATE(), [ReferenceFormat] = N'AAD' where [Id] = '83b35024-8aef-440d-8f59-8c1cc459c350'
end
GO
DELETE FROM WorkflowSections where SequenceId=1 and SectionId=1

INSERT [dbo].[WorkflowSections]
  ([Id], [WorkflowId], [SequenceId], [SectionId], [QnAData], [Title], [LinkTitle], [Status], [DisplayType], [DisallowedOrgTypes])
VALUES
  (N'b9c09252-3fee-455f-bc54-12c8788398b7', N'83b35024-8aef-440d-8f59-8c1cc459c350', 1, 1, N'
{
  "Pages": [
    {
      "PageId": "1",
      "SequenceId": "1",
      "SectionId": "1",
      "Title": "SQ-1-SE-1-PG-1-1",
      "LinkTitle": "SQ-1-SE-1-PG-1-4",
      "InfoText": "SQ-1-SE-1-PG-1-3",
      "Questions": [
        {
          "QuestionId": "CD-30",
          "Label": "SQ-1-SE-1-PG-1-CD-30-6",
          "ShortLabel": "SQ-1-SE-1-PG-1-CD-30-7",
          "QuestionBodyText": "SQ-1-SE-1-PG-1-CD-30-8",
          "Hint": "SQ-1-SE-1-PG-1-CD-30-5",
          "Input": {
            "Type": "ComplexRadio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": [
                  {
                    "QuestionId": "CD-30.1",
                    "Label": "SQ-1-SE-1-PG-2-CD-01.1-18",
                    "ShortLabel": "SQ-1-SE-1-PG-2-CD-01.1-19",
                    "ShortLabelRef": "1-1-2-CD-01.1-shortlabel",
                    "LabelRef": "1-1-2-CD-01.1-label",
                    "Hint": "SQ-1-SE-1-PG-2-CD-01.1-17",
                    "HintRef": "1-1-2-CD-01.1-hint",
                    "QuestionBodyText": "SQ-1-SE-1-PG-2-CD-01.1-20",
                    "QuestionBodyTextRef": "1-1-2-CD-01.1-questionbody",
                    "Input": {
                      "Type": "text",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter a trading name"
                        }
                      ]
                    },
                    "Order": null
                  }
                ]
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select yes if your organisation has a trading name"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "NextPage",
          "ReturnId": "2",
          "Condition": {
            "QuestionId": "CD-30",
            "MustEqual": "Yes"
          },
          "ConditionMet": false
        },
        {
          "Action": "NextPage",
          "ReturnId": "3",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-1-PG-1-2"
    },
    {
      "PageId": "2",
      "SequenceId": "1",
      "SectionId": "1",
      "Title": "SQ-1-SE-1-PG-2-9",
      "LinkTitle": "SQ-1-SE-1-PG-2-12",
      "InfoText": "SQ-1-SE-1-PG-2-11",
      "Questions": [
        {
          "QuestionId": "CD-01",
          "Label": "SQ-1-SE-1-PG-2-CD-01-14",
          "ShortLabel": "SQ-1-SE-1-PG-2-CD-01-15",
          "QuestionBodyText": "SQ-1-SE-1-PG-2-CD-01-16",
          "Hint": "SQ-1-SE-1-PG-2-CD-01-13",
          "Input": {
            "Type": "Radio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": null
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select yes if you want to use your trading name on the register"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "NextPage",
          "ReturnId": "3",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": false,
      "Visible": false,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-1-PG-2-10"
    },
    {
      "PageId": "3",
      "SequenceId": "1",
      "SectionId": "1",
      "Title": "SQ-1-SE-1-PG-3-21",
      "LinkTitle": "SQ-1-SE-1-PG-3-24",
      "InfoText": "SQ-1-SE-1-PG-3-23",
      "Questions": [
        {
          "QuestionId": "CD-02",
          "Label": "SQ-1-SE-1-PG-3-CD-02-26",
          "ShortLabel": "SQ-1-SE-1-PG-3-CD-02-27",
          "QuestionBodyText": "SQ-1-SE-1-PG-3-CD-02-28",
          "Hint": "SQ-1-SE-1-PG-3-CD-02-25",
          "Input": {
            "Type": "text",
            "Options": null,
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Enter name"
              }
            ]
          },
          "Order": null
        },
        {
          "QuestionId": "CD-03",
          "Label": "SQ-1-SE-1-PG-3-CD-03-30",
          "ShortLabel": "SQ-1-SE-1-PG-3-CD-03-31",
          "QuestionBodyText": "SQ-1-SE-1-PG-3-CD-03-32",
          "Hint": "SQ-1-SE-1-PG-3-CD-03-29",
          "Input": {
            "Type": "text",
            "Options": null,
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Enter address"
              }
            ]
          },
          "Order": null
        },
        {
          "QuestionId": "CD-04",
          "Label": "SQ-1-SE-1-PG-3-CD-04-34",
          "ShortLabel": "SQ-1-SE-1-PG-3-CD-04-35",
          "QuestionBodyText": "SQ-1-SE-1-PG-3-CD-04-36",
          "Hint": "SQ-1-SE-1-PG-3-CD-04-33",
          "Input": {
            "Type": "Postcode",
            "Options": null,
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Enter postcode"
              }
            ]
          },
          "Order": null
        },
        {
          "QuestionId": "CD-05",
          "Label": "SQ-1-SE-1-PG-3-CD-05-38",
          "ShortLabel": "SQ-1-SE-1-PG-3-CD-05-39",
          "QuestionBodyText": "SQ-1-SE-1-PG-3-CD-05-40",
          "Hint": "SQ-1-SE-1-PG-3-CD-05-37",
          "Input": {
            "Type": "Email",
            "Options": null,
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Enter email address"
              }
            ]
          },
          "Order": null
        },
        {
          "QuestionId": "CD-06",
          "Label": "SQ-1-SE-1-PG-3-CD-06-42",
          "ShortLabel": "SQ-1-SE-1-PG-3-CD-06-43",
          "QuestionBodyText": "SQ-1-SE-1-PG-3-CD-06-44",
          "Hint": "SQ-1-SE-1-PG-3-CD-06-41",
          "Input": {
            "Type": "text",
            "Options": null,
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Enter telephone number"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "NextPage",
          "ReturnId": "4",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-1-PG-3-22"
    },
    {
      "PageId": "4",
      "SequenceId": "1",
      "SectionId": "1",
      "Title": "SQ-1-SE-1-PG-4-45",
      "LinkTitle": "SQ-1-SE-1-PG-4-48",
      "InfoText": "SQ-1-SE-1-PG-4-47",
      "Questions": [
        {
          "QuestionId": "CD-07",
          "Label": "SQ-1-SE-1-PG-4-CD-07-50",
          "ShortLabel": "SQ-1-SE-1-PG-4-CD-07-51",
          "QuestionBodyText": "SQ-1-SE-1-PG-4-CD-07-52",
          "Hint": "SQ-1-SE-1-PG-4-CD-07-49",
          "Input": {
            "Type": "text",
            "Options": null,
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Enter name"
              }
            ]
          },
          "Order": null
        },
        {
          "QuestionId": "CD-08",
          "Label": "SQ-1-SE-1-PG-4-CD-08-54",
          "ShortLabel": "SQ-1-SE-1-PG-4-CD-08-55",
          "QuestionBodyText": "SQ-1-SE-1-PG-4-CD-08-56",
          "Hint": "SQ-1-SE-1-PG-4-CD-08-53",
          "Input": {
            "Type": "text",
            "Options": null,
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Enter address"
              }
            ]
          },
          "Order": null
        },
        {
          "QuestionId": "CD-09",
          "Label": "SQ-1-SE-1-PG-4-CD-09-58",
          "ShortLabel": "SQ-1-SE-1-PG-4-CD-09-59",
          "QuestionBodyText": "SQ-1-SE-1-PG-4-CD-09-60",
          "Hint": "SQ-1-SE-1-PG-4-CD-09-57",
          "Input": {
            "Type": "Postcode",
            "Options": null,
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Enter postcode"
              }
            ]
          },
          "Order": null
        },
        {
          "QuestionId": "CD-10",
          "Label": "SQ-1-SE-1-PG-4-CD-10-62",
          "ShortLabel": "SQ-1-SE-1-PG-4-CD-10-63",
          "QuestionBodyText": "SQ-1-SE-1-PG-4-CD-10-64",
          "Hint": "SQ-1-SE-1-PG-4-CD-10-61",
          "Input": {
            "Type": "Email",
            "Options": null,
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Enter email address"
              }
            ]
          },
          "Order": null
        },
        {
          "QuestionId": "CD-11",
          "Label": "SQ-1-SE-1-PG-4-CD-11-66",
          "ShortLabel": "SQ-1-SE-1-PG-4-CD-11-67",
          "QuestionBodyText": "SQ-1-SE-1-PG-4-CD-11-68",
          "Hint": "SQ-1-SE-1-PG-4-CD-11-65",
          "Input": {
            "Type": "text",
            "Options": null,
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Enter telephone number"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "NextPage",
          "ReturnId": "5",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-1-PG-4-46"
    },
    {
      "PageId": "5",
      "SequenceId": "1",
      "SectionId": "1",
      "Title": "SQ-1-SE-1-PG-5-69",
      "LinkTitle": "SQ-1-SE-1-PG-5-72",
      "InfoText": "SQ-1-SE-1-PG-5-71",
      "Questions": [
        {
          "QuestionId": "CD-12",
          "Label": "SQ-1-SE-1-PG-5-CD-12-74",
          "ShortLabel": "SQ-1-SE-1-PG-5-CD-12-75",
          "QuestionBodyText": "SQ-1-SE-1-PG-5-CD-12-76",
          "Hint": "SQ-1-SE-1-PG-5-CD-12-73",
          "Input": {
            "Type": "ComplexRadio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": [
                  {
                    "QuestionId": "CD-12.1",
                    "Label": "SQ-1-SE-1-PG-5-CD-12.1-78",
                    "Hint": "SQ-1-SE-1-PG-5-CD-12.1-77",
                    "Input": {
                      "Type": "number",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter your UKPRN"
                        }
                      ]
                    },
                    "Order": null,
                    "ShortLabel": "SQ-1-SE-1-PG-5-CD-12.1-79",
                    "QuestionBodyText": "SQ-1-SE-1-PG-5-CD-12.1-80"
                  }
                ]
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select yes if you have a UKPRN"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "NextPage",
          "ReturnId": "6",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-1-PG-5-70"
    },
    {
      "PageId": "6",
      "SequenceId": "1",
      "SectionId": "1",
      "Title": "SQ-1-SE-1-PG-6-81",
      "LinkTitle": "SQ-1-SE-1-PG-6-84",
      "InfoText": "SQ-1-SE-1-PG-6-83",
      "Questions": [
        {
          "QuestionId": "CD-13",
          "Label": "SQ-1-SE-1-PG-6-CD-13-86",
          "ShortLabel": "SQ-1-SE-1-PG-6-CD-13-87",
          "QuestionBodyText": "SQ-1-SE-1-PG-6-CD-13-88",
          "Hint": "SQ-1-SE-1-PG-6-CD-13-85",
          "Input": {
            "Type": "text",
            "Options": null,
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Enter name of person who has overall control"
              }
            ]
          },
          "Order": null
        },
        {
          "QuestionId": "CD-14",
          "Label": "SQ-1-SE-1-PG-6-CD-14-90",
          "ShortLabel": "SQ-1-SE-1-PG-6-CD-14-91",
          "QuestionBodyText": "SQ-1-SE-1-PG-6-CD-14-92",
          "Hint": "SQ-1-SE-1-PG-6-CD-14-89",
          "Input": {
            "Type": "ComplexRadio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": [
                  {
                    "QuestionId": "CD-14.1",
                    "Label": "SQ-1-SE-1-PG-6-CD-14.1-94",
                    "Hint": "SQ-1-SE-1-PG-6-CD-14.1-93",
                    "Input": {
                      "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter other organisation details"
                        }
                      ]
                    },
                    "Order": null,
                    "ShortLabel": "SQ-1-SE-1-PG-6-CD-14.1-95",
                    "QuestionBodyText": "SQ-1-SE-1-PG-6-CD-14.1-96"
                  }
                ]
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select yes if they hold any other positions or directorships of other organisations"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "NextPage",
          "ReturnId": "7",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-1-PG-6-82"
    },
    {
      "PageId": "7",
      "SequenceId": "1",
      "SectionId": "1",
      "Title": "SQ-1-SE-1-PG-7-97",
      "LinkTitle": "SQ-1-SE-1-PG-7-100",
      "InfoText": "SQ-1-SE-1-PG-7-99",
      "Questions": [
        {
          "QuestionId": "CD-15",
          "Label": "SQ-1-SE-1-PG-7-CD-15-102",
          "ShortLabel": "SQ-1-SE-1-PG-7-CD-15-103",
          "QuestionBodyText": "SQ-1-SE-1-PG-7-CD-15-104",
          "Hint": "SQ-1-SE-1-PG-7-CD-15-101",
          "Input": {
            "Type": "ComplexRadio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": [
                  {
                    "QuestionId": "CD-15.1",
                    "Label": "SQ-1-SE-1-PG-7-CD-15.1-106",
                    "Hint": "SQ-1-SE-1-PG-7-CD-15.1-105",
                    "Input": {
                      "Type": "text",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter Ofqual recognition number"
                        }
                      ]
                    },
                    "Order": null,
                    "ShortLabel": "SQ-1-SE-1-PG-7-CD-15.1-107",
                    "QuestionBodyText": "SQ-1-SE-1-PG-7-CD-15.1-108"
                  }
                ]
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select yes if you have an Ofqual recognition number"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "NextPage",
          "ReturnId": "8",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-1-PG-7-98"
    },
    {
      "PageId": "8",
      "SequenceId": "1",
      "SectionId": "1",
      "Title": "SQ-1-SE-1-PG-8-109",
      "LinkTitle": "SQ-1-SE-1-PG-8-112",
      "InfoText": "SQ-1-SE-1-PG-8-111",
      "Questions": [
        {
          "QuestionId": "CD-16",
          "Label": "SQ-1-SE-1-PG-8-CD-16-114",
          "ShortLabel": "SQ-1-SE-1-PG-8-CD-16-115",
          "QuestionBodyText": "SQ-1-SE-1-PG-8-CD-16-116",
          "Hint": "SQ-1-SE-1-PG-8-CD-16-113",
          "Input": {
            "Type": "ComplexRadio",
            "Options": [
              {
                "Label": "Public limited company",
                "Value": "Public limited company",
                "FurtherQuestions": null
              },
              {
                "Label": "Limited company",
                "Value": "Limited company",
                "FurtherQuestions": null
              },
              {
                "Label": "Limited liability partnership",
                "Value": "Limited liability partnership",
                "FurtherQuestions": null
              },
              {
                "Label": "Other partnership",
                "Value": "Other partnership",
                "FurtherQuestions": null
              },
              {
                "Label": "Sole trader",
                "Value": "Sole trader",
                "FurtherQuestions": null
              },
              {
                "Label": "Other",
                "Value": "Other",
                "FurtherQuestions": [
                  {
                    "QuestionId": "CD-16.1",
                    "Label": "SQ-1-SE-1-PG-8-CD-16.1-118",
                    "Hint": "SQ-1-SE-1-PG-8-CD-16.1-117",
                    "Input": {
                      "Type": "text",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter your trading status"
                        }
                      ]
                    },
                    "Order": null,
                    "ShortLabel": "SQ-1-SE-1-PG-8-CD-16.1-119",
                    "QuestionBodyText": "SQ-1-SE-1-PG-8-CD-16.1-120"
                  }
                ]
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select your trading status"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "NextPage",
          "ReturnId": "9",
          "Condition": {
            "QuestionId": "CD-16",
            "MustEqual": "Public limited company"
          },
          "ConditionMet": false
        },
        {
          "Action": "NextPage",
          "ReturnId": "9",
          "Condition": {
            "QuestionId": "CD-16",
            "MustEqual": "Limited company"
          },
          "ConditionMet": false
        },
        {
          "Action": "NextPage",
          "ReturnId": "9",
          "Condition": {
            "QuestionId": "CD-16",
            "MustEqual": "Limited liability partnership"
          },
          "ConditionMet": false
        },
        {
          "Action": "NextPage",
          "ReturnId": "11",
          "Condition": {
            "QuestionId": "CD-16",
            "MustEqual": "Other partnership"
          },
          "ConditionMet": false
        },
        {
          "Action": "NextPage",
          "ReturnId": "11",
          "Condition": {
            "QuestionId": "CD-16",
            "MustEqual": "Sole trader"
          },
          "ConditionMet": false
        },
        {
          "Action": "NextPage",
          "ReturnId": "11",
          "Condition": {
            "QuestionId": "CD-16",
            "MustEqual": "Other"
          },
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-1-PG-8-110"
    },
    {
      "PageId": "9",
      "SequenceId": "1",
      "SectionId": "1",
      "Title": "SQ-1-SE-1-PG-9-121",
      "LinkTitle": "SQ-1-SE-1-PG-9-124",
      "InfoText": "SQ-1-SE-1-PG-9-123",
      "Questions": [
        {
          "QuestionId": "CD-17",
          "Label": "SQ-1-SE-1-PG-9-CD-17-126",
          "ShortLabel": "SQ-1-SE-1-PG-9-CD-17-127",
          "QuestionBodyText": "SQ-1-SE-1-PG-9-CD-17-128",
          "Hint": "SQ-1-SE-1-PG-9-CD-17-125",
          "Input": {
            "Type": "ComplexRadio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": [
                  {
                    "QuestionId": "CD-17.1",
                    "Label": "SQ-1-SE-1-PG-9-CD-17.1-130",
                    "Hint": "SQ-1-SE-1-PG-9-CD-17.1-129",
                    "Input": {
                      "Type": "text",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter your company number"
                        } 
                      ]
                    },
                    "Order": null,
                    "ShortLabel": "SQ-1-SE-1-PG-9-CD-17.1-131",
                    "QuestionBodyText": "SQ-1-SE-1-PG-9-CD-17.1-132"
                  }
                ]
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select yes if you have a company number"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "NextPage",
          "ReturnId": "10",
          "Condition": {
            "QuestionId": "CD-17",
            "MustEqual": "No"
          },
          "ConditionMet": false
        },
        {
          "Action": "NextPage",
          "ReturnId": "11",
          "Condition": {
            "QuestionId": "CD-17",
            "MustEqual": "Yes"
          },
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-1-PG-9-122"
    },
    {
      "PageId": "10",
      "SequenceId": "1",
      "SectionId": "1",
      "Title": "SQ-1-SE-1-PG-10-133",
      "LinkTitle": "SQ-1-SE-1-PG-10-136",
      "InfoText": "SQ-1-SE-1-PG-10-135",
      "Questions": [
        {
          "QuestionId": "CD-18",
          "Label": "SQ-1-SE-1-PG-10-CD-18-138",
          "ShortLabel": "SQ-1-SE-1-PG-10-CD-18-139",
          "QuestionBodyText": "SQ-1-SE-1-PG-10-CD-18-140",
          "Hint": "SQ-1-SE-1-PG-10-CD-18-137",
          "Input": {
            "Type": "ComplexRadio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": [
                  {
                    "QuestionId": "CD-18.1",
                    "Label": "SQ-1-SE-1-PG-10-CD-18.1-142",
                    "Hint": "SQ-1-SE-1-PG-10-CD-18.1-141",
                    "Input": {
                      "Type": "text",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter the country your parent company is registered"
                        } 
                      ]
                    },
                    "Order": null,
                    "ShortLabel": "SQ-1-SE-1-PG-10-CD-18.1-143",
                    "QuestionBodyText": "SQ-1-SE-1-PG-10-CD-18.1-144"
                  },
                  {
                    "QuestionId": "CD-18.2",
                    "Label": "SQ-1-SE-1-PG-10-CD-18.2-146",
                    "Hint": "SQ-1-SE-1-PG-10-CD-18.2-145",
                    "Input": {
                      "Type": "text",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter the parent company registration number"
                        } 
                      ]
                    },
                    "Order": null,
                    "ShortLabel": "SQ-1-SE-1-PG-10-CD-18.2-147",
                    "QuestionBodyText": "SQ-1-SE-1-PG-10-CD-18.2-148"
                  }
                ]
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select yes if your parent company registered overseas"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "NextPage",
          "ReturnId": "11",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-1-PG-10-134"
    },
    {
      "PageId": "11",
      "SequenceId": "1",
      "SectionId": "1",
      "Title": "SQ-1-SE-1-PG-11-149",
      "LinkTitle": "SQ-1-SE-1-PG-11-152",
      "InfoText": "SQ-1-SE-1-PG-11-151",
      "Questions": [
        {
          "QuestionId": "CD-19",
          "Label": "SQ-1-SE-1-PG-11-CD-19-154",
          "ShortLabel": "SQ-1-SE-1-PG-11-CD-19-155",
          "QuestionBodyText": "SQ-1-SE-1-PG-11-CD-19-156",
          "Hint": "SQ-1-SE-1-PG-11-CD-19-153",
          "Input": {
            "Type": "Text",
            "Options": null,
            "Validations": [{
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Enter a name"
              }]
          },
          "Order": null
        },
        {
          "QuestionId": "CD-20",
          "Label": "SQ-1-SE-1-PG-11-CD-20-158",
          "ShortLabel": "SQ-1-SE-1-PG-11-CD-20-159",
          "QuestionBodyText": "SQ-1-SE-1-PG-11-CD-20-160",
          "Hint": "SQ-1-SE-1-PG-11-CD-20-157",
          "Input": {
            "Type": "Date",
            "Options": null,
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Enter a date"
              },
              {
                "Name":"Date",
                "Value":"",
                "ErrorMessage": "Date must be correct"
              }
            ]
          },
          "Order": null
        },
        {
          "QuestionId": "CD-21",
          "Label": "SQ-1-SE-1-PG-11-CD-21-162",
          "ShortLabel": "SQ-1-SE-1-PG-11-CD-21-163",
          "QuestionBodyText": "SQ-1-SE-1-PG-11-CD-21-164",
          "Hint": "SQ-1-SE-1-PG-11-CD-21-161",
          "Input": {
            "Type": "text",
            "Options": null,
            "Validations": [{
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Enter number of shares"
              }]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "NextPage",
          "ReturnId": "12",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": true,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-1-PG-11-150"
    },
    {
      "PageId": "12",
      "SequenceId": "1",
      "SectionId": "1",
      "Title": "SQ-1-SE-1-PG-12-165",
      "LinkTitle": "SQ-1-SE-1-PG-12-168",
      "InfoText": "SQ-1-SE-1-PG-12-167",
      "Questions": [
        {
          "QuestionId": "CD-22",
          "Label": "SQ-1-SE-1-PG-12-CD-22-170",
          "ShortLabel": "SQ-1-SE-1-PG-12-CD-22-171",
          "QuestionBodyText": "SQ-1-SE-1-PG-12-CD-22-172",
          "Hint": "SQ-1-SE-1-PG-12-CD-22-169",
          "Input": {
            "Type": "Radio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": null
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select yes if a director, or any person with significant control of your organisation have had any of the listed issues"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "NextPage",
          "ReturnId": "13",
          "Condition": {
            "QuestionId": "CD-22",
            "MustEqual": "Yes"
          },
          "ConditionMet": false
        },
        {
          "Action": "NextPage",
          "ReturnId": "14",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-1-PG-12-166"
    },
    {
      "PageId": "13",
      "SequenceId": "1",
      "SectionId": "1",
      "Title": "SQ-1-SE-1-PG-13-173",
      "LinkTitle": "SQ-1-SE-1-PG-13-176",
      "InfoText": "SQ-1-SE-1-PG-13-175",
      "Questions": [
        {
          "QuestionId": "CD-23",
          "Label": "SQ-1-SE-1-PG-13-CD-23-178",
          "ShortLabel": "SQ-1-SE-1-PG-13-CD-23-179",
          "QuestionBodyText": "SQ-1-SE-1-PG-13-CD-23-180",
          "Hint": "SQ-1-SE-1-PG-13-CD-23-177",
          "Input": {
            "Type": "Date",
            "Options": null,
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Enter a date"
              },
              {
                "Name":"Date",
                "Value":"",
                "ErrorMessage": "Date must be correct"
              }
            ]
          },
          "Order": null
        },
        {
          "QuestionId": "CD-24",
          "Label": "SQ-1-SE-1-PG-13-CD-24-182",
          "ShortLabel": "SQ-1-SE-1-PG-13-CD-24-183",
          "QuestionBodyText": "SQ-1-SE-1-PG-13-CD-24-184",
          "Hint": "SQ-1-SE-1-PG-13-CD-24-181",
          "Input": {
            "Type": "text",
            "Options": null,
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Field must not be empty"
              }
            ]
          },
          "Order": null
        },
        {
          "QuestionId": "CD-25",
          "Label": "SQ-1-SE-1-PG-13-CD-25-186",
          "ShortLabel": "SQ-1-SE-1-PG-13-CD-25-187",
          "QuestionBodyText": "SQ-1-SE-1-PG-13-CD-25-188",
          "Hint": "SQ-1-SE-1-PG-13-CD-25-185",
          "Input": {
            "Type": "text",
            "Options": null,
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Field must not be empty"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "NextPage",
          "ReturnId": "14",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": true,
      "Order": null,
      "Active": false,
      "Visible": false,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-1-PG-13-174"
    },
    {
      "PageId": "14",
      "SequenceId": "1",
      "SectionId": "1",
      "Title": "SQ-1-SE-1-PG-14-189",
      "LinkTitle": "SQ-1-SE-1-PG-14-192",
      "InfoText": "SQ-1-SE-1-PG-14-191",
      "Questions": [
        {
          "QuestionId": "CD-26",
          "Label": "SQ-1-SE-1-PG-14-CD-26-194",
          "ShortLabel": "SQ-1-SE-1-PG-14-CD-26-195",
          "QuestionBodyText": "SQ-1-SE-1-PG-14-CD-26-196",
          "Hint": "SQ-1-SE-1-PG-14-CD-26-193",
          "Input": {
            "Type": "ComplexRadio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": [
                  {
                    "QuestionId": "CD-26.1",
                    "Label": "SQ-1-SE-1-PG-14-CD-26.1-198",
                    "Hint": "SQ-1-SE-1-PG-14-CD-26.1-197",
                    "Input": {
                      "Type": "text",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter registered charity number"
                        }
                      ]
                    },
                    "Order": null,
                    "ShortLabel": "SQ-1-SE-1-PG-14-CD-26.1-199",
                    "QuestionBodyText": "SQ-1-SE-1-PG-14-CD-26.1-200"
                  }
                ]
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select yes if your organisation is a registered charity"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "NextPage",
          "ReturnId": "15",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-1-PG-14-190"
    },
    {
      "PageId": "15",
      "SequenceId": "1",
      "SectionId": "1",
      "Title": "SQ-1-SE-1-PG-15-201",
      "LinkTitle": "SQ-1-SE-1-PG-15-204",
      "InfoText": "SQ-1-SE-1-PG-15-203",
      "Questions": [
        {
          "QuestionId": "CD-27",
          "Label": "SQ-1-SE-1-PG-15-CD-27-206",
          "ShortLabel": "SQ-1-SE-1-PG-15-CD-27-207",
          "QuestionBodyText": "SQ-1-SE-1-PG-15-CD-27-208",
          "Hint": "SQ-1-SE-1-PG-15-CD-27-205",
          "Input": {
            "Type": "Radio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": null
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select an option"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "ReturnToSection",
          "ReturnId": "1",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-1-PG-15-202"
    }
  ],
  "FinancialApplicationGrade": null
}    
', N'Organisation details', N'Organisation details', N'Draft', N'Pages', N'')
GO



DELETE from WorkflowSections where SequenceId = 1 and SectionId=2;

INSERT [dbo].[WorkflowSections]
  ([Id], [WorkflowId], [SequenceId], [SectionId], [QnAData], [Title], [LinkTitle], [Status], [DisplayType], [DisallowedOrgTypes])
VALUES
  (N'5da45e68-d4fd-4fb6-9b04-4038d7adb4df', N'83b35024-8aef-440d-8f59-8c1cc459c350', 1, 2, N'

{
  "Pages": [
    {
      "PageId": "15",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "SQ-1-SE-2-PG-15-213",
      "LinkTitle": "SQ-1-SE-2-PG-15-LT-1",
      "InfoText": "SQ-1-SE-2-PG-15-215",
      "Questions": [
        {
          "QuestionId": "W_DEL-01",
          "Label": "SQ-1-SE-2-PG-15-W_DEL-01-218",
          "ShortLabel": "SQ-1-SE-2-PG-15-W_DEL-01-219",
          "QuestionBodyText": "SQ-1-SE-2-PG-15-W_DEL-01-220",
          "Hint": "SQ-1-SE-2-PG-15-W_DEL-01-217",
          "Input": {
            "Type": "text",
            "Options": null,
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Field must not be empty"
              }
            ]
          },
          "Order": null
        },
        {
          "QuestionId": "W_DEL-02",
          "Label": "SQ-1-SE-2-PG-15-W_DEL-02-222",
          "ShortLabel": "SQ-1-SE-2-PG-15-W_DEL-02-223",
          "QuestionBodyText": "SQ-1-SE-2-PG-15-W_DEL-02-224",
          "Hint": "SQ-1-SE-2-PG-15-W_DEL-02-221",
          "Input": {
            "Type": "text",
            "Options": null,
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Field must not be empty"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "NextPage",
          "ReturnId": "18",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-2-PG-15-214"
    },
    {
      "PageId": "18",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "SQ-1-SE-2-PG-18-233",
      "LinkTitle": "SQ-1-SE-2-PG-18-LT-1",
      "InfoText": "SQ-1-SE-2-PG-18-235",
      "Questions": [
        {
          "QuestionId": "W_DEL-04",
          "Label": "SQ-1-SE-2-PG-18-W_DEL-04-238",
          "ShortLabel": "SQ-1-SE-2-PG-18-W_DEL-04-239",
          "QuestionBodyText": "SQ-1-SE-2-PG-18-W_DEL-04-240",
          "Hint": "SQ-1-SE-2-PG-18-W_DEL-04-237",
          "Input": {
            "Type": "ComplexRadio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": null
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": [
                  {
                    "QuestionId": "W_DEL-04.1",
                    "Label": "SQ-1-SE-2-PG-18-W_DEL-04.1-242",
                    "ShortLabel": "SQ-1-SE-2-PG-18-W_DEL-04.1-243",
                    "Hint": "SQ-1-SE-2-PG-18-W_DEL-04.1-241",
                    "Input": {
                      "Type": "Date",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter a date"
                        },
                        {
                          "Name":"Date",
                          "Value":"",
                          "ErrorMessage": "Date must be correct"
                        }
                      ]
                    },
                    "Order": null,
                    "QuestionBodyText": "SQ-1-SE-2-PG-18-W_DEL-04.1-244"
                  }
                ]
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select an option"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "NextPage",
          "ReturnId": "193",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-2-PG-18-234"
    },
	   {
      "PageId": "193",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "SQ-1-SE-2-PG-19-245",
      "LinkTitle": "SQ-1-SE-2-PG-193-LT-1",
      "InfoText": "SQ-1-SE-2-PG-19-247",
      "Questions": [
            {
          "QuestionId": "M_DEL-09",
          "Label": "SQ-1-SE-2-PG-19-M_DEL-09-266",
          "ShortLabel": "SQ-1-SE-2-PG-19-M_DEL-09-267",
          "QuestionBodyText": "SQ-1-SE-2-PG-19-M_DEL-09-268",
          "Hint": "SQ-1-SE-2-PG-19-M_DEL-09-265",
          "Input": {
            "Type": "Radio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": null
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select an option"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "NextPage",
          "ReturnId": "194",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-2-PG-19-246"
    },
	   {
      "PageId": "194",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "SQ-1-SE-2-PG-19-245",
      "LinkTitle": "SQ-1-SE-2-PG-194-LT-1",
      "InfoText": "SQ-1-SE-2-PG-19-247",
      "Questions": [
           {
          "QuestionId": "M_DEL-10",
          "Label": "SQ-1-SE-2-PG-19-M_DEL-10-270",
          "ShortLabel": "SQ-1-SE-2-PG-19-M_DEL-10-271",
          "QuestionBodyText": "SQ-1-SE-2-PG-19-M_DEL-10-272",
          "Hint": "SQ-1-SE-2-PG-19-M_DEL-10-269",
          "Input": {
            "Type": "Radio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": null
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select an option"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "NextPage",
          "ReturnId": "195",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-2-PG-19-246"
    },
	   {
      "PageId": "195",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "SQ-1-SE-2-PG-19-245",
      "LinkTitle": "SQ-1-SE-2-PG-195-LT-1",
      "InfoText": "SQ-1-SE-2-PG-19-247",
      "Questions": [
                 {
          "QuestionId": "M_DEL-11",
          "Label": "SQ-1-SE-2-PG-19-M_DEL-11-274",
          "ShortLabel": "SQ-1-SE-2-PG-19-M_DEL-11-275",
          "QuestionBodyText": "SQ-1-SE-2-PG-19-M_DEL-11-276",
          "Hint": "SQ-1-SE-2-PG-19-M_DEL-11-273",
          "Input": {
            "Type": "Radio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": null
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select an option"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "NextPage",
          "ReturnId": "196",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-2-PG-19-246"
    },
	   {
      "PageId": "196",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "SQ-1-SE-2-PG-19-245",
      "LinkTitle": "SQ-1-SE-2-PG-196-LT-1",
      "InfoText": "SQ-1-SE-2-PG-19-247",
      "Questions": [
          {
          "QuestionId": "M_DEL-12",
          "Label": "SQ-1-SE-2-PG-19-M_DEL-12-278",
          "ShortLabel": "SQ-1-SE-2-PG-19-M_DEL-12-279",
          "QuestionBodyText": "SQ-1-SE-2-PG-19-M_DEL-12-280",
          "Hint": "SQ-1-SE-2-PG-19-M_DEL-12-277",
          "Input": {
            "Type": "Radio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": null
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select an option"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "NextPage",
          "ReturnId": "20",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-2-PG-19-246"
    },
    {
      "PageId": "20",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "SQ-1-SE-2-PG-20-281",
      "LinkTitle": "SQ-1-SE-2-PG-20-LT-1",
      "InfoText": "SQ-1-SE-2-PG-20-283",
      "Questions": [
        {
          "QuestionId": "D_DEL-13",
          "Label": "SQ-1-SE-2-PG-20-D_DEL-13-286",
          "ShortLabel": "SQ-1-SE-2-PG-20-D_DEL-13-287",
          "QuestionBodyText": "SQ-1-SE-2-PG-20-D_DEL-13-288",
          "Hint": "SQ-1-SE-2-PG-20-D_DEL-13-285",
          "Input": {
            "Type": "ComplexRadio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": [
                  {
                    "QuestionId": "D_DEL-13.1",
                    "Label": "SQ-1-SE-2-PG-20-D_DEL-13.1-290",
                    "ShortLabel": "SQ-1-SE-2-PG-20-D_DEL-13.1-291",
                    "QuestionBodyText": "SQ-1-SE-2-PG-20-D_DEL-13.1-292",
                    "Hint": "SQ-1-SE-2-PG-20-D_DEL-13.1-289",
                    "Input": {
                      "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Field must not be empty"
                        }
                      ]
                    },
                    "Order": null
                  }
                ]
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select an option"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "NextPage",
          "ReturnId": "201",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-2-PG-20-282"
    },
	{
      "PageId": "201",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "SQ-1-SE-2-PG-20-281",
      "LinkTitle": "SQ-1-SE-2-PG-201-LT-1",
      "InfoText": "SQ-1-SE-2-PG-20-283",
      "Questions": [
         {
          "QuestionId": "D_DEL-14",
          "Label": "SQ-1-SE-2-PG-20-D_DEL-14-294",
          "ShortLabel": "SQ-1-SE-2-PG-20-D_DEL-14-295",
          "QuestionBodyText": "SQ-1-SE-2-PG-20-D_DEL-14-296",
          "Hint": "SQ-1-SE-2-PG-20-D_DEL-14-293",
          "Input": {
            "Type": "Radio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": null
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select an option"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "NextPage",
          "ReturnId": "20.1",
          "Condition": {
            "QuestionId": "D_DEL-14",
            "MustEqual": "Yes"
          },
          "ConditionMet": false
        },
        {
          "Action": "NextPage",
          "ReturnId": "202",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-2-PG-20-282"
    },
    {
      "PageId": "20.1",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "SQ-1-SE-2-PG-20-281",
      "LinkTitle": "SQ-1-SE-2-PG-20.1-LT-1",
      "InfoText": "SQ-1-SE-2-PG-20.1-299",
      "Questions": [
        {
          "QuestionId": "D_DEL-13-1",
          "Label": "SQ-1-SE-2-PG-20.1-D_DEL-13-1-302",
          "ShortLabel": "SQ-1-SE-2-PG-20.1-D_DEL-13-1-303",
          "QuestionBodyText": "SQ-1-SE-2-PG-20.1-D_DEL-13-1-304",
          "Hint": "SQ-1-SE-2-PG-20.1-D_DEL-13-1-301",
          "Input": {
            "Type": "text",
            "Options": null,
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Field must not be empty"
              }
            ]
          },
          "Order": null
        },
        {
          "QuestionId": "D_DEL-13-2",
          "Label": "SQ-1-SE-2-PG-20.1-D_DEL-13-1-306",
          "ShortLabel": "SQ-1-SE-2-PG-20.1-D_DEL-13-1-307",
          "QuestionBodyText": "SQ-1-SE-2-PG-20.1-D_DEL-13-1-308",
          "Hint": "SQ-1-SE-2-PG-20.1-D_DEL-13-1-305",
          "Input": {
            "Type": "Date",
            "Options": null,
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Enter a date"
              },
              {
                "Name":"Date",
                "Value":"",
                "ErrorMessage": "Date must be correct"
              }
            ]
          },
          "Order": null
        },
        {
          "QuestionId": "D_DEL-13-3",
          "Label": "SQ-1-SE-2-PG-20.1-D_DEL-13-1-310",
          "ShortLabel": "SQ-1-SE-2-PG-20.1-D_DEL-13-1-311",
          "QuestionBodyText": "SQ-1-SE-2-PG-20.1-D_DEL-13-1-312",
          "Hint": "SQ-1-SE-2-PG-20.1-D_DEL-13-1-309",
          "Input": {
            "Type": "text",
            "Options": null,
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Field must not be empty"
              }
            ]
          },
          "Order": null
        },
        {
          "QuestionId": "D_DEL-13-4",
          "Label": "SQ-1-SE-2-PG-20.1-D_DEL-13-1-314",
          "ShortLabel": "SQ-1-SE-2-PG-20.1-D_DEL-13-1-315",
          "QuestionBodyText": "SQ-1-SE-2-PG-20.1-D_DEL-13-1-316",
          "Hint": "SQ-1-SE-2-PG-20.1-D_DEL-13-1-313",
          "Input": {
            "Type": "Date",
            "Options": null,
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Enter a date"
              },
              {
                "Name":"Date",
                "Value":"",
                "ErrorMessage": "Date must be correct"
              }
            ]
          },
          "Order": null
        }

      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "NextPage",
          "ReturnId": "202",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": true,
      "Order": null,
      "Active": false,
      "Visible": false,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-2-PG-20.1-298"
    },
	{
      "PageId": "202",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "SQ-1-SE-2-PG-20-281",
      "LinkTitle": "SQ-1-SE-2-PG-202-LT-1",
      "InfoText": "SQ-1-SE-2-PG-20-283",
      "Questions": [
         {
          "QuestionId": "A_DEL-21",
          "Label": "SQ-1-SE-2-PG-21-A_DEL-21-322",
          "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-21-323",
          "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-21-324",
          "Hint": "SQ-1-SE-2-PG-21-A_DEL-21-321",
          "Input": {
            "Type": "ComplexRadio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": [
                  {
                    "QuestionId": "A_DEL-21.1",
                    "Label": "SQ-1-SE-2-PG-21-A_DEL-21.1-326",
                    "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-21.1-327",
                    "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-21.1-328",
                    "Hint": "SQ-1-SE-2-PG-21-A_DEL-21.1-325",
                    "Input": {
                      "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Field must not be empty"
                        }
                      ]
                    },
                    "Order": null
                  }
                ]
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select an option"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
          {
            "Action": "NextPage",
			"ReturnId": "203",
            "Condition": null,
            "ConditionMet": false
          }
        ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-2-PG-20-282"
    },
	{
      "PageId": "203",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "SQ-1-SE-2-PG-20-281",
      "LinkTitle": "SQ-1-SE-2-PG-203-LT-1",
      "InfoText": "SQ-1-SE-2-PG-20-283",
      "Questions": [
        {
          "QuestionId": "A_DEL-22",
          "Label": "SQ-1-SE-2-PG-21-A_DEL-22-330",
          "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-22-331",
          "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-22-332",
          "Hint": "SQ-1-SE-2-PG-21-A_DEL-22-329",
          "Input": {
            "Type": "ComplexRadio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": [
                  {
                    "QuestionId": "A_DEL-22.1",
                    "Label": "SQ-1-SE-2-PG-21-A_DEL-22.1-334",
                    "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-22.1-335",
                    "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-22.1-336",
                    "Hint": "SQ-1-SE-2-PG-21-A_DEL-22.1-333",
                    "Input": {
                      "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Field must not be empty"
                        }
                      ]
                    },
                    "Order": null
                  }
                ]
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select an option"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
          {
            "Action": "NextPage",
			"ReturnId": "204",
            "Condition": null,
            "ConditionMet": false
          }
        ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-2-PG-20-282"
    },
	{
      "PageId": "204",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "SQ-1-SE-2-PG-20-281",
      "LinkTitle": "SQ-1-SE-2-PG-204-LT-1",
      "InfoText": "SQ-1-SE-2-PG-20-283",
      "Questions": [
        {
          "QuestionId": "A_DEL-23",
          "Label": "SQ-1-SE-2-PG-21-A_DEL-23-338",
          "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-23-339",
          "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-23-340",
          "Hint": "SQ-1-SE-2-PG-21-A_DEL-23-337",
          "Input": {
            "Type": "ComplexRadio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": [
                  {
                    "QuestionId": "A_DEL-23.1",
                    "Label": "SQ-1-SE-2-PG-21-A_DEL-23.1-342",
                    "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-23.1-343",
                    "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-23.1-344",
                    "Hint": "SQ-1-SE-2-PG-21-A_DEL-23.1-341",
                    "Input": {
                      "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Field must not be empty"
                        }
                      ]
                    },
                    "Order": null
                  }
                ]
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select an option"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
          {
            "Action": "NextPage",
			"ReturnId": "205",
            "Condition": null,
            "ConditionMet": false
          }
        ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-2-PG-20-282"
    },
	{
      "PageId": "205",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "SQ-1-SE-2-PG-20-281",
      "LinkTitle": "SQ-1-SE-2-PG-205-LT-1",
      "InfoText": "SQ-1-SE-2-PG-20-283",
      "Questions": [
       {
          "QuestionId": "A_DEL-24",
          "Label": "SQ-1-SE-2-PG-21-A_DEL-24-346",
          "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-24-347",
          "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-24-348",
          "Hint": "SQ-1-SE-2-PG-21-A_DEL-24-345",
          "Input": {
            "Type": "ComplexRadio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": [
                  {
                    "QuestionId": "A_DEL-24.1",
                    "Label": "SQ-1-SE-2-PG-21-A_DEL-24.1-350",
                    "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-24.1-351",
                    "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-24.1-352",
                    "Hint": "SQ-1-SE-2-PG-21-A_DEL-24.1-349",
                    "Input": {
                      "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Field must not be empty"
                        }
                      ]
                    },
                    "Order": null
                  }
                ]
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select an option"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
          {
            "Action": "NextPage",
			"ReturnId": "206",
            "Condition": null,
            "ConditionMet": false
          }
        ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-2-PG-20-282"
    },
	{
      "PageId": "206",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "SQ-1-SE-2-PG-20-281",
      "LinkTitle": "SQ-1-SE-2-PG-206-LT-1",
      "InfoText": "SQ-1-SE-2-PG-20-283",
      "Questions": [
       {
          "QuestionId": "A_DEL-25",
          "Label": "SQ-1-SE-2-PG-21-A_DEL-25-354",
          "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-25-355",
          "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-25-356",
          "Hint": "SQ-1-SE-2-PG-21-A_DEL-25-353",
          "Input": {
            "Type": "ComplexRadio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": [
                  {
                    "QuestionId": "A_DEL-25.1",
                    "Label": "SQ-1-SE-2-PG-21-A_DEL-25.1-358",
                    "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-25.1-359",
                    "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-25.1-360",
                    "Hint": "SQ-1-SE-2-PG-21-A_DEL-25.1-357",
                    "Input": {
                      "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Field must not be empty"
                        }
                      ]
                    },
                    "Order": null
                  },
                  {
                    "QuestionId": "A_DEL-25.2",
                    "Label": "SQ-1-SE-2-PG-21-A_DEL-25.2-362",
                    "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-25.2-363",
                    "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-25.2-364",
                    "Hint": "SQ-1-SE-2-PG-21-A_DEL-25.2-361",
                    "Input": {
                      "Type": "Text",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Field must not be empty"
                        }
                      ]
                    },
                    "Order": null
                  },
                  {
                    "QuestionId": "A_DEL-25.3",
                    "Label": "SQ-1-SE-2-PG-21-A_DEL-25.3-366",
                    "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-25.3-367",
                    "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-25.3-368",
                    "Hint": "SQ-1-SE-2-PG-21-A_DEL-25.3-365",
                    "Input": {
                      "Type": "Date",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter a date"
                        },
                        {
                          "Name":"Date",
                          "Value":"",
                          "ErrorMessage": "Date must be correct"
                        }
                      ]
                    },
                    "Order": null
                  }
                ]
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select an option"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
          {
            "Action": "NextPage",
			"ReturnId": "207",
            "Condition": null,
            "ConditionMet": false
          }
        ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-2-PG-20-282"
    },
	{
      "PageId": "207",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "SQ-1-SE-2-PG-20-281",
      "LinkTitle": "SQ-1-SE-2-PG-207-LT-1",
      "InfoText": "SQ-1-SE-2-PG-20-283",
      "Questions": [
        {
          "QuestionId": "A_DEL-26",
          "Label": "SQ-1-SE-2-PG-21-A_DEL-26-370",
          "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-26-371",
          "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-26-372",
          "Hint": "SQ-1-SE-2-PG-21-A_DEL-26-369",
          "Input": {
            "Type": "ComplexRadio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": [
                  {
                    "QuestionId": "A_DEL-26.1",
                    "Label": "SQ-1-SE-2-PG-21-A_DEL-26.1-374",
                    "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-26.1-375",
                    "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-26.1-376",
                    "Hint": "SQ-1-SE-2-PG-21-A_DEL-26.1-373",
                    "Input": {
                      "Type": "Date",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter a date"
                        },
                        {
                          "Name":"Date",
                          "Value":"",
                          "ErrorMessage": "Date must be correct"
                        }
                      ]
                    },
                    "Order": null
                  },
                  {
                    "QuestionId": "A_DEL-26.2",
                    "Label": "SQ-1-SE-2-PG-21-A_DEL-26.2-378",
                    "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-26.2-379",
                    "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-26.2-380",
                    "Hint": "SQ-1-SE-2-PG-21-A_DEL-26.2-377",
                    "Input": {
                      "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Field must not be empty"
                        }
                      ]
                    },
                    "Order": null
                  }
                ]
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select an option"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
          {
            "Action": "NextPage",
			"ReturnId": "208",
            "Condition": null,
            "ConditionMet": false
          }
        ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-2-PG-20-282"
    },
	{
      "PageId": "208",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "SQ-1-SE-2-PG-20-281",
      "LinkTitle": "SQ-1-SE-2-PG-208-LT-1",
      "InfoText": "SQ-1-SE-2-PG-20-283",
      "Questions": [
       {
          "QuestionId": "A_DEL-27",
          "Label": "SQ-1-SE-2-PG-21-A_DEL-27-382",
          "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-27-383",
          "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-27-384",
          "Hint": "SQ-1-SE-2-PG-21-A_DEL-27-381",
          "Input": {
            "Type": "ComplexRadio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": [
                  {
                    "QuestionId": "A_DEL-27.1",
                    "Label": "SQ-1-SE-2-PG-21-A_DEL-27.1-386",
                    "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-27.1-387",
                    "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-27.1-388",
                    "Hint": "SQ-1-SE-2-PG-21-A_DEL-27.1-385",
                    "Input": {
                      "Type": "Date",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter a date"
                        },
                        {
                          "Name":"Date",
                          "Value":"",
                          "ErrorMessage": "Date must be correct"
                        }
                      ]
                    },
                    "Order": null
                  },
                  {
                    "QuestionId": "A_DEL-27.2",
                    "Label": "SQ-1-SE-2-PG-21-A_DEL-27.2-390",
                    "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-27.2-391",
                    "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-27.2-392",
                    "Hint": "SQ-1-SE-2-PG-21-A_DEL-27.2-389",
                    "Input": {
                      "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Field must not be empty"
                        }
                      ]
                    },
                    "Order": null
                  }
                ]
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select an option"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
          {
            "Action": "NextPage",
			"ReturnId": "209",
            "Condition": null,
            "ConditionMet": false
          }
        ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-2-PG-20-282"
    },
	{
      "PageId": "209",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "SQ-1-SE-2-PG-20-281",
      "LinkTitle": "SQ-1-SE-2-PG-209-LT-1",
      "InfoText": "SQ-1-SE-2-PG-20-283",
      "Questions": [
        {
          "QuestionId": "A_DEL-28",
          "Label": "SQ-1-SE-2-PG-21-A_DEL-28-394",
          "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-28-395",
          "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-28-396",
          "Hint": "SQ-1-SE-2-PG-21-A_DEL-28-393",
          "Input": {
            "Type": "ComplexRadio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": [
                  {
                    "QuestionId": "A_DEL-28.1",
                    "Label": "SQ-1-SE-2-PG-21-A_DEL-28.1-398",
                    "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-28.1-399",
                    "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-28.1-400",
                    "Hint": "SQ-1-SE-2-PG-21-A_DEL-28.1-397",
                    "Input": {
                      "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Field must not be empty"
                        }
                      ]
                    },
                    "Order": null
                  }
                ]
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select an option"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
          {
            "Action": "NextPage",
			"ReturnId": "210",
            "Condition": null,
            "ConditionMet": false
          }
        ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-2-PG-20-282"
    },
	{
      "PageId": "210",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "SQ-1-SE-2-PG-20-281",
      "LinkTitle": "SQ-1-SE-2-PG-210-LT-1",
      "InfoText": "SQ-1-SE-2-PG-20-283",
      "Questions": [
        {
          "QuestionId": "A_DEL-29",
          "Label": "SQ-1-SE-2-PG-21-A_DEL-29-402",
          "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-29-403",
          "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-29-404",
          "Hint": "SQ-1-SE-2-PG-21-A_DEL-29-401",
          "Input": {
            "Type": "ComplexRadio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": [
                  {
                    "QuestionId": "A_DEL-29.1",
                    "Label": "SQ-1-SE-2-PG-21-A_DEL-29.1-406",
                    "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-29.1-407",
                    "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-29.1-408",
                    "Hint": "SQ-1-SE-2-PG-21-A_DEL-29.1-405",
                    "Input": {
                      "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Field must not be empty"
                        }
                      ]
                    },
                    "Order": null
                  }
                ]
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select an option"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
          {
            "Action": "NextPage",
			"ReturnId": "211",
            "Condition": null,
            "ConditionMet": false
          }
        ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-2-PG-20-282"
    },
	{
      "PageId": "211",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "SQ-1-SE-2-PG-20-281",
      "LinkTitle": "SQ-1-SE-2-PG-211-LT-1",
      "InfoText": "SQ-1-SE-2-PG-20-283",
      "Questions": [
        {
          "QuestionId": "A_DEL-30",
          "Label": "SQ-1-SE-2-PG-21-A_DEL-30-410",
          "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-30-411",
          "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-30-412",
          "Hint": "SQ-1-SE-2-PG-21-A_DEL-30-409",
          "Input": {
            "Type": "ComplexRadio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": [
                  {
                    "QuestionId": "A_DEL-30.1",
                    "Label": "SQ-1-SE-2-PG-21-A_DEL-30.1-414",
                    "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-30.1-415",
                    "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-30.1-416",
                    "Hint": "SQ-1-SE-2-PG-21-A_DEL-30.1-413",
                    "Input": {
                      "Type": "Date",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter a date"
                        },
                        {
                          "Name":"Date",
                          "Value":"",
                          "ErrorMessage": "Date must be correct"
                        }
                      ]
                    },
                    "Order": null
                  },
                  {
                    "QuestionId": "A_DEL-30.2",
                    "Label": "SQ-1-SE-2-PG-21-A_DEL-30.2-418",
                    "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-30.2-419",
                    "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-30.2-420",
                    "Hint": "SQ-1-SE-2-PG-21-A_DEL-30.2-417",
                    "Input": {
                      "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Field must not be empty"
                        }
                      ]
                    },
                    "Order": null
                  },
                  {
                    "QuestionId": "A_DEL-30.3",
                    "Label": "SQ-1-SE-2-PG-21-A_DEL-30.3-422",
                    "ShortLabel": "SQ-1-SE-2-PG-21-A_DEL-30.3-423",
                    "QuestionBodyText": "SQ-1-SE-2-PG-21-A_DEL-30.3-424",
                    "Hint": "SQ-1-SE-2-PG-21-A_DEL-30.3-421",
                    "Input": {
                      "Type": "Text",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Field must not be empty"
                        }
                      ]
                    },
                    "Order": null
                  }
                ]
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select an option"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
          {
            "Action": "NextPage",
			"ReturnId": "22",
            "Condition": null,
            "ConditionMet": false
          }
        ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-2-PG-20-282"
    },
    {
      "PageId": "22",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "SQ-1-SE-2-PG-22-425",
      "LinkTitle": "SQ-1-SE-2-PG-22-LT-1",
      "InfoText": "SQ-1-SE-2-PG-22-427",
      "Questions": [
        {
          "QuestionId": "A_DEL-28",
          "Label": "SQ-1-SE-2-PG-22-A_DEL-28-430",
          "ShortLabel": "SQ-1-SE-2-PG-22-A_DEL-28-431",
          "QuestionBodyText": "SQ-1-SE-2-PG-22-A_DEL-28-432",
          "Hint": "SQ-1-SE-2-PG-22-A_DEL-28-429",
          "Input": {
            "Type": "Radio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": null
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select an option"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "NextPage",
          "ReturnId": "221",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-2-PG-22-426"
    },
    {
      "PageId": "221",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "SQ-1-SE-2-PG-22-425",
      "LinkTitle": "SQ-1-SE-2-PG-221-LT-1",
      "InfoText": "SQ-1-SE-2-PG-22-427",
      "Questions": [
       {
          "QuestionId": "A_DEL-29",
          "Label": "SQ-1-SE-2-PG-22-A_DEL-29-434",
          "ShortLabel": "SQ-1-SE-2-PG-22-A_DEL-29-435",
          "QuestionBodyText": "SQ-1-SE-2-PG-22-A_DEL-29-436",
          "Hint": "SQ-1-SE-2-PG-22-A_DEL-29-433",
          "Input": {
            "Type": "Radio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": null
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select an option"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "NextPage",
          "ReturnId": "222",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-2-PG-22-426"
    },
    {
      "PageId": "222",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "SQ-1-SE-2-PG-22-425",
      "LinkTitle": "SQ-1-SE-2-PG-222-LT-1",
      "InfoText": "SQ-1-SE-2-PG-22-427",
      "Questions": [
       {
          "QuestionId": "A_DEL-30",
          "Label": "SQ-1-SE-2-PG-22-A_DEL-30-438",
          "ShortLabel": "SQ-1-SE-2-PG-22-A_DEL-30-439",
          "QuestionBodyText": "SQ-1-SE-2-PG-22-A_DEL-30-440",
          "Hint": "SQ-1-SE-2-PG-22-A_DEL-30-437",
          "Input": {
            "Type": "Radio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": null
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select an option"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "ReturnToSection",
          "ReturnId": "2",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-2-PG-22-426"
    }
  ],
  "FinancialApplicationGrade": null
}
   
', N'Declarations', N'Declarations', N'Draft', N'PagesWithSections', N'')
GO

DELETE from WorkflowSections where SequenceId=1 and SectionId=3
GO
INSERT [dbo].[WorkflowSections]
  ([Id], [WorkflowId], [SequenceId], [SectionId], [QnAData], [Title], [LinkTitle], [Status], [DisplayType], [DisallowedOrgTypes])
VALUES
  (N'580aa30f-f65b-4c05-808f-f8eb3d539459', N'83b35024-8aef-440d-8f59-8c1cc459c350', 1, 3, N'
{
  "Pages": [
    {
      "PageId": "23",
      "SequenceId": "1",
      "SectionId": "3",
      "Title": "SQ-1-SE-3-PG-23-441",
      "LinkTitle": "SQ-1-SE-3-PG-23-444",
      "InfoText": "SQ-1-SE-3-PG-23-443",
      "Questions": [
        {
          "QuestionId": "FHA-01",
          "Label": "SQ-1-SE-3-PG-23-FHA-01-446",
          "ShortLabel": "SQ-1-SE-3-PG-23-FHA-01-447",
          "QuestionBodyText": "SQ-1-SE-3-PG-23-FHA-01-448",
          "Hint": "SQ-1-SE-3-PG-23-FHA-01-445",
          "Input": {
            "Type": "FileUpload",
            "Options": null,
            "Validations": [
               {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select a file containing your financial evidences"
              }
            ]
          },
          "Order": null
        },
        {
          "QuestionId": "FHA-02",
          "Label": "SQ-1-SE-3-PG-23-FHA-02-450",
          "ShortLabel": "SQ-1-SE-3-PG-23-FHA-02-451",
          "QuestionBodyText": "SQ-1-SE-3-PG-23-FHA-02-452",
          "Hint": "SQ-1-SE-3-PG-23-FHA-02-449",
          "Input": {
            "Type": "FileUpload",
            "Options": null,
            "Validations": [
               {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select a file containg the latest available accounts for the UK ultimate parent company"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "ReturnToSection",
          "ReturnId": "3",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "BodyText": "SQ-1-SE-3-PG-23-442"
    }
  ],
  "FinancialApplicationGrade": null
}
', N'Financial Health Assessment', N'Financial Health Assessment', N'Draft', N'Pages', N'')
GO
DELETE from WorkFlowSequences  where sequenceId=1

INSERT [dbo].[WorkflowSequences]
  ([Id], [WorkflowId], [SequenceId], [Status], [IsActive])
VALUES
  (N'bde3cf18-b1a8-4b4b-8cdc-a26dd6e418bd', N'83b35024-8aef-440d-8f59-8c1cc459c350', 1, N'Draft', 1)
GO
DELETE from WorkFlowSequences  where sequenceId=2

INSERT [dbo].[WorkflowSequences]
  ([Id], [WorkflowId], [SequenceId], [Status], [IsActive])
VALUES
  (N'981325d9-c7a4-43d5-8ece-75fef3b080f5', N'83b35024-8aef-440d-8f59-8c1cc459c350', 2, N'Draft', 0)
GO




-- SECTION 4 WORK
--SECTION 4
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-T-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-T-1', '', 'Your policies and procedures', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-LT-1', '','Information commissioner''s office registration', 'Live', GETUTCDATE(), 'Import')



  GO
  DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-25-T-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-25-T-1', '', 'Your occupational competence', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-25-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-25-LT-1', '', 'Internal audit policy', 'Live', GETUTCDATE(), 'Import')


  GO
  DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-26-T-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-26-T-1', '', 'Your assessors', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-26-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-26-LT-1', '', 'Public liability insurance', 'Live', GETUTCDATE(), 'Import')

  GO
  DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-27-T-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-27-T-1', '', 'Your professional standards', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-27-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-27-LT-1', '', 'Professional indemnity insurance', 'Live', GETUTCDATE(), 'Import')

-- ROW F
  GO
  DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-T-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-T-1', '', 'Your end-point assessment delivery model', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-LT-1', '', 'Employers liability insurance', 'Live', GETUTCDATE(), 'Import')

  GO
  DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-T-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-T-1', '', 'Your end-point assessment competence', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-LT-1', '', 'Safeguarding policy', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-30-T-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-30-T-1', '', 'Your online information', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-30-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-30-LT-1', '', 'Prevent agenda policy', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-BT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-BT-1', '', '', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-25-BT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-25-BT-1', '', '', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-26-BT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-26-BT-1', '', '', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-27-BT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-27-BT-1', '', '', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-BT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-BT-1', '', '', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-BT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-BT-1', '', '', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-30-BT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-30-BT-1', '', '', 'Live', GETUTCDATE(), 'Import')



  GO
  DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-31-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-31-LT-1', '', 'Conflict of interest policy', 'Live', GETUTCDATE(), 'Import')


  GO
  DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-32-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-32-LT-1', '', 'Monitoring procedures', 'Live', GETUTCDATE(), 'Import')

  GO
  DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-33-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-33-LT-1', '', 'Monitoring processes', 'Live', GETUTCDATE(), 'Import')

  GO
  DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-34-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-34-LT-1', '', 'Complaints and appeals policy', 'Live', GETUTCDATE(), 'Import')

  GO
  DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-340-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-340-LT-1', '', 'Fair access', 'Live', GETUTCDATE(), 'Import')


  GO
  DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-35-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-35-LT-1', '', 'Consistency assurance', 'Live', GETUTCDATE(), 'Import')

  GO
  DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-36-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-36-LT-1', '', 'Continuous quality assurance', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-37-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-37-LT-1', '', 'Engagement with trailblazers and employers', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-38-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-38-LT-1', '', 'Professional organisation membership', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-39-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-39-LT-1', '', 'Number of assessors', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-40-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-40-LT-1', '', 'Assessment capacity', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-41-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-41-LT-1', '', 'Ability to deliver assessments', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-42-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-42-LT-1', '', 'Recruitment and training', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-43-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-43-LT-1', '', 'Skills and qualifications', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-44-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-44-LT-1', '', 'Continuous professional development', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-45-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-45-LT-1', '', 'End-point assessment delivery model', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-46-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-46-LT-1', '', 'Outsourcing of end-point assessments', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-47-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-47-LT-1', '', 'Engaging with employers and training providers', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-48-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-48-LT-1', '', 'Managing conflicts of interest', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-49-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-49-LT-1', '', 'Assessment locations', 'Live', GETUTCDATE(), 'Import')


GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-50-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-50-LT-1', '', 'Assessment methods', 'Live', GETUTCDATE(), 'Import')


GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-51-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-51-LT-1', '', 'Continuous resource development', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-52-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-52-LT-1', '', 'Secure IT infrastructure', 'Live', GETUTCDATE(), 'Import')


GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-53-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-53-LT-1', '', 'Assessment administration', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-54-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-54-LT-1', '', 'Assessment products and tools', 'Live', GETUTCDATE(), 'Import')


GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-55-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-55-LT-1', '', 'Assessment content', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-56-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-56-LT-1', '', 'Collation & confirmation of assessment outcomes', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-57-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-57-LT-1', '', 'Recording assessment results', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-58-LT-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-58-LT-1', '', 'Web address', 'Live', GETUTCDATE(), 'Import')



-- page 24 question 1
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-01-L-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-01-L-1', '', 'Information commissioner''s office (ICO) registration number', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-01-SL-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-01-SL-1', '', 'Information commissoner''s office registration', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-01-QB-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-01-QB-1', '', '', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-01-H-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-01-H-1', '','Provide your ICO registration number', 'Live', GETUTCDATE(), 'Import')


-- page 24 question 2
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-02-L-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-02-L-1', '', 'Internal audit policy', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-02-SL-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-02-SL-1', '', 'Internal audit policy', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-02-QB-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-02-QB-1', '','<p class="govuk-body">Upload a PDF of your organisation''s internal audit policy in respect to fraud and financial irregularity</p>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-02-H-1';
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-02-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

-- page 24 question 3
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-03-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-03-L-1', '', 'Public liability insurance', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-03-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-03-SL-1', '', 'Public liability insurance', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-03-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-03-QB-1', '','<p class="govuk-body">Upload a PDF of your public liability certificate of insurance</p>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-03-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-03-H-1', '', 'If you are providing any form of training or consultancy, you must have public liability insurance.', 'Live', GETUTCDATE(), 'Import')


-- page 24 question 4
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-04-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-04-L-1', '', 'Professional indemnity insurance', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-04-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-04-SL-1', '', 'Professional indemnity insurance', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-04-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-04-QB-1', '','<p class="govuk-body">Upload a PDF of your professional indemnity certificate of insurance</p>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-04-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-04-H-1', '', 'If you are providing any form of training or consultancy, you must have professional indemnity insurance.', 'Live', GETUTCDATE(), 'Import')

-- page 24 question 5
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-05-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-05-L-1', '', 'Employers liability insurance', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-05-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-05-SL-1', '', 'Employers liability insurance', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-05-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-05-QB-1', '','<p class="govuk-body">Upload a PDF of your employers liability certificate of insurance (optional)</p>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-05-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-05-H-1', '', 'If you have any employees, you must have employers liability insurance. ', 'Live', GETUTCDATE(), 'Import')

  -- page 24 question 6
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-06-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-06-L-1', '', 'Safeguarding policy', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-06-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-06-SL-1', '', 'Safeguarding policy', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-06-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-06-QB-1', '','<p class="govuk-body">Upload a PDF of your safeguarding policy</p>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-06-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-06-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

-- page 24 question 7
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-07-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-07-L-1', '', 'Prevent agenda policy', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-07-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-07-SL-1', '', 'Prevent agenda policy', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-07-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-07-QB-1', '','<p class="govuk-body">Upload your PDF conflict of interest policy document</p>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-07-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-07-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

  -- page 24 question 8
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-08-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-08-L-1', '', 'Conflict of interest policy', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-08-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-08-SL-1', '', 'Conflict of interest policy', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-08-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-08-QB-1', '','<p class="govuk-body">Upload a PDF of your conflict of interest policy</p>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-08-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-08-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

-- page 24 question 9
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-09-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-09-L-1', '', 'Monitoring procedures', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-09-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-09-SL-1', '', 'Monitoring procedures', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-09-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-09-QB-1', '','<p class="govuk-body">Upload a PDF of your procedures for monitoring assessor practices and decisions</p>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-09-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-09-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

-- page 24 question 10
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-10-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-10-L-1', '', 'Moderation processes', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-10-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-10-SL-1', '', 'Moderation processes', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-10-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-10-QB-1', '','<p class="govuk-body">Upload a PDF describing your standardisation and moderation activities, including how you sample assessment decisions</p>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-10-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-10-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

-- page 24 question 11
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-11-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-11-L-1', '', 'Complaints and appeals policy', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-11-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-11-SL-1', '', 'Employers liability insurance', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-11-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-11-QB-1', '','<p class="govuk-body">Upload a PDF of your complaints and appeals policy</p>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-11-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-11-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

  -- page 24 question 12
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-12-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-12-L-1', '', 'Fair access', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-12-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-12-SL-1', '', 'Fair access', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-12-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-12-QB-1', '','<p class="govuk-body">Upload a PDF of your fair access policy</p>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-12-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-12-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

  -- page 24 question 13
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-13-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-13-L-1', '', 'Consistency assurance', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-13-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-13-SL-1', '', 'Consistency assurance', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-13-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-13-QB-1', '','<p class="govuk-body">Upload a PDF of your strategy for ensuring comparability and consistency of assessment decisions</p>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-13-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-13-H-1', '', '', 'Live', GETUTCDATE(), 'Import')
  

  -- page 24 question 14
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-14-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-14-L-1', '', 'How do you continuously improve the quality of your assessment practice?', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-14-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-14-SL-1', '', 'How do you continuously improve the quality of your assessment practice?', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-14-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-14-QB-1', '','', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-14-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-24-CC-14-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

--  -- page 24 question 15
GO--
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-15-L-1';  -- Row L
--INSERT INTO Assets
--  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
--VALUES
--  (NEWID(), 'SQ-2-SE-4-PG-24-CC-15-L-1', '', 'Employers liability insurance', 'Live', GETUTCDATE(), 'Import')

GO--
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-15-SL-1'; -- Row J
--INSERT INTO Assets
--  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
--VALUES
--  (NEWID(), 'SQ-2-SE-4-PG-24-CC-15-SL-1', '', 'Employers liability insurance', 'Live', GETUTCDATE(), 'Import')

GO--
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-15-QB-1';   -- Row P
--INSERT INTO Assets
--  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
--VALUES
--  (NEWID(), 'SQ-2-SE-4-PG-24-CC-15-QB-1', '','Upload a PDF of your employers liability certificate of insurance (optional)', 'Live', GETUTCDATE(), 'Import')
GO--
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-24-CC-15-H-1';   -- Row AA
--INSERT INTO Assets
--  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
--VALUES
--  (NEWID(), 'SQ-2-SE-4-PG-24-CC-15-H-1', '', 'If you have any employees, you must have employers liability insurance. ', 'Live', GETUTCDATE(), 'Import')

  -- page 25 question 16
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-25-CC-16-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-25-CC-16-L-1', '', 'Give evidence of engagement with trailblazers and employers to demonstrate your organisation''s occupational competence to assess [StandardName]', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-25-CC-16-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-25-CC-16-SL-1', '', 'Engagement with trailblazers and employers', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-25-CC-16-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-25-CC-16-QB-1', '',N'<p class="govuk-body">Your evidence must demonstrate your organisation''s relevant experience of working with employers or working in the specific occupational area.</p><p class="govuk-body">Your evidence must not be over three years old and must not relate to the development and implementation of qualifications.</p><p class="govuk-body">You should give answers that relate to the assessment plan for the standard you are applying for.</p>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-25-CC-16-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-25-CC-16-H-1', '', '', 'Live', GETUTCDATE(), 'Import')



    -- page 25 question 19
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-25-CC-19-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-25-CC-19-L-1', '', 'Give details of membership of professional organisations', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-25-CC-19-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-25-CC-19-SL-1', '', 'Professional organisation membership', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-25-CC-19-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-25-CC-19-QB-1', '',N'<p class="govuk-body">Give details of why this supports best practice and skills for this assessment plan. Show how it demonstrates your competence to assess this standard.</p>', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-25-CC-19-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-25-CC-19-H-1', '', '', 'Live', GETUTCDATE(), 'Import')


    -- page 26 question 20
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-26-CC-20-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-26-CC-20-L-1', '', 'How many assessors do you have?', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-26-CC-20-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-26-CC-20-SL-1', '', 'Number of assessors', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-26-CC-20-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-26-CC-20-QB-1', '', '', 'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-26-CC-20-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-26-CC-20-H-1', '', 'This should include invigilators where the end-point assessment is an examination', 'Live', GETUTCDATE(), 'Import')

      -- page 26 question 21
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-26-CC-21-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-26-CC-21-L-1', '', 'What''s the volume of end-point assessments you can deliver?', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-26-CC-21-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-26-CC-21-SL-1', '', 'Assessment capacity',		 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-26-CC-21-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-26-CC-21-QB-1', '',N'',  'Live', GETUTCDATE(), 'I	mport')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-26-CC-21-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-26-CC-21-H-1', '', '', 'Live', GETUTCDATE(), 'Import')


      -- page 26 question 22
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-26-CC-22-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-26-CC-22-L-1', '', 'How will the volume of end-point assessments be achieved with the number of assessors you have?', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-26-CC-22-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-26-CC-22-SL-1', '', 'How will the volume of end-point assessments be achieved with the number of assessors you have?',		 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-26-CC-22-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-26-CC-22-QB-1', '', '',  'Live', GETUTCDATE(), 'I	mport')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-26-CC-22-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-26-CC-22-H-1', '', '', 'Live', GETUTCDATE(), 'Import')


      -- page 27 question 23
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-27-CC-23-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-27-CC-23-L-1', '', 'How do you recruit and train assessors?', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-27-CC-23-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-27-CC-23-SL-1', '', 'Recruitment and training','Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-27-CC-23-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-27-CC-23-QB-1', '',N'<p class="govuk-body">All assessors must be qualified to undertake assessments in line with the requirements laid out in the assessment plan.</p><p class="govuk-body">They must have expertise and experience in designing and developing assessment products and tools where this is a requirement of the assessment plan</p>',  'Live', GETUTCDATE(), 'I	mport')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-27-CC-23-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-27-CC-23-H-1', '', '', 'Live', GETUTCDATE(), 'Import')


      -- page 27 question 24
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-27-CC-24-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-27-CC-24-L-1', '', 'What experience, skills and qualifications do your assessors have?', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-27-CC-24-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-27-CC-24-SL-1', '', 'Skills and qualifications','Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-27-CC-24-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-27-CC-24-QB-1', '',N'<p class="govuk-body">You need to give examples of how, when and where the assessor has demonstrated their capability. Assessors must have current and relevant occupational and assessment experience. Give details, evidence and context for this experience of current and future assessors.</p>',  'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-27-CC-24-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-27-CC-24-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

      -- page 27 question 25
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-27-CC-25-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-27-CC-25-L-1', '', 'How do you ensure your assessors'' occupational expertise is maintained and kept current? ', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-27-CC-25-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-27-CC-25-SL-1', '', 'Continuous professional development','Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-27-CC-25-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-27-CC-25-QB-1', '',N'<p class="govuk-body">Give examples, of up to 500 words, for current professional development and recent experience of where and how they have demonstrated their suitability.</p><p class="govuk-body">Give details, evidence and context for this experience, for the assessors you have now or will have in place by the time you start delivery.</p>',  'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-27-CC-25-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-27-CC-25-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

      -- page 28 question 26
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-26-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-26-L-1', '', 'How will you conduct an end-point assessment for this standard?', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-26-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-26-SL-1', '', 'End-point assessment delivery model'	,'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-26-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-26-QB-1', '',N'',  'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-26-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-26-H-1', '', '', 'Live', GETUTCDATE(), 'Import')


-- page 28 question 27
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-27-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-27-L-1', '', 'Do you intend to outsource any of your end-point assessments?', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-27-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-27-SL-1', '', 'Outsourcing of end-point assessments'	,'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-27-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-27-QB-1', '',N'',  'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-27-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-27-H-1', '', '', 'Live', GETUTCDATE(), 'Import')


  -- page 28 question 28
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-28-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-28-L-1', '', 'Quality assurance of outsourced services', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-28-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-28-SL-1', '', 'Quality assurance of outsourced services'	,'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-28-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-28-QB-1', '',N'<p class="govuk-body">Give details of your procedures to obtain assurance in respect to the quality of the occupational  and assessment capacity of the outsourced services?</p>',  'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-28-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-28-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

  -- page 28 question 29
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-29-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-29-L-1', '', 'How will you engage with employers and training organisations?', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-29-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-29-SL-1', '', 'How will you engage with employers and training organisations?'	,'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-29-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-29-QB-1', '', '',  'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-29-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-29-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

  -- page 28 question 30
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-30-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-30-L-1', '', 'How will you manage any potential conflict of interest, particular to other functions your organisation may have?', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-30-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-30-SL-1', '', 'How will you manage any potential conflict of interest, particular to other functions your organisation may have?'	,'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-30-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-30-QB-1', '', '',  'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-30-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-30-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

  -- page 28 question 31
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-31-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-31-L-1', '', 'Where will you conduct end-point assessments?', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-31-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-31-SL-1', '', 'Assessment locations'	,'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-31-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-31-QB-1', '',N'',  'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-31-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-31-H-1', '', '', 'Live', GETUTCDATE(), 'Import')


  -- page 28 question 32
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-32-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-32-SL-1', '', 'Assessment methods'	,'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-32-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-32-L-1', '', 'How will you conduct end-point assessments?', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-32-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-32-QB-1', '',N'',  'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-32-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-32-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

  -- page 28 question 33
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-33-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-33-SL-1', '', 'How will you develop and maintain the required resources and assessment tools?'	,'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-33-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-33-L-1', '', 'How will you develop and maintain the required resources and assessment tools?', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-33-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-33-QB-1', '', '',  'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-28-CC-33-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-28-CC-33-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

  -- page 29 question 34
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-34-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-34-SL-1', '', 'Secure IT infrastructure'	,'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-34-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-34-L-1', '', 'Secure IT infrastructure', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-34-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-34-QB-1', '',N'<p class="govuk-body">Give full details of the secure IT infrastructure you will implement before providing a complete end-point assessment.</p>',  'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-34-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-34-H-1', '', '', 'Live', GETUTCDATE(), 'Import')


  -- page 29 question 35
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-35-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-35-SL-1', '', 'Assessment administration'	,'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-35-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-35-L-1', '', 'Assessment administration', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-35-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-35-QB-1', '',N'<p class="govuk-body">Give full details of processes in place for administration of assessments before providing a complete end-point assessment.</p>',  'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-35-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-35-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

  -- page 29 question 36
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-36-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-36-SL-1', '', 'Assessment products and tools'	,'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-36-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-36-L-1', '', 'Assessment products and tools', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-36-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-36-QB-1', '',N'<p class="govuk-body">Give full details of the strategies in place for development of assessment products and tools.</p>',  'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-36-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-36-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

  -- page 29 question 36
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-36-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-36-SL-1', '', 'Assessment products and tools'	,'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-36-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-36-L-1', '', 'Assessment products and tools', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-36-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-36-QB-1', '',N'<p class="govuk-body">Give full details of the strategies in place for development of assessment products and tools.</p>',  'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-36-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-36-H-1', '', '', 'Live', GETUTCDATE(), 'Import')


    -- page 29 question 37
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-37-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-37-SL-1', '', 'Assessment content'	,'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-37-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-37-L-1', '', 'Assessment content', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-37-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-37-QB-1', '',N'<p class="govuk-body">Give full details of the actions you will take and the processes you will implement as part of delivering a complete end-point assessment.</p>',  'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-37-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-37-H-1', '', '', 'Live', GETUTCDATE(), 'Import')


      -- page 29 question 38
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-38-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-38-SL-1', '', 'Collation & confirmation of assessment outcomes'	,'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-38-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-38-L-1', '', 'Collation & confirmation of assessment outcomes', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-38-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-38-QB-1', '',N'<p class="govuk-body">Give full details of how you collate and confirm assessment outcomes to employers, training providers and apprentices</p>',  'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-38-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-38-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

      -- page 29 question 39
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-39-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-39-SL-1', '', 'Recording assessment results'	,'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-39-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-39-L-1', '', 'Recording assessment results', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-39-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-39-QB-1', '',N'<p class="govuk-body">Give full details of the processes in place for recording and issuing assessment results and certificates.</p>',  'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-29-CC-39-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-29-CC-39-H-1', '', '', 'Live', GETUTCDATE(), 'Import')


      -- page 30 question 40
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-30-CC-40-SL-1'; -- Row J
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-30-CC-40-SL-1', '', 'Web address'	,'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-30-CC-40-L-1';  -- Row L
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-30-CC-40-L-1', '', 'Enter your web address', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-30-CC-40-QB-1';   -- Row P
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-30-CC-40-QB-1', '',N'',  'Live', GETUTCDATE(), 'Import')
GO
DELETE FROM Assets WHERE Reference = 'SQ-2-SE-4-PG-30-CC-40-H-1';   -- Row AA
INSERT INTO Assets
  (Id, Reference, Type, Text, Status, CreatedAt, CreatedBy)
VALUES
  (NEWID(), 'SQ-2-SE-4-PG-30-CC-40-H-1', '', '', 'Live', GETUTCDATE(), 'Import')

GO
DELETE FROM WorkflowSections where SequenceId = 2 and SectionId = 4
INSERT [dbo].[WorkflowSections]
  ([Id], [WorkflowId], [SequenceId], [SectionId], [QnAData], [Title], [LinkTitle], [Status], [DisplayType], [DisallowedOrgTypes])
VALUES
  (N'b4951ead-ee4a-49f2-a31e-3a658605e32a', N'83b35024-8aef-440d-8f59-8c1cc459c350', 2, 4, N'
{
    "Pages": [
      {
        "PageId": "24",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-24-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-24-LT-1",
        "InfoText": "SQ-2-SE-4-PG-24-IT-1",
        "Questions": [
		  {
            "QuestionId": "CC-01",
            "Label": "SQ-2-SE-4-PG-24-CC-01-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-01-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-01-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-01-H-1",
            "Input": {
              "Type": "text",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter your ICO registration number"
                        }
                      ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
			"ReturnId": "25",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-24-BT-1"
      },
	   {
        "PageId": "25",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-24-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-25-LT-1",
        "InfoText": "SQ-2-SE-4-PG-24-IT-1",
        "Questions": [
		  {
            "QuestionId": "CC-02",
            "Label": "SQ-2-SE-4-PG-24-CC-02-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-02-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-02-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-02-H-1",
            "Input": {
              "Type": "FileUpload",
              "Options": null,
              "Validations": [
                {
                  "Name": "Required",
                  "Value": null,
                  "ErrorMessage": "Select an internal audit policy document"
                },
                {
                  "Name": "FileType",
                  "Value": "pdf,application/pdf",
                  "ErrorMessage": "The selected file must be a PDF"
                }
              ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
            "Action": "NextPage",
			"ReturnId": "26",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-24-BT-1"
      },
	   {
        "PageId": "26",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-24-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-26-LT-1",
        "InfoText": "SQ-2-SE-4-PG-24-IT-1",
        "Questions": [
		 {
            "QuestionId": "CC-03",
            "Label": "SQ-2-SE-4-PG-24-CC-03-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-03-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-03-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-03-H-1",
            "Input": {
              "Type": "FileUpload",
              "Options": null,
              "Validations": [
                {
                  "Name": "Required",
                  "Value": null,
                  "ErrorMessage": "Select a public liability certificate of insurance"
                },
                {
                  "Name": "FileType",
                  "Value": "pdf,application/pdf",
                  "ErrorMessage": "The selected file must be a PDF"
                }
              ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
            "Action": "NextPage",
			"ReturnId": "27",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-24-BT-1"
      },
	   {
        "PageId": "27",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-24-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-27-LT-1",
        "InfoText": "SQ-2-SE-4-PG-24-IT-1",
        "Questions": [
		 {
            "QuestionId": "CC-04",
            "Label": "SQ-2-SE-4-PG-24-CC-04-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-04-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-04-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-04-H-1",
            "Input": {
              "Type": "FileUpload",
              "Options": null,
              "Validations": [
                {
                  "Name": "Required",
                  "Value": null,
                  "ErrorMessage": "Select a professional indemnity certificate of insurance"
                },
                {
                  "Name": "FileType",
                  "Value": "pdf,application/pdf",
                  "ErrorMessage": "The selected file must be a PDF"
                }
              ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
            "Action": "NextPage",
			      "ReturnId": "28",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-24-BT-1"
      },
	   {
        "PageId": "28",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-24-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-28-LT-1",
        "InfoText": "SQ-2-SE-4-PG-24-IT-1",
        "Questions": [
		 {
            "QuestionId": "CC-05",
            "Label": "SQ-2-SE-4-PG-24-CC-05-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-05-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-05-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-05-H-1",
            "Input": {
              "Type": "FileUpload",
              "Options": null,
              "Validations": [
                {
                  "Name": "FileType",
                  "Value": "pdf,application/pdf",
                  "ErrorMessage": "The selected file must be a PDF"
                }
              ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
            "Action": "NextPage",
            "ReturnId": "29",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-24-BT-1"
      },
	   {
        "PageId": "29",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-24-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-29-LT-1",
        "InfoText": "SQ-2-SE-4-PG-24-IT-1",
        "Questions": [
		{
            "QuestionId": "CC-06",
            "Label": "SQ-2-SE-4-PG-24-CC-06-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-06-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-06-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-06-H-1",
            "Input": {
              "Type": "FileUpload",
              "Options": null,
              "Validations": [
                {
                  "Name": "Required",
                  "Value": null,
                  "ErrorMessage": "Select a safeguarding policy document"
                },
                {
                  "Name": "FileType",
                  "Value": "pdf,application/pdf",
                  "ErrorMessage": "The selected file must be a PDF"
                }
              ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "30",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-24-BT-1"
      },
	   {
        "PageId": "30",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-24-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-30-LT-1",
        "InfoText": "SQ-2-SE-4-PG-24-IT-1",
        "Questions": [
		{
            "QuestionId": "CC-07",
            "Label": "SQ-2-SE-4-PG-24-CC-07-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-07-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-07-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-07-H-1",
            "Input": {
              "Type": "FileUpload",
              "Options": null,
              "Validations": [
                {
                  "Name": "Required",
                  "Value": null,
                  "ErrorMessage": "Select a prevent agenda policy document"
                },
                {
                  "Name": "FileType",
                  "Value": "pdf,application/pdf",
                  "ErrorMessage": "The selected file must be a PDF"
                }
              ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "31",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-24-BT-1"
      },
	   {
        "PageId": "31",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-24-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-31-LT-1",
        "InfoText": "SQ-2-SE-4-PG-24-IT-1",
        "Questions": [          
		{
            "QuestionId": "CC-08",
            "Label": "SQ-2-SE-4-PG-24-CC-08-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-08-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-08-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-08-H-1",
            "Input": {
              "Type": "FileUpload",
              "Options": null,
              "Validations": [
                {
                  "Name": "Required",
                  "Value": null,
                  "ErrorMessage": "Select a conflict of interest policy document"
                },
                {
                  "Name": "FileType",
                  "Value": "pdf,application/pdf",
                  "ErrorMessage": "The selected file must be a PDF"
                }
              ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "32",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-24-BT-1"
      },
	   {
        "PageId": "32",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-24-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-32-LT-1",
        "InfoText": "SQ-2-SE-4-PG-24-IT-1",
        "Questions": [          
		      {
            "QuestionId": "CC-09",
            "Label": "SQ-2-SE-4-PG-24-CC-09-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-09-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-09-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-09-H-1",
            "Input": {
              "Type": "FileUpload",
              "Options": null,
              "Validations": [
                {
                  "Name": "Required",
                  "Value": null,
                  "ErrorMessage": "Select a file describing your procedures for monitoring assessor practices and decisions"
                },
                {
                  "Name": "FileType",
                  "Value": "pdf,application/pdf",
                  "ErrorMessage": "The selected file must be a PDF"
                }
              ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "33",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-24-BT-1"
      },
	   {
        "PageId": "33",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-24-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-33-LT-1",
        "InfoText": "SQ-2-SE-4-PG-24-IT-1",
        "Questions": [          
		      {
            "QuestionId": "CC-10",
            "Label": "SQ-2-SE-4-PG-24-CC-10-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-10-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-10-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-10-H-1",
            "Input": {
              "Type": "FileUpload",
              "Options": null,
              "Validations": [
                {
                  "Name": "Required",
                  "Value": null,
                  "ErrorMessage": "Select an file describing your standardisation and moderation activities"
                },
                {
                  "Name": "FileType",
                  "Value": "pdf,application/pdf",
                  "ErrorMessage": "The selected file must be a PDF"
                }
              ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "34",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-24-BT-1"
      },
	   {
        "PageId": "34",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-24-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-34-LT-1",
        "InfoText": "SQ-2-SE-4-PG-24-IT-1",
        "Questions": [          
		 {
            "QuestionId": "CC-11",
            "Label": "SQ-2-SE-4-PG-24-CC-11-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-11-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-11-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-11-H-1",
            "Input": {
              "Type": "FileUpload",
              "Options": null,
              "Validations": [
                {
                  "Name": "Required",
                  "Value": null,
                  "ErrorMessage": "Select a compalints and appeals policy document"
                },
                {
                  "Name": "FileType",
                  "Value": "pdf,application/pdf",
                  "ErrorMessage": "The selected file must be a PDF"
                }
              ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "340",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-24-BT-1"
      },
	   {
        "PageId": "340",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-24-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-340-LT-1",
        "InfoText": "SQ-2-SE-4-PG-24-IT-1",
        "Questions": [          
		 {
            "QuestionId": "CC-12",
            "Label": "SQ-2-SE-4-PG-24-CC-12-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-12-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-12-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-12-H-1",
            "Input": {
              "Type": "FileUpload",
              "Options": null,
              "Validations": [
                {
                  "Name": "Required",
                  "Value": null,
                  "ErrorMessage": "Select a fair access policy document"
                },
                {
                  "Name": "FileType",
                  "Value": "pdf,application/pdf",
                  "ErrorMessage": "The selected file must be a PDF"
                }
              ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "35",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-24-BT-1"
      },
	   {
        "PageId": "35",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-24-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-35-LT-1",
        "InfoText": "SQ-2-SE-4-PG-24-IT-1",
        "Questions": [          
		  {
            "QuestionId": "CC-13",
            "Label": "SQ-2-SE-4-PG-24-CC-13-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-13-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-13-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-13-H-1",
            "Input": {
              "Type": "FileUpload",
              "Options": null,
              "Validations": [
                {
                  "Name": "Required",
                  "Value": null,
                  "ErrorMessage": "Select a file describing your comparability and consistency decisions"
                },
                {
                  "Name": "FileType",
                  "Value": "pdf,application/pdf",
                  "ErrorMessage": "The selected file must be a PDF"
                }
              ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "36",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-24-BT-1"
      },
	   {
        "PageId": "36",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-24-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-36-LT-1",
        "InfoText": "SQ-2-SE-4-PG-24-IT-1",
        "Questions": [          
		 {
            "QuestionId": "CC-14",
            "Label": "SQ-2-SE-4-PG-24-CC-14-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-14-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-14-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-14-H-1",
            "Input": {
              "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter how you continuously improve the quality of your assessment practice"
                        }
                      ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "37",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-24-BT-1"
      },
	  {
        "PageId": "37",   
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-25-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-37-LT-1",
        "InfoText": "SQ-2-SE-4-PG-25-IT-1",
        "Questions": [
		  {
            "QuestionId": "CC-16",
            "Label": "SQ-2-SE-4-PG-25-CC-16-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-25-CC-16-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-25-CC-16-QB-1",
            "Hint": "SQ-2-SE-4-PG-25-CC-16-H-1",
            "Input": {
              "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter your evidence of engagement with trailblazers and employers"
                        }
                      ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "38",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-25-BT-1"
      },
	  {
        "PageId": "38",   
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-25-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-38-LT-1",
        "InfoText": "SQ-2-SE-4-PG-25-IT-1",
        "Questions": [
		  {
            "QuestionId": "CC-19",
            "Label": "SQ-2-SE-4-PG-25-CC-19-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-25-CC-19-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-25-CC-19-QB-1",
            "Hint": "SQ-2-SE-4-PG-25-CC-19-H-1",
            "Input": {
              "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter details of membership to professional organisations"
                        }
                      ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
            "Action": "NextPage",
            "ReturnId": "39",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-25-BT-1"
      },
	  {
        "PageId": "39",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-26-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-39-LT-1",
        "InfoText": "SQ-2-SE-4-PG-26-IT-1",
        "Questions": [
		  {
            "QuestionId": "CC-20",
            "Label": "SQ-2-SE-4-PG-26-CC-20-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-26-CC-20-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-26-CC-20-QB-1",
            "Hint": "SQ-2-SE-4-PG-26-CC-20-H-1",
            "Input": {
              "Type": "text",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Provide the number of assessors your organisation has"
                        }
                      ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "40",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-26-BT-1"
      },
	  {
        "PageId": "40",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-26-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-40-LT-1",
        "InfoText": "SQ-2-SE-4-PG-26-IT-1",
        "Questions": [
		  {
            "QuestionId": "CC-21",
            "Label": "SQ-2-SE-4-PG-26-CC-21-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-26-CC-21-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-26-CC-21-QB-1",
            "Hint": "SQ-2-SE-4-PG-26-CC-21-H-1",
            "Input": {
              "Type": "text",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter the volume of end-point assessments you can deliver"
                        }
                      ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "41",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-26-BT-1"
      },
	  {
        "PageId": "41",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-26-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-41-LT-1",
        "InfoText": "SQ-2-SE-4-PG-26-IT-1",
        "Questions": [
		  {
            "QuestionId": "CC-22",
            "Label": "SQ-2-SE-4-PG-26-CC-22-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-26-CC-22-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-26-CC-22-QB-1",
            "Hint": "SQ-2-SE-4-PG-26-CC-22-H-1",
            "Input": {
              "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter how the volume of end-point assessments be achieved"
                        }
                      ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "42",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-26-BT-1"
      },
	  {
        "PageId": "42",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-27-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-42-LT-1",
        "InfoText": "SQ-2-SE-4-PG-27-IT-1",
        "Questions": [
		  {
            "QuestionId": "CC-23",
            "Label": "SQ-2-SE-4-PG-27-CC-23-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-27-CC-23-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-27-CC-23-QB-1",
            "Hint": "SQ-2-SE-4-PG-27-CC-23-H-1",
            "Input": {
              "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter how you recruit and train assessors"
                        }
                      ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "43",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-27-BT-1"
      },
	  {
        "PageId": "43",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-27-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-43-LT-1",
        "InfoText": "SQ-2-SE-4-PG-27-IT-1",
        "Questions": [
		  {
            "QuestionId": "CC-24",
            "Label": "SQ-2-SE-4-PG-27-CC-24-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-27-CC-24-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-27-CC-24-QB-1",
            "Hint": "SQ-2-SE-4-PG-27-CC-24-H-1",
            "Input": {
              "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter the experience, skills and qualifications your assessors have"
                        }
                      ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "44",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-27-BT-1"
      },
	  {
        "PageId": "44",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-27-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-44-LT-1",
        "InfoText": "SQ-2-SE-4-PG-27-IT-1",
        "Questions": [
		   {
            "QuestionId": "CC-25",
            "Label": "SQ-2-SE-4-PG-27-CC-25-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-27-CC-25-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-27-CC-25-QB-1",
            "Hint": "SQ-2-SE-4-PG-27-CC-25-H-1",
            "Input": {
              "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter how you ensure your assessors'' occupational expertise is maintained"
                        }
                      ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "45",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-27-BT-1"
      },
	  {
        "PageId": "45",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-28-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-45-LT-1",
        "InfoText": "SQ-2-SE-4-PG-28-IT-1",
        "Questions": [
		  {
            "QuestionId": "CC-26",
            "Label": "SQ-2-SE-4-PG-28-CC-26-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-28-CC-26-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-28-CC-26-QB-1",
            "Hint": "SQ-2-SE-4-PG-28-CC-26-H-1",
            "Input": {
              "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter how you will conduct an end-point assessment for this standard"
                        }
                      ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "46",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-28-BT-1"
      },
	  {
        "PageId": "46",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-28-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-46-LT-1",
        "InfoText": "SQ-2-SE-4-PG-28-IT-1",
        "Questions": [
		  {
            "QuestionId": "CC-27",
            "Label": "SQ-2-SE-4-PG-28-CC-27-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-28-CC-27-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-28-CC-27-QB-1",
            "Hint": "SQ-2-SE-4-PG-28-CC-27-H-1",
"Input": {
            "Type": "ComplexRadio",
            "Options": [
              {
                "Label": "Yes",
                "Value": "Yes",
                "FurtherQuestions": [
                  {
                    "QuestionId": "CC-27.1",
                    "Label": "SQ-2-SE-4-PG-28-CC-28-L-1",
                    "Hint":  "SQ-2-SE-4-PG-28-CC-28-H-1",
                    "Input": {
                      "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter the quality assurance of outsourced services"
                        }
                      ]
                    },
                    "Order": null,
                    "ShortLabel": "SQ-1-SE-1-PG-28-CD-28-SL-1",
                    "QuestionBodyText": "SQ-1-SE-1-PG-28-CD-28-QB-1"
                  }
                ]
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select yes if you intend to outsource any of your end-point assessments"
              }
            ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "47",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-28-BT-1"
      },
	  {
        "PageId": "47",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-28-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-47-LT-1",
        "InfoText": "SQ-2-SE-4-PG-28-IT-1",
        "Questions": [
		  {
            "QuestionId": "CC-29",
            "Label": "SQ-2-SE-4-PG-28-CC-29-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-28-CC-29-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-28-CC-29-QB-1",
            "Hint": "SQ-2-SE-4-PG-28-CC-29-H-1",
            "Input": {
              "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter how you will engage with employers and training organisations"
                        }
                      ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "48",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-28-BT-1"
      },
	  {
        "PageId": "48",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-28-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-48-LT-1",
        "InfoText": "SQ-2-SE-4-PG-28-IT-1",
        "Questions": [
		  {
            "QuestionId": "CC-30",
            "Label": "SQ-2-SE-4-PG-28-CC-30-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-28-CC-30-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-28-CC-30-QB-1",
            "Hint": "SQ-2-SE-4-PG-28-CC-30-H-1",
            "Input": {
              "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter how you will manage any potential conflict of interest"
                        }
                      ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "49",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-28-BT-1"
      },
	  {
        "PageId": "49",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-28-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-49-LT-1",
        "InfoText": "SQ-2-SE-4-PG-28-IT-1",
        "Questions": [
		  {
            "QuestionId": "CC-31",
            "Label": "SQ-2-SE-4-PG-28-CC-31-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-28-CC-31-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-28-CC-31-QB-1",
            "Hint": "SQ-2-SE-4-PG-28-CC-31-H-1",
            "Input": {
              "Type": "DataFed_CheckboxList",
              "Options": null,
              "Validations": [
                {
                  "Name": "Required",
                  "Value": null,
                  "ErrorMessage": "Enter where you will conduct end-point assessments"
                }
              ],
              "DataEndpoint":"DeliveryAreas"
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "50",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-28-BT-1"
      },
	  {
        "PageId": "50",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-28-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-50-LT-1",
        "InfoText": "SQ-2-SE-4-PG-28-IT-1",
        "Questions": [
		  {
            "QuestionId": "CC-32",
            "Label": "SQ-2-SE-4-PG-28-CC-32-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-28-CC-32-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-28-CC-32-QB-1",
            "Hint": "SQ-2-SE-4-PG-28-CC-32-H-1",
            "Input": {
              "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter how you will conduct end-point assessments"
                        }
                      ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "51",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-28-BT-1"
      },
	  {
        "PageId": "51",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-28-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-51-LT-1",
        "InfoText": "SQ-2-SE-4-PG-28-IT-1",
        "Questions": [
		  {
            "QuestionId": "CC-33",
            "Label": "SQ-2-SE-4-PG-28-CC-33-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-28-CC-33-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-28-CC-33-QB-1",
            "Hint": "SQ-2-SE-4-PG-28-CC-33-H-1",
            "Input": {
              "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter how you will develop and maintain the required resources and assessment tools"
                        }
                      ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "52",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-28-BT-1"
      },
	  {
        "PageId": "52",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-29-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-52-LT-1",
        "InfoText": "SQ-2-SE-4-PG-29-IT-1",
        "Questions": [
		  {
            "QuestionId": "CC-34",
            "Label": "SQ-2-SE-4-PG-29-CC-34-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-29-CC-34-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-29-CC-34-QB-1",
            "Hint": "SQ-2-SE-4-PG-29-CC-34-H-1",
            "Input": {
              "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter details of the secure IT infrastructure you will implement"
                        }
                      ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "53",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-29-BT-1"
      },
	  {
        "PageId": "53",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-29-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-53-LT-1",
        "InfoText": "SQ-2-SE-4-PG-29-IT-1",
        "Questions": [
		 {
            "QuestionId": "CC-35",
            "Label": "SQ-2-SE-4-PG-29-CC-35-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-29-CC-35-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-29-CC-35-QB-1",
            "Hint": "SQ-2-SE-4-PG-29-CC-35-H-1",
            "Input": {
              "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter details of processes in place for administration of assessments"
                        }
                      ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "54",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-29-BT-1"
      },
	  {
        "PageId": "54",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-29-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-54-LT-1",
        "InfoText": "SQ-2-SE-4-PG-29-IT-1",
        "Questions": [
		{
            "QuestionId": "CC-36",
            "Label": "SQ-2-SE-4-PG-29-CC-36-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-29-CC-36-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-29-CC-36-QB-1",
            "Hint": "SQ-2-SE-4-PG-29-CC-36-H-1",
            "Input": {
              "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter the strategies in place for development of assessment products and tools"
                        }
                      ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "55",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-29-BT-1"
      },
	  {
        "PageId": "55",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-29-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-55-LT-1",
        "InfoText": "SQ-2-SE-4-PG-29-IT-1",
        "Questions": [
		{
            "QuestionId": "CC-37",
            "Label": "SQ-2-SE-4-PG-29-CC-37-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-29-CC-37-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-29-CC-37-QB-1",
            "Hint": "SQ-2-SE-4-PG-29-CC-37-H-1",
            "Input": {
              "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter the actions you will take and the processes you will implement"
                        }
                      ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "56",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-29-BT-1"
      },
	  {
        "PageId": "56",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-29-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-56-LT-1",
        "InfoText": "SQ-2-SE-4-PG-29-IT-1",
        "Questions": [
		{
            "QuestionId": "CC-38",
            "Label": "SQ-2-SE-4-PG-29-CC-38-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-29-CC-38-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-29-CC-38-QB-1",
            "Hint": "SQ-2-SE-4-PG-29-CC-38-H-1",
            "Input": {
              "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter details of how you collate and confirm assessment outcomes to employers, training providers and apprentices"
                        }
                      ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "57",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-29-BT-1"
      },
	  {
        "PageId": "57",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-29-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-57-LT-1",
        "InfoText": "SQ-2-SE-4-PG-29-IT-1",
        "Questions": [
		{
            "QuestionId": "CC-39",
            "Label": "SQ-2-SE-4-PG-29-CC-39-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-29-CC-39-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-29-CC-39-QB-1",
            "Hint": "SQ-2-SE-4-PG-29-CC-39-H-1",
            "Input": {
              "Type": "Textarea",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter details of the processes in place for recording and issuing assessment results and certificates"
                        }
                      ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "NextPage",
            "ReturnId": "58",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-29-BT-1"
      },
	  {
        "PageId": "58",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-30-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-58-LT-1",
        "InfoText": "SQ-2-SE-4-PG-30-IT-1",
        "Questions": [
		  {
            "QuestionId": "CC-40",
            "Label": "SQ-2-SE-4-PG-30-CC-40-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-30-CC-40-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-30-CC-40-QB-1",
            "Hint": "SQ-2-SE-4-PG-30-CC-40-H-1",
            "Input": {
              "Type": "text",
                      "Options": null,
                      "Validations": []
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
             "Action": "ReturnToSection",
            "ReturnId": "4",
            "Condition": null,
            "ConditionMet": false
          }
        ],
        "Complete": false,
        "AllowMultipleAnswers": false,
        "Order": null,
        "Active": true,
        "Visible": true,
        "Feedback": null,
        "HasFeedback": false,
        "BodyText": "SQ-2-SE-4-PG-30-BT-1"
      }
    ],
    "FinancialApplicationGrade": null
  }  
', N'Apply to assess a standard', N'Apply to assess a standard', N'Draft', N'PagesWithSections', N'')
GO
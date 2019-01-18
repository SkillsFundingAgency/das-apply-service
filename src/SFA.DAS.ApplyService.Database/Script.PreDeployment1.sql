IF EXISTS(SELECT * FROM sys.objects o INNER JOIN sys.columns c ON o.object_id = c.object_id WHERE o.name = 'ApplicationSequences' AND c.name = 'SequenceData')
ALTER TABLE dbo.ApplicationSequences DROP COLUMN SequenceData
GO
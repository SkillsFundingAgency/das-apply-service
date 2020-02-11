/* Add pre Deployment SQL here */

-- APR-1176 : Clear out old Apply tables. They'll no longer be required
IF OBJECT_ID('dbo.ApplicationWorkflow', 'U') IS NOT NULL 
  DELETE FROM dbo.ApplicationWorkflow; 

IF OBJECT_ID('dbo.ApplicationSequences', 'U') IS NOT NULL 
  DELETE FROM dbo.ApplicationSequences; 

IF OBJECT_ID('dbo.ApplicationSections', 'U') IS NOT NULL 
  DELETE FROM dbo.ApplicationSections;

IF OBJECT_ID('dbo.Applications', 'U') IS NOT NULL 
  DELETE FROM dbo.Applications;

IF OBJECT_ID('dbo.WorkflowSequences', 'U') IS NOT NULL 
  DELETE FROM dbo.WorkflowSequences;

IF OBJECT_ID('dbo.WorkflowSections', 'U') IS NOT NULL 
  DELETE FROM dbo.WorkflowSections;

IF OBJECT_ID('dbo.Workflows', 'U') IS NOT NULL 
  DELETE FROM dbo.WorkflowSections;

IF OBJECT_ID('dbo.Assets', 'U') IS NOT NULL 
  DELETE FROM dbo.Assets; 

IF OBJECT_ID('dbo.QnAs', 'U') IS NOT NULL 
  DELETE FROM dbo.QnAs; 

IF OBJECT_ID('dbo.Entities', 'U') IS NOT NULL 
  DELETE FROM dbo.Entities; 

GO 

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'  AND  TABLE_NAME = 'ApplicationWorkflow'))
    DROP TABLE ApplicationWorkflow;
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'  AND  TABLE_NAME = 'ApplicationSequences'))
    DROP TABLE ApplicationSequences;
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'  AND  TABLE_NAME = 'ApplicationSections'))
    DROP TABLE ApplicationSections;
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Applications'))
    DROP TABLE Applications;
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'WorkflowSequences'))
    DROP TABLE WorkflowSequences;
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'WorkflowSections'))
    DROP TABLE WorkflowSections;
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Workflows'))
    DROP TABLE Workflows;
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Assets'))
    DROP TABLE Assets;
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'QnAs'))
    DROP TABLE QnAs;
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Entities'))
    DROP TABLE Entities;


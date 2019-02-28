/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

-- cleanup (for now)
--DELETE FROM [dbo].[EmailTemplates]

-- IF NOT EXISTS( SELECT * FROM [dbo].[EmailTemplates] WHERE [TemplateId] = '2ebc498c-2544-42db-b73e-a6c381c614df')
-- BEGIN
-- 	INSERT INTO [dbo].[EmailTemplates] ([Id], [Status], [TemplateName], [TemplateId], [CreatedAt], [CreatedBy])
-- 	VALUES (NEWID(), 'Live', 'ApplyEPAOResponse', '2ebc498c-2544-42db-b73e-a6c381c614df', GETDATE(), 'System')
-- END
-- GO
-- 
-- IF NOT EXISTS( SELECT * FROM [dbo].[EmailTemplates] WHERE [TemplateId] = 'acf63bea-41ff-4a45-a376-9d557c30bca0')
-- BEGIN
-- 	INSERT INTO [dbo].[EmailTemplates] ([Id], [Status], [TemplateName], [TemplateId], [CreatedAt], [CreatedBy])
-- 	VALUES (NEWID(), 'Live', 'ApplyEPAOUpdate', 'acf63bea-41ff-4a45-a376-9d557c30bca0', GETDATE(), 'System')
-- END
-- GO
-- 
-- IF NOT EXISTS( SELECT * FROM [dbo].[EmailTemplates] WHERE [TemplateId] = '88799189-fe12-4887-a13f-f7f76cd6945a')
-- BEGIN
-- 	INSERT INTO [dbo].[EmailTemplates] ([Id], [Status], [TemplateName], [TemplateId], [CreatedAt], [CreatedBy])
-- 	VALUES (NEWID(), 'Live', 'ApplySignupError', '88799189-fe12-4887-a13f-f7f76cd6945a', GETDATE(), 'System')
-- END
-- GO
-- 
-- IF NOT EXISTS( SELECT * FROM [dbo].[EmailTemplates] WHERE [TemplateId] = '68410850-909b-4669-a60a-f60e4b1cb89f')
-- BEGIN
-- 	INSERT INTO [dbo].[EmailTemplates] ([Id], [Status], [TemplateName], [TemplateId], [CreatedAt], [CreatedBy], [RecipientTemplate])
-- 	VALUES (NEWID(), 'Live', 'ApplyEPAOInitialSubmission', '68410850-909b-4669-a60a-f60e4b1cb89f', GETDATE(), 'System', 'ApplyEPAOAlertSubmission')
-- END
-- GO
-- 
-- -- test version to raj.balguri@digital.education.gov.uk live = Post16Non-CollegeFHA@Educationgovuk.onmicrosoft.com
-- IF NOT EXISTS( SELECT * FROM [dbo].[EmailTemplates] WHERE [TemplateId] = 'a56c47c8-6310-4f5c-a3f6-9e996c375557')
-- BEGIN
-- 	INSERT INTO [dbo].[EmailTemplates] ([Id], [Status], [TemplateName], [TemplateId], [Recipients], [CreatedAt], [CreatedBy])
-- 	VALUES (NEWID(), 'Live', 'ApplyEPAOAlertSubmission', 'a56c47c8-6310-4f5c-a3f6-9e996c375557', 'Post16Non-CollegeFHA@Educationgovuk.onmicrosoft.com', GETDATE(), 'System')
-- END
-- GO
-- 
-- IF NOT EXISTS( SELECT * FROM [dbo].[EmailTemplates] WHERE [TemplateId] = 'e0a52c44-10be-4164-9543-3c312769c4e3')
-- BEGIN
-- 	INSERT INTO [dbo].[EmailTemplates] ([Id], [Status], [TemplateName], [TemplateId], [CreatedAt], [CreatedBy])
-- 	VALUES (NEWID(), 'Live', 'ApplyEPAOStandardSubmission', 'e0a52c44-10be-4164-9543-3c312769c4e3', GETDATE(), 'System')
-- END
-- GO
-- 
-- IF NOT EXISTS( SELECT * FROM [dbo].[EmailTemplates] WHERE [TemplateId] = '5bb920f4-06ec-43c7-b00a-8fad33ce8066')
-- BEGIN
-- 	INSERT INTO [dbo].[EmailTemplates] ([Id], [Status], [TemplateName], [TemplateId], [CreatedAt], [CreatedBy])
-- 	VALUES (NEWID(), 'Live', 'EPAOUserApproveRequest', '5bb920f4-06ec-43c7-b00a-8fad33ce8066', GETDATE(), 'System')
-- END
-- GO
-- 
-- IF NOT EXISTS( SELECT * FROM [dbo].[EmailTemplates] WHERE [TemplateId] = '539204f8-e99a-4efa-9d1f-d0e58b26dd7b')
-- BEGIN
-- 	INSERT INTO [dbo].[EmailTemplates] ([Id], [Status], [TemplateName], [TemplateId], [CreatedAt], [CreatedBy])
-- 	VALUES (NEWID(), 'Live', 'EPAOUserApproveConfirm', '539204f8-e99a-4efa-9d1f-d0e58b26dd7b', GETDATE(), 'System')
-- END
-- GO
-- 
-- IF NOT EXISTS( select * from sys.sequences where object_id = object_id('AppRefSequence'))
-- BEGIN
-- 	CREATE SEQUENCE [dbo].[AppRefSequence]  AS [int] START WITH 100001 INCREMENT BY 1 MINVALUE 100000 MAXVALUE 2147483647 CYCLE  CACHE 
-- END
-- GO
-- 
-- UPDATE ApplicationSections
-- SET QnAData = JSON_MODIFY(QnAData, '$.FinancialApplicationGrade.SelectedGrade', 'Outstanding')
-- WHERE  JSON_VALUE(QnAData, '$.FinancialApplicationGrade.SelectedGrade') = 'Excellent'
-- GO

-- Add the Workflows
:r ..\WorkflowLatest.sql



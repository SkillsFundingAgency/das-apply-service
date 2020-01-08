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

-- AB 02/04/19 When coding has been done will need to swapout the templateIds for ApplyEPAOUpdate & ApplyEPAOResponse
-- Was acf63bea-41ff-4a45-a376-9d557c30bca0 
IF EXISTS (SELECT * FROM EmailTemplates WHERE TemplateName = N'ApplyEPAOUpdate' AND TemplateId = 'acf63bea-41ff-4a45-a376-9d557c30bca0')
BEGIN
UPDATE EmailTemplates SET TemplateId =  'ffe63c0d-b2b0-461f-b99a-73105d7d5fa3', UpdatedAt = GETDATE(), UpdatedBy ='System'
WHERE TemplateName = N'ApplyEPAOUpdate'
END

IF NOT EXISTS (SELECT * FROM EmailTemplates WHERE TemplateName = N'ApplyEPAOUpdate')
BEGIN
INSERT EmailTemplates ([Id], [Status], [TemplateName],[TemplateId],[CreatedAt],[CreatedBy]) 
VALUES (NEWID(), 'Live', N'ApplyEPAOUpdate', 'ffe63c0d-b2b0-461f-b99a-73105d7d5fa3', GETDATE(), 'System')
END

-- was 2ebc498c-2544-42db-b73e-a6c381c614df
IF EXISTS (SELECT * FROM EmailTemplates WHERE TemplateName = N'ApplyEPAOResponse' AND TemplateId = '2ebc498c-2544-42db-b73e-a6c381c614df')
BEGIN
UPDATE EmailTemplates SET TemplateId =  N'84174eab-f3c1-4274-8670-2fb5b21cbd77', UpdatedAt = GETDATE(), UpdatedBy ='System'
WHERE TemplateName = N'ApplyEPAOResponse'
END

IF NOT EXISTS (SELECT * FROM EmailTemplates WHERE TemplateName = N'ApplyEPAOResponse')
BEGIN
INSERT EmailTemplates ([Id], [Status],[TemplateName],[TemplateId],[CreatedAt],[CreatedBy]) 
VALUES (NEWID(), 'Live', N'ApplyEPAOResponse', N'84174eab-f3c1-4274-8670-2fb5b21cbd77', GETDATE(), 'System')  
END

DELETE FROM EmailTemplates
WHERE [TemplateName] IN ( 'EPAOUserApproveRequest' , 'EPAOUserApproveConfirm' )

IF NOT EXISTS (SELECT * FROM EmailTemplates WHERE TemplateName = N'RoATPGetHelpWithQuestion')
BEGIN
	INSERT INTO EmailTemplates ([Id], [Status],[TemplateName],[TemplateId],[Recipients],[CreatedAt],[CreatedBy]) 
	VALUES (NEWID(), 'Live', N'RoATPGetHelpWithQuestion', N'9d1e1a7e-3557-4781-8901-ea627ae70ec2', N'RoATP.SUPPORT@education.gov.uk', GETDATE(), 'System')
END

IF NOT EXISTS (SELECT * FROM EmailTemplates WHERE TemplateName = N'RoATPApplicationSubmitted')
BEGIN
	INSERT INTO EmailTemplates ([Id], [Status],[TemplateName],[TemplateId],[CreatedAt],[CreatedBy]) 
	VALUES (NEWID(), 'Live', N'RoATPApplicationSubmitted', N'f371098e-4e91-40ae-96da-d6f8d9c85251', GETDATE(), 'System')
END

IF NOT EXISTS (SELECT * FROM EmailTemplates WHERE TemplateName = N'RoATPApplicationSubmittedMain')
BEGIN
	INSERT INTO EmailTemplates ([Id], [Status],[TemplateName],[TemplateId],[CreatedAt],[CreatedBy]) 
	VALUES (NEWID(), 'Live', N'RoATPApplicationSubmittedMain', N'c0008332-7a20-41a4-94d4-57acbcebaef8', GETDATE(), 'System')
END

-- ON-1917 Clean up double spaces on Organisation Name
UPDATE [dbo].[Organisations]
SET	   [Name] = REPLACE([Name], '  ', ' ')
      ,[OrganisationDetails] = JSON_MODIFY([OrganisationDetails], '$.LegalName', REPLACE(JSON_VALUE([OrganisationDetails], '$.LegalName'), '  ', ' '))
WHERE CHARINDEX('  ', [Name]) > 0 OR CHARINDEX('  ', JSON_VALUE([OrganisationDetails], '$.LegalName')) > 0;
-- END OF: ON-1917

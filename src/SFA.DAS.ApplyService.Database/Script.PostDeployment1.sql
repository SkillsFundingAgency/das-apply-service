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
	VALUES (NEWID(), 'Live', N'RoATPGetHelpWithQuestion', N'c84f965c-b4c9-4328-9644-a659037d6a9c', N'RoATP.SUPPORT@education.gov.uk', GETDATE(), 'System')
END

-- START OF: ON-1502 Fixes - Remove once deployed to PROD
/*
UPDATE [ApplicationSections]
   SET [Status] = 'Evaluated'
 WHERE [NotRequired] = 1
GO

UPDATE [ApplicationSections]
   SET [QnAData] = JSON_MODIFY(QnAData, '$.FinancialApplicationGrade', JSON_QUERY('{"SelectedGrade":"Exempt", "GradedDateTime":"' + CONVERT(varchar(30), GETUTCDATE(), 126) + '"}'))
 WHERE [NotRequired] = 1 AND [SectionId] = 3
GO

UPDATE [ApplicationSequences]
   SET [Status] = 'Approved'
 WHERE [NotRequired] = 1 AND [SequenceId] = 1
GO

UPDATE app
   SET app.[ApplicationData] = JSON_MODIFY(app.ApplicationData, '$.InitSubmissionClosedDate', CONVERT(varchar(30), GETUTCDATE(), 126))
 FROM [Applications] app
 INNER JOIN [ApplicationSequences] seq ON app.Id = seq.ApplicationId
 WHERE seq.[NotRequired] = 1 AND seq.[SequenceId] = 1
GO
*/
-- END OF: ON-1502 Fixes - Remove once deployed to PROD


-- ON-1172 updating existing applications to include QuesstionTag against specific questions
/*
exec [Update_ApplicationSections_QuestionTags] 'CD-30', 'trading-name'
exec [Update_ApplicationSections_QuestionTags] 'CD-01', 'use-trading-name'
exec [Update_ApplicationSections_QuestionTags] 'CD-02', 'contact-name'
EXEC [Update_ApplicationSections_QuestionTags] 'CD-03', 'contact-address'
EXEC [Update_ApplicationSections_QuestionTags] 'CD-03_1', 'contact-address1'
exec [Update_ApplicationSections_QuestionTags] 'CD-03_2', 'contact-address2'
exec [Update_ApplicationSections_QuestionTags] 'CD-03_3', 'contact-address3'
exec [Update_ApplicationSections_QuestionTags] 'CD-03_4', 'contact-address4'
EXEC [Update_ApplicationSections_QuestionTags] 'CD-04', 'contact-postcode'
exec [Update_ApplicationSections_QuestionTags] 'CD-05', 'contact-email'
exec [Update_ApplicationSections_QuestionTags] 'CD-06', 'contact-phone-number'
exec [Update_ApplicationSections_QuestionTags] 'CD-12', 'company-ukprn'
exec [Update_ApplicationSections_QuestionTags] 'CD-17', 'company-number'
exec [Update_ApplicationSections_QuestionTags] 'CD-26', 'charity-number'
exec [Update_ApplicationSections_QuestionTags] 'CC-31', 'delivery-areas'
exec [Update_ApplicationSections_QuestionTags] 'CD-40', 'standard-website'
exec [Update_ApplicationSections_QuestionTags] 'CD-03_1', 'contact-address-1'
exec [Update_ApplicationSections_QuestionTags] 'CD-03_2', 'contact-address-2'
exec [Update_ApplicationSections_QuestionTags] 'CD-03_3', 'contact-address-3'
exec [Update_ApplicationSections_QuestionTags] 'CD-03_4', 'contact-address-4'
*/
-- END OF: ON-1172 remove or comment out once deployed to PROD

-- ON-1921 updating existing applications to include standard reference and standard level
:r UpdateApplicationDataStandards-ON-1921.sql
-- END OF: ON-1921 remove or comment out once deployed to PROD

-- ON-1917 Clean up double spaces on Organisation Name
UPDATE [dbo].[Organisations]
SET	   [Name] = REPLACE([Name], '  ', ' ')
      ,[OrganisationDetails] = JSON_MODIFY([OrganisationDetails], '$.LegalName', REPLACE(JSON_VALUE([OrganisationDetails], '$.LegalName'), '  ', ' '))
WHERE CHARINDEX('  ', [Name]) > 0 OR CHARINDEX('  ', JSON_VALUE([OrganisationDetails], '$.LegalName')) > 0;
-- END OF: ON-1917 


-- Add the Workflows
:r ..\WorkflowLatest.sql

:r ..\RoatpWorkflow.sql

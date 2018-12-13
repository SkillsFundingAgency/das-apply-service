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
IF NOT EXISTS( SELECT * FROM [dbo].[EmailTemplates] WHERE [TemplateId] = '2ebc498c-2544-42db-b73e-a6c381c614df')
BEGIN
	INSERT INTO [dbo].[EmailTemplates] ([Id], [Status], [TemplateName], [TemplateId], [Recipients], [CreatedAt], [CreatedBy])
	VALUES (NEWID(), 'Live', 'ApplyEPAOResponse', '2ebc498c-2544-42db-b73e-a6c381c614df', 'epao.helpdesk@education.gov.uk', GETDATE(), 'System')
END
GO

IF NOT EXISTS( SELECT * FROM [dbo].[EmailTemplates] WHERE [TemplateId] = 'acf63bea-41ff-4a45-a376-9d557c30bca0')
BEGIN
	INSERT INTO [dbo].[EmailTemplates] ([Id], [Status], [TemplateName], [TemplateId], [Recipients], [CreatedAt], [CreatedBy])
	VALUES (NEWID(), 'Live', 'ApplyEPAOUpdate', 'acf63bea-41ff-4a45-a376-9d557c30bca0', 'epao.helpdesk@education.gov.uk', GETDATE(), 'System')
END
GO

IF NOT EXISTS( SELECT * FROM [dbo].[EmailTemplates] WHERE [TemplateId] = '88799189-fe12-4887-a13f-f7f76cd6945a')
BEGIN
	INSERT INTO [dbo].[EmailTemplates] ([Id], [Status], [TemplateName], [TemplateId], [Recipients], [CreatedAt], [CreatedBy])
	VALUES (NEWID(), 'Live', 'ApplySignupError', '88799189-fe12-4887-a13f-f7f76cd6945a', 'epao.helpdesk@education.gov.uk', GETDATE(), 'System')
END
GO



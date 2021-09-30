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


--IF NOT EXISTS (SELECT * FROM EmailTemplates WHERE TemplateName = N'RoATPRequestInvitationToReapply')
--BEGIN
--	INSERT INTO EmailTemplates ([Id], [Status], [TemplateName], [TemplateId], [Recipients], [CreatedAt], [CreatedBy]) 
--	VALUES (NEWID(), 'Live', N'RoATPRequestInvitationToReapply', N'872e3ace-4625-4f9b-a909-c44cec7f71ca', N'helpdesk@manage-apprenticeships.service.gov.uk', GETDATE(), 'System')
--END



DELETE FROM EmailTemplates WHERE TemplateName = N'RoATPRequestInvitationToReapply'

	INSERT INTO EmailTemplates ([Id], [Status], [TemplateName], [TemplateId], [Recipients], [CreatedAt], [CreatedBy]) 
	VALUES (NEWID(), 'Live', N'RoATPRequestInvitationToReapply', N'872e3ace-4625-4f9b-a909-c44cec7f71ca', N'mark.cain@digital.education.gov.uk', GETDATE(), 'System')

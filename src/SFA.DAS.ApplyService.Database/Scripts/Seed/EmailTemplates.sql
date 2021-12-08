IF NOT EXISTS (SELECT * FROM EmailTemplates WHERE TemplateName = N'RoATPRequestInvitationToReapply')
BEGIN
	INSERT INTO EmailTemplates ([Id], [Status], [TemplateName], [TemplateId], [Recipients], [CreatedAt], [CreatedBy]) 
	VALUES (NEWID(), 'Live', N'RoATPRequestInvitationToReapply', N'872e3ace-4625-4f9b-a909-c44cec7f71ca', N'helpdesk@manage-apprenticeships.service.gov.uk', GETDATE(), 'System')
END

IF NOT EXISTS (SELECT * FROM EmailTemplates WHERE TemplateName = N'RoATPGetHelpWithQuestion')
BEGIN
	INSERT INTO EmailTemplates ([Id], [Status], [TemplateName], [TemplateId], [Recipients], [CreatedAt], [CreatedBy]) 
	VALUES (NEWID(), 'Live', N'RoATPGetHelpWithQuestion', N'9d1e1a7e-3557-4781-8901-ea627ae70ec2', N'helpdesk@manage-apprenticeships.service.gov.uk', GETDATE(), 'System')
END

IF NOT EXISTS (SELECT * FROM EmailTemplates WHERE TemplateName = N'RoATPApplicationSubmitted')
BEGIN
	INSERT INTO EmailTemplates ([Id], [Status], [TemplateName], [TemplateId],  [CreatedAt], [CreatedBy]) 
	VALUES (NEWID(), 'Live', N'RoATPApplicationSubmitted', N'4a44e79d-1e98-4b90-9d67-f575be97def6', GETDATE(), 'System')
END

IF NOT EXISTS (SELECT * FROM EmailTemplates WHERE TemplateName = N'RoATPApplicationUpdated')
BEGIN
	INSERT INTO EmailTemplates ([Id], [Status], [TemplateName], [TemplateId],  [CreatedAt], [CreatedBy]) 
	VALUES (NEWID(), 'Live', N'RoATPApplicationUpdated', N'ebb28424-b1ce-4374-b24b-a240f0cecdc1', GETDATE(), 'System')
END

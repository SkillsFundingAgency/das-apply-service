MERGE EmailTemplates
USING (VALUES (NEWID(), 'Live', N'RoATPRequestInvitationToReapply', N'872e3ace-4625-4f9b-a909-c44cec7f71ca', N'helpdesk@manage-apprenticeships.service.gov.uk', GETDATE(), 'System')) 
	AS et([Id], [Status], [TemplateName], [TemplateId], [Recipients],  [CreatedAt], [CreatedBy])
ON EmailTemplates.TemplateName = et.TemplateName
WHEN NOT MATCHED THEN
  INSERT ([Id], [Status], [TemplateName], [TemplateId], [Recipients],  [CreatedAt], [CreatedBy])
  VALUES ([Id], [Status], [TemplateName], [TemplateId], [Recipients],  [CreatedAt], [CreatedBy]); 

MERGE EmailTemplates
USING (VALUES (NEWID(), 'Live', N'RoATPGetHelpWithQuestion', N'9d1e1a7e-3557-4781-8901-ea627ae70ec2', N'helpdesk@manage-apprenticeships.service.gov.uk', GETDATE(), 'System')) 
	AS et([Id], [Status], [TemplateName], [TemplateId], [Recipients],  [CreatedAt], [CreatedBy])
ON EmailTemplates.TemplateName = et.TemplateName
WHEN NOT MATCHED THEN
  INSERT ([Id], [Status], [TemplateName], [TemplateId], [Recipients],  [CreatedAt], [CreatedBy])
  VALUES ([Id], [Status], [TemplateName], [TemplateId], [Recipients],  [CreatedAt], [CreatedBy]); 

  MERGE EmailTemplates
USING (VALUES (NEWID(), 'Live', N'RoATPApplicationSubmitted', N'4a44e79d-1e98-4b90-9d67-f575be97def6', null,GETDATE(), 'System')) 
	AS et([Id], [Status], [TemplateName], [TemplateId], [Recipients],  [CreatedAt], [CreatedBy])
ON EmailTemplates.TemplateName = et.TemplateName
WHEN NOT MATCHED THEN
  INSERT ([Id], [Status], [TemplateName], [TemplateId], [Recipients],  [CreatedAt], [CreatedBy])
  VALUES ([Id], [Status], [TemplateName], [TemplateId], [Recipients],  [CreatedAt], [CreatedBy]); 

  MERGE EmailTemplates
USING (VALUES (NEWID(), 'Live', N'RoATPApplicationUpdated', N'ebb28424-b1ce-4374-b24b-a240f0cecdc1', null, GETDATE(), 'System')) 
	AS et([Id], [Status], [TemplateName], [TemplateId], [Recipients],  [CreatedAt], [CreatedBy])
ON EmailTemplates.TemplateName = et.TemplateName
WHEN NOT MATCHED THEN
  INSERT ([Id], [Status], [TemplateName], [TemplateId], [Recipients],  [CreatedAt], [CreatedBy])
  VALUES ([Id], [Status], [TemplateName], [TemplateId], [Recipients],  [CreatedAt], [CreatedBy]); 

MERGE INTO [dbo].[EmailTemplates] AS Target 
USING (VALUES 
	(N'FE933A35-58BE-4958-A8C3-20EEFB53EF48', 'Live', N'RoATPRequestInvitationToReapply', N'872e3ace-4625-4f9b-a909-c44cec7f71ca', N'helpdesk@manage-apprenticeships.service.gov.uk', GETDATE(), 'System'),
	(N'39DEA5C0-4A93-4F14-8569-7872B4AE052D', 'Live', N'RoATPGetHelpWithQuestion', N'9d1e1a7e-3557-4781-8901-ea627ae70ec2', N'helpdesk@manage-apprenticeships.service.gov.uk', GETDATE(), 'System'),
	(N'4B8E0E07-86CE-44D6-98C5-905EEA2F6C4D', 'Live', N'RoATPApplicationSubmitted', N'4a44e79d-1e98-4b90-9d67-f575be97def6', null,GETDATE(), 'System'),
	(N'73DEB796-D912-456A-A685-24AE1FD83B44', 'Live', N'RoATPApplicationUpdated', N'ebb28424-b1ce-4374-b24b-a240f0cecdc1', null, GETDATE(), 'System')
) 
AS Source ([Id], [Status], [TemplateName], [TemplateId], [Recipients],  [CreatedAt], [CreatedBy]) 
ON Target.TemplateId = Source.TemplateId 
WHEN NOT MATCHED BY TARGET THEN 
INSERT ([Id], [Status], [TemplateName], [TemplateId], [Recipients],  [CreatedAt], [CreatedBy])
  VALUES ([Id], [Status], [TemplateName], [TemplateId], [Recipients],  [CreatedAt], [CreatedBy])
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

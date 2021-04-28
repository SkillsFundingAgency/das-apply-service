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
DELETE FROM [EmailTemplates]

IF NOT EXISTS (SELECT * FROM EmailTemplates WHERE TemplateName = N'RoATPGetHelpWithQuestion')
BEGIN
	INSERT INTO EmailTemplates ([Id], [Status],[TemplateName],[TemplateId],[Recipients],[CreatedAt],[CreatedBy]) 
	VALUES (NEWID(), 'Live', N'RoATPGetHelpWithQuestion', N'9d1e1a7e-3557-4781-8901-ea627ae70ec2', N'helpdesk@manage-apprenticeships.service.gov.uk', GETDATE(), 'System')
END

IF NOT EXISTS (SELECT * FROM EmailTemplates WHERE TemplateName = N'RoATPApplicationSubmitted')
BEGIN
	INSERT INTO EmailTemplates ([Id], [Status],[TemplateName],[TemplateId],[CreatedAt],[CreatedBy]) 
	VALUES (NEWID(), 'Live', N'RoATPApplicationSubmitted', N'7e7a46e1-d203-486a-ac1b-40e8a617ef0d', GETDATE(), 'System')
END

IF NOT EXISTS (SELECT * FROM EmailTemplates WHERE TemplateName = N'RoATPApplicationSubmittedMain')
BEGIN
	INSERT INTO EmailTemplates ([Id], [Status],[TemplateName],[TemplateId],[CreatedAt],[CreatedBy]) 
	VALUES (NEWID(), 'Live', N'RoATPApplicationSubmittedMain', N'68008288-6497-4de2-be3c-e1409538aad9', GETDATE(), 'System')
END

IF EXISTS (SELECT * FROM EmailTemplates WHERE TemplateName = N'RoATPApplicationWithdrawn')
BEGIN
	DELETE FROM EmailTemplates where TemplateName = N'RoATPApplicationWithdrawn'
END

IF NOT EXISTS (SELECT * FROM EmailTemplates WHERE TemplateName = N'RoATPApplicationUpdated')
BEGIN
	INSERT INTO EmailTemplates ([Id], [Status],[TemplateName],[TemplateId],[CreatedAt],[CreatedBy]) 
	VALUES (NEWID(), 'Live', N'RoATPApplicationUpdated', N'ebb28424-b1ce-4374-b24b-a240f0cecdc1', GETDATE(), 'System')
END


-- ARP-2424 Whitelisted Providers
BEGIN
	DELETE FROM WhitelistedProviders;

	INSERT INTO WhitelistedProviders ([UKPRN], [StartDateTime], [EndDateTime]) 
	VALUES (10002085, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
	       (10007165, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10007177, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10023326, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10024636, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10027061, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10027216, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10029699, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10032315, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10032663, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10033950, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10036126, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10037364, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10040392, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10040411, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10049431, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10061312, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10062335, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10063769, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10000565, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10000831, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10001113, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10001156, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10001777, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10004486, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10005204, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10019581, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10024704, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10029952, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10031982, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10034969, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10037391, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10061524, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59'),
		   (10013516, N'2021-05-17 00:00:00', N'2021-06-30 23:59:59')
END

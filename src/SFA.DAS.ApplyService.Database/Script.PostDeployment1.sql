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
	VALUES (NEWID(), 'Live', N'RoATPGetHelpWithQuestion', N'9d1e1a7e-3557-4781-8901-ea627ae70ec2', N'RoATP.SUPPORT@education.gov.uk', GETDATE(), 'System')
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

-- If we TRUNCATE table, we will not need to check 'NOT EXISTS' for each entry 
TRUNCATE TABLE [WhitelistedProviders]

INSERT INTO [dbo].[WhitelistedProviders] ([UKPRN]) VALUES (10031001)
INSERT INTO [dbo].[WhitelistedProviders] ([UKPRN]) VALUES (10039420)
INSERT INTO [dbo].[WhitelistedProviders] ([UKPRN]) VALUES (10052982)
INSERT INTO [dbo].[WhitelistedProviders] ([UKPRN]) VALUES (10060147)
INSERT INTO [dbo].[WhitelistedProviders] ([UKPRN]) VALUES (10062192)
INSERT INTO [dbo].[WhitelistedProviders] ([UKPRN]) VALUES (10064702)
INSERT INTO [dbo].[WhitelistedProviders] ([UKPRN]) VALUES (10081936)
INSERT INTO [dbo].[WhitelistedProviders] ([UKPRN]) VALUES (10082572)
INSERT INTO [dbo].[WhitelistedProviders] ([UKPRN]) VALUES (10083239)
INSERT INTO [dbo].[WhitelistedProviders] ([UKPRN]) VALUES (10084242)
INSERT INTO [dbo].[WhitelistedProviders] ([UKPRN]) VALUES (10084697)
INSERT INTO [dbo].[WhitelistedProviders] ([UKPRN]) VALUES (10084763)
INSERT INTO [dbo].[WhitelistedProviders] ([UKPRN]) VALUES (10084903)
INSERT INTO [dbo].[WhitelistedProviders] ([UKPRN]) VALUES (10085115)
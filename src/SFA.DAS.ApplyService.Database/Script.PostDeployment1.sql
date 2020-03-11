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

-- Whitelisted Providers

IF NOT EXISTS (SELECT * FROM WhitelistedProviders  WHERE [UKPRN] = 10031001)
BEGIN
	INSERT INTO WhitelistedProviders ([UKPRN]) VALUES (10031001)
END

IF NOT EXISTS (SELECT * FROM WhitelistedProviders  WHERE [UKPRN] = 10039420)
BEGIN
	INSERT INTO WhitelistedProviders ([UKPRN]) VALUES (10039420)
END

IF NOT EXISTS (SELECT * FROM WhitelistedProviders  WHERE [UKPRN] = 10052982)
BEGIN
	INSERT INTO WhitelistedProviders ([UKPRN]) VALUES (10052982)
END

IF NOT EXISTS (SELECT * FROM WhitelistedProviders  WHERE [UKPRN] = 10060147)
BEGIN
	INSERT INTO WhitelistedProviders ([UKPRN]) VALUES (10060147)
END

IF NOT EXISTS (SELECT * FROM WhitelistedProviders  WHERE [UKPRN] = 10062192)
BEGIN
	INSERT INTO WhitelistedProviders ([UKPRN]) VALUES (10062192)
END

IF NOT EXISTS (SELECT * FROM WhitelistedProviders  WHERE [UKPRN] = 10064702)
BEGIN
	INSERT INTO WhitelistedProviders ([UKPRN]) VALUES (10064702)
END

IF NOT EXISTS (SELECT * FROM WhitelistedProviders  WHERE [UKPRN] = 10081936)
BEGIN
	INSERT INTO WhitelistedProviders ([UKPRN]) VALUES (10081936)
END

IF NOT EXISTS (SELECT * FROM WhitelistedProviders  WHERE [UKPRN] = 10082572)
BEGIN
	INSERT INTO WhitelistedProviders ([UKPRN]) VALUES (10082572)
END

IF NOT EXISTS (SELECT * FROM WhitelistedProviders  WHERE [UKPRN] = 10083239)
BEGIN
	INSERT INTO WhitelistedProviders ([UKPRN]) VALUES (10083239)
END

IF NOT EXISTS (SELECT * FROM WhitelistedProviders  WHERE [UKPRN] = 10084242)
BEGIN
	INSERT INTO WhitelistedProviders ([UKPRN]) VALUES (10084242)
END

IF NOT EXISTS (SELECT * FROM WhitelistedProviders  WHERE [UKPRN] = 10084697)
BEGIN
	INSERT INTO WhitelistedProviders ([UKPRN]) VALUES (10084697)
END

IF NOT EXISTS (SELECT * FROM WhitelistedProviders  WHERE [UKPRN] = 10084763)
BEGIN
	INSERT INTO WhitelistedProviders ([UKPRN]) VALUES (10084763)
END

IF NOT EXISTS (SELECT * FROM WhitelistedProviders  WHERE [UKPRN] = 10084763)
BEGIN
	INSERT INTO WhitelistedProviders ([UKPRN]) VALUES (10084763)
END

IF NOT EXISTS (SELECT * FROM WhitelistedProviders  WHERE [UKPRN] = 10084903)
BEGIN
	INSERT INTO WhitelistedProviders ([UKPRN]) VALUES (10084903)
END

IF NOT EXISTS (SELECT * FROM WhitelistedProviders  WHERE [UKPRN] = 10085115)
BEGIN
	INSERT INTO WhitelistedProviders ([UKPRN]) VALUES (10085115)
END

IF NOT EXISTS (SELECT * FROM WhitelistedProviders  WHERE [UKPRN] = 10029571)
BEGIN
	INSERT INTO WhitelistedProviders ([UKPRN]) VALUES (10029571)
END

IF NOT EXISTS (SELECT * FROM WhitelistedProviders  WHERE [UKPRN] = 10033656)
BEGIN
	INSERT INTO WhitelistedProviders ([UKPRN]) VALUES (10033656)
END

IF NOT EXISTS (SELECT * FROM WhitelistedProviders  WHERE [UKPRN] = 10016231)
BEGIN
	INSERT INTO WhitelistedProviders ([UKPRN]) VALUES (10016231)
END

IF NOT EXISTS (SELECT * FROM WhitelistedProviders  WHERE [UKPRN] = 10055520)
BEGIN
	INSERT INTO WhitelistedProviders ([UKPRN]) VALUES (10055520)
END

IF NOT EXISTS (SELECT * FROM WhitelistedProviders  WHERE [UKPRN] = 10063652)
BEGIN
	INSERT INTO WhitelistedProviders ([UKPRN]) VALUES (10063652)
END

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

:r .\APR-2311.sql

---APR-2642.sql
BEGIN
if not exists(select * from BankHoliday where BankHolidayDate='2021-08-30')
	INSERT INTO [dbo].[BankHoliday] ([BankHolidayDate]) VALUES('2021-08-30')

if not exists(select * from BankHoliday where BankHolidayDate='2021-12-27')
	INSERT INTO [dbo].[BankHoliday] ([BankHolidayDate]) VALUES('2021-12-27')

if not exists(select * from BankHoliday where BankHolidayDate='2021-12-28')
	INSERT INTO [dbo].[BankHoliday] ([BankHolidayDate]) VALUES('2021-12-28')

if not exists(select * from BankHoliday where BankHolidayDate='2022-01-3')
	INSERT INTO [dbo].[BankHoliday] ([BankHolidayDate]) VALUES('2022-01-3')

if not exists(select * from BankHoliday where BankHolidayDate='2022-04-15')
	INSERT INTO [dbo].[BankHoliday] ([BankHolidayDate]) VALUES('2022-04-15')

if not exists(select * from BankHoliday where BankHolidayDate='2022-04-18')
	INSERT INTO [dbo].[BankHoliday] ([BankHolidayDate]) VALUES('2022-04-18')

if not exists(select * from BankHoliday where BankHolidayDate='2022-05-02')
	INSERT INTO [dbo].[BankHoliday] ([BankHolidayDate]) VALUES('2022-05-02')

if not exists(select * from BankHoliday where BankHolidayDate='2022-06-02')
	INSERT INTO [dbo].[BankHoliday] ([BankHolidayDate]) VALUES('2022-06-02')

if not exists(select * from BankHoliday where BankHolidayDate='2022-06-03')
	INSERT INTO [dbo].[BankHoliday] ([BankHolidayDate]) VALUES('2022-06-03')

if not exists(select * from BankHoliday where BankHolidayDate= '2022-08-29')
	INSERT INTO [dbo].[BankHoliday] ([BankHolidayDate]) VALUES( '2022-08-29')

if not exists(select * from BankHoliday where BankHolidayDate='2022-12-26')
	INSERT INTO [dbo].[BankHoliday] ([BankHolidayDate]) VALUES('2022-12-26')

if not exists(select * from BankHoliday where BankHolidayDate='2022-12-27')
	INSERT INTO [dbo].[BankHoliday] ([BankHolidayDate]) VALUES('2022-12-27')
END

---APR-2640.sql
IF EXISTS(SELECT * FROM sys.tables WHERE SCHEMA_NAME(schema_id) LIKE 'dbo' AND name LIKE 'AppealUpload')  
   DROP TABLE [dbo].[AppealUpload];
GO

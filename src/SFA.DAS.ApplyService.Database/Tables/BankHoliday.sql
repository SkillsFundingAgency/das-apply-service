﻿CREATE TABLE [dbo].[BankHoliday]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
	BankHolidayDate  DATE NOT NULL UNIQUE,
	Active bit not null default 1
)

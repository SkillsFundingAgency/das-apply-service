﻿
CREATE TABLE [dbo].[AllowedProviders]
(
	[UKPRN] INT NOT NULL, 
    [StartDateTime] DATETIME NOT NULL DEFAULT '2021-01-01 00:00:00', 
    [EndDateTime] DATETIME NOT NULL DEFAULT '9999-12-31 23:59:59',
    [AddedDateTime] DATETIME NULL,

    CONSTRAINT PK_AllowedProviders PRIMARY KEY (UKPRN)
)
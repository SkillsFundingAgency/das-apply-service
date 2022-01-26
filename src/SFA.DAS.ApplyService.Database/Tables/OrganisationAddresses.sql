CREATE TABLE [dbo].[OrganisationAddresses]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[OrganisationId] UNIQUEIDENTIFIER NOT NULL,
	[AddressType] [TINYINT] NOT NULL,
	[AddressLine1] NVARCHAR(MAX) NULL,
	[AddressLine2] NVARCHAR(MAX) NULL,
	[AddressLine3] NVARCHAR(MAX) NULL,
	[City] NVARCHAR(200) NULL,
	[Postcode] NVARCHAR(10) NULL,

	CONSTRAINT PK_OrganisationAddresses PRIMARY KEY (Id),
	CONSTRAINT FK_OrganisationAddresses_Organisation FOREIGN KEY (OrganisationId) REFERENCES Organisations(Id)
)

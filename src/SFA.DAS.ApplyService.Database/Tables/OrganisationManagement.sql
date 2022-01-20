CREATE TABLE [dbo].[OrganisationManagement]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[OrganisationId] UNIQUEIDENTIFIER NOT NULL,
	[FirstName] NVARCHAR(300) NOT NULL,
	[LastName] NVARCHAR(300) NOT NULL,
	[JobRole] NVARCHAR(300) NOT NULL,
	[TimeInRoleMonths] [INT] NOT NULL,
	[IsPartOfAnyOtherOrganisation] [BIT] NOT NULL,
	[OtherOrganisationNames] NVARCHAR(1000) NULL,
	[DateOfBirthMonth] TiNYINT NOT NULL,
	[DateOfBirthYear] INT NOT NULL,
	[ContactNumber] NVARCHAR(20) NOT NULL,
	[Email] NVARCHAR(350) NOT NULL,

	CONSTRAINT PK_OrganisationManagement PRIMARY KEY (Id),
	CONSTRAINT FK_OrganisationManagement_Organisation FOREIGN KEY (OrganisationId) REFERENCES Organisations(Id)
)

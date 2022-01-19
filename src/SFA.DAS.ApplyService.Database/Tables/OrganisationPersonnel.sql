CREATE TABLE [dbo].[OrganisationPersonnel]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[OrganisationId] UNIQUEIDENTIFIER NOT NULL,
	[PersonnelType] TINYINT NOT NULL,
	[Name] NVARCHAR(MAX) NOT NULL,
	[DateOfBirthMonth] TINYINT NULL,
	[DateOfBirthYear] INT NULL,

	CONSTRAINT PK_OrganisationPersonnel PRIMARY KEY (Id),
	CONSTRAINT FK_OrganisationPersonnel_Organisation FOREIGN KEY (OrganisationId) REFERENCES Organisations(Id)
)

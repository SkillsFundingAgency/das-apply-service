CREATE TABLE [dbo].[OrganisationSectorExperts]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[OrganisationSectorId] INT NOT NULL,
	[FirstName] NVARCHAR(300) NOT NULL,
	[LastName] NVARCHAR(300) NOT NULL,
	[JobRole] NVARCHAR(300) NOT NULL,
	[TimeInRole] NVARCHAR(300) NOT NULL,
	[IsPartOfAnyOtherOrganisation] [BIT] NOT NULL,
	[OtherOrganisationNames] NVARCHAR(1000) NULL,
	[DateOfBirthMonth] TiNYINT NOT NULL,
	[DateOfBirthYear] INT NOT NULL,
	[ContactNumber] NVARCHAR(50) NOT NULL,
	[Email] NVARCHAR(350) NOT NULL,
	[SectorTrainingExperienceDuration] NVARCHAR(30) NULL,
	[SectorTrainingExperienceDetails] NVARCHAR(MAX) NULL,
	[IsQualifiedForSector] BIT NOT NULL,
	[QualificationDetails] NVARCHAR(MAX) NULL,
	[IsApprovedByAwardingBodies] BIT NOT NULL, 
	[AwardingBodyNames] NVARCHAR(MAX) NULL, 
	[HasSectorOrTradeBodyMembership] BIT NOT NULL, 
	[SectorOrTradeBodyNames] NVARCHAR(MAX) NULL, 
	[TypeOfApprenticeshipDelivered] NVARCHAR(50) NULL, 
	[ExperienceInTrainingApprentices] NVARCHAR(100) NULL, 
	[TypicalDurationOfTrainingApprentices] NVARCHAR(100) NULL, 

	CONSTRAINT PK_OrganisationSectorExperts PRIMARY KEY (Id),
	CONSTRAINT FK_OrganisationSectorExperts_OrganisationSectors FOREIGN KEY (OrganisationSectorId) REFERENCES OrganisationSectors(Id)
)

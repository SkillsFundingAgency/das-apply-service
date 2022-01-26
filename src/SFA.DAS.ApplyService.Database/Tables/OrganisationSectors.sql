CREATE TABLE [dbo].[OrganisationSectors]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[OrganisationId] UNIQUEIDENTIFIER NOT NULL,
	[SectorName] NVARCHAR(200) NOT NULL, 
	[StandardsServed] NVARCHAR(MAX) NOT NULL, 
	[ExpectedNumberOfStarts] INT NOT NULL,
	[NumberOfTrainers] INT NOT NULL,

	CONSTRAINT PK_OrganisationSectors PRIMARY KEY (Id),
	CONSTRAINT FK_OrganisationSectors_Organisation FOREIGN KEY (OrganisationId) REFERENCES Organisations(Id)
)

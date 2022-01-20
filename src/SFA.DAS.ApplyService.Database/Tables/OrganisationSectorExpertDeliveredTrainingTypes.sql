CREATE TABLE [dbo].[OrganisationSectorExpertDeliveredTrainingTypes]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[OrganisationSectorExpertId] INT NOT NULL,
	[DeliveredTrainingType] NVARCHAR(300) NOT NULL, 

	CONSTRAINT PK_OrganisationSectorExpertDeliveredTrainingTypes PRIMARY KEY (Id),
	CONSTRAINT FK_OrganisationSectorExpertDeliveredTrainingTypes_OrganisationSectorExperts FOREIGN KEY (OrganisationSectorExpertId) REFERENCES OrganisationSectorExperts(Id)
)

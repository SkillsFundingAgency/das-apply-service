
CREATE TABLE [dbo].[OrganisationManagement](
	[Id] [int] NOT NULL IDENTITY(1,1),
	[OrganisationId] [uniqueidentifier] NOT NULL,
	[FirstName] [varchar](300) NOT NULL,
	[LastName] [varchar](300) NOT NULL,
	[JobRole] [varchar](300) NOT NULL,
	[TimeInRoleMonths] [int] NOT NULL,
	[IsPartOfAnyOtherOrganisation] [bit] NOT NULL,
	[OtherOrganisationNames] [varchar](1000) NULL,
	[DateOfBirthMonth] [tinyint] NOT NULL,
	[DateOfBirthYear] [int] NOT NULL,
	[ContactNumber] [varchar](50) NOT NULL,
	[Email] [varchar](350) NOT NULL,
 CONSTRAINT [PK_OrganisationManagement] PRIMARY KEY CLUSTERED 
 (
	[Id] ASC
 ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]
GO


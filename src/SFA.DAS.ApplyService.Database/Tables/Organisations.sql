-- create Organisation table for Apply
CREATE TABLE [dbo].[Organisations](
	[Id] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
   [Name]	[nvarchar](400)	NOT NULL,	-- Name of the Organisation (as result of Search)
   [OrganisationType]	[nvarchar](100)	NOT NULL,	 -- Type of the Organisation (as result of Search / input)
   [OrganisationUKPRN] [int] NULL, -- This is an 8 digit number starting with 1
   [OrganisationDetails]	[nvarchar](max)	NULL,	 -- JSON, includes Contact and Address Details (as result of Search),  
   -- Financial health check state, good-until-date
   -- "OrganisationReferenceType":How the Organisation reference was found - "RoEPAO", "RoATP", "EASAPI"
   -- "OrganisationReferenceId":The Organisation reference, saved after Search - e.g. EPAOrgID, UKPRN, Companies House Number
   Status	[nvarchar](	20)	NOT NULL,	-- 'initial', 'new' ,'inprogress','done', 'live', 'deleted'
   RoEPAOApproved [bit] NOT NULL DEFAULT 0, -- set when this organisation is fully approved,  as RoEPAO  
   RoATPApproved [bit] NOT NULL DEFAULT 0, -- set when this organisation is fully approved, as RoATP 
   CreatedAt	Datetime2(7)	NOT NULL,	--Date / Time that the record was created
   CreatedBy	[nvarchar](256)	NOT NULL,	--Username (staff or ApplyContact)
   UpdatedAt	Datetime2(7)		NULL,	--Date / Time of the last update
   UpdatedBy	[nvarchar](256)	NULL,	--Username (staff or ApplyContact)
   DeletedAt	Datetime2(7)		NULL,	--Date / Time of the soft delete
   DeletedBy	[nvarchar](256)	NULL	--Username (staff or ApplyContact)
    CONSTRAINT [PK_Organisations] PRIMARY KEY ([Id]),
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE UNIQUE INDEX [IXU_Organisations] ON [Organisations] ([Name])
GO
CREATE INDEX [IX_Organisations_OrganisationUKPRN] ON [Organisations] ([OrganisationUKPRN])
GO
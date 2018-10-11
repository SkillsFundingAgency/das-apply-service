-- create Contact table for Apply
CREATE TABLE [dbo].[Contacts](
	[Id] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
   Email	[nvarchar](120)	NOT NULL,	-- Email Address used to identify Applicant
   GivenNames	[nvarchar](250)	NOT NULL,	 -- Contact Given name(s)
   FamilyName	[nvarchar](250)	NOT NULL,	 -- Contact Family name
   SigninId [uniqueidentifier] NULL, -- SigninID (DFE or Provider Idams Username)
   SigninType	[nvarchar](20)	NOT NULL,	 -- When Known ('DFE Signin','Provider idAMS')
   ApplyOrganisationID [uniqueidentifier] NULL, -- When Known organisation for the apply contact
   Status	[nvarchar](	20)	NOT NULL,	-- 'draft' is being created or edited (and cannot be used),  'live' is live , 'deleted' is no longer to be used
   CreatedAt	Datetime2(7)	NOT NULL,	--Date / Time that the record was created
   CreatedBy	[nvarchar](30)	NOT NULL,	--Username (staff or ApplyContact)
   UpdatedAt	Datetime2(7)		NULL,	--Date / Time of the last update
   UpdatedBy	[nvarchar](30)	NULL,	--Username (staff or ApplyContact)
   DeletedAt	Datetime2(7)		NULL,	--Date / Time of the soft delete
   DeletedBy	[nvarchar](30)	NULL	--Username (staff or ApplyContact)
    CONSTRAINT [PK_ApplyContact] PRIMARY KEY ([Id]),
) 
GO

CREATE UNIQUE INDEX [IXU_Contacts] ON [Contacts] ([Email])
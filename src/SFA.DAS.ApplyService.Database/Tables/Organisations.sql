-- create Organisation table for Apply
CREATE TABLE [dbo].[Organisations](
	[Id] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
   [Name]	[nvarchar](400)	NOT NULL,	-- Name of the Organisation (as result of Search)
   [OrganisationType]	[nvarchar](100)	NOT NULL,	 -- Type of the Organisation (as result of Search)
   ApplicationType	[nvarchar](12)	NULL,	 -- Type of Application(s) 'RoEPAO' 'RoATP' 
   OrganisationDetails	[nvarchar](max)	NULL,	 -- Contact and Address Details (as result of Search) JSON
   ApplyOrganisationID [uniqueidentifier] NULL, -- When Known organisation for the apply contact
   Status	[nvarchar](	20)	NOT NULL,	-- 'new' ,'waiting','live','deleted'
   CreatedAt	Datetime2(7)	NOT NULL,	--Date / Time that the record was created
   CreatedBy	[nvarchar](30)	NOT NULL,	--Username (staff or ApplyContact)
   UpdatedAt	Datetime2(7)		NULL,	--Date / Time of the last update
   UpdatedBy	[nvarchar](30)	NULL,	--Username (staff or ApplyContact)
   DeletedAt	Datetime2(7)		NULL,	--Date / Time of the soft delete
   DeletedBy	[nvarchar](30)	NULL	--Username (staff or ApplyContact)
    CONSTRAINT [PK_Organisations] PRIMARY KEY ([Id]),
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE UNIQUE INDEX [IXU_Organisations] ON [Organisations] ([Name],[ApplicationType])



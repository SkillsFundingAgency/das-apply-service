-- create Application Entity table for Apply
CREATE TABLE [dbo].[Entities](
	[Id] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
   ApplyingOrganisationId [uniqueidentifier] NOT NULL, --  organisation for the application
   ApplicationType	[nvarchar](12)	NULL,	 -- Type of Application(s) 'RoEPAO', 'RoATP'  
   QnAData	[nvarchar](max)	NOT NULL,	-- QnA Data JSON Object 
   Status	[nvarchar](	20)	NOT NULL,	-- 'active' 'completed' 'withdrawn' 'deleted' 
   CreatedAt	Datetime2(7)	NOT NULL,	--Date / Time that the record was created
   CreatedBy	[nvarchar](30)	NOT NULL,	--Username (staff or ApplyContact)
   UpdatedAt	Datetime2(7)		NULL,	--Date / Time of the last update
   UpdatedBy	[nvarchar](30)	NULL,	--Username (staff or ApplyContact)
   WithdrawnAt	Datetime2(7)		NULL,	--Date / Time of the Application is withdrawn
   WithdrawnBy	[nvarchar](30)	NULL,	--Username (staff or ApplyContact)
   DeletedAt	Datetime2(7)		NULL,	--Date / Time of the soft delete
   DeletedBy	[nvarchar](30)	NULL	--Username (staff or ApplyContact)
    CONSTRAINT [PK_Entities] PRIMARY KEY ([Id]),
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


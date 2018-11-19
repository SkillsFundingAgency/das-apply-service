-- create Assets table for Apply
CREATE TABLE [dbo].[Assets](
	[Id] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
   Reference	[nvarchar](20)	NOT NULL,	-- External reference for this Asset (e.g. Q-101, P-245)
   [Type]	[nvarchar](10)	NOT NULL,	 -- 'Question', 'Prompt' etc, as required 
   Text	[nvarchar](max)	NULL,	-- Text content
   Format	[nvarchar](200)	NULL,	-- format rules (use to be determined)
   Status	[nvarchar](	20)	NOT NULL,	--The current process status of this record, 'live' – is live on the service, 'deleted' – is no longer allowed to be used
   CreatedAt	Datetime2(7)	NOT NULL,	--Date / Time that the record was created
   CreatedBy	[nvarchar](120)	NOT NULL,	--Username (staff)
   UpdatedAt	Datetime2(7)		NULL,	--Date / Time of the last update
   UpdatedBy	[nvarchar](120)	NULL,	--Username (staff)
   DeletedAt	Datetime2(7)		NULL,	--Date / Time of the soft delete
   DeletedBy	[nvarchar](120)	NULL	--Username (staff)
    CONSTRAINT [PK_Assets] PRIMARY KEY ([Id]),
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE UNIQUE INDEX [IXU_Assets] ON [Assets] ([Reference])




-- create Master QnA table for Apply
CREATE TABLE [dbo].[QnAs](
	[Id] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
   Description	[nvarchar](200)	NOT NULL,	-- Description of this Question and Answer
   Version	[nvarchar](10)	NOT NULL,	 -- 'Versioning 
   [Type]	[nvarchar](10)	NOT NULL,	 -- ('RoEPAO' or 'RoATP')
   [Data]	[nvarchar](max)	NULL,	-- The QnA data JSON object, containing structure of workflow and questions
   Status	[nvarchar](	20)	NOT NULL,	-- 'draft' is being created or edited (and cannot be used),  'live' is live , 'deleted' is no longer to be used
   CreatedAt	Datetime2(7)	NOT NULL,	--Date / Time that the record was created
   CreatedBy	[nvarchar](30)	NOT NULL,	--Username (staff)
   UpdatedAt	Datetime2(7)		NULL,	--Date / Time of the last update
   UpdatedBy	[nvarchar](30)	NULL,	--Username (staff)
   DeletedAt	Datetime2(7)		NULL,	--Date / Time of the soft delete
   DeletedBy	[nvarchar](30)	NULL	--Username (staff)
    CONSTRAINT [PK_QnAs] PRIMARY KEY ([Id]),
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE UNIQUE INDEX [IXU_QnAs] ON [QnAs] ([Description],[Version])



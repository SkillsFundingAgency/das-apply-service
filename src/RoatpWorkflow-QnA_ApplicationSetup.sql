-- Application Setup: Project and Workflow for Roatp
DECLARE @WorkflowId UNIQUEIDENTIFIER
DECLARE @ProjectId UNIQUEIDENTIFIER

SET @WorkflowId = '86F83D58-8608-4462-9A4E-65837AF04287'
SET @ProjectId = '70A0871F-42C1-48EF-8689-E63F0C91A487'

IF NOT EXISTS
	(SELECT 1 FROM [dbo].[Projects] WHERE Id = @ProjectId)
BEGIN
	INSERT INTO [dbo].[Projects]
		([Id]
		,[Name]
		,[Description]
		,[ApplicationDataSchema]
		,[CreatedAt]
		,[CreatedBy])
	VALUES
		(@ProjectId
		,'RoATP'
		,'Register of Apprenticeship Training Providers'
		,'{}'
		,GETDATE()
		,'Import')
END

IF NOT EXISTS
	(SELECT 1 FROM [dbo].[Workflows] WHERE [Type] = 'RoATP')
BEGIN
	INSERT INTO [dbo].[Workflows]
           ([Id]
		   ,[ProjectId]
           ,[Description]
           ,[Version]
           ,[Type]
           ,[CreatedAt]
           ,[CreatedBy]           
           ,[ReferenceFormat]
		   ,[ApplicationDataSchema])
     VALUES
           (@WorkflowId
		   ,@ProjectId
           ,'RoATP Workflow'
           ,'1.0'
           ,'RoATP'
           ,GETDATE()
           ,'Import'
           ,'AAD',
		   '{}')
END

DELETE FROM [dbo].[WorkflowSections]
WHERE ProjectId = @ProjectId

DELETE FROM [dbo].[WorkflowSequences]
WHERE WorkflowId = @WorkflowId

GO



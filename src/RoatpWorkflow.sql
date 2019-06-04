IF NOT EXISTS
	(SELECT 1 FROM [dbo].[Workflows] WHERE [Type] = 'RoATP')
BEGIN
	INSERT INTO [dbo].[Workflows]
           ([Id]
           ,[Description]
           ,[Version]
           ,[Type]
           ,[Status]
           ,[CreatedAt]
           ,[CreatedBy]           
           ,[ReferenceFormat])
     VALUES
           ('86F83D58-8608-4462-9A4E-65837AF04287'
           ,'RoATP Workflow'
           ,'1.0'
           ,'RoATP'
           ,'Live'
           ,GETDATE()
           ,'Import'
           ,'AAD')
END


DELETE FROM [dbo].[WorkflowSequences]
WHERE WorkflowId = '86F83D58-8608-4462-9A4E-65837AF04287'

DELETE FROM [dbo].[WorkflowSections]
WHERE WorkflowId = '86F83D58-8608-4462-9A4E-65837AF04287'

INSERT INTO [dbo].[WorkflowSequences]
           ([Id]
           ,[WorkflowId]
           ,[SequenceId]
           ,[Status]
           ,[IsActive])
     VALUES
           ('79F6D68E-1EC3-47A5-9BAA-4CBE987B3153'
           ,'86F83D58-8608-4462-9A4E-65837AF04287'
           ,1
           ,'Draft'
           ,1)
		   
-- REMOVE LATER
-- This is just to test that we can start a new workflow with a new workflow
		   
INSERT [dbo].[WorkflowSections]
  ([Id], [WorkflowId], [SequenceId], [SectionId], [QnAData], [Title], [LinkTitle], [Status], [DisplayType], [DisallowedOrgTypes])
VALUES
  (N'414FA7FE-FC4D-4141-8E28-48FE34C120B5', N'86F83D58-8608-4462-9A4E-65837AF04287', 1, 1, N'
{
  "Pages": [
    {
      "PageId": "23",
      "SequenceId": "1",
      "SectionId": "3",
      "Title": "SQ-1-SE-3-PG-23-441",
      "LinkTitle": "SQ-1-SE-3-PG-23-444",
      "InfoText": "SQ-1-SE-3-PG-23-443",
      "Details": {
        "Title": "SQ-1-SE-2-PG-23-DT-1",
        "Body": "SQ-1-SE-2-PG-23-DB-1"
      },
      "Questions": [
        {
          "QuestionId": "FHA-01",
          "Label": "SQ-1-SE-3-PG-23-FHA-01-446",
          "ShortLabel": "SQ-1-SE-3-PG-23-FHA-01-447",
          "QuestionBodyText": "SQ-1-SE-3-PG-23-FHA-01-448",
          "Hint": "SQ-1-SE-3-PG-23-FHA-01-445",
          "Input": {
            "Type": "FileUpload",
            "Options": null,
            "Validations": [
               {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select a file containing your accounts"
              },
              {
                "Name": "FileType",
                "Value": "pdf,application/pdf",
                "ErrorMessage": "The selected file must be a PDF"
              }
            ]
          },
          "Order": null
        },
        {
          "QuestionId": "FHA-02",
          "Label": "SQ-1-SE-3-PG-23-FHA-02-450",
          "ShortLabel": "SQ-1-SE-3-PG-23-FHA-02-451",
          "QuestionBodyText": "SQ-1-SE-3-PG-23-FHA-02-452",
          "Hint": "SQ-1-SE-3-PG-23-FHA-02-449",
          "Input": {
            "Type": "FileUpload",
            "Options": null,
            "Validations": [
              {
                "Name": "FileType",
                "Value": "pdf,application/pdf",
                "ErrorMessage": "The selected file must be a PDF"
              }
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [
        {
          "Action": "ReturnToSection",
          "ReturnId": "3",
          "Condition": null,
          "ConditionMet": false
        }
      ],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "NotRequiredOrgTypes": [],
      "BodyText": "SQ-1-SE-3-PG-23-442"
    }
  ],
  "FinancialApplicationGrade": null
}
', N'Financial health assessment', N'Financial health assessment', N'Draft', N'Pages', N'')

-- REMOVE LATER



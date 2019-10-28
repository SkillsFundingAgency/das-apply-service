-- Section 99 Setup Conditions of Acceptance
DECLARE @WorkflowId99 UNIQUEIDENTIFIER
DECLARE @ProjectId99 UNIQUEIDENTIFIER

SET @WorkflowId99 = '86F83D58-8608-4462-9A4E-65837AF04287'
SET @ProjectId99 = '70A0871F-42C1-48EF-8689-E63F0C91A487'

delete from workflowsections where id in
(select sectionID from WorkflowSequences where sequenceNo in (99)
)

delete from WorkflowSequences where sequenceNo in (99)


DECLARE @ConditionsOfAcceptanceSectionId UNIQUEIDENTIFIER
SET @ConditionsOfAcceptanceSectionId = 'A56E170E-F602-47DB-97DE-A5765B86C97A'
           
INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle], [DisplayType])
VALUES
  (@ConditionsOfAcceptanceSectionId, @ProjectId99, N'
{
  "Pages": [
    {
      "PageId": "999999",
      "SequenceId": null,
      "SectionId": null,
      "Title": "Conditions of acceptance",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "COA-1",
          "QuestionTag": "COA-Stage1-Application",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [      
        {
          "Action": "NextPage",
          "ReturnId": "3",
          "Conditions": [],
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
      "BodyText": ""
    }
  ]
}
', N'Conditions of acceptance', N'Conditions of acceptance', N'Pages')

DECLARE @ConditionsOfAcceptanceSequenceId UNIQUEIDENTIFIER
SET @ConditionsOfAcceptanceSequenceId = 'C636D66B-818C-478F-9970-68BFCED4F89A'

INSERT INTO [dbo].[WorkflowSequences]
           ([Id]
           ,[WorkflowId]
           ,[SequenceNo]
           ,[SectionNo]
           ,[SectionId]
           ,[IsActive])
     VALUES
           (@ConditionsOfAcceptanceSequenceId
           ,@WorkflowId99
           ,99
           ,1
           ,@ConditionsOfAcceptanceSectionId
           ,1)



GO
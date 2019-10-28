-- Section 6 setup Planning apprenticeship training
DECLARE @WorkflowId6 UNIQUEIDENTIFIER
DECLARE @ProjectId6 UNIQUEIDENTIFIER

SET @WorkflowId6 = '86F83D58-8608-4462-9A4E-65837AF04287'
SET @ProjectId6 = '70A0871F-42C1-48EF-8689-E63F0C91A487'


DECLARE @PlanningApprenticeshipTrainingSequenceId UNIQUEIDENTIFIER
SET @PlanningApprenticeshipTrainingSequenceId = '93621915-179F-49CD-9505-F3A66ED17637'

DECLARE @PlanningApprenticeshipTrainingSectionId UNIQUEIDENTIFIER
SET @PlanningApprenticeshipTrainingSectionId = '1252337D-E7AB-4473-9722-DA7BAB1E7091'

delete from workflowsequences where id = @PlanningApprenticeshipTrainingSequenceId

delete from workflowSections where id = @PlanningApprenticeshipTrainingSectionId

INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle], [DisplayType])
VALUES
  (@PlanningApprenticeshipTrainingSectionId, @ProjectId6, N'
{
	"Pages": [
		{
			"PageId": "6000",
			"SequenceId": null,
			"SectionId": null,
			"Title": "Planning apprenticeship training",
			"LinkTitle": "",
			"InfoText": "",
			 "Questions": [
        {
          "QuestionId": "PAT-10",
          "QuestionTag": "PAT-Introduction-Main",
          "Label": "Planning apprenticeship training",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">Lorem ipsum Planning apprenticeship training introduction page - main provider</p>",
          "Hint": "",
          "Input": {
            "Type": "Hidden",
            "Validations": []
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
       "Next": [      
        {
          "Action": "NextPage",
          "ReturnId": null,
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
    },
	{
			"PageId": "6001",
			"SequenceId": null,
			"SectionId": null,
			"Title": "Planning apprenticeship training",
			"LinkTitle": "",
			"InfoText": "",
			 "Questions": [
        {
          "QuestionId": "PAT-11",
          "QuestionTag": "PAT-Introduction-Employer",
          "Label": "Planning apprenticeship training",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">Lorem ipsum Planning apprenticeship training introduction page - employer provider</p>",
          "Hint": "",
          "Input": {
            "Type": "Hidden",
            "Validations": []
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
       "Next": [      
        {
          "Action": "NextPage",
          "ReturnId": null,
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
    },
	{
			"PageId": "6002",
			"SequenceId": null,
			"SectionId": null,
			"Title": "Planning apprenticeship training",
			"LinkTitle": "",
			"InfoText": "",
			 "Questions": [
        {
          "QuestionId": "PAT-12",
          "QuestionTag": "PAT-Introduction-Supporting",
          "Label": "Planning apprenticeship training",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">Lorem ipsum Planning apprenticeship training introduction page - supporting provider</p>",
          "Hint": "",
          "Input": {
            "Type": "Hidden",
            "Validations": []
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
       "Next": [      
        {
          "Action": "NextPage",
          "ReturnId": null,
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
', N'Introduction and what you''ll need', N'Introduction and what you''ll need', N'Pages')

INSERT INTO [dbo].[WorkflowSequences]
			([Id]
			,[WorkflowId]
			,[SequenceNo]
			,[SectionNo]
			,[SectionId]
			,[IsActive])
VALUES
			(@PlanningApprenticeshipTrainingSequenceId
			,@WorkFlowId6
			,6
			,1
			,@PlanningApprenticeshipTrainingSectionId
			,1)

GO
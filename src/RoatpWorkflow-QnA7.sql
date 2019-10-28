-- Section 7 setup: Delivering apprenticeship training
DECLARE @WorkflowId7 UNIQUEIDENTIFIER
DECLARE @ProjectId7 UNIQUEIDENTIFIER

SET @WorkflowId7 = '86F83D58-8608-4462-9A4E-65837AF04287'
SET @ProjectId7 = '70A0871F-42C1-48EF-8689-E63F0C91A487'

DECLARE @DeliveringApprenticeshipTrainingSequenceId UNIQUEIDENTIFIER
SET @DeliveringApprenticeshipTrainingSequenceId = '64DEFEB7-5D41-449B-909E-18F79689EBC5'

DECLARE @DeliveringApprenticeshipTrainingSectionId UNIQUEIDENTIFIER
SET @DeliveringApprenticeshipTrainingSectionId = '1D6F2D6C-2715-41ED-942B-1565E280D428'

delete from workflowsequences where id = @DeliveringApprenticeshipTrainingSequenceId

delete from workflowSections where id = @DeliveringApprenticeshipTrainingSectionId

INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle], [DisplayType])
VALUES
  (@DeliveringApprenticeshipTrainingSectionId, @ProjectId7, N'
{
	"Pages": [
		{
			"PageId": "7000",
			"SequenceId": null,
			"SectionId": null,
			"Title": "Delivering apprenticeship training",
			"LinkTitle": "",
			"InfoText": "",
			 "Questions": [
        {
          "QuestionId": "DAT-10",
          "QuestionTag": "DAT-Introduction-Main",
          "Label": "Delivering apprenticeship training",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">Lorem ipsum Delivering apprenticeship training introduction page - main provider</p>",
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
			"PageId": "7001",
			"SequenceId": null,
			"SectionId": null,
			"Title": "Delivering apprenticeship training",
			"LinkTitle": "",
			"InfoText": "",
			 "Questions": [
        {
          "QuestionId": "DAT-11",
          "QuestionTag": "DAT-Introduction-Employer",
          "Label": "Delivering apprenticeship training",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">Lorem ipsum Delivering apprenticeship training introduction page - employer provider</p>",
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
			"PageId": "7002",
			"SequenceId": null,
			"SectionId": null,
			"Title": "Delivering apprenticeship training",
			"LinkTitle": "",
			"InfoText": "",
			 "Questions": [
        {
          "QuestionId": "DAT-12",
          "QuestionTag": "DAT-Introduction-Supporting",
          "Label": "Delivering apprenticeship training",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">Lorem ipsum Delivering apprenticeship training introduction page - supporting provider</p>",
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
			(@DeliveringApprenticeshipTrainingSequenceId
			,@WorkFlowId7
			,7
			,1
			,@DeliveringApprenticeshipTrainingSectionId
			,1)

GO
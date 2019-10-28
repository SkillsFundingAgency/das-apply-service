-- Section 83 setup: Evaluating apprenticeship training
DECLARE @WorkflowId8 UNIQUEIDENTIFIER
DECLARE @ProjectId8 UNIQUEIDENTIFIER

SET @WorkflowId8 = '86F83D58-8608-4462-9A4E-65837AF04287'
SET @ProjectId8 = '70A0871F-42C1-48EF-8689-E63F0C91A487'

DECLARE @EvaluatingApprenticeshipTrainingSequenceId UNIQUEIDENTIFIER
SET @EvaluatingApprenticeshipTrainingSequenceId = 'C61EF6EE-EA34-4F4A-B612-14BCEA98D1ED'

DECLARE @EvaluatingApprenticeshipTrainingSectionId UNIQUEIDENTIFIER
SET @EvaluatingApprenticeshipTrainingSectionId = '21EF5397-AA9C-4E74-8D2A-D9E469532129'

delete from workflowsequences where id = @EvaluatingApprenticeshipTrainingSequenceId

delete from workflowSections where id = @EvaluatingApprenticeshipTrainingSectionId

INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle], [DisplayType])
VALUES
  (@EvaluatingApprenticeshipTrainingSectionId, @ProjectId8, N'
{
	"Pages": [
		{
			"PageId": "8000",
			"SequenceId": null,
			"SectionId": null,
			"Title": "Evaluating apprenticeship training",
			"LinkTitle": "",
			"InfoText": "",
			 "Questions": [
        {
          "QuestionId": "EAT-10",
          "QuestionTag": "DAT-Introduction-MainEmployer",
          "Label": "Evaluating apprenticeship training",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">Lorem ipsum Evaluating apprenticeship training introduction page - main and employer provider</p>",
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
			"PageId": "8001",
			"SequenceId": null,
			"SectionId": null,
			"Title": "Delivering apprenticeship training",
			"LinkTitle": "",
			"InfoText": "",
			 "Questions": [
        {
          "QuestionId": "EAT-11",
          "QuestionTag": "EAT-Introduction-Supporting",
          "Label": "Evaluating apprenticeship training",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">Lorem ipsum Evaluating apprenticeship training introduction page - supporting provider</p>",
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
			(@EvaluatingApprenticeshipTrainingSequenceId
			,@WorkFlowId8
			,8
			,1
			,@EvaluatingApprenticeshipTrainingSectionId
			,1)

GO
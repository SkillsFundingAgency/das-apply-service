-- Section 5 setup Readiness to engage
DECLARE @WorkflowId5 UNIQUEIDENTIFIER
DECLARE @ProjectId5 UNIQUEIDENTIFIER

SET @WorkflowId5 = '86F83D58-8608-4462-9A4E-65837AF04287'
SET @ProjectId5 = '70A0871F-42C1-48EF-8689-E63F0C91A487'

DECLARE @ReadinessToEngageSequenceId UNIQUEIDENTIFIER
SET @ReadinessToEngageSequenceId = 'B9BDE27F-5B68-4094-A4FD-1837A007C3F9'

DECLARE @ReadinessToEngageSectionId UNIQUEIDENTIFIER
SET @ReadinessToEngageSectionId = '9F30C43A-1241-4C4F-B105-17F3B9395EB5'

delete from workflowsequences where id = @ReadinessToEngageSequenceId

delete from workflowSections where id = @ReadinessToEngageSectionId

INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle], [DisplayType])
VALUES
  (@ReadinessToEngageSectionId, @ProjectId5, N'
{
	"Pages": [
		{
			"PageId": "5000",
			"SequenceId": null,
			"SectionId": null,
			"Title": "Readiness to engage",
			"LinkTitle": "",
			"InfoText": "",
			 "Questions": [
        {
          "QuestionId": "RTE-10",
          "QuestionTag": "RTE-Introduction-All",
          "Label": "Readiness to engage",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">Lorem ipsum Readiness to engage introduction page</p>",
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
			(@ReadinessToEngageSequenceId
			,@WorkFlowId5
			,3
			,1
			,@ReadinessToEngageSectionId
			,1)

GO

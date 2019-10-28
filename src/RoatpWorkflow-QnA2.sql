-- Section 2 setup Financial Health Assessment
DECLARE @WorkflowId2 UNIQUEIDENTIFIER
DECLARE @ProjectId2 UNIQUEIDENTIFIER

SET @WorkflowId2 = '86F83D58-8608-4462-9A4E-65837AF04287'
SET @ProjectId2 = '70A0871F-42C1-48EF-8689-E63F0C91A487'

DECLARE @FinancialHealthAssessmentSequenceId UNIQUEIDENTIFIER
SET @FinancialHealthAssessmentSequenceId = '991BEE4F-5334-4EED-B870-3FF1C65BD1E1'

DECLARE @FinancialHealthAssessmentSectionId UNIQUEIDENTIFIER
SET @FinancialHealthAssessmentSectionId = '185E61B7-7E34-4BA5-9EE8-F5DC20B0DD57'

delete from workflowsequences where id = @FinancialHealthAssessmentSequenceId

delete from workflowSections where id = @FinancialHealthAssessmentSectionId

INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle], [DisplayType])
VALUES
  (@FinancialHealthAssessmentSectionId, @ProjectId2, N'
{
	"Pages": [
		{
			"PageId": "2000",
			"SequenceId": null,
			"SectionId": null,
			"Title": "Financial health assessment",
			"LinkTitle": "",
			"InfoText": "",
			 "Questions": [
        {
          "QuestionId": "FHA-10",
          "QuestionTag": "Financial-Health-Introduction-All",
          "Label": "Financial health assessment",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">Lorem ipsum FHA introduction page</p>",
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
			(@FinancialHealthAssessmentSequenceId
			,@WorkFlowId2
			,2
			,1
			,@FinancialHealthAssessmentSectionId
			,1)

GO
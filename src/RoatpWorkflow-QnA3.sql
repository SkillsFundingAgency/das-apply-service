-- Section 3 setup Criminal and Compliance Checks
DECLARE @WorkflowId3 UNIQUEIDENTIFIER
DECLARE @ProjectId3 UNIQUEIDENTIFIER

SET @WorkflowId3 = '86F83D58-8608-4462-9A4E-65837AF04287'
SET @ProjectId3 = '70A0871F-42C1-48EF-8689-E63F0C91A487'

DECLARE @CriminalComplianceChecksSequenceId UNIQUEIDENTIFIER
SET @CriminalComplianceChecksSequenceId = '4FA30B18-D181-45B9-9F28-09139E0CE062'

DECLARE @CriminalComplianceChecksSectionId UNIQUEIDENTIFIER
SET @CriminalComplianceChecksSectionId = '349B0EEB-8D7E-47FC-A217-FDDBE7725CB7'

delete from workflowsequences where id = @CriminalComplianceChecksSequenceId

delete from workflowSections where id = @CriminalComplianceChecksSectionId

INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle], [DisplayType])
VALUES
  (@CriminalComplianceChecksSectionId, @ProjectId3, N'
{
	"Pages": [
		{
			"PageId": "3000",
			"SequenceId": null,
			"SectionId": null,
			"Title": "Criminal and compliance checks on your organisation",
			"LinkTitle": "",
			"InfoText": "",
			 "Questions": [
        {
          "QuestionId": "CC-10",
          "QuestionTag": "Criminal-Introduction-All",
          "Label": "Criminal and compliance checks on your organisation",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">Lorem ipsum criminal compliance checks introduction page</p>",
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
			(@CriminalComplianceChecksSequenceId
			,@WorkFlowId3
			,3
			,1
			,@CriminalComplianceChecksSectionId
			,1)

GO

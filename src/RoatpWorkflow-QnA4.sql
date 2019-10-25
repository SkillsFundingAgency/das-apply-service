-- Section 4 Setup: Protecting your apprentices
DECLARE @WorkflowId4 UNIQUEIDENTIFIER
DECLARE @ProjectId4 UNIQUEIDENTIFIER

SET @WorkflowId4 = '86F83D58-8608-4462-9A4E-65837AF04287'
SET @ProjectId4 = '70A0871F-42C1-48EF-8689-E63F0C91A487'


DECLARE @ProtectionOfApprenticesSequenceId UNIQUEIDENTIFIER
SET @ProtectionOfApprenticesSequenceId = '61861411-1794-420d-ab04-788ea4db8072'

DECLARE @ProtectionOfApprentices2SequenceId UNIQUEIDENTIFIER
SET @ProtectionOfApprentices2SequenceId = '436b3fba-1abf-4d13-a883-a552e7c437d2'

DECLARE @ProtectionOfApprentices3SequenceId UNIQUEIDENTIFIER
SET @ProtectionOfApprentices3SequenceId = '24bc1f2c-d2b2-4de0-b339-9d7664e091ab'


DECLARE @ProtectionOfApprentices2SectionId UNIQUEIDENTIFIER
SET @ProtectionOfApprentices2SectionId = '5dc1e1f5-7b28-47eb-9702-2b25f5d5782d'

DECLARE @ProtectionOfApprentices3SectionId UNIQUEIDENTIFIER
SET @ProtectionOfApprentices3SectionId = '82e510b7-ad91-4bd0-a60a-01b6ceef18d0'

DECLARE @ProtectionOfApprenticesSectionId UNIQUEIDENTIFIER
SET @ProtectionOfApprenticesSectionId = '80616b64-ffb4-45b3-9d4e-0b449bb441eb'

delete from workflowsequences where id = @ProtectionOfApprenticesSequenceId
delete from WorkflowSequences where id = @ProtectionOfApprentices2SequenceId
delete from WorkflowSequences where id = @ProtectionOfApprentices3SequenceId

delete from workflowSections where id = @ProtectionOfApprenticesSectionId
delete from workflowSections where id = @ProtectionOfApprentices2SectionId
delete from workflowSections where id = @ProtectionOfApprentices3SectionId

INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle], [DisplayType])
VALUES
  (@ProtectionOfApprenticesSectionId, @ProjectId4, N'
{
	"Pages": [
		{
			"PageId": "500",
			"SequenceId": "4",
			"SectionId": "1",
			"Title": "",
			"LinkTitle": "link title",
			"InfoText": "info text",
			 "Questions": [
        {
          "QuestionId": "PYA-10",
          "QuestionTag": "ProtectApprentice-Introduction-Main",
          "Label": "Protecting your apprentices",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\" id=\"pya-intro-main\">For this section you''ll need to upload your organisation''s:</p><ul class=\"govuk-list govuk-list--bullet\"><li>continuity plan for apprenticeship training</li><li>equality and diversity policy</li><li>safeguarding policy</li><li>prevent duty policy (if needed)</li><li>health and safety policy</li></ul><div class=\"govuk-warning-text\"><span class=\"govuk-warning-text__icon\" aria-hidden=\"true\">!</span><strong class=\"govuk-warning-text__text\"><span class=\"govuk-warning-text__assistive\">Warning</span>All policies and processes must be specific to your organisation, apprentices and trainers. They must also be signed by a senior employee. For example, a director or CEO. We will not accept policies or processes that are generic or taken from a third party.</strong></div>",
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
          "ReturnId": "530",
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
			"PageId": "510",
			"SequenceId": "4",
			"SectionId": "1",
			"Title": "",
			"LinkTitle": "link title",
			"InfoText": "info text",
			 "Questions": [
        {
          "QuestionId": "PYA-11",
          "QuestionTag": "ProtectApprentice-Introduction-Employer",
          "Label": "Protecting your apprentices",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\" id=\"pya-intro-emp\">For this section you''ll need to upload your organisation''s:</p><ul class=\"govuk-list govuk-list--bullet\"><li>continuity plan for apprenticeship training</li><li>equality and diversity policy</li><li>safeguarding policy</li><li>prevent duty policy (if needed)</li><li>health and safety policy</li></ul><div class=\"govuk-warning-text\"><span class=\"govuk-warning-text__icon\" aria-hidden=\"true\">!</span><strong class=\"govuk-warning-text__text\"><span class=\"govuk-warning-text__assistive\">Warning</span>All policies and processes must be specific to your organisation, apprentices and trainers. They must also be signed by a senior employee. For example, a director or CEO. We will not accept policies or processes that are generic or taken from a third party.</strong></div>",
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
          "ReturnId": "530",
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
			"PageId": "520",
			"SequenceId": "4",
			"SectionId": "1",
			"Title": "",
			"LinkTitle": "link title",
			"InfoText": "info text",
			 "Questions": [
        {
          "QuestionId": "PYA-12",
          "QuestionTag": "ProtectApprentice-Introduction-Supporting",
          "Label": "Protecting your apprentices",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\" id=\"pya-intro-supp\">For this section you''ll need to upload your organisation''s:</p><ul class=\"govuk-list govuk-list--bullet\"><li>equality and diversity policy</li><li>safeguarding policy</li><li>prevent duty policy (if needed)</li><li>health and safety policy</li></ul><div class=\"govuk-warning-text\"><span class=\"govuk-warning-text__icon\" aria-hidden=\"true\">!</span><strong class=\"govuk-warning-text__text\"><span class=\"govuk-warning-text__assistive\">Warning</span>All uploads must be specific to your organisation, apprentices and trainers. They must also be signed by a senior employee. For example, a director or CEO. We will not accept uploads that are generic or taken from a third party.</strong></div>",
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
          "ReturnId": "540",
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
INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle], [DisplayType])
VALUES
  (@ProtectionOfApprentices2SectionId, @ProjectId4, N'
{
	"Pages": [
		{
			"PageId": "530",
			"SequenceId": "4",
			"SectionId": "2",
			"Title": "Continuity plan for apprenticeship training",
			"LinkTitle": "Continuity plan for apprenticeship training",
			"InfoText": "Continuity plan for apprenticeship training",
			"Details": {
        "Title": "What is a significant event?",
        "Body": "<p class=\"govuk-body\">It''s when something happens that impacts the delivery of apprenticeship training. This includes things like a training location being unavailable or loss of systems and data.</p>"
      },
			 "Questions": [
        {
          "QuestionId": "PYA-20",
          "QuestionTag": "ProtectApprentice-Continuity",
          "Label": "Upload your organisation''s continuity plan for apprenticeship training",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\" id=\"pya-continuity-plan\">A continuity plan for apprenticeship training shows how your organisation will continue to deliver in case of a significant event.</p><p class=\"govuk-body\" id=\"pya-continuity-plan-2\">This must include how your organisation will:</p><ul class=\"govuk-list govuk-list--bullet\"><li>have different methods of communication</li><li>manage transportation needs</li><li>provide different operating locations (if needed)</li><li>back-up relevant business systems</li><li>back-up and restore data </li><li>have a list of emergency contacts - ESFA must be listed</li></ul><p class=\"govuk-body\">The file must be a PDF and smaller than 5MB.</p>",
          "Hint": "",
          "Input": {
            "Type": "FileUpload",
            "Validations": [
			    {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select your organisation''s continuity plan for apprenticeship training"
                },
				{
                "Name": "FileType",
                "Value": "pdf",
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
          "Action": "NextPage",
          "ReturnId": "540",
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
', N'Continuity plan for apprenticeship training', N'Continuity plan for apprenticeship training', N'Pages')


INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle],[DisplayType])
VALUES
  (@ProtectionOfApprentices3SectionId, @ProjectId4, N'
{
	"Pages": [
		{
			"PageId": "540",
			"SequenceId": "4",
			"SectionId": "3",
			"Title": "Equality and diversity policy",
			"LinkTitle": "Equality and diversity policy",
			"InfoText": "Equality and diversity policy",
			 "Questions": [
        {
          "QuestionId": "PYA-30",
          "QuestionTag": "ProtectApprentice-Equality",
          "Label": "Upload your organisation''s equality and diversity policy",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\" id=\"pya-equality-policy\">This must include how your organisation will:</p><ul class=\"govuk-list govuk-list--bullet\"><li>promote the policy</li><li>get engagement towards the policy</li><li>train its employees in implementing the policy</li><li>consider the policy when recruiting, delivering apprenticeship training and working with employers and apprentices</li></ul><p class=\"govuk-body\">The file must be a PDF and smaller than 5MB.</p>",
          "Hint": "",
          "Input": {
            "Type": "FileUpload",
            "Validations": [{
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select your organisation''s equality and diversity policy"
                },
				{
                "Name": "FileType",
                "Value": "pdf",
                "ErrorMessage": "The selected file must be a PDF"
              }]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
       "Next": [      
        {
          "Action": "NextPage",
          "ReturnId": "550",
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
', N'Equality and diversity policy', N'Equality and diversity policy',  N'Pages')



INSERT INTO [dbo].[WorkflowSequences]
			([Id]
			,[WorkflowId]
			,[SequenceNo]
			,[SectionNo]
			,[SectionId]
			,[IsActive])
VALUES
			(@ProtectionOfApprenticesSequenceId
			,@WorkFlowId4
			,4
			,1
			,@ProtectionOfApprenticesSectionId
			,1)

INSERT INTO [dbo].[WorkflowSequences]
			([Id]
			,[WorkflowId]
			,[SequenceNo]
			,[SectionNo]
			,[SectionId]
			,[IsActive])
VALUES
			(@ProtectionOfApprentices2SequenceId
			,@WorkFlowId4
			,4
			,2
			,@ProtectionOfApprentices2SectionId
			,1)

INSERT INTO [dbo].[WorkflowSequences]
			([Id]
			,[WorkflowId]
			,[SequenceNo]
			,[SectionNo]
			,[SectionId]
			,[IsActive])
VALUES
			(@ProtectionOfApprentices3SequenceId
			,@WorkFlowId4
			,4
			,3
			,@ProtectionOfApprentices3SectionId
			,1)

GO
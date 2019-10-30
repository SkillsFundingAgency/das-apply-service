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

DECLARE @ProtectionOfApprentices4SequenceId UNIQUEIDENTIFIER
SET @ProtectionOfApprentices4SequenceId = '94633287-8d89-456e-82e0-1e6967df6e85'


DECLARE @ProtectionOfApprentices2SectionId UNIQUEIDENTIFIER
SET @ProtectionOfApprentices2SectionId = '5dc1e1f5-7b28-47eb-9702-2b25f5d5782d'

DECLARE @ProtectionOfApprentices3SectionId UNIQUEIDENTIFIER
SET @ProtectionOfApprentices3SectionId = '82e510b7-ad91-4bd0-a60a-01b6ceef18d0'

DECLARE @ProtectionOfApprentices4SectionId UNIQUEIDENTIFIER
SET @ProtectionOfApprentices4SectionId = 'bbef7c29-ed73-4708-b67e-445846b67712'

DECLARE @ProtectionOfApprenticesSectionId UNIQUEIDENTIFIER
SET @ProtectionOfApprenticesSectionId = '80616b64-ffb4-45b3-9d4e-0b449bb441eb'

delete from workflowsequences where id = @ProtectionOfApprenticesSequenceId
delete from WorkflowSequences where id = @ProtectionOfApprentices2SequenceId
delete from WorkflowSequences where id = @ProtectionOfApprentices3SequenceId
delete from WorkflowSequences where id = @ProtectionOfApprentices4SequenceId


delete from workflowSections where id = @ProtectionOfApprenticesSectionId
delete from workflowSections where id = @ProtectionOfApprentices2SectionId
delete from workflowSections where id = @ProtectionOfApprentices3SectionId
delete from workflowSections where id = @ProtectionOfApprentices4SectionId

INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle], [DisplayType])
VALUES
  (@ProtectionOfApprenticesSectionId, @ProjectId4, N'
{
	"Pages": [
		{
			"PageId": "4000",
			"SequenceId": null,
			"SectionId": null,
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
          "ReturnId": "4010",
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
			"PageId": "4001",
			"SequenceId": null,
			"SectionId": null,
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
          "ReturnId": "4010",
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
			"PageId": "4002",
			"SequenceId": null,
			"SectionId": null,
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
          "ReturnId": "4010",
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
			"PageId": "4010",
			"SequenceId": null,
			"SectionId": null,
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
          "ReturnId": "4020",
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
			"PageId": "4020",
			"SequenceId": null,
			"SectionId": null,
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
          "ReturnId": "4030",
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



INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle],[DisplayType])
VALUES
  (@ProtectionOfApprentices4SectionId, @ProjectId4, N'
{
	"Pages": [
		{
			"PageId": "4030",
			"SequenceId": null,
			"SectionId": null,
			"Title": "Safeguarding and Prevent duty policy",
			"LinkTitle": "Safeguarding and Prevent duty policy",
			"InfoText": "Safeguarding and Prevent duty policy",
			 "Questions": [
        {
          "QuestionId": "PYA-40",
          "QuestionTag": "ProtectApprentice-Safeguarding",
          "Label": "Upload your organisation''s safeguarding policy",
          "ShortLabel": "",
           "QuestionBodyText": "<p class=\"govuk-body\" id=\"pya-equality-policy\">This must include how your organisation will:</p><ul class=\"govuk-list govuk-list--bullet\"><li>promote the policy</li><li>get commitment towards the policy</li><li>train its employees in implementing the policy</li><li>protect its apprentices</li><li>prevent abuse towards its apprentices</li><li>have  way of raising, recording and investigating concerns</li><li>have a way of apprentices getting support or guidance</li><li>monitor its IT usage</li></ul><p class=\"govuk-body\">Find out more about safeguarding in <a href=\"http://www.legislation.gov.uk/ukpga/2014/23/contents\" target=\"_blank\">the Care Act 2014 (opens in a new windows or tab)</a>.</p><p class=\"govuk-body\">The file must be a PDF and smaller than 5MB.</p>",
          "Hint": "",
          "Input": {
            "Type": "FileUpload",
            "Validations": [
			    {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select your organisation''s safeguarding policy"
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
          "ReturnId": "4035",
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
			"PageId": "4035",
			"SequenceId": null,
			"SectionId": null,
			"Title": "Safeguarding responsibility - title",
			"LinkTitle": "Safeguarding responsibility - link title",
			"InfoText": "Safeguarding responsibility - info text",
			 "Questions": [
			  {
          "QuestionId": "PYA-45",
          "QuestionTag": "ProtectApprentice-Safeguarding-responsibility",
          "Label": "Tell us who has overall responsibility for safeguarding in your organisation",
          "ShortLabel": "",
           "QuestionBodyText": "Holding page for safeguarding responsibility officer",
          "Hint": "",
          "Input": {
            "Type": "Hidden",
            "Validations": []
			},
          "Order": null
        }
      ],"PageOfAnswers": [],
       "Next": [      
        {
          "Action": "NextPage",
          "ReturnId": "4040",
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
', N'Safeguarding and Prevent duty policy', N'Safeguarding and Prevent duty policy',  N'Pages')


INSERT INTO [dbo].[WorkflowSequences]
			([Id]
			,[WorkflowId]
			,[SequenceNo]
			,[SectionNo]
			,[SectionId]
			,[IsActive])
VALUES
			(@ProtectionOfApprenticesSequenceId
			,@WorkflowId4
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
			,@WorkflowId4
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
			,@WorkflowId4
			,4
			,3
			,@ProtectionOfApprentices3SectionId
			,1)


INSERT INTO [dbo].[WorkflowSequences]
			([Id]
			,[WorkflowId]
			,[SequenceNo]
			,[SectionNo]
			,[SectionId]
			,[IsActive])
VALUES
			(@ProtectionOfApprentices4SequenceId
			,@WorkFlowId4
			,4
			,4
			,@ProtectionOfApprentices4SectionId
			,1)
GO
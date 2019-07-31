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
           ,0
           ,'Draft'
           ,1)
	
	
INSERT [dbo].[WorkflowSections]
  ([Id], [WorkflowId], [SequenceId], [SectionId], [QnAData], [Title], [LinkTitle], [Status], [DisplayType], [DisallowedOrgTypes])
VALUES
  (N'414FA7FE-FC4D-4141-8E28-48FE34C120B5', N'86F83D58-8608-4462-9A4E-65837AF04287', 0, 1, N'
{
  "Pages": [
    {
      "PageId": "1",
      "SequenceId": "0",
      "SectionId": "1",
      "Title": "Preamble",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "PRE-10",
          "QuestionTag": "UKPRN",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-20",
          "QuestionTag": "UKRLP-LegalName",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-30",
          "QuestionTag": "UKRLP-Website",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-31",
          "QuestionTag": "UKRLP-NoWebsite",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-40",
          "QuestionTag": "UKRLP-LegalAddress-Line1",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-41",
          "QuestionTag": "UKRLP-LegalAddress-Line2",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-42",
          "QuestionTag": "UKRLP-LegalAddress-Line3",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-43",
          "QuestionTag": "UKRLP-LegalAddress-Line4",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-44",
          "QuestionTag": "UKRLP-LegalAddress-Town",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-45",
          "QuestionTag": "UKRLP-LegalAddress-PostCode",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-46",
          "QuestionTag": "UKRLP-TradingName",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-50",
          "QuestionTag": "CH-ManualEntryRequired",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-51",
          "QuestionTag": "UKRLP-Verification-CompanyNumber",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-52",
          "QuestionTag": "CH-CompanyName",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-53",
          "QuestionTag": "CH-CompanyType",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-54",
          "QuestionTag": "CH-CompanyStatus",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-55",
          "QuestionTag": "CH-IncorporationDate",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-56",
          "QuestionTag": "UKRLP-Verification-Company",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-60",
          "QuestionTag": "CC-TrusteeManualEntry",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-61",
          "QuestionTag": "UKRLP-Verification-CharityRegNumber",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-62",
          "QuestionTag": "CC-CharityName",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-64",
          "QuestionTag": "CC-RegistrationDate",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-65",
          "QuestionTag": "UKRLP-Verification-Charity",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-70",
          "QuestionTag": "UKRLP-Verification-SoleTraderPartnership",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-80",
          "QuestionTag": "UKRLP-PrimaryVerificationSource",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-90",
          "QuestionTag": "OnROATP",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-91",
          "QuestionTag": "ROATP-CurrentStatus",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-92",
          "QuestionTag": "ROATP-RemovedReason",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-93",
          "QuestionTag": "ROATP-StatusDate",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
            "Validations": []            
          },
          "Order": null
        },
        {
          "QuestionId": "PRE-94",
          "QuestionTag": "ROATP-ProviderRoute",
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
          "ReturnId": "2",
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
      "BodyText": ""
    }
  ]
}
', N'Preamble', N'Preamble', N'Draft', N'Pages', N'')

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

INSERT [dbo].[WorkflowSections]
  ([Id], [WorkflowId], [SequenceId], [SectionId], [QnAData], [Title], [LinkTitle], [Status], [DisplayType], [DisallowedOrgTypes])
VALUES
  (N'A7962801-47FA-4A72-95DB-827E76D0A482', N'86F83D58-8608-4462-9A4E-65837AF04287', 1, 1, N'
{
  "Pages": [
    {
      "PageId": "2",
      "SequenceId": "1",
      "SectionId": "1",
      "Title": "Provider route",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-1",
          "QuestionTag": "Apply-ProviderRoute",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "ComplexRadio",
            "Options": [
              {
                "Label": "Main provider",
				"HintText" : "Your organisation can train apprentices for other organisations, its own employees, employees of connected organisations or act as a subcontractor for other main and employer providers.",
                "Value": "1",
                "FurtherQuestions": null
              },
              {
                "Label": "Employer provider",
				"HintText": "Your organisation can train its own employees, employees of connected organisations or act as a subcontractor for other employer or main providers.",
                "Value": "2",
                "FurtherQuestions": null
              },
			  {
                "Label": "Supporting provider",
				"HintText": "Your organisation will act as a subcontractor for main and employer providers to train apprentices up to a maximum of £500,000 per year. If your organisation is new on the register, this will be limited to £100,000 in its first year.",
                "Value": "3",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us your organisation''s provider route"
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
      "BodyText": ""
    }
  ]
}
', N'Your organisation', N'Your organisation', N'Draft', N'Pages', N'')

INSERT INTO [dbo].[WorkflowSequences]
           ([Id]
           ,[WorkflowId]
           ,[SequenceId]
           ,[Status]
           ,[IsActive])
     VALUES
           ('79F6D68E-1EC3-47A5-9BAA-4CBE987B3153'
           ,'86F83D58-8608-4462-9A4E-65837AF04287'
           ,99
           ,'Draft'
           ,1)

           
INSERT [dbo].[WorkflowSections]
  ([Id], [WorkflowId], [SequenceId], [SectionId], [QnAData], [Title], [LinkTitle], [Status], [DisplayType], [DisallowedOrgTypes])
VALUES
  (N'9D6A1DB9-0154-47F8-A486-C8380FE36ADE', N'86F83D58-8608-4462-9A4E-65837AF04287', 99, 1, N'
{
  "Pages": [
    {
      "PageId": "999999",
      "SequenceId": "99",
      "SectionId": "1",
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
      "BodyText": ""
    }
  ]
}
', N'Conditions of acceptance', N'Conditions of acceptance', N'Draft', N'Pages', N'')
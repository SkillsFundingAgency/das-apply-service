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
           ,[IsActive],
		   [Description])
     VALUES
           ('79F6D68E-1EC3-47A5-9BAA-4CBE987B3153'
           ,'86F83D58-8608-4462-9A4E-65837AF04287'
           ,0
           ,'Draft'
           ,1,
		   '')
	 		  	
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
           ,[IsActive],
		   [Description])
     VALUES
           ('79F6D68E-1EC3-47A5-9BAA-4CBE987B3153'
           ,'86F83D58-8608-4462-9A4E-65837AF04287'
           ,1
           ,'Draft'
           ,1
		   ,'Your organisation')

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
          "Label": "Choose your organisation''s provider route",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Radio",
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
          "ReturnId": "10",
          "Condition": {
            "QuestionId": "YO-1",
            "MustEqual": "1"
          },
          "ConditionMet": false
        },      
		{
          "Action": "NextPage",
          "ReturnId": "11",
          "Condition": {
            "QuestionId": "YO-1",
            "MustEqual": "2"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "12",
          "Condition": {
            "QuestionId": "YO-1",
            "MustEqual": "3"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": null,
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
', N'Provider route', N'Provider route', N'Draft', N'Pages', N'')

INSERT [dbo].[WorkflowSections]
  ([Id], [WorkflowId], [SequenceId], [SectionId], [QnAData], [Title], [LinkTitle], [Status], [DisplayType], [DisallowedOrgTypes])
VALUES
  (N'A7962801-47FA-4A72-95DB-827E76D0A482', N'86F83D58-8608-4462-9A4E-65837AF04287', 1, 2, N'
{
  "Pages": [
    {
      "PageId": "10",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "Introduction and what you''ll need",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-10",
          "QuestionTag": "Organisation-Introduction-Main",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">MAIN PROVIDER Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque sed interdum diam, vitae ornare tortor. Etiam ac lectus placerat, porttitor.</p>",
          "Hint": "",
          "Input": {
            "Type": "Hidden",
            "Validations": []
          },
          "Order": null
        },
		{
          "QuestionId": "YO-11",
          "QuestionTag": "Organisation-Introduction-Employer",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Hidden",
            "Validations": []
          },
          "Order": null
        },
		{
          "QuestionId": "YO-12",
          "QuestionTag": "Organisation-Introduction-Supporting",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
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
          "ReturnId": "20",
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
    },
	{
      "PageId": "11",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "Introduction and what you''ll need",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-11",
          "QuestionTag": "Organisation-Introduction-Employer",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">EMPLOYER PROVIDER Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque sed interdum diam, vitae ornare tortor. Etiam ac lectus placerat, porttitor.</p>",
          "Hint": "",
          "Input": {
            "Type": "Hidden",
            "Validations": []
          },
          "Order": null
        },
		{
          "QuestionId": "YO-10",
          "QuestionTag": "Organisation-Introduction-Main",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Hidden",
            "Validations": []
          },
          "Order": null
        },
		{
          "QuestionId": "YO-12",
          "QuestionTag": "Organisation-Introduction-Supporting",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
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
          "ReturnId": "20",
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
    },
	{
      "PageId": "12",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "Introduction and what you''ll need",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-12",
          "QuestionTag": "Organisation-Introduction-Supporting",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">SUPPORTING PROVIDER Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque sed interdum diam, vitae ornare tortor. Etiam ac lectus placerat, porttitor.</p>",
          "Hint": "",
          "Input": {
            "Type": "Hidden",
            "Validations": []
          },
          "Order": null
        },
		{
          "QuestionId": "YO-10",
          "QuestionTag": "Organisation-Introduction-Main",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Hidden",
            "Validations": []
          },
          "Order": null
        },
		{
          "QuestionId": "YO-11",
          "QuestionTag": "Organisation-Introduction-Employer",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
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
          "ReturnId": "20",
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
', N'Introduction and what you''ll need', N'Introduction and what you''ll need', N'Draft', N'Pages', N'')

INSERT [dbo].[WorkflowSections]
  ([Id], [WorkflowId], [SequenceId], [SectionId], [QnAData], [Title], [LinkTitle], [Status], [DisplayType], [DisallowedOrgTypes])
VALUES
  (N'A7962801-47FA-4A72-95DB-827E76D0A482', N'86F83D58-8608-4462-9A4E-65837AF04287', 1, 3, N'
{
  "Pages": [
    {
      "PageId": "20",
      "SequenceId": "1",
      "SectionId": "3",
      "Title": "Does your organisation have an ultimate parent company in the UK?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-20",
          "QuestionTag": "Has-ParentCompany",
          "Label": "Does your organisation have an ultimate parent company in the UK?",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">Your organisation will only have an ultimate parent company if it’s part of a group.</p><p class=\"govuk-body\">An ultimate parent company sits at the top of your organisation’s group and has the most responsibility.</p>",
          "Hint": "",
          "Input": {
            "Type": "ComplexRadio",
			"Options": [
			{
                "Label": "Yes",
				"Value": "Y",
                "FurtherQuestions": null
              },
			  {
                "Label": "No",
				"Value": "N",
                "FurtherQuestions": null
              }
			],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us if your organisation has an ultimate UK parent company"
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
          "ReturnId": "21",
          "Condition": {
            "QuestionId": "YO-20",
            "MustEqual": "Yes"
          },
          "ConditionMet": false
        },	  
        {
          "Action": "NextPage",
          "ReturnId": "30",
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
    },
	{
      "PageId": "21",
      "SequenceId": "1",
      "SectionId": "3",
      "Title": "Enter your organisation’s ultimate parent company details",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-21",
          "QuestionTag": "Add-ParentCompanyNumber",
          "Label": "<span class=\"govuk-label\">Company number</span>",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
			"InputClasses": "app-uppercase",
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Enter a company number"
              },
			  {
				"Name": "MinLength",
                "Value": 8,
                "ErrorMessage": "Enter a company number using 8 characters"
			  },
			  {
				"Name": "MaxLength",
                "Value": 8,
                "ErrorMessage": "Enter a company number using 8 characters"
			  },
			  {
				"Name": "Regex",
				"Value": "[A-Za-z0-9]{2}[0-9]{5}[A-Za-z0-9]{1}",
				"ErrorMessage": "Enter a valid company number"
			  }
            ]
          },
          "Order": null
        },
		{
          "QuestionId": "YO-22",
          "QuestionTag": "Add-ParentCompanyName",
          "Label": "<span class=\"govuk-label\">Company name</span>",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Text",
			"InputClasses": "app-uppercase",
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Enter a company name"
              },
			  {
				"Name": "MinLength",
                "Value": 2,
                "ErrorMessage": "Enter a company name using 2 characters or more"
			  },
			  {
				"Name": "MaxLength",
                "Value": 200,
                "ErrorMessage": "Enter a company number using 200 characters or less"
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
          "ReturnId": "30",
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
    },
	{
      "PageId": "30",
      "SequenceId": "1",
      "SectionId": "3",
      "Title": "What is your organisation''s Information Commissioner''s Office (ICO) registration number?",
      "LinkTitle": "",
      "InfoText": "",
	  "Details": {
        "Title": "I do not have an ICO registration number",
        "Body": "<p class=\"govuk-body\">To get an ICO registration number, you’ll need to <a href=\"https://ico.org.uk/registration/new\" target=\"_blank\">register with the ICO (opens in a new window or tab)</a>.</p><p class=\"govuk-body\">After you''ve registered and received an ICO registration number, sign back in to continue with this application.</p>"
      },
      "Questions": [
        {
          "QuestionId": "YO-30",
          "QuestionTag": "ICO-Number",
          "Label": "What is your organisation''s Information Commissioner''s Office (ICO) registration number?",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">This is an 8-digit registration number from the ICO data protection public register. Any organisation that processes personal data must have one.</p><p class=\"govuk-body\">If you''re not sure, <a href=\"https://ico.org.uk/ESDWebPages/Search\" target=\"_blank\">search for your organisation on the ICO data protection register (opens in a new window or tab)</a></p>",
          "Hint": "<span class=\"govuk-label\">ICO registration number</span>",
          "Input": {
            "Type": "Text",
			"InputClasses": "app-uppercase",
			"Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Enter an ICO registration number"
              },
			  {
				"Name": "MinLength",
                "Value": 8,
                "ErrorMessage": "Enter an ICO registration number using 8 characters"
			  },
			  {
				"Name": "MaxLength",
                "Value": 8,
                "ErrorMessage": "Enter an ICO registration number using 8 characters"
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
          "ReturnId": "40",
          "Condition": {
            "QuestionId": "PRE-31",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "60",
          "Condition": {
            "QuestionId": "YO-1",
            "MustEqual": "3"
          },
          "ConditionMet": false
        },
        {
          "Action": "NextPage",
          "ReturnId": "50",
          "Condition": {
            "QuestionId": "YO-1",
            "MustEqual": "1"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "50",
          "Condition": {
            "QuestionId": "YO-1",
            "MustEqual": "2"
          },
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
      "PageId": "40",
      "SequenceId": "1",
      "SectionId": "3",
      "Title": "Does your organisation have a website?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-40",
          "QuestionTag": "Has-Website",
          "Label": "Does your organisation have a website?",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "ComplexRadio",
			 "Options": [
              {
                "Label": "Yes",
				"Value": "Yes",
                "FurtherQuestions": null
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Select if your company has a website"
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
          "ReturnId": "40",
          "Condition": {
            "QuestionId": "PRE-31",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "60",
          "Condition": {
            "QuestionId": "YO-1",
            "MustEqual": "3"
          },
          "ConditionMet": false
        },
        {
          "Action": "NextPage",
          "ReturnId": "50",
          "Condition": {
            "QuestionId": "YO-1",
            "MustEqual": "1"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "50",
          "Condition": {
            "QuestionId": "YO-1",
            "MustEqual": "2"
          },
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
      "PageId": "40",
      "SequenceId": "1",
      "SectionId": "3",
      "Title": "PLACEHOLDER Website",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-40",
          "QuestionTag": "Has-Website",
          "Label": "PLACEHOLDER Website",
          "ShortLabel": "",
          "QuestionBodyText": "",
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
          "ReturnId": "999999",
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
    },
	{
      "PageId": "50",
      "SequenceId": "1",
      "SectionId": "3",
      "Title": "PLACEHOLDER Confirm Trading Main/Employer",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-50",
          "QuestionTag": "Confirm-Trading-MainEmployer",
          "Label": "PLACEHOLDER Confirm Trading Main/Employer",
          "ShortLabel": "",
          "QuestionBodyText": "",
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
          "ReturnId": "999999",
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
    },
	{
      "PageId": "60",
      "SequenceId": "1",
      "SectionId": "3",
      "Title": "PLACEHOLDER Confirm Trading Supporting",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-60",
          "QuestionTag": "Confirm-Trading-Supporting",
          "Label": "PLACEHOLDER Confirm Trading Supporting",
          "ShortLabel": "",
          "QuestionBodyText": "",
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
          "ReturnId": "60",
          "Condition": {
            "QuestionId": "YO-1",
            "MustEqual": "3"
          },
          "ConditionMet": false
        },
        {
          "Action": "NextPage",
          "ReturnId": "50",
          "Condition": {
            "QuestionId": "YO-1",
            "MustEqual": "1"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "50",
          "Condition": {
            "QuestionId": "YO-1",
            "MustEqual": "2"
          },
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
      "PageId": "50",
      "SequenceId": "1",
      "SectionId": "3",
      "Title": "PLACEHOLDER Confirm Trading Main/Employer",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-50",
          "QuestionTag": "Confirm-Trading-MainEmployer",
          "Label": "PLACEHOLDER Confirm Trading Main/Employer",
          "ShortLabel": "",
          "QuestionBodyText": "",
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
          "ReturnId": "999999",
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
    },
	{
      "PageId": "60",
      "SequenceId": "1",
      "SectionId": "3",
      "Title": "PLACEHOLDER Confirm Trading Supporting",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-60",
          "QuestionTag": "Confirm-Trading-Supporting",
          "Label": "PLACEHOLDER Confirm Trading Supporting",
          "ShortLabel": "",
          "QuestionBodyText": "",
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
          "ReturnId": "999999",
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
', N'Organisation details', N'Organisation details', N'Draft', N'Pages', N'')

INSERT INTO [dbo].[WorkflowSequences]
           ([Id]
           ,[WorkflowId]
           ,[SequenceId]
           ,[Status]
           ,[IsActive]
		   ,[Description])
     VALUES
           ('DEF40E31-71DA-40EC-8F33-9856C76C67DA'
           ,'86F83D58-8608-4462-9A4E-65837AF04287'
           ,2
           ,'Draft'
           ,1
		   ,'Financial evidence')
		   
INSERT INTO [dbo].[WorkflowSequences]
			([Id]
			,[WorkflowId]
			,[SequenceId]
			,[Status]
			,[IsActive]
			,[Description])
VALUES
			('B9FFC03D-2B9F-46C0-9481-44CD4C2E1E4F'
			,'86F83D58-8608-4462-9A4E-65837AF04287'
			,3
			,'Draft'
			,1
			,'Criminal and compliance checks')

INSERT INTO [dbo].[WorkflowSequences]
			([Id]
			,[WorkflowId]
			,[SequenceId]
			,[Status]
			,[IsActive]
			,[Description])
VALUES
			('4904E35B-6AF2-45C5-825B-EA41617287E1'
			,'86F83D58-8608-4462-9A4E-65837AF04287'
			,4
			,'Draft'
			,1
			,'Apprenticeship welfare')
			
INSERT INTO [dbo].[WorkflowSequences]
			([Id]
			,[WorkflowId]
			,[SequenceId]
			,[Status]
			,[IsActive]
			,[Description])
VALUES
			('8FA9490D-E7C0-40AE-97AF-38F0976B2A88'
			,'86F83D58-8608-4462-9A4E-65837AF04287'
			,5
			,'Draft'
			,1
			,'Readiness to engage')

INSERT INTO [dbo].[WorkflowSequences]
			([Id]
			,[WorkflowId]
			,[SequenceId]
			,[Status]
			,[IsActive]
			,[Description])
VALUES
			('C0D1550F-1372-404E-9CA9-D0021D190E7E'
			,'86F83D58-8608-4462-9A4E-65837AF04287'
			,6
			,'Draft'
			,1
			,'People and planning')

INSERT INTO [dbo].[WorkflowSequences]
			([Id]
			,[WorkflowId]
			,[SequenceId]
			,[Status]
			,[IsActive]
			,[Description])
VALUES
			('A8DC9146-4BEC-4B1E-B5AC-6D1698AF5FDF'
			,'86F83D58-8608-4462-9A4E-65837AF04287'
			,7
			,'Draft'
			,1
			,'Leaders and managers')
			

INSERT INTO [dbo].[WorkflowSequences]
           ([Id]
           ,[WorkflowId]
           ,[SequenceId]
           ,[Status]
           ,[IsActive]
		   ,[Description])
     VALUES
           ('79F6D68E-1EC3-47A5-9BAA-4CBE987B3153'
           ,'86F83D58-8608-4462-9A4E-65837AF04287'
           ,99
           ,'Draft'
           ,1
		   ,'')

           
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
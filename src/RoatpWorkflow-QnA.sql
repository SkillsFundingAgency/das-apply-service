DECLARE @WorkflowId UNIQUEIDENTIFIER
DECLARE @ProjectId UNIQUEIDENTIFIER

SET @WorkflowId = '86F83D58-8608-4462-9A4E-65837AF04287'
SET @ProjectId = '70A0871F-42C1-48EF-8689-E63F0C91A487'

IF NOT EXISTS
	(SELECT 1 FROM [dbo].[Projects] WHERE Id = @ProjectId)
BEGIN
	INSERT INTO [dbo].[Projects]
		([Id]
		,[Name]
		,[Description]
		,[ApplicationDataSchema]
		,[CreatedAt]
		,[CreatedBy])
	VALUES
		(@ProjectId
		,'RoATP'
		,'Register of Apprenticeship Training Providers'
		,'{}'
		,GETDATE()
		,'Import')
END

IF NOT EXISTS
	(SELECT 1 FROM [dbo].[Workflows] WHERE [Type] = 'RoATP')
BEGIN
	INSERT INTO [dbo].[Workflows]
           ([Id]
		   ,[ProjectId]
           ,[Description]
           ,[Version]
           ,[Type]
           ,[Status]
           ,[CreatedAt]
           ,[CreatedBy]           
           ,[ReferenceFormat]
		   ,[ApplicationDataSchema])
     VALUES
           (@WorkflowId
		   ,@ProjectId
           ,'RoATP Workflow'
           ,'1.0'
           ,'RoATP'
           ,'Live'
           ,GETDATE()
           ,'Import'
           ,'AAD',
		   '{}')
END


DELETE FROM [dbo].[WorkflowSections]
WHERE ProjectId = @ProjectId

DELETE FROM [dbo].[WorkflowSequences]
WHERE WorkflowId = @WorkflowId

DECLARE @PreambleSectionId UNIQUEIDENTIFIER
SET @PreambleSectionId = '076D997E-7F59-4C66-91A3-CC9B63231413'
	 		  	
INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle], [Status], [DisplayType])
VALUES
  (@PreambleSectionId, @ProjectId, N'
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
', N'Preamble', N'Preamble', N'Draft', N'Pages')

INSERT INTO [dbo].[WorkflowSequences]
           ([Id]
           ,[WorkflowId]
           ,[SequenceNo]
		   ,[SectionNo]
		   ,[SectionId]
           ,[Status]
           ,[IsActive])
     VALUES
           (NEWID()
           ,@WorkflowId
           ,0
		   ,1
		   ,@PreambleSectionId
           ,'Draft'
           ,1)

DECLARE @ProviderRouteSectionId UNIQUEIDENTIFIER
SET @ProviderRouteSectionId = '369F8A6A-DC8D-489C-9E1B-CDB5EF690EB9'

INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle], [Status], [DisplayType])
VALUES
  (@ProviderRouteSectionId, @ProjectId, N'
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
', N'Provider route', N'Provider route', N'Draft', N'Pages')

DECLARE @ProviderRouteSequenceId UNIQUEIDENTIFIER
SET @ProviderRouteSequenceId = '5B212C41-9A23-48F4-A58F-2868D1A04A0E'

INSERT INTO [dbo].[WorkflowSequences]
           ([Id]
           ,[WorkflowId]
           ,[SequenceNo]
		   ,[SectionNo]
		   ,[SectionId]
           ,[Status]
           ,[IsActive])
     VALUES
           (@ProviderRouteSequenceId
           ,@WorkflowId
           ,1
		   ,1
		   ,@ProviderRouteSectionId
           ,'Draft'
           ,1)

DECLARE @OrganisationIntroductionSectionId UNIQUEIDENTIFIER
SET @OrganisationIntroductionSectionId = '31532459-B353-474D-B24A-8CE5C9BDEA52'

INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle], [Status], [DisplayType])
VALUES
  (@OrganisationIntroductionSectionId, @ProjectId, N'
{
  "Pages": [
    {
      "PageId": "10",
      "SequenceId": "1",
      "SectionId": "2",
      "Title": "Your organisation",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-10",
          "QuestionTag": "Organisation-Introduction-Main",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">In this section we''ll ask for:</p><ul class=\"govuk-list govuk-list--bullet\"><li>your organsiation''s ICO registration number</li><li>who''s in control and their date of births</li><li>your organisation''s trading history</li><li>details of what your organisation is</li><li>Ofsted or other educational accreditations</li></ul><p class=\"govuk-body\">Depending on your answers, we may ask further questions.</p> <div class=\"govuk-warning-text\"> <span class=\"govuk-warning-text__icon\" aria-hidden=\"true\">!</span> <strong class=\"govuk-warning-text__text\"> <span class=\"govuk-warning-text__assistive\">Warning</span>Your organisation must have at least 12 months trading history and a training manager with at least 9 months experience in developing and delivering training. </strong> </div>",
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
      "Title": "Your organisation",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-11",
          "QuestionTag": "Organisation-Introduction-Employer",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">In this section we''ll ask for:</p><ul class=\"govuk-list govuk-list--bullet\"><li>your organsiation''s ICO registration number</li><li>who''s in control and their date of births</li><li>your organisation''s trading history</li><li>details of what your organisation is</li><li>Ofsted or other educational accreditations</li></ul><p class=\"govuk-body\">Depending on your answers, we may ask further questions.</p> <div class=\"govuk-warning-text\"> <span class=\"govuk-warning-text__icon\" aria-hidden=\"true\">!</span> <strong class=\"govuk-warning-text__text\"> <span class=\"govuk-warning-text__assistive\">Warning</span>Your organisation must have at least 12 months trading history and a training manager with at least 9 months experience in developing and delivering training. </strong> </div>",
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
      "Title": "Your organisation",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-12",
          "QuestionTag": "Organisation-Introduction-Supporting",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">In this section we''ll ask for:</p><ul class=\"govuk-list govuk-list--bullet\"><li>your organsiation''s ICO registration number</li><li>who''s in control and their date of births</li><li>your organisation''s trading history</li><li>details of what your organisation is</li><li>Ofsted or other educational accreditations</li></ul><p class=\"govuk-body\">Depending on your answers, we may ask further questions.</p> <div class=\"govuk-warning-text\"> <span class=\"govuk-warning-text__icon\" aria-hidden=\"true\">!</span> <strong class=\"govuk-warning-text__text\"> <span class=\"govuk-warning-text__assistive\">Warning</span>Your organisation must have at least 3 months trading history and a training manager with at least 3 months experience in developing and delivering training. </strong> </div>",
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
', N'Introduction and what you''ll need', N'Introduction and what you''ll need', N'Draft', N'Pages')

DECLARE @OrganisationIntroductionSequenceId UNIQUEIDENTIFIER
SET @OrganisationIntroductionSequenceId = 'B9A63CBD-4456-4DA4-8FAD-63D2F79D33F4'

INSERT INTO [dbo].[WorkflowSequences]
           ([Id]
           ,[WorkflowId]
           ,[SequenceNo]
		   ,[SectionNo]
		   ,[SectionId]
           ,[Status]
           ,[IsActive])
     VALUES
           (@OrganisationIntroductionSequenceId
           ,@WorkflowId
           ,1
		   ,2
		   ,@OrganisationIntroductionSectionId
           ,'Draft'
           ,1)

DECLARE @OrgansiationDetailsSectionId UNIQUEIDENTIFIER
SET @OrgansiationDetailsSectionId = '81DF6266-13E4-4928-8774-0C9E0C74B551'

INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle], [Status], [DisplayType])
VALUES
  (@OrgansiationDetailsSectionId, @ProjectId, N'
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
            "Type": "Radio",
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
				"Value": "[A-Za-z0-9]{2}[0-9]{4}[A-Za-z0-9]{2}",
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
      "BodyText": "",
	  "NotRequired": true
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
            "QuestionTag": "UKRLP-NoWebsite",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "60",
          "Condition": {
            "QuestionTag": "Apply-ProviderRoute",
            "MustEqual": "3"
          },
          "ConditionMet": false
        },
        {
          "Action": "NextPage",
          "ReturnId": "50",
          "Condition": {
            "QuestionTag": "Apply-ProviderRoute",
            "MustEqual": "1"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "51",
          "Condition": {
            "QuestionTag": "Apply-ProviderRoute",
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
                "FurtherQuestions": [
				{
                    "QuestionId": "YO-41",
                    "Hint": "",
                    "Label": "Enter main website address",
                    "Input": {
                      "Type": "Text",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter a website"
                        },
						{
                          "Name": "MaxLength",
                          "Value": 100,
                          "ErrorMessage": "Enter a website using 100 characters or less"
                        },
						{
							"Name": "Regex",
							"Value": "^(http:\\/\\/www\\.|https:\\/\\/www\\.|http:\\/\\/|https:\\/\\/)?[a-z0-9]+([\\-\\.]{1}[a-z0-9]+)*\\.[a-z]{2,5}(:[0-9]{1,5})?(\\/.*)?$",
							"ErrorMessage" : "Enter a website using the correct format. For example, www.company.co.uk"
						}
                      ]
                    },
                    "Order": null
                  }
				]
              },
              {
                "Label": "No",
                "Value": "No",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "ComplexRadioType",
                "Value": null,
                "ErrorMessage": "Tell us if your organisation has a website"
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
          "ReturnId": "60",
          "Condition": {
            "QuestionTag": "Apply-ProviderRoute",
            "MustEqual": "3"
          },
          "ConditionMet": false
        },
        {
          "Action": "NextPage",
          "ReturnId": "50",
          "Condition": {
            "QuestionTag": "Apply-ProviderRoute",
            "MustEqual": "1"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "51",
          "Condition": {
            "QuestionTag": "Apply-ProviderRoute",
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
      "Title": "How long has your organisation been trading for?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-50",
          "QuestionTag": "Confirm-Trading-Main",
          "Label": "How long has your organisation been trading for?",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">Trading includes buying, selling, advertising, renting a property or employing someone.</p>",
          "Hint": "",
          "Input": {
            "Type": "Radio",
			"Options": [
			{
                "Label": "Less than 12 months",
				"Value": "1",
                "FurtherQuestions": null
              },
			  {
                "Label": "12 to 18 months",
				"Value": "2",
                "FurtherQuestions": null
              },
			  {
                "Label": "19 to 23 months",
				"Value": "3",
                "FurtherQuestions": null
              },
			  {
                "Label": "More than 23 months",
				"Value": "4",
                "FurtherQuestions": null
              }
			],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us how long your organisation has been trading for"
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
          "ReturnId": "10001",
          "Condition": {
            "QuestionId": "YO-50",
            "MustEqual": "1"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "130",
          "Condition": {
            "QuestionTag": "CH-ManualEntryRequired",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "70",
          "Condition": {
            "QuestionTag": "UKRLP-Verification-Company",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "90",
          "Condition": {
            "QuestionTag": "CC-TrusteeManualEntry",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "80",
          "Condition": {
            "QuestionTag": "UKRLP-Verification-Charity",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "100",
          "Condition": {
            "QuestionTag": "UKRLP-Verification-SoleTradePartnership",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "140",
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
      "BodyText": "",
	  "NotRequired": true
    },
	{
      "PageId": "51",
      "SequenceId": "1",
      "SectionId": "3",
      "Title": "How long has your organisation been trading for?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-51",
          "QuestionTag": "Confirm-Trading-Employer",
          "Label": "How long has your organisation been trading for?",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">Trading includes buying, selling, advertising, renting a property or employing someone.</p>",
          "Hint": "",
          "Input": {
            "Type": "Radio",
			"Options": [
			{
                "Label": "Less than 12 months",
				"Value": "1",
                "FurtherQuestions": null
              },
			  {
                "Label": "12 to 18 months",
				"Value": "2",
                "FurtherQuestions": null
              },
			  {
                "Label": "19 to 23 months",
				"Value": "3",
                "FurtherQuestions": null
              },
			  {
                "Label": "More than 23 months",
				"Value": "4",
                "FurtherQuestions": null
              }
			],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us how long your organisation has been trading for"
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
          "ReturnId": "10002",
          "Condition": {
            "QuestionId": "YO-51",
            "MustEqual": "1"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "130",
          "Condition": {
            "QuestionTag": "CH-ManualEntryRequired",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "70",
          "Condition": {
            "QuestionTag": "UKRLP-Verification-Company",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "90",
          "Condition": {
            "QuestionTag": "CC-TrusteeManualEntry",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "80",
          "Condition": {
            "QuestionTag": "UKRLP-Verification-Charity",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "100",
          "Condition": {
            "QuestionTag": "UKRLP-Verification-SoleTradePartnership",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "140",
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
      "BodyText": "",	  
	  "NotRequired": true
    },
	{
      "PageId": "60",
      "SequenceId": "1",
      "SectionId": "3",
      "Title": "How long has your organisation been trading for?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-60",
          "QuestionTag": "Confirm-Trading-Supporting",
          "Label": "How long has your organisation been trading for?",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">Trading includes buying, selling, advertising, renting a property or employing someone.</p>",
          "Hint": "",
          "Input": {
            "Type": "Radio",
			"Options": [
			{
                "Label": "Less than 3 months",
				"Value": "1",
                "FurtherQuestions": null
              },
			  {
                "Label": "3 to 6 months",
				"Value": "2",
                "FurtherQuestions": null
              },
			  {
                "Label": "7 to 11 months",
				"Value": "3",
                "FurtherQuestions": null
              },
			  {
                "Label": "More than 11 months",
				"Value": "4",
                "FurtherQuestions": null
              }
			],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us how long your organisation has been trading for"
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
          "ReturnId": "10003",
          "Condition": {
            "QuestionId": "YO-60",
            "MustEqual": "1"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "130",
          "Condition": {
            "QuestionTag": "CH-ManualEntryRequired",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "70",
          "Condition": {
            "QuestionTag": "UKRLP-Verification-Company",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "90",
          "Condition": {
            "QuestionTag": "CC-TrusteeManualEntry",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "80",
          "Condition": {
            "QuestionTag": "UKRLP-Verification-Charity",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "100",
          "Condition": {
            "QuestionTag": "UKRLP-Verification-SoleTradePartnership",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "140",
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
      "BodyText": "",
	  "NotRequired": true
    },
	{
      "PageId": "10001",
      "SequenceId": "1",
      "SectionId": "3",
      "Title": "Your organisation is not eligible to apply to join RoATP",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "SHUT-001",
          "QuestionTag": "Shutter-TradingPeriod-Main",
          "Label": "Your organisation is not eligible to apply to join RoATP",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">To apply as a main provider, your organisation must have been trading for at least 12 months.</p><p class=\"govuk-body\">You can try again once your organisation has enough trading history.</p><p class=\"govuk-body\"><a href=\"https://www.gov.uk\">Back to GOV.UK</a></p>",
          "Hint": "",
          "Input": {
            "Type": "Hidden",
			"Validations": []
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "NotRequiredOrgTypes": [],
      "BodyText": "",
	  "NotRequired": true
    },
	{
      "PageId": "10002",
      "SequenceId": "1",
      "SectionId": "3",
      "Title": "Your organisation is not eligible to apply to join RoATP",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "SHUT-002",
          "QuestionTag": "Shutter-TradingPeriod-Employer",
          "Label": "Your organisation is not eligible to apply to join RoATP",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">To apply as an employer provider, your organisation must have been trading for at least 12 months.</p><p class=\"govuk-body\">You can try again once your organisation has enough trading history.</p><p class=\"govuk-body\"><a href=\"https://www.gov.uk\">Back to GOV.UK</a></p>",
          "Hint": "",
          "Input": {
            "Type": "Hidden",
			"Validations": []
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "NotRequiredOrgTypes": [],
      "BodyText": "",
	  "NotRequired": true
    },
	{
      "PageId": "10003",
      "SequenceId": "1",
      "SectionId": "3",
      "Title": "Your organisation is not eligible to apply to join RoATP",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "SHUT-003",
          "QuestionTag": "Shutter-TradingPeriod-Supporting",
          "Label": "Your organisation is not eligible to apply to join RoATP",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">To apply as a supporting provider, your organisation must have been trading for at least 3 months.</p><p class=\"govuk-body\">You can try again once your organisation has enough trading history.</p><p class=\"govuk-body\"><a href=\"https://www.gov.uk\">Back to GOV.UK</a></p>",
          "Hint": "",
          "Input": {
            "Type": "Hidden",
			"Validations": []
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "NotRequiredOrgTypes": [],
      "BodyText": "",
	  "NotRequired": true
    }
  ]
}
', N'Organisation details', N'Organisation details', N'Draft', N'Pages')

DECLARE @OrganisationDetailsSequenceId UNIQUEIDENTIFIER
SET @OrganisationDetailsSequenceId = '0B946C18-E335-4440-88D5-4345599F72E1'

INSERT INTO [dbo].[WorkflowSequences]
           ([Id]
           ,[WorkflowId]
           ,[SequenceNo]
		   ,[SectionNo]
		   ,[SectionId]
           ,[Status]
           ,[IsActive])
     VALUES
           (@OrganisationDetailsSequenceId
           ,@WorkflowId
           ,1
		   ,3
		   ,@OrgansiationDetailsSectionId
           ,'Draft'
           ,1)

DECLARE @ConfirmWhosInControlSectionId UNIQUEIDENTIFIER
SET @ConfirmWhosInControlSectionId = 'A3DA075D-858F-4498-8BFF-0360BD5EC459'

INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle], [Status], [DisplayType])
VALUES
  (@ConfirmWhosInControlSectionId, @ProjectId, N'
{
  "Pages": [   	
	{
      "PageId": "70",
      "SequenceId": "1",
      "SectionId": "4",
      "Title": "Confirm who''s in control",
      "LinkTitle": "",
      "InfoText": "",
	  "Details": {
        "Title": "What if these details are wrong?",
        "Body": "<p class=\"govuk-body\">Contact Companies House to <a href=\"https://www.gov.uk/file-changes-to-a-company-with-companies-house\" target=\"_blank\">change these details (opens in a new window or tab)</a>.</p><p class=\"govuk-body\">After your changes have been confirmed, sign back in to continue with this application.</p>"
      },
      "Questions": [
        {
          "QuestionId": "YO-70",
          "QuestionTag": "Companies-House-Directors",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">These are the details we found on Companies House.</p>",
          "Hint": "",
          "Input": {
            "Type": "TabularData",			
            "Validations": [              
            ]
          },
          "Order": null
        },
		{
          "QuestionId": "YO-71",
          "QuestionTag": "Companies-House-PSCs",
          "Label": "",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "TabularData",			
            "Validations": [              
            ]
          },
          "Order": null
        },
		{
          "QuestionId": "YO-75",
          "QuestionTag": "Directors-PSCs-Confirmed",
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
          "ReturnId": "90",
          "Condition": {
            "QuestionTag": "CC-TrusteeManualEntry",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "80",
          "Condition": {
            "QuestionTag": "UKRLP-Verification-Charity",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "140",
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
      "BodyText": "",
	  "NotRequired": true
    },
	{
      "PageId": "80",
      "SequenceId": "1",
      "SectionId": "4",
      "Title": "Confirm who''s in control",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-80",
          "QuestionTag": "Confirm-Trustees-NoDoB",
          "Label": "Confirm who''s in control",
          "ShortLabel": "",
          "QuestionBodyText": "PLACEHOLDER Confirm Trustees DOB",
          "Hint": "",
          "Input": {
            "Type": "Hidden",			
            "Validations": [              
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [      
		{
          "Action": "NextPage",
          "ReturnId": "140",
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
      "BodyText": "",
	  "NotRequired": true
    },		
	{
      "PageId": "90",
      "SequenceId": "1",
      "SectionId": "4",
      "Title": "Confirm who''s in control",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-90",
          "QuestionTag": "Add-Trustees",
          "Label": "Confirm who''s in control",
          "ShortLabel": "",
          "QuestionBodyText": "PLACEHOLDER Add Trustees",
          "Hint": "",
          "Input": {
            "Type": "Hidden",			
            "Validations": [              
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [      
		{
          "Action": "NextPage",
          "ReturnId": "140",
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
      "BodyText": "",
	  "NotRequired": true
    },
	{
      "PageId": "100",
      "SequenceId": "1",
      "SectionId": "4",
      "Title": "Tell us your organisation''s type",
      "LinkTitle": "",
      "InfoText": "",
	  "Details": {
        "Title": "What is a sole trader or partnership?",
        "Body": "<p class=\"govuk-body\">A sole trader is someone who''s self-employed and is the only owner of their business.</p><p class=\"govuk-body\">A partnership is when two or more people agree to share the profits, costs and risks of running a business.</p>"
      },
      "Questions": [
        {
          "QuestionId": "YO-100",
          "QuestionTag": "SoleTrade-or-Partnership",
          "Label": "Tell us your organisation''s type",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Radio",
			"Options": [
			{
                "Label": "Sole trader",
				"Value": "Sole trader",
                "FurtherQuestions": null
              },
			  {
                "Label": "Partnership",
				"Value": "Partnership",
                "FurtherQuestions": null
              }
			],
			"Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us what your organisation is"
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
          "ReturnId": "110",
          "Condition": {
            "QuestionId": "YO-100",
            "MustEqual": "Partnership"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "120",
          "Condition": {
            "QuestionId": "YO-100",
            "MustEqual": "Sole trader"
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
      "BodyText": "",
	  "NotRequired": true
    },	
	{
      "PageId": "110",
      "SequenceId": "1",
      "SectionId": "4",
      "Title": "PLACEHOLDER Add Partner",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-110",
          "QuestionTag": "Add-Partner",
          "Label": "PLACEHOLDER Add Partner",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Hidden",			
            "Validations": [              
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [      
		{
          "Action": "NextPage",
          "ReturnId": "140",
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
      "BodyText": "",
	  "NotRequired": true
    },
	{
      "PageId": "120",
      "SequenceId": "1",
      "SectionId": "4",
      "Title": "What is {{UKRLP-LegalName}}''s date of birth?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-120",
          "QuestionTag": "Add-SoleTrade-DOB",
          "Label": "What is {{UKRLP-LegalName}}''s date of birth?",
          "ShortLabel": "Date of birth",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "MonthAndYear",			
            "Validations": [
              {
                "Name": "MonthAndYear",
                "Value": null,
                "ErrorMessage": "Enter a date of birth using a month and year"
              },
			  {
                "Name": "MonthAndYearNotInFuture",
                "Value": null,
                "ErrorMessage": "Enter a date of birth using a month and year that''s in the past"
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
      "BodyText": "",
	  "NotRequired": true
    },
	{
      "PageId": "130",
      "SequenceId": "1",
      "SectionId": "4",
      "Title": "PLACEHOLDER Add People In Control",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-130",
          "QuestionTag": "Add-PeopleInControl",
          "Label": "PLACEHOLDER Add People In Control",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Hidden",			
            "Validations": [              
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [      
		{
          "Action": "NextPage",
          "ReturnId": "140",
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
      "BodyText": "",
	  "NotRequired": true
    }
  ]
}
', N'Confirm who''s in control', N'Confirm who''s in control', N'Draft', N'Pages')

DECLARE @ConfirmWhosInControlSequenceId UNIQUEIDENTIFIER
SET @ConfirmWhosInControlSequenceId = '2639B6CA-55F4-4578-9DDB-0B3E3F4A2139'

INSERT INTO [dbo].[WorkflowSequences]
           ([Id]
           ,[WorkflowId]
           ,[SequenceNo]
		   ,[SectionNo]
		   ,[SectionId]
           ,[Status]
           ,[IsActive])
     VALUES
           (@ConfirmWhosInControlSequenceId
           ,@WorkflowId
           ,1
		   ,4
		   ,@ConfirmWhosInControlSectionId
           ,'Draft'
           ,1)

DECLARE @DescribeYourOrganisationSectionId UNIQUEIDENTIFIER
SET @DescribeYourOrganisationSectionId = 'D9A819CD-93F5-4312-ABCC-B272B35E09DA'

INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle], [Status], [DisplayType])
VALUES
  (@DescribeYourOrganisationSectionId, @ProjectId, N'
{
  "Pages": [
    {
      "PageId": "140",
      "SequenceId": "1",
      "SectionId": "5",
      "Title": "What is your organisation?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-140",
          "QuestionTag": "Organisation-Type-MainSupporting",
          "Label": "What is your organisation?",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Radio",
            "Options": [
              {
                "Label": "A Group Training Association",
                "Value": "A Group Training Association",
                "FurtherQuestions": null
              },
              {
                "Label": "A public body",
                "Value": "A public body",
                "FurtherQuestions": null
              },
			  {
                "Label": "An Apprenticeship Training Agency",
                "Value": "An Apprenticeship Training Agency",
                "FurtherQuestions": null
              },
			  {
                "Label": "An educational institute",
                "Value": "An educational institute",
                "FurtherQuestions": null
              },
			  {
                "Label": "An employer training apprentices in other organisations",
                "Value": "An employer training apprentices in other organisations",
                "FurtherQuestions": null
              },
			  {
                "Label": "An Independent Training Provider",
                "Value": "An Independent Training Provider",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us what your organisation is"
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
          "ReturnId": "160",
          "Condition": {
            "QuestionId": "YO-140",
            "MustEqual": "An educational institute"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "230",
          "Condition": {
            "QuestionId": "YO-140",
            "MustEqual": "An employer training apprentices in other organisations"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "170",
          "Condition": {
            "QuestionId": "YO-140",
            "MustEqual": "A public body"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "230",
          "Condition": {
            "QuestionId": "YO-140",
            "MustEqual": "An Apprenticeship Training Agency"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "230",
          "Condition": {
            "QuestionId": "YO-140",
            "MustEqual": "An Independent Training Provider"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "230",
          "Condition": {
            "QuestionId": "YO-140",
            "MustEqual": "A Group Training Association"
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
      "PageId": "150",
      "SequenceId": "1",
      "SectionId": "5",
      "Title": "What is your organisation?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-150",
          "QuestionTag": "Organisation-Type-Employer",
          "Label": "What is your organisation?",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Radio",
            "Options": [
              {              
                "Label": "A public body",
                "Value": "A public body",
                "FurtherQuestions": null
              },
			  {
                "Label": "An educational institute",
                "Value": "An educational institute",
                "FurtherQuestions": null
              },
			  {
                "Label": "None of the above",
                "Value": "None of the above",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us what your organisation is"
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
          "ReturnId": "161",
          "Condition": {
            "QuestionId": "YO-150",
            "MustEqual": "An educational institute"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "171",
          "Condition": {
            "QuestionId": "YO-150",
            "MustEqual": "A public body"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "220",
          "Condition": {
            "QuestionTag": "CH-ManualEntryRequired",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "220",
          "Condition": {
            "QuestionTag": "UKRLP-Verification-Company",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "220",
          "Condition": {
            "QuestionTag": "CC-TrusteeManualEntry",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "220",
          "Condition": {
            "QuestionTag": "UKRLP-Verification-Charity",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "230",
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
      "PageId": "160",
      "SequenceId": "1",
      "SectionId": "5",
      "Title": "What type of educational institute is your organisation?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-160",
          "QuestionTag": "Organisation-EducationInstitute-MainSupporting",
          "Label": "What type of educational institute is your organisation?",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Radio",
            "Options": [
              {              
                "Label": "Academy",
                "Value": "Academy",
                "FurtherQuestions": null
              },
			  {
                "Label": "Further Education Institute",
                "Value": "Further Education Institute",
                "FurtherQuestions": null
              },
			  {
                "Label": "General Further Education College",
                "Value": "General Further Education College",
                "FurtherQuestions": null
              },
			  {
                "Label": "Higher Education Institute",
                "Value": "Higher Education Institute",
                "FurtherQuestions": null
              },
			  {
                "Label": "Multi-Academy Trust",
                "Value": "Multi-Academy Trust",
                "FurtherQuestions": null
              },
			  {
                "Label": "National College",
                "Value": "National College",
                "FurtherQuestions": null
              },
			  {
                "Label": "School",
                "Value": "School",
                "FurtherQuestions": null
              },
			  {
                "Label": "Sixth Form College",
                "Value": "Sixth Form College",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us what type of educational institute your organisation is"
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
          "ReturnId": "200",
          "Condition": {
            "QuestionId": "YO-160",
            "MustEqual": "Academy"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "200",
          "Condition": {
            "QuestionId": "YO-160",
            "MustEqual": "Further Education Institute"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "210",
          "Condition": {
            "QuestionId": "YO-160",
            "MustEqual": "General Further Education College"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "190",
          "Condition": {
            "QuestionId": "YO-160",
            "MustEqual": "Higher Education Institute"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "200",
          "Condition": {
            "QuestionId": "YO-160",
            "MustEqual": "Multi-Academy Trust"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "210",
          "Condition": {
            "QuestionId": "YO-160",
            "MustEqual": "National College"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "180",
          "Condition": {
            "QuestionId": "YO-160",
            "MustEqual": "School"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "210",
          "Condition": {
            "QuestionId": "YO-160",
            "MustEqual": "Sixth Form College"
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
      "PageId": "161",
      "SequenceId": "1",
      "SectionId": "5",
      "Title": "What type of educational institute is your organisation?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-161",
          "QuestionTag": "Organisation-EducationInstitute-Employer",
          "Label": "What type of educational institute is your organisation?",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Radio",
            "Options": [
              {              
                "Label": "Academy",
                "Value": "Academy",
                "FurtherQuestions": null
              },
			  {
                "Label": "Further Education Institute",
                "Value": "Further Education Institute",
                "FurtherQuestions": null
              },
			  {
                "Label": "General Further Education College",
                "Value": "General Further Education College",
                "FurtherQuestions": null
              },
			  {
                "Label": "Higher Education Institute",
                "Value": "Higher Education Institute",
                "FurtherQuestions": null
              },
			  {
                "Label": "Multi-Academy Trust",
                "Value": "Multi-Academy Trust",
                "FurtherQuestions": null
              },
			  {
                "Label": "National College",
                "Value": "National College",
                "FurtherQuestions": null
              },
			  {
                "Label": "School",
                "Value": "School",
                "FurtherQuestions": null
              },
			  {
                "Label": "Sixth Form College",
                "Value": "Sixth Form College",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us what type of educational institute your organisation is"
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
          "ReturnId": "201",
          "Condition": {
            "QuestionId": "YO-161",
            "MustEqual": "Academy"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "201",
          "Condition": {
            "QuestionId": "YO-161",
            "MustEqual": "Further Education Institute"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "211",
          "Condition": {
            "QuestionId": "YO-161",
            "MustEqual": "General Further Education College"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "191",
          "Condition": {
            "QuestionId": "YO-161",
            "MustEqual": "Higher Education Institute"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "201",
          "Condition": {
            "QuestionId": "YO-161",
            "MustEqual": "Multi-Academy Trust"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "211",
          "Condition": {
            "QuestionId": "YO-161",
            "MustEqual": "National College"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "181",
          "Condition": {
            "QuestionId": "YO-161",
            "MustEqual": "School"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "211",
          "Condition": {
            "QuestionId": "YO-161",
            "MustEqual": "Sixth Form College"
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
      "PageId": "170",
      "SequenceId": "1",
      "SectionId": "5",
      "Title": "What type of public body is your organisation?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-170",
          "QuestionTag": "Organisation-PublicBody-MS",
          "Label": "What type of public body is your organisation?",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Radio",
            "Options": [
              {              
                "Label": "Executive agency",
                "Value": "Executive agency",
                "FurtherQuestions": null
              },
			  {
                "Label": "Fire authority",
                "Value": "Fire authority",
                "FurtherQuestions": null
              },
			  {
                "Label": "Government department",
                "Value": "Government department",
                "FurtherQuestions": null
              },
			  {
                "Label": "Local authority",
                "Value": "Local authority",
                "FurtherQuestions": null
              },
			  {
                "Label": "NHS Trust",
                "Value": "NHS Trust",
                "FurtherQuestions": null
              },
			  {
                "Label": "Non-departmental public body (NDPB)",
                "Value": "Non-departmental public body (NDPB)",
                "FurtherQuestions": null
              },
			  {
                "Label": "Police",
                "Value": "Police",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us what type of public body your organisation is"
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
          "ReturnId": "230",
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
      "PageId": "171",
      "SequenceId": "1",
      "SectionId": "5",
      "Title": "What type of public body is your organisation?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-171",
          "QuestionTag": "Organisation-PublicBody-Emp",
          "Label": "What type of public body is your organisation?",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Radio",
            "Options": [
              {              
                "Label": "Executive agency",
                "Value": "Executive agency",
                "FurtherQuestions": null
              },
			  {
                "Label": "Fire authority",
                "Value": "Fire authority",
                "FurtherQuestions": null
              },
			  {
                "Label": "Government department",
                "Value": "Government department",
                "FurtherQuestions": null
              },
			  {
                "Label": "Local authority",
                "Value": "Local authority",
                "FurtherQuestions": null
              },
			  {
                "Label": "NHS Trust",
                "Value": "NHS Trust",
                "FurtherQuestions": null
              },
			  {
                "Label": "Non-departmental public body (NDPB)",
                "Value": "Non-departmental public body (NDPB)",
                "FurtherQuestions": null
              },
			  {
                "Label": "Police",
                "Value": "Police",
                "FurtherQuestions": null
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us what type of public body your organisation is"
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
          "ReturnId": "220",
          "Condition": {
            "QuestionTag": "UKRLP-Verification-Company",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "220",
          "Condition": {
            "QuestionTag": "UKRLP-Verification-Charity",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "230",
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
      "PageId": "180",
      "SequenceId": "1",
      "SectionId": "5",
      "Title": "What type of school is your organisation?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-180",
          "QuestionTag": "Organisation-School-MS",
          "Label": "What type of school is your organisation?",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Radio",
            "Options": [
              {              
                "Label": "Free school",
                "Value": "Free school"
              },
			  {
                "Label": "Local Education Authority (LEA) school",
                "Value": "Local Education Authority (LEA) school"
              },
			  {
                "Label": "None of the above",
                "Value": "None of the above"
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us what type of school your organisation is"
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
          "ReturnId": "200",
          "Condition": {
            "QuestionId": "YO-180",
            "MustEqual": "Free school"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "230",
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
      "PageId": "181",
      "SequenceId": "1",
      "SectionId": "5",
      "Title": "What type of school is your organisation?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-181",
          "QuestionTag": "Organisation-School-Emp",
          "Label": "What type of school is your organisation?",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Radio",
            "Options": [
              {              
                "Label": "Free school",
                "Value": "Free school"
              },
			  {
                "Label": "Local Education Authority (LEA) school",
                "Value": "Local Education Authority (LEA) school"
              },
			  {
                "Label": "None of the above",
                "Value": "None of the above"
              }
            ],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us what type of school your organisation is"
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
          "ReturnId": "201",
          "Condition": {
            "QuestionId": "YO-181",
            "MustEqual": "Free school"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "220",
          "Condition": {
            "QuestionTag": "UKRLP-Verification-Company",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "220",
          "Condition": {
            "QuestionTag": "UKRLP-Verification-Charity",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "230",
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
      "PageId": "190",
      "SequenceId": "1",
      "SectionId": "5",
      "Title": "Is your organisation funded by the Office for Students?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-190",
          "QuestionTag": "Organisation-OfSFunded-MS",
          "Label": "Is your organisation funded by the Office for Students?",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Radio",
			"Options": [
			{
                "Label": "Yes",
				"Value": "Yes"
              },
			  {
                "Label": "No",
				"Value": "No"
              }
			],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us if your organisation is funded by the Office for Students"
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
          "ReturnId": "230",
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
      "PageId": "191",
      "SequenceId": "1",
      "SectionId": "5",
      "Title": "Is your organisation funded by the Office for Students?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-191",
          "QuestionTag": "Organisation-OfSFunded-Emp",
          "Label": "Is your organisation funded by the Office for Students?",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Radio",
			"Options": [
			{
                "Label": "Yes",
				"Value": "Yes"
              },
			  {
                "Label": "No",
				"Value": "No"
              }
			],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us if your organisation is funded by the Office for Students"
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
          "ReturnId": "220",
          "Condition": {
            "QuestionTag": "UKRLP-Verification-Company",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "220",
          "Condition": {
            "QuestionTag": "UKRLP-Verification-Charity",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "230",
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
      "PageId": "200",
      "SequenceId": "1",
      "SectionId": "5",
      "Title": "Is your organisation already registered with ESFA?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-200",
          "QuestionTag": "Organisation-RegisteredESFA-MS",
          "Label": "Is your organisation already registered with ESFA?",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Radio",
			"Options": [
			{
                "Label": "Yes",
				"Value": "Yes"
              },
			  {
                "Label": "No",
				"Value": "No"
              }
			],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us if your organisation is already registered with ESFA"
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
          "ReturnId": "230",
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
      "PageId": "201",
      "SequenceId": "1",
      "SectionId": "5",
      "Title": "Is your organisation already registered with ESFA?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-201",
          "QuestionTag": "Organisation-RegisteredESFA-Emp",
          "Label": "Is your organisation already registered with ESFA?",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Radio",
			"Options": [
			{
                "Label": "Yes",
				"Value": "Yes"
              },
			  {
                "Label": "No",
				"Value": "No"
              }
			],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us if your organisation is already registered with ESFA"
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
          "ReturnId": "220",
          "Condition": {
            "QuestionTag": "UKRLP-Verification-Company",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "220",
          "Condition": {
            "QuestionTag": "UKRLP-Verification-Charity",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "230",
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
      "PageId": "210",
      "SequenceId": "1",
      "SectionId": "5",
      "Title": "Is your organisation receiving funding from ESFA?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-210",
          "QuestionTag": "Organisation-FundedESFA-MS",
          "Label": "Is your organisation receiving funding from ESFA?",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Radio",
			"Options": [
			{
                "Label": "Yes",
				"Value": "Yes"
              },
			  {
                "Label": "No",
				"Value": "No"
              }
			],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us if your organisation is receiving funding from ESFA"
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
          "ReturnId": "230",
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
      "PageId": "211",
      "SequenceId": "1",
      "SectionId": "5",
      "Title": "Is your organisation receiving funding from ESFA?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-211",
          "QuestionTag": "Organisation-FundedESFA-Emp",
          "Label": "Is your organisation receiving funding from ESFA?",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Radio",
			"Options": [
			{
                "Label": "Yes",
				"Value": "Yes"
              },
			  {
                "Label": "No",
				"Value": "No"
              }
			],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us if your organisation is receiving funding from ESFA"
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
          "ReturnId": "220",
          "Condition": {
            "QuestionTag": "UKRLP-Verification-Company",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "220",
          "Condition": {
            "QuestionTag": "UKRLP-Verification-Charity",
            "MustEqual": "TRUE"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "230",
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
      "PageId": "220",
      "SequenceId": "1",
      "SectionId": "5",
      "Title": "How will your organisation train its apprentices?",
      "LinkTitle": "",
      "InfoText": "",
	  "Details": {
		"Title": "What is a connected company or charity?",
		"Body": "<p class=\"govuk-body\">A connected company is part of the same group and has the same ultimate parent company as your organisation.</p><p class=\"govuk-body\">A connected charity is part of the same group of charities and has a majority of the same controlling trustees as your organisation.</p>"
	  },
      "Questions": [
        {
          "QuestionId": "YO-220",
          "QuestionTag": "Organisation-TrainingApprentices",
          "Label": "How will your organisation train its apprentices?",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Radio",
			"Options": [
			{
                "Label": "In your organisation",
				"Value": "In your organisation"
              },
			  {
                "Label": "In connected companies or charities",
				"Value": "In connected companies or charities"
              },
			  {
                "Label": "In your organisation and connected companies or charities",
				"Value": "In your organisation and connected companies or charities"
              }
			],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us how your organisation will train its apprentices"
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
          "ReturnId": "230",
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
      "PageId": "230",
      "SequenceId": "1",
      "SectionId": "5",
      "Title": "How would you describe your organisation?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-230",
          "QuestionTag": "Organisation-DescribeOrganisation",
          "Label": "How would you describe your organisation?",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">Select all that apply.</p>",
          "Hint": "",
          "Input": {
            "Type": "CheckBoxList",
			"Options": [
			  {
                "Label": "A public service mutual",
				"Value": "A public service mutual",
				"HintText": "An organisation that’s left the public sector but still delivers public services."
              },
			  {
                "Label": "A sheltered workshop",
				"Value": "A sheltered workshop",
				"HintText": "An organisation that provides employment opportunities for people who are developmentally, physically, or mentally impaired."
              },
			  {
                "Label": "A small or medium enterprise (SME)",
				"Value": "A small or medium enterprise (SME)",
				"HintText": "As explained by the <a href=\"https://ec.europa.eu/growth/smes/business-friendly-environment/sme-definition_en\" target=\"blank\">European Commission (opens in a new window or tab).</a>"
              },
			  {
                "Label": "A third sector organisation",
				"Value": "A third sector organisation",
				"HintText": "An organisation that does voluntary or community work. For example, a charity."
			  },
			  {
                "Label": "None of the above",
				"Value": "None of the above"
              }
			],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us how you would describe your organisation"
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
', N'Describe your organisation', N'Describe your organisation', N'Draft', N'Pages')

DECLARE @DescribeYourOrganisationSequenceId UNIQUEIDENTIFIER
SET @DescribeYourOrganisationSequenceId = '9E5A819F-2C74-4302-897C-1883DF65701D'

INSERT INTO [dbo].[WorkflowSequences]
           ([Id]
           ,[WorkflowId]
           ,[SequenceNo]
		   ,[SectionNo]
		   ,[SectionId]
           ,[Status]
           ,[IsActive])
     VALUES
           (@DescribeYourOrganisationSequenceId
           ,@WorkflowId
           ,1
		   ,5
		   ,@DescribeYourOrganisationSectionId
           ,'Draft'
           ,1)

DECLARE @ExperienceAccreditationsSectionId UNIQUEIDENTIFIER
SET @ExperienceAccreditationsSectionId = '0CCD068A-EE62-4CE8-8E06-659E97FD3696'

INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle], [Status], [DisplayType])
VALUES
  (@ExperienceAccreditationsSectionId, @ProjectId, N'
{
  "Pages": [
    {
      "PageId": "240",
      "SequenceId": "1",
      "SectionId": "6",
      "Title": "Does your organisation offer initial teacher training?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-240",
          "QuestionTag": "ITT-Accredited",
          "Label": "Does your organisation offer initial teacher training?",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">This means your organisation has been accredited by the Department for Education (DfE) to offer initial teacher training</p><p class=\"govuk-body\">Find out more about <a href=\"https://www.gov.uk/government/collections/initial-teacher-training\" target=\"blank\">initial teacher training (opens in a new window or tab)</a>.</p>",
          "Hint": "",
          "Input": {
            "Type": "Radio",
			"Options": [
			  {
                "Label": "Yes",
				"Value": "Yes"
              },
			  {
                "Label": "No",
				"Value": "No"
              }
			],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us if your organisation is Initial Teacher Training (ITT) accredited"
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
          "ReturnId": "250",
          "Condition": {
            "QuestionId": "YO-240",
            "MustEqual": "Yes"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "260",
          "Condition": {
            "QuestionId": "YO-240",
            "MustEqual": "No"
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
      "PageId": "250",
      "SequenceId": "1",
      "SectionId": "6",
      "Title": "Is the postgraduate teaching apprenticeship the only apprenticeship your organisation intends to deliver?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-250",
          "QuestionTag": "ITT-PGTA",
          "Label": "Is the postgraduate teaching apprenticeship the only apprenticeship your organisation intends to deliver?",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">The postgraduate teaching apprenticeship is an employment-based initial teacher training route leading to qualified teacher status.</p>",
          "Hint": "",
          "Input": {
            "Type": "Radio",
			"Options": [
			  {
                "Label": "Yes",
				"Value": "Yes"
              },
			  {
                "Label": "No",
				"Value": "No"
              }
			],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us if your organisation will only deliver post graduate teaching apprenticeships"
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
          "ReturnId": "260",
          "Condition": {
            "QuestionId": "YO-250",
            "MustEqual": "No"
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
    },
	{
      "PageId": "260",
      "SequenceId": "1",
      "SectionId": "6",
      "Title": "Has your organisation had a full Ofsted inspection in further education and skills?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-260",
          "QuestionTag": "Ofsted-FE-Skills",
          "Label": "Has your organisation had a full Ofsted inspection in further education and skills?",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">If you’re not sure, <a href=\"https://reports.ofsted.gov.uk/\" target=\"_blank\">check if your organisation’s had an Ofsted inspection (opens in a new window or tab)</a>.</p>",
          "Hint": "",
          "Input": {
            "Type": "Radio",
			"Options": [
			  {
                "Label": "Yes",
				"Value": "Yes"
              },
			  {
                "Label": "No",
				"Value": "No"
              }
			],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us if your organisation has had an Ofsted inspection in further education and skills"
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
          "ReturnId": "270",
          "Condition": {
            "QuestionId": "YO-260",
            "MustEqual": "Yes"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "290",
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
      "PageId": "270",
      "SequenceId": "1",
      "SectionId": "6",
      "Title": "Did your organisation get a grade for apprenticeships in this full Ofsted inspection?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-270",
          "QuestionTag": "Ofsted-Apprenticeship-Grade",
          "Label": "Did your organisation get a grade for apprenticeships in this full Ofsted inspection?",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">If you''re not sure, <a href=\"https://reports.ofsted.gov.uk/\" target=\"blank\">check your organisation''s Ofsted inspection report  (opens in a new window or tab)</a>.</p>",
          "Hint": "",
          "Input": {
            "Type": "Radio",
			"Options": [
			  {
                "Label": "Yes",
				"Value": "Yes"
              },
			  {
                "Label": "No",
				"Value": "No"
              }
			],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us if your organisation was awarded a grade for apprenticeships in this Ofsted inspection"
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
          "ReturnId": "280",
          "Condition": {
            "QuestionId": "YO-270",
            "MustEqual": "No"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "301",
          "Condition": {
            "QuestionTag": "Organisation-OfSFunded-MS",
            "MustEqual": "Yes"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "301",
          "Condition": {
            "QuestionTag": "Organisation-OfSFunded-Emp",
            "MustEqual": "Yes"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "300",
          "Condition": {
            "QuestionId": "YO-270",
            "MustEqual": "Yes"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "280",
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
      "PageId": "280",
      "SequenceId": "1",
      "SectionId": "6",
      "Title": "What grade did your organisation get for overall effectiveness in this full Ofsted inspection?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-280",
          "QuestionTag": "Ofsted-OE-Grade",
          "Label": "What grade did your organisation get for overall effectiveness in this full Ofsted inspection?",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Radio",
			"Options": [
			  {
                "Label": "Outstanding",
				"Value": "Outstanding"
              },
			  {
                "Label": "Good",
				"Value": "Good"
              },
			  {
                "Label": "Requires improvement",
				"Value": "Requires improvement"
              },
			  {
                "Label": "Inadequate",
				"Value": "Inadequate"
              }
			],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us what grade your organisation got"
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
          "ReturnId": null,
          "Condition": {
            "QuestionId": "YO-280",
            "MustEqual": "Requires improvement"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "311",
          "Condition": {
            "QuestionId": "YO-280",
            "MustEqual": "Inadequate"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "310",
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
      "PageId": "290",
      "SequenceId": "1",
      "SectionId": "6",
      "Title": "Has your organisation had a monitoring visit for apprenticeships in further education and skills?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-290",
          "QuestionTag": "Monitoring-Visit",
          "Label": "Has your organisation had a monitoring visit for apprenticeships in further education and skills?",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Radio",
			"Options": [
			  {
                "Label": "Yes",
				"Value": "Yes"
              },
			  {
                "Label": "No",
				"Value": "No"
              }
			],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us if your organisation has had a monitoring visit for apprenticeships in further education and skills"
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
    },
	{
      "PageId": "300",
      "SequenceId": "1",
      "SectionId": "6",
      "Title": "What grade did your organisation get for apprenticeships in this full Ofsted inspection?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-300",
          "QuestionTag": "Ofsted-Apprenticeships-Grade",
          "Label": "What grade did your organisation get for apprenticeships in this full Ofsted inspection?",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Radio",
			"Options": [
			  {
                "Label": "Outstanding",
				"Value": "Outstanding"
              },
			  {
                "Label": "Good",
				"Value": "Good"
              },
			  {
                "Label": "Requires improvement",
				"Value": "Requires improvement"
              },
			  {
                "Label": "Inadequate",
				"Value": "Inadequate"
              }
			],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us what grade your organisation got"
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
          "ReturnId": "280",
          "Condition": {
            "QuestionId": "YO-300",
            "MustEqual": "Requires improvement"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "311",
          "Condition": {
            "QuestionId": "YO-300",
            "MustEqual": "Inadequate"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "310",
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
      "PageId": "301",
      "SequenceId": "1",
      "SectionId": "6",
      "Title": "What grade did your organisation get for apprenticeships in this full Ofsted inspection?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-301",
          "QuestionTag": "Ofsted-Apprenticeships-Grade-OFS",
          "Label": "What grade did your organisation get for apprenticeships in this full Ofsted inspection?",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Radio",
			"Options": [
			  {
                "Label": "Outstanding",
				"Value": "Outstanding"
              },
			  {
                "Label": "Good",
				"Value": "Good"
              },
			  {
                "Label": "Requires improvement",
				"Value": "Requires improvement"
              },
			  {
                "Label": "Inadequate",
				"Value": "Inadequate"
              }
			],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us what grade your organisation got"
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
          "ReturnId": null,
          "Condition": {
            "QuestionId": "YO-301",
            "MustEqual": "Requires improvement"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "311",
          "Condition": {
            "QuestionId": "YO-301",
            "MustEqual": "Inadequate"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "310",
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
      "PageId": "310",
      "SequenceId": "1",
      "SectionId": "6",
      "Title": "Did your organisation get this grade within the last 3 years?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-310",
          "QuestionTag": "Ofsted-Grade-Last3Years",
          "Label": "Did your organisation get this grade within the last 3 years?",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">If you''re not sure, <a href=\"https://reports.ofsted.gov.uk/\" target=\"blank\">check your organisation''s Ofsted inspection report  (opens in a new window or tab)</a>.</p>",
          "Hint": "",
          "Input": {
            "Type": "Radio",
			"Options": [
			  {
                "Label": "Yes",
				"Value": "Yes"
              },
			  {
                "Label": "No",
				"Value": "No"
              }
			],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us if your organisation was awarded this grade within the last 3 years"
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
          "ReturnId": "320",
          "Condition": {
            "QuestionId": "YO-310",
            "MustEqual": "Yes"
          },
          "ConditionMet": false
        },
		{
          "Action": "NextPage",
          "ReturnId": "330",
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
      "PageId": "311",
      "SequenceId": "1",
      "SectionId": "6",
      "Title": "Did your organisation get this grade within the last 3 years?",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-311",
          "QuestionTag": "Ofsted-Grade-Last3Years",
          "Label": "Did your organisation get this grade within the last 3 years?",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">If you''re not sure, <a href=\"https://reports.ofsted.gov.uk/\" target=\"blank\">check your organisation''s Ofsted inspection report  (opens in a new window or tab)</a>.</p>",
          "Hint": "",
          "Input": {
            "Type": "Radio",
			"Options": [
			  {
                "Label": "Yes",
				"Value": "Yes"
              },
			  {
                "Label": "No",
				"Value": "No"
              }
			],
            "Validations": [
              {
                "Name": "Required",
                "Value": null,
                "ErrorMessage": "Tell us if your organisation was awarded this grade within the last 3 years"
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
          "ReturnId": "10004",
          "Condition": {
            "QuestionId": "YO-311",
            "MustEqual": "Yes"
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
    },
	{
      "PageId": "320",
      "SequenceId": "1",
      "SectionId": "6",
      "Title": "PLACEHOLDER Maintained Funding",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-320",
          "QuestionTag": "Maintained-Funding",
          "Label": "PLACEHOLDER Maintained Funding",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Hidden",
			"Options": [
			  
			],
            "Validations": [
             
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [		
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
    },
	{
      "PageId": "330",
      "SequenceId": "1",
      "SectionId": "6",
      "Title": "PLACEHOLDER Had Short Inspection",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "YO-330",
          "QuestionTag": "Had-Short-Inspection",
          "Label": "PLACEHOLDER Had Short Inspection",
          "ShortLabel": "",
          "QuestionBodyText": "",
          "Hint": "",
          "Input": {
            "Type": "Hidden",
			"Options": [
			  
			],
            "Validations": [
             
            ]
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [		
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
    },
	{
      "PageId": "10004",
      "SequenceId": "1",
      "SectionId": "6",
      "Title": "Your organisation is not eligible to apply to join RoATP",
      "LinkTitle": "",
      "InfoText": "",
      "Questions": [
        {
          "QuestionId": "SHUT-004",
          "QuestionTag": "Shutter-Inadequate-Grade-3-Years",
          "Label": "Your organisation is not eligible to apply to join RoATP",
          "ShortLabel": "",
          "QuestionBodyText": "<p class=\"govuk-body\">This is because your organisation got an ''inadequate'' grade within the last 3 years.</p><p class=\"govuk-body\"><a href=\"https://www.gov.uk\">Back to GOV.UK</a></p>",
          "Hint": "",
          "Input": {
            "Type": "Hidden",
			"Validations": []
          },
          "Order": null
        }
      ],
      "PageOfAnswers": [],
      "Next": [],
      "Complete": false,
      "AllowMultipleAnswers": false,
      "Order": null,
      "Active": true,
      "Visible": true,
      "Feedback": null,
      "HasFeedback": false,
      "NotRequiredOrgTypes": [],
      "BodyText": "",
	  "NotRequired": true
    }
  ]
}
', N'Experience and accreditations', N'Experience and accreditations', N'Draft', N'Pages')

DECLARE @ExperienceAccreditationsSequenceId UNIQUEIDENTIFIER
SET @ExperienceAccreditationsSequenceId = '803C1398-7B76-4351-94C4-7F1B641053EA'

INSERT INTO [dbo].[WorkflowSequences]
           ([Id]
           ,[WorkflowId]
           ,[SequenceNo]
		   ,[SectionNo]
		   ,[SectionId]
           ,[Status]
           ,[IsActive])
     VALUES
           (@ExperienceAccreditationsSequenceId
           ,@WorkflowId
           ,1
		   ,6
		   ,@ExperienceAccreditationsSectionId
           ,'Draft'
           ,1)



--INSERT INTO [dbo].[WorkflowSequences]
--           ([Id]
--           ,[WorkflowId]
--           ,[SequenceId]
--           ,[Status]
--           ,[IsActive]
--		   ,[Description])
--     VALUES
--           ('DEF40E31-71DA-40EC-8F33-9856C76C67DA'
--           ,'86F83D58-8608-4462-9A4E-65837AF04287'
--           ,2
--           ,'Draft'
--           ,1
--		   ,'Financial evidence')
		   
--INSERT INTO [dbo].[WorkflowSequences]
--			([Id]
--			,[WorkflowId]
--			,[SequenceId]
--			,[Status]
--			,[IsActive]
--			,[Description])
--VALUES
--			('B9FFC03D-2B9F-46C0-9481-44CD4C2E1E4F'
--			,'86F83D58-8608-4462-9A4E-65837AF04287'
--			,3
--			,'Draft'
--			,1
--			,'Criminal and compliance checks')

--INSERT INTO [dbo].[WorkflowSequences]
--			([Id]
--			,[WorkflowId]
--			,[SequenceId]
--			,[Status]
--			,[IsActive]
--			,[Description])
--VALUES
--			('4904E35B-6AF2-45C5-825B-EA41617287E1'
--			,'86F83D58-8608-4462-9A4E-65837AF04287'
--			,4
--			,'Draft'
--			,1
--			,'Apprenticeship welfare')
			
--INSERT INTO [dbo].[WorkflowSequences]
--			([Id]
--			,[WorkflowId]
--			,[SequenceId]
--			,[Status]
--			,[IsActive]
--			,[Description])
--VALUES
--			('8FA9490D-E7C0-40AE-97AF-38F0976B2A88'
--			,'86F83D58-8608-4462-9A4E-65837AF04287'
--			,5
--			,'Draft'
--			,1
--			,'Readiness to engage')

--INSERT INTO [dbo].[WorkflowSequences]
--			([Id]
--			,[WorkflowId]
--			,[SequenceId]
--			,[Status]
--			,[IsActive]
--			,[Description])
--VALUES
--			('C0D1550F-1372-404E-9CA9-D0021D190E7E'
--			,'86F83D58-8608-4462-9A4E-65837AF04287'
--			,6
--			,'Draft'
--			,1
--			,'People and planning')

--INSERT INTO [dbo].[WorkflowSequences]
--			([Id]
--			,[WorkflowId]
--			,[SequenceId]
--			,[Status]
--			,[IsActive]
--			,[Description])
--VALUES
--			('A8DC9146-4BEC-4B1E-B5AC-6D1698AF5FDF'
--			,'86F83D58-8608-4462-9A4E-65837AF04287'
--			,7
--			,'Draft'
--			,1
--			,'Leaders and managers')

DECLARE @ConditionsOfAcceptanceSectionId UNIQUEIDENTIFIER
SET @ConditionsOfAcceptanceSectionId = 'A56E170E-F602-47DB-97DE-A5765B86C97A'
           
INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle], [Status], [DisplayType])
VALUES
  (@ConditionsOfAcceptanceSectionId, @ProjectId, N'
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
', N'Conditions of acceptance', N'Conditions of acceptance', N'Draft', N'Pages')

DECLARE @ConditionsOfAcceptanceSequenceId UNIQUEIDENTIFIER
SET @ConditionsOfAcceptanceSequenceId = 'C636D66B-818C-478F-9970-68BFCED4F89A'

INSERT INTO [dbo].[WorkflowSequences]
           ([Id]
           ,[WorkflowId]
           ,[SequenceNo]
           ,[SectionNo]
           ,[SectionId]
           ,[Status]
           ,[IsActive])
     VALUES
           (@ConditionsOfAcceptanceSequenceId
           ,@WorkflowId
           ,99
           ,1
           ,@ConditionsOfAcceptanceSectionId
           ,'Draft'
           ,1)


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

delete from workflowSections where id = @ProtectionOfApprenticesSectionId
delete from workflowSections where id = @ProtectionOfApprentices2SectionId


INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle], [Status], [DisplayType])
VALUES
  (@ProtectionOfApprenticesSectionId, @ProjectId, N'
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
          "QuestionBodyText": "<p class=\"govuk-body\" id=\"pya-intro-main\">For this section you''ll need to upload your organisation''s:</p>
       <ul class=\"govuk-list govuk-list--bullet\">
	          <li>continuity plan for apprenticeship training</li>
        <li>equality and diversity policy</li>
        <li>safeguarding policy</li>
        <li>prevent duty policy (if needed)</li>
        <li>health and safety policy</li>
      </ul>
	  <div class=\"govuk-warning-text\">
  <span class=\"govuk-warning-text__icon\" aria-hidden=\"true\">!</span>
  <strong class=\"govuk-warning-text__text\">
    <span class=\"govuk-warning-text__assistive\">Warning</span>
    All policies and processes must be specific to your organisation, apprentices and trainers. They must also be signed by a senior employee. For example, a director or CEO. We will not accept policies or processes that are generic or taken from a third party.
  </strong>
	</div>",
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
          "QuestionBodyText": "<p class=\"govuk-body\" id=\"pya-intro-emp\">For this section you''ll need to upload your organisation''s:</p>
       <ul class=\"govuk-list govuk-list--bullet\">
	          <li>continuity plan for apprenticeship training</li>
        <li>equality and diversity policy</li>
        <li>safeguarding policy</li>
        <li>prevent duty policy (if needed)</li>
        <li>health and safety policy</li>
      </ul>
	  <div class=\"govuk-warning-text\">
  <span class=\"govuk-warning-text__icon\" aria-hidden=\"true\">!</span>
  <strong class=\"govuk-warning-text__text\">
    <span class=\"govuk-warning-text__assistive\">Warning</span>
    All policies and processes must be specific to your organisation, apprentices and trainers. They must also be signed by a senior employee. For example, a director or CEO. We will not accept policies or processes that are generic or taken from a third party.
  </strong>
	</div>",
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
          "QuestionBodyText": "<p class=\"govuk-body\" id=\"pya-intro-supp\">For this section you''ll need to upload your organisation''s:</p>
       <ul class=\"govuk-list govuk-list--bullet\">
        <li>equality and diversity policy</li>
        <li>safeguarding policy</li>
        <li>prevent duty policy (if needed)</li>
        <li>health and safety policy</li>
      </ul>
	  <div class=\"govuk-warning-text\">
  <span class=\"govuk-warning-text__icon\" aria-hidden=\"true\">!</span>
  <strong class=\"govuk-warning-text__text\">
    <span class=\"govuk-warning-text__assistive\">Warning</span>
    All uploads must be specific to your organisation, apprentices and trainers. They must also be signed by a senior employee. For example, a director or CEO. We will not accept uploads that are generic or taken from a third party.
  </strong>
	</div>",
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
', N'Introduction and what you''ll need', N'Introduction and what you''ll need', N'Draft', N'Pages')

INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle], [Status], [DisplayType])
VALUES
  (@ProtectionOfApprentices2SectionId, @ProjectId, N'
{
	"Pages": [
		{
			"PageId": "530",
			"SequenceId": "4",
			"SectionId": "2",
			"Title": "Continuity plan for apprenticeship training",
			"LinkTitle": "Continuity plan for apprenticeship training",
			"InfoText": "Continuity plan for apprenticeship training",
			 "Questions": [
        {
          "QuestionId": "PYA-20",
          "QuestionTag": "ProtectApprentice-Continuity",
          "Label": "Continuity plan for apprenticeship training",
          "ShortLabel": "",
          "QuestionBodyText": "Holding page",
          "Hint": "This is a holding page",
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
', N'Continuity plan for apprenticeship training', N'Continuity plan for apprenticeship training', N'Draft', N'Pages')


INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle], [Status], [DisplayType])
VALUES
  (@ProtectionOfApprentices3SectionId, @ProjectId, N'
{
	"Pages": [
		{
			"PageId": "540",
			"SequenceId": "4",
			"SectionId": "3",
			"Title": "Equality and Diversity policy",
			"LinkTitle": "Equality and Diversity policy",
			"InfoText": "Equality and Diversity policy",
			 "Questions": [
        {
          "QuestionId": "PYA-30",
          "QuestionTag": "ProtectApprentice-Equality",
          "Label": "Equality and Diversity policy",
          "ShortLabel": "",
          "QuestionBodyText": "Holding page",
          "Hint": "This is a holding page",
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
          "ReturnId": "550",
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
', N'Equality and Diversity policy', N'Equality and Diversity policy', N'Draft', N'Pages')



INSERT INTO [dbo].[WorkflowSequences]
			([Id]
			,[WorkflowId]
			,[SequenceNo]
			,[SectionNo]
			,[SectionId]
			,[Status]
			,[IsActive])
VALUES
			(@ProtectionOfApprenticesSequenceId
			,@WorkFlowId
			,4
			,1
			,@ProtectionOfApprenticesSectionId
			,'Draft'
			,1)

INSERT INTO [dbo].[WorkflowSequences]
			([Id]
			,[WorkflowId]
			,[SequenceNo]
			,[SectionNo]
			,[SectionId]
			,[Status]
			,[IsActive])
VALUES
			(@ProtectionOfApprentices2SequenceId
			,@WorkFlowId
			,4
			,2
			,@ProtectionOfApprentices2SectionId
			,'Draft'
			,1)

INSERT INTO [dbo].[WorkflowSequences]
			([Id]
			,[WorkflowId]
			,[SequenceNo]
			,[SectionNo]
			,[SectionId]
			,[Status]
			,[IsActive])
VALUES
			(@ProtectionOfApprentices3SequenceId
			,@WorkFlowId
			,4
			,3
			,@ProtectionOfApprentices3SectionId
			,'Draft'
			,1)

GO
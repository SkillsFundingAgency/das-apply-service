-- Section 0, Preamble setup
DECLARE @WorkflowId0 UNIQUEIDENTIFIER
DECLARE @ProjectId0 UNIQUEIDENTIFIER

SET @WorkflowId0 = '86F83D58-8608-4462-9A4E-65837AF04287'
SET @ProjectId0 = '70A0871F-42C1-48EF-8689-E63F0C91A487'


delete from workflowsections where id in
(select sectionID from WorkflowSequences where sequenceNo in (0)
)

delete from WorkflowSequences where sequenceNo in (0)

DECLARE @PreambleSectionId UNIQUEIDENTIFIER
SET @PreambleSectionId = '076D997E-7F59-4C66-91A3-CC9B63231413'
	 		  	
INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle], [DisplayType])
VALUES
  (@PreambleSectionId, @ProjectId0, N'
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
', N'Preamble', N'Preamble', N'Pages')
GO
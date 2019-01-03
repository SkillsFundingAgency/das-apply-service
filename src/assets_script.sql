-- delete from assets

if not exists(select * from assets where reference ='SQ-2-SE-4-PG-24-T-1')
	INSERT [dbo].[Assets] ([Id], [Reference], [Type], [Text], [Format], [Status], [CreatedAt], [CreatedBy], [UpdatedAt], [UpdatedBy], [DeletedAt], [DeletedBy]) 
	VALUES (N'20f75188-0ef0-46a2-80b3-04944fa47fcc', N'SQ-2-SE-4-PG-24-T-1', N'', N'Your policies and practices', N'', N'Live', getutcdate(), N'Scripted', NULL, NULL, NULL, NULL)
GO

if not exists(select * from assets where reference ='SQ-2-SE-4-PG-24-LT-1')
	INSERT [dbo].[Assets] ([Id], [Reference], [Type], [Text], [Format], [Status], [CreatedAt], [CreatedBy], [UpdatedAt], [UpdatedBy], [DeletedAt], [DeletedBy]) 
	VALUES (N'270f802b-6955-4376-896c-3a16e973ecee', N'SQ-2-SE-4-PG-24-LT-1', N'', N'Your policies and practices', N'', N'Live', getutcdate(), N'Scripted', NULL, NULL, NULL, NULL)
GO

if not exists(select * from assets where reference ='SQ-2-SE-4-PG-24-CC-03-L-1')
	INSERT [dbo].[Assets] ([Id], [Reference], [Type], [Text], [Format], [Status], [CreatedAt], [CreatedBy], [UpdatedAt], [UpdatedBy], [DeletedAt], [DeletedBy]) 
	VALUES (N'49c0f417-b66d-474c-a907-6fa46d335703', N'SQ-2-SE-4-PG-24-CC-03-L-1', N'', N'Upload a copy of your fraud and financial irregularity policy', N'', N'Live', getutcdate(), N'Scripted', NULL, NULL, NULL, NULL)
GO

if not exists(select * from assets where reference ='SQ-2-SE-4-PG-24-CC-07-L-1')
	INSERT [dbo].[Assets] ([Id], [Reference], [Type], [Text], [Format], [Status], [CreatedAt], [CreatedBy], [UpdatedAt], [UpdatedBy], [DeletedAt], [DeletedBy]) 
	VALUES (N'3b5549de-9ec1-4eed-b5ec-5ce1b632a241', N'SQ-2-SE-4-PG-24-CC-07-L-1', N'', N'Upload a copy of your insurance certificate', N'', N'Live', getutcdate(), N'Scripted', NULL, NULL, NULL, NULL)
GO

if not exists(select * from assets where reference ='SQ-2-SE-4-PG-24-CC-03-SL-1')
	INSERT [dbo].[Assets] ([Id], [Reference], [Type], [Text], [Format], [Status], [CreatedAt], [CreatedBy], [UpdatedAt], [UpdatedBy], [DeletedAt], [DeletedBy]) 
	VALUES (N'e8b003b7-e95d-40bd-ac6c-968f21eaa170', N'SQ-2-SE-4-PG-24-CC-03-SL-1', N'', N'Fraud & financial irregularity policy', N'', N'Live', getutcdate(), N'Scripted', NULL, NULL, NULL, NULL)
GO

if not exists(select * from assets where reference ='SQ-2-SE-4-PG-24-CC-07-SL-1')
	INSERT [dbo].[Assets] ([Id], [Reference], [Type], [Text], [Format], [Status], [CreatedAt], [CreatedBy], [UpdatedAt], [UpdatedBy], [DeletedAt], [DeletedBy]) 
	VALUES (N'3cac84ae-a24a-4298-8bca-9588c524f809', N'SQ-2-SE-4-PG-24-CC-07-SL-1', N'', N'Proof of insurance', N'', N'Live', getutcdate(), N'Scripted', NULL, NULL, NULL, NULL)
GO

if not exists(select * from assets where reference ='SQ-2-SE-4-PG-24-CC-03-QB-1')
	INSERT [dbo].[Assets] ([Id], [Reference], [Type], [Text], [Format], [Status], [CreatedAt], [CreatedBy], [UpdatedAt], [UpdatedBy], [DeletedAt], [DeletedBy]) 
	VALUES (N'c0ad0e34-2f98-486f-a2c3-116c324b4336', N'SQ-2-SE-4-PG-24-CC-03-QB-1', N'', N'', N'', N'Live', getutcdate(), N'Scripted', NULL, NULL, NULL, NULL)
GO

if not exists(select * from assets where reference ='SQ-2-SE-4-PG-24-CC-03-H-1')
	INSERT [dbo].[Assets] ([Id], [Reference], [Type], [Text], [Format], [Status], [CreatedAt], [CreatedBy], [UpdatedAt], [UpdatedBy], [DeletedAt], [DeletedBy]) 
	VALUES (N'ba8b0f1b-f662-47e9-a574-1f99337f4538', N'SQ-2-SE-4-PG-24-CC-03-H-1', N'', N'', N'', N'Live', getutcdate(), N'Scripted', NULL, NULL, NULL, NULL)
GO

if not exists(select * from assets where reference ='SQ-2-SE-4-PG-24-CC-07-QB-1')
	INSERT [dbo].[Assets] ([Id], [Reference], [Type], [Text], [Format], [Status], [CreatedAt], [CreatedBy], [UpdatedAt], [UpdatedBy], [DeletedAt], [DeletedBy]) 
	VALUES (N'1b6cb5ee-4ef2-4105-b9d2-9291911d0427', N'SQ-2-SE-4-PG-24-CC-07-QB-1', N'', N'', N'', N'Live', getutcdate(), N'Scripted', NULL, NULL, NULL, NULL)
GO

if not exists(select * from assets where reference ='SQ-2-SE-4-PG-24-CC-07-H-1')
	INSERT [dbo].[Assets] ([Id], [Reference], [Type], [Text], [Format], [Status], [CreatedAt], [CreatedBy], [UpdatedAt], [UpdatedBy], [DeletedAt], [DeletedBy]) 
	VALUES (N'3e0d0016-99ae-40a6-9c38-000f1f1d06c5', N'SQ-2-SE-4-PG-24-CC-07-H-1', N'', N'', N'', N'Live', getutcdate(), N'Scripted', NULL, NULL, NULL, NULL)
GO

if not exists (select * from workflowsections where id = 'b4951ead-ee4a-49f2-a31e-3a658605e32a')
INSERT [dbo].[WorkflowSections] ([Id], [WorkflowId], [SequenceId], [SectionId], [QnAData], [Title], [LinkTitle], [Status], [DisplayType], [DisallowedOrgTypes]) 
VALUES (N'b4951ead-ee4a-49f2-a31e-3a658605e32a', N'83b35024-8aef-440d-8f59-8c1cc459c350', 2, 4, N'
{
    "Pages": [
      {
        "PageId": "24",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-24-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-24-LT-1",
        "InfoText": "SQ-2-SE-4-PG-24-IT-1",
        "Questions": [
          {
            "QuestionId": "CC-03",
            "Label": "SQ-2-SE-4-PG-24-CC-03-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-03-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-03-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-03-H-1",
            "Input": {
              "Type": "FileUpload",
              "Options": null,
              "Validations": [
                {
                  "Name": "Required",
                  "Value": null,
                  "ErrorMessage": "Upload a file"
                }
              ]
            },
            "Order": null
          },
          {
            "QuestionId": "CC-07",
            "Label": "SQ-2-SE-4-PG-24-CC-07-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-07-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-07-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-07-H-1",
            "Input": {
              "Type": "FileUpload",
              "Options": null,
              "Validations": [
              ]
            },
            "Order": null
          }
        ],
        "PageOfAnswers": [],
        "Next": [
          {
            "Action": "ReturnToSection",
            "ReturnId": "4",
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
        "BodyText": "NP"
      }
    ],
    "FinancialApplicationGrade": null
  }  
', N'Capacity & Capability', N'Capacity & Capability', N'Draft', N'Pages', N'')
GO

-- NB NEED TO MAKE SAME FIX TO Section 3!!!!
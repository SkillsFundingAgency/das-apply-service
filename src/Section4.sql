

DELETE FROM WorkflowSections where SequenceId = 2 and SectionId = 4
INSERT [dbo].[WorkflowSections]
  ([Id], [WorkflowId], [SequenceId], [SectionId], [QnAData], [Title], [LinkTitle], [Status], [DisplayType], [DisallowedOrgTypes])
VALUES
  (N'b4951ead-ee4a-49f2-a31e-3a658605e32a', N'83b35024-8aef-440d-8f59-8c1cc459c350', 2, 4, N'
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
            "QuestionId": "CC-01",
            "Label": "SQ-2-SE-4-PG-24-CC-01-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-01-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-01-QB-1",
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
        "BodyText": "SQ-2-SE-4-PG-24-BT-1"
      }
    ],
    "FinancialApplicationGrade": null
  }  
', N'Apply to assess a standard', N'Apply to assess a standard', N'Draft', N'Pages', N'')
GO
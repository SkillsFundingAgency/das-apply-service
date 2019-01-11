

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
            "Hint": "SQ-2-SE-4-PG-24-CC-01-H-1",
            "Input": {
              "Type": "text",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Enter your ICO registration number"
                        }
                      ]
            },
            "Order": null
          },
          {
            "QuestionId": "CC-02",
            "Label": "SQ-2-SE-4-PG-24-CC-02-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-02-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-02-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-02-H-1",
            "Input": {
              "Type": "FileUpload",
              "Options": null,
              "Validations": [
                {
                  "Name": "Required",
                  "Value": null,
                  "ErrorMessage": "Upload your PDF policy document"
                }
              ]
            },
            "Order": null
          },
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
                  "ErrorMessage": "Upload your PDF of your public liability certificate of insurance"
                }
              ]
            },
            "Order": null
          },
          {
            "QuestionId": "CC-04",
            "Label": "SQ-2-SE-4-PG-24-CC-04-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-04-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-04-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-04-H-1",
            "Input": {
              "Type": "FileUpload",
              "Options": null,
              "Validations": [
                {
                  "Name": "Required",
                  "Value": null,
                  "ErrorMessage": "Upload your PDF of your professional indemnity certificate of insurance"
                }
              ]
            },
            "Order": null
          },
          {
            "QuestionId": "CC-05",
            "Label": "SQ-2-SE-4-PG-24-CC-05-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-05-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-05-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-05-H-1",
            "Input": {
              "Type": "FileUpload",
              "Options": null,
              "Validations": []
            },
            "Order": null
          },
          {
            "QuestionId": "CC-06",
            "Label": "SQ-2-SE-4-PG-24-CC-06-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-06-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-06-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-06-H-1",
            "Input": {
              "Type": "FileUpload",
              "Options": null,
              "Validations": [
                {
                  "Name": "Required",
                  "Value": null,
                  "ErrorMessage": "Upload your PDF of your policy safeguarding document"
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
                {
                  "Name": "Required",
                  "Value": null,
                  "ErrorMessage": "Upload your PDF Prevent Agenda policy document"
                }
              ]
            },
            "Order": null
          },
          {
            "QuestionId": "CC-08",
            "Label": "SQ-2-SE-4-PG-24-CC-08-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-08-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-08-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-08-H-1",
            "Input": {
              "Type": "FileUpload",
              "Options": null,
              "Validations": [
                {
                  "Name": "Required",
                  "Value": null,
                  "ErrorMessage": "Upload your PDF conflict of interest policy document"
                }
              ]
            },
            "Order": null
          },
          {
            "QuestionId": "CC-09",
            "Label": "SQ-2-SE-4-PG-24-CC-09-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-09-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-09-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-09-H-1",
            "Input": {
              "Type": "FileUpload",
              "Options": null,
              "Validations": [
                {
                  "Name": "Required",
                  "Value": null,
                  "ErrorMessage": "Upload your PDF procedure for monitoring assessor policy document"
                }
              ]
            },
            "Order": null
          },
          {
            "QuestionId": "CC-10",
            "Label": "SQ-2-SE-4-PG-24-CC-10-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-10-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-10-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-10-H-1",
            "Input": {
              "Type": "FileUpload",
              "Options": null,
              "Validations": [
                {
                  "Name": "Required",
                  "Value": null,
                  "ErrorMessage": "Upload your PDF standardisation and moderation policy document"
                }
              ]
            },
            "Order": null
          },
          {
            "QuestionId": "CC-11",
            "Label": "SQ-2-SE-4-PG-24-CC-11-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-11-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-11-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-11-H-1",
            "Input": {
              "Type": "FileUpload",
              "Options": null,
              "Validations": [
                {
                  "Name": "Required",
                  "Value": null,
                  "ErrorMessage": "Upload your PDF compalints policy document"
                }
              ]
            },
            "Order": null
          },
          {
            "QuestionId": "CC-12",
            "Label": "SQ-2-SE-4-PG-24-CC-12-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-12-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-12-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-12-H-1",
            "Input": {
              "Type": "FileUpload",
              "Options": null,
              "Validations": [
                {
                  "Name": "Required",
                  "Value": null,
                  "ErrorMessage": "Upload your PDF fair access policy document"
                }
              ]
            },
            "Order": null
          },
          {
            "QuestionId": "CC-13",
            "Label": "SQ-2-SE-4-PG-24-CC-13-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-13-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-13-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-13-H-1",
            "Input": {
              "Type": "FileUpload",
              "Options": null,
              "Validations": [
                {
                  "Name": "Required",
                  "Value": null,
                  "ErrorMessage": "Upload your PDF comparability and consistency policy document"
                }
              ]
            },
            "Order": null
          },
		  {
            "QuestionId": "CC-14",
            "Label": "SQ-2-SE-4-PG-24-CC-14-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-24-CC-14-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-24-CC-14-QB-1",
            "Hint": "SQ-2-SE-4-PG-24-CC-14-H-1",
            "Input": {
              "Type": "text",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Field must not be empty"
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
      },
	  {
        "PageId": "25",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-25-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-25-LT-1",
        "InfoText": "SQ-2-SE-4-PG-25-IT-1",
        "Questions": [
		  {
            "QuestionId": "CC-16",
            "Label": "SQ-2-SE-4-PG-25-CC-16-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-25-CC-16-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-25-CC-16-QB-1",
            "Hint": "SQ-2-SE-4-PG-25-CC-16-H-1",
            "Input": {
              "Type": "text",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Field must not be empty"
                        }
                      ]
            },
            "Order": null
          },
		  {
            "QuestionId": "CC-19",
            "Label": "SQ-2-SE-4-PG-25-CC-19-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-25-CC-19-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-25-CC-19-QB-1",
            "Hint": "SQ-2-SE-4-PG-25-CC-19-H-1",
            "Input": {
              "Type": "text",
                      "Options": null,
                      "Validations": []
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
      },
	  {
        "PageId": "26",
        "SequenceId": "2",
        "SectionId": "4",
        "Title": "SQ-2-SE-4-PG-26-T-1",
        "LinkTitle": "SQ-2-SE-4-PG-26-LT-1",
        "InfoText": "SQ-2-SE-4-PG-26-IT-1",
        "Questions": [
		  {
            "QuestionId": "CC-20",
            "Label": "SQ-2-SE-4-PG-26-CC-20-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-26-CC-20-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-26-CC-20-QB-1",
            "Hint": "SQ-2-SE-4-PG-25-CC-16-H-1",
            "Input": {
              "Type": "text",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Provide number of assessors"
                        }
                      ]
            },
            "Order": null
          },
		  {
            "QuestionId": "CC-21",
            "Label": "SQ-2-SE-4-PG-26-CC-21-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-26-CC-21-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-26-CC-21-QB-1",
            "Hint": "SQ-2-SE-4-PG-25-CC-16-H-1",
            "Input": {
              "Type": "text",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "number of assessors - THIS SOUNDS WRONG"
                        }
                      ]
            },
            "Order": null
          },
		  {
            "QuestionId": "CC-22",
            "Label": "SQ-2-SE-4-PG-26-CC-22-L-1",
            "ShortLabel": "SQ-2-SE-4-PG-26-CC-22-SL-1",
            "QuestionBodyText": "SQ-2-SE-4-PG-26-CC-22-QB-1",
            "Hint": "SQ-2-SE-4-PG-25-CC-16-H-1",
            "Input": {
              "Type": "text",
                      "Options": null,
                      "Validations": [
                        {
                          "Name": "Required",
                          "Value": null,
                          "ErrorMessage": "Field must not be empty"
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
', N'Apply to assess a standard', N'Apply to assess a standard', N'Draft', N'Questions', N'')
GO
-- Section 5 setup Readiness to engage
DECLARE @WorkflowId5 UNIQUEIDENTIFIER
DECLARE @ProjectId5 UNIQUEIDENTIFIER

SET @WorkflowId5 = '86F83D58-8608-4462-9A4E-65837AF04287'
SET @ProjectId5 = '70A0871F-42C1-48EF-8689-E63F0C91A487'

DECLARE @ReadinessToEngageSequenceId UNIQUEIDENTIFIER
SET @ReadinessToEngageSequenceId = 'B9BDE27F-5B68-4094-A4FD-1837A007C3F9'

DECLARE @ReadinessToEngageSequence2Id UNIQUEIDENTIFIER
SET @ReadinessToEngageSequence2Id = 'E24A0F22-D93C-4B6C-9D1A-89B85CC0211F'

DECLARE @ReadinessToEngageSequence3Id UNIQUEIDENTIFIER
SET @ReadinessToEngageSequence3Id = '11529066-D793-48E6-9E02-5A2086AFC5A3'

DECLARE @ReadinessToEngageSequence4Id UNIQUEIDENTIFIER
SET @ReadinessToEngageSequence4Id = '6276C61D-FC68-42ED-AAEA-4E77A56959B6'

delete from workflowsequences where id = @ReadinessToEngageSequenceId
delete from workflowsequences where id = @ReadinessToEngageSequence2Id 
delete from workflowsequences where id = @ReadinessToEngageSequence3Id 
delete from workflowsequences where id = @ReadinessToEngageSequence4Id 

DECLARE @ReadinessToEngageSectionId UNIQUEIDENTIFIER
SET @ReadinessToEngageSectionId = '9F30C43A-1241-4C4F-B105-17F3B9395EB5'

DECLARE @ReadinessToEngageSection2Id UNIQUEIDENTIFIER
SET @ReadinessToEngageSection2Id = '9923783B-F08F-4D20-96F1-AFB008F338DD'

DECLARE @ReadinessToEngageSection3Id UNIQUEIDENTIFIER
SET @ReadinessToEngageSection3Id = 'E3856257-86A2-41B6-9ED5-FA5E3D83A1B3'

DECLARE @ReadinessToEngageSection4Id UNIQUEIDENTIFIER
SET @ReadinessToEngageSection4Id = 'E64600A8-B825-41AE-A4D8-9AF256F95F7A'

delete from workflowSections where id = @ReadinessToEngageSectionId
delete from workflowSections where id = @ReadinessToEngageSection2Id
delete from workflowSections where id = @ReadinessToEngageSection3Id
delete from workflowSections where id = @ReadinessToEngageSection4Id

INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle], [DisplayType])
VALUES
  (@ReadinessToEngageSectionId, @ProjectId5, N'
{
    "Pages": [{
            "PageId": "5000",
            "SequenceId": null,
            "SectionId": null,
            "Title": "Introduction and what you''ll need",
            "LinkTitle": "Introduction and what you''ll need",
            "InfoText": "",
            "Questions": [{
                    "QuestionId": "RTE-10",
                    "QuestionTag": "ReadinessToEngage-Introduction-Main",
                    "Label": "Readiness to engage",
                    "ShortLabel": "",
                    "QuestionBodyText": "<p class=\"govuk-body\" id=\"rte-intro-main\">For this section, you’ll need to know how your organisation will:</p>
                    <ul class=\"govuk-list govuk-list--bullet\">
                    <li>engage with employers and subcontractors</li>
                    <li>manage its relationship with employers and subcontractors</li>
                    <li>assess prior learning - including English and maths qualifications</li>
                    </ul>
                    <p class=\"govuk-body\">You''ll also need to upload your organisation''s:</p>
                    <ul class=\"govuk-list govuk-list--bullet\">
                    <li>complaints policy</li>
                    <li>contract for services template with employers</li>
                    <li>commitment statement template</li>
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
            "Next": [{
                    "Action": "NextPage",
                    "ReturnId": "5100",
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
            "PageId": "5010",
            "SequenceId": null,
            "SectionId": null,
            "Title": "Introduction and what you''ll need",
            "LinkTitle": "Introduction and what you''ll need",
            "InfoText": "",
            "Questions": [{
                    "QuestionId": "RTE-11",
                    "QuestionTag": "ReadinessToEngage-Introduction-Employer",
                    "Label": "Readiness to engage",
                    "ShortLabel": "",
                    "QuestionBodyText": "<p class=\"govuk-body\" id=\"rte-intro-emp\">For this section, you’ll need to know how your organisation will:</p>
                    <ul class=\"govuk-list govuk-list--bullet\">
                    <li>engage with subcontractors</li>
                    <li>manage its relationship with subcontractors</li>
                    <li>assess prior learning - including English and maths qualifications</li>
                    </ul>
                    <p class=\"govuk-body\">You''ll also need to upload your organisation''s:</p>
                    <ul class=\"govuk-list govuk-list--bullet\">
                    <li>commitment statement template</li>
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
            "Next": [{
                    "Action": "NextPage",
                    "ReturnId": "5100",
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
            "PageId": "5020",
            "SequenceId": null,
            "SectionId": null,
            "Title": "Introduction and what you''ll need",
            "LinkTitle": "Introduction and what you''ll need",
            "InfoText": "",
            "Questions": [{
                    "QuestionId": "RTE-12",
                    "QuestionTag": "ReadinessToEngage-Introduction-Supporting",
                    "Label": "Readiness to engage",
                    "ShortLabel": "",
                    "QuestionBodyText": "<p class=\"govuk-body\" id=\"rte-intro-supp\">For this section, you’ll need to know how your organisation will:</p>
                    <ul class=\"govuk-list govuk-list--bullet\">
                    <li>engage with employers and subcontractors</li>
                    <li>manage its relationship with employers and subcontractors</li>
                    <li>assess prior learning - including English and maths qualifications</li>
                    </ul>
                    <p class=\"govuk-body\">You''ll also need to upload your organisation''s:</p>
                    <ul class=\"govuk-list govuk-list--bullet\">
                    <li>complaints policy</li>
                    <li>contract for services template with employers</li>
                    <li>commitment statement template</li>
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
            "Next": [{
                    "Action": "NextPage",
                    "ReturnId": "5100",
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
  (@ReadinessToEngageSection2Id, @ProjectId5, N'
{
    "Pages": [{
            "PageId": "5100",
            "SequenceId": null,
            "SectionId": null,
            "Title": "Engaging with employers",
            "LinkTitle": "Engaging with employers",
            "InfoText": "",
            "Questions": [{
                    "QuestionId": "RTE-20",
                    "QuestionTag": "ReadinessToEngage-Engaging",
                    "Label": "Has your organisation engaged with employers to deliver apprenticeship training to their employees?",
                    "ShortLabel": "",
                    "QuestionBodyText": "<p class=\"govuk-body\" id=\"rte-engaging-all\"></p>",
                    "Hint": "",
                    "Input": {
                        "Type": "Radio",
                        "Options": [{
                                "Label": "Yes",
                                "Value": "Yes",
                                "FurtherQuestions": null
                            }, {
                                "Label": "No",
                                "Value": "No",
                                "FurtherQuestions": null
                            }
                        ],
                        "Validations": [{
                                "Name": "Required",
                                "Value": null,
                                "ErrorMessage": "Tell us if your organisation has engaged with employers to deliver apprenticeship training to their employees"
                            }
                        ]
                    },
                    "Order": null
                }
            ],
            "PageOfAnswers": [],
            "Next": [{
                    "Action": "NextPage",
                    "ReturnId": "5110",
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
            "PageId": "5110",
            "SequenceId": null,
            "SectionId": null,
            "Title": "Engaging with employers",
            "LinkTitle": "Engaging with employers",
            "InfoText": "",
            "Questions": [{
                    "QuestionId": "RTE-21",
                    "QuestionTag": "ReadinessToEngage-ManageRelationship-How",
                    "Label": "How will your organisation manage its relationships with employers?",
                    "ShortLabel": "",
                    "QuestionBodyText": "<p class=\"govuk-body\" id=\"rte-managerelationship-how-all\">Your answer should include how your organisation will:</p>
                    <ul class=\"govuk-list govuk-list--bullet\">
                    <li>continuously monitor and improve employer engagement</li>
                    <li>regularly review feedback from employers</li>
                    <li>manage concerns and issues raised by employers</li>
                    <li>manage communication with employers</li>
                    </ul>",
                    "Hint": "",
                    "Input": {
                        "Type": "Textarea",
                        "Validations": [{
                                "Name": "Required",
                                "Value": null,
                                "ErrorMessage": "Enter how you continuously improve the quality of your assessment practice"
                            }, {
                                "Name": "MaxCharCount",
                                "Value": 750,
                                "ErrorMessage": "Your answer must be 750 characters or less"
                            }
                        ]
                    },
                    "Order": null
                }
            ],
            "PageOfAnswers": [],
            "Next": [{
                    "Action": "NextPage",
                    "ReturnId": "5120",
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
            "PageId": "5120",
            "SequenceId": null,
            "SectionId": null,
            "Title": "Engaging with employers",
            "LinkTitle": "Engaging with employers",
            "InfoText": "",
            "Questions": [{
                    "QuestionId": "RTE-22.1",
                    "QuestionTag": "",
                    "QuestionBodyText": "",
                    "Input": {
                        "Type": "text",
                        "Validations": [{
                                "Name": "Required",
                                "Value": null,
                                "ErrorMessage": "Enter a full name"
                            }, {
                                "Name": "MaxCharCount",
                                "Value": 255,
                                "ErrorMessage": "Enter a full name using 255 characters or less"
                            }
                        ],
                        "Options": [],
                        "InputClasses": null,
                        "DataEndpoint": null
                    },
                    "Order": null,
                    "Value": null,
                    "ErrorMessages": null,
                    "Label": "Full name"
                }, {
                    "QuestionId": "RTE-22.2",
                    "QuestionTag": "",
                    "Label": "Email",
                    "QuestionBodyText": "",
                    "Input": {
                        "Type": "text",
                        "Validations": [{
                                "Name": "EmailAddressIsValid",
                                "Value": null,
                                "ErrorMessage": "Enter an email in the correct format, like name@example.com"
                            }, {
                                "Name": "Required",
                                "Value": null,
                                "ErrorMessage": "Enter an email"
                            }, {
                                "Name": "MaxCharCount",
                                "Value": 70,
                                "ErrorMessage": "Enter an email using 70 characters or less"
                            }
                        ],
                        "Options": [],
                        "InputClasses": null,
                        "DataEndpoint": null
                    },
                    "Order": null,
                    "Value": null,
                    "ErrorMessages": null
                }, {
                    "QuestionId": "RTE-22.2",
                    "QuestionTag": "",
                    "Label": "Contact number",
                    "QuestionBodyText": "",
                    "Input": {
                        "Type": "text",
                        "Validations": [{
                                "Name": "Regex",
                                "Value": "^[0-9]*$",
                                "ErrorMessage": "Enter a valid contact number using only numbers"
                            }, {
                                "Name": "Required",
                                "Value": null,
                                "ErrorMessage": "Enter a contact number using 10 digits or more"
                            }, {
                                "Name": "MaxCharCount",
                                "Value": null,
                                "ErrorMessage": "Enter a contact number using 20 digits or less"
                            }
                        ],
                        "Options": [],
                        "InputClasses": null,
                        "DataEndpoint": null
                    },
                    "Order": null,
                    "Value": null,
                    "ErrorMessages": null
                }
            ],
            "PageOfAnswers": [],
            "Next": [{
                    "Action": "NextPage",
                    "ReturnId": "5130",
                    "Condition": null,
                    "Conditions": [],
                    "ConditionMet": false
                }
            ],
            "Complete": false,
            "AllowMultipleAnswers": false,
            "Active": true,
            "NotRequiredOrgTypes": null,
            "NotRequiredConditions": [],
            "NotRequired": false,
            "BodyText": "<p class=\"govuk-body\" id=\"rte-managerelationship-who-all\">This should be someone who has the overall responsibility and can make decisions independently.</p><p class=\"govuk-body\">We''ll only contact them if there is a concern.</p>",
            "ActivatedByPageId": ""
        },
        {
            "PageId": "5130",
            "SequenceId": null,
            "SectionId": null,
            "Title": "Engaging with employers",
            "LinkTitle": "Engaging with employers",
            "InfoText": "",
            "Questions": [{
                    "QuestionId": "RTE-23",
                    "QuestionTag": "ReadinessToEngage-ManageRelationship-Promote",
                    "Label": "How will your organisation promote apprenticeships to employers?",
                    "ShortLabel": "",
                    "QuestionBodyText": "<p class=\"govuk-body\" id=\"rte-managerelationship-how-all\">For example, through find apprenticeship training, National Careers Services or other national branding.</p>",
                    "Hint": "",
                    "Input": {
                        "Type": "Textarea",
                        "Validations": [{
                                "Name": "Required",
                                "Value": null,
                                "ErrorMessage": "Enter how you promote apprenticeships to employers"
                            }, {
                                "Name": "MaxCharCount",
                                "Value": 750,
                                "ErrorMessage": "Your answer must be 750 characters or less"
                            }
                        ]
                    },
                    "Order": null
                }
            ],
            "PageOfAnswers": [],
            "Next": [{
                    "Action": "NextPage",
                    "ReturnId": "5200",
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
', N'Engaging with employers', N'Engaging with employers', N'Pages')



INSERT [dbo].[WorkflowSections]
  ([Id], [ProjectId], [QnAData], [Title], [LinkTitle], [DisplayType])
VALUES
  (@ReadinessToEngageSection3Id, @ProjectId5, N'
{
    "Pages": [{
            "PageId": "5200",
            "SequenceId": null,
            "SectionId": null,
            "Title": "Complaints policy",
            "LinkTitle": "Complaints policy",
            "InfoText": "",
            "Questions": [{
                    "QuestionId": "RTE-30",
                    "QuestionTag": "ReadinessToEngage-Compliants-Policy",
                    "Label": "Upload your organisation’s complaints policy",
                    "ShortLabel": "",
                    "QuestionBodyText": "<p class=\"govuk-body\" id=\"rte-complaints-policy\">This must include:</p>
					<ul class=\"govuk-list govuk-list--bullet\">
                    <li>what a complaint is</li>
                    <li>how to raise a complaint</li>
                    <li>how long it takes for a complaint to be resolved</li>
                    </ul>
					<p class=\"govuk-body\">The file must be a PDF and smaller than 5MB.</p>",
                    "Hint": "",
                    "Input": {
                        "Type": ""FileUpload",
                        "Validations": [{
                                "Name": "Required",
                                "Value": null,
                                "ErrorMessage": "Select your organisation''s complaints policy"
                            }, {
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
            "Next": [{
                    "Action": "NextPage",
                    "ReturnId": "5210",
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
        },{
            "PageId": "5210",
            "SequenceId": null,
            "SectionId": null,
            "Title": "Complaints policy",
            "LinkTitle": "Complaints policy",
            "InfoText": "",
            "Questions": [{
                    "QuestionId": "RTE-31",
                    "QuestionTag": "ReadinessToEngage-Compliants-Website",
                    "Label": "Enter the website link for your organisation’s complaints policy",
                    "ShortLabel": "",
                    "QuestionBodyText": "<p class=\"govuk-body\" id=\"rte-complaints-policy\">The complaints policy must be published on your organisation’s website and be available to apprentices and employers.</p>
					<p class=\"govuk-body\">Website link</p>",
                    "Hint": "For example, http://www.example.com/complaintspolicy",
                    "Input": {
                        "Type": ""Text",
                        "Validations": [{
                                "Name": "Required",
                                "Value": null,
                                "ErrorMessage": "Enter the website link for your organisation''s complaints policy"
                            }, {
                                "Name": "MaxCharCount",
                                "Value": 100,
                                "ErrorMessage": "Enter a website link using 100 characters or less"
                            }
                        ]
                    },
                    "Order": null
                }
            ],
            "PageOfAnswers": [],
            "Next": [{
                    "Action": "NextPage",
                    "ReturnId": "5300",
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
', N'Complaints policy', N'Complaints policy', N'Pages')


INSERT INTO [dbo].[WorkflowSequences]
			([Id]
			,[WorkflowId]
			,[SequenceNo]
			,[SectionNo]
			,[SectionId]
			,[IsActive])
VALUES
			(@ReadinessToEngageSequenceId
			,@WorkFlowId5
			,5
			,1
			,@ReadinessToEngageSectionId
			,1)

INSERT INTO [dbo].[WorkflowSequences]
			([Id]
			,[WorkflowId]
			,[SequenceNo]
			,[SectionNo]
			,[SectionId]
			,[IsActive])
VALUES
			(@ReadinessToEngageSequence2Id
			,@WorkFlowId5
			,5
			,2
			,@ReadinessToEngageSection2Id
			,1)

INSERT INTO [dbo].[WorkflowSequences]
			([Id]
			,[WorkflowId]
			,[SequenceNo]
			,[SectionNo]
			,[SectionId]
			,[IsActive])
VALUES
			(@ReadinessToEngageSequence3Id
			,@WorkFlowId5
			,5
			,3
			,@ReadinessToEngageSection3Id
			,1)

INSERT INTO [dbo].[WorkflowSequences]
			([Id]
			,[WorkflowId]
			,[SequenceNo]
			,[SectionNo]
			,[SectionId]
			,[IsActive])
VALUES
			(@ReadinessToEngageSequence4Id
			,@WorkFlowId5
			,5
			,4
			,@ReadinessToEngageSection4Id
			,1)

GO

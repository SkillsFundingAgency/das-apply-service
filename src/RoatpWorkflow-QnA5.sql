-- Section 5 setup Readiness to engage
DECLARE @WorkflowId5 UNIQUEIDENTIFIER
DECLARE @ProjectId5 UNIQUEIDENTIFIER

SET @WorkflowId5 = '86F83D58-8608-4462-9A4E-65837AF04287'
SET @ProjectId5 = '70A0871F-42C1-48EF-8689-E63F0C91A487'

DECLARE @ReadinessToEngageSequenceId UNIQUEIDENTIFIER
SET @ReadinessToEngageSequenceId = 'B9BDE27F-5B68-4094-A4FD-1837A007C3F9'

DECLARE @ReadinessToEngageSequence2Id UNIQUEIDENTIFIER
SET @ReadinessToEngageSequence2Id = 'E24A0F22-D93C-4B6C-9D1A-89B85CC0211F'

delete from workflowsequences where id = @ReadinessToEngageSequenceId
delete from workflowsequences where id = @ReadinessToEngageSequence2Id 

DECLARE @ReadinessToEngageSectionId UNIQUEIDENTIFIER
SET @ReadinessToEngageSectionId = '9F30C43A-1241-4C4F-B105-17F3B9395EB5'

DECLARE @ReadinessToEngageSection2Id UNIQUEIDENTIFIER
SET @ReadinessToEngageSection2Id = '9923783B-F08F-4D20-96F1-AFB008F338DD'

delete from workflowSections where id = @ReadinessToEngageSectionId
delete from workflowSections where id = @ReadinessToEngageSection2Id

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

GO

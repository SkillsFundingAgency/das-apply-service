--APR-2311 convert FinancialGrade to separate tables
INSERT INTO [dbo].[FinancialReview]
           (
            [ApplicationId]
           ,[Status]
           ,[SelectedGrade]
		   ,[FinancialDueDate]
           ,[GradedBy]
           ,[GradedOn]
           ,[Comments]
           ,[ExternalComments]
           ,[FinancialEvidences]
           ,[ClarificationRequestedOn]
           ,[ClarificationRequestedBy]
           ,[ClarificationResponse])
SELECT 
    [ApplicationId],
    [FinancialReviewStatus] as Status,                  
    JSON_VALUE(apply.FinancialGrade, '$.SelectedGrade') AS SelectedGrade,
	JSON_VALUE(apply.FinancialGrade, '$.FinancialDueDate') AS FinancialDueDate,
	JSON_VALUE(apply.FinancialGrade, '$.GradedBy') AS GradedBy,	
	JSON_VALUE(apply.FinancialGrade, '$.GradedDateTime') AS GradedOn,	
	JSON_VALUE(apply.FinancialGrade, '$.Comments') AS Comments,	
	JSON_VALUE(apply.FinancialGrade, '$.ExternalComments') AS ExternalComments,	
	JSON_QUERY(apply.FinancialGrade, '$.FinancialEvidences') AS FinancialEvidences,	
	JSON_VALUE(apply.FinancialGrade, '$.ClarificationRequestedOn') AS ClarificationRequestedOn,		
	JSON_VALUE(apply.FinancialGrade, '$.ClarificationRequestedBy') AS ClarificationRequestedBy,		
	JSON_VALUE(apply.FinancialGrade, '$.ClarificationResponse') AS ClarificationResponse
FROM [dbo].[Apply]
WHERE ApplicationID NOT IN (SELECT ApplicationId FROM FinancialReview) AND FinancialGrade IS NOT NULL

-- There may be up to 4 clarification files
INSERT INTO [dbo].[FinancialReviewClarificationFile]
            (applicationId, Filename)
SELECT
    apply.ApplicationId,
    FinancialGrade.Filename
FROM [dbo].[Apply] apply
CROSS APPLY OPENJSON(apply.FinancialGrade,'$.ClarificationFiles') WITH (Filename VARCHAR(MAX)) FinancialGrade
WHERE apply.FinancialGrade IS NOT NULL

-- END OF APR-2311
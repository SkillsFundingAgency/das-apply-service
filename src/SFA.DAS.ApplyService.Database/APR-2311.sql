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
           ,FinancialEvidences
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
        JSON_VALUE(apply.FinancialGrade, '$.FinancialEvidences') AS FinancialEvidences,	
		JSON_VALUE(apply.FinancialGrade, '$.ClarificationRequestedOn') AS ClarificationRequestedOn,		
		JSON_VALUE(apply.FinancialGrade, '$.ClarificationRequestedBy') AS ClarificationRequestedBy,		
		JSON_VALUE(apply.FinancialGrade, '$.ClarificationResponse') AS ClarificationResponse
  FROM [dbo].[Apply] apply
  where applicationID not in (select applicationId from FinancialReview)
  and FinancialGrade is not null

-- there can be up to 4 clarification files
INSERT INTO FinancialReviewClarificationFile (applicationId, Filename)
SELECT apply.ApplicationId, FinancialGrade.Filename
FROM [dbo].[Apply] apply
CROSS APPLY OPENJSON(apply.FinancialGrade,'$.ClarificationFiles') WITH (Filename varchar(255)) FinancialGrade
WHERE apply.FinancialGrade IS NOT NULL

-- END OF APR-2311
BEGIN TRAN

UPDATE ApplicationSections SET QnAData = JSON_MODIFY(QnAData, '$.Pages[0].PageOfAnswers[0].Answers[1].QuestionId','FHA-01')
WHERE SectionId = 3 
AND JSON_QUERY(QnAData, '$.Pages[0].PageOfAnswers[0].Answers') != '[]'

UPDATE ApplicationSections SET QnAData = REPLACE(JSON_MODIFY(QnaData, 'lax $.Pages[0].Questions[1]', NULL),',null','')
WHERE SectionId = 3 

ROLLBACK
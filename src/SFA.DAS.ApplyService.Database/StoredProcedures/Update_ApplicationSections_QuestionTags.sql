CREATE PROCEDURE [dbo].[Update_ApplicationSections_QuestionTags]
      @QuestionId NVARCHAR(50),
	  @QuestionTag NVARCHAR(50)
AS
BEGIN 
SET NOCOUNT ON

DECLARE @json nvarchar(max)
DECLARE @jsonQuestions nvarchar(max)

select id  into #applicationsToProcess from applicationSections

declare @ApplicationIdToProcess uniqueidentifier

-- process every applicationSection record
WHILE EXISTS (select * from #applicationsToProcess)
	BEGIN

	select top 1 @ApplicationIdToProcess = id from #applicationsToProcess
	select top 1 @json= json_QUERY(QnAData)  from ApplicationSections where Id = @ApplicationIdToProcess

	-- find the question id among questions
	select pages.[Key] as pagesKey,questions.[key] questionsKey,  
	JSON_Value(questions.value,'$.QuestionId') QuestionId, JSON_Value(questions.value,'$.QuestionTag') as QuestionTag
	 into #tmpQuestionProcessor
	 from OPENJSON(@json,'$.Pages') as pages
			CROSS APPLY OPENJSON(pages.value) as pageitem
			 CROSS APPLY OPENJSON(pageitem.value) as questions
			 where pageItem.[key]='Questions'
			 and  JSON_Value(questions.value,'$.QuestionId') = @questionId

	 -- find the question id among further questions
	 select pages.[key] pagesKey, questions.[key] questionsKey, furtherQuestions.[key] optionsKey,  fffQuestions.[key] furtherQuestionsKey, 
	 JSON_Value(fffQuestions.value,'$.QuestionId') QuestionId, JSON_Value(fffQuestions.value,'$.QuestionTag') as QuestionTag
	 into #tmpFurtherQuestionProcessor
	 from OPENJSON(@json,'$.Pages') as pages
			CROSS APPLY OPENJSON(pages.value) as pageitem
			 CROSS APPLY OPENJSON(pageitem.value) as questions
			  cross apply OPENJSON(questions.value) as questionsItem
			   cross apply OPENJSON(questionsItem.value) as options
				cross apply OPENJSON(options.value) as furtherQuestions
				 cross apply OPENJSON(furtherQuestions.value) as ffQuestions
	 				 cross apply OPENJSON(ffQuestions.value) as fffQuestions
			 where pageItem.[key]='Questions'
			 and questionsItem.[key] = 'Input'
			 and options.[key] = 'Options'
			 and JSON_Query(furtherQuestions.value,'$.FurtherQuestions') is not null
			 and ffQuestions.[key]= 'FurtherQuestions'
			 and  JSON_Value(fffQuestions.value,'$.QuestionId') = @questionId
	 
	 DECLARE @jsonQuestionId varchar(max)
	 DECLARE @jsonQuestionTag varchar(max)

	 if exists (select * from #tmpQuestionProcessor)
	 BEGIN

	 -- process any question found
	  select top 1 @jsonQuestionId = '$.Pages[' +pagesKey + '].Questions[' + questionsKey + '].QuestionId',
				 @jsonQuestionTag = '$.Pages[' +pagesKey + '].Questions[' + questionsKey + '].QuestionTag'
				  from #tmpQuestionProcessor		
	
		update ApplicationSections 
				set QnAData = JSON_MODIFY(QnAData, @jsonQuestionTag, @QuestionTag)
					where id = @ApplicationIdToProcess 
					and JSON_VALUE(QnAData, @jsonQuestionId) = @questionId 
					and ( JSON_VALUE(QnAData, @jsonQuestionTag) != @QuestionTag OR  JSON_VALUE(QnAData, @jsonQuestionTag) is null  OR @QuestionTag is null)
	 END

	 -- process any further question found
	  if exists (select * from #tmpFurtherQuestionProcessor)
		 BEGIN
		   select top 1 @jsonQuestionId = '$.Pages[' +pagesKey + '].Questions[' + questionsKey + '].Input.Options[' + optionsKey +'].FurtherQuestions[' + furtherQuestionsKey + '].QuestionId',
					 @jsonQuestionTag = '$.Pages[' +pagesKey + '].Questions[' + questionsKey + '].Input.Options[' + optionsKey +'].FurtherQuestions[' + furtherQuestionsKey + '].QuestionTag'
					  from #tmpFurtherQuestionProcessor
			
					update ApplicationSections 
					set QnAData = JSON_MODIFY(QnAData, @jsonQuestionTag, @QuestionTag)
						where id = @ApplicationIdToProcess 
						and JSON_VALUE(QnAData, @jsonQuestionId) = @questionId 
						and ( JSON_VALUE(QnAData, @jsonQuestionTag) != @QuestionTag OR  JSON_VALUE(QnAData, @jsonQuestionTag) is null OR @QuestionTag is null)
		 END

	 drop table #tmpQuestionProcessor
	 drop table #tmpFurtherQuestionProcessor
	 delete from #applicationsToProcess where id = @ApplicationIdToProcess
	END


 drop table #applicationsToProcess

 END 
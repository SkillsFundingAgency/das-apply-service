using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.Services
{
    public class ExtractAnswerValueService: IExtractAnswerValueService
    {

        public string ExtractAnswerValueFromQuestionId(IReadOnlyCollection<AssessorAnswer> answers, string questionId)
        {
            if (answers == null) return string.Empty;
            if (questionId == null) return string.Empty;
            var answer = answers.FirstOrDefault(x => x.QuestionId == questionId);
            return answer?.Value;
        }

        public  string ExtractFurtherQuestionAnswerValueFromQuestionId(AssessorPage assessorPage, string questionId)
        {
            var mainQuestionAnswer = ExtractAnswerValueFromQuestionId(assessorPage.Answers, questionId);
            var furtherQuestionsQuestionId = assessorPage?.Questions
                .FirstOrDefault(x => x.QuestionId == questionId)?.Options?
                .FirstOrDefault(o => o.Value == mainQuestionAnswer)?.FurtherQuestions?.FirstOrDefault()
                ?.QuestionId;

            return ExtractAnswerValueFromQuestionId(assessorPage?.Answers, furtherQuestionsQuestionId);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.Services.Assessor
{
    public class ExtractAnswerValueService: IExtractAnswerValueService
    {

        public string ExtractAnswerValueFromQuestionId(IReadOnlyCollection<AssessorAnswer> answers, string questionId)
        {
            if (answers == null) return string.Empty;
            if (questionId == null) return string.Empty;
            var answer = answers.FirstOrDefault(x => x.QuestionId == questionId);
            return answer?.Value ?? string.Empty;
        }

        public  string ExtractFurtherQuestionAnswerValueFromQuestionId(AssessorPage assessorPage, string questionId)
        {
            var mainQuestionAnswer = ExtractAnswerValueFromQuestionId(assessorPage.Answers, questionId);

            var furtherQuestionsQuestionId = assessorPage?.Questions?
                .FirstOrDefault(x => x.QuestionId == questionId)?.Options?
                .FirstOrDefault(o => o.Value == mainQuestionAnswer)?.FurtherQuestions?.FirstOrDefault()
                ?.QuestionId;

            return ExtractAnswerValueFromQuestionId(assessorPage?.Answers, furtherQuestionsQuestionId);
        }
    }
}

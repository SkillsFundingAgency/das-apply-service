using System.Collections.Generic;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.Services
{
    public interface IExtractAnswerValueService
    {
        string ExtractAnswerValueFromQuestionId(IReadOnlyCollection<AssessorAnswer> answers, string questionId);
        string ExtractFurtherQuestionAnswerValueFromQuestionId(AssessorPage assessorPage, string questionId);
    }
}

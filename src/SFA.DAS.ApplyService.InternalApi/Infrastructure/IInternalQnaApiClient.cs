using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    public interface IInternalQnaApiClient
    {
        Task<string> GetQuestionTag(Guid applicationId, string questionTag);
        Task<Page> GetPageBySectionNo(Guid applicationId, int sequenceNo, int sectionNo, string pageId);
        Task<string> GetAnswerValue(Guid applicationId, int sequenceNo, int sectionNo, string pageId, string questionId);
        Task<string> GetAnswerValueFromActiveQuestion(Guid applicationId, int sequenceNo, int sectionNo, params PageAndQuestion[] possibleQuestions);
    }
}

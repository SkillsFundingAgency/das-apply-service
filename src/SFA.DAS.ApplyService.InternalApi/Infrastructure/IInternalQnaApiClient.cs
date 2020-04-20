using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    public interface IInternalQnaApiClient
    {
        Task<string> GetQuestionTag(Guid applicationId, string questionTag);
        Task<Page> GetPageBySectionNo(Guid applicationId, int sequenceNo, int sectionNo, string pageId);
        Task<string> GetAnswerValue(Guid applicationId, int sequenceNo, int sectionNo, string pageId, string questionId);

        Task<Answer> GetAnswerByTag(Guid applicationId, string questionTag, string questionId = null);

        Task<TabularData> GetTabularDataByTag(Guid applicationId, string questionTag);
    }
}

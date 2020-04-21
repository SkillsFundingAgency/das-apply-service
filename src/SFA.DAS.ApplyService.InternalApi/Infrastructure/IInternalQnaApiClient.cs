using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    public interface IInternalQnaApiClient
    {
        Task<string> GetQuestionTag(Guid applicationId, string questionTag);
        Task<Page> GetPageBySectionNo(Guid applicationId, int sequenceNo, int sectionNo, string pageId);
        Task<string> GetAnswerValue(Guid applicationId, int sequenceNo, int sectionNo, string pageId, string questionId);

        Task<FileStreamResult> GetDownloadFile(Guid applicationId, int sequenceNo, int sectionNo, string pageId, string questionId);

        Task<Answer> GetAnswerByTag(Guid applicationId, string questionTag, string questionId = null);

        Task<TabularData> GetTabularDataByTag(Guid applicationId, string questionTag);
    }
}

using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    public interface IInternalQnaApiClient
    {
        Task<ApplicationSequence> GetSequenceBySequenceNo(Guid applicationId, int sequenceNo);

        Task<IEnumerable<ApplicationSection>> GetSections(Guid applicationId, Guid sequenceId);

        Task<string> GetQuestionTag(Guid applicationId, string questionTag);
        Task<Page> GetPageBySectionNo(Guid applicationId, int sequenceNo, int sectionNo, string pageId);
        Task<string> GetAnswerValue(Guid applicationId, int sequenceNo, int sectionNo, string pageId, string questionId);
        Task<string> GetAnswerValueFromActiveQuestion(Guid applicationId, int sequenceNo, int sectionNo, string pageId, string questionId);
        Task<string> GetAnswerValueFromActiveQuestion(Guid applicationId, int sequenceNo, int sectionNo, params PageAndQuestion[] possibleQuestions);

        Task<FileStreamResult> GetDownloadFile(Guid applicationId, int sequenceNo, int sectionNo, string pageId, string questionId);

        Task<Answer> GetAnswerByTag(Guid applicationId, string questionTag, string questionId = null);

        Task<TabularData> GetTabularDataByTag(Guid applicationId, string questionTag);
    }
}


using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HPSF.Wellknown;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Apply;
    using Domain.Entities;
    using SFA.DAS.ApplyService.Application.Apply;
    using SFA.DAS.ApplyService.Application.Apply.UpdatePageAnswers;

    public interface IQnaApiClient
    {
        Task<StartApplicationResponse> StartApplication(string userReference, string workflowType, string applicationData);
        Task<ApplicationSequence> GetSequence(Guid applicationId, Guid sequenceId);
        Task<ApplicationSection> GetSection(Guid applicationId, Guid sectionId);
        Task<ApplicationSection> GetSectionBySectionNo(Guid applicationId, int sequenceNo, int sectionNo);
        Task<Page> GetPage(Guid applicationId, Guid sectionId, string pageId);
        Task<Answer> GetAnswer(Guid applicationId, Guid sectionId, string pageId, string questionId); 
        Task<SetPageAnswersResponse> UpdatePageAnswers(Guid applicationId, Guid sectionId, string pageId, List<Answer> answers);
        Task<IEnumerable<ApplicationSequence>> GetSequences(Guid applicationId);
        Task<IEnumerable<ApplicationSection>> GetSections(Guid applicationId, Guid sequenceId);
        Task<Answer> GetAnswerByTag(Guid applicationId, string questionTag, string questionId = null);

        Task<UploadPageAnswersResult> Upload(Guid applicationId, Guid sectionId, string pageId, IFormFileCollection files);

        Task<HttpResponseMessage> DownloadFile(Guid applicationId, Guid sectionId, string pageId, string questionId,
            string filename);

        Task DeleteFile(Guid applicationId, Guid sectionId, string pageId, string questionId, string fileName);

        Task<SkipPageResponse> SkipPageBySectionNo(Guid applicationId, int sequenceNo, int sectionNo, string pageId);
        Task<SkipPageResponse> SkipPage(Guid applicationId, Guid sectionId, string pageId);
    }
}

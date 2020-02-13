
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
    using Newtonsoft.Json.Linq;
    using SFA.DAS.ApplyService.Application.Apply;

    public interface IQnaApiClient
    {
        Task<StartQnaApplicationResponse> StartApplication(string userReference, string workflowType, string applicationData);

        Task<object> GetApplicationData(Guid applicationId);
        Task<object> GetQuestionTagData(Guid applicationId, string questionTag);

        Task<IEnumerable<ApplicationSequence>> GetSequences(Guid applicationId);
        Task<ApplicationSequence> GetSequence(Guid applicationId, Guid sequenceId);
        Task<ApplicationSequence> GetSequenceBySequenceNo(Guid applicationId, int sequenceNo);

        Task<IEnumerable<ApplicationSection>> GetSections(Guid applicationId);
        Task<IEnumerable<ApplicationSection>> GetSections(Guid applicationId, Guid sequenceId);
        Task<ApplicationSection> GetSection(Guid applicationId, Guid sectionId);
        Task<ApplicationSection> GetSectionBySectionNo(Guid applicationId, int sequenceNo, int sectionNo);
        Task<Page> GetPage(Guid applicationId, Guid sectionId, string pageId);
        Task<Answer> GetAnswer(Guid applicationId, Guid sectionId, string pageId, string questionId); 
        Task<SetPageAnswersResponse> UpdatePageAnswers(Guid applicationId, Guid sectionId, string pageId, List<Answer> answers);
        Task<Answer> GetAnswerByTag(Guid applicationId, string questionTag, string questionId = null);
        Task<UploadPageAnswersResult> Upload(Guid applicationId, Guid sectionId, string pageId, IFormFileCollection files);
        Task<Page> GetPageBySectionNo(Guid applicationId, int sequenceNo, int sectionNo, string pageId);
               
        Task<ResetPageAnswersResponse> ResetPageAnswers(Guid applicationId, Guid sectionId, string pageId);
        Task<AddPageAnswerResponse> AddPageAnswerToMultipleAnswerPage(Guid applicationId, Guid sectionId, string pageId, List<Answer> answer);
        Task<Page> RemovePageAnswerFromMultipleAnswerPage(Guid applicationId, Guid sectionId, string pageId, Guid answerId);

        Task<SkipPageResponse> SkipPage(Guid applicationId, Guid sectionId, string pageId);
        Task<SkipPageResponse> SkipPageBySectionNo(Guid applicationId, int sequenceNo, int sectionNo, string pageId);
        Task<HttpResponseMessage> DownloadFile(Guid applicationId, Guid sectionId, string pageId, string questionId, string filename);
        Task DeleteFile(Guid applicationId, Guid sectionId, string pageId, string questionId, string filename);
       
    }
}

﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{

    public interface IQnaApiClient
    {
        Task<StartQnaApplicationResponse> StartApplication(string userReference, string workflowType, string applicationData);

        Task<JObject> GetApplicationData(Guid applicationId);
        Task<string> GetQuestionTag(Guid applicationId, string questionTag);

        Task<IEnumerable<ApplicationSequence>> GetSequences(Guid applicationId);
        Task<ApplicationSequence> GetSequence(Guid applicationId, Guid sequenceId);
        Task<ApplicationSequence> GetSequenceBySequenceNo(Guid applicationId, int sequenceNo);

        Task<IEnumerable<ApplicationSection>> GetSections(Guid applicationId);
        Task<IEnumerable<ApplicationSection>> GetSections(Guid applicationId, Guid sequenceId);
        Task<ApplicationSection> GetSection(Guid applicationId, Guid sectionId);
        Task<ApplicationSection> GetSectionBySectionNo(Guid applicationId, int sequenceNo, int sectionNo);
        Task<Page> GetPage(Guid applicationId, Guid sectionId, string pageId);
        Task<Page> GetPageBySectionNo(Guid applicationId, int sequenceNo, int sectionNo, string pageId);
        Task<Answer> GetAnswer(Guid applicationId, int sequenceNo, int sectionNo, string pageId, string questionId);
        Answer GetAnswer(Page pageContainingQuestion, string questionId);
        Task<SetPageAnswersResponse> UpdatePageAnswers(Guid applicationId, Guid sectionId, string pageId, List<Answer> answers);
        Task<SetPageAnswersResponse> UpdatePageAnswers(Guid applicationId, int sequenceNo, int sectionNo, string pageId, List<Answer> answers);
        Task<Answer> GetAnswerByTag(Guid applicationId, string questionTag, string questionId = null);
        Task<UploadPageAnswersResult> Upload(Guid applicationId, Guid sectionId, string pageId, IFormFileCollection files);

        Task<bool> CanUpdatePage(Guid applicationId, Guid sectionId, string pageId);
        Task<bool> CanUpdatePageBySectionNo(Guid applicationId, int sequenceNo, int sectionNo, string pageId);

        Task<ResetPageAnswersResponse> ResetPageAnswers(Guid applicationId, Guid sectionId, string pageId);

        Task<ResetSectionAnswersResponse> ResetPageAnswersBySection(Guid applicationId, int sequenceNo, int sectionNo);

        Task<ResetPageAnswersResponse> ResetPageAnswersBySequenceAndSectionNumber(Guid applicationId, int sequenceNo,
            int sectionNo, string pageId);
        Task<AddPageAnswerResponse> AddPageAnswerToMultipleAnswerPage(Guid applicationId, Guid sectionId, string pageId, List<Answer> answer);
        Task<Page> RemovePageAnswerFromMultipleAnswerPage(Guid applicationId, Guid sectionId, string pageId, Guid answerId);

        Task<SkipPageResponse> SkipPage(Guid applicationId, Guid sectionId, string pageId);
        Task<SkipPageResponse> SkipPageBySectionNo(Guid applicationId, int sequenceNo, int sectionNo, string pageId);
        Task<HttpResponseMessage> DownloadFile(Guid applicationId, Guid sectionId, string pageId, string questionId, string filename);
        Task DeleteFile(Guid applicationId, Guid sectionId, string pageId, string questionId, string filename);

        // APR-2877 
        Task ResetSectionPagesIncomplete(Guid applicationId, int sequenceNo, int sectionNo,  List<string> pageIdsExcluded);




    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.UpdatePageAnswers;
using SFA.DAS.ApplyService.Application.Apply.Upload;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public interface IApplicationApiClient
    {
        //Task<Page> GetPage(Guid applicationId, string pageId, Guid userId);

//        Task<UpdatePageAnswersResult> UpdatePageAnswers(Guid applicationId, Guid userId, string pageId,
//            List<Answer> answers);

        //Task<Sequence> GetSequence(Guid applicationId, string sequenceId, Guid userId);

        //Task<List<Section>> GetSections(Guid applicationId, Guid userId);

        Task<List<Domain.Entities.Application>> GetApplicationsFor(Guid userId);

        Task<UploadResult> Upload(string applicationId, string userId, int sequenceId, int sectionId, string pageId, IFormFileCollection files);
        
        Task<byte[]> Download(Guid applicationId, Guid userId, string pageId, string questionId, string filename);
        Task<ApplicationSequence> GetSequence(Guid applicationId, Guid userId);
        Task<ApplicationSection> GetSection(Guid applicationId, int sequenceId, int sectionId, Guid userId);
        
        Task<Page> GetPage(Guid applicationId, int sequenceId, int sectionId, string pageId, Guid userId);

        Task<UpdatePageAnswersResult> UpdatePageAnswers(Guid applicationId, Guid userId, int sequenceId, int sectionId,
            string pageId, List<Answer> answers);

        Task StartApplication(Guid userId);
        Task Submit(Guid applicationId, int sequenceId, Guid userId);
        Task DeleteAnswer(Guid applicationId, int sequenceId, int sectionId, string pageId, Guid answerId, Guid userId);
        Task ImportWorkflow(IFormFile file);
        Task UpdateApplicationData<T>(T applicationData, Guid applicationId);
        Task<Domain.Entities.Application> GetApplication(Guid applicationId);
        
    }
}
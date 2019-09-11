
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
        Task<Page> GetPage(Guid applicationId, Guid sectionId, string pageId);
        Task<Answer> GetAnswer(Guid applicationId, Guid sectionId, string pageId, string questionId); 
        Task<UpdatePageAnswersResult> UpdatePageAnswers(Guid applicationId, Guid sectionId, string pageId, List<Answer> answers);
        Task<IEnumerable<ApplicationSequence>> GetSequences(Guid applicationId);
        Task<IEnumerable<ApplicationSection>> GetSections(Guid applicationId, Guid sequenceId);
        Task<Answer> GetAnswerByTag(Guid applicationId, string questionTag);
    }
}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.UpdatePageAnswers;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types;

using StartApplicationResponse = SFA.DAS.ApplyService.Application.Apply.StartApplicationResponse;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    using Application.Apply.GetAnswers;
    using SFA.DAS.ApplyService.Domain.Roatp;

    public interface IApplicationApiClient
    {
        Task<StartApplicationResponse> StartApplication(StartApplicationRequest startApplicationRequest);






        Task<List<Domain.Entities.Application>> GetApplications(Guid userId, bool createdBy);
        Task<ApplicationSequence> GetSequence(Guid applicationId, Guid userId);
        Task<IEnumerable<ApplicationSequence>> GetSequences(Guid applicationId);
        Task<ApplicationSection> GetSection(Guid applicationId, int sequenceId, int sectionId, Guid userId);
        Task<IEnumerable<ApplicationSection>> GetSections(Guid applicationId, int sequenceId, Guid userId);
        Task<Domain.Apply.Page> GetPage(Guid applicationId, int sequenceId, int sectionId, string pageId, Guid userId);

        Task<SetPageAnswersResponse> UpdatePageAnswers(Guid applicationId, Guid userId, int sequenceId, int sectionId,
            string pageId, List<Answer> answers, bool saveNewAnswers);

        Task<bool> Submit(Guid applicationId, int sequenceId, Guid userId, string userEmail);
        Task DeleteAnswer(Guid applicationId, int sequenceId, int sectionId, string pageId, Guid answerId, Guid userId);
        Task ImportWorkflow(IFormFile file);
        Task UpdateApplicationData<T>(T applicationData, Guid applicationId);
        Task<Domain.Entities.Application> GetApplication(Guid applicationId);
       
        Task<string> GetApplicationStatus(Guid applicationId, int standardCode);

        Task<List<StandardCollation>> GetStandards();
        Task<List<Option>> GetQuestionDataFedOptions(string dataEndpoint);
        Task DeleteFile(Guid applicationId, Guid userId, int sequenceId, int sectionId, string pageId, string questionId);
        Task<Organisation> GetOrganisationByUserId(Guid userId);
        Task<Organisation> GetOrganisationByUkprn(string ukprn);
        Task<Organisation> GetOrganisationByName(string name);
        Task<GetAnswersResponse> GetAnswer(Guid applicationId, string questionIdentifer);
        Task<IEnumerable<RoatpApplicationStatus>> GetExistingApplicationStatus(string ukprn);
    }
}
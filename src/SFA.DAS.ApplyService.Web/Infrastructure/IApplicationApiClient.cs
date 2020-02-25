using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Start;
using SFA.DAS.ApplyService.Application.Apply.Submit;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    using SFA.DAS.ApplyService.Application.Apply.Roatp;
    using SFA.DAS.ApplyService.Domain.Roatp;

    public interface IApplicationApiClient
    {
        Task<Guid> StartApplication(StartApplicationRequest startApplicationRequest);
        Task<bool> SubmitApplication(SubmitApplicationRequest submitApplicationRequest);
        Task<bool> ChangeProviderRoute(ChangeProviderRouteRequest changeProviderRouteRequest);

        Task<Domain.Entities.Apply> GetApplication(Guid applicationId);
        Task<List<Domain.Entities.Apply>> GetApplications(Guid userId, bool createdBy);




        Task<IEnumerable<ApplicationSequence>> GetSequences(Guid applicationId);
        
        Task<SetPageAnswersResponse> UpdatePageAnswers(Guid applicationId, Guid userId, int sequenceId, int sectionId,
            string pageId, List<Answer> answers, bool saveNewAnswers);

        Task ImportWorkflow(IFormFile file);
        
       
        Task<string> GetApplicationStatus(Guid applicationId, int standardCode);

        Task<List<StandardCollation>> GetStandards();
        Task<List<Option>> GetQuestionDataFedOptions(string dataEndpoint);
        Task DeleteFile(Guid applicationId, Guid userId, int sequenceId, int sectionId, string pageId, string questionId);
        Task<Organisation> GetOrganisationByUserId(Guid userId);
        Task<Organisation> GetOrganisationByUkprn(string ukprn);
        Task<Organisation> GetOrganisationByName(string name);
        Task<IEnumerable<RoatpApplicationStatus>> GetExistingApplicationStatus(string ukprn);

        Task<bool> UpdateApplicationStatus(Guid applicationId, string applicationStatus);
    }
}
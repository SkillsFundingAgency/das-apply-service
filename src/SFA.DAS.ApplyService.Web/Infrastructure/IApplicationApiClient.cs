using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Start;
using SFA.DAS.ApplyService.Application.Apply.Submit;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types;

using StartQnaApplicationResponse = SFA.DAS.ApplyService.Application.Apply.StartQnaApplicationResponse;

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
        Task<List<Domain.Entities.Apply>> GetApplications(Guid signinId, bool createdBy);
        Task<Apply> GetApplicationByUserId(Guid applicationId, Guid signinId);

        Task<IEnumerable<RoatpSequences>> GetRoatpSequences();


        Task<ApplicationSequence> GetSequence(Guid applicationId, Guid userId);
        Task<IEnumerable<ApplicationSequence>> GetSequences(Guid applicationId);
        Task<ApplicationSection> GetSection(Guid applicationId, int sequenceId, int sectionId, Guid userId);
        Task<IEnumerable<ApplicationSection>> GetSections(Guid applicationId, int sequenceId, Guid userId);
        Task<Domain.Apply.Page> GetPage(Guid applicationId, int sequenceId, int sectionId, string pageId, Guid userId);

        Task<SetPageAnswersResponse> UpdatePageAnswers(Guid applicationId, Guid userId, int sequenceId, int sectionId,
            string pageId, List<Answer> answers, bool saveNewAnswers);

        Task DeleteAnswer(Guid applicationId, int sequenceId, int sectionId, string pageId, Guid answerId, Guid userId);
        Task ImportWorkflow(IFormFile file);
        
       
        Task<List<Option>> GetQuestionDataFedOptions(string dataEndpoint);
        Task DeleteFile(Guid applicationId, Guid userId, int sequenceId, int sectionId, string pageId, string questionId);
        Task<Organisation> GetOrganisationByUserId(Guid userId);
        Task<Organisation> GetOrganisationByUkprn(string ukprn);
        Task<Organisation> GetOrganisationByName(string name);
        Task<IEnumerable<RoatpApplicationStatus>> GetExistingApplicationStatus(string ukprn);

        Task<bool> UpdateApplicationStatus(Guid applicationId, string applicationStatus);
    }
}
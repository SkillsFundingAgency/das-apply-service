using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Application.Apply.Start;
using SFA.DAS.ApplyService.Application.Apply.Submit;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.InternalApi.Types;
using SFA.DAS.ApplyService.Infrastructure.ApiClients;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class ApplicationApiClient : ApiClientBase<ApplicationApiClient>, IApplicationApiClient
    {
        public ApplicationApiClient(ILogger<ApplicationApiClient> logger, IConfigurationService configurationService, ITokenService tokenService) : base(logger)
        {
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(configurationService.GetConfig().Result.InternalApi.Uri);
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.GetToken());
        }

        public async Task<Guid> StartApplication(StartApplicationRequest startApplicationRequest)
        {
            return await Post<StartApplicationRequest, Guid>($"/Application/Start", startApplicationRequest);
        }

        public async Task<bool> SubmitApplication(SubmitApplicationRequest submitApplicationRequest)
        {
            return await Post<SubmitApplicationRequest, bool>($"/Application/Submit", submitApplicationRequest);
        }

        public async Task<bool> ChangeProviderRoute(ChangeProviderRouteRequest changeProviderRouteRequest)
        {
            return await Post<ChangeProviderRouteRequest, bool>($"/Application/ChangeProviderRoute", changeProviderRouteRequest);
        }

        public async Task<Domain.Entities.Apply> GetApplication(Guid applicationId)
        {
            return await Get<Domain.Entities.Apply>($"Application/{applicationId}");
        }

        public async Task<List<Domain.Entities.Apply>> GetApplications(Guid userId, bool createdBy)
        {
            if (!createdBy)
            {
                return await Get<List<Domain.Entities.Apply>>($"Applications/{userId}/Organisation");
            }

            return await Get<List<Domain.Entities.Apply>>($"Applications/{userId}");
        }

        public async Task<IEnumerable<RoatpSequences>> GetRoatpSequences()
        {
            return await Get<List<RoatpSequences>>($"roatp-sequences");
        }


        // NOTE: This is old stuff or things which are not migrated over yet       
        public async Task<ApplicationSequence> GetSequence(Guid applicationId, Guid userId)
        {
            return await Get<ApplicationSequence>($"Application/{applicationId}/User/{userId}/Sections");
        }

        public async Task<IEnumerable<ApplicationSequence>> GetSequences(Guid applicationId)
        {
            return await Get<List<ApplicationSequence>>($"Application/{applicationId}/Sequences");
        }

        public async Task<ApplicationSection> GetSection(Guid applicationId, int sequenceId, int sectionId, Guid userId)
        {
            return await Get<ApplicationSection>($"Application/{applicationId}/User/{userId}/Sequences/{sequenceId}/Sections/{sectionId}");
        }

        public async Task<IEnumerable<ApplicationSection>> GetSections(Guid applicationId, int sequenceId, Guid userId)
        {
            return await Get<List<ApplicationSection>>($"Application/{applicationId}/User/{userId}/Sequences/{sequenceId}/Sections");
        }

        public async Task<Page> GetPage(Guid applicationId, int sequenceId, int sectionId, string pageId, Guid userId)
        {
            return await Get<Page>($"Application/{applicationId}/User/{userId}/Sequence/{sequenceId}/Sections/{sectionId}/Pages/{pageId}");
        }

        public async Task<SetPageAnswersResponse> UpdatePageAnswers(Guid applicationId, Guid userId, int sequenceId,
            int sectionId, string pageId, List<Answer> answers, bool saveNewAnswers)
        {
            return await Post<dynamic, SetPageAnswersResponse>(
                    $"Application/{applicationId}/User/{userId}/Sequence/{sequenceId}/Sections/{sectionId}/Pages/{pageId}",
                    new { answers, saveNewAnswers });
        }

        public async Task DeleteAnswer(Guid applicationId, int sequenceId, int sectionId, string pageId, Guid answerId,
            Guid userId)
        {
            await Post(
                    $"Application/{applicationId}/User/{userId}/Sequence/{sequenceId}/Sections/{sectionId}/Pages/{pageId}/DeleteAnswer/{answerId}",
                    new { });
        }

        public async Task ImportWorkflow(IFormFile file)
        {
            var formDataContent = new MultipartFormDataContent();

            var fileContent = new StreamContent(file.OpenReadStream())
                {Headers = {ContentLength = file.Length, ContentType = new MediaTypeHeaderValue(file.ContentType)}};
            formDataContent.Add(fileContent, file.Name, file.FileName);

            _logger.LogInformation($"API ImportWorkflow > Added content {file.FileName}");

            await _httpClient.PostAsync($"/Import/Workflow", formDataContent);

            _logger.LogInformation($"API ImportWorkflow > After post to Internal API");
        }

        public async Task<string> GetApplicationStatus(Guid applicationId, int standardCode)
        {
            return await Get($"Application/{applicationId}/standard/{standardCode}/check-status");
        }

        public async Task<List<StandardCollation>> GetStandards()
        {
            return await Get<List<StandardCollation>>($"all-standards");
        }

        public async Task<List<Option>> GetQuestionDataFedOptions(string dataEndpoint)
        {
            return await Get<List<Option>>($"QuestionOptions/{dataEndpoint}");
        }

        public async Task DeleteFile(Guid applicationId, Guid userId, int sequenceId, int sectionId, string pageId, string questionId)
        {
            await Post($"/DeleteFile", new { applicationId, userId, sequenceId, sectionId, pageId, questionId });
        }

        public async Task<Organisation> GetOrganisationByUserId(Guid userId)
        {
            return await Get<Organisation>($"organisations/userId/{userId}");
        }

        public async Task<Organisation> GetOrganisationByUkprn(string ukprn)
        {
            return await Get<Organisation>($"organisations/ukprn/{ukprn}");
        }

        public async Task<Organisation> GetOrganisationByName(string name)
        {
            return await Get<Organisation>($"organisations/name/{WebUtility.UrlEncode(name)}");
        }

        public async Task<IEnumerable<RoatpApplicationStatus>> GetExistingApplicationStatus(string ukprn)
        {
            return await Get<List<RoatpApplicationStatus>>($"/Applications/Existing/{ukprn}");
        }

        public async Task<bool> UpdateApplicationStatus(Guid applicationId, string applicationStatus)
        {
            return await Post<dynamic, bool>($"/Application/Status", new { applicationId, applicationStatus });          
        }
    }
}
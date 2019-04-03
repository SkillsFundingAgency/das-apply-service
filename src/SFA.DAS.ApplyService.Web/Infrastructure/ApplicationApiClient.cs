using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Download;
using SFA.DAS.ApplyService.Application.Apply.UpdatePageAnswers;
using SFA.DAS.ApplyService.Application.Apply.Upload;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class ApplicationApiClient : IApplicationApiClient
    {
        private readonly ILogger<ApplicationApiClient> _logger;
        private static readonly HttpClient _httpClient = new HttpClient();

        public ApplicationApiClient(IConfigurationService configurationService, ILogger<ApplicationApiClient> logger)
        {
            _logger = logger;
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(configurationService.GetConfig().Result.InternalApi.Uri);
            }
        }

        public async Task<List<Domain.Entities.Application>> GetApplicationsFor(Guid userId)
        {
            return await (await _httpClient.GetAsync($"/Applications/{userId}")).Content
                .ReadAsAsync<List<Domain.Entities.Application>>();
        }

        public async Task<UploadResult> Upload(Guid applicationId, string userId, int sequenceId, int sectionId,
            string pageId, IFormFileCollection files)
        {
            var formDataContent = new MultipartFormDataContent();
            foreach (var file in files)
            {
                var fileContent = new StreamContent(file.OpenReadStream())
                    {Headers = {ContentLength = file.Length, ContentType = new MediaTypeHeaderValue(file.ContentType)}};
                formDataContent.Add(fileContent, file.Name, file.FileName);
            }

            return await (await _httpClient.PostAsync(
                    $"/Upload/Application/{applicationId}/User/{userId}/Sequence/{sequenceId}/Section/{sectionId}/Page/{pageId}",
                    formDataContent)).Content
                .ReadAsAsync<UploadResult>();
        }
        
        public async Task<HttpResponseMessage> Download(Guid applicationId, Guid userId, int sequenceId, int sectionId, string pageId, string questionId,string filename)
        {
            var downloadResponse = await _httpClient.GetAsync(
                $"/Download/Application/{applicationId}/User/{userId}/Sequence/{sequenceId}/Section/{sectionId}/Page/{pageId}/Question/{questionId}/{filename}");
            return downloadResponse;
        }

        public async Task<FileInfoResponse> FileInfo(Guid applicationId, Guid userId, int sequenceId, int sectionId, string pageId, string questionId,string filename)
        {
            var downloadResponse = await (await _httpClient.GetAsync(
                $"/FileInfo/Application/{applicationId}/User/{userId}/Sequence/{sequenceId}/Section/{sectionId}/Page/{pageId}/Question/{questionId}/{filename}")).Content.ReadAsAsync<FileInfoResponse>();
            return downloadResponse;
        }
        
        public async Task<ApplicationSequence> GetSequence(Guid applicationId, Guid userId)
        {
            return await (await _httpClient.GetAsync($"Application/{applicationId}/User/{userId}/Sections")).Content
                .ReadAsAsync<ApplicationSequence>();
        }

        public async Task<ApplicationSection> GetSection(Guid applicationId, int sequenceId, int sectionId, Guid userId)
        {
            return await (await _httpClient.GetAsync(
                    $"Application/{applicationId}/User/{userId}/Sequences/{sequenceId}/Sections/{sectionId}")).Content
                .ReadAsAsync<ApplicationSection>();
        }

        public async Task<Page> GetPage(Guid applicationId, int sequenceId, int sectionId, string pageId, Guid userId)
        {
            return await (await _httpClient.GetAsync(
                    $"Application/{applicationId}/User/{userId}/Sequence/{sequenceId}/Sections/{sectionId}/Pages/{pageId}")
                )
                .Content.ReadAsAsync<Page>();
        }

        public async Task<UpdatePageAnswersResult> UpdatePageAnswers(Guid applicationId, Guid userId, int sequenceId,
            int sectionId, string pageId, List<Answer> answers)
        {
            return await (await _httpClient.PostAsJsonAsync(
                    $"Application/{applicationId}/User/{userId}/Sequence/{sequenceId}/Sections/{sectionId}/Pages/{pageId}",
                    answers)).Content
                .ReadAsAsync<UpdatePageAnswersResult>();
        }

        public async Task<StartApplicationResponse> StartApplication(Guid userId)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync("/Application/Start", new {userId});
            var startApplicationResponse = await httpResponse.Content.ReadAsAsync<StartApplicationResponse>();
            return startApplicationResponse;
        }

        public async Task Submit(Guid applicationId, int sequenceId, Guid userId, string userEmail)
        {
            await _httpClient.PostAsJsonAsync("/Applications/Submit", new {applicationId, sequenceId, userId, userEmail });
        }

        public async Task DeleteAnswer(Guid applicationId, int sequenceId, int sectionId, string pageId, Guid answerId,
            Guid userId)
        {
            await _httpClient.PostAsJsonAsync(
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

        public async Task UpdateApplicationData<T>(T applicationData, Guid applicationId)
        {
            await _httpClient.PostAsJsonAsync($"/Application/{applicationId}/UpdateApplicationData", applicationData);
        }

        public async Task<Domain.Entities.Application> GetApplication(Guid applicationId)
        {
            return await (await _httpClient.GetAsync($"Application/{applicationId}")).Content
                .ReadAsAsync<Domain.Entities.Application>();
        }

        public async Task<string> GetApplicationStatus(Guid applicationId, int standardCode)
        {
            return await(await _httpClient.GetAsync(
                $"Application/{applicationId}/standard/{standardCode}/check-status")).Content.ReadAsStringAsync();
        }

        public async Task<List<StandardCollation>> GetStandards()
        {
            return await(await _httpClient.GetAsync("all-standards")).Content.ReadAsAsync<List<StandardCollation>>();
        }

        public async Task<List<Option>> GetQuestionDataFedOptions(string dataEndpoint)
        {
            return await(await _httpClient.GetAsync($"QuestionOptions/{dataEndpoint}")).Content.ReadAsAsync<List<Option>>();
        }

        public async Task DeleteFile(Guid applicationId, Guid userId, int sequenceId, int sectionId, string pageId, string questionId)
        {
            await _httpClient.PostAsJsonAsync($"/DeleteFile", new {applicationId, userId, sequenceId, sectionId, pageId, questionId});
        }

        public async Task<Organisation> GetOrganisationByUserId(Guid userId)
        {
            return await(await _httpClient.GetAsync($"organisations/userId/{userId}")).Content.ReadAsAsync<Organisation>();
        }

        public async Task<Organisation> GetOrganisationByUkprn(string ukprn)
        {
            return await (await _httpClient.GetAsync($"organisations/ukprn/{ukprn}")).Content.ReadAsAsync<Organisation>();
        }
        public async Task<Organisation> GetOrganisationByName(string name)
        {
            return await (await _httpClient.GetAsync($"organisations/name/{WebUtility.UrlEncode(name)}")).Content.ReadAsAsync<Organisation>();
        }
    }
}
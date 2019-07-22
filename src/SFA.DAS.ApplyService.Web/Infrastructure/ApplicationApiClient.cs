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
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class ApplicationApiClient : ApiClient, IApplicationApiClient
    {
        private readonly ILogger<ApplicationApiClient> _logger;

        public ApplicationApiClient(ILogger<ApplicationApiClient> logger, IConfigurationService configurationService) : base (logger, configurationService)
        {
            _logger = logger;
        }

        public async Task<List<Domain.Entities.Application>> GetApplications(Guid userId, bool createdBy)
        {
            if (!createdBy)
            {
                return await Get<List<Domain.Entities.Application>>($"/Applications/{userId}/Organisation");
            }
            else
            {
                return await Get<List<Domain.Entities.Application>>($"/Applications/{userId}");
            }
        }

        public async Task<UploadResult> Upload(Guid applicationId, string userId, int sequenceId, int sectionId, string pageId, IFormFileCollection files)
        {
            var formDataContent = new MultipartFormDataContent();
            foreach (var file in files)
            {
                var fileContent = new StreamContent(file.OpenReadStream())
                    { Headers = { ContentLength = file.Length, ContentType = new MediaTypeHeaderValue(file.ContentType) } };
                formDataContent.Add(fileContent, file.Name, file.FileName);

                _logger.LogInformation($"API Upload > Added content {file.FileName}");
            }

            return await PostFileContent<UploadResult>($"/Upload/Application/{applicationId}/User/{userId}/Sequence/{sequenceId}/Section/{sectionId}/Page/{pageId}", formDataContent);
        }

        public async Task<HttpResponseMessage> Download(Guid applicationId, Guid userId, int sequenceId, int sectionId, string pageId, string questionId, string filename)
        {
            return await Get($"/Download/Application/{applicationId}/User/{userId}/Sequence/{sequenceId}/Section/{sectionId}/Page/{pageId}/Question/{questionId}/{filename}");
        }

        public async Task<FileInfoResponse> FileInfo(Guid applicationId, Guid userId, int sequenceId, int sectionId, string pageId, string questionId, string filename)
        {
            return await Get<FileInfoResponse>($"/FileInfo/Application/{applicationId}/User/{userId}/Sequence/{sequenceId}/Section/{sectionId}/Page/{pageId}/Question/{questionId}/{filename}");
        }

        public async Task<ApplicationSequence> GetSequence(Guid applicationId, Guid userId)
        {
            return await Get<ApplicationSequence>($"Application/{applicationId}/User/{userId}/Sections");
        }

        public async Task<ApplicationSection> GetSection(Guid applicationId, int sequenceId, int sectionId, Guid userId)
        {
            return await Get<ApplicationSection>($"Application/{applicationId}/User/{userId}/Sequences/{sequenceId}/Sections/{sectionId}");
        }

        public async Task<Page> GetPage(Guid applicationId, int sequenceId, int sectionId, string pageId, Guid userId)
        {
            return await Get<Page>($"Application/{applicationId}/User/{userId}/Sequence/{sequenceId}/Sections/{sectionId}/Pages/{pageId}");
        }

        public async Task<UpdatePageAnswersResult> UpdatePageAnswers(Guid applicationId, Guid userId, int sequenceId, int sectionId, string pageId, List<Answer> answers, bool saveNewAnswers)
        {
            return await Post<UpdatePageAnswersResult>($"Application/{applicationId}/User/{userId}/Sequence/{sequenceId}/Sections/{sectionId}/Pages/{pageId}", new { answers, saveNewAnswers });
        }

        public async Task<StartApplicationResponse> StartApplication(Guid userId)
        {
            return await Post<StartApplicationResponse>($"/Application/Start", new { userId });
        }

        public async Task<bool> Submit(Guid applicationId, int sequenceId, Guid userId, string userEmail)
        {
            return await Post<bool>($"/Application/Submit", new { applicationId, sequenceId, userId, userEmail });
        }

        public async Task DeleteAnswer(Guid applicationId, int sequenceId, int sectionId, string pageId, Guid answerId, Guid userId)
        {
            await Post($"Application/{applicationId}/User/{userId}/Sequence/{sequenceId}/Sections/{sectionId}/Pages/{pageId}/DeleteAnswer/{answerId}", new { });
        }

        public async Task ImportWorkflow(IFormFile file)
        {
            var formDataContent = new MultipartFormDataContent();

            var fileContent = new StreamContent(file.OpenReadStream())
                { Headers = { ContentLength = file.Length, ContentType = new MediaTypeHeaderValue(file.ContentType) } };
            formDataContent.Add(fileContent, file.Name, file.FileName);

            _logger.LogInformation($"API ImportWorkflow > Added content {file.FileName}");

            await PostFileContent($"/Import/Workflow", formDataContent);

            _logger.LogInformation($"API ImportWorkflow > After post to Internal API");
        }

        public async Task UpdateApplicationData<T>(Guid applicationId, T applicationData)
        {
            await Post($"/Application/{applicationId}/UpdateApplicationData", applicationData);
        }

        public async Task<Domain.Entities.Application> GetApplication(Guid applicationId)
        {
            return await Get<Domain.Entities.Application>($"Application/{applicationId}");
        }

        public async Task<string> GetApplicationStatus(Guid applicationId, int standardCode)
        {
            return await Get<string>($"Application/{applicationId}/standard/{standardCode}/check-status");
        }

        public async Task<List<StandardCollation>> GetStandards()
        {
            return await Get<List<StandardCollation>>("all-standards");
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
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.UpdatePageAnswers;
using SFA.DAS.ApplyService.Application.Apply.Upload;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class ApplicationApiClient : IApplicationApiClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public ApplicationApiClient(IConfigurationService configurationService)
        {
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(configurationService.GetConfig().Result.InternalApi.Uri);
            }
        }
        
        public async Task<Page> GetPage(Guid applicationId, string pageId, Guid userId)
        {
            return await (await _httpClient.GetAsync($"/Application/{applicationId}/User/{userId}/Pages/{pageId}")).Content
                .ReadAsAsync<Page>();
        }

        public async Task<UpdatePageAnswersResult> UpdatePageAnswers(Guid applicationId, Guid userId, string pageId, List<Answer> answers)
        {
            return await (await _httpClient.PostAsJsonAsync(
                    $"/Application/{applicationId}/User/{userId}/Pages/{pageId}", answers)).Content
                .ReadAsAsync<UpdatePageAnswersResult>();
        }

        public async Task<Sequence> GetSequence(Guid applicationId, string sequenceId, Guid userId)
        {
            return await (await _httpClient.GetAsync($"/Application/{applicationId}/User/{userId}/Sequences/{sequenceId}")).Content
                .ReadAsAsync<Sequence>();
        }

        public async Task<List<Sequence>> GetSequences(Guid applicationId, Guid userId)
        {
            return await (await _httpClient.GetAsync($"/Application/{applicationId}/User/{userId}/Sequences")).Content
                .ReadAsAsync<List<Sequence>>();
        }

        public async Task<List<Entity>> GetApplicationsFor(Guid userId)
        {
            return await (await _httpClient.GetAsync($"/Applications/{userId}")).Content.ReadAsAsync<List<Entity>>();
        }

        public async Task<UploadResult> Upload(string applicationId, string userId, string pageId, IFormFileCollection files)
        {
            var formDataContent = new MultipartFormDataContent();
            foreach (var file in files)
            {
                var fileContent = new StreamContent(file.OpenReadStream())
                    {Headers = {ContentLength = file.Length, ContentType = new MediaTypeHeaderValue(file.ContentType)}};
                formDataContent.Add(fileContent, file.Name, file.FileName);
            }

            return await (await _httpClient.PostAsync(
                    $"/Application/{applicationId}/User/{userId}/Page/{pageId}/Upload", formDataContent)).Content
                .ReadAsAsync<UploadResult>();
        }

        public async Task<byte[]> Download(Guid applicationId, Guid userId, string pageId, string questionId, string filename)
        {
            var stream = await _httpClient.GetByteArrayAsync(
                $"/Application/{applicationId}/User/{userId}/Page/{pageId}/Question/{questionId}/{filename}/Download");
            return stream;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class ApplicationApiClient 
    {
        private readonly HttpClient _httpClient;

        public ApplicationApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
    }
}
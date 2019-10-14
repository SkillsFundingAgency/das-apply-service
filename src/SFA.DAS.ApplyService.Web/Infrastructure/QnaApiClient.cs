using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    using Newtonsoft.Json.Linq;
    using SFA.DAS.ApplyService.Application.Apply.UpdatePageAnswers;

    public class QnaApiClient : IQnaApiClient
    {
        private readonly ILogger<QnaApiClient> _logger;
        private readonly IQnaTokenService _tokenService;
        private static readonly HttpClient _httpClient = new HttpClient();

        public QnaApiClient(IConfigurationService configurationService, ILogger<QnaApiClient> logger, IQnaTokenService tokenService)
        {
            _logger = logger;
            _tokenService = tokenService;
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(configurationService.GetConfig().Result.QnaApiAuthentication.ApiBaseAddress);
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenService.GetToken());
        }

        public async Task<Answer> GetAnswer(Guid applicationId, Guid sectionId, string pageId, string questionId)
        {
            var pageContainingQuestion = await GetPage(applicationId, sectionId, pageId);

            foreach (var question in pageContainingQuestion.Questions)
            {
                if (question.QuestionId == questionId)
                {
                    if (pageContainingQuestion.PageOfAnswers != null && pageContainingQuestion.PageOfAnswers.Count > 0)
                    {
                        foreach (var pageOfAnswers in pageContainingQuestion.PageOfAnswers)
                        {
                            var pageAnswer =
                                pageOfAnswers.Answers.FirstOrDefault(x => x.QuestionId == questionId);
                            if (pageAnswer != null)
                            {
                                return await Task.FromResult(pageAnswer);
                            }
                        }
                    }
                }
            }

            return await Task.FromResult(new Answer { QuestionId = questionId, Value = string.Empty });
        }

        public async Task<Answer> GetAnswerByTag(Guid applicationId, string questionTag)
        {
            var answer = new Answer();
            var applicationDataJson = await (await _httpClient.GetAsync(
                    $"Applications/{applicationId}/applicationData")
                )
                .Content.ReadAsAsync<object>();
            var applicationData = JObject.Parse(applicationDataJson.ToString());
            if (applicationData != null)
            {
                var answerData = applicationData[questionTag];
                if (answerData != null)
                {
                    answer.Value = answerData.Value<string>();
                }
            }

            return await Task.FromResult(answer);
        }

        public async Task<Page> GetPage(Guid applicationId, Guid sectionId, string pageId)
        {
            return await(await _httpClient.GetAsync(
                    $"Applications/{applicationId}/sections/{sectionId}/pages/{pageId}")
                )
                .Content.ReadAsAsync<Page>();
        }

        public async Task<ApplicationSection> GetSection(Guid applicationId, Guid sectionId)
        {
            return await (await _httpClient.GetAsync(
                    $"Applications/{applicationId}/sections/{sectionId}")
                )
                .Content.ReadAsAsync<ApplicationSection>();
        }

        public async Task<IEnumerable<ApplicationSection>> GetSections(Guid applicationId, Guid sequenceId)
        {
            return await(await _httpClient.GetAsync(
                    $"Applications/{applicationId}/Sequences/{sequenceId}/sections")
                )
                .Content.ReadAsAsync<IEnumerable<ApplicationSection>>();
        }

        public async Task<ApplicationSequence> GetSequence(Guid applicationId, Guid sequenceId)
        {
            return await(await _httpClient.GetAsync(
                    $"Applications/{applicationId}/Sequences/{sequenceId}")
                )
                .Content.ReadAsAsync<ApplicationSequence>();
        }

        public async Task<IEnumerable<ApplicationSequence>> GetSequences(Guid applicationId)
        {
            return await(await _httpClient.GetAsync(
                    $"Applications/{applicationId}/Sequences")
                )
                .Content.ReadAsAsync<IEnumerable<ApplicationSequence>>();
        }

        public async Task<StartApplicationResponse> StartApplication(string userReference, string workflowType, string applicationData)
        {
            var startApplicationRequest = new StartQnaApplicationRequest
            {
                UserReference = userReference,
                WorkflowType = workflowType,
                ApplicationData = applicationData
            };

            return await(await _httpClient.PostAsJsonAsync(
                    "/Applications/Start",
                    startApplicationRequest)).Content
                .ReadAsAsync<StartApplicationResponse>();
        }

        public async Task<UpdatePageAnswersResult> UpdatePageAnswers(Guid applicationId, Guid sectionId, string pageId, List<Answer> answers)
        {
            return await (await _httpClient.PostAsJsonAsync(
                        $"/Applications/{applicationId}/sections/{sectionId}/pages/{pageId}",
                        answers)).Content
                    .ReadAsAsync<UpdatePageAnswersResult>();
        }

    }
}

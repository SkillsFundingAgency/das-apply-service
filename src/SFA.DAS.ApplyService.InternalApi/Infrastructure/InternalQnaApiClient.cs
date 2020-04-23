using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Models.Roatp;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    public class InternalQnaApiClient : IInternalQnaApiClient
    {
        private readonly ILogger<InternalQnaApiClient> _logger;
        private readonly IQnaTokenService _tokenService;
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly string _environmentName;

        protected readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public InternalQnaApiClient(IConfigurationService configurationService, ILogger<InternalQnaApiClient> logger, IQnaTokenService tokenService)
        {
            _logger = logger;
            _tokenService = tokenService;

            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(configurationService.GetConfig().Result.QnaApiAuthentication.ApiBaseAddress);
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenService.GetToken());

            _environmentName = configurationService.GetEnvironmentName();
        }
        public async Task<string> GetQuestionTag(Guid applicationId, string questionTag)
        {
            var response = await _httpClient.GetAsync($"Applications/{applicationId}/applicationData/{questionTag}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<string>();
            }
            else
            {
                var json = await response.Content.ReadAsStringAsync();
                var apiError = JsonConvert.DeserializeObject<ApiError>(json);
                var apiErrorMessage = apiError?.Message ?? json;

                _logger.LogError($"Error in QnaApiClient.GetQuestionTag() - applicationId {applicationId} | questionTag : {questionTag} | StatusCode : {response.StatusCode} | ErrorMessage: { apiErrorMessage }");
                return null;
            }
        }

        public async Task<Page> GetPageBySectionNo(Guid applicationId, int sequenceNo, int sectionNo, string pageId)
        {
            var response = await _httpClient.GetAsync($"Applications/{applicationId}/sequences/{sequenceNo}/sections/{sectionNo}/pages/{pageId}");
            return await response.Content.ReadAsAsync<Page>();
        }

        public async Task<string> GetAnswerValue(Guid applicationId, int sequenceNo, int sectionNo, string pageId, string questionId)
        {
            var pageContainingQuestion = await GetPageBySectionNo(applicationId, sequenceNo, sectionNo, pageId);

            return GetAnswerValue(questionId, pageContainingQuestion);
        }

        public async Task<string> GetAnswerValueFromActiveQuestion(Guid applicationId, int sequenceNo, int sectionNo, params PageAndQuestion[] possibleQuestions)
        {
            foreach (var question in possibleQuestions)
            {
                var pageContainingQuestion = await GetPageBySectionNo(applicationId, sequenceNo, sectionNo, question.PageId);

                if (!pageContainingQuestion.Active)
                {
                    continue;
                }

                return GetAnswerValue(question.QuestionId, pageContainingQuestion);
            }

            return null;
        }

        public async Task<string> GetAnswerValueFromActiveQuestion(Guid applicationId, int sequenceNo, int sectionNo, string pageId, string questionId)
        {
            var pageContainingQuestion = await GetPageBySectionNo(applicationId, sequenceNo, sectionNo, pageId);

            if (!pageContainingQuestion.Active)
            {
                return null;
            }

            return GetAnswerValue(questionId, pageContainingQuestion);
        }

        private static string GetAnswerValue(string questionId, Page pageContainingQuestion)
        {
            if (pageContainingQuestion?.Questions != null)
            {
                foreach (var question in pageContainingQuestion.Questions)
                {
                    if (question.QuestionId == questionId && pageContainingQuestion.PageOfAnswers != null)
                    {
                        foreach (var pageOfAnswers in pageContainingQuestion.PageOfAnswers)
                        {
                            var pageAnswer = pageOfAnswers.Answers.FirstOrDefault(x => x.QuestionId == questionId);
                            if (pageAnswer != null)
                            {
                                {
                                    return pageAnswer.Value;
                                }
                            }
                        }
                    }
                    else // In case question/answer is buried in FurtherQuestions
                    {
                        var furtherQuestionAnswer = GetAnswerFromFurtherQuestions(question, pageContainingQuestion, questionId);
                        if (furtherQuestionAnswer != null)
                        {
                            return furtherQuestionAnswer;
                        }
                    }
                }
            }

            return null;
        }

        private static string GetAnswerFromFurtherQuestions(Question question, Page pageContainingQuestion, string questionId)
        {
            if (question?.Input?.Options != null)
            {
                foreach (var option in question?.Input?.Options)
                {
                    foreach (var furtherQuestion in option?.FurtherQuestions ?? Enumerable.Empty<Question>())
                    {
                        if (furtherQuestion.QuestionId == questionId && pageContainingQuestion.PageOfAnswers != null)
                        {
                            foreach (var pageOfAnswers in pageContainingQuestion.PageOfAnswers)
                            {
                                var pageAnswer = pageOfAnswers.Answers.FirstOrDefault(x => x.QuestionId == questionId);
                                if (pageAnswer != null)
                                {
                                    return pageAnswer.Value;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        public async Task<FileStreamResult> GetDownloadFile(Guid applicationId, int sequenceNo, int sectionNo, string pageId, string questionId)
        {
            var response = await _httpClient.GetAsync($"Applications/{applicationId}/sequences/{sequenceNo}/sections/{sectionNo}/pages/{pageId}/questions/{questionId}/download");

            var fileStream = await response.Content.ReadAsStreamAsync();
            var result = new FileStreamResult(fileStream, response.Content.Headers.ContentType.MediaType);
            result.FileDownloadName = response.Content.Headers.ContentDisposition.FileName;
            return result;
        }

        public async Task<Answer> GetAnswerByTag(Guid applicationId, string questionTag, string questionId = null)
        {
            var answer = new Answer { QuestionId = questionId };

            var questionTagData = await GetQuestionTag(applicationId, questionTag);
            if (questionTagData != null)
            {
                answer.Value = questionTagData;
            }

            return answer;
        }

        public async Task<TabularData> GetTabularDataByTag(Guid applicationId, string questionTag)
        {
            var answer = await GetAnswerByTag(applicationId, questionTag);

            if (answer?.Value == null)
            {
                return null;
            }

            var tabularData = JsonConvert.DeserializeObject<TabularData>(answer.Value);

            return tabularData;
        }
    }
}

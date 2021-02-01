using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Infrastructure.ApiClients;
using SFA.DAS.ApplyService.Infrastructure.Firewall;
using StartQnaApplicationResponse = SFA.DAS.ApplyService.Application.Apply.StartQnaApplicationResponse;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class QnaApiClient : ApiClientBase<QnaApiClient>, IQnaApiClient
    {
        private readonly IQnaTokenService _tokenService;
        private readonly RetryPolicy<HttpResponseMessage> _retryPolicy;

        public QnaApiClient(HttpClient httpClient, ILogger<QnaApiClient> logger, IQnaTokenService tokenService) : base(httpClient, logger)
        {
            _tokenService = tokenService;
            _retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenService.GetToken());
        }

        public async Task<StartQnaApplicationResponse> StartApplication(string userReference, string workflowType, string applicationData)
        {
            var startApplicationRequest = new StartQnaApplicationRequest
            {
                UserReference = userReference,
                WorkflowType = workflowType,
                ApplicationData = applicationData
            };

            var response = await _httpClient.PostAsJsonAsync($"/Applications/Start", startApplicationRequest);

            var json = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<StartQnaApplicationResponse>(json);
            }
            else
            {
                var apiError = GetApiErrorFromJson(json);
                var apiErrorMessage = apiError?.Message ?? json;

                _logger.LogError($"Error Starting Application in QnA. UserReference : {userReference} | WorkflowType : {workflowType} | ApplicationData : {applicationData} | StatusCode : {response.StatusCode} | Response: {apiErrorMessage}");
                return new StartQnaApplicationResponse { ApplicationId = Guid.Empty };
            }
        }

        public async Task<object> GetApplicationData(Guid applicationId)
        {
            var response = await _httpClient.GetAsync($"Applications/{applicationId}/applicationData");

            return await response.Content.ReadAsAsync<object>();
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
                var apiError = GetApiErrorFromJson(json);
                var apiErrorMessage = apiError?.Message ?? json;

                _logger.LogError($"Error in QnaApiClient.GetQuestionTag() - applicationId {applicationId} | questionTag : {questionTag} | StatusCode : {response.StatusCode} | ErrorMessage: { apiErrorMessage }");
                return null; 
            }           
        }

        public async Task<IEnumerable<ApplicationSequence>> GetSequences(Guid applicationId)
        {
            var response = await _httpClient.GetAsync($"Applications/{applicationId}/Sequences");

            return await response.Content.ReadAsAsync<IEnumerable<ApplicationSequence>>();
        }

        public async Task<ApplicationSequence> GetSequence(Guid applicationId, Guid sequenceId)
        {
            var response = await _httpClient.GetAsync($"Applications/{applicationId}/Sequences/{sequenceId}");

            return await response.Content.ReadAsAsync<ApplicationSequence>();
        }

        public async Task<ApplicationSequence> GetSequenceBySequenceNo(Guid applicationId, int sequenceNo)
        {
            var response = await _httpClient.GetAsync($"Applications/{applicationId}/Sequences/{sequenceNo}");

            return await response.Content.ReadAsAsync<ApplicationSequence>();
        }


        public async Task<IEnumerable<ApplicationSection>> GetSections(Guid applicationId)
        {
            var response = await _httpClient.GetAsync($"Applications/{applicationId}/sections");

            return await response.Content.ReadAsAsync<IEnumerable<ApplicationSection>>();
        }

        public async Task<IEnumerable<ApplicationSection>> GetSections(Guid applicationId, Guid sequenceId)
        {
            var response = await _httpClient.GetAsync($"Applications/{applicationId}/Sequences/{sequenceId}/sections");

            return await response.Content.ReadAsAsync<IEnumerable<ApplicationSection>>();
        }

        public async Task<ApplicationSection> GetSection(Guid applicationId, Guid sectionId)
        {
            var response = await _httpClient.GetAsync($"Applications/{applicationId}/sections/{sectionId}");

            return await response.Content.ReadAsAsync<ApplicationSection>();
        }

        public async Task<ApplicationSection> GetSectionBySectionNo(Guid applicationId, int sequenceNo, int sectionNo)
        {
            var response = await _httpClient.GetAsync($"Applications/{applicationId}/sequences/{sequenceNo}/sections/{sectionNo}");

            return await response.Content.ReadAsAsync<ApplicationSection>();
        }


        public async Task<Page> GetPage(Guid applicationId, Guid sectionId, string pageId)
        {
            var response = await _httpClient.GetAsync($"Applications/{applicationId}/sections/{sectionId}/pages/{pageId}");

            return await response.Content.ReadAsAsync<Page>();
        }

        public async Task<Page> GetPageBySectionNo(Guid applicationId, int sequenceNo, int sectionNo, string pageId)
        {
            var response = await _httpClient.GetAsync($"Applications/{applicationId}/sequences/{sequenceNo}/sections/{sectionNo}/pages/{pageId}");

            return await response.Content.ReadAsAsync<Page>();
        }


        public async Task<Answer> GetAnswer(Guid applicationId, int sequenceNo, int sectionNo, string pageId, string questionId)
        {
            var pageContainingQuestion = await GetPageBySectionNo(applicationId, sequenceNo, sectionNo, pageId);

            return GetAnswer(pageContainingQuestion, questionId);
        }

        public Answer GetAnswer(Page pageContainingQuestion, string questionId)
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
                                return pageAnswer;
                            }
                        }
                    }
                }
            }

            return new Answer { QuestionId = questionId };
        }

        public async Task<Answer> GetAnswerByTag(Guid applicationId, string questionTag, string questionId = null)
        {
            var answer = new Answer { QuestionId = questionId };
        
            var questionTagData = await GetQuestionTag(applicationId, questionTag);
            if(questionTagData != null)
            {
                answer.Value = questionTagData.ToString();
            }

            return answer;
        }
        
        public async Task<SetPageAnswersResponse> UpdatePageAnswers(Guid applicationId, Guid sectionId, string pageId, List<Answer> answers)
        {
            // NOTE: This should be called SetPageAnswers, but leaving alone for now
            var response = await _httpClient.PostAsJsonAsync($"/Applications/{applicationId}/sections/{sectionId}/pages/{pageId}", answers);

            var json = await response.Content.ReadAsStringAsync();

            return HandleUpdatePageAnswersResponse(applicationId, pageId, response, json);
        }

        public async Task<SetPageAnswersResponse> UpdatePageAnswers(Guid applicationId, int sequenceNo, int sectionNo, string pageId, List<Answer> answers)
        {
            // NOTE: This should be called SetPageAnswers, but leaving alone for now
            var response = await _httpClient.PostAsJsonAsync($"/Applications/{applicationId}/sequences/{sequenceNo}/sections/{sectionNo}/pages/{pageId}", answers);

            var json = await response.Content.ReadAsStringAsync();

            return HandleUpdatePageAnswersResponse(applicationId, pageId, response, json);
        }

        private SetPageAnswersResponse HandleUpdatePageAnswersResponse(Guid applicationId, string pageId, HttpResponseMessage response, string json)
        {
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<SetPageAnswersResponse>(json);
            }
            else
            {
                var apiError = GetApiErrorFromJson(json);
                var apiErrorMessage = apiError?.Message ?? json;

                _logger.LogError(
                    $"Error Updating Page Answers into QnA. Application: {applicationId} | PageId: {pageId} | StatusCode : {response.StatusCode} | Response: {apiErrorMessage}");

                var validationErrorMessage = "Cannot save answers at this time. Please contact your system administrator.";

                var validationError = new KeyValuePair<string, string>(string.Empty, validationErrorMessage);
                return new SetPageAnswersResponse
                    {ValidationPassed = false, ValidationErrors = new List<KeyValuePair<string, string>> {validationError}};
            }
        }

        public async Task<bool> CanUpdatePage(Guid applicationId, Guid sectionId, string pageId)
        {
            var response = await _httpClient.GetAsync($"Applications/{applicationId}/sections/{sectionId}/pages/{pageId}/canupdate");

            return await response.Content.ReadAsAsync<bool>();
        }

        public async Task<bool> CanUpdatePageBySectionNo(Guid applicationId, int sequenceNo, int sectionNo, string pageId)
        {
            var response = await _httpClient.GetAsync($"Applications/{applicationId}/sequences/{sequenceNo}/sections/{sectionNo}/pages/{pageId}/canupdate");

            return await response.Content.ReadAsAsync<bool>();
        }

        public async Task<ResetPageAnswersResponse> ResetPageAnswers(Guid applicationId, Guid sectionId, string pageId)
        {

            var response = await _httpClient.PostAsJsonAsync($"/Applications/{applicationId}/sections/{sectionId}/pages/{pageId}/reset", new{});

            var json = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ResetPageAnswersResponse>(json);
            }
            else
            {
                var apiError = GetApiErrorFromJson(json);
                var apiErrorMessage = apiError?.Message ?? json;
                var errorMessage =
                    $"Error Resetting Page Answers into QnA. Application: {applicationId} | SectionId: {sectionId} | PageId: {pageId} | StatusCode : {response.StatusCode} | Response: {apiErrorMessage}";

                _logger.LogError(errorMessage);

                return new ResetPageAnswersResponse { ValidationPassed = false, ValidationErrors = null  };
            }
        }

        public async Task<ResetPageAnswersResponse> ResetPageAnswersBySequenceAndSectionNumber(Guid applicationId, int sequenceNo,
            int sectionNo, string pageId)
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"/Applications/{applicationId}/sequences/{sequenceNo}/sections/{sectionNo}/pages/{pageId}/reset",
                new { });

            var json = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ResetPageAnswersResponse>(json);
            }
            else
            {
                var apiError = GetApiErrorFromJson(json);
                var apiErrorMessage = apiError?.Message ?? json;
                var errorMessage =
                    $"Error Resetting Page Answers into QnA. Application: {applicationId} | SequenceNo: {sequenceNo}| SectionNo: {sectionNo} | PageId: {pageId} | StatusCode : {response.StatusCode} | Response: {apiErrorMessage}";

                _logger.LogError(errorMessage);

                return new ResetPageAnswersResponse {ValidationPassed = false, ValidationErrors = null};
            }
        }

        public Task<AddPageAnswerResponse> AddPageAnswerToMultipleAnswerPage(Guid applicationId, Guid sectionId, string pageId, List<Answer> answer)
        {
            // Not used. May need in future. See how EPAO Assessor Service does it
            throw new NotImplementedException();
        }

        public Task<Page> RemovePageAnswerFromMultipleAnswerPage(Guid applicationId, Guid sectionId, string pageId, Guid answerId)
        {
            // Not used. May need in future. See how EPAO Assessor Service does it
            throw new NotImplementedException();
        }


        public async Task<SkipPageResponse> SkipPage(Guid applicationId, Guid sectionId, string pageId)
        {
            var response = await _httpClient.PostAsJsonAsync($"Applications/{applicationId}/sections/{sectionId}/pages/{pageId}/skip", new object());

            return await response.Content.ReadAsAsync<SkipPageResponse>();
        }

        public async Task<SkipPageResponse> SkipPageBySectionNo(Guid applicationId, int sequenceNo, int sectionNo, string pageId)
        {
            var response = await _httpClient.PostAsJsonAsync($"Applications/{applicationId}/sequences/{sequenceNo}/sections/{sectionNo}/pages/{pageId}/skip", new object());

            return await response.Content.ReadAsAsync<SkipPageResponse>();
        }


        public async Task<UploadPageAnswersResult> Upload(Guid applicationId, Guid sectionId, string pageId, IFormFileCollection files)
        {
            // NOTE: This should be called UploadFiles, but leaving alone for now
            var formDataContent = new MultipartFormDataContent();

            if (files is null || files.Count < 1)
            {
                // we need to add an empty file so that the header lengths match
                var emptyStream = new MemoryStream();
                var fileContent = new StreamContent(emptyStream)
                { Headers = { ContentLength = emptyStream.Length, ContentType = new MediaTypeHeaderValue("application/pdf") } };
                formDataContent.Add(fileContent, "empty.pdf");
            }
            else
            {
                foreach (var file in files)
                {
                    var fileContent = new StreamContent(file.OpenReadStream())
                    { Headers = { ContentLength = file.Length, ContentType = new MediaTypeHeaderValue(file.ContentType) } };
                    formDataContent.Add(fileContent, file.Name, file.FileName);
                }
            }

            var response = await _httpClient.PostAsync($"/applications/{applicationId}/sections/{sectionId}/pages/{pageId}/upload", formDataContent);

            var json = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<UploadPageAnswersResult>(json);
            }
            else
            {
                var apiError = GetApiErrorFromJson(json);
                var apiErrorMessage = apiError?.Message ?? json;

                _logger.LogError($"Error Uploading files into QnA. Application: {applicationId} | SectionId: {sectionId} | PageId: {pageId} | StatusCode : {response.StatusCode} | Response: {apiErrorMessage}");

                var validationErrorMessage = "Cannot upload files at this time. Please contact your system administrator.";

                var validationError = new KeyValuePair<string, string>(string.Empty, validationErrorMessage);
                return new UploadPageAnswersResult { ValidationPassed = false, ValidationErrors = new List<KeyValuePair<string, string>> { validationError } };
            }
        }

        public async Task<HttpResponseMessage> DownloadFile(Guid applicationId, Guid sectionId, string pageId, string questionId, string filename)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, $"/applications/{applicationId}/sections/{sectionId}/pages/{pageId}/questions/{questionId}/download/{filename}"))
            {
                return await RequestToDownloadFile(request, $"Could not download file {filename}");
            }
        }

        public async Task DeleteFile(Guid applicationId, Guid sectionId, string pageId, string questionId, string filename)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Delete, $"/applications/{applicationId}/sections/{sectionId}/pages/{pageId}/questions/{questionId}/download/{filename}"))
            {
                await Delete(request);
            }
        }



        protected async Task<HttpResponseMessage> RequestToDownloadFile(HttpRequestMessage request, string message = null)
        {
            HttpRequestMessage clonedRequest = null;

            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                clonedRequest = new HttpRequestMessage(request.Method, request.RequestUri);
                clonedRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenService.GetToken());

                return await _httpClient.SendAsync(clonedRequest);

            });

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return result;
            }

            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                if (message == null)
                {
                    if (!request.RequestUri.IsAbsoluteUri)
                        message = "Could not find " + request.RequestUri;
                    else
                        message = "Could not find " + request.RequestUri.PathAndQuery;
                }

                RaiseResponseError(message, clonedRequest, result);
            }

            RaiseResponseError(clonedRequest, result);

            return result;
        }


        protected static void RaiseResponseError(string message, HttpRequestMessage failedRequest, HttpResponseMessage failedResponse)
        {
            if (failedResponse.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception(message, CreateRequestException(failedRequest, failedResponse));
            }

            throw CreateRequestException(failedRequest, failedResponse);
        }

        protected static void RaiseResponseError(HttpRequestMessage failedRequest, HttpResponseMessage failedResponse)
        {
            throw CreateRequestException(failedRequest, failedResponse);
        }

        private static HttpRequestException CreateRequestException(HttpRequestMessage failedRequest, HttpResponseMessage failedResponse)
        {
            return new HttpRequestException(
                string.Format($"The Client request for {{0}} {{1}} failed. Response Status: {{2}}, Response Body: {{3}}",
                    failedRequest.Method.ToString().ToUpperInvariant(),
                    failedRequest.RequestUri,
                    (int)failedResponse.StatusCode,
                    failedResponse.Content.ReadAsStringAsync().Result));
        }


        protected async Task Delete(HttpRequestMessage requestMessage)
        {
            HttpRequestMessage clonedRequest = null;
            var response = await _retryPolicy.ExecuteAsync(async () =>
            {
                clonedRequest = new HttpRequestMessage(requestMessage.Method, requestMessage.RequestUri);
                clonedRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenService.GetToken());

                return await _httpClient.SendAsync(clonedRequest);

            });

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new HttpRequestException();
            }
        }

        private static ApiError GetApiErrorFromJson(string json)
        {
            try
            {
              return JsonConvert.DeserializeObject<ApiError>(json);
            }
            catch(JsonException)
            {
                return null;
            }
        }
    }
}

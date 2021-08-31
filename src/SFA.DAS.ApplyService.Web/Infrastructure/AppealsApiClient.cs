using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Infrastructure.ApiClients;
using SFA.DAS.ApplyService.InternalApi.Types.Requests.Appeals;
using SFA.DAS.ApplyService.InternalApi.Types.Responses.Appeals;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class AppealsApiClient : ApiClientBase<AppealsApiClient>, IAppealsApiClient
    {
        public AppealsApiClient(HttpClient httpClient, ILogger<AppealsApiClient> logger, ITokenService tokenService) : base(httpClient, logger)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.GetToken());

        }

        public async Task<GetAppealResponse> GetAppeal(Guid applicationId)
        {
            return await Get<GetAppealResponse>($"/Appeals/{applicationId}");
        }

        public async Task<bool> MakeAppeal(Guid applicationId, string howFailedOnPolicyOrProcesses, string howFailedOnEvidenceSubmitted, string signinId, string userName)
        {
            var request = new MakeAppealRequest
            {
                HowFailedOnPolicyOrProcesses = howFailedOnPolicyOrProcesses,
                HowFailedOnEvidenceSubmitted = howFailedOnEvidenceSubmitted,
                UserId = signinId,
                UserName = userName
            };

            var result = await Post($"/Appeals/{applicationId}", request);

            return result == HttpStatusCode.OK;
        }

        public async Task<GetAppealFileListResponse> GetAppealFileList(Guid applicationId)
        {
            return await Get<GetAppealFileListResponse>($"/Appeals/{applicationId}/files");
        }

        public async Task<HttpResponseMessage> DownloadFile(Guid applicationId, string fileName)
        {
            return await GetResponse($"/Appeals/{applicationId}/files/{fileName}");
        }

        public async Task<bool> UploadFile(Guid applicationId, IFormFile appealFileToUpload, string signinId, string userName)
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent(applicationId.ToString()), nameof(UploadAppealFileRequest.ApplicationId) },
                { new StringContent(signinId), nameof(UploadAppealFileRequest.UserId) },
                { new StringContent(userName), nameof(UploadAppealFileRequest.UserName) }
            };

            if (appealFileToUpload != null )
            {
                    var fileContent = new StreamContent(appealFileToUpload.OpenReadStream())
                    { Headers = { ContentLength = appealFileToUpload.Length, ContentType = new MediaTypeHeaderValue(appealFileToUpload.ContentType) } };

                    content.Add(fileContent, nameof(UploadAppealFileRequest.AppealFile), appealFileToUpload.FileName);
            }

            try
            {
                using (var response = await _httpClient.PostAsync($"/Appeals/{applicationId}/files", content))
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"Error when uploading appeal file for Application: {applicationId} | File: {appealFileToUpload?.FileName}");
                return false;
            }
        }

        public async Task<bool> DeleteFile(Guid applicationId, string fileName, string signinId, string userName)
        {
            var request = new DeleteAppealFileRequest
            {
                UserId = signinId,
                UserName = userName
            };

            var result = await Post($"/Appeals/{applicationId}/files/delete/{fileName}", request);

            return result == HttpStatusCode.OK;
        }
    }
}

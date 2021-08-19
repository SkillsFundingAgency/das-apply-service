using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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
    }
}

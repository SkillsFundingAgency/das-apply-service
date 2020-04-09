namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    using Microsoft.Extensions.Logging;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using SFA.DAS.ApplyService.InternalApi.Types;
    using SFA.DAS.ApplyService.InternalApi.Types.CharityCommission;
    using SFA.DAS.ApplyService.Infrastructure.ApiClients;

    public class CharityCommissionApiClient : ApiClientBase<CharityCommissionApiClient>, ICharityCommissionApiClient
    {
        public CharityCommissionApiClient(HttpClient httpClient, ILogger<CharityCommissionApiClient> logger, ITokenService tokenService) : base(httpClient, logger)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.GetToken());
        }

        public async Task<ApiResponse<Charity>> GetCharityDetails(int charityNumber)
        {
            var responseMessage = await GetResponse($"charity-commission-lookup?charityNumber={charityNumber}");

            if (responseMessage.StatusCode == HttpStatusCode.InternalServerError ||
                responseMessage.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                return new ApiResponse<Charity> { Success = false };
            }

            return new ApiResponse<Charity>
            {
                Success = true,
                Response = await responseMessage.Content.ReadAsAsync<Charity>()
            };
        }
    }
}
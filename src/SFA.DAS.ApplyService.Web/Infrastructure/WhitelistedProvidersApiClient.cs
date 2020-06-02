using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Infrastructure.ApiClients;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class WhitelistedProvidersApiClient : ApiClientBase<WhitelistedProvidersApiClient>, IWhitelistedProvidersApiClient
    {
        public WhitelistedProvidersApiClient(HttpClient httpClient, ILogger<WhitelistedProvidersApiClient> logger, ITokenService tokenService) : base(httpClient, logger)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.GetToken());
        }

        public async Task<bool> CheckIsWhitelistedUkprn(int ukprn)
        {
            return await Get<bool>($"whitelistedproviders/ukprn/{ukprn}");
        }
    }
}

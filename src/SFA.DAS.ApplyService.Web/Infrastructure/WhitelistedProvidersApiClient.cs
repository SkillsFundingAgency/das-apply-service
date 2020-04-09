using Microsoft.Extensions.Logging;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Infrastructure.ApiClients;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class WhitelistedProvidersApiClient : ApiClientBase<WhitelistedProvidersApiClient>, IWhitelistedProvidersApiClient
    {
        public WhitelistedProvidersApiClient(ILogger<WhitelistedProvidersApiClient> logger, IConfigurationService configurationService, ITokenService tokenService) : base(logger)
        {
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(configurationService.GetConfig().Result.InternalApi.Uri);
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.GetToken());
        }

        public async Task<bool> CheckIsWhitelistedUkprn(int ukprn)
        {
            return await Get<bool>($"whitelistedproviders/ukprn/{ukprn}");
        }
    }
}

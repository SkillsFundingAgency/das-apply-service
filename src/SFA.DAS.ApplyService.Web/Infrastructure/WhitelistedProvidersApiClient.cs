using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class WhitelistedProvidersApiClient : IWhitelistedProvidersApiClient
    {
        private readonly ILogger<ApplicationApiClient> _logger;
        private readonly ITokenService _tokenService;
        private static readonly HttpClient _httpClient = new HttpClient();

        public WhitelistedProvidersApiClient(IConfigurationService configurationService, ILogger<ApplicationApiClient> logger, ITokenService tokenService)
        {
            _logger = logger;
            _tokenService = tokenService;
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(configurationService.GetConfig().Result.InternalApi.Uri);
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenService.GetToken());
        }

        public async Task<bool> CheckIsWhitelistedUkprn(int ukprn)
        {
            return await (await _httpClient.GetAsync($"whitelistedproviders/ukprn/{ukprn}")).Content.ReadAsAsync<bool>();
        }
    }
}

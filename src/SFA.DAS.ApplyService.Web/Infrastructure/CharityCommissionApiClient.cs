
﻿namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    using Microsoft.Extensions.Logging;
    using SFA.DAS.ApplyService.Configuration;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using SFA.DAS.ApplyService.InternalApi.Types.CharityCommission;

    public class CharityCommissionApiClient : ICharityCommissionApiClient
    {
        private ILogger<CharityCommissionApiClient> _logger;

        private static readonly HttpClient _httpClient = new HttpClient();

        public CharityCommissionApiClient(IConfigurationService configurationService,
            ILogger<CharityCommissionApiClient> logger)
        {
            _logger = logger;
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(configurationService.GetConfig().Result.InternalApi.Uri);
            }
        }

        public async Task<Charity> GetCharityDetails(int charityNumber)
        {
            return await (await _httpClient.GetAsync($"charity-commission-lookup?charityNumber={charityNumber}")).Content.ReadAsAsync<Charity>();
        }
    }
}

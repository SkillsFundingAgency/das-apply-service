namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using SFA.DAS.ApplyService.Configuration;
    using SFA.DAS.ApplyService.Domain.Ukrlp;

    public class UkrlpApiClient : IUkrlpApiClient
    {
        private readonly ILogger<UkrlpApiClient> _logger;
        private static readonly HttpClient _httpClient = new HttpClient();

        public UkrlpApiClient(IConfigurationService configurationService, ILogger<UkrlpApiClient> logger)
        {
            _logger = logger;
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(configurationService.GetConfig().Result.InternalApi.Uri);
            }
        }

        public async Task<UkrlpLookupResults> GetTrainingProviderByUkprn(long ukprn)
        {
            return await (await _httpClient.GetAsync($"/ukrlp-lookup?ukprn={ukprn}")).Content
                .ReadAsAsync<UkrlpLookupResults>();
        }

    }
}
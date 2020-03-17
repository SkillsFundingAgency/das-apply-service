namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Configuration;
    using Domain.Roatp;
    using Microsoft.Extensions.Logging;
    using SFA.DAS.ApplyService.InternalApi.Models.Roatp;
    using SFA.DAS.ApplyService.InternalApi.Models.Ukrlp;

    public class RoatpApiClient : IRoatpApiClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<RoatpApiClient> _logger;
        private readonly IRoatpTokenService _tokenService;
        private string _baseAddress;

        public RoatpApiClient(HttpClient client, ILogger<RoatpApiClient> logger, IConfigurationService configurationService, IRoatpTokenService tokenService)
        {
            _logger = logger;
            _baseAddress = configurationService.GetConfig().Result.RoatpApiAuthentication.ApiBaseAddress;
            _client = client;
            _tokenService = tokenService;
        }

        public async Task<OrganisationRegisterStatus> GetOrganisationRegisterStatus(string ukprn)
        {
            _logger.LogInformation($"Looking up RoATP status for UKPRN {ukprn}");

            var apiResponse = await Get<OrganisationRegisterStatus>(
                $"{_baseAddress}/api/v1/organisation/register-status?&ukprn={ukprn}");

            return apiResponse;
        }

        public async Task<IEnumerable<ProviderType>> GetProviderTypes()
        {
            _logger.LogInformation($"Retrieving RoATP provider types");

            var apiResponse = await Get<IEnumerable<ProviderType>>($"{_baseAddress}/api/v1/lookupData/providerTypes");

            return apiResponse;
        }

        public async Task<UkprnLookupResponse> GetUkrlpDetails(string ukprn)
        {
            _logger.LogInformation($"Retrieving UKRLP details for {ukprn}");

            var apiResponse = await Get<UkprnLookupResponse>($"{_baseAddress}/api/v1/ukrlp/lookup/{ukprn}");

            return apiResponse;
        }

        public async Task<IEnumerable<ProviderDetails>> GetUkrlpProviderDetails(string ukprn)
        {
            var res =
                await Get<UkprnLookupResponse>($"{_baseAddress}/api/v1/ukrlp/lookup/{ukprn}");

            return res.Results;
        }

        private async Task<T> Get<T>(string uri)
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _tokenService.GetToken());

            using (var response = await _client.GetAsync(new Uri(uri, UriKind.Absolute)))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<T>();
                }

                return default(T);
            }
        }
    }
}

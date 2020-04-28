namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using SFA.DAS.ApplyService.Domain.Roatp;
    using SFA.DAS.ApplyService.InternalApi.Models.Roatp;
    using SFA.DAS.ApplyService.InternalApi.Models.Ukrlp;
    using SFA.DAS.ApplyService.Infrastructure.ApiClients;
    using System;

    public class RoatpApiClient : ApiClientBase<RoatpApiClient>, IRoatpApiClient
    {
        public RoatpApiClient(HttpClient httpClient, ILogger<RoatpApiClient> logger, IRoatpTokenService tokenService) : base(httpClient, logger)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.GetToken(_httpClient.BaseAddress.ToString()));
        }

        public async virtual Task<OrganisationRegisterStatus> GetOrganisationRegisterStatus(string ukprn)
        {
            _logger.LogInformation($"Looking up RoATP status for UKPRN {ukprn}");

            return await Get<OrganisationRegisterStatus>($"/api/v1/organisation/register-status?&ukprn={ukprn}");
        }

        public async Task<IEnumerable<ProviderType>> GetProviderTypes()
        {
            _logger.LogInformation($"Retrieving RoATP provider types");

            return await Get<List<ProviderType>>($"/api/v1/lookupData/providerTypes");
        }

        public async virtual Task<UkprnLookupResponse> GetUkrlpDetails(string ukprn)
        {
            _logger.LogInformation($"Retrieving UKRLP details for {ukprn}");

            var apiResponse = await Get<UkprnLookupResponse>($"{_httpClient.BaseAddress}/api/v1/ukrlp/lookup/{ukprn}");

            return apiResponse;
        }

        private async Task<T> Get<T>(string uri)
        {
            using (var response = await _httpClient.GetAsync(new Uri(uri, UriKind.Absolute)))
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

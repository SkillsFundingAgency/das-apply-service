namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Configuration;
    using Microsoft.Extensions.Logging;
    using SFA.DAS.ApplyService.InternalApi.Models.Roatp;

    public class RoatpApiClient : IRoatpApiClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<RoatpApiClient> _logger;
        private readonly IRoatpTokenService _tokenService;
        private string _baseAddress;

        public RoatpApiClient(HttpClient client, ILogger<RoatpApiClient> logger, IApplyConfig config, IRoatpTokenService tokenService)
        {
            _logger = logger;
            _baseAddress = config.RoatpApiAuthentication.ApiBaseAddress;
            _client = client;
            _tokenService = tokenService;
        }

        public async Task<DuplicateCheckResponse> DuplicateUKPRNCheck(Guid organisationId, long ukprn)
        {
            _logger.LogInformation($"Performing duplicate UKPRN check for {ukprn}");

            var apiResponse = await Get<DuplicateCheckResponse>(
                $"{_baseAddress}/api/v1/duplicateCheck/ukprn?ukprn={ukprn}&organisationId={organisationId}");

            return apiResponse;
        }

        public async Task<OrganisationReapplyStatus> GetOrganisationReapplyStatus(Guid organisationId)
        {
            _logger.LogInformation($"Looking up reapply status for organisation id {organisationId}");

            var apiResponse = await Get<OrganisationReapplyStatus>(
                $"{_baseAddress}/api/v1/organisation/reapply-status?&organisationId={organisationId}");

            return apiResponse;
        }

        public async Task<IEnumerable<ProviderType>> GetProviderTypes()
        {
            _logger.LogInformation($"Retrieving RoATP provider types");

            var apiResponse = await Get<IEnumerable<ProviderType>>($"{_baseAddress}/api/v1/lookupData/providerTypes");

            return apiResponse;
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

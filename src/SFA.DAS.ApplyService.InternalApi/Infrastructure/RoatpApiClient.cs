namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using SFA.DAS.ApplyService.Configuration;
    using SFA.DAS.ApplyService.Domain.Roatp;
    using SFA.DAS.ApplyService.InternalApi.Models.Roatp;
    using SFA.DAS.ApplyService.InternalApi.Models.Ukrlp;
    using SFA.DAS.ApplyService.Infrastructure.ApiClients;

    public class RoatpApiClient : ApiClientBase<RoatpApiClient>, IRoatpApiClient
    {
        public RoatpApiClient(ILogger<RoatpApiClient> logger, IConfigurationService configurationService, IRoatpTokenService tokenService) : base(logger)
        {
            var _config = configurationService.GetConfig().Result;

            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(_config.RoatpApiAuthentication.ApiBaseAddress);
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.GetToken());
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

            return await Get<UkprnLookupResponse>($"/api/v1/ukrlp/lookup/{ukprn}");
        }
    }
}

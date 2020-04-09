namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using SFA.DAS.ApplyService.Configuration;
    using SFA.DAS.ApplyService.Domain.Roatp;
    using SFA.DAS.ApplyService.Domain.Ukrlp;
    using SFA.DAS.ApplyService.Infrastructure.ApiClients;

    public class RoatpApiClient : ApiClientBase<RoatpApiClient>, IRoatpApiClient
    {
        public RoatpApiClient(ILogger<RoatpApiClient> logger, IConfigurationService configurationService, ITokenService tokenService) : base(logger)
        {
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(configurationService.GetConfig().Result.InternalApi.Uri);
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.GetToken());
        }

        public async Task<IEnumerable<ApplicationRoute>> GetApplicationRoutes()
        {
            return await Get<List<ApplicationRoute>>($"/all-roatp-routes");
        }

        public async Task<OrganisationRegisterStatus> GetOrganisationRegisterStatus(long ukprn)
        {
            return await Get<OrganisationRegisterStatus>($"/ukprn-on-register?ukprn={ukprn}");
        }
        
        public async Task<IEnumerable<ProviderDetails>> GetUkrlpProviderDetails(string ukprn)
        {
            return await Get<List<ProviderDetails>>($"//api/v1/ukrlp/lookup/{ukprn}");
        }
    }
}
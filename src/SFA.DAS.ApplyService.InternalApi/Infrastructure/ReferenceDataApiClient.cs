using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Infrastructure.ApiClients;
using SFA.DAS.ApplyService.InternalApi.Models.ReferenceData;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    public class ReferenceDataApiClient : ApiClientBase<ReferenceDataApiClient>
    {
        private readonly IApplyConfig _config;

        public ReferenceDataApiClient(HttpClient httpClient, ILogger<ReferenceDataApiClient> logger, IConfigurationService configurationService) : base(httpClient, logger)
        {
            _config = configurationService.GetConfig().GetAwaiter().GetResult();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
        }

        public async Task<IEnumerable<Types.OrganisationSearchResult>> SearchOrgansiation(string searchTerm, bool exactMatch)
        {
            _logger.LogInformation($"Searching Reference Data API. Search Term: {searchTerm}");
            var apiResponse = await Get<List<Organisation>>($"/api/organisations?searchTerm={searchTerm}");

            if(exactMatch)
            {
                apiResponse = apiResponse?.Where(r => r.Name.Equals(searchTerm, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            return Mapper.Map<IEnumerable<Organisation>, IEnumerable<Types.OrganisationSearchResult>>(apiResponse);
        }

        private string GetToken()
        {
            var tenantId = _config.ReferenceDataApiAuthentication.TenantId;
            var clientId = _config.ReferenceDataApiAuthentication.ClientId;
            var clientSecret = _config.ReferenceDataApiAuthentication.ClientSecret;
            var resourceId = _config.ReferenceDataApiAuthentication.ResourceId;

            var authority = $"https://login.microsoftonline.com/{tenantId}";
            var clientCredential = new ClientCredential(clientId, clientSecret);
            var context = new AuthenticationContext(authority, true);
            var result = context.AcquireTokenAsync(resourceId, clientCredential).Result;

            return result.AccessToken;
        }
    }
}

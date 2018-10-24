using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System;
using SFA.DAS.ApplyService.InternalApi.Models.AssessorService;
using AutoMapper;
using SFA.DAS.ApplyService.Configuration;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    public class AssessorServiceApiClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<AssessorServiceApiClient> _logger;
        private readonly IApplyConfig _config;

        public AssessorServiceApiClient(HttpClient client, ILogger<AssessorServiceApiClient> logger, IConfigurationService configurationService)
        {
            _client = client;
            _logger = logger;
            _config = configurationService.GetConfig().Result;
        }

        public async Task<IEnumerable<Types.Organisation>> SearchOrgansiation(string searchTerm)
        {
            var apiResponse = await Get<IEnumerable<OrganisationSummary>>($"/api/ao/assessment-organisations/search/{searchTerm}");

            return Mapper.Map<IEnumerable<OrganisationSummary>, IEnumerable<Types.Organisation>>(apiResponse);
        }

        private async Task<T> Get<T>(string uri)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());

            using (var response = await _client.GetAsync(new Uri(uri, UriKind.Relative)))
            {
                return await response.Content.ReadAsAsync<T>();
            }
        }

        private string GetToken()
        {
            var tenantId = _config.AssessorServiceApiAuthentication.TenantId;
            var clientId = _config.AssessorServiceApiAuthentication.ClientId;
            var clientSecret = _config.AssessorServiceApiAuthentication.ClientSecret;
            var resourceId = _config.AssessorServiceApiAuthentication.ResourceId;

            var authority = $"https://login.microsoftonline.com/{tenantId}";
            var clientCredential = new ClientCredential(clientId, clientSecret);
            var context = new AuthenticationContext(authority, true);
            var result = context.AcquireTokenAsync(resourceId, clientCredential).Result;

            return result.AccessToken;
        }
    }
}

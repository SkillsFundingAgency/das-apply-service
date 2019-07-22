using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.InternalApi.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class OrganisationSearchApiClient : ApiClient
    {
        private readonly ILogger<OrganisationSearchApiClient> _logger;

        public OrganisationSearchApiClient(ILogger<OrganisationSearchApiClient> logger, IConfigurationService configurationService) : base(logger, configurationService)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<OrganisationSearchResult>> SearchOrganisation(string searchTerm)
        {
            return await Get<IEnumerable<OrganisationSearchResult>>($"/OrganisationSearch?searchTerm={searchTerm}");
        }

        public async Task<OrganisationSearchResult> GetOrganisationByEmail(string email)
        {
            // NOTE: Original author of this code wanted to log the call and then process things manually.
            // If this not required, move to: return await Get<OrganisationSearchResult>($"/OrganisationSearch/email/{WebUtility.UrlEncode(email)}");
            _logger.LogInformation($"Calling OrganisationSearch/email from: {_httpClient.BaseAddress}/OrganisationSearch/email/{email}");

            using (var httpResponseMessage = await Get($"/OrganisationSearch/email/{WebUtility.UrlEncode(email)}"))
            {
                var responseAsString = await httpResponseMessage.Content.ReadAsStringAsync();

                _logger.LogInformation($"Content received from OrganisationSearch/email: {responseAsString}");

                return JsonConvert.DeserializeObject<OrganisationSearchResult>(responseAsString);
            }
        }

        public async Task<IEnumerable<OrganisationType>> GetOrganisationTypes()
        {
            var types = await Get<IEnumerable<OrganisationType>>($"/OrganisationTypes");

            return types?.OrderBy(t => t.Type.Equals("Public Sector", StringComparison.InvariantCultureIgnoreCase)).AsEnumerable();
        }

        public async Task<bool> IsCompanyActivelyTrading(string companyNumber)
        {
            return await Get<bool>($"/OrganisationSearch/{companyNumber}/isActivelyTrading");
        }
    }
}

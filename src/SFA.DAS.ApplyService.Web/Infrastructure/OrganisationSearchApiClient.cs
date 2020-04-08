using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.InternalApi.Types;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class OrganisationSearchApiClient : ApiClientBase<OrganisationSearchApiClient>
    {
        public OrganisationSearchApiClient(ILogger<OrganisationSearchApiClient> logger, IConfigurationService configurationService, ITokenService tokenService) : base(logger)
        {
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(configurationService.GetConfig().Result.InternalApi.Uri);
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.GetToken());
        }

        public async Task<IEnumerable<OrganisationSearchResult>> SearchOrganisation(string searchTerm)
        {
            return await Get<List<OrganisationSearchResult>>($"/OrganisationSearch?searchTerm={searchTerm}");
        }

        public async Task<OrganisationSearchResult> GetOrganisationByEmail(string email)
        {
            return await Get<OrganisationSearchResult>($"/OrganisationSearch/email/{WebUtility.UrlEncode(email)}");
        }

        public async Task<IEnumerable<OrganisationType>> GetOrganisationTypes()
        {
            var types = await Get<List<OrganisationType>>($"/OrganisationTypes");

            return types?.OrderBy(t => t.Type.Equals("Public Sector", StringComparison.InvariantCultureIgnoreCase)).AsEnumerable();
        }

        public async Task<bool> IsCompanyActivelyTrading(string companyNumber)
        {
            return await Get<bool>($"/OrganisationSearch/{companyNumber}/isActivelyTrading");
        }
    }
}

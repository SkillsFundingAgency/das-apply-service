using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    public class OrganisationSearchController : Controller
    {
        private readonly AssessorServiceApiClient _assessorServiceApiClient;
        private readonly ProviderRegisterApiClient _providerRegisterApiClient;
        private readonly ReferenceDataApiClient _referenceDataApiClient;

        public OrganisationSearchController(AssessorServiceApiClient assessorServiceApiClient, ProviderRegisterApiClient providerRegisterApiClient, ReferenceDataApiClient referenceDataApiClient)
        {
            _assessorServiceApiClient = assessorServiceApiClient;
            _providerRegisterApiClient = providerRegisterApiClient;
            _referenceDataApiClient = referenceDataApiClient;
        }

        [HttpGet("OrganisationSearch")]
        public async Task<IEnumerable<Organisation>> Search(string searchTerm)
        {
            IEnumerable<Organisation> results;

            // FIRST - Search EPAO Register
            results = await _assessorServiceApiClient.SearchOrgansiation(searchTerm);

            if (results == null || !results.Any())
            {
                // SECOND - Search Provider Register 
                results = await _providerRegisterApiClient.SearchOrgansiation(searchTerm); 

                if (results == null || !results.Any())
                {
                    // THIRD - Use Reference Data API
                    results = await _referenceDataApiClient.SearchOrgansiation(searchTerm);
                }
            }

            return results;
        }
    }
}
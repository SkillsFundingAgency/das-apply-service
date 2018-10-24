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
            IEnumerable<Organisation> results = null;

            // FIRST - Search EPAO Register <-- hit assessor service url
            var response1 = await _assessorServiceApiClient.SearchOrgansiation(searchTerm);

            if (response1 == null || !response1.Any())
            {
                // SECOND - Search Provider Register 
                var response2 = await _providerRegisterApiClient.SearchOrgansiation(searchTerm); 

                if (response2 == null || !response2.Any())
                {
                    // THIRD use Reference Data API
                    var response3 = await _referenceDataApiClient.SearchOrgansiation(searchTerm);
                }
            }

            return results;
        }
    }
}
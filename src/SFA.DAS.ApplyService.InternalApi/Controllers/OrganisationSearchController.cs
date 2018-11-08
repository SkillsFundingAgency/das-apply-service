using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    public class OrganisationSearchController : Controller
    {
        private readonly ILogger<OrganisationSearchController> _logger;
        private readonly AssessorServiceApiClient _assessorServiceApiClient;
        private readonly ProviderRegisterApiClient _providerRegisterApiClient;
        private readonly ReferenceDataApiClient _referenceDataApiClient;

        public OrganisationSearchController(ILogger<OrganisationSearchController> logger, AssessorServiceApiClient assessorServiceApiClient, ProviderRegisterApiClient providerRegisterApiClient, ReferenceDataApiClient referenceDataApiClient)
        {
            _logger = logger;
            _assessorServiceApiClient = assessorServiceApiClient;
            _providerRegisterApiClient = providerRegisterApiClient;
            _referenceDataApiClient = referenceDataApiClient;
        }

        [HttpGet("OrganisationSearch")]
        public async Task<IEnumerable<Organisation>> OrganisationSearch(string searchTerm)
        {
            List<Organisation> results = new List<Organisation>();

            // EPAO Register
            try
            {
                var epaoResults = await _assessorServiceApiClient.SearchOrgansiation(searchTerm);
                if (epaoResults != null) results.AddRange(epaoResults);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error from EPAO Register. Message: {ex.Message}");
            }

            // Provider Register
            try
            {
                var providerResults = await _providerRegisterApiClient.SearchOrgansiation(searchTerm);
                if (providerResults != null) results.AddRange(providerResults);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error from Provider Register. Message: {ex.Message}");
            }

            // Reference Data API
            try
            {
                var referenceResults = await _referenceDataApiClient.SearchOrgansiation(searchTerm);
                if (referenceResults != null) results.AddRange(referenceResults);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error from Reference Data API. Message: {ex.Message}");
            }

            // de-dupe
            return Dedupe(results);
        }

        private IEnumerable<Organisation> Dedupe(IEnumerable<Organisation> organisations)
        {
            var query = organisations.GroupBy(org => org.Name.ToUpperInvariant())
                .Select(group => 
                    new Organisation
                    {
                        Id = group.Select(g => g.Id).FirstOrDefault(Id => !string.IsNullOrWhiteSpace(Id)),
                        Ukprn = group.Select(g => g.Ukprn).FirstOrDefault(Ukprn => Ukprn.HasValue),
                        Name = group.Select(g => g.Name).FirstOrDefault(Name => !string.IsNullOrWhiteSpace(Name)),
                        Address = group.Select(g => g.Address).FirstOrDefault(Address => Address != null),
                        OrganisationType = group.Select(g => g.OrganisationType).FirstOrDefault(OrganisationType => !string.IsNullOrWhiteSpace(OrganisationType)),
                        OrganisationReferenceType = string.Join(", ", group.Select(g => g.Id).Where(Id => !string.IsNullOrWhiteSpace(Id))),
                    }
                );

            return query.OrderBy(org => org.Name).AsEnumerable();
        }

        [HttpGet("OrganisationByEmail")]
        public async Task<Organisation> GetOrganisationByEmail(string emailAddress)
        {
            Organisation result = null;

            try
            {
                result = await _assessorServiceApiClient.GetOrganisationByEmail(emailAddress);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error from EPAO Register. Message: {ex.Message}");
            }

            return result;
        }

        [HttpGet("OrganisationTypes")]
        public async Task<IEnumerable<string>> GetOrganisationTypes()
        {
            IEnumerable<string> results = null;

            try
            {
                results = await _assessorServiceApiClient.GetOrgansiationTypes();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error from EPAO Register. Message: {ex.Message}");
            }

            return results;
        }
    }
}
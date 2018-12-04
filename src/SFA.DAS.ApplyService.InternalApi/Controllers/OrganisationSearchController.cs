using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        public async Task<IEnumerable<OrganisationSearchResult>> OrganisationSearch(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return new List<OrganisationSearchResult>();
            }
            else if (IsValidEpaOrganisationId(searchTerm))
            {
                return await OrganisationSearchByEpao(searchTerm);
            }
            else if (IsValidUkprn(searchTerm, out var ukprn))
            {
                return await OrganisationSearchByUkprn(ukprn);
            }
            else
            {
                return await OrganisationSearchByName(searchTerm);
            }
        }

        private bool IsValidEpaOrganisationId(string organisationIdToCheck)
        {
            var regex = new Regex(@"[eE][pP][aA][0-9]{4,9}$");
            return regex.Match(organisationIdToCheck).Success;
        }

        private bool IsValidUkprn(string stringToCheck, out int ukprn)
        {
            if (!int.TryParse(stringToCheck, out ukprn))
            {
                return false;
            }

            return ukprn >= 10000000 && ukprn <= 99999999;
        }

        private async Task<IEnumerable<OrganisationSearchResult>> OrganisationSearchByUkprn(int ukprn)
        {
            IEnumerable<OrganisationSearchResult> epaoResults = null;
            OrganisationSearchResult providerResult = null;
            IEnumerable<OrganisationSearchResult> referenceResults = null;

            // EPAO Register
            try
            {
                epaoResults = await _assessorServiceApiClient.SearchOrgansiation(ukprn.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error from EPAO Register. UKPRN: {ukprn} , Message: {ex.Message}");
            }

            // Provider Register
            try
            {
                providerResult = await _providerRegisterApiClient.SearchOrgansiationByUkprn(ukprn);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error from Provider Register. UKPRN: {ukprn} , Message: {ex.Message}");
            }

            // Reference Data API
            try
            {
                string searchTerm = null;

                // This API has issues with Ltd & Limited and double spaces in some records
                // If you try to search by UKPRN it interprets this as Company Name so must use actual name instead
                if (epaoResults?.Count() == 1)
                {
                    searchTerm = epaoResults.First().Name;
                }
                else if (providerResult != null)
                {
                    searchTerm = providerResult.Name;
                }

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    referenceResults = await _referenceDataApiClient.SearchOrgansiation(searchTerm, true);

                    if (referenceResults?.Count() > 0)
                    {
                        foreach (var result in referenceResults)
                        {
                            result.Ukprn = ukprn;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error from Reference Data API. UKPRN: {ukprn} , Message: {ex.Message}");
            }

            var results = new List<OrganisationSearchResult>();

            if (epaoResults != null) results.AddRange(epaoResults);
            if (providerResult != null) results.Add(providerResult);
            if (referenceResults != null) results.AddRange(referenceResults);

            return Dedupe(results);
        }

        private async Task<IEnumerable<OrganisationSearchResult>> OrganisationSearchByEpao(string epao)
        {
            IEnumerable<OrganisationSearchResult> epaoResults = null;
            OrganisationSearchResult providerResult = null;
            IEnumerable<OrganisationSearchResult> referenceResults = null;

            // EPAO Register
            try
            {
                epaoResults = await _assessorServiceApiClient.SearchOrgansiation(epao);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error from EPAO Register. EPAO: {epao} , Message: {ex.Message}");
            }

            // Provider Register
            try
            {
                // If the EPAO ID is not on this register then you can't search by UKPRN as there is no EPAO UKPRN search
                providerResult = await _providerRegisterApiClient.SearchOrgansiationByEpao(epao);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error from Provider Register. EPAO: {epao} , Message: {ex.Message}");
            }

            // Reference Data API
            try
            {
                int? ukprn = null;
                string searchTerm = null;

                // This API has issues with Ltd & Limited and double spaces in some records
                // If you try to search by EPAO ID it interprets this as Company Name so must use actual name instead
                if (epaoResults?.Count() == 1)
                {
                    searchTerm = epaoResults.First().Name;
                    ukprn = epaoResults.First().Ukprn;
                }
                else if (providerResult != null)
                {
                    searchTerm = providerResult.Name;
                    ukprn = providerResult.Ukprn;
                }

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    referenceResults = await _referenceDataApiClient.SearchOrgansiation(searchTerm, true);

                    if (referenceResults?.Count() > 0)
                    {
                        foreach (var result in referenceResults)
                        {
                            result.Ukprn = ukprn;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error from Reference Data API. EPAO: {epao} , Message: {ex.Message}");
            }

            var results = new List<OrganisationSearchResult>();

            if (epaoResults != null) results.AddRange(epaoResults);
            if (providerResult != null) results.Add(providerResult);
            if (referenceResults != null) results.AddRange(referenceResults);

            return Dedupe(results);
        }

        private async Task<IEnumerable<OrganisationSearchResult>> OrganisationSearchByName(string name)
        {
            IEnumerable<OrganisationSearchResult> epaoResults = null;
            IEnumerable<OrganisationSearchResult> providerResults = null;
            IEnumerable<OrganisationSearchResult> referenceResults = null;

            // EPAO Register
            try
            {
                epaoResults = await _assessorServiceApiClient.SearchOrgansiation(name);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error from EPAO Register. Name: {name} , Message: {ex.Message}");
            }

            // Provider Register
            try
            {
                // If we have an exact result from EPAO Register and it has a UKPRN then search by UKPRN
                if (epaoResults?.Count() == 1 && epaoResults.First().Ukprn.HasValue)
                {
                    var providerUkprnResult = await _providerRegisterApiClient.SearchOrgansiationByUkprn(epaoResults.First().Ukprn.Value);

                    // Remember there is only Provider UKPRN search so if it is an EPAO UKPRN then you won't find it
                    if (providerUkprnResult != null)
                    {
                        providerResults = new List<OrganisationSearchResult> { providerUkprnResult };
                    }
                }

                // If not an exact result from EPAO Register OR... no results returned from UKPRN search, then carry on with searching by name
                if (providerResults is null)
                {
                    providerResults = await _providerRegisterApiClient.SearchOrgansiationByName(name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error from Provider Register. Name: {name} , Message: {ex.Message}");
            }

            // Reference Data API
            try
            {
                bool mustBeExactMatch = false;

                // This API has issues with Ltd & Limited and double spaces in some records
                // Note that these two results could bring back different names
                if (epaoResults?.Count() == 1 && string.Equals(epaoResults.First().Name.Trim(), name, StringComparison.InvariantCultureIgnoreCase))
                {
                    name = epaoResults.First().Name.Trim();
                    mustBeExactMatch = true;
                }
                else if (epaoResults?.Count() <= 1 & providerResults?.Count() == 1 && string.Equals(providerResults.First().Name.Trim(), name, StringComparison.InvariantCultureIgnoreCase))
                {
                    name = providerResults.First().Name.Trim();
                    mustBeExactMatch = true;
                }

                referenceResults = await _referenceDataApiClient.SearchOrgansiation(name, mustBeExactMatch);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error from Reference Data API. Name: {name} , Message: {ex.Message}");
            }

            var results = new List<OrganisationSearchResult>();

            if (epaoResults != null) results.AddRange(epaoResults);
            if (providerResults != null) results.AddRange(providerResults);
            if (referenceResults != null) results.AddRange(referenceResults);

            return Dedupe(results);
        }


        private IEnumerable<OrganisationSearchResult> Dedupe(IEnumerable<OrganisationSearchResult> organisations)
        {
            // TODO: Numbers & Approved
            var nameMerge = organisations.GroupBy(org => org.Name.ToUpperInvariant())
                .Select(group =>
                    new OrganisationSearchResult
                    {
                        Id = group.Select(g => g.Id).FirstOrDefault(Id => !string.IsNullOrWhiteSpace(Id)),
                        Ukprn = group.Select(g => g.Ukprn).FirstOrDefault(Ukprn => Ukprn.HasValue),
                        LegalName = group.Select(g => g.LegalName).FirstOrDefault(LegalName => !string.IsNullOrWhiteSpace(LegalName)),
                        TradingName = group.Select(g => g.TradingName).FirstOrDefault(TradingName => !string.IsNullOrWhiteSpace(TradingName)),
                        ProviderName = group.Select(g => g.ProviderName).FirstOrDefault(ProviderName => !string.IsNullOrWhiteSpace(ProviderName)),
                        Address = group.Select(g => g.Address).FirstOrDefault(Address => Address != null),
                        OrganisationType = group.Select(g => g.OrganisationType).FirstOrDefault(OrganisationType => !string.IsNullOrWhiteSpace(OrganisationType)),
                        OrganisationReferenceType = group.Select(g => g.OrganisationReferenceType).FirstOrDefault(OrganisationReferenceType => !string.IsNullOrWhiteSpace(OrganisationReferenceType)),
                        OrganisationReferenceId = string.Join(",", group.Select(g => g.Id).Where(Id => !string.IsNullOrWhiteSpace(Id))),
                        RoEPAOApproved = group.Select(g => g.RoEPAOApproved).FirstOrDefault(RoEPAOApproved => RoEPAOApproved != false),
                        RoATPApproved = group.Select(g => g.RoATPApproved).FirstOrDefault(RoATPApproved => RoATPApproved != false),
                        CompanyNumber = group.Select(g => g.CompanyNumber).FirstOrDefault(CompanyNumber => !string.IsNullOrWhiteSpace(CompanyNumber)),
                        CharityNumber = group.Select(g => g.CharityNumber).FirstOrDefault(CharityNumber => !string.IsNullOrWhiteSpace(CharityNumber))
                    }
                );

            var ukprnMerge = nameMerge.GroupBy(org => new { filter = org.Ukprn.HasValue ? org.Ukprn.ToString() : org.Name.ToUpperInvariant() })
                .Select(group =>
                    new OrganisationSearchResult
                    {
                        Id = group.Select(g => g.Id).FirstOrDefault(Id => !string.IsNullOrWhiteSpace(Id)),
                        Ukprn = group.Select(g => g.Ukprn).FirstOrDefault(Ukprn => Ukprn.HasValue),
                        LegalName = group.Select(g => g.LegalName).FirstOrDefault(LegalName => !string.IsNullOrWhiteSpace(LegalName)),
                        TradingName = group.Select(g => g.TradingName).FirstOrDefault(TradingName => !string.IsNullOrWhiteSpace(TradingName)),
                        ProviderName = group.Select(g => g.ProviderName).FirstOrDefault(ProviderName => !string.IsNullOrWhiteSpace(ProviderName)),
                        Address = group.Select(g => g.Address).FirstOrDefault(Address => Address != null),
                        OrganisationType = group.Select(g => g.OrganisationType).FirstOrDefault(OrganisationType => !string.IsNullOrWhiteSpace(OrganisationType)),
                        OrganisationReferenceType = group.Select(g => g.OrganisationReferenceType).FirstOrDefault(OrganisationReferenceType => !string.IsNullOrWhiteSpace(OrganisationReferenceType)),
                        OrganisationReferenceId = string.Join(",", group.Select(g => g.Id).Where(Id => !string.IsNullOrWhiteSpace(Id))),
                        CompanyNumber = group.Select(g => g.CompanyNumber).FirstOrDefault(CompanyNumber => !string.IsNullOrWhiteSpace(CompanyNumber)),
                        CharityNumber = group.Select(g => g.CharityNumber).FirstOrDefault(CharityNumber => !string.IsNullOrWhiteSpace(CharityNumber)),
                        RoEPAOApproved = group.Select(g => g.RoEPAOApproved).FirstOrDefault(RoEPAOApproved => RoEPAOApproved != false),
                        RoATPApproved = group.Select(g => g.RoATPApproved).FirstOrDefault(RoATPApproved => RoATPApproved != false)
                    }
                );

            return ukprnMerge.OrderByDescending(org => org.Ukprn).ToList();
        }

        [HttpGet("OrganisationSearch/email/{email}")]
        public async Task<OrganisationSearchResult> GetOrganisationByEmail(string email)
        {
            OrganisationSearchResult result;

            // EPAO Register
            try
            {
                result = await _assessorServiceApiClient.GetOrganisationByEmail(email);
            }
            catch (Exception ex)
            {
                result = null;
                _logger.LogError($"Error from EPAO Register. Message: {ex.Message}");
            }

            // de-dupe
            return result;
        }

        [HttpGet("OrganisationTypes")]
        public async Task<IEnumerable<OrganisationType>> GetOrganisationTypes()
        {
            IEnumerable<OrganisationType> results = null;

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
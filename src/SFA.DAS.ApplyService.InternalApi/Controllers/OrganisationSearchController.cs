using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.InternalApi.Models.AssessorService;

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
            IEnumerable<OrganisationSearchResult> epaoResults = await GetEpaoRegisterResults(ukprn.ToString());
            IEnumerable<OrganisationSearchResult> providerResults = null;
            IEnumerable<OrganisationSearchResult> referenceResults = null;

            var providerRegisterNames = new List<string>();
            if (epaoResults?.Count() == 1)
            {
                providerRegisterNames.Add(epaoResults.First().TradingName);
                providerRegisterNames.Add(epaoResults.First().LegalName);
            }
            providerResults = await GetProviderRegisterResults(null, providerRegisterNames, ukprn);

            // If you try to search Reference Data API by UKPRN it interprets this as Company Number so must use actual name instead
            var referenceDataApiNames = new List<string> (providerRegisterNames);
            if (providerResults?.Count() == 1)
            {
                referenceDataApiNames.Add(providerResults.First().ProviderName);
            }
            referenceResults = await GetReferenceDataResults(null, referenceDataApiNames, ukprn);

            var results = new List<OrganisationSearchResult>();
            if (epaoResults != null) results.AddRange(epaoResults);
            if (providerResults != null) results.AddRange(providerResults);
            if (referenceResults != null) results.AddRange(referenceResults);

            return Dedupe(results);
        }

        private async Task<IEnumerable<OrganisationSearchResult>> OrganisationSearchByEpao(string epaoId)
        {
            IEnumerable<OrganisationSearchResult> epaoResults = await GetEpaoRegisterResults(epaoId);
            IEnumerable<OrganisationSearchResult> providerResults = null;
            IEnumerable<OrganisationSearchResult> referenceResults = null;
            int? ukprn = null;

            var providerRegisterNames = new List<string>();
            if (epaoResults?.Count() == 1)
            {
                providerRegisterNames.Add(epaoResults.First().TradingName);
                providerRegisterNames.Add(epaoResults.First().LegalName);
                ukprn = epaoResults.First().Ukprn;
            }
            providerResults = await GetProviderRegisterResults(null, providerRegisterNames, ukprn);

            // If you try to search Reference Data API by EPAO ID it interprets this as Company Name so must use actual name instead
            var referenceDataApiNames = new List<string>(providerRegisterNames);
            if (providerResults?.Count() == 1)
            {
                referenceDataApiNames.Add(providerResults.First().ProviderName);
            }
            referenceResults = await GetReferenceDataResults(null, referenceDataApiNames, ukprn);

            var results = new List<OrganisationSearchResult>();
            if (epaoResults != null) results.AddRange(epaoResults);
            if (providerResults != null) results.AddRange(providerResults);
            if (referenceResults != null) results.AddRange(referenceResults);

            return Dedupe(results);
        }

        private async Task<IEnumerable<OrganisationSearchResult>> OrganisationSearchByName(string name)
        {
            IEnumerable<OrganisationSearchResult> epaoResults = await GetEpaoRegisterResults(name);
            IEnumerable<OrganisationSearchResult> providerResults = null;
            IEnumerable<OrganisationSearchResult> referenceResults = null;
            int? ukprn = null;

            var providerRegisterNames = new List<string>();
            if (epaoResults?.Count() == 1)
            {
                providerRegisterNames.Add(epaoResults.First().TradingName);
                providerRegisterNames.Add(epaoResults.First().LegalName);
                ukprn = epaoResults.First().Ukprn;
            }
            providerResults = await GetProviderRegisterResults(name, providerRegisterNames, ukprn);

            var referenceDataApiNames = new List<string>(providerRegisterNames);
            if (providerResults?.Count() == 1)
            {
                referenceDataApiNames.Add(providerResults.First().ProviderName);
            }
            referenceResults = await GetReferenceDataResults(name, referenceDataApiNames, ukprn);

            var results = new List<OrganisationSearchResult>();
            if (epaoResults != null) results.AddRange(epaoResults);
            if (providerResults != null) results.AddRange(providerResults);
            if (referenceResults != null) results.AddRange(referenceResults);

            return Dedupe(results);
        }

        private async Task<IEnumerable<OrganisationSearchResult>> GetEpaoRegisterResults(string searchTerm)
        {
            var results = new List<OrganisationSearchResult>();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                try
                {
                    var response = await _assessorServiceApiClient.SearchOrgansiation(searchTerm);
                    if (response != null) results.AddRange(response);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error from EPAO Register. Search Term: {searchTerm} , Message: {ex.Message}");
                }
            }

            return results.GroupBy(r => r.Ukprn).Select(group => group.First()).ToList();
        }

        private async Task<IEnumerable<OrganisationSearchResult>> GetProviderRegisterResults(string name, IEnumerable<string> exactNames, int? ukprn)
        {
            var results = new List<OrganisationSearchResult>();

            if (!string.IsNullOrEmpty(name))
            {
                try
                {
                    var response = await _providerRegisterApiClient.SearchOrgansiationByName(name, false);
                    if (response != null) results.AddRange(response);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error from Provider Register. {name} , Message: {ex.Message}");
                }
            }

            if (exactNames != null)
            {
                foreach (var exactName in exactNames)
                {
                    try
                    {
                        var response = await _providerRegisterApiClient.SearchOrgansiationByName(exactName, true);
                        if (response != null) results.AddRange(response);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error from Provider Register. Exact Name: {exactName} , Message: {ex.Message}");
                    }
                }
            }

            if (ukprn.HasValue)
            {
                try
                {
                    var ukprnResponse = await _providerRegisterApiClient.SearchOrgansiationByUkprn(ukprn.Value);
                    if (ukprnResponse != null) results.Add(ukprnResponse);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error from Provider Register. UKPRN: {ukprn.Value} , Message: {ex.Message}");
                }
            }

            return results.GroupBy(r => r.Ukprn).Select(group => group.First()).ToList();
        }

        private async Task<IEnumerable<OrganisationSearchResult>> GetReferenceDataResults(string name, IEnumerable<string> exactNames, int? ukprn)
        {
            var results = new List<OrganisationSearchResult>();

            if (!string.IsNullOrEmpty(name))
            {
                try
                {
                    var response = await _referenceDataApiClient.SearchOrgansiation(name, false);
                    if (response != null) results.AddRange(response);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error from Reference Data Api. Name: {name} , Message: {ex.Message}");
                }
            }

            if (exactNames != null)
            {
                foreach (var exactName in exactNames)
                {
                    try
                    {
                        var response = await _referenceDataApiClient.SearchOrgansiation(exactName, true);
                        if (response != null)
                        {
                            if (ukprn.HasValue)
                            {
                                // The results from this API don't currently return UKPRN
                                foreach (var r in response)
                                {
                                    r.Ukprn = ukprn;
                                }
                            }

                            results.AddRange(response);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error from Provider Register. Exact Name: {exactName} , Message: {ex.Message}");
                    }
                }
            }

            return results;
        }

        private IEnumerable<OrganisationSearchResult> Dedupe(IEnumerable<OrganisationSearchResult> organisations)
        {
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
        public async Task<IEnumerable<Types.OrganisationType>> GetOrganisationTypes()
        {
            IEnumerable<Types.OrganisationType> results = null;

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
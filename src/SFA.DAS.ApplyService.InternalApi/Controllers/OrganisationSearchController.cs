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
            _logger.LogInformation("Handling Organisation Search Request");
            if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm.Length < 2)
            {
                return new List<OrganisationSearchResult>();
            }

            if (IsValidEpaOrganisationId(searchTerm))
            {
                _logger.LogInformation($@"Searching Organisations based on EPAO ID: [{searchTerm}]");
                return await OrganisationSearchByEpao(searchTerm);
            }

            // NOTE: This is required because there are occasions where charity or company number can be interpreted as a ukprn
            var results = new List<OrganisationSearchResult>();
            if (IsValidUkprn(searchTerm, out var ukprn))
            {
                _logger.LogInformation($@"Searching Organisations based on UKPRN: [{searchTerm}]");
                var resultFromUkprn = await OrganisationSearchByUkprn(ukprn);
                if (resultFromUkprn != null) results.AddRange(resultFromUkprn);
            }

            _logger.LogInformation($@"Searching Organisations based on name or charity number or company number wildcard: [{searchTerm}]");
            var resultFromName = await OrganisationSearchByNameOrCharityNumberOrCompanyNumber(searchTerm);
            if (resultFromName != null) results.AddRange(resultFromName);

            return Dedupe(results);
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

        private async Task<IEnumerable<OrganisationSearchResult>> OrganisationSearchByNameOrCharityNumberOrCompanyNumber(string name)
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

                    if (int.TryParse(name, out int companyOrCharityNumber))
                    {
                        // NOTE: API requires leading zeroes in order to search company number or charity number
                        var response2 = await _referenceDataApiClient.SearchOrgansiation(companyOrCharityNumber.ToString("D8"), false);
                        if (response2 != null) results.AddRange(response2);
                    }
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
                        OrganisationReferenceId = string.Join(",", group.Select(g => g.Id).Where(Id => !string.IsNullOrWhiteSpace(Id)).Distinct()),
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
                        OrganisationReferenceId = string.Join(",", group.Select(g => g.OrganisationReferenceId).Where(Id => !string.IsNullOrWhiteSpace(Id)).Distinct()),
                        RoEPAOApproved = group.Select(g => g.RoEPAOApproved).FirstOrDefault(RoEPAOApproved => RoEPAOApproved != false),
                        RoATPApproved = group.Select(g => g.RoATPApproved).FirstOrDefault(RoATPApproved => RoATPApproved != false),
                        CompanyNumber = group.Select(g => g.CompanyNumber).FirstOrDefault(CompanyNumber => !string.IsNullOrWhiteSpace(CompanyNumber)),
                        CharityNumber = group.Select(g => g.CharityNumber).FirstOrDefault(CharityNumber => !string.IsNullOrWhiteSpace(CharityNumber))
                    }
                );

            var companyNumberMerge = ukprnMerge.GroupBy(org => new { filter = org.CompanyNumber != null ? org.CompanyNumber.PadLeft(8, '0') : org.Name.ToUpperInvariant() })
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
                        OrganisationReferenceId = string.Join(",", group.Select(g => g.OrganisationReferenceId).Where(Id => !string.IsNullOrWhiteSpace(Id)).Distinct()),
                        RoEPAOApproved = group.Select(g => g.RoEPAOApproved).FirstOrDefault(RoEPAOApproved => RoEPAOApproved != false),
                        RoATPApproved = group.Select(g => g.RoATPApproved).FirstOrDefault(RoATPApproved => RoATPApproved != false),
                        CompanyNumber = group.Select(g => g.CompanyNumber).FirstOrDefault(CompanyNumber => !string.IsNullOrWhiteSpace(CompanyNumber)),
                        CharityNumber = group.Select(g => g.CharityNumber).FirstOrDefault(CharityNumber => !string.IsNullOrWhiteSpace(CharityNumber))
                    }
                );

            var charityNumberMerge = companyNumberMerge.GroupBy(org => new { filter = org.CharityNumber != null ? org.CharityNumber.PadLeft(8, '0') : org.Name.ToUpperInvariant() })
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
                        OrganisationReferenceId = string.Join(",", group.Select(g => g.OrganisationReferenceId).Where(Id => !string.IsNullOrWhiteSpace(Id)).Distinct()),
                        RoEPAOApproved = group.Select(g => g.RoEPAOApproved).FirstOrDefault(RoEPAOApproved => RoEPAOApproved != false),
                        RoATPApproved = group.Select(g => g.RoATPApproved).FirstOrDefault(RoATPApproved => RoATPApproved != false),
                        CompanyNumber = group.Select(g => g.CompanyNumber).FirstOrDefault(CompanyNumber => !string.IsNullOrWhiteSpace(CompanyNumber)),
                        CharityNumber = group.Select(g => g.CharityNumber).FirstOrDefault(CharityNumber => !string.IsNullOrWhiteSpace(CharityNumber))
                    }
                );

            return charityNumberMerge.OrderByDescending(org => org.Ukprn).ToList();
        }

        [HttpGet("OrganisationSearch/email")]
        public async Task<OrganisationSearchResult> GetOrganisationByEmail(string email)
        {
            
            _logger.LogInformation($"GetOrganisationByEmail({email})");
            
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

            if (result == null)
            {
                _logger.LogInformation($"GetOrganisationByEmail({email}) result : null");
            }
            else
            {
                _logger.LogInformation($"GetOrganisationByEmail({email}) result : {result.Name}");
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
using AutoMapper;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.InternalApi.Models.CompaniesHouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.InternalApi.Types.CompaniesHouse;
using SFA.DAS.ApplyService.InternalApi.Types;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    /// <summary>
    /// Companies House API docs are located at: https://developer.companieshouse.gov.uk/api/docs/index.html
    /// There is a Web-Friendly version located at: https://beta.companieshouse.gov.uk/
    /// </summary>
    public class CompaniesHouseApiClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<CompaniesHouseApiClient> _logger;
        private readonly IApplyConfig _config;

        public CompaniesHouseApiClient(HttpClient client, ILogger<CompaniesHouseApiClient> logger, IConfigurationService configurationService)
        {
            _client = client;
            _logger = logger;
            _config = configurationService.GetConfig().Result;
        }

        public async Task<ApiResponse<Company>> GetCompany(string companyNumber)
        {
            var company = await GetCompanyDetails(companyNumber);

            if (company.Success && company.Response != null)
            {
                company.Response.Officers = await GetOfficers(companyNumber);
                company.Response.PeopleWithSignificantControl = await GetPeopleWithSignificantControl(companyNumber);
            }

            return company;
        }

        public async Task<bool> IsCompanyActivelyTrading(string companyNumber)
        {
            var isTrading = false;

            var company = await GetCompanyDetails(companyNumber);

            if (company != null)
            {
                isTrading = "active".Equals(company.Response.Status, StringComparison.InvariantCultureIgnoreCase) 
                            && company.Response.DissolvedOn == null && company.Response.IsLiquidated != true;
            }

            return isTrading;
        }

        #region HTTP Request Helpers
        private AuthenticationHeaderValue GetBasicAuthHeader()
        {
            var bytes = Encoding.ASCII.GetBytes($"{_config.CompaniesHouseApiAuthentication.ApiKey}:");
            var token = Convert.ToBase64String(bytes);
            return new AuthenticationHeaderValue("Basic", token);
        }

        private async Task<ApiResponse<T>> Get<T>(string uri) 
        {
            _client.DefaultRequestHeaders.Authorization = GetBasicAuthHeader();

            using (var responseMessage = await _client.GetAsync(new Uri(uri, UriKind.Relative)))
            {
                var response = new ApiResponse<T>
                {
                   Success = responseMessage.IsSuccessStatusCode
                };

                if (responseMessage.IsSuccessStatusCode)
                {
                    response.Response = await responseMessage.Content.ReadAsAsync<T>();
                }

                return await Task.FromResult(response);
            }
        }
        #endregion HTTP Request Helpers

        private async Task<ApiResponse<Company>> GetCompanyDetails(string companyNumber)
        {
            _logger.LogInformation($"Searching Companies House - Company Details. Company Number: {companyNumber}");
            var apiResponse = await Get<CompaniesHouseDetails>($"/company/{companyNumber}");
            var response = Mapper.Map<CompaniesHouseDetails, Company>(apiResponse.Response);
            return new ApiResponse<Company> { Success = apiResponse.Success, Response = response };
        }

        private async Task<IEnumerable<Types.CompaniesHouse.Officer>> GetOfficers(string companyNumber, bool activeOnly = true)
        {
            _logger.LogInformation($"Searching Companies House - Officers. Company Number: {companyNumber}");
            var apiResponse = await Get<OfficerList>($"/company/{companyNumber}/officers?items_per_page=100");

            var items = activeOnly ? apiResponse.Response.items?.Where(i => i.resigned_on is null) : apiResponse.Response.items;

            var officers = Mapper.Map<IEnumerable<Models.CompaniesHouse.Officer>, IEnumerable<Types.CompaniesHouse.Officer>> (items);

            foreach(var officer in officers)
            {
                officer.Disqualifications = await GetOfficerDisqualifications(officer.Id);
            }

            return officers;
        }

        private async Task<IEnumerable<Types.CompaniesHouse.Disqualification>> GetOfficerDisqualifications(string officerId)
        {
            _logger.LogInformation($"Searching Companies House - Natural Officer's Disqualifications. Officer Id: {officerId}");
            var apiResponseNatural = await Get<DisqualificationList>($"/disqualified-officers/natural/{officerId}");

            _logger.LogInformation($"Searching Companies House - Corporate Officer's Disqualifications. Officer Id: {officerId}");
            var apiResponseCorporate = await Get<DisqualificationList>($"/disqualified-officers/corporate/{officerId}");

            var disqualifications = new List<Models.CompaniesHouse.Disqualification>();

            if(apiResponseNatural?.Response.disqualifications != null)
            {
                disqualifications.AddRange(apiResponseNatural.Response.disqualifications);
            }
            if (apiResponseCorporate?.Response.disqualifications != null)
            {
                disqualifications.AddRange(apiResponseCorporate.Response.disqualifications);
            }

            return Mapper.Map<IEnumerable<Models.CompaniesHouse.Disqualification>, IEnumerable<Types.CompaniesHouse.Disqualification>> (disqualifications);
        }

        private async Task<IEnumerable<Types.CompaniesHouse.PersonWithSignificantControl>> GetPeopleWithSignificantControl(string companyNumber, bool activeOnly = true)
        {
            _logger.LogInformation($"Searching Companies House - People With Significant Control. Company Number: {companyNumber}");
            var apiResponse = await Get<PersonWithSignificantControlList>($"/company/{companyNumber}/persons-with-significant-control?items_per_page=100");

            var items = activeOnly ? apiResponse.Response.items?.Where(i => i.ceased_on is null) : apiResponse.Response.items;
            return Mapper.Map<IEnumerable<Models.CompaniesHouse.PersonWithSignificantControl>, IEnumerable<Types.CompaniesHouse.PersonWithSignificantControl>>(items);
        }

        private async Task<IEnumerable<dynamic>> GetCharges(string companyNumber)
        {
            _logger.LogInformation($"Searching Companies House - Charges. Company Number: {companyNumber}");
            var apiResponse = await Get<ChargeList>($"/company/{companyNumber}/charges");
            return apiResponse.Response.items;
            //return Mapper.Map<IEnumerable<Charge>, IEnumerable<Types.CompaniesHouse.Charge>> (apiResponse.items);
        }

        private async Task<dynamic> GetInsolvencyDetails(string companyNumber)
        {
            _logger.LogInformation($"Searching Companies House - Insolvency. Company Number: {companyNumber}");
            var apiResponse = await Get<InsolvencyDetails>($"/company/{companyNumber}/insolvency");
            return apiResponse;
            //return Mapper.Map<InsolvencyDetails, InsolvencyDetails> (apiResponse);
        }
    }
}

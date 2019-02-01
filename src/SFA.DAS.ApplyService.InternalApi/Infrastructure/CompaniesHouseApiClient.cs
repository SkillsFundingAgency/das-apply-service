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

        private AuthenticationHeaderValue GetBasicAuthHeader()
        {
            var bytes = Encoding.ASCII.GetBytes($"{_config.CompaniesHouseApiAuthentication.ApiKey}:");
            var token = Convert.ToBase64String(bytes);
            return new AuthenticationHeaderValue("Basic", token);
        }

        private async Task<T> Get<T>(string uri)
        {
            _client.DefaultRequestHeaders.Authorization = GetBasicAuthHeader();

            using (var response = await _client.GetAsync(new Uri(uri, UriKind.Relative)))
            {
                return await response.Content.ReadAsAsync<T>();
            }
        }

        public async Task<dynamic> GetCompanyDetails(string companyNumber)
        {
            _logger.LogInformation($"Searching Companies House - Company Details. Company Number: {companyNumber}");
            var apiResponse = await Get<CompanyDetails>($"/company/{companyNumber}");
            return apiResponse;
            //return Mapper.Map<CompanyDetails, Types.CompaniesHouse.CompanyDetails> (apiResponse);
        }

        public async Task<IEnumerable<dynamic>> GetOfficers(string companyNumber)
        {
            _logger.LogInformation($"Searching Companies House - Officers. Company Number: {companyNumber}");
            var apiResponse = await Get<OfficerList>($"/company/{companyNumber}/officers");
            return apiResponse.items;
            //return Mapper.Map<IEnumerable<Officer>, IEnumerable<Types.CompaniesHouse.Officer>> (apiResponse.items);
        }

        private async Task<IEnumerable<dynamic>> GetOfficerDisqualifications(string officerId)
        {
            // TODO: Figure out what the difference between these are
            _logger.LogInformation($"Searching Companies House - Natural Officer's Disqualifications. Officer Id: {officerId}");
            var apiResponse1 = await Get<IEnumerable<dynamic>>($"/disqualified-officers/natural/{officerId}");

            _logger.LogInformation($"Searching Companies House - Corporate Officer's Disqualifications. Officer Id: {officerId}");
            var apiResponse2 = await Get<IEnumerable<dynamic>>($"/disqualified-officers/corporate/{officerId}");

            return null;
        }

        public async Task<IEnumerable<dynamic>> GetPeopleWithSignficantControl(string companyNumber)
        {
            _logger.LogInformation($"Searching Companies House - People With Significant Control. Company Number: {companyNumber}");
            var apiResponse = await Get<IEnumerable<dynamic>>($"/company/{companyNumber}/persons-with-significant-control");
            return apiResponse;
            //return Mapper.Map<IEnumerable<PeopleWithSignficantControl>, IEnumerable<Types.CompaniesHouse.PeopleWithSignficantControl>> (apiResponse);
        }

        public async Task<IEnumerable<dynamic>> GetCharges(string companyNumber)
        {
            _logger.LogInformation($"Searching Companies House - Charges. Company Number: {companyNumber}");
            var apiResponse = await Get<ChargeList>($"/company/{companyNumber}/charges");
            return apiResponse.items;
            //return Mapper.Map<IEnumerable<Charge>, IEnumerable<Types.CompaniesHouse.Charge>> (apiResponse.items);
        }

        public async Task<dynamic> GetInsolvencyDetails(string companyNumber)
        {
            _logger.LogInformation($"Searching Companies House - Insolvency. Company Number: {companyNumber}");
            var apiResponse = await Get<InsolvencyDetails>($"/company/{companyNumber}/insolvency");
            return apiResponse;
            //return Mapper.Map<InsolvencyDetails, InsolvencyDetails> (apiResponse);
        }
    }
}

﻿using AutoMapper;
using CharityCommissionService;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Configuration;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;

    /// <summary>
    /// Charity Commission API docs are located at: http://apps.charitycommission.gov.uk/Showcharity/API/SearchCharitiesV1/Docs/DevGuideHome.aspx
    /// Charity Commission WSDL is located at: https://apps.charitycommission.gov.uk/Showcharity/API/SearchCharitiesV1/SearchCharitiesV1.asmx?WSDL
    /// There is a Web-Friendly version located at: http://beta.charitycommission.gov.uk/charity-search/
    /// </summary>
    public class CharityCommissionOuterApiClient
    {
        private const string _acceptHeaderName = "Accept";
        protected const string _contentType = "application/json";

        private readonly HttpClient _client;
        private readonly ILogger<CharityCommissionOuterApiClient> _logger;
        private readonly IApplyConfig _config;

        public CharityCommissionOuterApiClient()
        {
            // Constructor used for Mocking CharityCommissionOuterApiClient
        }


        public CharityCommissionOuterApiClient(HttpClient client, ILogger<CharityCommissionOuterApiClient> logger, IConfigurationService configurationService)
        {
            _client = client;
            _logger = logger;
            _config = configurationService.GetConfig().Result;

            if (!client.DefaultRequestHeaders.Contains(_acceptHeaderName))
            {
                client.DefaultRequestHeaders.Add(_acceptHeaderName, _contentType);
            }
        }

        public async virtual Task<Types.CharityCommission.Charity> GetCharity(int charityNumber)
        {
            try
            {
                return await GetCharityDetails(charityNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to retrieve details from Charity Commission API", ex);
                throw new ServiceUnavailableException("Unable to retrieve details from Charity Commission API");
            }
        }

        private async Task<Types.CharityCommission.Charity> GetCharityDetails(int charityNumber)
        {
            _logger.LogInformation($"Searching Charity Commission - Charity Details. Charity Number: {charityNumber}");
            var apiResponse = await _client.GetAsync($"Charities/{charityNumber}");
            var json = await apiResponse.Content.ReadAsStringAsync();
            var GetCharityByRegisteredCharityNumberResult = JsonConvert.DeserializeObject<Charity>(json);
            return Mapper.Map<Charity, Types.CharityCommission.Charity>(GetCharityByRegisteredCharityNumberResult);
        }
    }
}

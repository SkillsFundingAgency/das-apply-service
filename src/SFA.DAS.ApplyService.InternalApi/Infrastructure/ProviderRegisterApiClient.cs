using AutoMapper;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.InternalApi.Models.ProviderRegister;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    public class ProviderRegisterApiClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<ProviderRegisterApiClient> _logger;

        public ProviderRegisterApiClient(HttpClient client, ILogger<ProviderRegisterApiClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<IEnumerable<Types.OrganisationSearchResult>> SearchOrgansiationByName(string name, bool exactMatch)
        {
            _logger.LogInformation($"Searching Provider Register. Name: {name}");
            var apiResponse = await Get<IEnumerable<Provider>>($"/providers/search?keywords={name}");

            if (exactMatch)
            {
                apiResponse = apiResponse?.Where(r => r.ProviderName.Equals(name, StringComparison.InvariantCultureIgnoreCase)).AsEnumerable();
            }

            return Mapper.Map<IEnumerable<Provider>, IEnumerable<Types.OrganisationSearchResult>>(apiResponse);
        }

        public async Task<Types.OrganisationSearchResult> SearchOrgansiationByUkprn(int ukprn)
        {
            _logger.LogInformation($"Searching Provider Register. Ukprn: {ukprn}");
            var apiResponse = await Get<Provider>($"/providers/{ukprn}");

            return Mapper.Map<Provider, Types.OrganisationSearchResult>(apiResponse);
        }

        private async Task<T> Get<T>(string uri)
        {
            var result = default(T);
            try
            {
                using (var response = await _client.GetAsync(new Uri(uri, UriKind.Relative)))
                {
                    try
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            result = await response.Content.ReadAsAsync<T>();
                        }
                        else
                        {
                            _logger.LogWarning($"GET: HTTP {(int)response.StatusCode} response from: {uri}");
                        }
                    }
                    catch (Exception ex)
                    {
                        var actualResponse = string.Empty;
                        try
                        {
                            actualResponse = await response.Content.ReadAsStringAsync();
                        }
                        catch
                        {
                            // safe to ignore any errors
                        }
                        _logger.LogError(ex, $"GET: HTTP {(int)response.StatusCode} Error getting response from: {uri} - ActualResponse: {actualResponse}");
                        // NOTE: We're not throwing exception here. The upstream code expects it to return: default(T)
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"GET: HTTP Error when processing request to: {uri}");
            }

            return result;
        }
    }
}

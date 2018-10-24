using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System;
using AutoMapper;
using SFA.DAS.ApplyService.InternalApi.Models.ProviderRegister;
using SFA.DAS.ApplyService.Configuration;

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

        public async Task<IEnumerable<Types.Organisation>> SearchOrgansiation(string searchTerm)
        {
            //var ukprn  = await Get<object>($"/providers/{ukprn}");

            var apiResponse = await Get<IEnumerable<Provider>>($"/providers/search?keywords={searchTerm}");

            return Mapper.Map<IEnumerable<Provider>, IEnumerable<Types.Organisation>>(apiResponse);
        }

        private async Task<T> Get<T>(string uri)
        {
            using (var response = await _client.GetAsync(new Uri(uri, UriKind.Relative)))
            {
                return await response.Content.ReadAsAsync<T>();
            }
        }

    }
}

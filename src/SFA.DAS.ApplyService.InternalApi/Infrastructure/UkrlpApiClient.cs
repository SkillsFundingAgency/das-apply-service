using AutoMapper;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Configuration;
    using Microsoft.Extensions.Logging;
    using SFA.DAS.ApplyService.InternalApi.Models.Ukrlp;
    using System.Text;

    public class UkrlpApiClient : IUkrlpApiClient
    {
        private readonly ILogger<UkrlpApiClient> _logger;

        private IConfigurationService _config;

        private IApplyConfig _applyConfig;

        private HttpClient _httpClient;

        private IUkrlpSoapSerializer _serializer;

        public UkrlpApiClient(ILogger<UkrlpApiClient> logger, IConfigurationService config, HttpClient httpClient, IUkrlpSoapSerializer serializer)
        {
            _logger = logger;
            _config = config;
            _applyConfig = _config.GetConfig().Result;
            _httpClient = httpClient;
            _serializer = serializer;
        }

        public async Task<List<ProviderDetails>> GetTrainingProviderByUkprn(long ukprn)
        {
            // Due to a bug in .net core, we have to parse the SOAP XML from UKRLP by hand
            // If this ever gets fixed then look to remove this code and replace with 'Add connected service'
            // https://github.com/dotnet/wcf/issues/3228

            var request = _serializer.BuildUkrlpSoapRequest(ukprn, _applyConfig.UkrlpApiAuthentication.StakeholderId,
                _applyConfig.UkrlpApiAuthentication.QueryId);

            var requestMessage =
                new HttpRequestMessage(HttpMethod.Post, _applyConfig.UkrlpApiAuthentication.ApiBaseAddress)
                {
                    Content = new StringContent(request, Encoding.UTF8, "text/xml")
                };

            var responseMessage = await _httpClient.SendAsync(requestMessage);
                
            if (!responseMessage.IsSuccessStatusCode)
            {
                return await Task.FromResult(new List<ProviderDetails>());
            }

            string soapXml = await responseMessage.Content.ReadAsStringAsync();
            var matchingProviderRecords = _serializer.DeserialiseMatchingProviderRecordsResponse(soapXml);

            ProviderDetails providerDetails = null;

            if (matchingProviderRecords != null)
            {
                providerDetails =
                    Mapper.Map<ProviderDetails>(matchingProviderRecords);

                var result = new List<ProviderDetails>
                {
                    providerDetails
                };
                return await Task.FromResult(result);
            }
            else
            {
                return await Task.FromResult(new List<ProviderDetails>());
            }
        }
    }
}

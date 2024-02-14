using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    using Configuration;
    using Microsoft.Azure.Services.AppAuthentication;

    public class RoatpTokenService : IRoatpTokenService
    {
        private readonly IApplyConfig _configuration;

        public RoatpTokenService(IConfigurationService configurationService)
        {
            _configuration = configurationService.GetConfig().Result;
        }

        public async Task<string> GetToken(Uri baseUri)
        {
            if (baseUri != null && baseUri.IsLoopback)
                return string.Empty;

            return await new AzureServiceTokenProvider().GetAccessTokenAsync(_configuration.RoatpApiAuthentication.Identifier);
        }
    }
}

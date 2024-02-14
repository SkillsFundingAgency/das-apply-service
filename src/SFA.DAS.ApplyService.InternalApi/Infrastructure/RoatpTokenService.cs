using System;
using System.Threading.Tasks;
using Azure.Identity;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    using Azure.Core;
    using Configuration;

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

            var defaultAzureCredential = new DefaultAzureCredential();
            var result = await defaultAzureCredential.GetTokenAsync(
                new TokenRequestContext(scopes: new string[] { _configuration.RoatpApiAuthentication.Identifier + "/.default" }) { });

            return result.Token;
        }
    }
}

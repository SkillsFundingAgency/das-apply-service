using System;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    using Configuration;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    public class RoatpTokenService : IRoatpTokenService
    {
        private readonly IApplyConfig _configuration;

        public RoatpTokenService(IConfigurationService configurationService)
        {
            _configuration = configurationService.GetConfig().Result;
        }

        public string GetToken(Uri baseUri)
        {
            if (baseUri != null && baseUri.IsLoopback)
                return string.Empty;

            var tenantId = _configuration.RoatpApiAuthentication.TenantId;
            var clientId = _configuration.RoatpApiAuthentication.ClientId;
            var appKey = _configuration.RoatpApiAuthentication.ClientSecret;
            var resourceId = _configuration.RoatpApiAuthentication.ResourceId;

            var authority = $"https://login.microsoftonline.com/{tenantId}";
            var clientCredential = new ClientCredential(clientId, appKey);
            var context = new AuthenticationContext(authority, true);
            var result = context.AcquireTokenAsync(resourceId, clientCredential).Result;

            return result.AccessToken;
        }
    }
}

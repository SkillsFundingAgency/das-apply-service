using System;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    using Configuration;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    public class RoatpTokenService : IRoatpTokenService
    {
        private readonly IApplyConfig _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public RoatpTokenService(IConfigurationService configurationService, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configurationService.GetConfig().Result;
            _hostingEnvironment = hostingEnvironment;
        }

        public string GetToken(string baseUrl)
        {
            var uri = new Uri(baseUrl);
            if (uri.IsLoopback)
            {
                return string.Empty;
            }

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

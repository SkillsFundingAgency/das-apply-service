
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using SFA.DAS.ApplyService.Configuration;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    public class QnaTokenService : InternalApi.Infrastructure.IQnaTokenService
    {
        private readonly IConfigurationService _configurationService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IApplyConfig _configuration;

        public QnaTokenService(IConfigurationService configurationService, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _configurationService = configurationService;
            _configuration = configurationService.GetConfig().Result;
        }

        public string GetToken()
        {
            if (_hostingEnvironment.IsDevelopment())
                return string.Empty;

            var tenantId = _configuration.QnaApiAuthentication.TenantId;
            var clientId = _configuration.QnaApiAuthentication.ClientId;
            var appKey = _configuration.QnaApiAuthentication.ClientSecret;
            var resourceId = _configuration.QnaApiAuthentication.ResourceId;

            var authority = $"https://login.microsoftonline.com/{tenantId}";
            var clientCredential = new ClientCredential(clientId, appKey);
            var context = new AuthenticationContext(authority, true);
            var result = context.AcquireTokenAsync(resourceId, clientCredential).Result;

            return result.AccessToken;
        }
    }
}

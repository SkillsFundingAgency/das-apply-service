using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using SFA.DAS.ApplyService.Configuration;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    public class TokenService : ITokenService
    {
        private readonly IApplyConfig _config;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IHostingEnvironment hostingEnvironment, ILogger<TokenService> logger, IConfigurationService configurationService)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _config = configurationService.GetConfig().Result;
        }

        public string GetToken()
        {
            if (_hostingEnvironment.IsDevelopment())
                return string.Empty;
            
            var tenantId = _config.InternalApi.TenantId;
            var clientId = _config.InternalApi.ClientId;
            var appKey = _config.InternalApi.ClientSecret;
            var resourceId = _config.InternalApi.ResourceId;

            var authority = $"https://login.microsoftonline.com/{tenantId}";
            var clientCredential = new ClientCredential(clientId, appKey);
            var context = new AuthenticationContext(authority, true);
            var result = context.AcquireTokenAsync(resourceId, clientCredential).Result;
                
            return result.AccessToken;
        }
    }

    public interface ITokenService
    {
        string GetToken();
    }
}
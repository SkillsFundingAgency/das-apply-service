using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using SFA.DAS.ApplyService.Configuration;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class TokenService : ITokenService
    {
        private readonly IApplyConfig _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IApplyConfig configuration, IHostingEnvironment hostingEnvironment, ILogger<TokenService> logger)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public string GetToken()
        {
            if (_hostingEnvironment.IsDevelopment())
                return string.Empty;

            _logger.LogInformation("Inside GetToken.");
            if (_configuration == null)
            {
                _logger.LogInformation("_configuration is null");    
            }
            else
            {
                _logger.LogInformation("_configuration is not null");
            }

            if (_configuration.InternalApi == null)
            {
                _logger.LogInformation("_configuration.InternalApi is null");
            }
            
            if (_configuration.InternalApi == null)
            {
                _logger.LogInformation("_configuration.InternalApi is null");
            }
            
            var tenantId = _configuration.InternalApi.TenantId;
            var clientId = _configuration.InternalApi.ClientId;
            var appKey = _configuration.InternalApi.ClientSecret;
            var resourceId = _configuration.InternalApi.ResourceId;

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
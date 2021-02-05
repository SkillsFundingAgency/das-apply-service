using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using SFA.DAS.ApplyService.Configuration;

namespace SFA.DAS.ApplyService.EmailService.Infrastructure
{
    public class EmailTokenService : IEmailTokenService
    {
        private readonly IApplyConfig _config;
        private readonly IHostingEnvironment _hostingEnvironment;

        public EmailTokenService(IHostingEnvironment hostingEnvironment, IConfigurationService configurationService)
        {
            _hostingEnvironment = hostingEnvironment;
            _config = configurationService.GetConfig().GetAwaiter().GetResult();
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
            var result = context.AcquireTokenAsync(resourceId, clientCredential).GetAwaiter().GetResult();

            return result.AccessToken;
        }
    }

    public interface IEmailTokenService
    {
        string GetToken();
    }
}

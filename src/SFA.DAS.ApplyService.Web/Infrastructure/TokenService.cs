using Microsoft.Azure.Services.AppAuthentication;
using SFA.DAS.ApplyService.Configuration;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class TokenService : ITokenService
    {
        private readonly IConfigurationService _configurationService;

        public TokenService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public string GetToken()
        {
            var configuration = _configurationService.GetConfig().GetAwaiter().GetResult();
            
            if (string.IsNullOrEmpty(configuration.InternalApi.Identifier))
                return string.Empty;

            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var generateTokenTask = azureServiceTokenProvider.GetAccessTokenAsync(configuration.InternalApi.Identifier);

            return generateTokenTask.GetAwaiter().GetResult();
        }
    }

    public interface ITokenService
    {
        string GetToken();
    }
}
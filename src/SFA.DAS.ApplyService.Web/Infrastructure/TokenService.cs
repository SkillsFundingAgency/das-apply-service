using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Services.AppAuthentication;
using SFA.DAS.ApplyService.Configuration;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class TokenService : ITokenService
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfigurationService _configurationService;

        public TokenService(IHostingEnvironment hostingEnvironment, IConfigurationService configurationService)
        {
            _hostingEnvironment = hostingEnvironment;
            _configurationService = configurationService;
        }

        public string GetToken()
        {
            if (_hostingEnvironment.IsDevelopment())
                return string.Empty;

            var configuration = _configurationService.GetConfig().GetAwaiter().GetResult();

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
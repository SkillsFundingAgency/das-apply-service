using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Hosting;
using SFA.DAS.ApplyService.Configuration;

namespace SFA.DAS.ApplyService.EmailService.Infrastructure
{
    public class EmailTokenService : IEmailTokenService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfigurationService _configurationService;

        public EmailTokenService(IConfigurationService configurationService, IWebHostEnvironment hostingEnvironment)
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

    public interface IEmailTokenService
    {
        string GetToken();
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Services.AppAuthentication;
using SFA.DAS.ApplyService.Configuration;

namespace SFA.DAS.ApplyService.EmailService.Infrastructure
{
    public class EmailTokenService : IEmailTokenService
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfigurationService _configurationService;

        public EmailTokenService(IConfigurationService configurationService, IHostingEnvironment hostingEnvironment)
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
            var generateTokenTask = azureServiceTokenProvider.GetAccessTokenAsync(configuration.QnaApiAuthentication.Identifier);

            return generateTokenTask.GetAwaiter().GetResult();
        }
    }

    public interface IEmailTokenService
    {
        string GetToken();
    }
}

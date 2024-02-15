using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    using Configuration;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Azure.Services.AppAuthentication;
    using Microsoft.Extensions.Hosting;

    public class RoatpTokenService : IRoatpTokenService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfigurationService _configurationService;

        public RoatpTokenService(IWebHostEnvironment hostingEnvironment, IConfigurationService configurationService)
        {
            _hostingEnvironment = hostingEnvironment;
            _configurationService = configurationService;
        }

        public async Task<string> GetToken()
        {
            if (_hostingEnvironment.IsDevelopment())
                return string.Empty;

            var configuration = await _configurationService.GetConfig();

            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var generatedToken = await azureServiceTokenProvider.GetAccessTokenAsync(configuration.QnaApiAuthentication.Identifier);

            return generatedToken;
        }
    }
}

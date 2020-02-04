using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.EmailService;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{

    public class EmailTemplateClient : IEmailTemplateClient
    {
        private readonly ILogger<EmailTemplateClient> _logger;
        private readonly ITokenService _tokenService;
        private static readonly HttpClient _httpClient = new HttpClient();

        public EmailTemplateClient(IConfigurationService configurationService, ILogger<EmailTemplateClient> logger, ITokenService tokenService)
        {
            _logger = logger;
            _tokenService = tokenService;
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(configurationService.GetConfig().Result.InternalApi.Uri);
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenService.GetToken());
        }

        public async Task<EmailTemplate> GetEmailTemplate(string templateName)
        {
            return await (await _httpClient.GetAsync($"/emailTemplates/{templateName}")).Content
                .ReadAsAsync<EmailTemplate> ();
        }
        
    }
}
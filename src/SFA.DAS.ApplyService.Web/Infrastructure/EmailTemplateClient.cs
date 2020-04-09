using Microsoft.Extensions.Logging;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.EmailService;
using SFA.DAS.ApplyService.Infrastructure.ApiClients;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class EmailTemplateClient : ApiClientBase<EmailTemplateClient>, IEmailTemplateClient
    {
        public EmailTemplateClient(ILogger<EmailTemplateClient> logger, IConfigurationService configurationService, ITokenService tokenService) : base(logger)
        {
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(configurationService.GetConfig().Result.InternalApi.Uri);
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.GetToken());
        }

        public async Task<EmailTemplate> GetEmailTemplate(string templateName)
        {
            return await Get<EmailTemplate>($"/emailTemplates/{templateName}");
        }
        
    }
}
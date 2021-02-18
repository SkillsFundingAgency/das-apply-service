using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.EmailService.Interfaces;
using SFA.DAS.ApplyService.Infrastructure.ApiClients;

namespace SFA.DAS.ApplyService.EmailService.Infrastructure
{
    public class EmailTemplateClient : ApiClientBase<EmailTemplateClient>, IEmailTemplateClient
    {
        public EmailTemplateClient(HttpClient httpClient, ILogger<EmailTemplateClient> logger, IEmailTokenService tokenService) : base(httpClient, logger)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.GetToken());
        }

        public async Task<EmailTemplate> GetEmailTemplate(string templateName)
        {
            return await Get<EmailTemplate>($"/emailTemplates/{templateName}");
        }

    }
}

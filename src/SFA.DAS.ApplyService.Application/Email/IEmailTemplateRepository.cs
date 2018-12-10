using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Email
{
    public interface IEmailTemplateRepository
    {
        Task<EmailTemplate> GetEmailTemplate(string templateName);
    }
}

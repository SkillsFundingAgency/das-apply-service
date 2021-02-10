using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Domain.Interfaces
{
    public interface IEmailTemplateRepository
    {
        Task<EmailTemplate> GetEmailTemplate(string templateName);
    }
}

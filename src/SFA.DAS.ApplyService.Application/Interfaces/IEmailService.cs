using SFA.DAS.ApplyService.Domain.Entities;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Interfaces
{
    [System.Obsolete("Email Service has been replaced with NotificationApiEmailService")]
    public interface IEmailService
    {
        Task SendEmail(string templateName, string toAddress, dynamic tokens);

        Task SendEmailToContact(string templateName, Contact contact, dynamic tokens);
    }
}
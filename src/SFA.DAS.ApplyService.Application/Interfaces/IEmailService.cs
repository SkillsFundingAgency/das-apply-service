using SFA.DAS.ApplyService.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string templateName, string toAddress, dynamic tokens);

        Task SendEmailToContact(string templateName, Contact contact, dynamic tokens);

        Task SendEmailToContacts(string templateName, IEnumerable<Contact> contacts, dynamic tokens);
    }
}
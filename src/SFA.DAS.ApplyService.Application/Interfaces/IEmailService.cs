using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string templateName, string toAddress, dynamic replacements);
    }
}
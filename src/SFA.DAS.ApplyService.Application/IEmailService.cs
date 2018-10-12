using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application
{
    public interface IEmailService
    {
        Task SendEmail(string ToAddress, int emailId, object replacements);
    }
}
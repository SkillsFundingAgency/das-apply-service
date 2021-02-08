using SFA.DAS.ApplyService.Domain.Roatp;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.EmailService.Interfaces
{
    public interface IGetHelpWithQuestionEmailService
    {
        Task SendGetHelpWithQuestionEmail(GetHelpWithQuestion getHelpWithQuestion);
    }
}

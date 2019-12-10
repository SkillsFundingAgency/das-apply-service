using SFA.DAS.ApplyService.Domain.Roatp;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.EmailService
{
    public interface ISubmitApplicationConfirmationEmailService
    {
        Task SendGetHelpWithQuestionEmail(ApplicationSubmitConfirmation applicationSubmitConfirmation);
    }
}

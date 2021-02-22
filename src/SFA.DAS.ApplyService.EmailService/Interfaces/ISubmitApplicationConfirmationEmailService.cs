using SFA.DAS.ApplyService.Domain.Roatp;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.EmailService.Interfaces
{
    public interface ISubmitApplicationConfirmationEmailService
    {
        Task SendSubmitConfirmationEmail(ApplicationSubmitConfirmation applicationSubmitConfirmation);
    }
}

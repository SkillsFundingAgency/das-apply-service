using SFA.DAS.ApplyService.Domain.Roatp;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Email
{
    public interface IGetHelpWithQuestionEmailService
    {
        Task SendGetHelpWithQuestionEmail(GetHelpWithQuestion getHelpWithQuestion);
    }
}

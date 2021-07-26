using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.EmailService.Interfaces
{
    public interface IApplicationUpdatedEmailService
    {
        Task SendEmail(Guid applicationId);
    }
}

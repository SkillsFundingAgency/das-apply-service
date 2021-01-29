using SFA.DAS.ApplyService.Application.Apply.Oversight;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Services
{
    public interface IRegistrationDetailsService
    {
        Task<RoatpRegistrationDetails> GetRegistrationDetails(Guid applicationId);
    }
}

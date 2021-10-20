using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Services
{
    public interface IReapplicationCheckService
    {
        Task<bool> ReapplicationAllowed(Guid signInId, Guid? organisationId);
        Task<bool> ReapplicationRequestedAndPending(Guid signInId, Guid? organisationId);
        Task<string> ReapplicationUkprnForUser(Guid signInId);
        Task<Guid?> ReapplicationApplicationIdForUser(Guid signInId);

        Task<bool> ApplicationInFlightWithDifferentUser(Guid signInId, string ukprn);
    }
}

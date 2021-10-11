using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Services
{
    public interface IReapplicationCheckService
    {
        Task<bool> ReapplicationAllowed(Guid signInId, Guid? organisationId);
        Task<string> ReapplicationUkprnForUser(Guid signInId);

        Task<string> ReapplicationUkprnPresent(Guid signInId);
    }
}

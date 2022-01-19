using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Web.Infrastructure.Interfaces
{
    public interface IRefreshTrusteesService
    {
        Task<RefreshTrusteeResult> RefreshTrustees(Guid applicationId, Guid userId);
    }
}

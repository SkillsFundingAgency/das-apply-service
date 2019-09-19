using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Apply.Roatp;

namespace SFA.DAS.ApplyService.Web.Infrastructure.Interfaces
{
    public interface IGetCurrentApplicationDetailsService
    {
        Task<int> GetProviderTypeId(Guid applicationId);
    }
}

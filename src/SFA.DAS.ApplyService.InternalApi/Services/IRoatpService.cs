using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Roatp;

namespace SFA.DAS.ApplyService.InternalApi.Services;

public interface IRoatpService
{
    Task<OrganisationRegisterStatus> GetRegisterStatus(int ukprn);
}

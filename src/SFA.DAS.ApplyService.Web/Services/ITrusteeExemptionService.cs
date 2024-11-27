using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Services;

public interface ITrusteeExemptionService
{
    Task<bool> IsProviderExempted(string ukprn);
}
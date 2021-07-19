using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public interface IAllowedProvidersApiClient
    {
        Task<bool> CanUkprnStartApplication(int ukprn);
    }
}

using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public interface IAllowedProvidersApiClient
    {
        Task<bool> IsUkprnOnAllowedList(int ukprn);
    }
}

using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Domain.Interfaces
{
    public interface IAllowedProvidersRepository
    {
        Task<bool> IsUkprnOnAllowedProvidersList(int ukprn);
    }
}

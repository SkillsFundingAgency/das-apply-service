using SFA.DAS.ApplyService.InternalApi.Types.CharityCommission;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    public interface IOuterApiClient
    {
        Task<Charity> GetCharity(int charityNumber);
    }
}
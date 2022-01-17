using System.Threading.Tasks;
using SFA.DAS.ApplyService.InternalApi.Types.CharityCommission;

namespace SFA.DAS.ApplyService.Infrastructure.ApiClients
{
    public interface IOuterApiClient
    {
        Task<Charity> GetCharityDetails(int charityNumber);
    }
}

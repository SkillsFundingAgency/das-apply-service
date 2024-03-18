using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    public interface IRoatpTokenService
    {
        Task<string> GetToken();
    }
}

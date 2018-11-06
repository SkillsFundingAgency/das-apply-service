using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Interfaces
{
    public interface IKeyProvider
    {
        Task<string> GetKey();
    }
}
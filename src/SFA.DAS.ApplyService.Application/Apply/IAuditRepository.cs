using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Audit;

namespace SFA.DAS.ApplyService.Application.Apply
{
    public interface IAuditRepository
    {
        Task Add(Audit audit);
    }
}

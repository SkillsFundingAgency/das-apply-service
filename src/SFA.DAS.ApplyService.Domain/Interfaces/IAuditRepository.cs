using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Domain.Interfaces
{
    public interface IAuditRepository
    {
        Task Add(Audit.Audit audit);
    }
}

using SFA.DAS.ApplyService.Domain.Audit;

namespace SFA.DAS.ApplyService.Application.Interfaces
{
    public interface IAuditService
    {
        void StartTracking(UserAction userAction, string userId, string userName);
        void AuditInsert(IAuditable trackedObject);
        void AuditUpdate(IAuditable trackedObject);
        void AuditDelete(IAuditable trackedObject);
        void Save();
    }
}

namespace SFA.DAS.ApplyService.Domain.Interfaces
{
    public interface IAuditRepository
    {
        void Add(Audit.Audit audit);
    }
}

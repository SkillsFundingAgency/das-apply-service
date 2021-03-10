using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Domain.Interfaces
{
    public interface IAppealRepository
    {
        void Add(Appeal entity);
    }
}

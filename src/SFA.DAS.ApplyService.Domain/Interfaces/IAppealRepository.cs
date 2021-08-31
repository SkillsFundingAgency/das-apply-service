using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Domain.Interfaces
{
    public interface IAppealRepository
    {
        void Add(Appeal entity);
        void Update(Appeal entity);
        void Remove(Guid entityId);

        Task<Appeal> GetByApplicationId(Guid applicationId);
    }
}

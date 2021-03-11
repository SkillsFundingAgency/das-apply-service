using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Domain.Interfaces
{
    public interface IAppealRepository
    {
        void Add(Appeal entity);
        Task<Appeal> GetByOversightReviewId(Guid oversightReviewId);
    }
}

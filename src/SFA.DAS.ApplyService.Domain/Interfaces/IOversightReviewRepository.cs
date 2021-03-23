using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Domain.Interfaces
{
    public interface IOversightReviewRepository
    {
        Task<OversightReview> GetByApplicationId(Guid applicationId);
        Task<OversightReview> GetById(Guid entityId);
        void Add(OversightReview entity);
        void Update(OversightReview entity);
    }
}

using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Interfaces
{
    public interface IOversightReviewRepository
    {
        Task<OversightReview> GetByApplicationId(Guid applicationId);
        Task Add(OversightReview entity);
        Task Update(OversightReview entity);
    }
}

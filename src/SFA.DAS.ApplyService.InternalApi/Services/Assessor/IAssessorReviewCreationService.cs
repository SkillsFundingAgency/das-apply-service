using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Services.Assessor
{
    public interface IAssessorReviewCreationService
    {
        Task CreateEmptyReview(Guid applicationId, string assessorUserId, int assessorNumber);
    }
}
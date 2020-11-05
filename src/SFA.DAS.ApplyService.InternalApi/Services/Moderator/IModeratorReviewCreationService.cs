using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Services.Moderator
{
    public interface IModeratorReviewCreationService
    {
        Task CreateEmptyReview(Guid applicationId, string moderatorUserId, string moderatorUserName);
    }
}

using MediatR;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply.Moderator
{
    public class GetAllModeratorPageReviewOutcomesRequest : IRequest<List<ModeratorPageReviewOutcome>>
    {
        public GetAllModeratorPageReviewOutcomesRequest(Guid applicationId, string userId)
        {
            ApplicationId = applicationId;
            UserId = userId;
        }

        public Guid ApplicationId { get; }
        public string UserId { get; }
    }
}

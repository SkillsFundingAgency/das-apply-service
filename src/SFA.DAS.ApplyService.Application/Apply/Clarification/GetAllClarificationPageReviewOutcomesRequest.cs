using MediatR;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply.Clarification
{
    public class GetAllClarificationPageReviewOutcomesRequest : IRequest<List<ClarificationPageReviewOutcome>>
    {
        public GetAllClarificationPageReviewOutcomesRequest(Guid applicationId, string userId)
        {
            ApplicationId = applicationId;
            UserId = userId;
        }

        public Guid ApplicationId { get; }
        public string UserId { get; }
    }
}

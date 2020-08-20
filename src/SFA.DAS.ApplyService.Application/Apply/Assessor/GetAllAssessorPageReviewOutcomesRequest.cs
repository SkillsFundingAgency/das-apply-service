using MediatR;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class GetAllAssessorPageReviewOutcomesRequest : IRequest<List<AssessorPageReviewOutcome>>
    {
        public GetAllAssessorPageReviewOutcomesRequest(Guid applicationId, string userId)
        {
            ApplicationId = applicationId;
            UserId = userId;
        }

        public Guid ApplicationId { get; }
        public string UserId { get; }
    }
}

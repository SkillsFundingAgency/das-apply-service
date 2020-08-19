using MediatR;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class GetAllAssessorReviewOutcomesRequest : IRequest<List<AssessorPageReviewOutcome>>
    {
        public GetAllAssessorReviewOutcomesRequest(Guid applicationId,
                                                        int assessorType,
                                                        string userId)
        {
            ApplicationId = applicationId;
            AssessorType = assessorType;
            UserId = userId;
        }

        public Guid ApplicationId { get; }
        public int AssessorType { get; }
        public string UserId { get; }
    }
}

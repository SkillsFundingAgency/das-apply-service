using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class GetAllAssessorReviewOutcomesRequest : IRequest<List<PageReviewOutcome>>
    {
        public GetAllAssessorReviewOutcomesRequest()
        {

        }

        public GetAllAssessorReviewOutcomesRequest(Guid applicationId,
                                                           int assessorType,
                                                        string userId)
        {
            ApplicationId = applicationId;
            AssessorType = assessorType;
            UserId = userId;
        }

        public Guid ApplicationId { get; set; }
        public int AssessorType { get; set; }
        public string UserId { get; set; }
    }
}

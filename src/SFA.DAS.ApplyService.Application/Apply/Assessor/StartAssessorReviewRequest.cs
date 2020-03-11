using MediatR;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class StartAssessorReviewRequest : IRequest<bool>
    {
        public Guid ApplicationId { get; set; }

        public string Reviewer { get; set; }

        public StartAssessorReviewRequest(Guid applicationId, string reviewer)
        {
            ApplicationId = applicationId;
            Reviewer = reviewer;
        }
    }
}

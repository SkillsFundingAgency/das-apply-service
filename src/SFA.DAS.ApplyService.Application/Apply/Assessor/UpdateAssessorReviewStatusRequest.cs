using MediatR;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class UpdateAssessorReviewStatusRequest : IRequest
    {
        public UpdateAssessorReviewStatusRequest(Guid applicationId, int assessorType, string userId, string status)
        {
            ApplicationId = applicationId;
            AssessorType = assessorType;
            UserId = userId;
            Status = status;
        }

        public Guid ApplicationId { get; }
        public int AssessorType { get; }
        public string UserId { get; }
        public string Status { get; }
    }
}

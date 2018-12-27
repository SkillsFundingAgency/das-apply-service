using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Review
{
    public class StartApplicationReviewRequest : IRequest
    {
        public Guid ApplicationId { get; }
        public int SequenceId { get; }

        public StartApplicationReviewRequest(Guid applicationId, int sequenceId)
        {
            ApplicationId = applicationId;
            SequenceId = sequenceId;
        }
    }
}
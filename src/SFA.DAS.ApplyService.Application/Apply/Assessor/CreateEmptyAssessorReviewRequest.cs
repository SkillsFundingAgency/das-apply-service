using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class CreateEmptyAssessorReviewRequest : IRequest
    {
        public Guid ApplicationId { get; }
        public string AssessorUserId { get; }
        public string AssessorUserName { get; }
        public List<AssessorPageReviewOutcome> PageReviewOutcomes { get; }

        public CreateEmptyAssessorReviewRequest(Guid applicationId, string assessorUserId, string assessorUserName, List<AssessorPageReviewOutcome> pageReviewOutcomes)
        {
            ApplicationId = applicationId;
            AssessorUserId = assessorUserId;
            AssessorUserName = assessorUserName;
            PageReviewOutcomes = pageReviewOutcomes;
        }
    }
}

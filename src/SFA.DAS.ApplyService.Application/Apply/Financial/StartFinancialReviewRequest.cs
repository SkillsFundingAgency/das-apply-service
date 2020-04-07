using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Financial
{
    public class StartFinancialReviewRequest : IRequest<bool>
    {
        public Guid ApplicationId { get; }
        public string Reviewer { get; set; }

        public StartFinancialReviewRequest(Guid applicationId, string reviewer)
        {
            ApplicationId = applicationId;
            Reviewer = reviewer;
        }
    }
}
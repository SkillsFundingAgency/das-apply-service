using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Financial
{
    public class StartFinancialReviewRequest : IRequest
    {
        public Guid ApplicationId { get; }

        public StartFinancialReviewRequest(Guid applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}
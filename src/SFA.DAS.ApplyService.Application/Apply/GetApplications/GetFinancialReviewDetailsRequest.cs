using System;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.GetApplications
{
    public class GetFinancialReviewDetailsRequest : IRequest<FinancialReviewDetails>
    {
        public Guid ApplicationId { get; }

        public GetFinancialReviewDetailsRequest(Guid applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}
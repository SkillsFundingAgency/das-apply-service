using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Financial
{
    public class RecordGradeRequest : IRequest<bool>
    {
        public Guid ApplicationId { get; set; }
        public FinancialReviewDetails FinancialReviewDetails { get; set; }
        public RecordGradeRequest(Guid applicationId, FinancialReviewDetails financialReviewDetails)
        {
            ApplicationId = applicationId;
            FinancialReviewDetails = financialReviewDetails;
        }
    }
}

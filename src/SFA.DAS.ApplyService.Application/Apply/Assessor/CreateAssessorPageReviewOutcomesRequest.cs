using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class CreateAssessorPageReviewOutcomesRequest : IRequest
    {
        public List<AssessorPageReviewOutcome> AssessorPageReviewOutcomes { get; set; }
    }
}

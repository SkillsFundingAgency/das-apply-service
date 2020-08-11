using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class AssessorSummaryRequest : IRequest<RoatpAssessorSummary>
    {
        public AssessorSummaryRequest(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
    }
}

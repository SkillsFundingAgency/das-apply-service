using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class AssessorApplicationCountsRequest : IRequest<RoatpAssessorApplicationCounts>
    {
        public AssessorApplicationCountsRequest(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
    }
}

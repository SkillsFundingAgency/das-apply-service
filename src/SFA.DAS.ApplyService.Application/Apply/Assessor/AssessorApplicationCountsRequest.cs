using MediatR;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class AssessorApplicationCountsRequest : IRequest<AssessorApplicationCounts>
    {
        public AssessorApplicationCountsRequest(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
    }
}

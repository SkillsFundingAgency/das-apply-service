using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class InProgressAssessorApplicationsRequest : IRequest<List<RoatpAssessorApplicationSummary>>
    {
        public InProgressAssessorApplicationsRequest(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
    }
}

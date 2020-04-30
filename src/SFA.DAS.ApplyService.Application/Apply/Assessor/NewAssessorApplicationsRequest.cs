using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class NewAssessorApplicationsRequest : IRequest<List<RoatpAssessorApplicationSummary>>
    {
        public NewAssessorApplicationsRequest(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
    }
}

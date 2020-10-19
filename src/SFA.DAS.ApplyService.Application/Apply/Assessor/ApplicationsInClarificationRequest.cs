using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class ApplicationsInClarificationRequest : IRequest<List<ClarificationApplicationSummary>>
    {
        public ApplicationsInClarificationRequest(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
    }
}

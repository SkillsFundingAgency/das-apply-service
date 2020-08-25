using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class ApplicationsInModerationRequest : IRequest<List<ModerationApplicationSummary>>
    {
        public ApplicationsInModerationRequest(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
    }
}

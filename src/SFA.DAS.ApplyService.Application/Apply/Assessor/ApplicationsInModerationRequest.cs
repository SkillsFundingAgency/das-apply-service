using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class ApplicationsInModerationRequest : IRequest<List<RoatpModerationApplicationSummary>>
    {
        public ApplicationsInModerationRequest()
        {
        }
    }
}

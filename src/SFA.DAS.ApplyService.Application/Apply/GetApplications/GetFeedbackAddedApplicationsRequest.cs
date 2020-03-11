using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply.GetApplications
{
    public class GetFeedbackAddedApplicationsRequest : IRequest<IEnumerable<RoatpApplicationSummaryItem>>
    {
    }
}

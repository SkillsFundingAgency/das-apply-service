using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply.Review.Applications
{
    public class FeedbackAddedApplicationsRequest : IRequest<List<ApplicationSummaryItem>>
    {
    }
}

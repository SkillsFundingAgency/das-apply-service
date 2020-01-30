using MediatR;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply.Financial.Applications
{
    public class FeedbackAddedFinancialApplicationsRequest : IRequest<List<Domain.Entities.Apply>>
    {
    }
}

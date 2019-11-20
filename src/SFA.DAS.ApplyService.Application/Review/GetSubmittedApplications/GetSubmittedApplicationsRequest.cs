using MediatR;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Review.GetSubmittedApplications
{
    public class GetSubmittedApplicationsRequest : IRequest<List<Domain.Entities.Application>>
    {
    }
}

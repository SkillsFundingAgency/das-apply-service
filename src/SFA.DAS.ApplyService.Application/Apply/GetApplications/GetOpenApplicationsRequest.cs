using MediatR;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply.GetApplications
{
    public class GetOpenApplicationsRequest : IRequest<IEnumerable<Domain.Entities.Apply>>
    {
    }
}

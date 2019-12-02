using MediatR;
using SFA.DAS.ApplyService.Domain.Entities.Review;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Review.GetGatewayInProgress
{
    public class GetGatewayInProgressRequest : IRequest<List<Gateway>>
    {
    }
}

using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class GetGatewayPagesRequest : IRequest<List<GatewayPageAnswerSummary>>
    {
        public Guid ApplicationId { get; }

        public GetGatewayPagesRequest(Guid applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}

using MediatR;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class EvaluateGatewayRequest : IRequest<bool>
    {
        public Guid ApplicationId { get; }
        public bool IsGatewayApproved { get; }
        public string EvaluatedBy { get; }

        public EvaluateGatewayRequest(Guid applicationId, bool isGatewayApproved, string evaluatedBy)
        {
            ApplicationId = applicationId;
            IsGatewayApproved = isGatewayApproved;
            EvaluatedBy = evaluatedBy;
        }
    }
}

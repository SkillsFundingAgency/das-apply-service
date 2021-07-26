using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway.ExternalApiCheckDetails
{
    public class UpdateExternalApiCheckDetailsRequest : IRequest<bool>
    {
        public Guid ApplicationId { get; }
        public ApplyGatewayDetails ApplyGatewayDetails { get; }
        public string UserId { get; set; }
        public string UserName { get; }

        public UpdateExternalApiCheckDetailsRequest(Guid applicationId, ApplyGatewayDetails applyGatewayDetails, string userId, string userName)
        {
            ApplicationId = applicationId;
            ApplyGatewayDetails = applyGatewayDetails;
            UserId = userId;
            UserName = userName;
        }
    }
}

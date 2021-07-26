using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class GetGatewayPageAnswerRequest : IRequest<GatewayPageAnswer>
    {
        public Guid ApplicationId { get; }
        public string PageId { get; }

        public GetGatewayPageAnswerRequest(Guid applicationId, string pageId)
        {
            ApplicationId = applicationId;
            PageId = pageId;
        }
    }
}

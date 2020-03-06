using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class UpsertGatewayPageAnswerRequest
    {
        public Guid ApplicationId { get; }
        public string PageId { get; }
        public string Status { get; }
        public GatewayPageData GatewayPageData { get; }
        public string UserName { get; }

        public UpsertGatewayPageAnswerRequest(Guid applicationId, string pageId, string status, GatewayPageData gatewayPageData, string username)
        {
            ApplicationId = applicationId;
            PageId = pageId;
            Status = status;
            GatewayPageData = gatewayPageData;
            UserName = username;
        }
    }
}

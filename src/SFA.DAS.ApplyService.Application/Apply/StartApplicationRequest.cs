using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply
{
    public class StartApplicationRequest : IRequest<StartApplicationResponse>
    {
        public Guid ApplicationId { get; set; }

        public Guid UserId { get; set; }

        public string ApplicationType { get; set; }

        public StartApplicationRequest(Guid applicationId, Guid userId, string applicationType)
        {
            ApplicationId = applicationId;
            UserId = userId;
            ApplicationType = applicationType;
        }
    }
}
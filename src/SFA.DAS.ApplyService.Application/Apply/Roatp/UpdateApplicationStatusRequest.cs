using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class UpdateApplicationStatusRequest : IRequest<bool>
    {
        public Guid ApplicationId { get; set; }
        public string ApplicationStatus { get; set; }
        public string UserId { get; set; }

        public UpdateApplicationStatusRequest(Guid applicationId, string applicationStatus, string userId)
        {
            ApplicationId = applicationId;
            ApplicationStatus = applicationStatus;
            UserId = userId;
        }
    }
}

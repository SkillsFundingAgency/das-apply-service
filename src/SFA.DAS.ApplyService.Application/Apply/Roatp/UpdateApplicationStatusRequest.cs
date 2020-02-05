using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class UpdateApplicationStatusRequest : IRequest<bool>
    {
        public Guid ApplicationId { get; set; }
        public string ApplicationStatus { get; set; }

        public UpdateApplicationStatusRequest(Guid applicationId, string applicationStatus)
        {
            ApplicationId = applicationId;
            ApplicationStatus = applicationStatus;
        }
    }
}

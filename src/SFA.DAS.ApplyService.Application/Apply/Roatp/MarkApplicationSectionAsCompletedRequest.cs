using MediatR;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class MarkApplicationSectionAsCompletedRequest : IRequest<bool>
    { 
        public Guid ApplicationId { get; set; }
        public Guid ApplicationSectionId { get; set; }

        public MarkApplicationSectionAsCompletedRequest(Guid applicationId, Guid applicationSectionId)
        {
            ApplicationId = applicationId;
            ApplicationSectionId = applicationSectionId;
        }
    }
}

using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{

    public class RemoveApplicationSectionCompletedRequest : IRequest<bool>
    {
        public Guid ApplicationId { get; set; }
        public Guid ApplicationSectionId { get; set; }

        public RemoveApplicationSectionCompletedRequest(Guid applicationId, Guid applicationSectionId)
        {
            ApplicationId = applicationId;
            ApplicationSectionId = applicationSectionId;
        }
    }
}

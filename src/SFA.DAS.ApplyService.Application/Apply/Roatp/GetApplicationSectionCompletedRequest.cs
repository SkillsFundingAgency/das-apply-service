using MediatR;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class GetApplicationSectionCompletedRequest : IRequest<bool>
    {
        public Guid ApplicationId { get; set; }
        public Guid ApplicationSectionId { get; set; }

        public GetApplicationSectionCompletedRequest(Guid applicationId, Guid applicationSectionId)
        {
            ApplicationId = applicationId;
            ApplicationSectionId = applicationSectionId;
        }
    }
}

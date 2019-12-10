using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class GetApplicationDataRequest : IRequest<RoatpApplicationData>
    {
        public Guid ApplicationId { get; set; }

        public GetApplicationDataRequest(Guid applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}

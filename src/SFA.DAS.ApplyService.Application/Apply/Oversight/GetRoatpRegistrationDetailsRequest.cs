using MediatR;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class GetRoatpRegistrationDetailsRequest : IRequest<RoatpRegistrationDetails>
    {
        public Guid ApplicationId { get; }

        public GetRoatpRegistrationDetailsRequest(Guid applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}

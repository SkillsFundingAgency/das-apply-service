using MediatR;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.GetApplications
{
    public class GetContactForApplicationRequest : IRequest<Domain.Entities.Contact>
    {
        public Guid ApplicationId { get; }

        public GetContactForApplicationRequest(Guid applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}

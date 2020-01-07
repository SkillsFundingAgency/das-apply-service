using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.GetApplications
{
    public class GetApplicationRequest : IRequest<Domain.Entities.Apply>
    {
        public Guid ApplicationId { get; }

        public GetApplicationRequest(Guid applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}
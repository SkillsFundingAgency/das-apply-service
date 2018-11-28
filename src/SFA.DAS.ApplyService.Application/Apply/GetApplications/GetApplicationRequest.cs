using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.GetApplications
{
    public class GetApplicationRequest : IRequest<Domain.Entities.Application>
    {
        public Guid ApplicationId { get; }

        public GetApplicationRequest(Guid applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}
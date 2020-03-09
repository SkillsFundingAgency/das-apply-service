using System;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

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
using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.GetApplications
{
    public class GetApplicationByUserRequest : IRequest<Domain.Entities.Apply>
    {
        public Guid ApplicationId { get; }
        public Guid SigninId { get; }

        public GetApplicationByUserRequest(Guid applicationId, Guid signinId)
        {
            SigninId = signinId;
            ApplicationId = applicationId;
        }
    }
}
using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.GetApplications
{
    public class GetApplicationByUserIdRequest : IRequest<Domain.Entities.Apply>
    {
        public Guid ApplicationId { get; }
        public Guid SigninId { get; }

        public GetApplicationByUserIdRequest(Guid applicationId, Guid signinId)
        {
            SigninId = signinId;
            ApplicationId = applicationId;
        }
    }
}
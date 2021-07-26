using System;
using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.GetApplications
{
    public class GetApplicationsRequest : IRequest<List<Domain.Entities.Apply>>
    {
        public Guid SigninId { get; }

        public bool CreatedBy { get; }

        public GetApplicationsRequest(Guid signinId, bool createdBy)
        {
            SigninId = signinId;
            CreatedBy = createdBy;
        }
    }
}
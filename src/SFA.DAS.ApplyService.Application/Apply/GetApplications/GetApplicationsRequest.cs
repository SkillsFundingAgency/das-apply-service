using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.GetApplications
{
    public class GetApplicationsRequest : IRequest<List<Domain.Entities.Application>>
    {
        public Guid UserId { get; }

        public GetApplicationsRequest(Guid userId)
        {
            UserId = userId;
        }
    }
}
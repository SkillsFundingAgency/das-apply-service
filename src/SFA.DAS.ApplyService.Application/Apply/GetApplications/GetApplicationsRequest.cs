using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.GetApplications
{
    public class GetApplicationsRequest : IRequest<List<Domain.Entities.Apply>>
    {
        public Guid UserId { get; }

        public bool CreatedBy { get; }

        public GetApplicationsRequest(Guid userId, bool createdBy)
        {
            UserId = userId;
            CreatedBy = createdBy;
        }
    }
}
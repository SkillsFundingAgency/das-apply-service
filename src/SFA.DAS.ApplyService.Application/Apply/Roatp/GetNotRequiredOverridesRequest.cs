using MediatR;
using System;
using System.Collections.Generic;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class GetNotRequiredOverridesRequest : IRequest<List<NotRequiredOverride>>
    {
        public Guid ApplicationId { get; }

        public GetNotRequiredOverridesRequest(Guid applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}

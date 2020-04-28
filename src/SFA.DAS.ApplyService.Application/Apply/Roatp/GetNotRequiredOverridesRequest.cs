using MediatR;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class GetNotRequiredOverridesRequest : IRequest<NotRequiredOverrideConfiguration>
    {
        public Guid ApplicationId { get; }

        public GetNotRequiredOverridesRequest(Guid applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}

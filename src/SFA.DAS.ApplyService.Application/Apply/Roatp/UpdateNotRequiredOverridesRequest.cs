using MediatR;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class UpdateNotRequiredOverridesRequest : IRequest<bool>
    {
        public Guid ApplicationId { get; }
        public NotRequiredOverrideConfiguration NotRequiredOverrides { get; }

        public UpdateNotRequiredOverridesRequest(Guid applicationId, NotRequiredOverrideConfiguration notRequiredOverrides)
        {
            ApplicationId = applicationId;
            NotRequiredOverrides = notRequiredOverrides;
        }
    }
}

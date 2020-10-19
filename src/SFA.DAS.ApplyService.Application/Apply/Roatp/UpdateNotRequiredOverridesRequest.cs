using MediatR;
using System;
using System.Collections.Generic;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class UpdateNotRequiredOverridesRequest : IRequest<bool>
    {
        public Guid ApplicationId { get; }
        public IEnumerable<NotRequiredOverride> NotRequiredOverrides { get; }

        public UpdateNotRequiredOverridesRequest(Guid applicationId, IEnumerable<NotRequiredOverride> notRequiredOverrides)
        {
            ApplicationId = applicationId;
            NotRequiredOverrides = notRequiredOverrides;
        }
    }
}

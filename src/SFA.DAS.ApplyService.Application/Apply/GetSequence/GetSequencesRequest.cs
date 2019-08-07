using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.GetSequence
{
    using System.Collections.Generic;

    public class GetSequencesRequest : IRequest<IEnumerable<ApplicationSequence>>
    {
        public Guid ApplicationId { get; }

        public GetSequencesRequest(Guid applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}

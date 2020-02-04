using System;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.GetSection
{
    using System.Collections.Generic;

    public class GetSectionsRequest : IRequest<IEnumerable<ApplicationSection>>
    {
        public Guid ApplicationId { get; }
        public int SequenceId { get; }
        public Guid UserId { get; }

        public GetSectionsRequest(Guid applicationId, int sequenceId, Guid userId)
        {
            ApplicationId = applicationId;
            SequenceId = sequenceId;
            UserId = userId;
        }
    }
}
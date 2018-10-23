using System;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.GetSequence
{
    public class GetSequenceRequest : IRequest<Sequence>
    {
        public Guid ApplicationId { get; }
        public Guid UserId { get; }
        public string SequenceId { get; }

        public GetSequenceRequest(Guid applicationId, Guid userId, string sequenceId)
        {
            ApplicationId = applicationId;
            UserId = userId;
            SequenceId = sequenceId;
        }
    }
}
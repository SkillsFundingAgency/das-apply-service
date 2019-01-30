using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.GetSequence
{
    public class GetSequenceRequest : IRequest<ApplicationSequence>
    {
        public Guid ApplicationId { get; }
        public Guid? UserId { get; }
        public int SequenceId { get; }

        public GetSequenceRequest(Guid applicationId, Guid? userId, int sequenceId)
        {
            ApplicationId = applicationId;
            UserId = userId;
            SequenceId = sequenceId;
        }
    }
}

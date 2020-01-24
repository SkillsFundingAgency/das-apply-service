using MediatR;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply.Review.Applications
{
    public class OpenApplicationsRequest : IRequest<List<Domain.Entities.Apply>>
    {
        public int SequenceId { get; }

        public OpenApplicationsRequest(int sequenceId)
        {
            SequenceId = sequenceId;
        }
    }
}

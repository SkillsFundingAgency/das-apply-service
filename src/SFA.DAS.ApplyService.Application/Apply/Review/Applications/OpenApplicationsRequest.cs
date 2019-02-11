using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply.Review.Applications
{
    public class OpenApplicationsRequest : IRequest<List<ApplicationSummaryItem>>
    {
        public int SequenceId { get; }

        public OpenApplicationsRequest(int sequenceId)
        {
            SequenceId = sequenceId;
        }
    }
}

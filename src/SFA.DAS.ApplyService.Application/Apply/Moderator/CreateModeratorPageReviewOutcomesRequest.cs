using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;

namespace SFA.DAS.ApplyService.Application.Apply.Moderator
{
    public class CreateModeratorPageReviewOutcomesRequest : IRequest
    {
        public List<ModeratorPageReviewOutcome> ModeratorPageReviewOutcomes { get; set; }
    }
}

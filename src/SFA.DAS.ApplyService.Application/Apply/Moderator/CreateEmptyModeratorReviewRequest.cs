using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;

namespace SFA.DAS.ApplyService.Application.Apply.Moderator
{
    public class CreateEmptyModeratorReviewRequest : IRequest
    {
        public Guid ApplicationId { get; }
        public string ModeratorUserId { get; }
        public List<ModeratorPageReviewOutcome> PageReviewOutcomes { get; }

        public CreateEmptyModeratorReviewRequest(Guid applicationId, string moderatorUserId, List<ModeratorPageReviewOutcome> pageReviewOutcomes)
        {
            ApplicationId = applicationId;
            ModeratorUserId = moderatorUserId;
            PageReviewOutcomes = pageReviewOutcomes;
        }
    }
}

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
        public string ModeratorUserName { get; }
        public List<ModeratorPageReviewOutcome> PageReviewOutcomes { get; }

        public CreateEmptyModeratorReviewRequest(Guid applicationId, string moderatorUserId, string moderatorUserName, List<ModeratorPageReviewOutcome> pageReviewOutcomes)
        {
            ApplicationId = applicationId;
            ModeratorUserId = moderatorUserId;
            ModeratorUserName = moderatorUserName;
            PageReviewOutcomes = pageReviewOutcomes;
        }
    }
}

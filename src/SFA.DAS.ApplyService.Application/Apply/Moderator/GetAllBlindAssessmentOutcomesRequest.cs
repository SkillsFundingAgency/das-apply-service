using MediatR;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply.Moderator
{
    public class GetAllBlindAssessmentOutcomesRequest : IRequest<List<BlindAssessmentOutcome>>
    {
        public GetAllBlindAssessmentOutcomesRequest(Guid applicationId)
        {
            ApplicationId = applicationId;
        }

        public Guid ApplicationId { get; }
    }
}

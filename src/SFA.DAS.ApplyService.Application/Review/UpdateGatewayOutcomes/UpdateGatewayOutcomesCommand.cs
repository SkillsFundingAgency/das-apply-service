using MediatR;
using SFA.DAS.ApplyService.Domain.Review;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Review.UpdateGatewayOutcomes
{
    public class UpdateGatewayOutcomesCommand : IRequest
    {
        public Guid ApplicationId { get; set; }
        public string UserId { get; set; }
        public DateTime ChangedAt { get; set; }
        public List<Outcome> OutcomesDelta { get; set; }
    }
}

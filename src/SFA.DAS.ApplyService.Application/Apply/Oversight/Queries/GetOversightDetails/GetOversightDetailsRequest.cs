using System;
using MediatR;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetOversightDetails
{
    public class GetOversightDetailsRequest : IRequest<ApplicationOversightDetails>
    {
        public Guid ApplicationId { get; }

        public GetOversightDetailsRequest(Guid applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}
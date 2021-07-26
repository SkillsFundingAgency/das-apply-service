using System;
using MediatR;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetOversightDetails
{
    public class GetOversightApplicationDetailsRequest : IRequest<ApplicationOversightDetails>
    {
        public Guid ApplicationId { get; }

        public GetOversightApplicationDetailsRequest(Guid applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}
using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.CheckOrganisationStandardStatus
{
    public class CheckOrganisationStandardStatusRequest : IRequest<string>
    {
        public Guid ApplicationId { get; }
        public int StandardId { get; }

        public CheckOrganisationStandardStatusRequest(Guid applicationId, int standardId)
        {
            ApplicationId = applicationId;
            StandardId = standardId;
        }
    }
}
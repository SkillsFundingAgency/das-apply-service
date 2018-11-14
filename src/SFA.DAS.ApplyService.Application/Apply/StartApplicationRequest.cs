using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply
{
    public class StartApplicationRequest : IRequest
    {
        public string ApplicationType { get; }
        public Guid ApplyingOrganisationId { get; }
        public Guid UserId { get; set; }
        public int OrganisationType { get; }

        public StartApplicationRequest(string applicationType, Guid applyingOrganisationId, Guid userId, int organisationType)
        {
            ApplicationType = applicationType;
            ApplyingOrganisationId = applyingOrganisationId;
            UserId = userId;
            OrganisationType = organisationType;
        }
    }
}
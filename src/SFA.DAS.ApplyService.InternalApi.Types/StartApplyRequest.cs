using System;

namespace SFA.DAS.ApplyService.InternalApi.Types
{
    public class StartApplyRequest
    {
        public string ApplicationType { get; set; }
        public Guid ApplyingOrganisationId { get; set; }
        public Guid UserId { get; set; }
        public int OrganisationType { get; set; }
    }
}
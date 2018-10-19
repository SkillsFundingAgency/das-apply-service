using System;

namespace SFA.DAS.ApplyService.InternalApi.Types
{
    public class StartApplyRequest
    {
        public string ApplicationType { get; set; }
        public Guid ApplyingOrganisationId { get; set; }
        public string Username { get; set; }
    }
}
using System;

namespace SFA.DAS.ApplyService.InternalApi.Types
{
    public class StartApplyRequest
    {
        public Guid ApplicationId { get; set; }
        public Guid UserId { get; set; }
        public string ApplicationType { get; set; }
    }
}
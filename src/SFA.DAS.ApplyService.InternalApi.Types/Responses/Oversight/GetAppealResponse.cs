using System;

namespace SFA.DAS.ApplyService.InternalApi.Types.Responses.Oversight
{
    public class GetAppealResponse
    {
        public string Message { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}

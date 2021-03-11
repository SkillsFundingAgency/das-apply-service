using System;

namespace SFA.DAS.ApplyService.Domain.QueryResults
{
    public class Appeal
    {
        public string Message { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

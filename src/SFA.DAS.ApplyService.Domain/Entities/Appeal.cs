using System;
using SFA.DAS.ApplyService.Application.Interfaces;

namespace SFA.DAS.ApplyService.Domain.Entities
{
    // TODO: APPEALREVIEW - Review once appeal work starts
    public class Appeal : IAuditable
    {
        public Guid Id { get; set; }
        public Guid OversightReviewId { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedOn { get; set; }

        public Appeal()
        {
            Id = Guid.NewGuid();
            CreatedOn = DateTime.UtcNow;
        }
    }
}

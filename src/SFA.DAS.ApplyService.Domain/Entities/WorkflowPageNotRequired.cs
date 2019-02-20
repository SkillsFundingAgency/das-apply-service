using System;

namespace SFA.DAS.ApplyService.Domain.Entities
{
    public class WorkflowPageNotRequired
    {
        public Guid Id { get; set; }
        public Guid WorkflowId { get; set; }
        public int SequenceId { get; set; }
        public int SectionId { get; set; }
        public int PageId { get; set; }
        public string OrganisationType { get; set; }
        public string Status { get; set; }
    }
}

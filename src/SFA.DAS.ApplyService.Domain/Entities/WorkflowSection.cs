using System;

namespace SFA.DAS.ApplyService.Domain.Entities
{
    public class WorkflowSection
    {
        public Guid Id { get; set; }
        public Guid WorkflowId { get; set; }
        public int SequenceId { get; set; }
        public int SectionId { get; set; }
        public string Title { get; set; }
        public string LinkTitle { get; set; }
        public string Status { get; set; }
        public string DisplayType { get; set; }
        public QnAData QnAData { get; set; }
    }
}
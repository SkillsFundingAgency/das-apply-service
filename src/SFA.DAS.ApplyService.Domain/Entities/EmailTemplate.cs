using System;

namespace SFA.DAS.ApplyService.Domain.Entities
{
    public class EmailTemplate : EntityBase
    {
        public Guid Id { get; set; }
        public string TemplateName { get; set; }
        public string TemplateId { get; set; }
        public string Recipients { get; set; }

        public string RecipientTemplate { get; set; }
    }
}

using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class Input
    {
        public string Type { get; set; }
        public dynamic Options { get; set; }
        public List<ValidationDefinition> Validations { get; set; }
    }
}
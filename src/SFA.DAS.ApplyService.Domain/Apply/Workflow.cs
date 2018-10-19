using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class Workflow
    {
        public List<Sequence> Sequences { get; set; }
    }
}
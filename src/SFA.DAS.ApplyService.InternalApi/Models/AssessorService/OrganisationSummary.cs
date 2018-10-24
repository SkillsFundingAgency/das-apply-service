using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Models.AssessorService
{
    public class OrganisationSummary
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public long? Ukprn { get; set; }
    }
}

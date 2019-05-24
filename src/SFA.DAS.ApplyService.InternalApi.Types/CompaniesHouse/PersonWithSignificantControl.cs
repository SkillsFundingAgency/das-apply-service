using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.Types.CompaniesHouse
{
    public class PersonWithSignificantControl
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public IEnumerable<string> NaturesOfControl { get; set; }
        public DateTime NotifiedOn { get; set; }
        public DateTime? CeasedOn { get; set; }
    }
}

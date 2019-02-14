using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.Types.CompaniesHouse
{
    public class Officer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public Address Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime AppointedOn { get; set; }
        public DateTime? ResignedOn { get; set; }

        public IEnumerable<Disqualification> Disqualifications { get; set; }
    }
}

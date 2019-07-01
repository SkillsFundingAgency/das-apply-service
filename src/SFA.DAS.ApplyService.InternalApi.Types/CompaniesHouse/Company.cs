using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.Types.CompaniesHouse
{
    public class Company
    {
        public string CompanyNumber { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public Address RegisteredOfficeAddress { get; set; }
        public IEnumerable<string> NatureOfBusiness { get; set; } // sic codes
        public DateTime? IncorporatedOn { get; set; } // date_of_creation
        public DateTime? DissolvedOn { get; set; } // date_of_cessation
        public bool? IsLiquidated { get; set; }
        public IEnumerable<string> PreviousNames { get; set; }

        public Accounts Accounts { get; set; }
        public IEnumerable<Officer> Officers { get; set; }
        public IEnumerable<PersonWithSignificantControl> PeopleWithSignificantControl { get; set; }

        public bool ManualEntryRequired { get; set; }
    }
}

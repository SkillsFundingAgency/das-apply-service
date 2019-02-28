using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.Types.CharityCommission
{
    public class Charity
    {
        public string CharityNumber { get; set; }
        public string CompanyNumber { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public Address RegisteredOfficeAddress { get; set; }

        public IEnumerable<string> NatureOfBusiness { get; set; }

        public DateTime? IncorporatedOn { get; set; }
        public DateTime? DissolvedOn { get; set; }

        public Accounts Accounts { get; set; }

        public IEnumerable<Trustee> Trustees { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.Types.CharityCommission
{
    public class Charity
    {
        public string CharityNumber { get; set; }
        
        public string RegistrationNumber { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }
        
        public string Type { get; set; }

        public DateTime? RegistrationDate  { get; set; }
        
        public DateTime? RemovalDate { get; set; }

        public IEnumerable<Trustee> Trustees { get; set; }

        public bool IsActivelyTrading
        {
            get
            {
                return Status.Equals("registered", StringComparison.InvariantCultureIgnoreCase) && RemovalDate == null;
            }
        }
        
        public bool TrusteeManualEntryRequired { get; set; }
    }
}

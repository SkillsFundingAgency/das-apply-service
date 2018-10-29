using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Models
{
    public class Organisation
    {
        public int Ukprn { get; set; }

        public string Name { get; set; }

        public OrganisationAddress Address { get; set; }
    }

    public class OrganisationAddress
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
    }
}


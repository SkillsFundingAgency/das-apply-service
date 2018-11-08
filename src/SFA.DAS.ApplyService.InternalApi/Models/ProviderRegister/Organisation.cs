using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Models.ProviderRegister
{
    public class Organisation
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string OrganisationType { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.InternalApi.Types
{
    public class MigrateContactOrganisation
    {
        public Contact contact { get; set; }
        public Organisation organisation { get; set; }
    }
}

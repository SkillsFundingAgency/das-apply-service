using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ApplyService.InternalApi.Types
{
    public class UpdateContactOrgId
    {
        public Guid ContactId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}

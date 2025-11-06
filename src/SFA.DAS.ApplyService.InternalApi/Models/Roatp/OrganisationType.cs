
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.Models.Roatp;

public class OrganisationType
{
    public int Id { get; set; }
    public string Description { get; set; }
    public const int Unassigned = 0;
}

public record OrganisationTypesResponse(IEnumerable<OrganisationType> OrganisationTypes);

using Newtonsoft.Json;
using SFA.DAS.ApplyService.InternalApi.Types;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.ViewModels
{
    public class OrganisationSearchViewModel
    {
        public string SearchString { get; set; }

        public string OrganisationTypeFilter { get; set; }

        public string Name { get; set; }

        public int? Ukprn { get; set; }

        public string Postcode { get; set; }

        public string OrganisationType { get; set; }

        [JsonIgnore]
        public IEnumerable<OrganisationSearchResult> Organisations { get; set; }

        [JsonIgnore]
        public IEnumerable<OrganisationType> OrganisationTypes { get; set; }
    }
}

using Newtonsoft.Json;
using SFA.DAS.ApplyService.Web.Models;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.ViewModels
{
    public class OrganisationSearchViewModel
    {
        public string SearchString { get; set; }

        public Organisation SelectedOrganisation { get; set; }

        [JsonIgnore]
        public IEnumerable<Organisation> Organisations { get; set; }

        [JsonIgnore]
        public IEnumerable<OrganisationType> OrganisationTypes { get; set; }
    }
}

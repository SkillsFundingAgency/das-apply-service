using Newtonsoft.Json;
using SFA.DAS.ApplyService.InternalApi.Types;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApplyService.Web.ViewModels
{
    public class OrganisationSearchViewModel
    {
        public string SearchString { get; set; }

        public string Name { get; set; }

        public int? Ukprn { get; set; }

        public string Postcode { get; set; }

        public string OrganisationType { get; set; }

        [JsonIgnore]
        public IEnumerable<OrganisationSearchResult> Organisations { get; set; }

        [JsonIgnore]
        public IEnumerable<OrganisationType> OrganisationTypes { get; set; }

        public string OrganisationFoundString()
        {
            var result = "0 results found";
            if (Organisations != null && Organisations.Any())
            {
                var resultsString = Organisations.Count() > 1 ? "results" : "result";
                result = $"{Organisations.Count()} {resultsString} found";
            }
            return result;
        }
    }
}

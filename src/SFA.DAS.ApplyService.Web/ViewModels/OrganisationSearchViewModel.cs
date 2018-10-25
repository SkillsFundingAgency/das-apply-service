using SFA.DAS.ApplyService.Web.Models;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.ViewModels
{
    public class OrganisationSearchViewModel
    {
        public string SearchString { get; set; }
        public IEnumerable<Organisation> Organisations { get; set; }
    }
}


namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    using System.Collections.Generic;

    public class TaskListViewModel
    {
        public string UKPRN { get; set; }
        public string OrganisationName { get; set; }
        public IEnumerable<Domain.Entities.ApplicationSequence> ApplicationSequences { get; set; }
    }
}

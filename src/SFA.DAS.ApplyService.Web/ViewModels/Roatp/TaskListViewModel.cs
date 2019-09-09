
namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using SFA.DAS.ApplyService.Application.Apply.Roatp;

    public class TaskListViewModel
    {
        public Guid ApplicationId { get; set; }
        public string UKPRN { get; set; }
        public string OrganisationName { get; set; }
        public IEnumerable<ApplicationSequence> ApplicationSequences { get; set; }

        public string CssClass(int sequenceId, int sectionId, bool sequential = false)
        {
            var status = RoatpTaskListWorkflowHandler.SectionStatus(ApplicationSequences, sequenceId, sectionId, sequential);

            if (status == String.Empty)
            {
                return "hidden";
            }

            var cssClass = status.ToLower();
            cssClass = cssClass.Replace(" ", "");
            
            return cssClass;
        }

        public string SectionStatus(int sequenceId, int sectionId, bool sequential = false)
        {
            return RoatpTaskListWorkflowHandler.SectionStatus(ApplicationSequences, sequenceId, sectionId, sequential);
        }

        public bool PreviousSectionCompleted(int sequenceId, int sectionId, bool sequential = false)
        {
            var sequence = ApplicationSequences.FirstOrDefault(x => x.SequenceId == sequenceId);

            return RoatpTaskListWorkflowHandler.PreviousSectionCompleted(sequence, sectionId, sequential);
        }

        public bool VerifiedCompaniesHouse { get; set; }
        public bool VerifiedCharityCommision { get; set; }
    }
}

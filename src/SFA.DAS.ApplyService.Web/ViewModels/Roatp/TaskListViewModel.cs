
namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using SFA.DAS.ApplyService.Application.Apply.Roatp;

    public class TaskListViewModel
    {
        private const string SupportingProviderApplicationRouteId = "3";

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
        public bool CompaniesHouseManualEntry { get; set; }
        public bool VerifiedCharityCommision { get; set; }
        public bool CharityCommissionManualEntry { get; set; }
        public string ApplicationRouteId { get; set; }

        public string WhosInControlStartPageId
        {
            get
            {
                var whosInControlStartPageId = RoatpWorkflowPageIds.WhosInControl.SoleTraderPartnership;
                if (VerifiedCompaniesHouse)
                {
                    whosInControlStartPageId = RoatpWorkflowPageIds.WhosInControl.CompaniesHouseStartPage;
                    if (CompaniesHouseManualEntry)
                    {
                        whosInControlStartPageId = RoatpWorkflowPageIds.WhosInControl.AddPeopleInControl;
                    }
                }
                else
                {
                    if (VerifiedCharityCommision)
                    {
                        whosInControlStartPageId = RoatpWorkflowPageIds.WhosInControl.CharityCommissionStartPage;
                        if (CharityCommissionManualEntry)
                        {
                            whosInControlStartPageId = RoatpWorkflowPageIds.WhosInControl.CharityCommissionNoTrustees;
                        }
                    }
                }
                return whosInControlStartPageId;
            }
        }

        public string DescribeOrganisationStartPageId
        {
            get
            {
                var describeOrganisationStartPageId = RoatpWorkflowPageIds.DescribeYourOrganisation.MainEmployerStartPage;

                if (ApplicationRouteId == SupportingProviderApplicationRouteId)
                {
                    describeOrganisationStartPageId = RoatpWorkflowPageIds.DescribeYourOrganisation.SupportingStartPage;
                }

                return describeOrganisationStartPageId;
            }
        }
    }
}

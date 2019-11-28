using SFA.DAS.ApplyService.Web.Configuration;
using SFA.DAS.ApplyService.Web.Services;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using SFA.DAS.ApplyService.Application.Apply.Roatp;

    public class TaskListViewModel
    {
        private readonly IRoatpTaskListWorkflowService _roatpTaskListWorkflowService;

        public TaskListViewModel(IRoatpTaskListWorkflowService roatpTaskListWorkflowService)
        {
            _roatpTaskListWorkflowService = roatpTaskListWorkflowService;
        }


        private const string EmployerApplicationRouteId = "2";

        public Guid ApplicationId { get; set; }
        public string UKPRN { get; set; }
        public string OrganisationName { get; set; }
        public string TradingName { get; set; }

        public List<NotRequiredOverrideConfiguration> NotRequiredOverrides { get; set; }


        public string ApplicationRouteShortText
        {
            get
            {
                switch(ApplicationRouteId)
                {
                    case "1":
                        {
                            return "Main";                            
                        }
                    case "2":
                        {
                            return "Employer";
                        }
                    case "3":
                        {
                            return "Supporting";
                        }
                }

                return string.Empty;
            }
        }
    
        public string PageStatusCompleted => "completed";
        public int IntroductionSectionId => 1;
        public int Sequence1Id => 1;

        public IEnumerable<ApplicationSequence> ApplicationSequences { get; set; }
        
        public string CssClass(int sequenceId, int sectionId, bool sequential = false)
        {
            var status = _roatpTaskListWorkflowService.SectionStatus(ApplicationSequences, NotRequiredOverrides, sequenceId, sectionId, ApplicationRouteId, sequential);

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
            return _roatpTaskListWorkflowService.SectionStatus(ApplicationSequences, NotRequiredOverrides, sequenceId, sectionId, ApplicationRouteId, sequential);
        }

        public bool PreviousSectionCompleted(int sequenceId, int sectionId, bool sequential = false)
        {
            var sequence = ApplicationSequences.FirstOrDefault(x => x.SequenceId == sequenceId);

            return _roatpTaskListWorkflowService.PreviousSectionCompleted(sequence, sectionId, sequential);
        }

        public bool IntroductionPageNextSectionUnavailable(int sequenceId, int sectionId)
        {
            // This block disables the other sequences if YourOrganisation sequence isn't complete
            // TECH DEBT: This is processor intensive, see it could be done a better way
            if (sequenceId != RoatpWorkflowSequenceIds.YourOrganisation)
            {
                var yourOrganisationSequence = ApplicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);

                foreach(var section in yourOrganisationSequence.Sections)
                {
                    var sectionStatus = _roatpTaskListWorkflowService.SectionStatus(ApplicationSequences,NotRequiredOverrides, RoatpWorkflowSequenceIds.YourOrganisation, section.SectionId,ApplicationRouteId, true);
                    if (sectionStatus.ToLower() != PageStatusCompleted)
                    {
                        return true;
                    }
                }
            }

            // CriminalComplianceChecks has two intro pages...
            if (sequenceId == RoatpWorkflowSequenceIds.CriminalComplianceChecks)
            {
                var SecondCriminialIntroductionSectionId = 3;
                if (sectionId > SecondCriminialIntroductionSectionId)
                {
                    var statusOfSecondIntroductionPage = SectionStatus(sequenceId, SecondCriminialIntroductionSectionId);
                    return statusOfSecondIntroductionPage.ToLower() != PageStatusCompleted;
                }
            }

            var statusOfIntroductionPage = SectionStatus(sequenceId,IntroductionSectionId);
            if (sequenceId > Sequence1Id && sectionId != IntroductionSectionId && statusOfIntroductionPage.ToLower() != PageStatusCompleted)
                return true;

            return false;
        }

        public bool VerifiedCompaniesHouse { get; set; }
        public bool CompaniesHouseManualEntry { get; set; }
        public bool VerifiedCharityCommission { get; set; }
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
                    if (VerifiedCharityCommission)
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
                var describeOrganisationStartPageId = RoatpWorkflowPageIds.DescribeYourOrganisation.MainSupportingStartPage;

                if (ApplicationRouteId == EmployerApplicationRouteId)
                {
                    describeOrganisationStartPageId = RoatpWorkflowPageIds.DescribeYourOrganisation.EmployerStartPage;
                }

                return describeOrganisationStartPageId;
            }
        }
    }
}

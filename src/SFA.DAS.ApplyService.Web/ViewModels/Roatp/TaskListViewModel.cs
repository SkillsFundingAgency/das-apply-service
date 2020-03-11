
namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using SFA.DAS.ApplyService.Application.Apply.Roatp;
    using SFA.DAS.ApplyService.Domain.Roatp;
    using SFA.DAS.ApplyService.Web.Configuration;
    using SFA.DAS.ApplyService.Web.Services;

    public class TaskListViewModel : ApplicationSummaryViewModel
    {
        private readonly IRoatpTaskListWorkflowService _taskListWorkflowService;

        public List<NotRequiredOverrideConfiguration> NotRequiredOverrides { get; set; }
    
        public int IntroductionSectionId => 1;
        public int Sequence1Id => 1;
        public string EmployerApplicationRouteId => "2";

        public IEnumerable<ApplicationSequence> ApplicationSequences { get; set; }

        public OrganisationVerificationStatus OrganisationVerificationStatus { get; set; }
        
        public bool YourOrganisationSequenceCompleted { get; set; }
        public bool ApplicationSequencesCompleted { get; set; }

        public TaskListViewModel(IRoatpTaskListWorkflowService taskListWorkflowService,
                                 OrganisationVerificationStatus organisationVerificationStatus,
                                 Guid applicationId)
        {
            _taskListWorkflowService = taskListWorkflowService;
            OrganisationVerificationStatus = organisationVerificationStatus;
            ApplicationId = applicationId;
            ApplicationSequences = _taskListWorkflowService.GetApplicationSequences(ApplicationId);
        }

        public string CssClass(int sequenceId, int sectionId)
        {
            var status = _taskListWorkflowService.SectionStatus(ApplicationId, sequenceId, sectionId, ApplicationSequences, OrganisationVerificationStatus);

            return ConvertTaskListSectionStatusToCssClass(status);
        }

        public string SectionStatus(int sequenceId, int sectionId)
        {
            return _taskListWorkflowService.SectionStatus(ApplicationId, sequenceId, sectionId, ApplicationSequences, OrganisationVerificationStatus);
        }

        public string FinishSectionStatus(int sectionId)
        {
            return _taskListWorkflowService.FinishSectionStatus(ApplicationId, sectionId, ApplicationSequences, ApplicationSequencesCompleted);
        }

        public string FinishCss(int sectionId)
        {
            var status = _taskListWorkflowService.FinishSectionStatus(ApplicationId, sectionId, ApplicationSequences, ApplicationSequencesCompleted);

            return ConvertTaskListSectionStatusToCssClass(status);
        }

        public bool PreviousSectionCompleted(int sequenceId, int sectionId)
        {                      
            return _taskListWorkflowService.PreviousSectionCompleted(ApplicationId, sequenceId, sectionId, ApplicationSequences, OrganisationVerificationStatus);
        }
        
        public string SectionQuestionsStatus(int sequenceId, int sectionId)
        {
            return _taskListWorkflowService.SectionQuestionsStatus(ApplicationId, sequenceId, sectionId, ApplicationSequences);
        }

        public bool IntroductionPageNextSectionUnavailable(int sequenceId, int sectionId)
        {
            // Disable the other sequences if YourOrganisation sequence isn't complete
            if (sequenceId != RoatpWorkflowSequenceIds.YourOrganisation)
            {
                return !YourOrganisationSequenceCompleted;
            }

            // CriminalComplianceChecks has two intro pages...
            if (sequenceId == RoatpWorkflowSequenceIds.CriminalComplianceChecks)
            {
                var SecondCriminialIntroductionSectionId = 3;
                if (sectionId > SecondCriminialIntroductionSectionId)
                {
                    var statusOfSecondIntroductionPage = SectionQuestionsStatus(sequenceId, SecondCriminialIntroductionSectionId);
                    if (statusOfSecondIntroductionPage != TaskListSectionStatus.Completed)
                    {
                        return true;
                    }
                }
            }

            var statusOfIntroductionPage = SectionQuestionsStatus(sequenceId,IntroductionSectionId);
            if (sequenceId > Sequence1Id && sectionId != IntroductionSectionId && statusOfIntroductionPage != TaskListSectionStatus.Completed)
                return true;

            return false;
        }

        public string WhosInControlStartPageId
        {
            get
            {
                var whosInControlStartPageId = RoatpWorkflowPageIds.WhosInControl.SoleTraderPartnership;
                if (OrganisationVerificationStatus.VerifiedCompaniesHouse)
                {
                    whosInControlStartPageId = RoatpWorkflowPageIds.WhosInControl.CompaniesHouseStartPage;
                    if (OrganisationVerificationStatus.CompaniesHouseManualEntry)
                    {
                        whosInControlStartPageId = RoatpWorkflowPageIds.WhosInControl.AddPeopleInControl;
                    }
                }
                else
                {
                    if (OrganisationVerificationStatus.VerifiedCharityCommission)
                    {
                        whosInControlStartPageId = RoatpWorkflowPageIds.WhosInControl.CharityCommissionStartPage;
                        if (OrganisationVerificationStatus.CharityCommissionManualEntry)
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
              

        private string ConvertTaskListSectionStatusToCssClass(string sectionStatus)
        {
            switch (sectionStatus)
            {
                case TaskListSectionStatus.Blank:
                    {
                        return "hidden";                        
                    }
                case TaskListSectionStatus.InProgress:
                    {
                        return "inprogress";
                    }
                case TaskListSectionStatus.NotRequired:
                    {
                        return "notrequired";
                    }
                default:
                    {
                        return sectionStatus.ToLower();
                    }
            }
        }
    }
}

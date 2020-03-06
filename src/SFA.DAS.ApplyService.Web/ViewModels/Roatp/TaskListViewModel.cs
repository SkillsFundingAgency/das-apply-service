
namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using SFA.DAS.ApplyService.Application.Apply.Roatp;
    using SFA.DAS.ApplyService.Web.Configuration;
    using SFA.DAS.ApplyService.Web.Services;

    public class TaskListViewModel : ApplicationSummaryViewModel
    {
        private readonly IRoatpTaskListWorkflowService _taskListWorkflowService;
        private readonly IRoatpOrganisationVerificationService _organisationVerificationService;

        public List<NotRequiredOverrideConfiguration> NotRequiredOverrides { get; set; }
    
        public int IntroductionSectionId => 1;
        public int Sequence1Id => 1;
        public string EmployerApplicationRouteId => "2";

        public List<ApplicationSequence> ApplicationSequences { get; set; }
        
        public TaskListViewModel(IRoatpTaskListWorkflowService taskListWorkflowService, 
                                 IRoatpOrganisationVerificationService organisationVerificationService,
                                 Guid applicationId)
        {
            _taskListWorkflowService = taskListWorkflowService;
            _organisationVerificationService = organisationVerificationService;
            ApplicationId = applicationId;
            ApplicationSequences = _taskListWorkflowService.GetApplicationSequences(ApplicationId);
        }

        public string CssClass(int sequenceId, int sectionId)
        {
            var status = _taskListWorkflowService.SectionStatus(ApplicationId, sequenceId, sectionId, ApplicationSequences);

            return ConvertTaskListSectionStatusToCssClass(status);
        }

        public string SectionStatus(int sequenceId, int sectionId)
        {
            return _taskListWorkflowService.SectionStatus(ApplicationId, sequenceId, sectionId, ApplicationSequences);
        }

        public bool PreviousSectionCompleted(int sequenceId, int sectionId)
        {                      
            return _taskListWorkflowService.PreviousSectionCompleted(ApplicationId, sequenceId, sectionId, ApplicationSequences);
        }

        public bool ApplicationSequencesCompleted()
        {
            return _taskListWorkflowService.ApplicationSequencesCompleted(ApplicationId, ApplicationSequences);
        }

        public bool IntroductionPageNextSectionUnavailable(int sequenceId, int sectionId)
        {
            // This block disables the other sequences if YourOrganisation sequence isn't complete
            // TECH DEBT: This is processor intensive, see if it could be done a better way
            if (sequenceId != RoatpWorkflowSequenceIds.YourOrganisation)
            {
                var yourOrganisationSequence = ApplicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);

                foreach(var section in yourOrganisationSequence.Sections)
                {
                    var sectionStatus = _taskListWorkflowService.SectionStatus(ApplicationId, RoatpWorkflowSequenceIds.YourOrganisation, section.SectionId, ApplicationSequences);
                    if (sectionStatus != TaskListSectionStatus.Completed)
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
                    if (statusOfSecondIntroductionPage != TaskListSectionStatus.Completed)
                    {
                        return true;
                    }
                }
            }

            var statusOfIntroductionPage = SectionStatus(sequenceId,IntroductionSectionId);
            if (sequenceId > Sequence1Id && sectionId != IntroductionSectionId && statusOfIntroductionPage != TaskListSectionStatus.Completed)
                return true;

            return false;
        }

        public string WhosInControlStartPageId
        {
            get
            {
                var whosInControlStartPageId = RoatpWorkflowPageIds.WhosInControl.SoleTraderPartnership;
                var organisationVerificationStatus = _organisationVerificationService.GetOrganisationVerificationStatus(ApplicationId).GetAwaiter().GetResult();
                if (organisationVerificationStatus.VerifiedCompaniesHouse)
                {
                    whosInControlStartPageId = RoatpWorkflowPageIds.WhosInControl.CompaniesHouseStartPage;
                    if (organisationVerificationStatus.CompaniesHouseManualEntry)
                    {
                        whosInControlStartPageId = RoatpWorkflowPageIds.WhosInControl.AddPeopleInControl;
                    }
                }
                else
                {
                    if (organisationVerificationStatus.VerifiedCharityCommission)
                    {
                        whosInControlStartPageId = RoatpWorkflowPageIds.WhosInControl.CharityCommissionStartPage;
                        if (organisationVerificationStatus.CharityCommissionManualEntry)
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

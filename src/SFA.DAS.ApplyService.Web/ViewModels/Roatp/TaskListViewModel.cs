
namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using SFA.DAS.ApplyService.Application.Apply.Roatp;
    using SFA.DAS.ApplyService.Web.Configuration;
    using SFA.DAS.ApplyService.Web.Infrastructure;
    using SFA.DAS.ApplyService.Web.Services;

    public class TaskListViewModel : ApplicationSummaryViewModel
    {
        private const string EmployerApplicationRouteId = "2";
        private readonly IQnaApiClient _qnaApiClient;

        public List<NotRequiredOverrideConfiguration> NotRequiredOverrides { get; set; }
    
        public string PageStatusCompleted => "completed";
        public int IntroductionSectionId => 1;
        public int Sequence1Id => 1;

        public IEnumerable<ApplicationSequence> ApplicationSequences { get; set; }
        
        public TaskListViewModel(IQnaApiClient qnaApiClient)
        {
            _qnaApiClient = qnaApiClient;
        }

        public string CssClass(int sequenceId, int sectionId)
        {
            var status = RoatpTaskListWorkflowService.SectionStatus(ApplicationSequences, NotRequiredOverrides, sequenceId, sectionId, ApplicationRouteId);

            if (status == String.Empty)
            {
                return "hidden";
            }

            var cssClass = status.ToLower();
            cssClass = cssClass.Replace(" ", "");
            
            return cssClass;
        }

        public string SectionStatus(int sequenceId, int sectionId)
        {
            return RoatpTaskListWorkflowService.SectionStatus(ApplicationSequences, NotRequiredOverrides, sequenceId, sectionId, ApplicationRouteId);
        }

        public string WhosInControlCss
        {
            get
            {
                var status = WhosInControlSectionStatus;

                if (status == String.Empty)
                {
                    return "hidden";
                }

                var cssClass = status.ToLower();
                cssClass = cssClass.Replace(" ", "");

                return cssClass;
            }
        }

        public string WhosInControlSectionStatus
        {
            get
            {
                if (VerifiedCompaniesHouse && VerifiedCharityCommission)
                {
                    if ((CompaniesHouseDataConfirmed && !CharityCommissionDataConfirmed)
                        || (!CompaniesHouseDataConfirmed && CharityCommissionDataConfirmed))
                    {
                        return "In Progress";
                    }
                    if (CompaniesHouseDataConfirmed && CharityCommissionDataConfirmed)
                    {
                        return "Completed";
                    }
                }

                if (VerifiedCompaniesHouse && !VerifiedCharityCommission)
                {
                    if (CompaniesHouseDataConfirmed)
                    {
                        return "Completed";
                    }
                }

                if (!VerifiedCompaniesHouse && VerifiedCharityCommission)
                {
                    if (CharityCommissionDataConfirmed)
                    {
                        return "Completed";
                    }
                }

                if (!VerifiedCompaniesHouse && !VerifiedCharityCommission)
                {
                    if (WhosInControlConfirmed)
                    {
                        return "Completed";
                    }
                }

                return "Next";
            }          
        }
        
        public string FinishCss(int sectionId)
        {
            var status = FinishSectionStatus(sectionId);

            if (status == String.Empty)
            {
                return "hidden";
            }

            var cssClass = status.ToLower();
            cssClass = cssClass.Replace(" ", "");

            return cssClass;           
        }

        public string FinishSectionStatus(int sectionId)
        {
            var shutterPageActive = false;
            if (!ApplicationSequencesCompleted())
            {
                return string.Empty;
            }

            var shutterPageIds = new List<string>();
            if (sectionId == RoatpWorkflowSectionIds.Finish.ApplicationPermissionsAndChecks)
            {
                shutterPageIds.Add(RoatpWorkflowPageIds.Finish.ApplicationPermissionsChecksShutterPage);
            }
            else if (sectionId == RoatpWorkflowSectionIds.Finish.TermsAndConditions)
            {
                shutterPageIds.Add(RoatpWorkflowPageIds.Finish.TermsConditionsCOAPart2ShutterPage);
                shutterPageIds.Add(RoatpWorkflowPageIds.Finish.TermsConditionsCOAPart3ShutterPage);
            }

            if (shutterPageIds.Any())
            {
                var finishSequence = ApplicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.Finish);
                var section = finishSequence.Sections.FirstOrDefault(x => x.SectionId == sectionId);
                
                foreach(var shutterPageId in shutterPageIds)
                {
                    var shutterPage = _qnaApiClient.GetPage(ApplicationId, section.Id, shutterPageId)
                                        .GetAwaiter().GetResult();

                    if (shutterPage != null && shutterPage.Active)
                    {
                        shutterPageActive = true;
                        return "In Progress";                        
                    }
                }                    
                
            }

            var sectionStatus = RoatpTaskListWorkflowService.SectionStatus(ApplicationSequences, NotRequiredOverrides, RoatpWorkflowSequenceIds.Finish, sectionId, ApplicationRouteId);
            if (!shutterPageActive)
            {
                return sectionStatus;
            }
            return string.Empty;
        }

        public bool PreviousSectionCompleted(int sequenceId, int sectionId)
        {
            var sequence = ApplicationSequences.FirstOrDefault(x => x.SequenceId == sequenceId);

            if (sequenceId == RoatpWorkflowSequenceIds.YourOrganisation && sectionId == RoatpWorkflowSectionIds.YourOrganisation.DescribeYourOrganisation)
            {
                return (WhosInControlSectionStatus == "Completed");
            }

            if (sequenceId == RoatpWorkflowSequenceIds.Finish)
            {
                if (sectionId == 1)
                {
                    return true;
                }
                return (FinishSectionStatus(sectionId) == "Completed");
            }

            return RoatpTaskListWorkflowService.PreviousSectionCompleted(sequence, sectionId, sequence.Sequential);
        }

        public bool IntroductionPageNextSectionUnavailable(int sequenceId, int sectionId)
        {
            if (sequenceId != RoatpWorkflowSequenceIds.YourOrganisation)
            {
                var yourOrganisationSequence = ApplicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);

                foreach(var section in yourOrganisationSequence.Sections)
                {
                    var sectionStatus = RoatpTaskListWorkflowService.SectionStatus(ApplicationSequences, NotRequiredOverrides, RoatpWorkflowSequenceIds.YourOrganisation, section.SectionId, ApplicationRouteId);
                    if (sectionStatus.ToLower() != PageStatusCompleted)
                    {
                        return true;
                    }
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

        public bool CompaniesHouseDataConfirmed { get; set; }
        public bool CharityCommissionDataConfirmed { get; set; }
        public bool WhosInControlConfirmed { get; set; }

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

        public bool ApplicationSequencesCompleted()
        {
            var applicationSequences = ApplicationSequences.Where(seq => seq.SequenceId != RoatpWorkflowSequenceIds.Finish);
            foreach (var sequence in applicationSequences)
            {
                foreach(var section in sequence.Sections)
                {
                    var sectionStatus = SectionStatus(sequence.SequenceId, section.SectionId).ToLower();
                    if (sectionStatus != "not required" && sectionStatus != "completed")
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}

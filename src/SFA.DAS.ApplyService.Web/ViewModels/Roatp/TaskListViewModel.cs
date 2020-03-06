
namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using SFA.DAS.ApplyService.Application.Apply.Roatp;
    using SFA.DAS.ApplyService.Domain.Apply;
    using SFA.DAS.ApplyService.Web.Configuration;
    using SFA.DAS.ApplyService.Web.Infrastructure;
    using SFA.DAS.ApplyService.Web.Services;

    public class TaskListViewModel : ApplicationSummaryViewModel
    {
        private const string MainApplicationRouteId = "1";
        private const string EmployerApplicationRouteId = "2";
        private const string SupportingApplicationRouteId = "3";
        private const string ConfirmedAnswer = "Yes";
        private readonly IQnaApiClient _qnaApiClient;

        public List<NotRequiredOverrideConfiguration> NotRequiredOverrides { get; set; }
    
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

            return ConvertTaskListSectionStatusToCssClass(status);
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

                return ConvertTaskListSectionStatusToCssClass(status);
            }
        }

        public string WhosInControlSectionStatus
        {
            get
            {
                if (SectionStatus(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails) == TaskListSectionStatus.Blank)
                {
                    return TaskListSectionStatus.Blank;
                }

                if (VerifiedCompaniesHouse && VerifiedCharityCommission)
                {
                    if ((CompaniesHouseDataConfirmed && !CharityCommissionDataConfirmed)
                        || (!CompaniesHouseDataConfirmed && CharityCommissionDataConfirmed))
                    {
                        return TaskListSectionStatus.InProgress;
                    }
                    if (CompaniesHouseDataConfirmed && CharityCommissionDataConfirmed)
                    {
                        return TaskListSectionStatus.Completed;
                    }
                }

                if (VerifiedCompaniesHouse && !VerifiedCharityCommission)
                {
                    if (CompaniesHouseDataConfirmed)
                    {
                        return TaskListSectionStatus.Completed;
                    }
                }

                if (!VerifiedCompaniesHouse && VerifiedCharityCommission)
                {
                    if (CharityCommissionDataConfirmed)
                    {
                        return TaskListSectionStatus.Completed;
                    }
                }

                if (WhosInControlConfirmed)
                {
                    return TaskListSectionStatus.Completed;
                }

                return TaskListSectionStatus.Next;
            }          
        }
        
        public string FinishCss(int sectionId)
        {
            var status = FinishSectionStatus(sectionId);

            return ConvertTaskListSectionStatusToCssClass(status);
        }

        public string FinishSectionStatus(int sectionId)
        {
            if (!ApplicationSequencesCompleted())
            {
                return TaskListSectionStatus.Blank;
            }
            var finishSequence = ApplicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.Finish);

            // TODO: APR-1193 We are calling PreviousSectionCompleted() from FinishSequence.cshtml, then it calls this FinishSectionStatus() method, 
            // which calls PreviousSectionCompleted() again with the code bellow. 
            //if (!PreviousSectionCompleted(finishSequence.SequenceId, sectionId))
            //{
            //    return TaskListSectionStatus.Blank;
            //}

            if (sectionId == RoatpWorkflowSectionIds.Finish.CommercialInConfidenceInformation)
            {
                var commercialInConfidenceAnswer = _qnaApiClient.GetAnswerByTag(ApplicationId, RoatpWorkflowQuestionTags.FinishCommercialInConfidence).GetAwaiter().GetResult();
                if (commercialInConfidenceAnswer != null && !String.IsNullOrWhiteSpace(commercialInConfidenceAnswer.Value))
                {
                    // Section 9.2 handled InProgress State
                    if(commercialInConfidenceAnswer.Value.Equals("yes", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return TaskListSectionStatus.Completed;
                    }
                    else
                    {
                        return TaskListSectionStatus.InProgress;
                    }                  
                }
                else
                {
                    return TaskListSectionStatus.Next;
                }
            }

            if (sectionId == RoatpWorkflowSectionIds.Finish.ApplicationPermissionsAndChecks)
            {
                var permissionPersonalDetails = _qnaApiClient.GetAnswerByTag(ApplicationId, RoatpWorkflowQuestionTags.FinishPermissionPersonalDetails).GetAwaiter().GetResult();
                var accuratePersonalDetails = _qnaApiClient.GetAnswerByTag(ApplicationId, RoatpWorkflowQuestionTags.FinishAccuratePersonalDetails).GetAwaiter().GetResult();
                var permissionSubmitApplication = _qnaApiClient.GetAnswerByTag(ApplicationId, RoatpWorkflowQuestionTags.FinishPermissionSubmitApplication).GetAwaiter().GetResult();

                if (String.IsNullOrWhiteSpace(permissionPersonalDetails.Value) 
                    && String.IsNullOrWhiteSpace(accuratePersonalDetails.Value)
                    && String.IsNullOrWhiteSpace(permissionSubmitApplication.Value))
                {
                    return TaskListSectionStatus.Next;
                }

                if (permissionPersonalDetails.Value == ConfirmedAnswer 
                    && accuratePersonalDetails.Value == ConfirmedAnswer 
                    && permissionSubmitApplication.Value == ConfirmedAnswer)
                {
                    return TaskListSectionStatus.Completed;
                }
                return TaskListSectionStatus.InProgress;
            }

            if (sectionId == RoatpWorkflowSectionIds.Finish.TermsAndConditions)
            {
                Answer conditionsOfAcceptance2 = null;
                //Answer conditionsOfAcceptance3 = null;

                if (ApplicationRouteId == MainApplicationRouteId || ApplicationRouteId == EmployerApplicationRouteId)
                {
                    conditionsOfAcceptance2 = _qnaApiClient.GetAnswerByTag(ApplicationId, RoatpWorkflowQuestionTags.FinishCOA2MainEmployer).GetAwaiter().GetResult();
                    //conditionsOfAcceptance3 = _qnaApiClient.GetAnswerByTag(ApplicationId, RoatpWorkflowQuestionTags.FinishCOA3MainEmployer).GetAwaiter().GetResult();
                }
                else if (ApplicationRouteId == SupportingApplicationRouteId)
                {
                    // TODO: TODO: APR-1193 - This is the return value for 9.3 section for Supporting Provider 
                    //conditionsOfAcceptance2 = _qnaApiClient.GetAnswerByTag(ApplicationId, RoatpWorkflowQuestionTags.FinishCOA2Supporting).GetAwaiter().GetResult();
                    //conditionsOfAcceptance3 = _qnaApiClient.GetAnswerByTag(ApplicationId, RoatpWorkflowQuestionTags.FinishCOA3Supporting).GetAwaiter().GetResult();
                    return TaskListSectionStatus.NotRequired;
                }

                if (string.IsNullOrWhiteSpace(conditionsOfAcceptance2?.Value) /*&& String.IsNullOrWhiteSpace(conditionsOfAcceptance3?.Status)*/)
                {
                    return TaskListSectionStatus.Next;
                }

                if (conditionsOfAcceptance2?.Value == ConfirmedAnswer /*&& conditionsOfAcceptance3?.Status == ConfirmedAnswer*/)
                {
                    return TaskListSectionStatus.Completed;
                }

                return TaskListSectionStatus.InProgress;
            }

            return TaskListSectionStatus.Next;
        }

        public bool PreviousSectionCompleted(int sequenceId, int sectionId)
        {
            var sequence = ApplicationSequences.FirstOrDefault(x => x.SequenceId == sequenceId);

            if (sequenceId == RoatpWorkflowSequenceIds.YourOrganisation)
            {
                switch(sectionId)
                {
                    case RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed:
                        {
                            return true;                            
                        }
                    case RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails:
                        {
                            return (SectionStatus(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed)
                                    == TaskListSectionStatus.Completed);                            
                        }
                    case RoatpWorkflowSectionIds.YourOrganisation.WhosInControl:
                        {
                            return (SectionStatus(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed)
                                    == TaskListSectionStatus.Completed)
                                    && (SectionStatus(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails)
                                    == TaskListSectionStatus.Completed);                            
                        }
                    case RoatpWorkflowSectionIds.YourOrganisation.DescribeYourOrganisation:
                        {
                            return (SectionStatus(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed)
                                    == TaskListSectionStatus.Completed)
                                    && (SectionStatus(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails)
                                    == TaskListSectionStatus.Completed)
                                    && (WhosInControlSectionStatus == TaskListSectionStatus.Completed);                            
                        }
                    case RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations:
                        {
                            return (SectionStatus(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed)
                                    == TaskListSectionStatus.Completed)
                                    && (SectionStatus(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails)
                                    == TaskListSectionStatus.Completed)
                                    && (WhosInControlSectionStatus == TaskListSectionStatus.Completed)
                                    && (SectionStatus(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.DescribeYourOrganisation)
                                    == TaskListSectionStatus.Completed);                            
                        }
                }
            }

            if (sequenceId == RoatpWorkflowSequenceIds.Finish)
            {
                if (sectionId == 1)
                {
                    return true;
                }

                // TODO: APR-1193 - I have added the check for the case of section 9.3 being NotRequired for Supporting Provider
                if (ApplicationRouteId == SupportingApplicationRouteId && sectionId == 4)
                {
                    return (FinishSectionStatus(sectionId - 2) == TaskListSectionStatus.Completed);
                }
                else
                {
                    return (FinishSectionStatus(sectionId - 1) == TaskListSectionStatus.Completed);
                }
            }

            return RoatpTaskListWorkflowService.PreviousSectionCompleted(sequence, sectionId, sequence.Sequential);
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
                    var sectionStatus = RoatpTaskListWorkflowService.SectionStatus(ApplicationSequences, NotRequiredOverrides, RoatpWorkflowSequenceIds.YourOrganisation, section.SectionId, ApplicationRouteId);
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
                    var sectionStatus = SectionStatus(sequence.SequenceId, section.SectionId);
                    if (sectionStatus != TaskListSectionStatus.NotRequired && sectionStatus != TaskListSectionStatus.Completed)
                    {
                        return false;
                    }
                }
            }

            return true;
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

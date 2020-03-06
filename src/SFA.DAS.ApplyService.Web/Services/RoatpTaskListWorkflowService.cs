using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MoreLinq;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Web.Configuration;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Services
{
    public class RoatpTaskListWorkflowService : IRoatpTaskListWorkflowService
    {
        private const string ConfirmedAnswer = "Yes";
        
        private readonly IQnaApiClient _qnaApiClient;
        private readonly IRoatpOrganisationVerificationService _organisationVerificationService;
        private readonly INotRequiredOverridesService _notRequiredOverridesService;
        private readonly List<TaskListConfiguration> _configuration;
        
        public RoatpTaskListWorkflowService(IQnaApiClient qnaApiClient, IRoatpOrganisationVerificationService organisationVerificationService,
                                            INotRequiredOverridesService notRequiredOverridesService, IOptions<List<TaskListConfiguration>> configuration)
        {
            _qnaApiClient = qnaApiClient;
            _organisationVerificationService = organisationVerificationService;
            _notRequiredOverridesService = notRequiredOverridesService;
            _configuration = configuration.Value;
        }

        public string SectionStatus(Guid applicationId, int sequenceId, int sectionId, List<ApplicationSequence> applicationSequences)
        {        
            
            if (sequenceId == RoatpWorkflowSequenceIds.YourOrganisation
                && sectionId == RoatpWorkflowSectionIds.YourOrganisation.WhosInControl)
            {
                return WhosInControlSectionStatus(applicationId, applicationSequences);
            }

            if (sequenceId == RoatpWorkflowSequenceIds.Finish)
            {
                return FinishSectionStatus(applicationId, sectionId, applicationSequences);
            }

            var sequence = applicationSequences?.FirstOrDefault(x => (int)x.SequenceId == sequenceId);

            var section = sequence?.Sections?.FirstOrDefault(x => x.SectionId == sectionId);
                                   
            if (section == null)
            {
                return string.Empty;
            }
            
            var notRequiredOverrides = _notRequiredOverridesService.GetNotRequiredOverrides(applicationId);

            if (notRequiredOverrides!=null && notRequiredOverrides.Any(condition => 
                                                          condition.AllConditionsMet &&
                                                          sectionId == condition.SectionId &&
                                                          sequenceId == condition.SequenceId))
            {
                return TaskListSectionStatus.NotRequired;
            }

            if (!PreviousSectionCompleted(applicationId, sequence.SequenceId, sectionId, applicationSequences))
            {
                return string.Empty;
            }

            var questionsCompleted = SectionCompletedQuestionsCount(section);
                        
            var sectionText = GetSectionText(questionsCompleted, section, sequence.Sequential);
            
            return sectionText;
        }

        public bool PreviousSectionCompleted(Guid applicationId, int sequenceId, int sectionId, List<ApplicationSequence> applicationSequences)
        {
            var sequence = applicationSequences.FirstOrDefault(x => x.SequenceId == sequenceId);

            if (sequence.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation)
            {
                switch (sectionId)
                {
                    case RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed:
                        {
                            return true;
                        }
                    case RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails:
                        {
                            return (SectionStatus(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed, applicationSequences)
                                    == TaskListSectionStatus.Completed);
                        }
                    case RoatpWorkflowSectionIds.YourOrganisation.WhosInControl:
                        {
                            return (SectionStatus(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed, applicationSequences)
                                    == TaskListSectionStatus.Completed)
                                    && (SectionStatus(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails, applicationSequences)
                                    == TaskListSectionStatus.Completed);
                        }
                    case RoatpWorkflowSectionIds.YourOrganisation.DescribeYourOrganisation:
                        {
                            return (SectionStatus(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed, applicationSequences)
                                    == TaskListSectionStatus.Completed)
                                    && (SectionStatus(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails, applicationSequences)
                                    == TaskListSectionStatus.Completed)
                                    && (SectionStatus(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, applicationSequences) == TaskListSectionStatus.Completed);
                        }
                    case RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations:
                        {
                            return (SectionStatus(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed, applicationSequences)
                                    == TaskListSectionStatus.Completed)
                                    && (SectionStatus(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails, applicationSequences)
                                    == TaskListSectionStatus.Completed)
                                    && (SectionStatus(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, applicationSequences) == TaskListSectionStatus.Completed)
                                    && (SectionStatus(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.DescribeYourOrganisation, applicationSequences)
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
                var previousSectionStatus = SectionStatus(applicationId, sequenceId, sectionId - 1, applicationSequences); 
                
                return (previousSectionStatus == TaskListSectionStatus.Completed ||  previousSectionStatus == TaskListSectionStatus.NotRequired);
            }

            if (sequence.Sequential && sectionId > 1)
            {
                var previousSection = sequence.Sections.FirstOrDefault(x => x.SectionId == (sectionId - 1));
                if (previousSection == null)
                {
                    return false;
                }

                if (previousSection.PagesActive == previousSection.PagesComplete && previousSection.PagesComplete > 0)
                {
                    return true;
                }

                var previousSectionsCompletedCount = SectionCompletedQuestionsCount(previousSection);
                if (previousSectionsCompletedCount == 0)
                    return false;                               

                var previousSectionQuestionsCount = previousSection.QnAData.Pages.Where(p => p.NotRequired == false).SelectMany(x => x.Questions)
                    .DistinctBy(q => q.QuestionId).Count();
                if (previousSectionsCompletedCount < previousSectionQuestionsCount)
                {
                    return false;
                }
            }

            return true;
        }

        public List<ApplicationSequence> GetApplicationSequences(Guid applicationId)
        {
            var sequences = _qnaApiClient.GetSequences(applicationId).GetAwaiter().GetResult();

            PopulateAdditionalSequenceFields(sequences);

            var filteredSequences = sequences.Where(x => x.SequenceId != RoatpWorkflowSequenceIds.Preamble && x.SequenceId != RoatpWorkflowSequenceIds.ConditionsOfAcceptance).OrderBy(y => y.SequenceId);

            foreach (var sequence in filteredSequences)
            {
                var sections = _qnaApiClient.GetSections(applicationId, sequence.Id).GetAwaiter().GetResult();
                sequence.Sections = sections.ToList();
            }

            return filteredSequences.ToList();
        }
        
        private void PopulateAdditionalSequenceFields(IEnumerable<ApplicationSequence> sequences)
        {
            foreach (var sequence in sequences)
            {
                var selectedSequence = _configuration.FirstOrDefault(x => x.Id == sequence.SequenceId);
                if (selectedSequence != null)
                {
                    sequence.Description = selectedSequence.Title;
                    sequence.Sequential = selectedSequence.Sequential;
                }
            }
        }
        
        private static int SectionCompletedQuestionsCount(ApplicationSection section)
        {
            int answeredQuestions = 0;
            
            var pages = section.QnAData.Pages.Where(p => p.NotRequired == false);
            foreach (var page in pages)
            {
                var questionIds = page.Questions.Select(x => x.QuestionId);
                foreach (var questionId in questionIds)
                {
                    foreach (var pageOfAnswers in page.PageOfAnswers)
                    {
                        var matchedAnswer = pageOfAnswers.Answers.FirstOrDefault(y => y.QuestionId == questionId);
                        if (matchedAnswer != null && !String.IsNullOrEmpty(matchedAnswer.Value))
                        {
                            answeredQuestions++;
                            break;
                        }
                    }
                }
            }

            return answeredQuestions;
        }

        private static string GetSectionText(int completedCount, ApplicationSection section,  bool sequential)
        {
            var pagesCompleted = section.QnAData.Pages.Count(x => x.Complete);
            var pagesActive = section.QnAData.Pages.Count(x => x.Active);

            if ((section.PagesComplete == section.PagesActive && section.PagesActive > 0))
                return TaskListSectionStatus.Completed;

            if (sequential && completedCount == 0)
            {
                return TaskListSectionStatus.Next;
            }

            if (completedCount > 0)
            {
                return TaskListSectionStatus.InProgress;
            }

            return TaskListSectionStatus.Blank;

        }


        private string WhosInControlSectionStatus(Guid applicationId, List<ApplicationSequence> applicationSequences)
        {
            
                if (SectionStatus(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails, applicationSequences) == TaskListSectionStatus.Blank)
                {
                    return TaskListSectionStatus.Blank;
                }

                var organisationVerificationStatus = _organisationVerificationService.GetOrganisationVerificationStatus(applicationId).GetAwaiter().GetResult();

                if (organisationVerificationStatus.VerifiedCompaniesHouse
                    && organisationVerificationStatus.VerifiedCharityCommission)
                {
                    if ((organisationVerificationStatus.CompaniesHouseDataConfirmed 
                        && !organisationVerificationStatus.CharityCommissionDataConfirmed)
                        || (!organisationVerificationStatus.CompaniesHouseDataConfirmed
                        && organisationVerificationStatus.CharityCommissionDataConfirmed))
                    {
                        return TaskListSectionStatus.InProgress;
                    }
                    if (organisationVerificationStatus.CompaniesHouseDataConfirmed
                        && organisationVerificationStatus.CharityCommissionDataConfirmed)
                    {
                        return TaskListSectionStatus.Completed;
                    }
                }

                if (organisationVerificationStatus.VerifiedCompaniesHouse
                    && !organisationVerificationStatus.VerifiedCharityCommission)
                {
                    if (organisationVerificationStatus.CompaniesHouseDataConfirmed)
                    {
                        return TaskListSectionStatus.Completed;
                    }
                }

                if (!organisationVerificationStatus.VerifiedCompaniesHouse
                    && organisationVerificationStatus.VerifiedCharityCommission)
                {
                    if (organisationVerificationStatus.CharityCommissionDataConfirmed)
                    {
                        return TaskListSectionStatus.Completed;
                    }
                }

                if (organisationVerificationStatus.WhosInControlConfirmed)
                {
                    return TaskListSectionStatus.Completed;
                }

                return TaskListSectionStatus.Next;
            
        }
        
        private string FinishSectionStatus(Guid applicationId, int sectionId, List<ApplicationSequence> applicationSequences)
        {
            if (!ApplicationSequencesCompleted(applicationId, applicationSequences))
            {
                return TaskListSectionStatus.Blank;
            }
            var finishSequence = applicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.Finish);

            // TODO: APR-1193 We are calling PreviousSectionCompleted() from FinishSequence.cshtml, then it calls this FinishSectionStatus() method, 
            // which calls PreviousSectionCompleted() again with the code bellow. 
            //if (!PreviousSectionCompleted(finishSequence.SequenceId, sectionId))
            //{
            //    return TaskListSectionStatus.Blank;
            //}

            if (sectionId == RoatpWorkflowSectionIds.Finish.CommercialInConfidenceInformation)
            {
                var commercialInConfidenceAnswer = _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.FinishCommercialInConfidence).GetAwaiter().GetResult();
                if (commercialInConfidenceAnswer != null && !String.IsNullOrWhiteSpace(commercialInConfidenceAnswer.Value))
                {
                    // Section 9.2 handled InProgress State
                    if (commercialInConfidenceAnswer.Value.Equals("yes", StringComparison.InvariantCultureIgnoreCase))
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
                var permissionPersonalDetails = _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.FinishPermissionPersonalDetails).GetAwaiter().GetResult();
                var accuratePersonalDetails = _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.FinishAccuratePersonalDetails).GetAwaiter().GetResult();
                var permissionSubmitApplication = _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.FinishPermissionSubmitApplication).GetAwaiter().GetResult();

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

                //if (ApplicationRouteId == MainApplicationRouteId || ApplicationRouteId == EmployerApplicationRouteId)
                //{
                //    conditionsOfAcceptance2 = _qnaApiClient.GetAnswerByTag(ApplicationId, RoatpWorkflowQuestionTags.FinishCOA2MainEmployer).GetAwaiter().GetResult();
                //    //conditionsOfAcceptance3 = _qnaApiClient.GetAnswerByTag(ApplicationId, RoatpWorkflowQuestionTags.FinishCOA3MainEmployer).GetAwaiter().GetResult();
                //}
                //else if (ApplicationRouteId == SupportingApplicationRouteId)
                //{
                //    // TODO: TODO: APR-1193 - This is the return value for 9.3 section for Supporting Provider 
                //    //conditionsOfAcceptance2 = _qnaApiClient.GetAnswerByTag(ApplicationId, RoatpWorkflowQuestionTags.FinishCOA2Supporting).GetAwaiter().GetResult();
                //    //conditionsOfAcceptance3 = _qnaApiClient.GetAnswerByTag(ApplicationId, RoatpWorkflowQuestionTags.FinishCOA3Supporting).GetAwaiter().GetResult();
                //    return TaskListSectionStatus.NotRequired;
                //}

                if (string.IsNullOrWhiteSpace(conditionsOfAcceptance2?.Value) /*&& String.IsNullOrWhiteSpace(conditionsOfAcceptance3?.Value)*/)
                {
                    return TaskListSectionStatus.Next;
                }

                if (conditionsOfAcceptance2?.Value == ConfirmedAnswer /*&& conditionsOfAcceptance3?.Value == ConfirmedAnswer*/)
                {
                    return TaskListSectionStatus.Completed;
                }

                return TaskListSectionStatus.InProgress;
            }

            return TaskListSectionStatus.Next;
        }

        public bool ApplicationSequencesCompleted(Guid applicationId, List<ApplicationSequence> applicationSequences)
        {
            var nonFinishSequences = applicationSequences.Where(seq => seq.SequenceId != RoatpWorkflowSequenceIds.Finish);
            foreach (var sequence in nonFinishSequences)
            {
                foreach (var section in sequence.Sections)
                {
                    var sectionStatus = SectionStatus(applicationId, sequence.SequenceId, section.SectionId, applicationSequences);
                    if (sectionStatus != TaskListSectionStatus.NotRequired && sectionStatus != TaskListSectionStatus.Completed)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}

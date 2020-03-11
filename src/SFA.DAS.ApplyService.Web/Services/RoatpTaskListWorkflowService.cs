﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoreLinq;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.Web.Configuration;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Services
{
    public class RoatpTaskListWorkflowService : IRoatpTaskListWorkflowService
    {
        private const string ConfirmedAnswer = "Yes";
        
        private readonly IQnaApiClient _qnaApiClient;
        private readonly INotRequiredOverridesService _notRequiredOverridesService;
        private readonly List<TaskListConfiguration> _configuration;
        private readonly ILogger<RoatpTaskListWorkflowService> _logger;
        
        public RoatpTaskListWorkflowService(IQnaApiClient qnaApiClient, INotRequiredOverridesService notRequiredOverridesService, 
                                            IOptions<List<TaskListConfiguration>> configuration, ILogger<RoatpTaskListWorkflowService> logger)
        {
            _qnaApiClient = qnaApiClient;
            _notRequiredOverridesService = notRequiredOverridesService;
            _configuration = configuration.Value;
            _logger = logger;
        }

        public string SectionStatus(Guid applicationId, int sequenceId, int sectionId, 
                                    IEnumerable<ApplicationSequence> applicationSequences, OrganisationVerificationStatus organisationVerificationStatus)
        {
            //_logger.LogDebug($"Getting section status for application {applicationId} sequence {sequenceId} section {sectionId}");
            
            var notRequiredOverrides = _notRequiredOverridesService.GetNotRequiredOverrides(applicationId);

            if (notRequiredOverrides != null && notRequiredOverrides.Any(condition =>
                                                            condition.AllConditionsMet &&
                                                            sectionId == condition.SectionId &&
                                                            sequenceId == condition.SequenceId))
            {
                return TaskListSectionStatus.NotRequired;
            }

            if (sequenceId == RoatpWorkflowSequenceIds.YourOrganisation
                && sectionId == RoatpWorkflowSectionIds.YourOrganisation.WhosInControl)
            {
                return WhosInControlSectionStatus(applicationId, applicationSequences, organisationVerificationStatus);
            }

            var sequence = applicationSequences?.FirstOrDefault(x => (int)x.SequenceId == sequenceId);

            var section = sequence?.Sections?.FirstOrDefault(x => x.SectionId == sectionId);
                                   
            if (section == null)
            {
                return string.Empty;
            }

            if (!PreviousSectionCompleted(applicationId, sequence.SequenceId, sectionId, applicationSequences, organisationVerificationStatus))
            {
                return string.Empty;
            }

            var questionsCompleted = SectionCompletedQuestionsCount(section);
                        
            var sectionText = GetSectionText(questionsCompleted, section, sequence.Sequential);
            
            return sectionText;
        }

        public bool PreviousSectionCompleted(Guid applicationId, int sequenceId, int sectionId, IEnumerable<ApplicationSequence> applicationSequences, OrganisationVerificationStatus organisationVerificationStatus)
        {
            var sequence = applicationSequences.FirstOrDefault(x => x.SequenceId == sequenceId);

            if (sequence.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation)
            {
                var complete = true;
                for(var index = 1; index < sectionId; index++)
                {
                    if (SectionStatus(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, index, applicationSequences, organisationVerificationStatus) != TaskListSectionStatus.Completed)
                    {
                        complete = false;
                        break;
                    }
                }
                return complete;
            }

            if (sequenceId == RoatpWorkflowSequenceIds.Finish)
            {
                if (sectionId == 1)
                {
                    return true;
                }
                var previousSectionStatus = SectionStatus(applicationId, sequenceId, sectionId - 1, applicationSequences, organisationVerificationStatus); 
                
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

        public string FinishSectionStatus(Guid applicationId, int sectionId, IEnumerable<ApplicationSequence> applicationSequences, bool applicationSequencesCompleted)
        {
            if (!applicationSequencesCompleted)
            {
                return TaskListSectionStatus.Blank;
            }
            var finishSequence = applicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.Finish);

            var notRequiredOverrides = _notRequiredOverridesService.GetNotRequiredOverrides(applicationId);

            if (notRequiredOverrides != null && notRequiredOverrides.Any(condition =>
                                                            condition.AllConditionsMet &&
                                                            sectionId == condition.SectionId &&
                                                            finishSequence.SequenceId == condition.SequenceId))
            {
                return TaskListSectionStatus.NotRequired;
            }

            var finishSection = _qnaApiClient.GetSectionBySectionNo(applicationId, RoatpWorkflowSequenceIds.Finish, sectionId).GetAwaiter().GetResult();

            var sectionPages = finishSection.QnAData.Pages.Count();
            var completedCount = 0;
            var pagesWithAnswers = 0;
            foreach (var page in finishSection.QnAData.Pages)
            {
                if (page.PageOfAnswers != null && page.PageOfAnswers.Any())
                {
                    pagesWithAnswers++;
                    var pageofAnswers = page.PageOfAnswers.FirstOrDefault();
                    foreach (var answer in pageofAnswers.Answers)
                    {
                        if (answer.Value == ConfirmedAnswer)
                        {
                            completedCount++;
                        }
                    }
                }
            }
            if (completedCount == sectionPages)
            {
                return TaskListSectionStatus.Completed;
            }
            if (completedCount < sectionPages && pagesWithAnswers > 0)
            {
                return TaskListSectionStatus.InProgress;
            }

            return TaskListSectionStatus.Next;
        }

        public IEnumerable<ApplicationSequence> GetApplicationSequences(Guid applicationId)
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


        private string WhosInControlSectionStatus(Guid applicationId, IEnumerable<ApplicationSequence> applicationSequences, OrganisationVerificationStatus organisationVerificationStatus)
        {
            
                if (SectionStatus(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails, applicationSequences, organisationVerificationStatus) == TaskListSectionStatus.Blank)
                {
                    return TaskListSectionStatus.Blank;
                }

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
       
        

    }
}

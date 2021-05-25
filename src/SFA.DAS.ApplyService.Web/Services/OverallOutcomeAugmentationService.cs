using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Services.Assessor;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Services
{
    public class OverallOutcomeAugmentationService : IOverallOutcomeAugmentationService
    {
        private readonly IApplicationApiClient _apiClient;
        private readonly IQnaApiClient _qnaApiClient;
        private readonly IAssessorLookupService _assessorLookupService;

        public OverallOutcomeAugmentationService(IApplicationApiClient apiClient, IQnaApiClient qnaApiClient,
            IAssessorLookupService assessorLookupService)
        {
            _apiClient = apiClient;
            _qnaApiClient = qnaApiClient;
            _assessorLookupService = assessorLookupService;
        }

        public async Task AugmentModelWithModerationFailDetails(ApplicationSummaryWithModeratorDetailsViewModel model,
            string userId)
        {
            var sequences = await _apiClient.GetClarificationSequences(model.ApplicationId);
            var passFailDetails = await _apiClient.GetAllClarificationPageReviewOutcomes(model.ApplicationId, userId);
            var moderationFailedDetails = passFailDetails.Where(x => x.ModeratorReviewStatus == ModerationStatus.Fail).ToList();

            if (moderationFailedDetails.Any())
            {
                AddPagesToSequencesFromFailedDetails(sequences, moderationFailedDetails);

                var sequencesWithModerationFails = new List<AssessorSequence>();
                BuildSequencesWithModerationFails(sequences, sequencesWithModerationFails);

                var allSections = await _qnaApiClient.GetSections(model.ApplicationId);

                RemoveInactiveOrEmptyPagesSectionsSequencesFromSequences(sequencesWithModerationFails, allSections);
                AddPageTitlesToSequences(sequencesWithModerationFails);
                AddAnswersToSequences(sequencesWithModerationFails, allSections);
                AddQuestionsToSequences(sequencesWithModerationFails, allSections);
                AddSequenceTitlesToSequences(sequencesWithModerationFails);

                model.Sequences = sequencesWithModerationFails;
                model.PagesWithGuidance =
                    GatherGuidancePagesForSequenceQuestions(sequencesWithModerationFails, allSections);
            }
        }

        private void AddSequenceTitlesToSequences(List<AssessorSequence> sequencesWithModerationFails)
        {
            foreach (var sequence in sequencesWithModerationFails)
            {
                sequence.SequenceTitle =
                    _assessorLookupService.GetTitleForSequence(sequence.SequenceNumber)?.Replace("checks",string.Empty,StringComparison.InvariantCultureIgnoreCase).Trim();
            }
        }

        private void AddQuestionsToSequences(List<AssessorSequence> sequencesWithModerationFails,
            IEnumerable<ApplicationSection> allSections)
        {
            foreach (var sequence in sequencesWithModerationFails)
            {
                foreach (var section in sequence.Sections)
                {
                    foreach (var page in section.Pages)
                    {
                        var selectedSection = allSections.FirstOrDefault(x =>
                            x.SequenceId == section.SequenceNumber && x.SectionId == section.SectionNumber);

                        var pageDetails =
                            selectedSection?.QnAData?.Pages?.SingleOrDefault(p => p.PageId == page.PageId);

                        if (pageDetails != null && pageDetails.Active)
                        {
                            page.Questions = GatherAndTypePageQuestions(pageDetails);
                        }
                    }
                }
            }
        }

        private List<PageWithGuidance> GatherGuidancePagesForSequenceQuestions(
            List<AssessorSequence> sequencesWithModerationFails, IEnumerable<ApplicationSection> allSections)
        {
            var guidancePages = new List<PageWithGuidance>();
            foreach (var sequence in sequencesWithModerationFails)
            {
                foreach (var section in sequence.Sections)
                {
                    foreach (var page in section.Pages)
                    {
                        var selectedSection = allSections.FirstOrDefault(x =>
                            x.SequenceId == section.SequenceNumber && x.SectionId == section.SectionNumber);

                        var pageDetails =
                            selectedSection?.QnAData?.Pages?.SingleOrDefault(p => p.PageId == page.PageId);

                        if (pageDetails != null && pageDetails.Active)
                        {
                            var guidancePage = new PageWithGuidance
                            {
                                PageId = page.PageId,
                                GuidanceInformation = GetGuidanceInformation(pageDetails)
                            };

                            guidancePages.Add(guidancePage);
                        }
                    }
                }
            }

            return guidancePages;
        }

        private static void AddAnswersToSequences(List<AssessorSequence> sequencesWithModerationFails,
            IEnumerable<ApplicationSection> allSections)
        {
            foreach (var sequence in sequencesWithModerationFails)
            {
                foreach (var section in sequence.Sections)
                {
                    foreach (var page in section.Pages)
                    {
                        var selectedSection = allSections.FirstOrDefault(x =>
                            x.SequenceId == section.SequenceNumber && x.SectionId == section.SectionNumber);

                        var pageDetails =
                            selectedSection?.QnAData?.Pages?.SingleOrDefault(p => p.PageId == page.PageId);

                        if (pageDetails != null && pageDetails.Active)
                        {
                            page.PageOfAnswers = pageDetails.PageOfAnswers;
                        }
                    }
                }
            }
        }

        private void AddPageTitlesToSequences(List<AssessorSequence> sequencesWithModerationFails)
        {
            foreach (var sequence in sequencesWithModerationFails)
            {
                foreach (var section in sequence.Sections)
                {
                    foreach (var page in section.Pages)
                    {
                        page.Title = GetTitleForPage(page);
                    }
                }
            }
        }

        private static void RemoveInactiveOrEmptyPagesSectionsSequencesFromSequences(List<AssessorSequence> sequencesWithModerationFails,
            IEnumerable<ApplicationSection> allSections)
        {
            var sequencesToRemove = new List<AssessorSequence>();

            foreach (var sequence in sequencesWithModerationFails)
            {
                var sectionsToRemove = new List<AssessorSection>();
                foreach (var section in sequence.Sections)
                {
                    var pagesToRemove = new List<Page>();

                    foreach (var page in section.Pages)
                    {
                        var selectedSection = allSections.FirstOrDefault(x =>
                            x.SequenceId == section.SequenceNumber && x.SectionId == section.SectionNumber);

                        var pageDetails =
                            selectedSection?.QnAData?.Pages?.SingleOrDefault(p => p.PageId == page.PageId);
                        if (pageDetails == null || !pageDetails.Active)
                        {
                            pagesToRemove.Add(page);
                        }
                    }

                    if (pagesToRemove.Any())
                    {
                        foreach (var pageToRemove in pagesToRemove)
                            section.Pages.Remove(pageToRemove);
                    }

                    if (section.Pages.Count==0)
                        sectionsToRemove.Add(section);
                }

                if (sectionsToRemove.Any())
                {
                    foreach (var sectionToRemove in sectionsToRemove)
                    {
                        sequence.Sections.Remove(sectionToRemove);
                    }
                }

                if (sequence.Sections.Count == 0)
                {
                    sequencesToRemove.Add(sequence);
                }
            }

            if (sequencesToRemove.Any())
            {
                foreach (var sequence in sequencesToRemove)
                {
                    sequencesWithModerationFails.Remove(sequence);
                }
            }
        }

        private static void BuildSequencesWithModerationFails(List<AssessorSequence> sequences,
            List<AssessorSequence> sequencesWithModerationFails)
        {
            foreach (var sequence in sequences)
            {
                foreach (var section in sequence.Sections)
                {
                    if (section.Pages != null)
                    {
                        if (sequencesWithModerationFails.All(x => x.SequenceNumber != sequence.SequenceNumber))
                            sequencesWithModerationFails.Add(new AssessorSequence
                            {
                                SequenceNumber = sequence.SequenceNumber,
                                SequenceTitle = sequence.SequenceTitle,
                                Sections = new List<AssessorSection>()
                            });

                        sequencesWithModerationFails.FirstOrDefault(x => x.SequenceNumber == section.SequenceNumber)
                            ?.Sections.Add(section);
                    }
                }
            }
        }

        private static void AddPagesToSequencesFromFailedDetails(List<AssessorSequence> sequences, List<ClarificationPageReviewOutcome> failedDetails)
        {
            foreach (var sequence in sequences)
            {
                foreach (var section in sequence.Sections)
                {
                    foreach (var outcome in failedDetails)
                    {
                        if (outcome.SequenceNumber == section.SequenceNumber &&
                            outcome.SectionNumber == section.SectionNumber)
                        {
                            if (section.Pages==null)
                                section.Pages = new List<Page>();

                            if (section.Pages.All(x => x.PageId != outcome.PageId))
                            {
                                section.Pages.Add(new Page {PageId = outcome.PageId, Active = true});
                            }
                        }
                    }
                }
            }
        }


        private string GetTitleForPage(Page page)
        {
            var title = _assessorLookupService.GetTitleForPage(page.PageId);
            if (string.IsNullOrEmpty(title))
            {
                title = _assessorLookupService.GetSectorNameForPage(page.PageId);
            }

            return title;
        }


        private List<Question> GatherAndTypePageQuestions(Page pageDetails)
        {
            foreach (var q in pageDetails.Questions)
            {
                if (string.Equals(q.Input.Type, QuestionType.CheckboxList,
                    StringComparison.CurrentCultureIgnoreCase))
                    q.Input.Type = QuestionType.CheckboxList;

                if (string.Equals(q.Input.Type, QuestionType.ComplexCheckboxList,
                    StringComparison.CurrentCultureIgnoreCase))
                    q.Input.Type = QuestionType.ComplexCheckboxList;

                if (string.Equals(q.Input.Type, QuestionType.Checkbox,
                    StringComparison.CurrentCultureIgnoreCase))
                    q.Input.Type = QuestionType.Checkbox;
            }

            return pageDetails?.Questions;
        }


        private static List<string> GetGuidanceInformation(Page page)
        {
            var guidanceInformation = new List<string>();

            if (page == null) return guidanceInformation;

            if (!string.IsNullOrEmpty(page.BodyText))
            {
                guidanceInformation.Add(page.BodyText);
            }

            if (page.Questions != null)
            {
                foreach (var question in page.Questions)
                {
                    if (!string.IsNullOrEmpty(question.QuestionBodyText))
                    {
                        guidanceInformation.Add(question.QuestionBodyText);
                    }
                }
            }

            return guidanceInformation;
        }

    }

    public interface IOverallOutcomeAugmentationService
    {
         Task AugmentModelWithModerationFailDetails(ApplicationSummaryWithModeratorDetailsViewModel model,
            string userId);
    }
}

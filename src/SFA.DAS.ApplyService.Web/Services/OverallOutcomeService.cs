using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Application.Services.Assessor;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Services
{
    public class OverallOutcomeService : IOverallOutcomeService
    {
        private readonly IOutcomeApiClient _apiClient;
        private readonly IQnaApiClient _qnaApiClient;
        private readonly IAssessorLookupService _assessorLookupService;

        public OverallOutcomeService(IOutcomeApiClient apiClient, IQnaApiClient qnaApiClient,
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
            var moderationFailedDetails = passFailDetails.Where(x => x.Status == ModerationStatus.Fail 
                                                                     || (x.Status==null && x.ModeratorReviewStatus==ModerationStatus.Fail)).ToList();

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
                model.PagesWithClarifications = GatherClarificationPages(moderationFailedDetails);
            }
        }

        private static List<ClarificationPage> GatherClarificationPages(List<ClarificationPageReviewOutcome> moderationFailedDetails)
        {
            if (moderationFailedDetails == null) return new List<ClarificationPage>();

            return moderationFailedDetails.Where(x => x.Status != null)
                .Select(page => new ClarificationPage
                {
                    SequenceNumber = page.SequenceNumber,
                    SectionNumber = page.SectionNumber,
                    PageId = page.PageId,
                    ClarificationResponse = page.ClarificationResponse,
                    ClarificationFile = page.ClarificationFile
                })
                .ToList();
        }

        public ApplicationSummaryViewModel BuildApplicationSummaryViewModel(Apply application, string emailAddress)
            {
                var applicationData = application.ApplyData.ApplyDetails;

                var model = new ApplicationSummaryViewModel
                {
                    ApplicationId = application.ApplicationId,
                    UKPRN = applicationData.UKPRN,
                    OrganisationName = applicationData.OrganisationName,
                    TradingName = applicationData.TradingName,
                    ApplicationRouteId = applicationData.ProviderRoute.ToString(),
                    ApplicationReference = applicationData.ReferenceNumber,
                    SubmittedDate = applicationData?.ApplicationSubmittedOn,
                    ExternalComments = application.ExternalComments ?? application.ApplyData.GatewayReviewDetails?.ExternalComments,
                    EmailAddress = emailAddress,
                    FinancialReviewStatus = application?.FinancialReviewStatus,
                    FinancialGrade = application?.FinancialGrade?.SelectedGrade,
                    FinancialExternalComments = application?.FinancialGrade?.ExternalComments,
                    GatewayReviewStatus = application?.GatewayReviewStatus,
                    ModerationStatus = application?.ModerationStatus
                };
                return model;
            }

        public async Task<ApplicationSummaryWithModeratorDetailsViewModel> BuildApplicationSummaryViewModelWithModerationDetails(Apply application, string emailAddress)
        {
            var applicationData = application.ApplyData.ApplyDetails;

            var oversightReview = await _apiClient.GetOversightReview(application.ApplicationId);

            var applicationUnsuccessfulModerationFail = false;
            var applicationUnsuccessfulModerationPassOverturned = false;
            var applicationUnsuccessfulModerationPassAndApproved = false;
            if (application?.GatewayReviewStatus == GatewayAnswerStatus.Pass)
            {
                if (application?.ModerationStatus != null
                    && application?.ModerationStatus == ModerationStatus.Fail
                    && oversightReview.ModerationApproved.HasValue
                    && oversightReview.ModerationApproved == true)
                {
                    applicationUnsuccessfulModerationFail = true;
                }

                if (application?.ModerationStatus != null
                    && application.ModerationStatus == ModerationStatus.Pass)
                {
                    if (oversightReview.ModerationApproved.HasValue && oversightReview.ModerationApproved == false)
                    {
                        applicationUnsuccessfulModerationPassOverturned = true;
                    }

                    if (oversightReview.ModerationApproved.HasValue && oversightReview.ModerationApproved == true)
                    {
                        applicationUnsuccessfulModerationPassAndApproved = true;
                    }
                }
            }

            var model = new ApplicationSummaryWithModeratorDetailsViewModel
            {
                ApplicationId = application.ApplicationId,
                UKPRN = applicationData.UKPRN,
                OrganisationName = applicationData.OrganisationName,
                TradingName = applicationData.TradingName,
                ApplicationRouteId = applicationData.ProviderRoute.ToString(),
                ApplicationReference = applicationData.ReferenceNumber,
                SubmittedDate = applicationData?.ApplicationSubmittedOn,
                ExternalComments = application?.ApplyData?.GatewayReviewDetails?.ExternalComments,
                EmailAddress = emailAddress,
                FinancialReviewStatus = application?.FinancialReviewStatus,
                FinancialGrade = application?.FinancialGrade?.SelectedGrade,
                FinancialExternalComments = application?.FinancialGrade?.ExternalComments,
                GatewayReviewStatus = application?.GatewayReviewStatus,
                ModerationStatus = application?.ModerationStatus,
                ModerationPassOverturnedToFail = false,
                ModerationPassAndApproved = applicationUnsuccessfulModerationPassAndApproved
            };

            if (applicationUnsuccessfulModerationPassOverturned)
            {
                model.ModerationPassOverturnedToFail = true;
                model.OversightExternalComments = oversightReview.ExternalComments;
            }
            if (applicationUnsuccessfulModerationFail)
            {
                await AugmentModelWithModerationFailDetails(model,
                   emailAddress);
            }

            return model;
        }

        public async Task<OutcomeSectorDetailsViewModel> GetSectorDetailsViewModel(Guid applicationId, string pageId)
        {
            var sectorDetails = await _apiClient.GetClarificationSectorDetails(applicationId, pageId);
            var model = new OutcomeSectorDetailsViewModel
            {
                ApplicationId = applicationId, 
                SectorDetails = sectorDetails
            };
            return model;
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
            foreach (var section in sequencesWithModerationFails.SelectMany(sequence => sequence.Sections))
            {
                var selectedSection = GetSectionForSequenceNumberSectionNumber(allSections, section.SequenceNumber, section.SectionNumber);
                foreach (var page in section.Pages)
                    {
                        var pageDetails =
                            selectedSection?.QnAData?.Pages?.SingleOrDefault(p => p.PageId == page.PageId);

                        if (pageDetails != null && pageDetails.Active)
                        {
                            page.Questions = GatherAndTypePageQuestions(pageDetails);
                        }
                    }
                
            }
        }

        private List<PageWithGuidance> GatherGuidancePagesForSequenceQuestions(
            List<AssessorSequence> sequencesWithModerationFails, IEnumerable<ApplicationSection> allSections)
        {
            var guidancePages = new List<PageWithGuidance>();
            foreach (var section in sequencesWithModerationFails.SelectMany(sequence => sequence.Sections))
            {
                guidancePages.AddRange(from page in section.Pages let selectedSection = GetSectionForSequenceNumberSectionNumber(allSections, section.SequenceNumber, section.SectionNumber) 
                    let pageDetails = selectedSection?.QnAData?.Pages?.SingleOrDefault(p => p.PageId == page.PageId) where pageDetails != null && pageDetails.Active select new PageWithGuidance {PageId = page.PageId, GuidanceInformation = GetGuidanceInformation(pageDetails)});
            }

            return guidancePages;
        }

        private static void AddAnswersToSequences(List<AssessorSequence> sequencesWithModerationFails,
            IEnumerable<ApplicationSection> allSections)
        {
            foreach (var section in sequencesWithModerationFails.SelectMany(sequence => sequence.Sections))
            {
                var selectedSection = GetSectionForSequenceNumberSectionNumber(allSections, section.SequenceNumber, section.SectionNumber);
                foreach (var page in section.Pages)
                {
                    var pageDetails =
                        selectedSection?.QnAData?.Pages?.SingleOrDefault(p => p.PageId == page.PageId);

                    if (pageDetails != null && pageDetails.Active)
                    {
                        page.PageOfAnswers = pageDetails.PageOfAnswers;
                    }
                }
            }
        }

        private static ApplicationSection GetSectionForSequenceNumberSectionNumber(IEnumerable<ApplicationSection> allSections, int sequenceNumber, int sectionNumber)
        {
            return allSections.FirstOrDefault(x => x.SequenceId == sequenceNumber && x.SectionId == sectionNumber);
        }

        private void AddPageTitlesToSequences(List<AssessorSequence> sequencesWithModerationFails)
        {
            foreach (var section in sequencesWithModerationFails.SelectMany(sequence => sequence.Sections))
            {
            foreach (var page in section.Pages)
                {
                    page.Title = GetTitleForPage(page);
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
                        var selectedSection =  GetSectionForSequenceNumberSectionNumber(allSections, section.SequenceNumber, section.SectionNumber);

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
                    if (section.Pages == null) continue;
                    if (!sequencesWithModerationFails.Any(x => x.SequenceNumber == sequence.SequenceNumber))
                        sequencesWithModerationFails.Add(new AssessorSequence
                        {
                            SequenceNumber = sequence.SequenceNumber,
                            SequenceTitle = sequence.SequenceTitle,
                            Sections = new List<AssessorSection>()
                        });

                    sequencesWithModerationFails.
                        FirstOrDefault(x => x.SequenceNumber == section.SequenceNumber)?.Sections.Add(section);
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
                if (QuestionType.CheckboxList.Equals(q.Input.Type, StringComparison.CurrentCultureIgnoreCase))
                    q.Input.Type = QuestionType.CheckboxList;

                if (QuestionType.ComplexCheckboxList.Equals(q.Input.Type, StringComparison.CurrentCultureIgnoreCase))
                    q.Input.Type = QuestionType.ComplexCheckboxList;

                if (QuestionType.Checkbox.Equals(q.Input.Type, StringComparison.CurrentCultureIgnoreCase))
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
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Services.Assessor;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Controllers.Roatp
{
    [Authorize]
    public class RoatpOverallOutcomesController : Controller
    {
        private readonly IApplicationApiClient _apiClient;
        private readonly IQnaApiClient _qnaApiClient;
        private readonly IAssessorLookupService _assessorLookupService;
        private readonly ILogger<RoatpOverallOutcomesController> _logger;

        public RoatpOverallOutcomesController(IApplicationApiClient apiClient, IQnaApiClient qnaApiClient,
            IAssessorLookupService assessorLookupService, ILogger<RoatpOverallOutcomesController> logger)
        {
            _apiClient = apiClient;
            _qnaApiClient = qnaApiClient;
            _assessorLookupService = assessorLookupService;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> ApplicationRejected(Guid applicationId)
        {
            var application = await _apiClient.GetApplication(applicationId);
            var applicationData = application.ApplyData.ApplyDetails;

            var model = new ApplicationSummaryWithModeratorDetails
            {
                ApplicationId = application.ApplicationId,
                UKPRN = applicationData.UKPRN,
                OrganisationName = applicationData.OrganisationName,
                TradingName = applicationData.TradingName,
                ApplicationRouteId = applicationData.ProviderRoute.ToString(),
                ApplicationReference = applicationData.ReferenceNumber,
                SubmittedDate = applicationData?.ApplicationSubmittedOn,
                ExternalComments = application?.ApplyData?.GatewayReviewDetails?.ExternalComments,
                EmailAddress = User.GetEmail(),
                FinancialReviewStatus = application?.FinancialReviewStatus,
                FinancialGrade = application?.FinancialGrade?.SelectedGrade,
                FinancialExternalComments = application?.FinancialGrade?.ExternalComments,
                GatewayReviewStatus = application?.GatewayReviewStatus,
                ModerationStatus = application?.ModerationStatus
            };

            var oversightReview = await _apiClient.GetOversightReview(applicationId);
            // special page for pmo fail and/or moderation fail

            var guidancePages = new List<AssessorPage>();

            var applicationUnsuccessful = false;
            var failedOnModeration = false;
            if (model.GatewayReviewStatus == GatewayAnswerStatus.Pass)
            {
                if (model.ModerationStatus != null
                    && model.ModerationStatus == ModerationStatus.Fail
                    && oversightReview.ModerationApproved.HasValue
                    && oversightReview.ModerationApproved == true)
                {
                    failedOnModeration = true;
                    applicationUnsuccessful = true;
                }

                if (model.FinancialReviewStatus != null
                    && model.FinancialReviewStatus == FinancialReviewStatus.Fail)
                    applicationUnsuccessful = true;
            }

            if (applicationUnsuccessful)
            {
                if (failedOnModeration)
                {
                    // build the model
                    var sequenceDetails = await _apiClient.GetClarificationSequences(applicationId);
                    var userId = User.GetUserId().ToString();
                    var passFailDetails = await _apiClient.GetAllClarificationPageReviewOutcomes(applicationId, userId);
                    var failedDetails = passFailDetails.Where(x => x.ModeratorReviewStatus == ModerationStatus.Fail);



                    // get the failed sequence details
                    var failedSequenceDetails = new List<AssessorSequence>();
                    if (failedDetails.Any())
                    {
                        foreach (var sequence in sequenceDetails)
                        {
                            foreach (var section in sequence.Sections)
                            {
                                foreach (var question in failedDetails)
                                {
                                    if (question.SequenceNumber == section.SequenceNumber &&
                                        question.SectionNumber == section.SectionNumber)
                                    {
                                        if (section.Pages == null)
                                            section.Pages = new List<Page>();

                                        section.Pages.Add(new Page { PageId = question.PageId });
                                    }
                                }
                            }
                        }

                        // rebuild the matched sequences/sections/pageIds into a single block without extraneous sequences/sectors
                        var matchedSequences = new List<AssessorSequence>();
                        foreach (var sequence in sequenceDetails)
                        {
                            foreach (var section in sequence.Sections)
                            {
                                if (section.Pages != null)
                                {

                                    if (!matchedSequences.Any(x => x.SequenceNumber == sequence.SequenceNumber))
                                        matchedSequences.Add(new AssessorSequence
                                        {
                                            SequenceNumber = sequence.SequenceNumber,
                                            SequenceTitle = sequence.SequenceTitle,
                                            Sections = new List<AssessorSection>()
                                        });

                                    matchedSequences.FirstOrDefault(x => x.SequenceNumber == section.SequenceNumber)
                                        .Sections.Add(section);

                                }
                            }
                        }


                        // MFCMFC insert the questions and answers for each question
                        foreach (var sequence in matchedSequences)
                        {
                            foreach (var section in sequence.Sections)
                            {
                                var pagesToRemove = new List<Page>();

                                foreach (var page in section.Pages)
                                {
                                    var selectedSection = await _qnaApiClient.GetSectionBySectionNo(applicationId,
                                        sequence.SequenceNumber, section.SectionNumber);

                                    page.Title = _assessorLookupService.GetTitleForPage(page.PageId);
                                    if (string.IsNullOrEmpty(page.Title))
                                    {
                                        page.Title = _assessorLookupService.GetSectorNameForPage(page.PageId);
                                    }
                                    var pageDetails = selectedSection.GetPage(page.PageId);
                                    if (pageDetails == null || !pageDetails.Active)
                                    {
                                        pagesToRemove.Add(page);
                                    }
                                    else
                                    {
                                        page.SequenceId = pageDetails.SequenceId;
                                        page.SectionId = pageDetails.SectionId;
                                        page.PageOfAnswers = pageDetails.PageOfAnswers;

                                        foreach (var q in pageDetails.Questions)
                                        {
                                            if (string.Equals(q.Input.Type, QuestionType.CheckboxList, StringComparison.CurrentCultureIgnoreCase))
                                                q.Input.Type = QuestionType.CheckboxList;

                                            if (string.Equals(q.Input.Type, QuestionType.ComplexCheckboxList, StringComparison.CurrentCultureIgnoreCase))
                                                q.Input.Type = QuestionType.ComplexCheckboxList;

                                            if (string.Equals(q.Input.Type, QuestionType.Checkbox, StringComparison.CurrentCultureIgnoreCase))
                                                q.Input.Type = QuestionType.Checkbox;
                                        }

                                        page.Questions = pageDetails.Questions;

                                        page.DisplayType = page.DisplayType;
                                        page.LinkTitle = pageDetails.LinkTitle;

                                        var assessorPage = await _apiClient.GetAssessorPage(applicationId,
                                            sequence.SequenceNumber, section.SectionNumber, page.PageId);
                                        guidancePages.Add(assessorPage);
                                    }

                                }

                                if (pagesToRemove.Any())
                                {
                                    foreach (var pageToRemove in pagesToRemove)
                                        section.Pages.Remove(pageToRemove);
                                }
                            }
                        }


                        foreach (var sequence in matchedSequences)
                        {
                            sequence.SequenceTitle =
                                _assessorLookupService.GetTitleForSequence(sequence.SequenceNumber);

                        }

                        model.Sequences = matchedSequences;
                        model.PagesWithGuidance = guidancePages;

                        // var selectedSection = await _qnaApiClient.GetSectionBySectionNo(applicationId, sequence.SequenceNumber, section.SectionNumber);
                        //
                        // var pageToCollect = selectedSection.GetPage(question.PageId);

                    }



                }

                return View("~/Views/Roatp/ApplicationUnsuccessfulPostGateway.cshtml", model);
            }

            return View("~/Views/Roatp/ApplicationRejected.cshtml", model);
        }



        public async Task<IActionResult> DownloadFile(Guid applicationId, Guid sectionId,
            string pageId, string questionId, string filename)
        {
            var response = await _qnaApiClient.DownloadFile(applicationId, sectionId, pageId, questionId,
                filename);

            if (response.IsSuccessStatusCode)
            {
                var fileStream = await response.Content.ReadAsStreamAsync();

                return File(fileStream, response.Content.Headers.ContentType.MediaType, filename);
            }

            return NotFound();
        }
    }
}

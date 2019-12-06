using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.EmailService;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Configuration;
using SFA.DAS.ApplyService.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Controllers.Roatp
{
    [Authorize]
    public class GetHelpController : Controller
    {
        private readonly ILogger<GetHelpController> _logger;
        private readonly IQnaApiClient _qnaApiClient;
        private readonly IApplicationApiClient _applicationApiClient;
        private readonly IUsersApiClient _usersApiClient;
        private readonly List<TaskListConfiguration> _taskListConfiguration;
        private readonly ISessionService _sessionService;
        private readonly IGetHelpWithQuestionEmailService _emailService;

        private const string ApplicationDetailsKey = "Roatp_Application_Details";
        private const string GetHelpSubmittedForPageKey = "Roatp_GetHelpSubmitted_{0}";
        private const string GetHelpQuestionKey = "Roatp_GetHelpQuestion_{0}";
        private const string GetHelpErrorMessageKey = "Roatp_GetHelp_ErrorMessage";
        private const int GetHelpTextMaxLength = 250;
        private const string MinLengthErrorMessage = "Tell us what you need help with";
        private const string MaxLengthErrorMessage = "Enter at least 250 characters or less";

        public GetHelpController(ILogger<GetHelpController> logger, IQnaApiClient qnaApiClient, IApplicationApiClient applicationApiClient,
            IUsersApiClient usersApiClient, ISessionService sessionService, IOptions<List<TaskListConfiguration>> taskListConfiguration,
            IGetHelpWithQuestionEmailService emailService)
        {
            _logger = logger;
            _qnaApiClient = qnaApiClient;
            _applicationApiClient = applicationApiClient;
            _usersApiClient = usersApiClient;
            _sessionService = sessionService;
            _taskListConfiguration = taskListConfiguration.Value;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Index(Guid? applicationId, int sequenceId, int sectionId, string pageId, string title, string getHelp, string controller, string action)
        {
            var getHelpQuery = new GetHelpWithQuestion();

            if (String.IsNullOrWhiteSpace(getHelp))
            {
                _sessionService.Set(GetHelpErrorMessageKey, MinLengthErrorMessage);
                var questionKey = string.Format(GetHelpQuestionKey, pageId);
                _sessionService.Set(questionKey, string.Empty);
                return RedirectToAction(action, controller, new { applicationId, sequenceId, sectionId, pageId });
            }

            if (getHelp.Length > GetHelpTextMaxLength)
            {
                _sessionService.Set(GetHelpErrorMessageKey, MaxLengthErrorMessage);
                var questionKey = string.Format(GetHelpQuestionKey, pageId);
                _sessionService.Set(questionKey, getHelp);
                return RedirectToAction(action, controller, new { applicationId, sequenceId, sectionId, pageId });
            }

            if (applicationId.HasValue && applicationId.Value != Guid.Empty)
            {
                var sequences = await _qnaApiClient.GetSequences(applicationId.Value);
                var currentSequence = sequences.Single(x => x.SequenceId == sequenceId);
                var sections = await _qnaApiClient.GetSections(applicationId.Value, currentSequence.Id);
                var currentSection = sections.FirstOrDefault(x => x.SectionId == sectionId);

                var page = await _qnaApiClient.GetPage(applicationId.Value, currentSection.Id, pageId);
                if (page == null || String.IsNullOrEmpty(page.Title))
                {
                    getHelpQuery.PageTitle = title;
                }
                else
                {
                    getHelpQuery.PageTitle = page.Title;
                }
                var sequenceConfig = _taskListConfiguration.FirstOrDefault(x => x.Id == sequenceId);
                if (sequenceConfig != null)
                {
                    getHelpQuery.ApplicationSequence = sequenceConfig.Title;
                }
                if (currentSection != null)
                {
                    getHelpQuery.ApplicationSection = currentSection.Title;
                }

                var organisationName = await _qnaApiClient.GetAnswerByTag(applicationId.Value, RoatpWorkflowQuestionTags.UkrlpLegalName);
                if (organisationName != null && !String.IsNullOrWhiteSpace(organisationName.Value))
                {
                    getHelpQuery.OrganisationName = organisationName.Value;
                }
                else
                {
                    getHelpQuery.OrganisationName = "Not available";
                }

                var organisationUKPRN = await _qnaApiClient.GetAnswerByTag(applicationId.Value, RoatpWorkflowQuestionTags.UKPRN);
                if (organisationUKPRN != null && !String.IsNullOrWhiteSpace(organisationUKPRN.Value))
                {
                    getHelpQuery.UKPRN = organisationUKPRN.Value;
                }
                else
                {
                    getHelpQuery.UKPRN = "Not available";
                }
            }
            else
            {
                // in preamble so we don't have an application set up yet
                var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);
                getHelpQuery.PageTitle = title;
                getHelpQuery.ApplicationSequence = "Preamble";
                getHelpQuery.ApplicationSection = "Preamble";
                var ukprn = applicationDetails?.UKPRN.ToString();
                if (String.IsNullOrWhiteSpace(ukprn))
                {
                    ukprn = "Not available";
                }
                getHelpQuery.UKPRN = ukprn;
                var organisationName = applicationDetails?.UkrlpLookupDetails?.ProviderName;
                if (String.IsNullOrWhiteSpace(organisationName))
                {
                    organisationName = "Not available";
                }
                getHelpQuery.OrganisationName = organisationName;
            }
            
            getHelpQuery.GetHelpQuery = getHelp;

            var userDetails = await _usersApiClient.GetUserBySignInId(User.GetSignInId());

            getHelpQuery.EmailAddress = userDetails.Email;
            getHelpQuery.ApplicantFullName = $"{userDetails.GivenNames} {userDetails.FamilyName}";

            await _emailService.SendGetHelpWithQuestionEmail(getHelpQuery);

            var sessionKey = string.Format(GetHelpSubmittedForPageKey, pageId);
            _sessionService.Set(sessionKey, true);
            _sessionService.Set(GetHelpErrorMessageKey, string.Empty);

            return RedirectToAction(action, controller, new { applicationId, sequenceId, sectionId, pageId });
        }
    }
}
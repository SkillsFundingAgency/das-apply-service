using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Configuration;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
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

        private const string ApplicationDetailsKey = "Roatp_Application_Details";

        public GetHelpController(ILogger<GetHelpController> logger, IQnaApiClient qnaApiClient, IApplicationApiClient applicationApiClient,
            IUsersApiClient usersApiClient, ISessionService sessionService, IOptions<List<TaskListConfiguration>> taskListConfiguration)
        {
            _logger = logger;
            _qnaApiClient = qnaApiClient;
            _applicationApiClient = applicationApiClient;
            _usersApiClient = usersApiClient;
            _sessionService = sessionService;
            _taskListConfiguration = taskListConfiguration.Value;
        }

        public async Task<IActionResult> Index(Guid? applicationId, int sequenceId, int sectionId, string pageId, string title, string getHelp)
        {
            var model = new GetHelpWithQuestionViewModel();

            if (applicationId.HasValue && applicationId.Value != Guid.Empty)
            {
                var sequences = await _qnaApiClient.GetSequences(applicationId.Value);
                var currentSequence = sequences.Single(x => x.SequenceId == sequenceId);
                var sections = await _qnaApiClient.GetSections(applicationId.Value, currentSequence.Id);
                var currentSection = sections.Single(x => x.SectionId == sectionId);

                var page = await _qnaApiClient.GetPage(applicationId.Value, currentSection.Id, pageId);

                model.PageId = pageId;
                model.Title = page.Title;
                var sequenceConfig = _taskListConfiguration.FirstOrDefault(x => x.Id == sequenceId);
                if (sequenceConfig != null)
                {
                    model.SequenceId = sequenceId.ToString();
                }
                model.SectionId = currentSection.SectionId;

                var organisationName = await _qnaApiClient.GetAnswerByTag(applicationId.Value, RoatpWorkflowQuestionTags.UkrlpLegalName);
                if (organisationName != null && !String.IsNullOrWhiteSpace(organisationName.Value))
                {
                    model.OrganisationName = organisationName.Value;
                }
                else
                {
                    model.OrganisationName = "(not set)";
                }

                var organisationUKPRN = await _qnaApiClient.GetAnswerByTag(applicationId.Value, RoatpWorkflowQuestionTags.UKPRN);
                if (organisationUKPRN != null && !String.IsNullOrWhiteSpace(organisationUKPRN.Value))
                {
                    model.UKPRN = organisationUKPRN.Value;
                }
                else
                {
                    model.UKPRN = "(not set)";
                }
            }
            else
            {
                // in preamble so we don't have an application set up yet
                var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);
                model.PageId = "1";
                model.Title = title;
                model.SequenceId = "0";
                model.SectionId = 1;
                model.OrganisationName = applicationDetails?.UkrlpLookupDetails?.ProviderName;
                model.UKPRN = applicationDetails?.UKPRN.ToString();
            }


            model.ApplicantsQuery = getHelp;

            var userDetails = await _usersApiClient.GetUserBySignInId(User.GetSignInId());

            model.EmailAddress = userDetails.Email;
            model.ApplicantFullName = $"{userDetails.GivenNames} {userDetails.FamilyName}";
            
            return View("~/Views/Roatp/GetHelpSummary.cshtml", model);
        }
    }
}
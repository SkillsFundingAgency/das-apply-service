using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.EmailService.Interfaces;
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
        private readonly IUsersApiClient _usersApiClient;
        private readonly List<TaskListConfiguration> _taskListConfiguration;
        private readonly ISessionService _sessionService;
        private readonly IGetHelpWithQuestionEmailService _emailService;

        private const string ApplicationDetailsKey = "Roatp_Application_Details";
        private const string GetHelpSubmittedForPageKey = "Roatp_GetHelpSubmitted_{0}";
        private const string GetHelpQuestionKey = "Roatp_GetHelpQuestion_{0}";
        private const string GetHelpErrorMessageKey = "Roatp_GetHelp_ErrorMessage_{0}";
        private const int GetHelpTextMaxLength = 250;
        private const string MinLengthErrorMessage = "Tell us what you need help with";
        private const string MaxLengthErrorMessage = "Enter at least 250 characters or less";

        public GetHelpController(ILogger<GetHelpController> logger, IQnaApiClient qnaApiClient,
            IUsersApiClient usersApiClient, ISessionService sessionService, IOptions<List<TaskListConfiguration>> taskListConfiguration,
            IGetHelpWithQuestionEmailService emailService)
        {
            _logger = logger;
            _qnaApiClient = qnaApiClient;
            _usersApiClient = usersApiClient;
            _sessionService = sessionService;
            _taskListConfiguration = taskListConfiguration.Value;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Index(Guid? applicationId, int sequenceId, int sectionId, string pageId, string title, string getHelp, string controller, string action)
        {
            var errorMessageKey = string.Format(GetHelpErrorMessageKey, pageId);

            if (string.IsNullOrWhiteSpace(getHelp))
            {
                _sessionService.Set(errorMessageKey, MinLengthErrorMessage);
                var questionKey = string.Format(GetHelpQuestionKey, pageId);
                _sessionService.Set(questionKey, string.Empty);
                return RedirectToAction(action, controller, new { applicationId, sequenceId, sectionId, pageId });
            }

            if (getHelp.Length > GetHelpTextMaxLength)
            {
                _sessionService.Set(errorMessageKey, MaxLengthErrorMessage);
                var questionKey = string.Format(GetHelpQuestionKey, pageId);
                _sessionService.Set(questionKey, getHelp);
                return RedirectToAction(action, controller, new { applicationId, sequenceId, sectionId, pageId });
            }

            var sequenceConfig = _taskListConfiguration.FirstOrDefault(x => x.Id == sequenceId);
            var getHelpQuery = new GetHelpWithQuestion
            {
                ApplicationSequence = sequenceConfig?.Title ?? $"Not available (Sequence {sequenceId})",
                ApplicationSection = $"Not available (Section {sectionId}))",
                PageTitle = title ?? action,
                OrganisationName = "Not available",
                UKPRN = "Not available",
                CompanyNumber = "Not available",
                CharityNumber = "Not available",
            };

            if (applicationId.HasValue && applicationId.Value != Guid.Empty)
            {
                try 
                {
                    var qnaApplicationData = await _qnaApiClient.GetApplicationData(applicationId.Value) as JObject;

                    var organisationName = qnaApplicationData.GetValue(RoatpWorkflowQuestionTags.UkrlpLegalName)?.Value<string>();
                    if (!string.IsNullOrWhiteSpace(organisationName))
                    {
                        getHelpQuery.OrganisationName = organisationName;
                    }

                    var organisationUKPRN = qnaApplicationData.GetValue(RoatpWorkflowQuestionTags.UKPRN)?.Value<string>();
                    if (!string.IsNullOrWhiteSpace(organisationUKPRN))
                    {
                        getHelpQuery.UKPRN = organisationUKPRN;
                    }

                    var organisationCompanyNumber = qnaApplicationData.GetValue(RoatpWorkflowQuestionTags.UKRLPVerificationCompanyNumber)?.Value<string>();
                    if (!string.IsNullOrWhiteSpace(organisationCompanyNumber))
                    {
                        getHelpQuery.CompanyNumber = organisationCompanyNumber;
                    }

                    var organisationCharityNumber = qnaApplicationData.GetValue(RoatpWorkflowQuestionTags.UKRLPVerificationCharityRegNumber)?.Value<string>();
                    if (!string.IsNullOrWhiteSpace(organisationCharityNumber))
                    {
                        getHelpQuery.CharityNumber = organisationCharityNumber;
                    }

                    var currentSection = await _qnaApiClient.GetSectionBySectionNo(applicationId.Value, sequenceId, sectionId);
                    if (!string.IsNullOrEmpty(currentSection?.Title))
                    {
                        getHelpQuery.ApplicationSection = currentSection.Title;
                    }

                    var currentPage = currentSection?.QnAData.Pages.FirstOrDefault( pg => pg.PageId == pageId);
                    if (!string.IsNullOrEmpty(currentPage?.Title))
                    {
                        getHelpQuery.PageTitle = currentPage.Title;
                    }
                }
                catch(ApplyService.Infrastructure.Exceptions.ApiClientException apiEx)
                {
                    // Safe to ignore any QnA issues. We just want to send help with as much info as possible.
                    _logger.LogError(apiEx, $"Unable to retrieve QNA details for application : {applicationId.Value}");
                }
                catch (NullReferenceException nullRefEx)
                {
                    // Safe to ignore any QnA issues. We just want to send help with as much info as possible.
                    _logger.LogError(nullRefEx, $"QNA details were not found for application: {applicationId.Value}");
                }
            }
            else
            {
                // in preamble so we don't have an application set up yet
                getHelpQuery.ApplicationSequence = "Preamble";
                getHelpQuery.ApplicationSection = "Preamble";

                var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);   

                if(applicationDetails != null)
                {
                    getHelpQuery.UKPRN = applicationDetails.UKPRN.ToString();

                    var organisationName = applicationDetails.UkrlpLookupDetails?.ProviderName;
                    if (!string.IsNullOrWhiteSpace(organisationName))
                    {
                        getHelpQuery.OrganisationName = organisationName;
                    }

                    var organisationCompanyNumber = applicationDetails.UkrlpLookupDetails?.VerificationDetails?.FirstOrDefault(x => x.VerificationAuthority == Domain.Ukrlp.VerificationAuthorities.CompaniesHouseAuthority)?.VerificationId;
                    if (!string.IsNullOrWhiteSpace(organisationCompanyNumber))
                    {
                        getHelpQuery.CompanyNumber = organisationCompanyNumber;
                    }

                    var organisationCharityNumber = applicationDetails.UkrlpLookupDetails?.VerificationDetails?.FirstOrDefault(x => x.VerificationAuthority == Domain.Ukrlp.VerificationAuthorities.CharityCommissionAuthority)?.VerificationId;
                    if (!string.IsNullOrWhiteSpace(organisationCharityNumber))
                    {
                        getHelpQuery.CharityNumber = organisationCharityNumber;
                    }
                }                
            }
            
            getHelpQuery.GetHelpQuery = getHelp;

            var userDetails = await _usersApiClient.GetUserBySignInId(User.GetSignInId());

            getHelpQuery.EmailAddress = userDetails.Email;
            getHelpQuery.ApplicantFullName = $"{userDetails.GivenNames} {userDetails.FamilyName}";

            await _emailService.SendGetHelpWithQuestionEmail(getHelpQuery);

            var sessionKey = string.Format(GetHelpSubmittedForPageKey, pageId);
            _sessionService.Set(sessionKey, true);
            _sessionService.Set(errorMessageKey, string.Empty);

            return RedirectToAction(action, controller, new { applicationId, sequenceId, sectionId, pageId });
        }
    }
}
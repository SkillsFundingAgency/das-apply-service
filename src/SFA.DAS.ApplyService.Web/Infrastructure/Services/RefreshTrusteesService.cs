using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Polly;
using SFA.DAS.ApplyService.Domain.CharityCommission;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Infrastructure.ApiClients;
using SFA.DAS.ApplyService.Web.Infrastructure.Interfaces;
using Polly.Retry;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Web.Infrastructure.Services
{
    public class RefreshTrusteesService: IRefreshTrusteesService
    {
        private readonly IQnaApiClient _qnaApiClient;
        private readonly IOuterApiClient _outerApiClient;
        private readonly IOrganisationApiClient _organisationApiClient;
        private readonly IApplicationApiClient _applicationApiClient;
        private readonly ILogger<RefreshTrusteesService> _logger;
        private readonly RetryPolicy _retryPolicy;

        public RefreshTrusteesService(IQnaApiClient qnaApiClient,IOuterApiClient outerApiClient, IOrganisationApiClient organisationApiClient, IApplicationApiClient applicationApiClient, ILogger<RefreshTrusteesService> logger)
        {
            _outerApiClient = outerApiClient;
            _organisationApiClient = organisationApiClient;
            _applicationApiClient = applicationApiClient;
            _logger = logger;
            _qnaApiClient = qnaApiClient;
            _retryPolicy = GetRetryPolicy();
        }

        public async Task<RefreshTrusteeResult> RefreshTrustees(Guid applicationId, Guid userId)
        {
            var organisation = await _organisationApiClient.GetByApplicationId(applicationId);
            var ukprn = organisation?.OrganisationUkprn?.ToString();
            var charityNumber = organisation?.OrganisationDetails?.CharityNumber;

            if (ukprn == null || charityNumber == null || !int.TryParse(charityNumber, out var charityNumberValue))
            {
                _logger.LogInformation($"RefreshTrusteesService: Refresh failure for applicationId {applicationId}, ukprn: [{ukprn}], Charity number: [{charityNumber}]");
                
                return new RefreshTrusteeResult { CharityDetailsNotFound = true, CharityNumber = charityNumber };
            }

            var currentStatusesForUkrpn = await _applicationApiClient.GetExistingApplicationStatus(ukprn);
            var isStatusInProgress = currentStatusesForUkrpn.Any(x => x.Status==ApplicationStatus.InProgress);

            if (!isStatusInProgress)
            {
                _logger.LogInformation($"RefreshTrusteesService: Refresh failure for applicationId {applicationId} as status '{ApplicationStatus.InProgress}' expected.");
                return new RefreshTrusteeResult { CharityDetailsNotFound = true, CharityNumber = charityNumber };
            }

            var charityDetails = await _outerApiClient.GetCharityDetails(charityNumberValue);

            if (charityDetails == null || !charityDetails.IsActivelyTrading || charityDetails.Trustees == null || !charityDetails.Trustees.Any())
            {
                _logger.LogInformation($"RefreshTrusteesService:  Failure for applicationId {applicationId}  to retrieve trustee details from charity '{charityNumberValue}'");
                return new RefreshTrusteeResult { CharityDetailsNotFound = true, CharityNumber = charityNumber };
            }

            _logger.LogInformation($"RefreshTrusteesService: updating organisation trustees applicationId {applicationId}");

            organisation.OrganisationDetails.CharityCommissionDetails.Trustees = charityDetails.Trustees.Select(trusteeValue => new Trustee { Name = trusteeValue.Name, Id = trusteeValue.Id.ToString() }).ToList();
            organisation.UpdatedBy = userId.ToString();

            try
            {
                var success = await _retryPolicy.ExecuteAsync(
                    context => _organisationApiClient.UpdateTrustees(ukprn, charityDetails.Trustees?.ToList(), userId), new Context());

                if (!success)
                {
                    throw new Exception($"RefreshTrusteesService for Application {applicationId} update trustees failed");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"RefreshTrusteesService for Application {applicationId} update trustees failed: [{ex.Message}]");

            }

            var applicationDetails = new Domain.Roatp.ApplicationDetails { CharitySummary = Mapper.Map<CharityCommissionSummary>(charityDetails) };
            var trusteesAnswers = RoatpPreambleQuestionBuilder.CreateCharityCommissionWhosInControlQuestions(applicationDetails);

            _logger.LogInformation($"RefreshTrusteesService: resetting page answers for charities for page 80, 86 and section 3.4, applicationId {applicationId}");
            var resetSection1_3_80 = _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, RoatpWorkflowPageIds.WhosInControl.CharityCommissionTrustees);
            var resetSection1_3_86 = _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, RoatpWorkflowPageIds.WhosInControl.CharityCommissionTrusteesDob);
            var resetSection3_4 = _qnaApiClient.ResetPageAnswersBySection(applicationId, RoatpWorkflowSequenceIds.CriminalComplianceChecks, RoatpWorkflowSectionIds.CriminalComplianceChecks.CheckOnWhosInControl);
            await Task.WhenAll(resetSection1_3_80, resetSection1_3_86, resetSection3_4);

            _logger.LogInformation($"RefreshTrusteesService: updating page answers for charities, applicationId {applicationId}");

            
            try
            { 
                var response = await _retryPolicy.ExecuteAsync(
                    context => _qnaApiClient.UpdatePageAnswers(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, RoatpWorkflowPageIds.WhosInControl.CharityCommissionTrustees, trusteesAnswers.ToList<Answer>()), new Context());

                if (!response.ValidationPassed)
                {
                    throw new Exception($"RefreshTrusteesService for Application {applicationId} update qna page answers failed");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"RefreshTrusteesService for Application {applicationId} update qna page answers failed: [{ex.Message}]");
            }

            return new RefreshTrusteeResult
            {
                CharityNumber = "1223232",
                CharityDetailsNotFound = false
            };
        }

        private RetryPolicy GetRetryPolicy()
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(new[]
                    {
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(2),
                        TimeSpan.FromSeconds(4)
                    },
                    (exception, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning(
                            $"Error retrieving response from API. Reason: {exception.Message}. Retrying in {timeSpan.Seconds} secs...attempt: {retryCount}");
                    });
        }
    }
}

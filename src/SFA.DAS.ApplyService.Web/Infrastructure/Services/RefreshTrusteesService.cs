using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.CharityCommission;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Infrastructure.ApiClients;
using SFA.DAS.ApplyService.Web.Infrastructure.Interfaces;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.InternalApi.Types.CharityCommission;

namespace SFA.DAS.ApplyService.Web.Infrastructure.Services
{
    public class RefreshTrusteesService: IRefreshTrusteesService
    {
        private readonly IQnaApiClient _qnaApiClient;
        private readonly IOuterApiClient _outerApiClient;
        private readonly IOrganisationApiClient _organisationApiClient;
        private readonly IApplicationApiClient _applicationApiClient;
        private readonly ILogger<RefreshTrusteesService> _logger;


        public RefreshTrusteesService(IQnaApiClient qnaApiClient,IOuterApiClient outerApiClient, IOrganisationApiClient organisationApiClient, IApplicationApiClient applicationApiClient, ILogger<RefreshTrusteesService> logger)
        {
            _outerApiClient = outerApiClient;
            _organisationApiClient = organisationApiClient;
            _applicationApiClient = applicationApiClient;
            _logger = logger;
            _qnaApiClient = qnaApiClient;
        }

        public async Task<RefreshTrusteesResult> RefreshTrustees(Guid applicationId, Guid userId)
        {
            var organisation = await _organisationApiClient.GetByApplicationId(applicationId);
            var ukprn = organisation?.OrganisationUkprn?.ToString();
            var charityNumber = organisation?.OrganisationDetails?.CharityNumber;

            if (ukprn == null || charityNumber == null || !int.TryParse(charityNumber, out var charityNumberValue))
            {
                _logger.LogInformation($"RefreshTrusteesService: Refresh failure for applicationId {applicationId}, ukprn: [{ukprn}], Charity number: [{charityNumber}]");
                
                return new RefreshTrusteesResult { CharityDetailsNotFound = true, CharityNumber = charityNumber };
            }

            var application = await _applicationApiClient.GetApplication(applicationId);

            if (application.ApplicationStatus!=ApplicationStatus.InProgress)
            {
                _logger.LogInformation($"RefreshTrusteesService: Refresh failure for applicationId {applicationId} as status '{ApplicationStatus.InProgress}' expected.");
                return new RefreshTrusteesResult { CharityDetailsNotFound = true, CharityNumber = charityNumber };
            }

            Charity charityDetails;

            try
            {
                _logger.LogInformation($"RefreshTrusteesService:  retrieving charity details for applicationId {applicationId}  for charity registration number '{charityNumberValue}'");
                charityDetails = await _outerApiClient.GetCharityDetails(charityNumberValue);
            }
            catch(Exception ex)
            {
                var message =
                    $"RefreshTrusteesService: Exception for Application {applicationId}, failed to get charity details for charity number: [{charityNumberValue}]";
                _logger.LogError(ex,message);
                throw new InvalidOperationException(message,ex);
            }

            if (charityDetails == null || !charityDetails.IsActivelyTrading || charityDetails.Trustees == null || !charityDetails.Trustees.Any())
            {
                _logger.LogInformation($"RefreshTrusteesService:  Failure for applicationId {applicationId}  to retrieve trustee details from charity '{charityNumberValue}'");
                return new RefreshTrusteesResult { CharityDetailsNotFound = true, CharityNumber = charityNumber };
            }

            _logger.LogInformation($"RefreshTrusteesService: updating organisation trustees applicationId {applicationId}");

            try
            {
                var success = await _organisationApiClient.UpdateTrustees(ukprn, charityDetails.Trustees?.ToList(), userId);

                if (!success)
                {
                    throw new InvalidOperationException($"RefreshTrusteesService for Application {applicationId} update trustees failed");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"RefreshTrusteesService for Application {applicationId} update trustees failed: [{ex.Message}]");

            }

            var applicationDetails = new Domain.Roatp.ApplicationDetails { CharitySummary = Mapper.Map<CharityCommissionSummary>(charityDetails) };
            var trusteesAnswers = RoatpPreambleQuestionBuilder.CreateCharityCommissionWhosInControlQuestions(applicationDetails);

            _logger.LogInformation($"RefreshTrusteesService: resetting page answers for charities for page 80, 86 and section 3.4, applicationId {applicationId}");
            try
            {
                var resetSection1_3_80 = _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, RoatpWorkflowPageIds.WhosInControl.CharityCommissionTrustees);
                var resetSection1_3_86 = _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, RoatpWorkflowPageIds.WhosInControl.CharityCommissionTrusteesDob);
                var resetSection3_4 = _qnaApiClient.ResetPageAnswersBySection(applicationId, RoatpWorkflowSequenceIds.CriminalComplianceChecks, RoatpWorkflowSectionIds.CriminalComplianceChecks.CheckOnWhosInControl);
                await Task.WhenAll(resetSection1_3_80, resetSection1_3_86, resetSection3_4);

                _logger.LogInformation($"RefreshTrusteesService: updating page answers for charities, applicationId {applicationId}");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"RefreshTrusteesService for Application {applicationId} reset qna page answers failed: [{ex.Message}]");
            }

            try
            {
                var response = await _qnaApiClient.UpdatePageAnswers(applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                    RoatpWorkflowPageIds.WhosInControl.CharityCommissionTrustees, trusteesAnswers.ToList<Answer>());

                if (!response.ValidationPassed)
                {
                    throw new InvalidOperationException($"RefreshTrusteesService for Application {applicationId} update qna page answers failed");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"RefreshTrusteesService for Application {applicationId} update qna page answers failed: [{ex.Message}]");
            }

            return new RefreshTrusteesResult
            {
                CharityNumber = charityNumber,
                CharityDetailsNotFound = false
            };
        }
    }
}

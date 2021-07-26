using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply.Oversight;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Models.Roatp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Services
{
    public class RegistrationDetailsService : IRegistrationDetailsService
    {
        private readonly IInternalQnaApiClient _qnaApiClient;
        private readonly IRoatpApiClient _roatpApiClient;
        private readonly ILogger<RegistrationDetailsService> _logger;

        private const int EmployerProviderTypeId = 2;
        private const string PublicBodyOrganisationType = "A public body";
        private const string EducationalInstituteOrganisationType = "An educational institute";

        public RegistrationDetailsService(IInternalQnaApiClient qnaApiClient, IRoatpApiClient roatpApiClient, ILogger<RegistrationDetailsService> logger)
        {
            _qnaApiClient = qnaApiClient;
            _roatpApiClient = roatpApiClient;
            _logger = logger;

        }

        public async Task<RoatpRegistrationDetails> GetRegistrationDetails(Guid applicationId)
        {
            _logger.LogInformation($"Retrieving registration details for application id {applicationId}");
            
            var providerTypeId = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.ProviderRoute);
            int providerTypeIdValue;
            int.TryParse(providerTypeId.Value, out providerTypeIdValue);
            var registrationDetails = new RoatpRegistrationDetails
            {
                ProviderTypeId = providerTypeIdValue
            };

            var preamblePage = await _qnaApiClient.GetPageBySectionNo(applicationId, RoatpWorkflowSequenceIds.Preamble,
                                                                      RoatpWorkflowSectionIds.Preamble, RoatpWorkflowPageIds.Preamble);

            var preambleAnswers = preamblePage.PageOfAnswers.FirstOrDefault();

            var legalNameAnswer = preambleAnswers.Answers.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalName);
            var tradingNameAnswer = preambleAnswers.Answers.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpTradingName);
            var ukprnAnswer = preambleAnswers.Answers.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.UKPRN);

            registrationDetails.LegalName = legalNameAnswer.Value;
            registrationDetails.TradingName = tradingNameAnswer?.Value;
            registrationDetails.UKPRN = ukprnAnswer.Value;
            registrationDetails.OrganisationTypeId = await MapOrganisationDetailsQuestionsToRoatpOrganisationType(applicationId, registrationDetails.ProviderTypeId);

            var verifiedCompanyAnswer = preambleAnswers.Answers.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany);
            if (verifiedCompanyAnswer != null && verifiedCompanyAnswer.Value == "TRUE")
            {
                var companyNumberAnswer = preambleAnswers.Answers.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpVerificationCompanyNumber);
                registrationDetails.CompanyNumber = companyNumberAnswer.Value;
            }

            var verifiedCharityAnswer = preambleAnswers.Answers.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity);
            if (verifiedCharityAnswer != null && verifiedCharityAnswer.Value == "TRUE")
            {
                var charityNumberAnswer = preambleAnswers.Answers.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpVerificationCharityRegNumber);
                registrationDetails.CharityNumber = charityNumberAnswer.Value;
            }

            return registrationDetails;
        }

        private async Task<int> MapOrganisationDetailsQuestionsToRoatpOrganisationType(Guid applicationId, int providerTypeId)
        {
            int organisationTypeId = 0; // default to 'Unassigned'

            var organisationTypes = await _roatpApiClient.GetOrganisationTypes(providerTypeId);

            string organisationTypePageId = RoatpWorkflowPageIds.DescribeYourOrganisation.MainSupportingStartPage;
            string organisationTypeQuestionId = RoatpYourOrganisationQuestionIdConstants.OrganisationTypeMainSupporting;

            if (providerTypeId == EmployerProviderTypeId)
            {
                organisationTypePageId = RoatpWorkflowPageIds.DescribeYourOrganisation.EmployerStartPage;
                organisationTypeQuestionId = RoatpYourOrganisationQuestionIdConstants.OrganisationTypeEmployer;
            }

            var organisationTypePage = await _qnaApiClient.GetPageBySectionNo(applicationId, RoatpWorkflowSequenceIds.YourOrganisation,
                                                                        RoatpWorkflowSectionIds.YourOrganisation.DescribeYourOrganisation, organisationTypePageId);

            var organisationDetailsAnswers = organisationTypePage.PageOfAnswers.FirstOrDefault();
            var organisationTypeAnswer = organisationDetailsAnswers.Answers.FirstOrDefault(x => x.QuestionId == organisationTypeQuestionId);

            var matchingOrganisationType = organisationTypes.FirstOrDefault(x => x.Type.Equals(organisationTypeAnswer.Value.ToString()));

            if (matchingOrganisationType != null)
            {
                return matchingOrganisationType.Id;
            }

            switch (organisationTypeAnswer.Value)
            {
                case PublicBodyOrganisationType:
                    organisationTypeId = await MapPublicBodyOrganisationType(applicationId, providerTypeId, organisationTypes);
                    break;
                case EducationalInstituteOrganisationType:
                    organisationTypeId = await MapEducationalInstituteOrganisationType(applicationId, providerTypeId, organisationTypes);
                    break;
            }

            return organisationTypeId;
        }

        private async Task<int> MapPublicBodyOrganisationType(Guid applicationId, int providerTypeId, IEnumerable<OrganisationType> organisationTypes)
        {
            var publicBodyOrganisationTypePageId = RoatpWorkflowPageIds.DescribeYourOrganisation.PublicBodyType;

            var publicBodyOrganisationTypeQuestionId = RoatpYourOrganisationQuestionIdConstants.PublicBodyType;

            var publicBodyOrganisationTypePage = await _qnaApiClient.GetPageBySectionNo(applicationId, RoatpWorkflowSequenceIds.YourOrganisation,
                                                 RoatpWorkflowSectionIds.YourOrganisation.DescribeYourOrganisation, publicBodyOrganisationTypePageId);

            var organisationDetailsAnswers = publicBodyOrganisationTypePage.PageOfAnswers.FirstOrDefault();
            var organisationTypeAnswer = organisationDetailsAnswers.Answers.FirstOrDefault(x => x.QuestionId == publicBodyOrganisationTypeQuestionId);

            var matchingOrganisationType = organisationTypes.FirstOrDefault(x => x.Type.Equals(organisationTypeAnswer.Value.ToString()));

            if (matchingOrganisationType != null)
            {
                return matchingOrganisationType.Id;
            }

            return 0;
        }

        private async Task<int> MapEducationalInstituteOrganisationType(Guid applicationId, int providerTypeId, 
                                                                        IEnumerable<OrganisationType> organisationTypes)
        {
            var educationOrganisationTypePageId = RoatpWorkflowPageIds.DescribeYourOrganisation.EducationalInstituteType;

            var educationOrganisationTypeQuestionId = RoatpYourOrganisationQuestionIdConstants.EducationalInstituteType;

            var educationOrganisationTypePage = await _qnaApiClient.GetPageBySectionNo(applicationId, RoatpWorkflowSequenceIds.YourOrganisation,
                                                 RoatpWorkflowSectionIds.YourOrganisation.DescribeYourOrganisation, educationOrganisationTypePageId);

            var organisationDetailsAnswers = educationOrganisationTypePage.PageOfAnswers.FirstOrDefault();
            var organisationTypeAnswer = organisationDetailsAnswers.Answers.FirstOrDefault(x => x.QuestionId == educationOrganisationTypeQuestionId);

            var matchingOrganisationType = organisationTypes.FirstOrDefault(x => x.Type.Equals(organisationTypeAnswer.Value.ToString()));

            if (matchingOrganisationType != null)
            {
                return matchingOrganisationType.Id;
            }

            return 0;
        }
    }
}

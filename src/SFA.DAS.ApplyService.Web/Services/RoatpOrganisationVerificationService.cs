using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.Web.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Services
{
    public class RoatpOrganisationVerificationService : IRoatpOrganisationVerificationService
    {
        private readonly IQnaApiClient _qnaApiClient;

        public RoatpOrganisationVerificationService(IQnaApiClient qnaApiClient)
        {
            _qnaApiClient = qnaApiClient;
        }

        public async Task<OrganisationVerificationStatus> GetOrganisationVerificationStatus(Guid applicationId)
        {
            var verifiedCompaniesHouse = await _qnaApiClient.GetAnswer(applicationId, RoatpWorkflowSequenceIds.Preamble, RoatpWorkflowSectionIds.Preamble, RoatpWorkflowPageIds.Preamble, RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany);
            var companiesHouseManualEntry = await _qnaApiClient.GetAnswer(applicationId, RoatpWorkflowSequenceIds.Preamble, RoatpWorkflowSectionIds.Preamble, RoatpWorkflowPageIds.Preamble, RoatpPreambleQuestionIdConstants.CompaniesHouseManualEntryRequired);
            var verifiedCharityCommission = await _qnaApiClient.GetAnswer(applicationId, RoatpWorkflowSequenceIds.Preamble, RoatpWorkflowSectionIds.Preamble, RoatpWorkflowPageIds.Preamble, RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity);
            var charityCommissionManualEntry = await _qnaApiClient.GetAnswer(applicationId, RoatpWorkflowSequenceIds.Preamble, RoatpWorkflowSectionIds.Preamble, RoatpWorkflowPageIds.Preamble, RoatpPreambleQuestionIdConstants.CharityCommissionTrusteeManualEntry);
            var companiesHouseDataConfirmed = await _qnaApiClient.GetAnswer(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, RoatpWorkflowPageIds.WhosInControl.CompaniesHouseStartPage, RoatpYourOrganisationQuestionIdConstants.CompaniesHouseDetailsConfirmed);
            var charityCommissionDataConfirmed = await _qnaApiClient.GetAnswer(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, RoatpWorkflowPageIds.WhosInControl.CharityCommissionStartPage, RoatpYourOrganisationQuestionIdConstants.CharityCommissionDetailsConfirmed);

            var whosInControlConfirmed = false;

            var soleTraderDateOfBirthAnswer = await _qnaApiClient.GetAnswer(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, RoatpWorkflowPageIds.WhosInControl.AddSoleTraderDob, RoatpYourOrganisationQuestionIdConstants.AddSoleTradeDob);
            if (soleTraderDateOfBirthAnswer != null && !String.IsNullOrEmpty(soleTraderDateOfBirthAnswer.Value))
            {
                whosInControlConfirmed = true;
            }
            var partnersDetailsAnswer = await _qnaApiClient.GetAnswer(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, RoatpWorkflowPageIds.WhosInControl.AddPartners, RoatpYourOrganisationQuestionIdConstants.AddPartners);
            if (partnersDetailsAnswer != null && !String.IsNullOrEmpty(partnersDetailsAnswer.Value))
            {
                whosInControlConfirmed = true;
            }
            var pscsDetailsAnswer = await _qnaApiClient.GetAnswer(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, RoatpWorkflowPageIds.WhosInControl.AddPeopleInControl, RoatpYourOrganisationQuestionIdConstants.AddPeopleInControl);
            if (pscsDetailsAnswer != null && !String.IsNullOrEmpty(pscsDetailsAnswer.Value))
            {
                whosInControlConfirmed = true;
            }

            var status = new OrganisationVerificationStatus
            {
                VerifiedCompaniesHouse = (verifiedCompaniesHouse != null && verifiedCompaniesHouse.Value == "TRUE"),
                VerifiedCharityCommission = (verifiedCharityCommission != null && verifiedCharityCommission.Value == "TRUE"),
                CompaniesHouseManualEntry = (companiesHouseManualEntry != null && companiesHouseManualEntry.Value == "TRUE"),
                CharityCommissionManualEntry = (charityCommissionManualEntry != null && charityCommissionManualEntry.Value == "TRUE"),
                CompaniesHouseDataConfirmed = (companiesHouseDataConfirmed != null && companiesHouseDataConfirmed.Value == "Y"),
                CharityCommissionDataConfirmed = (charityCommissionDataConfirmed != null && charityCommissionDataConfirmed.Value == "Y"),
                WhosInControlConfirmed = whosInControlConfirmed
            };

            return status;
        }      
       
    }
}

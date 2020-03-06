using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;
using System;
using System.Collections.Generic;
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
            var sequences = await _qnaApiClient.GetSequences(applicationId);
            var preambleSequence = sequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.Preamble);
            var preambleSections = await _qnaApiClient.GetSections(applicationId, preambleSequence.Id);
            var preambleSection = preambleSections.FirstOrDefault();
            var yourOrganisationSequence = sequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);
            var yourOrganisationSections = await _qnaApiClient.GetSections(applicationId, yourOrganisationSequence.Id);
            var whosInControlSection = yourOrganisationSections.FirstOrDefault(x => x.SectionId == RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);

            var verifiedCompaniesHouse = await _qnaApiClient.GetAnswer(applicationId, preambleSection.Id, RoatpWorkflowPageIds.Preamble, RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany);
            var companiesHouseManualEntry = await _qnaApiClient.GetAnswer(applicationId, preambleSection.Id, RoatpWorkflowPageIds.Preamble, RoatpPreambleQuestionIdConstants.CompaniesHouseManualEntryRequired);
            var verifiedCharityCommission = await _qnaApiClient.GetAnswer(applicationId, preambleSection.Id, RoatpWorkflowPageIds.Preamble, RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity);
            var charityCommissionManualEntry = await _qnaApiClient.GetAnswer(applicationId, preambleSection.Id, RoatpWorkflowPageIds.Preamble, RoatpPreambleQuestionIdConstants.CharityCommissionTrusteeManualEntry);
            var companiesHouseDataConfirmed = await _qnaApiClient.GetAnswer(applicationId, whosInControlSection.Id, RoatpWorkflowPageIds.WhosInControl.CompaniesHouseStartPage, RoatpYourOrganisationQuestionIdConstants.CompaniesHouseDetailsConfirmed);
            var charityCommissionDataConfirmed = await _qnaApiClient.GetAnswer(applicationId, whosInControlSection.Id, RoatpWorkflowPageIds.WhosInControl.CharityCommissionStartPage, RoatpYourOrganisationQuestionIdConstants.CharityCommissionDetailsConfirmed);

            var whosInControlConfirmed = false;

            var soleTraderDateOfBirthAnswer = await _qnaApiClient.GetAnswer(applicationId, whosInControlSection.Id, RoatpWorkflowPageIds.WhosInControl.AddSoleTraderDob, RoatpYourOrganisationQuestionIdConstants.AddSoleTradeDob);
            if (soleTraderDateOfBirthAnswer != null && !String.IsNullOrEmpty(soleTraderDateOfBirthAnswer.Value))
            {
                whosInControlConfirmed = true;
            }
            var partnersDetailsAnswer = await _qnaApiClient.GetAnswer(applicationId, whosInControlSection.Id, RoatpWorkflowPageIds.WhosInControl.AddPartners, RoatpYourOrganisationQuestionIdConstants.AddPartners);
            if (partnersDetailsAnswer != null && !String.IsNullOrEmpty(partnersDetailsAnswer.Value))
            {
                whosInControlConfirmed = true;
            }
            var pscsDetailsAnswer = await _qnaApiClient.GetAnswer(applicationId, whosInControlSection.Id, RoatpWorkflowPageIds.WhosInControl.AddPeopleInControl, RoatpYourOrganisationQuestionIdConstants.AddPeopleInControl);
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
                CharityCommissionDataConfirmed = (charityCommissionDataConfirmed != null && charityCommissionDataConfirmed.Value == "TRUE"),
                WhosInControlConfirmed = whosInControlConfirmed
            };

            return status;
        }      
       
    }
}

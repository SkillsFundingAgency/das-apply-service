using Newtonsoft.Json.Linq;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.InternalApi.Types;
using SFA.DAS.ApplyService.Web.Infrastructure;
using System;
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
            try
            {
                var qnaApplicationData = await _qnaApiClient.GetApplicationData(applicationId);

                return new OrganisationVerificationStatus
                {
                    VerifiedCompaniesHouse = VerifiedCompaniesHouse(qnaApplicationData),
                    VerifiedCharityCommission = VerifiedCharityCommission(qnaApplicationData),
                    CompaniesHouseManualEntry = CompaniesHouseManualEntry(qnaApplicationData),
                    CharityCommissionManualEntry = CharityCommissionManualEntry(qnaApplicationData),
                    CompaniesHouseDataConfirmed = CompaniesHouseDataConfirmed(qnaApplicationData),
                    CharityCommissionDataConfirmed = CharityCommissionDataConfirmed(qnaApplicationData),
                    WhosInControlConfirmed = WhosInControlConfirmed(qnaApplicationData),
                    WhosInControlStarted = WhosInControlStarted(qnaApplicationData)
                };
            }
            catch (NullReferenceException)
            {
                return new OrganisationVerificationStatus();
            }
        }

        private static bool VerifiedCompaniesHouse(JObject qnaApplicationData)
        {
            var verifiedCompaniesHouse = qnaApplicationData.GetValue(RoatpWorkflowQuestionTags.UkrlpVerificationCompany)?.Value<string>();

            return verifiedCompaniesHouse == "TRUE";
        }

        private static bool VerifiedCharityCommission(JObject qnaApplicationData)
        {
            var verifiedCharityCommission = qnaApplicationData.GetValue(RoatpWorkflowQuestionTags.UkrlpVerificationCharity)?.Value<string>();

            return verifiedCharityCommission == "TRUE";
        }

        private static bool CompaniesHouseManualEntry(JObject qnaApplicationData)
        {
            var companiesHouseManualEntry = qnaApplicationData.GetValue(RoatpWorkflowQuestionTags.ManualEntryRequiredCompaniesHouse)?.Value<string>();

            return companiesHouseManualEntry == "TRUE";
        }

        private static bool CharityCommissionManualEntry(JObject qnaApplicationData)
        {
            var charityCommissionManualEntry = qnaApplicationData.GetValue(RoatpWorkflowQuestionTags.ManualEntryRequiredCharityCommission)?.Value<string>();

            return charityCommissionManualEntry == "TRUE";
        }

        private static bool CompaniesHouseDataConfirmed(JObject qnaApplicationData)
        {
            var directorsAndPSCsConfirmed = qnaApplicationData.GetValue(RoatpWorkflowQuestionTags.DirectorsPSCsConfirmed)?.Value<string>();

            return directorsAndPSCsConfirmed == "Y";
        }

        private static bool CharityCommissionDataConfirmed(JObject qnaApplicationData)
        {
            var trusteesConfirmed = qnaApplicationData.GetValue(RoatpWorkflowQuestionTags.TrusteesConfirmed)?.Value<string>();
            var trusteesDobConfirmed = qnaApplicationData.GetValue(RoatpWorkflowQuestionTags.TrusteesDobConfirmed)?.Value<string>();

            return trusteesConfirmed == "Y" && trusteesDobConfirmed == "Y";
        }

        private static bool WhosInControlStarted(JObject qnaApplicationData)
        {
            var soleTraderOrPartnership = qnaApplicationData.GetValue(RoatpWorkflowQuestionTags.SoleTraderOrPartnership)?.Value<string>();

            return soleTraderOrPartnership == RoatpOrganisationTypes.SoleTrader || soleTraderOrPartnership == RoatpOrganisationTypes.Partnership;
        }

        private static bool WhosInControlConfirmed(JObject qnaApplicationData)
        {
            var soleTraderOrPartnership = qnaApplicationData.GetValue(RoatpWorkflowQuestionTags.SoleTraderOrPartnership)?.Value<string>();

            if (soleTraderOrPartnership == RoatpOrganisationTypes.SoleTrader)
            {
                var soleTraderDateOfBirthAnswer = qnaApplicationData.GetValue(RoatpWorkflowQuestionTags.SoleTradeDob)?.Value<string>();
                return !string.IsNullOrEmpty(soleTraderDateOfBirthAnswer);
            }
            else if (soleTraderOrPartnership == RoatpOrganisationTypes.Partnership)
            {
                var partnersDetailsAnswer = qnaApplicationData.GetValue(RoatpWorkflowQuestionTags.AddPartners)?.Value<string>();
                return !string.IsNullOrEmpty(partnersDetailsAnswer);
            }
            else
            {
                var pscsDetailsAnswer = qnaApplicationData.GetValue(RoatpWorkflowQuestionTags.AddPeopleInControl)?.Value<string>();
                return !string.IsNullOrEmpty(pscsDetailsAnswer);
            }
        }
    }
}

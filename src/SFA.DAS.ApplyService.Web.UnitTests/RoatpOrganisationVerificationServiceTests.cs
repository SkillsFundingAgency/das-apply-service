using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.InternalApi.Types;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.UnitTests
{
    [TestFixture]
    public class RoatpOrganisationVerificationServiceTests
    {
        private Mock<IQnaApiClient> _qnaApiClient;
        private IRoatpOrganisationVerificationService _service;
        private Guid _applicationId;

        [SetUp]
        public void Before_each_test()
        {
            _applicationId = Guid.NewGuid();

            _qnaApiClient = new Mock<IQnaApiClient>();

            _service = new RoatpOrganisationVerificationService(_qnaApiClient.Object);
        }

        [TestCase("TRUE", true)]
        [TestCase("", false)]
        [TestCase(null, false)]

        public async Task Verified_companies_house_matches_answer_in_preamble(string answerValue, bool expectedVerificationResult)
        {
            var _qnaApplicationData = new JObject
            {
                [RoatpWorkflowQuestionTags.UkrlpVerificationCompany] = answerValue
            };

            _qnaApiClient.Setup(x => x.GetApplicationData(_applicationId)).ReturnsAsync(_qnaApplicationData);

            var verificationResult = await _service.GetOrganisationVerificationStatus(_applicationId);

            verificationResult.VerifiedCompaniesHouse.Should().Be(expectedVerificationResult);
        }

        [TestCase("TRUE", true)]
        [TestCase("", false)]
        [TestCase(null, false)]

        public async Task Verified_charity_commission_matches_answer_in_preamble(string answerValue, bool expectedVerificationResult)
        {
            var _qnaApplicationData = new JObject
            {
                [RoatpWorkflowQuestionTags.UkrlpVerificationCharity] = answerValue
            };

            _qnaApiClient.Setup(x => x.GetApplicationData(_applicationId)).ReturnsAsync(_qnaApplicationData);

            var verificationResult = await _service.GetOrganisationVerificationStatus(_applicationId);

            verificationResult.VerifiedCharityCommission.Should().Be(expectedVerificationResult);
        }

        [TestCase("TRUE", true)]
        [TestCase("", false)]
        [TestCase(null, false)]

        public async Task Companies_house_information_manual_entry_required_matches_answer_in_preamble(string answerValue, bool expectedVerificationResult)
        {
            var _qnaApplicationData = new JObject
            {
                [RoatpWorkflowQuestionTags.ManualEntryRequiredCompaniesHouse] = answerValue
            };

            _qnaApiClient.Setup(x => x.GetApplicationData(_applicationId)).ReturnsAsync(_qnaApplicationData);

            var verificationResult = await _service.GetOrganisationVerificationStatus(_applicationId);

            verificationResult.CompaniesHouseManualEntry.Should().Be(expectedVerificationResult);
        }

        [TestCase("TRUE", true)]
        [TestCase("", false)]
        [TestCase(null, false)]

        public async Task Charity_commission_information_manual_entry_required_matches_answer_in_preamble(string answerValue, bool expectedVerificationResult)
        {
            var _qnaApplicationData = new JObject
            {
                [RoatpWorkflowQuestionTags.ManualEntryRequiredCharityCommission] = answerValue
            };

            _qnaApiClient.Setup(x => x.GetApplicationData(_applicationId)).ReturnsAsync(_qnaApplicationData);

            var verificationResult = await _service.GetOrganisationVerificationStatus(_applicationId);

            verificationResult.CharityCommissionManualEntry.Should().Be(expectedVerificationResult);
        }

        [TestCase("Y", true)]
        [TestCase("", false)]
        [TestCase(null, false)]

        public async Task Companies_house_data_confirmed_matches_answer_in_whos_in_control_pages(string answerValue, bool expectedVerificationResult)
        {
            var _qnaApplicationData = new JObject
            {
                [RoatpWorkflowQuestionTags.DirectorsPSCsConfirmed] = answerValue
            };

            _qnaApiClient.Setup(x => x.GetApplicationData(_applicationId)).ReturnsAsync(_qnaApplicationData);

            var verificationResult = await _service.GetOrganisationVerificationStatus(_applicationId);

            verificationResult.CompaniesHouseDataConfirmed.Should().Be(expectedVerificationResult);
        }

        [TestCase("Y", "Y", true)]
        [TestCase("Y", "N", false)]
        [TestCase("", "", false)]
        [TestCase(null, null, false)]

        public async Task Charity_commission_data_confirmed_matches_answer_in_whos_in_control_pages(string trusteesConfirmed, string trusteesDobConfirmed, bool expectedVerificationResult)
        {
            var _qnaApplicationData = new JObject
            {
                [RoatpWorkflowQuestionTags.TrusteesConfirmed] = trusteesConfirmed,
                [RoatpWorkflowQuestionTags.TrusteesDobConfirmed] = trusteesDobConfirmed,
            };

            _qnaApiClient.Setup(x => x.GetApplicationData(_applicationId)).ReturnsAsync(_qnaApplicationData);

            var verificationResult = await _service.GetOrganisationVerificationStatus(_applicationId);

            verificationResult.CharityCommissionDataConfirmed.Should().Be(expectedVerificationResult);
        }

        [Test]
        public async Task Whos_in_control_confirmed_is_true_if_manually_entered_sole_trader_details()
        {
            var _qnaApplicationData = new JObject
            {
                [RoatpWorkflowQuestionTags.SoleTraderOrPartnership] = RoatpOrganisationTypes.SoleTrader,
                [RoatpWorkflowQuestionTags.SoleTradeDob] = "details",
            };

            _qnaApiClient.Setup(x => x.GetApplicationData(_applicationId)).ReturnsAsync(_qnaApplicationData);

            var verificationResult = await _service.GetOrganisationVerificationStatus(_applicationId);

            verificationResult.WhosInControlStarted.Should().BeTrue();
            verificationResult.WhosInControlConfirmed.Should().BeTrue();
        }

        [Test]
        public async Task Whos_in_control_confirmed_is_false_if_no_manually_entered_sole_trader_details()
        {
            var _qnaApplicationData = new JObject
            {
                [RoatpWorkflowQuestionTags.SoleTraderOrPartnership] = RoatpOrganisationTypes.SoleTrader,
                [RoatpWorkflowQuestionTags.SoleTradeDob] = null,
            };

            _qnaApiClient.Setup(x => x.GetApplicationData(_applicationId)).ReturnsAsync(_qnaApplicationData);

            var verificationResult = await _service.GetOrganisationVerificationStatus(_applicationId);

            verificationResult.WhosInControlStarted.Should().BeTrue();
            verificationResult.WhosInControlConfirmed.Should().BeFalse();
        }

        [Test]
        public async Task Whos_in_control_confirmed_is_true_if_manually_entered_partner_details()
        {
            var _qnaApplicationData = new JObject
            {
                [RoatpWorkflowQuestionTags.SoleTraderOrPartnership] = RoatpOrganisationTypes.Partnership,
                [RoatpWorkflowQuestionTags.AddPartners] = "details",
            };

            _qnaApiClient.Setup(x => x.GetApplicationData(_applicationId)).ReturnsAsync(_qnaApplicationData);

            var verificationResult = await _service.GetOrganisationVerificationStatus(_applicationId);

            verificationResult.WhosInControlStarted.Should().BeTrue();
            verificationResult.WhosInControlConfirmed.Should().BeTrue();
        }

        [Test]
        public async Task Whos_in_control_confirmed_is_false_if_no_manually_entered_partner_details()
        {
            var _qnaApplicationData = new JObject
            {
                [RoatpWorkflowQuestionTags.SoleTraderOrPartnership] = RoatpOrganisationTypes.Partnership,
                [RoatpWorkflowQuestionTags.AddPartners] = null,
            };

            _qnaApiClient.Setup(x => x.GetApplicationData(_applicationId)).ReturnsAsync(_qnaApplicationData);

            var verificationResult = await _service.GetOrganisationVerificationStatus(_applicationId);

            verificationResult.WhosInControlStarted.Should().BeTrue();
            verificationResult.WhosInControlConfirmed.Should().BeFalse();
        }

        [Test]
        public async Task Whos_in_control_confirmed_is_true_if_manually_entered_psc_details()
        {
            var _qnaApplicationData = new JObject
            {
                [RoatpWorkflowQuestionTags.SoleTraderOrPartnership] = null,
                [RoatpWorkflowQuestionTags.AddPeopleInControl] = "details",
            };
            
            _qnaApiClient.Setup(x => x.GetApplicationData(_applicationId)).ReturnsAsync(_qnaApplicationData);

            var verificationResult = await _service.GetOrganisationVerificationStatus(_applicationId);

            verificationResult.WhosInControlStarted.Should().BeFalse();
            verificationResult.WhosInControlConfirmed.Should().BeTrue();
        }

        [Test]
        public async Task Whos_in_control_confirmed_is_false_if_no_manually_entered_psc_details()
        {
            var _qnaApplicationData = new JObject
            {
                [RoatpWorkflowQuestionTags.SoleTraderOrPartnership] = null,
                [RoatpWorkflowQuestionTags.AddPeopleInControl] = null,
            };

            _qnaApiClient.Setup(x => x.GetApplicationData(_applicationId)).ReturnsAsync(_qnaApplicationData);

            var verificationResult = await _service.GetOrganisationVerificationStatus(_applicationId);

            verificationResult.WhosInControlStarted.Should().BeFalse();
            verificationResult.WhosInControlConfirmed.Should().BeFalse();
        }
    }
}

using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.UnitTests
{
    [TestFixture]
    public class RoatpOrganisationVerificationServiceTests
    {
        private Mock<IQnaApiClient> _qnaApiClient;
        private RoatpOrganisationVerificationService _service;
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

        public void Verified_companies_house_matches_answer_in_preamble(string answerValue, bool expectedVerificationResult)
        {
            _qnaApiClient.Setup(x => x.GetAnswer(_applicationId,
                                                 RoatpWorkflowSequenceIds.Preamble,
                                                 RoatpWorkflowSectionIds.Preamble,
                                                 RoatpWorkflowPageIds.Preamble,
                                                 RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany))
                         .ReturnsAsync(new Domain.Apply.Answer { Value = answerValue });

            var verificationResult = _service.GetOrganisationVerificationStatus(_applicationId).GetAwaiter().GetResult();

            verificationResult.VerifiedCompaniesHouse.Should().Be(expectedVerificationResult);
        }

        [TestCase("TRUE", true)]
        [TestCase("", false)]
        [TestCase(null, false)]

        public void Verified_charity_commission_matches_answer_in_preamble(string answerValue, bool expectedVerificationResult)
        {
            _qnaApiClient.Setup(x => x.GetAnswer(_applicationId, 
                                                 RoatpWorkflowSequenceIds.Preamble,
                                                 RoatpWorkflowSectionIds.Preamble, 
                                                 RoatpWorkflowPageIds.Preamble,
                                                 RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity))
                         .ReturnsAsync(new Domain.Apply.Answer { Value = answerValue });

            var verificationResult = _service.GetOrganisationVerificationStatus(_applicationId).GetAwaiter().GetResult();

            verificationResult.VerifiedCharityCommission.Should().Be(expectedVerificationResult);
        }

        [TestCase("TRUE", true)]
        [TestCase("", false)]
        [TestCase(null, false)]

        public void Companies_house_information_manual_entry_required_matches_answer_in_preamble(string answerValue, bool expectedVerificationResult)
        {
            _qnaApiClient.Setup(x => x.GetAnswer(_applicationId, 
                                                 RoatpWorkflowSequenceIds.Preamble,
                                                 RoatpWorkflowSectionIds.Preamble, 
                                                 RoatpWorkflowPageIds.Preamble,
                                                 RoatpPreambleQuestionIdConstants.CompaniesHouseManualEntryRequired))
                         .ReturnsAsync(new Domain.Apply.Answer { Value = answerValue });

            var verificationResult = _service.GetOrganisationVerificationStatus(_applicationId).GetAwaiter().GetResult();

            verificationResult.CompaniesHouseManualEntry.Should().Be(expectedVerificationResult);
        }

        [TestCase("TRUE", true)]
        [TestCase("", false)]
        [TestCase(null, false)]

        public void Charity_commission_information_manual_entry_required_matches_answer_in_preamble(string answerValue, bool expectedVerificationResult)
        {
            _qnaApiClient.Setup(x => x.GetAnswer(_applicationId, 
                                                 RoatpWorkflowSequenceIds.Preamble,
                                                 RoatpWorkflowSectionIds.Preamble, 
                                                 RoatpWorkflowPageIds.Preamble,
                                                 RoatpPreambleQuestionIdConstants.CharityCommissionTrusteeManualEntry))
                         .ReturnsAsync(new Domain.Apply.Answer { Value = answerValue });

            var verificationResult = _service.GetOrganisationVerificationStatus(_applicationId).GetAwaiter().GetResult();

            verificationResult.CharityCommissionManualEntry.Should().Be(expectedVerificationResult);
        }

        [TestCase("Y", true)]
        [TestCase("", false)]
        [TestCase(null, false)]

        public void Companies_house_data_confirmed_matches_answer_in_whos_in_control_pages(string answerValue, bool expectedVerificationResult)
        {
            _qnaApiClient.Setup(x => x.GetAnswer(_applicationId, 
                                                 RoatpWorkflowSequenceIds.YourOrganisation,
                                                 RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, 
                                                 RoatpWorkflowPageIds.WhosInControl.CompaniesHouseStartPage,
                                                 RoatpYourOrganisationQuestionIdConstants.CompaniesHouseDetailsConfirmed))
                         .ReturnsAsync(new Domain.Apply.Answer { Value = answerValue });

            var verificationResult = _service.GetOrganisationVerificationStatus(_applicationId).GetAwaiter().GetResult();

            verificationResult.CompaniesHouseDataConfirmed.Should().Be(expectedVerificationResult);
        }

        [TestCase("Y", true)]
        [TestCase("", false)]
        [TestCase(null, false)]

        public void Charity_commission_data_confirmed_matches_answer_in_whos_in_control_pages(string answerValue, bool expectedVerificationResult)
        {
            _qnaApiClient.Setup(x => x.GetAnswer(_applicationId,
                                                 RoatpWorkflowSequenceIds.YourOrganisation,
                                                 RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                                                 RoatpWorkflowPageIds.WhosInControl.CharityCommissionStartPage,
                                                 RoatpYourOrganisationQuestionIdConstants.CharityCommissionDetailsConfirmed))
                         .ReturnsAsync(new Domain.Apply.Answer { Value = answerValue });

            var verificationResult = _service.GetOrganisationVerificationStatus(_applicationId).GetAwaiter().GetResult();

            verificationResult.CharityCommissionDataConfirmed.Should().Be(expectedVerificationResult);
        }

        [Test]
        public void Whos_in_control_confirmed_is_true_if_manually_entered_sole_trader_details()
        {
            _qnaApiClient.Setup(x => x.GetAnswer(_applicationId,
                                                 RoatpWorkflowSequenceIds.YourOrganisation,
                                                 RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                                                 RoatpWorkflowPageIds.WhosInControl.AddSoleTraderDob,
                                                 RoatpYourOrganisationQuestionIdConstants.AddSoleTradeDob))
                         .ReturnsAsync(new Domain.Apply.Answer { Value = "details" });

            _qnaApiClient.Setup(x => x.GetAnswer(_applicationId,
                                                 RoatpWorkflowSequenceIds.YourOrganisation,
                                                 RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                                                 RoatpWorkflowPageIds.WhosInControl.SoleTraderPartnership,
                                                 RoatpYourOrganisationQuestionIdConstants.SoleTradeOrPartnership))
                                                 .ReturnsAsync(new Domain.Apply.Answer { Value = RoatpOrganisationTypes.SoleTrader });

            var verificationResult = _service.GetOrganisationVerificationStatus(_applicationId).GetAwaiter().GetResult();

            verificationResult.WhosInControlConfirmed.Should().BeTrue();
        }

        [Test]
        public void Whos_in_control_confirmed_is_true_if_manually_entered_partner_details()
        {
            _qnaApiClient.Setup(x => x.GetAnswer(_applicationId,
                                                 RoatpWorkflowSequenceIds.YourOrganisation,
                                                 RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                                                 RoatpWorkflowPageIds.WhosInControl.AddPartners,
                                                 RoatpYourOrganisationQuestionIdConstants.AddPartners))
                                                 .ReturnsAsync(new Domain.Apply.Answer { Value = "details" });

            _qnaApiClient.Setup(x => x.GetAnswer(_applicationId,
                                                 RoatpWorkflowSequenceIds.YourOrganisation,
                                                 RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, 
                                                 RoatpWorkflowPageIds.WhosInControl.SoleTraderPartnership,
                                                 RoatpYourOrganisationQuestionIdConstants.SoleTradeOrPartnership))
                                                 .ReturnsAsync(new Domain.Apply.Answer { Value = RoatpOrganisationTypes.Partnership });

            var verificationResult = _service.GetOrganisationVerificationStatus(_applicationId).GetAwaiter().GetResult();

            verificationResult.WhosInControlConfirmed.Should().BeTrue();
        }

        [Test]
        public void Whos_in_control_confirmed_is_true_if_manually_entered_psc_details()
        {
            _qnaApiClient.Setup(x => x.GetAnswer(_applicationId, 
                                                 RoatpWorkflowSequenceIds.YourOrganisation,
                                                 RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, 
                                                 RoatpWorkflowPageIds.WhosInControl.AddPeopleInControl,
                                                 RoatpYourOrganisationQuestionIdConstants.AddPeopleInControl))
                         .ReturnsAsync(new Domain.Apply.Answer { Value = "details" });

            var verificationResult = _service.GetOrganisationVerificationStatus(_applicationId).GetAwaiter().GetResult();

            verificationResult.WhosInControlConfirmed.Should().BeTrue();
        }

        [TestCase("")]
        [TestCase(null)]
        public void Whos_in_control_confirmed_is_false_if_no_manually_entered_details(string answerValue)
        {
            _qnaApiClient.Setup(x => x.GetAnswer(_applicationId,
                                                 RoatpWorkflowSequenceIds.YourOrganisation,
                                                 RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, 
                                                 RoatpWorkflowPageIds.WhosInControl.AddSoleTraderDob,
                                                 RoatpYourOrganisationQuestionIdConstants.AddSoleTradeDob))
                                                 .ReturnsAsync(new Domain.Apply.Answer { Value = answerValue });

            _qnaApiClient.Setup(x => x.GetAnswer(_applicationId, 
                                                 RoatpWorkflowSequenceIds.YourOrganisation,
                                                 RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                                                 RoatpWorkflowPageIds.WhosInControl.AddPartners,
                                                 RoatpYourOrganisationQuestionIdConstants.AddPartners))
                                                 .ReturnsAsync(new Domain.Apply.Answer { Value = answerValue });

            _qnaApiClient.Setup(x => x.GetAnswer(_applicationId, 
                                                 RoatpWorkflowSequenceIds.YourOrganisation,
                                                 RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, 
                                                 RoatpWorkflowPageIds.WhosInControl.AddPeopleInControl,
                                                 RoatpYourOrganisationQuestionIdConstants.AddPeopleInControl))
                                                 .ReturnsAsync(new Domain.Apply.Answer { Value = answerValue });

            var verificationResult = _service.GetOrganisationVerificationStatus(_applicationId).GetAwaiter().GetResult();

            verificationResult.WhosInControlConfirmed.Should().BeFalse();
        }
    }
}

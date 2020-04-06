using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Models.Roatp;
using SFA.DAS.ApplyService.InternalApi.Services;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
    public class CriminalComplianceChecksQuestionLookupServiceTests
    {
        private Mock<IOptions<List<CriminalComplianceGatewayConfig>>> _config;
        private Mock<IOptions<List<CriminalComplianceGatewayOverrideConfig>>> _overrideConfig;

        private Mock<IInternalQnaApiClient> _apiClient;
        private CriminalComplianceChecksQuestionLookupService _lookupService;
        private Guid _applicationId;

        [SetUp]
        public void Before_each_test()
        {
            _applicationId = Guid.NewGuid();

            _apiClient = new Mock<IInternalQnaApiClient>();
            
            var config = new List<CriminalComplianceGatewayConfig>
            {
                new CriminalComplianceGatewayConfig
                {
                    GatewayPageId = "GatewayPage1",
                    QnaPageId = "1000",
                    QnaQuestionId = "CC-1",
                    SectionId = RoatpWorkflowSectionIds.CriminalComplianceChecks.ChecksOnYourOrganisation
                },
                new CriminalComplianceGatewayConfig
                {
                    GatewayPageId = "GatewayPage2",
                    QnaPageId = "2000",
                    QnaQuestionId = "CC-2",
                    SectionId = RoatpWorkflowSectionIds.CriminalComplianceChecks.ChecksOnYourOrganisation
                },
                new CriminalComplianceGatewayConfig
                {
                    GatewayPageId = "GatewayPage3",
                    QnaPageId = "2000",
                    QnaQuestionId = "CC-3",
                    SectionId = RoatpWorkflowSectionIds.CriminalComplianceChecks.CheckOnWhosInControl
                },
            };
            _config = new Mock<IOptions<List<CriminalComplianceGatewayConfig>>>();
            _config.Setup(x => x.Value).Returns(config);

            var overrideConfig = new List<CriminalComplianceGatewayOverrideConfig>
            {
                new CriminalComplianceGatewayOverrideConfig
                {
                    GatewayPageId = "GatewayPage3",
                    QnaPageId = "2001",
                    QnaQuestionId = "CC-31",
                    SectionId = RoatpWorkflowSectionIds.CriminalComplianceChecks.CheckOnWhosInControl
                }
            };

            _overrideConfig = new Mock<IOptions<List<CriminalComplianceGatewayOverrideConfig>>>();
            _overrideConfig.Setup(x => x.Value).Returns(overrideConfig);

            _lookupService = new CriminalComplianceChecksQuestionLookupService(_config.Object, _overrideConfig.Object, _apiClient.Object);
        }

        [Test]
        public void Lookup_service_retrieves_page_id_and_question_id_from_config_by_gateway_page_id()
        {
            var questionDetails = _lookupService.GetQuestionDetailsForGatewayPageId(_applicationId, "GatewayPage1");

            questionDetails.PageId.Should().Be("1000");
            questionDetails.QuestionId.Should().Be("CC-1");
        }

        [Test]
        public void Lookup_service_returns_null_if_gateway_page_id_not_found_in_config()
        {
            var questionDetails = _lookupService.GetQuestionDetailsForGatewayPageId(_applicationId, "GatewayPage4");

            questionDetails.Should().BeNull();
        }

        [Test]
        public void Lookup_service_returns_override_if_one_present_for_sole_trader_applicants()
        {
            _apiClient.Setup(x => x.GetQuestionTag(_applicationId, RoatpWorkflowQuestionTags.SoleTraderOrPartnership)).ReturnsAsync("Sole trader");

            var questionDetails = _lookupService.GetQuestionDetailsForGatewayPageId(_applicationId, "GatewayPage3");
            
            questionDetails.PageId.Should().Be("2001");
            questionDetails.QuestionId.Should().Be("CC-31");
        }

        [Test]
        public void Lookup_service_returns_default_if_no_overide_present_for_sole_trader_applicants()
        {
            var questionDetails = _lookupService.GetQuestionDetailsForGatewayPageId(_applicationId, "GatewayPage3");

            questionDetails.PageId.Should().Be("2000");
            questionDetails.QuestionId.Should().Be("CC-3");
        }
    }
}

using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.InternalApi.Models.Roatp;
using SFA.DAS.ApplyService.InternalApi.Services;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
    public class CriminalComplianceChecksQuestionLookupServiceTests
    {
        private Mock<IOptions<List<CriminalComplianceGatewayConfig>>> _configuration;
        private CriminalComplianceChecksQuestionLookupService _lookupService;

        [SetUp]
        public void Before_each_test()
        {
            _configuration = new Mock<IOptions<List<CriminalComplianceGatewayConfig>>>();
        }

        [Test]
        public void Lookup_service_retrieves_page_id_and_question_id_from_config_by_gateway_page_id()
        {
            var config = new List<CriminalComplianceGatewayConfig>
            {
                new CriminalComplianceGatewayConfig
                {
                    GatewayPageId = "GatewayPage1",
                    QnaPageId = "1000",
                    QnaQuestionId = "CC-1"
                },
                new CriminalComplianceGatewayConfig
                {
                    GatewayPageId = "GatewayPage2",
                    QnaPageId = "2000",
                    QnaQuestionId = "CC-2"
                }
            };

            _configuration.Setup(x => x.Value).Returns(config);

            _lookupService = new CriminalComplianceChecksQuestionLookupService(_configuration.Object);
            var questionDetails = _lookupService.GetQuestionDetailsForGatewayPageId("GatewayPage1");

            questionDetails.PageId.Should().Be("1000");
            questionDetails.QuestionId.Should().Be("CC-1");
        }

        [Test]
        public void Lookup_service_returns_null_if_gateway_page_id_not_found_in_config()
        {
            var config = new List<CriminalComplianceGatewayConfig>
            {
                new CriminalComplianceGatewayConfig
                {
                    GatewayPageId = "GatewayPage1",
                    QnaPageId = "1000",
                    QnaQuestionId = "CC-1"
                },
                new CriminalComplianceGatewayConfig
                {
                    GatewayPageId = "GatewayPage2",
                    QnaPageId = "2000",
                    QnaQuestionId = "CC-2"
                }
            };

            _configuration.Setup(x => x.Value).Returns(config);

            _lookupService = new CriminalComplianceChecksQuestionLookupService(_configuration.Object);
            var questionDetails = _lookupService.GetQuestionDetailsForGatewayPageId("GatewayPage3");

            questionDetails.Should().BeNull();
        }
    }
}

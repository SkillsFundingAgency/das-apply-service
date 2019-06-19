using AutoMapper;

namespace SFA.DAS.ApplyService.InternalApi.IntegrationTests
{
    using System.Linq;
    using System.Net.Http;
    using Configuration;
    using FluentAssertions;
    using Infrastructure;
    using Microsoft.Extensions.Logging;
    using Moq;
    using NUnit.Framework;
    using SFA.DAS.ApplyService.InternalApi.AutoMapper;

  [TestFixture]
    public class UkrlpLookupTests
    {
        private Mock<ILogger<UkrlpApiClient>> _logger;
        private Mock<IConfigurationService> _config;

        [SetUp]
        public void Before_each_test()
        {
            Mapper.Reset();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<UkrlpVerificationDetailsProfile>();
                cfg.AddProfile<UkrlpContactPersonalDetailsProfile>();
                cfg.AddProfile<UkrlpContactAddressProfile>();
                cfg.AddProfile<UkrlpProviderAliasProfile>();
                cfg.AddProfile<UkrlpProviderContactProfile>();
                cfg.AddProfile<UkrlpProviderDetailsProfile>();
            });

            Mapper.AssertConfigurationIsValid();

            _logger = new Mock<ILogger<UkrlpApiClient>>();
            _config = new Mock<IConfigurationService>();
            var apiConfig = new UkrlpApiAuthentication
            {
                QueryId = "2",
                StakeholderId = "2",
                ApiBaseAddress = "http://webservices.ukrlp.co.uk/UkrlpProviderQueryWS5/ProviderQueryServiceV5"
            };
            var applyConfig = new Mock<IApplyConfig>();

            applyConfig.SetupGet(x => x.UkrlpApiAuthentication).Returns(apiConfig);

            _config.Setup(x => x.GetConfig()).ReturnsAsync(applyConfig.Object);

        }

        [Test]
        public void Matching_UKPRN_returns_single_result()
        {
            var ukprn = 10012385;
          
            var client = new UkrlpApiClient(_logger.Object, _config.Object, new HttpClient(),
                new UkrlpSoapSerializer());

            var result = client.GetTrainingProviderByUkprn(ukprn).GetAwaiter().GetResult();

            result.Should().NotBeNull();
            result.Count.Should().Be(1);
            var matchResult = result[0];
            matchResult.UKPRN.Should().Be(ukprn.ToString());
            matchResult.ProviderStatus.Should().Be("Active");
            matchResult.ContactDetails.FirstOrDefault(x => x.ContactType == "L").Should().NotBeNull();
            matchResult.VerificationDate.Should().NotBeNull();
            matchResult.VerificationDetails
                .FirstOrDefault(x => x.VerificationAuthority == "Sole Trader or Non-limited Partnership")
                .Should().NotBeNull();
            matchResult.ProviderAliases.Count.Should().Be(1);
        }

        [Test]
        public void Non_matching_UKPRN_returns_no_results()
        {
            var ukprn = 99998888;

            var client = new UkrlpApiClient(_logger.Object, _config.Object, new HttpClient(),
                new UkrlpSoapSerializer());

            var result = client.GetTrainingProviderByUkprn(ukprn).GetAwaiter().GetResult();

            result.Should().NotBeNull();
            result.Count.Should().Be(0);
        }

        [Test]
        public void Inactive_UKPRN_returns_no_results()
        {
            var ukprn = 10019227;

            var client = new UkrlpApiClient(_logger.Object, _config.Object, new HttpClient(),
                new UkrlpSoapSerializer());

            var result = client.GetTrainingProviderByUkprn(ukprn).GetAwaiter().GetResult();

            result.Should().NotBeNull();
            result.Count.Should().Be(0);
        }
    }
}

using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Web.Services;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.UnitTests.Services;

[TestFixture]

public class TrusteeExemptionServiceTests
{
    private Mock<IConfigurationService> _config;
    private TrusteeExemptionService _service;

    [SetUp]
    public void Before_each_test()
    {
        _config = new Mock<IConfigurationService>();

        _service = new TrusteeExemptionService(_config.Object);
    }

    [TestCase("11111111", "11111111", true)]
    [TestCase("11111111", "", false)]
    [TestCase("11111111", "11111111,22222222", true)]
    [TestCase("11111111", "22222222,11111111", true)]
    [TestCase("33333333", "22222222", false)]
    [TestCase("33333333", "22222222,11111111", false)]
    public async Task ServiceChecksUkprnForExemptions(string ukprn, string listOfExemptions, bool isExempt)
    {

        _config.Setup(x => x.GetConfig()).ReturnsAsync(new ApplyConfig
        { ProvidersExemptedFromHavingTrustees = listOfExemptions });

        var result = await _service.IsProviderExempted(ukprn);

        result.Should().Be(isExempt);
    }
}
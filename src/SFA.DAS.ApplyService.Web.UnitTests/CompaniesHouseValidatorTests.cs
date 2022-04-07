using Microsoft.Extensions.Logging;
using Moq;

namespace SFA.DAS.ApplyService.Web.UnitTests
{
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.DAS.ApplyService.Web.Validators;

    [TestFixture]
    public class CompaniesHouseValidatorTests
    {
        [TestCase("", "active", true)]
        [TestCase("12345678", "Active", true)]
        [TestCase("12345678", "ACTIVE", true)]
          [TestCase("12345678", "", true)]
        [TestCase("12345678", null, true)]
        [TestCase("12345678", "liquidation", false)]
        [TestCase("12345678", "dissolved", false)]
        [TestCase("12345678", "receivership", false)]
        [TestCase("12345678", "administration", false)]
        [TestCase("12345678", "voluntary-arrangement", false)]
        [TestCase("12345678", "converted-closed", false)]
        [TestCase("12345678", "insolvency-proceedings", false)]
        [TestCase("10043575", "voluntary-arrangement", true)] // APR-2989 special exception
        public void Validator_returns_expected_validity_for_company_number_and_status(string ukprn, string companyStatus, bool expectedValidity)
        {
            var isValid = CompaniesHouseValidator.CompaniesHouseStatusValid(ukprn, companyStatus, Mock.Of<ILogger>());

            isValid.Should().Be(expectedValidity);
        }
    }
}

namespace SFA.DAS.ApplyService.Web.UnitTests
{
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.DAS.ApplyService.Web.Validators;

    [TestFixture]
    public class CompaniesHouseValidatorTests
    {
        [TestCase("12345678", "active", true)]
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
        [TestCase("IP345678", "", true)]
        [TestCase("SP345678", "", true)]
        [TestCase("IC345678", "", true)]
        [TestCase("SI345678", "", true)]
        [TestCase("NP345678", "", true)]
        [TestCase("NV345678", "", true)]
        [TestCase("RC345678", "", true)]
        [TestCase("SR345678", "", true)]
        [TestCase("NR345678", "", true)]
        [TestCase("NO345678", "", true)]
        [TestCase("IP345678", "converted-closed", false)]
        [TestCase("SP345678", "converted-closed", false)]
        [TestCase("IC345678", "converted-closed", false)]
        [TestCase("SI345678", "converted-closed", false)]
        [TestCase("NP345678", "converted-closed", false)]
        [TestCase("NV345678", "converted-closed", false)]
        [TestCase("RC345678", "converted-closed", false)]
        [TestCase("SR345678", "converted-closed", false)]
        [TestCase("NR345678", "converted-closed", false)]
        [TestCase("NO345678", "converted-closed", false)]
        public void Validator_returns_expected_validity_for_company_number_and_status(string companyNumber, string companyStatus, bool expectedValidity)
        {
            bool isValid = CompaniesHouseValidator.CompaniesHouseStatusValid(companyNumber, companyStatus);

            isValid.Should().Be(expectedValidity);
        }
    }
}

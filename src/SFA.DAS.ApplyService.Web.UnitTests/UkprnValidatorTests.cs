namespace SFA.DAS.ApplyService.Web.UnitTests
{
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.DAS.ApplyService.Web.Validators;

    [TestFixture]
    public class UkprnValidatorTests
    {
        [TestCase("")]
        [TestCase(null)]
        public void Validator_returns_false_for_missing_ukprn(string ukprn)
        {
            bool isValid = UkprnValidator.IsValidUkprn(ukprn, out var ukprnValue);

            isValid.Should().BeFalse();
            ukprnValue.Should().Be(0);
        }

        [TestCase("9999999")]
        [TestCase("1")]
        [TestCase("111111111")]
        [TestCase("1000123A")]
        public void Validator_returns_false_for_invalid_ukprn(string ukprn)
        {
            bool isValid = UkprnValidator.IsValidUkprn(ukprn, out _);

            isValid.Should().BeFalse();
        }

        [Test]
        public void Validator_returns_true_for_a_valid_ukprn()
        {
            string ukprn = "10002000";

            bool isValid = UkprnValidator.IsValidUkprn(ukprn, out var ukprnValue);

            isValid.Should().BeTrue();
            ukprnValue.Should().Be(10002000);
        }
    }
}

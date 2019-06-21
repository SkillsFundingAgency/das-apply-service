
namespace SFA.DAS.ApplyService.Web.UnitTests
{
    using FluentAssertions;
    using NUnit.Framework;
    using Resources;
    using Validators;

    [TestFixture]
    public class UkprnValidatorTests
    {
        [TestCase("")]
        [TestCase(null)]
        public void Validator_returns_appropriate_validation_message_for_missing_ukprn(string ukprn)
        {
            long ukprnValue;
            string validationMessage = UkprnValidator.IsValidUkprn(ukprn, out ukprnValue);

            validationMessage.Should().Be(UkprnValidationMessages.MissingUkprn);
            ukprnValue.Should().Be(0);
        }

        [TestCase("9999999")]
        [TestCase("1")]
        [TestCase("111111111")]
        [TestCase("1000123A")]
        public void Validator_returns_appropriate_validation_message_for_invalid_ukprn(string ukprn)
        {
            long ukprnValue;
            string validationMessage = UkprnValidator.IsValidUkprn(ukprn, out ukprnValue);

            validationMessage.Should().Be(UkprnValidationMessages.InvalidUkprn);
        }

        [Test]
        public void Validator_does_not_return_message_for_a_valid_ukprn()
        {
            string ukprn = "10002000";

            long ukprnValue;
            string validationMessage = UkprnValidator.IsValidUkprn(ukprn, out ukprnValue);

            validationMessage.Should().BeEmpty();
            ukprnValue.Should().Be(10002000);
        }
    }
}

using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Web.Resources;
using SFA.DAS.ApplyService.Web.Validators;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.UnitTests.Validators
{
    [TestFixture]
    public class EnterNewUkprnViewModelValidatorTests
    {
        private EnterNewUkprnViewModelValidator _validator;
        private Mock<IUkprnWhitelistValidator> _ukrpnWhitelistValidator;

        [SetUp]
        public void Arrange()
        {
            _ukrpnWhitelistValidator = new Mock<IUkprnWhitelistValidator>();
            _ukrpnWhitelistValidator.Setup(x => x.IsWhitelistedUkprn(It.Is<long>(l => l == 10037482)))
                .ReturnsAsync(true);

            _validator = new EnterNewUkprnViewModelValidator(_ukrpnWhitelistValidator.Object);
        }

        [Test]
        public void Validate_Returns_Valid_When_Ukprn_Is_Valid_And_Whitelisted()
        {
            var viewModel = new EnterNewUkprnViewModel
            {
                ApplicationId = Guid.NewGuid(),
                Ukprn = "10037482"
            };

            var result = _validator.Validate(viewModel);

            Assert.IsTrue(result.IsValid);
        }

        [TestCase("", "Enter a UKPRN")]
        [TestCase(" ", "Enter a UKPRN")]
        [TestCase("12345", "Enter a UKPRN using 8 numbers")]
        public void Validate_Returns_Appropriate_Error_For_Invalid_Or_Missing_Ukprn(string ukprn, string expectedErrorMessage)
        {
            var viewModel = new EnterNewUkprnViewModel
            {
                ApplicationId = Guid.NewGuid(),
                Ukprn = ukprn
            };

            var result = _validator.Validate(viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(viewModel.Ukprn), result.Errors[0].PropertyName);
            Assert.AreEqual(expectedErrorMessage, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_Ukprn_Is_Not_Whitelisted()
        {
            var viewModel = new EnterNewUkprnViewModel
            {
                ApplicationId = Guid.NewGuid(),
                Ukprn = "10002000"
            };

            var result = _validator.Validate(viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(viewModel.Ukprn), result.Errors[0].PropertyName);
            Assert.AreEqual(UkprnValidationMessages.NotWhitelistedUkprn, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Error_When_Ukprn_Is_Same_As_Current()
        {
            var viewModel = new EnterNewUkprnViewModel
            {
                ApplicationId = Guid.NewGuid(),
                Ukprn = "10037482",
                CurrentUkprn = 10037482
            };

            var result = _validator.Validate(viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(viewModel.Ukprn), result.Errors[0].PropertyName);
            Assert.AreEqual(UkprnValidationMessages.NewUkprnMustNotBeSame, result.Errors[0].ErrorMessage);
        }
    }
}

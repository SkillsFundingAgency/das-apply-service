using System;
using NUnit.Framework;
using SFA.DAS.ApplyService.Web.Validators;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.UnitTests.Validators
{
    [TestFixture]
    public class OneInTwelveMonthsValidatorTests
    {
        private OneInTwelveMonthsValidator _validator;

        [SetUp]
        public void Arrange()
        {
            _validator = new OneInTwelveMonthsValidator();
        }

        [Test]
        public void Validate_Returns_Valid_When_Applicant_Selects_True()
        {
            var viewModel = new OneInTwelveMonthsViewModel
            {
                ApplicationId = Guid.NewGuid(),
                HasOneInTwelveMonths = true
            };

            var result = _validator.Validate(viewModel);

            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void Validate_Returns_Valid_When_Applicant_Selects_False()
        {
            var viewModel = new OneInTwelveMonthsViewModel
            {
                ApplicationId = Guid.NewGuid(),
                HasOneInTwelveMonths = false
            };

            var result = _validator.Validate(viewModel);

            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_Applicant_Selects_Nothing()
        {
            var viewModel = new OneInTwelveMonthsViewModel
            {
                ApplicationId = Guid.NewGuid(),
                HasOneInTwelveMonths = null
            };

            var result = _validator.Validate(viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(viewModel.HasOneInTwelveMonths), result.Errors[0].PropertyName);
        }
    }
}

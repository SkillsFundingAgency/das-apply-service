using System;
using NUnit.Framework;
using SFA.DAS.ApplyService.Web.Validators;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.UnitTests.Validators
{
    [TestFixture]
    public class TwoInTwelveMonthsValidatorTests
    {
        private TwoInTwelveMonthsValidator _validator;

        [SetUp]
        public void Arrange()
        {
            _validator = new TwoInTwelveMonthsValidator();
        }

        [Test]
        public void Validate_Returns_Valid_When_Applicant_Selects_True()
        {
            var viewModel = new TwoInTwelveMonthsViewModel
            {
                ApplicationId = Guid.NewGuid(),
                HasTwoInTwelveMonths = true
            };

            var result = _validator.Validate(viewModel);

            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void Validate_Returns_Valid_When_Applicant_Selects_False()
        {
            var viewModel = new TwoInTwelveMonthsViewModel
            {
                ApplicationId = Guid.NewGuid(),
                HasTwoInTwelveMonths = false
            };

            var result = _validator.Validate(viewModel);

            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_Applicant_Selects_Nothing()
        {
            var viewModel = new TwoInTwelveMonthsViewModel
            {
                ApplicationId = Guid.NewGuid(),
                HasTwoInTwelveMonths = null
            };

            var result = _validator.Validate(viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(viewModel.HasTwoInTwelveMonths), result.Errors[0].PropertyName);
        }
    }
}

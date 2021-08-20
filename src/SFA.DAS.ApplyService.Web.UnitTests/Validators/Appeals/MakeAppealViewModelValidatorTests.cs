using System;
using NUnit.Framework;
using SFA.DAS.ApplyService.Web.Validators.Appeals;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp.Appeals;

namespace SFA.DAS.ApplyService.Web.UnitTests.Validators.Appeals
{
    [TestFixture]
    public class MakeAppealViewModelValidatorTests
    {
        private MakeAppealViewModelValidator _validator;

        private MakeAppealViewModel _viewModel;

        [SetUp]
        public void Arrange()
        {
            _validator = new MakeAppealViewModelValidator();

            _viewModel = new MakeAppealViewModel
            {
                ApplicationId = Guid.NewGuid()
            };
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(true, true)]
        public void Validate_Returns_Valid_When_At_Least_One_Appeal_Option_Is_Selected(bool appealOnPolicyOrProcesses, bool appealOnEvidenceSubmitted)
        {
            _viewModel.AppealOnPolicyOrProcesses = appealOnPolicyOrProcesses;
            _viewModel.AppealOnEvidenceSubmitted = appealOnEvidenceSubmitted;

            var result = _validator.Validate(_viewModel);

            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_No_Appeal_Option_Is_Selected()
        {
            _viewModel.AppealOnPolicyOrProcesses = false;
            _viewModel.AppealOnEvidenceSubmitted = false;

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.AppealOnPolicyOrProcesses), result.Errors[0].PropertyName);
            Assert.AreEqual(MakeAppealViewModelValidator.NoAppealOptionSelectedError, result.Errors[0].ErrorMessage);
        }
    }
}

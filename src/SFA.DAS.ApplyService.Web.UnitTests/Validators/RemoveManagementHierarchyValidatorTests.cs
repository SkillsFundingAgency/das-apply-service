using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Web.Validators;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.UnitTests.Validators
{
    [TestFixture]
    public class RemoveManagementHierarchyValidatorTests
    {
        private RemoveManagementHierarchyValidator _validator;

        private ConfirmRemoveManagementHierarchyViewModel _viewModel;

        [SetUp]
        public void Arrange()
        {
            _validator = new RemoveManagementHierarchyValidator();

            _viewModel = new ConfirmRemoveManagementHierarchyViewModel
            {
                ApplicationId = Guid.NewGuid(),
                Index = 1,
                Name = "FirstName LastName",
                ActionName = "RemoveManagementHierarchy",
                BackAction = "ConfirmManagementHierarchy",
                GetHelpAction = "RemoveManagementHierarchy",
                Confirmation = "Yes"
            };
        }

        [Test]
        public void Validate_Returns_Valid_When_Manatory_Fields_Are_Present_And_Correct()
        {
            var result = _validator.Validate(_viewModel);

            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_Confirmation_Missing()
        {
            _viewModel.Confirmation = null;

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.Confirmation), result.Errors[0].PropertyName);
            Assert.AreEqual($"{RemoveManagementHierarchyValidator.ConfirmationMissingError} {_viewModel.Name}", result.Errors[0].ErrorMessage);
        }
    }
}

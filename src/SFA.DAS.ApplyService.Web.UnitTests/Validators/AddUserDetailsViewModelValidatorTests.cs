using System.Linq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Web.Validators;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.UnitTests.Validators
{
    [TestFixture]
    public class AddUserDetailsViewModelValidatorTests
    {

        private AddUserDetailsViewModelValidator _validator;
        private AddUserDetailsViewModel _viewModel;

        [SetUp]
        public void Arrange()
        {
            _validator = new AddUserDetailsViewModelValidator();
            _viewModel = new AddUserDetailsViewModel
            {

                FirstName = "FirstName",
                LastName = "LastName",
            };
        }

        [Test]
        public void Validate_Returns_No_Error_Messages_When_Mandatory_Fields_Are_Present_And_Correct()
        {
            var results = _validator.Validate(_viewModel);
            Assert.IsTrue(results.IsValid);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_FirstName_Missing()
        {
            _viewModel.FirstName = null;

            var results = _validator.Validate(_viewModel);

            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.FirstName), results.Errors[0].PropertyName);
            Assert.AreEqual(CreateAccountViewModelValidator.FirstNameMinLengthError, results.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_FirstName_Exceeds_MaxLength()
        {
            const int maxLength = 250;
            _viewModel.FirstName = string.Concat(Enumerable.Repeat("a", maxLength + 1));
        
            var results = _validator.Validate(_viewModel);
            
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.FirstName),  results.Errors[0].PropertyName);
            Assert.AreEqual(CreateAccountViewModelValidator.FirstNameMaxLengthError, results.Errors[0].ErrorMessage);
        }
        
        [Test]
        public void Validate_Returns_Appropriate_Error_When_LastName_Missing()
        {
            _viewModel.LastName = null;
        
            var results = _validator.Validate(_viewModel);

            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.LastName), results.Errors[0].PropertyName);
            Assert.AreEqual(CreateAccountViewModelValidator.LastNameMinLengthError, results.Errors[0].ErrorMessage);
        }
        
        [Test]
        public void Validate_Returns_Appropriate_Error_When_LastName_Exceeds_MaxLength()
        {
            const int maxLength = 250;
            _viewModel.LastName = string.Concat(Enumerable.Repeat("a", maxLength + 1));
        
            var results = _validator.Validate(_viewModel);
            
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.LastName), results.Errors[0].PropertyName);
            Assert.AreEqual(CreateAccountViewModelValidator.LastNameMaxLengthError, results.Errors[0].ErrorMessage);
        }
    }
}

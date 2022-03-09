using System.Linq;
using NPOI.OpenXmlFormats.Spreadsheet;
using NUnit.Framework;
using SFA.DAS.ApplyService.Web.Validators;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.UnitTests.Validators
{
    [TestFixture]
    public class CreateAccountViewModelValidatorTests
    {

        private CreateAccountViewModelValidator _validator;
        private CreateAccountViewModel _viewModel;

        [SetUp]
        public void Arrange()
        {
            _validator = new CreateAccountViewModelValidator();
            _viewModel = new CreateAccountViewModel
            {

                GivenName = "FirstName",
                FamilyName = "LastName",
                Email = "name@location.com"
            };
        }

        [Test]
        public void Validate_Returns_No_Error_Messages_When_Mandatory_Fields_Are_Present_And_Correct()
        {
            var results = _validator.Validate(_viewModel);
            Assert.IsTrue(results.IsValid);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_GivenName_Missing()
        {
            _viewModel.GivenName = null;

            var results = _validator.Validate(_viewModel);

            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.GivenName), results.Errors[0].PropertyName);
            Assert.AreEqual(CreateAccountViewModelValidator.FirstNameMinLengthError, results.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_GivenName_Exceeds_MaxLength()
        {
            const int maxLength = 250;
            _viewModel.GivenName = string.Concat(Enumerable.Repeat("a", maxLength + 1));
        
            var results = _validator.Validate(_viewModel);
            
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.GivenName),  results.Errors[0].PropertyName);
            Assert.AreEqual(CreateAccountViewModelValidator.FirstNameMaxLengthError, results.Errors[0].ErrorMessage);
        }
        
        [Test]
        public void Validate_Returns_Appropriate_Error_When_FamilyName_Missing()
        {
            _viewModel.FamilyName = null;
        
            var results = _validator.Validate(_viewModel);

            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.FamilyName), results.Errors[0].PropertyName);
            Assert.AreEqual(CreateAccountViewModelValidator.LastNameMinLengthError, results.Errors[0].ErrorMessage);
        }
        
        [Test]
        public void Validate_Returns_Appropriate_Error_When_FamilyName_Exceeds_MaxLength()
        {
            const int maxLength = 250;
            _viewModel.FamilyName = string.Concat(Enumerable.Repeat("a", maxLength + 1));
        
            var results = _validator.Validate(_viewModel);
            
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.FamilyName), results.Errors[0].PropertyName);
            Assert.AreEqual(CreateAccountViewModelValidator.LastNameMaxLengthError, results.Errors[0].ErrorMessage);
        }
        
        [Test]
        public void Validate_Returns_Appropriate_Error_When_Email_Missing()
        {
            _viewModel.Email = null;
        
            var results = _validator.Validate(_viewModel);
        
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.Email), results.Errors[0].PropertyName);
            Assert.AreEqual(CreateAccountViewModelValidator.EmailError, results.Errors[0].ErrorMessage);
        }
        
        [Test]
        public void Validate_Returns_Appropriate_Error_When_Email_Exceeds_MaxLength()
        {
            const int maxLength = 256;
            _viewModel.Email = string.Concat(Enumerable.Repeat("a", maxLength)) + "@location.com";
        
            var results = _validator.Validate(_viewModel);
            
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.Email), results.Errors[0].PropertyName);
            Assert.AreEqual(CreateAccountViewModelValidator.EmailErrorTooLong, results.Errors[0].ErrorMessage);
        }

        [TestCase("wrong-format")]
        [TestCase("test")]
        [TestCase("test@test")]
        [TestCase("test@test      .com")]
        public void Validate_Returns_Appropriate_Error_When_Email_is_Wrong_Format(string incorrectEmail)
        {
            _viewModel.Email = incorrectEmail;
            var results = _validator.Validate(_viewModel);

            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.Email), results.Errors[0].PropertyName);
            Assert.AreEqual(ManagementHierarchyValidator.EmailErrorWrongFormat, results.Errors[0].ErrorMessage);
        }

        [TestCase("test@test.uk")]
        [TestCase("test@test.com")]
        [TestCase("test@test.co.uk")]
        [TestCase("test.test@test.uk")]
        [TestCase("test-test@test.uk")]
        [TestCase("test.test@test.com")]
        [TestCase("test-test@test.com")]
        [TestCase("test.test@test.co.uk")]
        [TestCase("test-test@test.co.uk")]
        [TestCase("joe.joe@20-20test.com")]
        public void Validate_Returns_Valid_When_Email_is_Correct_Format(string correctEmail)
        {
            _viewModel.Email = correctEmail;
        
            var results = _validator.Validate(_viewModel);
            Assert.IsTrue(results.IsValid);
        }
    }
}

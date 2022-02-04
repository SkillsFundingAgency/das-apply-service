using System.Linq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Web.Validators;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.UnitTests.Validators
{
    [TestFixture]
    public class CreateAccountViewModelValidatorTests
    {
   
        private CreateAccountViewModel _viewModel;

        [SetUp]
        public void Arrange()
        {
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
            var errors = CreateAccountViewModelValidator.Validate(_viewModel);
            Assert.IsFalse(errors.Any());
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_GivenName_Missing()
        {
            _viewModel.GivenName = null;

            var errors = CreateAccountViewModelValidator.Validate(_viewModel);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(nameof(_viewModel.GivenName), errors[0].Field);
            Assert.AreEqual(ManagementHierarchyValidator.FirstNameMinLengthError, errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_GivenName_Exceeds_MaxLength()
        {
            const int maxLength = 250;
            _viewModel.GivenName = string.Concat(Enumerable.Repeat("a", maxLength + 1));
        
            var errors = CreateAccountViewModelValidator.Validate(_viewModel);
            
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(nameof(_viewModel.GivenName), errors[0].Field);
            Assert.AreEqual(CreateAccountViewModelValidator.FirstNameMaxLengthError, errors[0].ErrorMessage);
        }
        
        [Test]
        public void Validate_Returns_Appropriate_Error_When_FamilyName_Missing()
        {
            _viewModel.FamilyName = null;
        
            var errors = CreateAccountViewModelValidator.Validate(_viewModel);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(nameof(_viewModel.FamilyName), errors[0].Field);
            Assert.AreEqual(CreateAccountViewModelValidator.LastNameMinLengthError, errors[0].ErrorMessage);
        }
        
        [Test]
        public void Validate_Returns_Appropriate_Error_When_FamilyName_Exceeds_MaxLength()
        {
            const int maxLength = 250;
            _viewModel.FamilyName = string.Concat(Enumerable.Repeat("a", maxLength + 1));
        
            var errors = CreateAccountViewModelValidator.Validate(_viewModel);
            
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(nameof(_viewModel.FamilyName), errors[0].Field);
            Assert.AreEqual(CreateAccountViewModelValidator.LastNameMaxLengthError, errors[0].ErrorMessage);
        }
        
        [Test]
        public void Validate_Returns_Appropriate_Error_When_Email_Missing()
        {
            _viewModel.Email = null;
        
            var errors = CreateAccountViewModelValidator.Validate(_viewModel);
        
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(nameof(_viewModel.Email), errors[0].Field);
            Assert.AreEqual(CreateAccountViewModelValidator.EmailError, errors[0].ErrorMessage);
        }
        
        [Test]
        public void Validate_Returns_Appropriate_Error_When_Email_Exceeds_MaxLength()
        {
            const int maxLength = 256;
            _viewModel.Email = string.Concat(Enumerable.Repeat("a", maxLength)) + "@location.com";
        
            var errors = CreateAccountViewModelValidator.Validate(_viewModel);
            
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(nameof(_viewModel.Email), errors[0].Field);
            Assert.AreEqual(CreateAccountViewModelValidator.EmailErrorTooLong, errors[0].ErrorMessage);
        }

        [TestCase("wrong-format")]
        [TestCase("test")]
        [TestCase("test@test")]
        [TestCase("test@test      .com")]
        public void Validate_Returns_Appropriate_Error_When_Email_is_Wrong_Format(string incorrectEmail)
        {
            _viewModel.Email = incorrectEmail;
            var errors = CreateAccountViewModelValidator.Validate(_viewModel);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(nameof(_viewModel.Email), errors[0].Field);
            Assert.AreEqual(ManagementHierarchyValidator.EmailErrorWrongFormat, errors[0].ErrorMessage);
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
        public void Validate_Returns_Valid_When_Email_is_Correct_Format(string correctEmail)
        {
            _viewModel.Email = correctEmail;
        
            var errors = CreateAccountViewModelValidator.Validate(_viewModel);
            Assert.IsFalse(errors.Any());
        }
    }
}

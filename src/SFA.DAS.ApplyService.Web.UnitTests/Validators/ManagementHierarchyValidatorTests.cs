using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Web.Validators;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.UnitTests.Validators
{
    [TestFixture]
    public class ManagementHierarchyValidatorTests
    {
        private ManagementHierarchyValidator _validator;

        private AddEditManagementHierarchyViewModel _viewModel;

        [SetUp]
        public void Arrange()
        {
            _validator = new ManagementHierarchyValidator();

            _viewModel = new AddEditManagementHierarchyViewModel
            {
                ApplicationId = Guid.NewGuid(),
                FirstName = "FirstName",
                LastName = "LastName",
                JobRole = "JobRole",
                TimeInRoleYears = "1",
                TimeInRoleMonths = "1",
                IsPartOfOtherOrgThatGetsFunding = "Yes",
                OtherOrgName = "OtherOrgName",
                DobMonth = "1",
                DobYear = "2000",
                Email = "name@location.com",
                ContactNumber = "123456789"
            };
        }

        [Test]
        public void Validate_Returns_Valid_When_Manatory_Fields_Are_Present_And_Correct()
        {
            var result = _validator.Validate(_viewModel);

            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_FirstName_Missing()
        {
            _viewModel.FirstName = null;

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.FirstName), result.Errors[0].PropertyName);
            Assert.AreEqual(ManagementHierarchyValidator.FirstNameMinLengthError, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_FirstName_Exceeds_MaxLength()
        {
            const int maxLength = 255;
            _viewModel.FirstName = string.Concat(Enumerable.Repeat("a", maxLength + 1));

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.FirstName), result.Errors[0].PropertyName);
            Assert.AreEqual(ManagementHierarchyValidator.FirstNameMaxLengthError, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_LastName_Missing()
        {
            _viewModel.LastName = null;

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.LastName), result.Errors[0].PropertyName);
            Assert.AreEqual(ManagementHierarchyValidator.LastNameMinLengthError, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_LastName_Exceeds_MaxLength()
        {
            const int maxLength = 255;
            _viewModel.LastName = string.Concat(Enumerable.Repeat("a", maxLength + 1));

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.LastName), result.Errors[0].PropertyName);
            Assert.AreEqual(ManagementHierarchyValidator.LastNameMaxLengthError, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_JobRole_Missing()
        {
            _viewModel.JobRole = null;

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.JobRole), result.Errors[0].PropertyName);
            Assert.AreEqual(ManagementHierarchyValidator.JobRoleMinLengthError, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_JobRole_Exceeds_MaxLength()
        {
            const int maxLength = 255;
            _viewModel.JobRole = string.Concat(Enumerable.Repeat("a", maxLength + 1));

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.JobRole), result.Errors[0].PropertyName);
            Assert.AreEqual(ManagementHierarchyValidator.JobRoleMaxLengthError, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_TimeInRoleMonths_Missing()
        {
            _viewModel.TimeInRoleMonths = null;

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.TimeInRoleMonths), result.Errors[0].PropertyName);
            Assert.AreEqual(ManagementHierarchyValidator.TimeInRoleError, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_TimeInRoleMonths_is_Wrong_Format()
        {
            _viewModel.TimeInRoleMonths = "wrong-format";

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.TimeInRoleMonths), result.Errors[0].PropertyName);
            Assert.AreEqual(ManagementHierarchyValidator.TimeInRoleError, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_TimeInRoleMonths_Not_LessThanOrEqualTo11()
        {
            const int maxMonths = 11;
            _viewModel.TimeInRoleMonths = (maxMonths + 1).ToString();

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.TimeInRoleMonths), result.Errors[0].PropertyName);
            Assert.AreEqual(ManagementHierarchyValidator.TimeInRoleMonthsTooBigError, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_TimeInRoleYears_Missing()
        {
            _viewModel.TimeInRoleYears = null;

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.TimeInRoleYears), result.Errors[0].PropertyName);
            Assert.AreEqual(ManagementHierarchyValidator.TimeInRoleError, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_TimeInRoleYears_is_Wrong_Format()
        {
            _viewModel.TimeInRoleYears = "wrong-format";

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.TimeInRoleYears), result.Errors[0].PropertyName);
            Assert.AreEqual(ManagementHierarchyValidator.TimeInRoleError, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_TimeInRoleYears_Not_LessThanOrEqualTo99()
        {
            const int maxYears = 99;
            _viewModel.TimeInRoleYears = (maxYears + 1).ToString();

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.TimeInRoleYears), result.Errors[0].PropertyName);
            Assert.AreEqual(ManagementHierarchyValidator.TimeInRoleYearsTooBigError, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_IsPartOfOtherOrgThatGetsFunding_Missing()
        {
            _viewModel.IsPartOfOtherOrgThatGetsFunding = null;

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.IsPartOfOtherOrgThatGetsFunding), result.Errors[0].PropertyName);
            Assert.AreEqual(ManagementHierarchyValidator.IsPartOfOtherOrgThatGetsFundingError, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_IsPartOfOtherOrgThatGetsFunding_Yes_And_OtherOrgName_Missing()
        {
            _viewModel.IsPartOfOtherOrgThatGetsFunding = "Yes";
            _viewModel.OtherOrgName = null;

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.OtherOrgName), result.Errors[0].PropertyName);
            Assert.AreEqual(ManagementHierarchyValidator.OtherOrgNameError, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_IsPartOfOtherOrgThatGetsFunding_Yes_And_OtherOrgName_Exceeds_MaxLength()
        {
            _viewModel.IsPartOfOtherOrgThatGetsFunding = "Yes";

            const int maxLength = 750;
            _viewModel.OtherOrgName = string.Concat(Enumerable.Repeat("a", maxLength + 1));

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.OtherOrgName), result.Errors[0].PropertyName);
            Assert.AreEqual(ManagementHierarchyValidator.OtherOrgNameLengthError, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_DobMonth_Missing()
        {
            _viewModel.DobMonth = null;

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.DobMonth), result.Errors[0].PropertyName);
            Assert.AreEqual(DateOfBirthAnswerValidator.InvalidIncompleteDateOfBirthErrorMessage, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_DobYear_Missing()
        {
            _viewModel.DobYear = null;

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.DobYear), result.Errors[0].PropertyName);
            Assert.AreEqual(DateOfBirthAnswerValidator.InvalidIncompleteDateOfBirthErrorMessage, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_EmailNumber_Missing()
        {
            _viewModel.Email = null;

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.Email), result.Errors[0].PropertyName);
            Assert.AreEqual(ManagementHierarchyValidator.EmailError, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_Email_Exceeds_MaxLength()
        {
            const int maxLength = 320;
            _viewModel.Email = string.Concat(Enumerable.Repeat("a", maxLength)) + "@location.com";

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.Email), result.Errors[0].PropertyName);
            Assert.AreEqual(ManagementHierarchyValidator.EmailErrorTooLong, result.Errors[0].ErrorMessage);
        }

        [TestCase("wrong-format")]
        [TestCase("test")]
        [TestCase("test@test")]
        [TestCase("test@test      .com")]
        public void Validate_Returns_Appropriate_Error_When_Email_is_Wrong_Format(string incorrectEmail)
        {
            _viewModel.Email = incorrectEmail;

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.Email), result.Errors[0].PropertyName);
            Assert.AreEqual(ManagementHierarchyValidator.EmailErrorWrongFormat, result.Errors[0].ErrorMessage);
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

            var result = _validator.Validate(_viewModel);

            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_ContactNumber_Missing()
        {
            _viewModel.ContactNumber = null;

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.ContactNumber), result.Errors[0].PropertyName);
            Assert.AreEqual(ManagementHierarchyValidator.ContactNumberError, result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Validate_Returns_Appropriate_Error_When_ContactNumber_Exceeds_MaxLength()
        {
            const int maxLength = 50;
            _viewModel.ContactNumber = string.Concat(Enumerable.Repeat("a", maxLength + 1));

            var result = _validator.Validate(_viewModel);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(nameof(_viewModel.ContactNumber), result.Errors[0].PropertyName);
            Assert.AreEqual(ManagementHierarchyValidator.ContactNumberTooLongError, result.Errors[0].ErrorMessage);
        }
    }
}

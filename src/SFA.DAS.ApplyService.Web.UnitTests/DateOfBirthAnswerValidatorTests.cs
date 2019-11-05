using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Web.Validators;
using System;
using System.Linq;

namespace SFA.DAS.ApplyService.Web.UnitTests
{
    [TestFixture]
    public class DateOfBirthAnswerValidatorTests
    {
        private const string FieldPrefix = "DirectorDateOfBirth";

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("0")]
        public void Validator_returns_error_if_missing_month_field(string monthValue)
        {
            Answer dobMonth = new Answer { Value = monthValue };
            var dobYear = new Answer { Value = "2000" };

            var errorMessages = DateOfBirthAnswerValidator.ValidateDateOfBirth(dobMonth, dobYear, FieldPrefix);

            var fieldKey = $"{FieldPrefix}Month";

            var validationError = errorMessages.FirstOrDefault(x => x.Field == fieldKey);
            validationError.Should().NotBeNull();
            validationError.ErrorMessage.Should().Be(DateOfBirthAnswerValidator.InvalidIncompleteDateOfBirthErrorMessage);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("0")]
        [TestCase("-1")]
        public void Validator_returns_error_if_missing_or_negative_year_field(string yearValue)
        {
            var dobMonth = new Answer { Value = "12" };
            Answer dobYear = null;

            var errorMessages = DateOfBirthAnswerValidator.ValidateDateOfBirth(dobMonth, dobYear, FieldPrefix);

            var fieldKey = $"{FieldPrefix}Year";

            var validationError = errorMessages.FirstOrDefault(x => x.Field == fieldKey);
            validationError.Should().NotBeNull();
            validationError.ErrorMessage.Should().Be(DateOfBirthAnswerValidator.InvalidIncompleteDateOfBirthErrorMessage);
        }

        [Test]
        public void Validator_returns_errors_if_both_fields_missing()
        {
            Answer dobMonth = null;
            Answer dobYear = null;

            var errorMessages = DateOfBirthAnswerValidator.ValidateDateOfBirth(dobMonth, dobYear, FieldPrefix);

            var fieldKey = $"{FieldPrefix}Month";

            var validationError = errorMessages.FirstOrDefault(x => x.Field == fieldKey);
            validationError.Should().NotBeNull();
            validationError.ErrorMessage.Should().Be(DateOfBirthAnswerValidator.MissingDateOfBirthErrorMessage);
        }

        [TestCase("0")]
        [TestCase("-1")]
        [TestCase("13")]
        public void Validator_returns_error_if_month_value_is_invalid(string monthValue)
        {
            Answer dobMonth = new Answer { Value = monthValue };

            Answer dobYear = new Answer { Value = "2000" };

            var errorMessages = DateOfBirthAnswerValidator.ValidateDateOfBirth(dobMonth, dobYear, FieldPrefix);

            var fieldKey = $"{FieldPrefix}Month";

            var validationError = errorMessages.FirstOrDefault(x => x.Field == fieldKey);
            validationError.Should().NotBeNull();
            validationError.ErrorMessage.Should().Be(DateOfBirthAnswerValidator.InvalidIncompleteDateOfBirthErrorMessage);
        }

        [TestCase("999")]
        public void Validator_returns_error_if_year_value_invalid(string yearValue)
        {
            Answer dobMonth = new Answer { Value = "12" };

            Answer dobYear = new Answer { Value = yearValue };

            var errorMessages = DateOfBirthAnswerValidator.ValidateDateOfBirth(dobMonth, dobYear, FieldPrefix);

            var fieldKey = $"{FieldPrefix}Year";

            var validationError = errorMessages.FirstOrDefault(x => x.Field == fieldKey);
            validationError.Should().NotBeNull();
            validationError.ErrorMessage.Should().Be(DateOfBirthAnswerValidator.DateOfBirthYearLengthErrorMessage);
        }

        [Test]
        public void Validator_returns_error_if_month_and_year_are_current_date()
        {
            Answer dobMonth = new Answer { Value = DateTime.Now.Month.ToString() };
            Answer dobYear = new Answer { Value = DateTime.Now.Year.ToString() };

            var errorMessages = DateOfBirthAnswerValidator.ValidateDateOfBirth(dobMonth, dobYear, FieldPrefix);

            var fieldKey = $"{FieldPrefix}Month";

            var validationError = errorMessages.FirstOrDefault(x => x.Field == fieldKey);
            validationError.Should().NotBeNull();
            validationError.ErrorMessage.Should().Be(DateOfBirthAnswerValidator.DateOfBirthInFutureErrorMessage);
        }

        [Test]
        public void Validator_returns_error_if_month_and_year_in_future()
        {
            var dobDate = DateTime.Now.AddMonths(1);

            Answer dobMonth = new Answer { Value = dobDate.Month.ToString() };

            Answer dobYear = new Answer { Value = dobDate.Year.ToString() };

            var errorMessages = DateOfBirthAnswerValidator.ValidateDateOfBirth(dobMonth, dobYear, FieldPrefix);

            var fieldKey = $"{FieldPrefix}Month";

            var validationError = errorMessages.FirstOrDefault(x => x.Field == fieldKey);
            validationError.Should().NotBeNull();
            validationError.ErrorMessage.Should().Be(DateOfBirthAnswerValidator.DateOfBirthInFutureErrorMessage);
        }
    }

}
